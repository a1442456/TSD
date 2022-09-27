namespace Cen.Common.Data.DataSource.Infrastructure.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Reflection;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Emit;
    using System.Collections.Concurrent;
    using System.Reflection.PortableExecutable;

    /// <summary>
    /// Internal helper class used to generate dynamic classes
    /// </summary>
    public class ClassFactory
    {
        public static ClassFactory Instance { get; private set; }
        
        private readonly FactoryLoadContext _assemblyLoadContext;

        private int classCount;
        private readonly ReaderWriterLockSlim rwLock;
        private readonly ConcurrentDictionary<string, AssemblyMetadata> _metadataFileCache;
        private static string TO_STRING_METHOD_TEMPLATE =
           "public override string ToString() " +
           "{ " +
               "var props = System.Reflection.TypeExtensions.GetProperties(this.GetType(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public); " +
               "var sb = new System.Text.StringBuilder(); " +
                "sb.Append(\"{\"); " +
                "for (int i = 0; i < props.Length; i++) " +
                "{ " +
                "    if (i > 0) sb.Append(\", \"); " +
                "    sb.Append(props[i].Name); " +
                "    sb.Append(\"=\"); " +
                "    sb.Append(props[i].GetValue(this, null)); " +
                "} " +
                "sb.Append(\"}\"); " +
                "return sb.ToString(); " +
            "}";

        public static void Create()
        {
            Instance = new ClassFactory();
        }

        private ClassFactory()
        {
            _assemblyLoadContext = new FactoryLoadContext();

            rwLock = new ReaderWriterLockSlim();
            _metadataFileCache = new ConcurrentDictionary<string, AssemblyMetadata>(StringComparer.OrdinalIgnoreCase);
    }

        public Type GetDynamicClass(IEnumerable<DynamicProperty> properties)
        {
            string typeName = "DynamicClass" + (classCount + 1);

            var compilationUnit = DeclareCompilationUnit()
                .AddMembers(DeclareClass(typeName)
                    .AddMembers(properties.Select(DeclareDynamicProperty).ToArray())
                    .AddMembers(DeclareToStringMethod())
                );

            var compilation = CreateCompilation(compilationUnit.SyntaxTree);

            IncrementClassCounter();

            return EmitType(typeName, compilation);
        }

        private Type EmitType(string typeName, CSharpCompilation compilation)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult result;
                result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);

                    var assembly = _assemblyLoadContext.Load(ms);
                    return assembly.GetType(typeName);
                }

                throw new Exception("Unable to build type" + typeName);
            }
        }

        private void IncrementClassCounter()
        {
            rwLock.EnterWriteLock();
            try
            {
                classCount++;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private ClassDeclarationSyntax DeclareClass(string typeName)
        {
            return SyntaxFactory.ClassDeclaration(typeName)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        }

        private CompilationUnitSyntax DeclareCompilationUnit()
        {
            var unit = SyntaxFactory.CompilationUnit();
            unit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")));

            return unit;
        }

        private CSharpCompilation CreateCompilation(SyntaxTree syntaxTree)
        {
            var assemblyName = "DynamicClasses" + classCount;
            return CSharpCompilation.Create(assemblyName,
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                        syntaxTrees: new[] { syntaxTree },
                        references: GetReferences()
            );
        }

        private IEnumerable<MetadataReference> GetReferences()
        {
            var references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            if (references.Count == 0)
            {
                throw new InvalidOperationException("Unable to create MetadataReference");
            }

            return references;
        }

        private MetadataReference CreateMetadataFileReference(string path)
        {
            var metadata = _metadataFileCache.GetOrAdd(path, _ =>
            {
                using (var stream = File.OpenRead(path))
                {
                    var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                    return AssemblyMetadata.Create(moduleMetadata);
                }
            });

            return metadata.GetReference(filePath: path);
        }

        private PropertyDeclarationSyntax DeclareDynamicProperty(DynamicProperty property)
        {
            var nonNullableType = Nullable.GetUnderlyingType(property.Type);

            var propertyType = nonNullableType == null ?
                SyntaxFactory.ParseTypeName(property.Type.FullName) :
                SyntaxFactory.NullableType(SyntaxFactory.ParseTypeName(nonNullableType.FullName));
           
            return SyntaxFactory.PropertyDeclaration(propertyType, property.Name)
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                    SyntaxFactory.AccessorList(
                        SyntaxFactory.List(new[] {
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                            })
                ));
        }

        private MethodDeclarationSyntax DeclareToStringMethod()
        {
            return CSharpSyntaxTree
                .ParseText(TO_STRING_METHOD_TEMPLATE)
                .GetRoot()
                .ChildNodes()
                .First() as MethodDeclarationSyntax;
        }
    }
}