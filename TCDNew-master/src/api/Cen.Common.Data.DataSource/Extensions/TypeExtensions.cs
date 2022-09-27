namespace Cen.Common.Data.DataSource.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

    internal static class TypeExtensions
    {
        internal static readonly Type[] PredefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };

        internal static bool IsPredefinedType(this Type type)
        {
            foreach (Type t in PredefinedTypes)
            {
                if (t == type)
                {
                    return true;
                }
            }
            return false;
        }

        internal static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        internal static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        internal static bool IsDynamicObject(this Type type)
        {
            return type == typeof(object) || type.IsCompatibleWith(typeof(System.Dynamic.IDynamicMetaObjectProvider));
        }
        internal static bool IsNullableType(this Type type)
        {
            return type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        internal static Type GetNonNullableType(this Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }

        internal static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        internal static Type FindGenericType(this Type type, Type genericType)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType() && type.GetGenericTypeDefinition() == genericType) return type;
                if (genericType.IsInterface())
                {
                    foreach (Type intfType in type.GetInterfaces())
                    {
                        Type found = intfType.FindGenericType(genericType);
                        if (found != null) return found;
                    }
                }
                type = type.GetTypeInfo().BaseType;
            }
            return null;
        }
        internal static MemberInfo FindPropertyOrField(this Type type, string memberName)
        {
            MemberInfo memberInfo = type.FindPropertyOrField(memberName, false);

            if (memberInfo == null)
            {
                memberInfo = type.FindPropertyOrField(memberName, true);
            }

            return memberInfo;
        }
        internal static MemberInfo FindPropertyOrField(this Type type, string memberName, bool staticAccess)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.DeclaredOnly |
                (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            foreach (Type t in type.SelfAndBaseTypes())
            {
                var members = t.GetProperties(flags)
                    .OfType<MemberInfo>()
                    .Concat(t.GetFields(flags).OfType<MemberInfo>())
                    .Where(m => m.Name.IsCaseInsensitiveEqual(memberName)).ToArray();

                if (members.Length != 0) return members[0];
            }
            return null;
        }
        internal static IEnumerable<Type> SelfAndBaseTypes(this Type type)
        {
            if (type.IsInterface())
            {
                List<Type> types = new List<Type>();
                AddInterface(types, type);
                return types;
            }
            return SelfAndBaseClasses(type);
        }
        internal static IEnumerable<Type> SelfAndBaseClasses(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.GetTypeInfo().BaseType;
            }
        }
        static void AddInterface(List<Type> types, Type type)
        {
            if (!types.Contains(type))
            {
                types.Add(type);
                foreach (Type t in type.GetInterfaces()) AddInterface(types, t);
            }
        }
        internal static PropertyInfo GetIndexerPropertyInfo(this Type type, params Type[] indexerArguments)
        {
            return
                (from p in type.GetProperties()
                 where AreArgumentsApplicable(indexerArguments, p.GetIndexParameters())
                 select p).FirstOrDefault();
        }
        private static bool AreArgumentsApplicable(IEnumerable<Type> arguments, IEnumerable<ParameterInfo> parameters)
        {
            var argumentList = arguments.ToList();
            var parameterList = parameters.ToList();

            if (argumentList.Count != parameterList.Count)
            {
                return false;
            }

            for (int i = 0; i < argumentList.Count; i++)
            {
                if (parameterList[i].ParameterType != argumentList[i])
                {
                    return false;
                }
            }

            return true;
        }
        internal static string GetTypeName(this Type type)
        {
            Type baseType = GetNonNullableType(type);
            string s = baseType.Name;
            if (type != baseType) s += '?';
            return s;
        }
        internal static bool IsCompatibleWith(this Type source, Type target)
        {
            if (source == target) return true;
            if (!target.IsValueType()) return target.IsAssignableFrom(source);
            Type st = source.GetNonNullableType();
            Type tt = target.GetNonNullableType();
            if (st != source && tt == target) return false;

            if (st.IsEnumType() || tt.IsEnumType())
            {
               return st == tt;
            }

            if (st == typeof(SByte))
            {
                return tt == typeof(SByte) || tt == typeof(Int16) || 
                    tt == typeof(Int32) || tt == typeof(Int64) || 
                    tt == typeof(Single) || tt == typeof(Double) || tt == typeof(Decimal);
            }
            else if (st == typeof(Byte))
            {
                return tt == typeof(Byte) || tt == typeof(Int16) || tt == typeof(UInt16) || tt == typeof(Int32) ||
                    tt == typeof(UInt32) || tt == typeof(Int64) || tt == typeof(UInt64) || tt == typeof(Single) ||
                    tt == typeof(Double) || tt == typeof(Decimal);
            }
            else if (st == typeof(Int16))
            {
                return tt == typeof(Int16) || tt == typeof(Int32) ||
                    tt == typeof(Int64) || tt == typeof(Single) ||
                    tt == typeof(Double) || tt == typeof(Decimal);
            }
            else if (st == typeof(UInt16))
            {
                return tt == typeof(UInt16) || tt == typeof(Int32) || tt == typeof(UInt32) ||
                    tt == typeof(Int64) || tt == typeof(UInt64) || tt == typeof(Single) ||
                    tt == typeof(Double) || tt == typeof(Decimal);
            }
            else if (st == typeof(Int32))
            {
                return tt == typeof(Int32) || tt == typeof(Int64) ||
                    tt == typeof(Single) || tt == typeof(Double) ||
                    tt == typeof(Decimal);
            }
            else if (st == typeof(UInt32))
            {
                return tt == typeof(UInt32) || tt == typeof(Int64) || tt == typeof(UInt64) ||
                    tt == typeof(Single) || tt == typeof(Double) ||
                    tt == typeof(Decimal);
            }
            else if (st == typeof(Int64))
            {
                return tt == typeof(Int64) || tt == typeof(Single) ||
                    tt == typeof(Double) || tt == typeof(Decimal);
            }
            else if (st == typeof(UInt64))
            {
                return tt == typeof(UInt64) || tt == typeof(Single) ||
                       tt == typeof(Double) || tt == typeof(Decimal);
            }
            else if (st == typeof(Single)) {
                return tt == typeof(Single) || tt == typeof(Double);
            }

            return false;
        }
        internal static string FirstSortableProperty(this Type type)
        {
            PropertyInfo firstSortableProperty = type.GetProperties().Where(property => property.PropertyType.IsPredefinedType()).FirstOrDefault();

            if (firstSortableProperty == null)
            {
                throw new NotSupportedException(Errors.CannotFindPropertyToSortBy);
            }

            return firstSortableProperty.Name;
        }

        internal static object DefaultValue(this Type type)
        {
            if (type.IsValueType())
                return Activator.CreateInstance(type);
            return null;
        }
        internal static bool IsEnumType(this Type type)
        {
            return GetNonNullableType(type).GetTypeInfo().IsEnum;
        }

        internal static bool IsNumericType(this Type type)
        {
            return GetNumericTypeKind(type) != 0;
        }

        internal static int GetNumericTypeKind(this Type type)
        {
            if (type == null)
            {
                return 0;
            }

            type = GetNonNullableType(type);

            if (type.IsEnumType())
            {
                return 0;
            }

            if (type == typeof(char) || type == typeof(Single) || type == typeof(Double) || type == typeof(Decimal)) { 
                return 1;
            } else if (type == typeof(SByte) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64)) {
                return 2;
            } else if (type == typeof(Byte) || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64)) {
                return 3;
            }
            return 0;
        }

        internal static string ToJavaScriptType(this Type type)
        {
            if (type == null)
            {
                return "Object";
            }

            if (type == typeof(char) || type == typeof(char?))
            {
                return "String";
            }

            if (IsNumericType(type))
            {
                return "Number";
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return "Date";
            }

            if (type == typeof(string))
            {
                return "String";
            }

            if (type == typeof(bool) || type == typeof(bool?))
            {
                return "Boolean";
            }

            if (type.IsEnumType())
            {
                return "Number";
            }

            if (type.GetNonNullableType() == typeof(Guid))
            {
                return "String";
            }

            return "Object";
        }

		internal static bool IsPlainType(this Type type)
		{
			return !type.IsDynamicObject();
		}
	}
}