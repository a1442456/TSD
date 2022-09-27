using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Cen.Common.Crypto
{
    public static class RSAHelpers
    {
        public static RSA GetRSAPrivateFromPemFile(string pemFile)
        {
            RsaPrivateCrtKeyParameters BCKeyParams;

            using (TextReader reader = File.OpenText(pemFile))
            {
                BCKeyParams = (RsaPrivateCrtKeyParameters)new PemReader(reader).ReadObject();
            }

            var parms = new RSAParameters();

            parms.Modulus   = BCKeyParams.Modulus.ToByteArray();
            parms.P         = BCKeyParams.P.ToByteArray();
            parms.Q         = BCKeyParams.Q.ToByteArray();
            parms.DP        = BCKeyParams.DP.ToByteArray();
            parms.DQ        = BCKeyParams.DQ.ToByteArray();
            parms.InverseQ  = BCKeyParams.QInv.ToByteArray();
            parms.D         = BCKeyParams.Exponent.ToByteArray();
            parms.Exponent  = BCKeyParams.PublicExponent.ToByteArray();

            var rsa = RSA.Create();
            rsa.ImportParameters(parms);
            
            return rsa;
        }
        
        public static RSA GetRSAPublicFromPemFile(string pemFile)
        {
            RsaKeyParameters BCKeyParms;

            using (TextReader reader = File.OpenText(pemFile))
            {
                BCKeyParms = (RsaKeyParameters)new PemReader(reader).ReadObject();
            }

            var parms = new RSAParameters();

            parms.Modulus   = BCKeyParms.Modulus.ToByteArray();
            parms.Exponent  = BCKeyParms.Exponent.ToByteArray();

            var rsa = RSA.Create();
            rsa.ImportParameters(parms);
            
            return rsa;
        }
    }
}