using System.IO;
using System.Reflection;
using System.Text;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Cen.Wms.Client.Utils
{
    public static class EdDsaHelpers
    {
        public static void InitSecurityKeys(string privateKeyFileName, string publicKeyFileName)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var privateKeyFilePath = basePath + "\\" + privateKeyFileName;
            var publicKeyFilePath = basePath + "\\" + publicKeyFileName;

            if (!File.Exists(privateKeyFilePath))
            {
                var ed25519KeyPairGenerator = new Ed25519KeyPairGenerator();
                ed25519KeyPairGenerator.Init(new Ed25519KeyGenerationParameters(SecureRandom.GetInstance("SHA256PRNG")));
                var asymmetricCipherKeyPair = ed25519KeyPairGenerator.GenerateKeyPair();
                var ed25519PrivateKeyParameters = (Ed25519PrivateKeyParameters) asymmetricCipherKeyPair.Private;
                var ed25519PublicKeyParameters = (Ed25519PublicKeyParameters) asymmetricCipherKeyPair.Public;

                var ed25519PrivateKey = ed25519PrivateKeyParameters.GetEncoded();
                using (var privateKeyStreamWriter = new StreamWriter(privateKeyFilePath, false, Encoding.UTF8))
                {
                    privateKeyStreamWriter.Write(HexStringConverter.ByteArrayToHexString(ed25519PrivateKey));
                }
                
                var ed25519PublicKey = ed25519PublicKeyParameters.GetEncoded();
                using (var publicKeyStreamWriter = new StreamWriter(publicKeyFilePath, false, Encoding.UTF8))
                {
                    publicKeyStreamWriter.Write(HexStringConverter.ByteArrayToHexString(ed25519PublicKey));
                }
            }
        }

        public static string GetPublicKey(string publicKeyFileName)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var publicKeyFilePath = basePath + "\\" + publicKeyFileName;

            string publicKeyText;
            using (var streamReader = new StreamReader(publicKeyFilePath, Encoding.UTF8))
            {
                publicKeyText = streamReader.ReadToEnd();
            }

            return publicKeyText;
        }

        public static string GetPrivateKey(string privateKeyFileName)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            var privateKeyFilePath = basePath + "\\" + privateKeyFileName;

            string privateKeyText;
            using (var streamReader = new StreamReader(privateKeyFilePath, Encoding.UTF8))
            {
                privateKeyText = streamReader.ReadToEnd();
            }

            return privateKeyText;
        }
    }
}
