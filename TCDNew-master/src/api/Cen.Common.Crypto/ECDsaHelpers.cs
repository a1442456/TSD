using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Cen.Common.Crypto
{
    public static class ECDsaHelpers
    {
        public static ECDsa GetECDsaPrivateFromPemFile(string pemFile)
        {
            AsymmetricCipherKeyPair BCKeyParams;
            ECPrivateKeyParameters PrivateBCKeyParams;
            ECPublicKeyParameters PublicBCKeyParams;

            using (TextReader reader = File.OpenText(pemFile))
            {
                BCKeyParams = (AsymmetricCipherKeyPair)new PemReader(reader).ReadObject();
                PrivateBCKeyParams = (ECPrivateKeyParameters) BCKeyParams.Private;
                PublicBCKeyParams = (ECPublicKeyParameters) BCKeyParams.Public;
            }
            
            var parms = new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                D = PrivateBCKeyParams.D.ToByteArray().ToArray(),
                Q = new ECPoint {
                    X = PublicBCKeyParams.Q.AffineXCoord.GetEncoded(),
                    Y = PublicBCKeyParams.Q.AffineYCoord.GetEncoded()
                }
            };

            return ECDsa.Create(parms);
        }
        
        public static ECDsa GetECDsaPublicFromPemFile(string pemFile)
        {
            ECPublicKeyParameters PublicBCKeyParms;

            using (TextReader reader = File.OpenText(pemFile))
            {
                PublicBCKeyParms = (ECPublicKeyParameters)new PemReader(reader).ReadObject();
            }

            var parms = new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint {
                    X = PublicBCKeyParms.Q.AffineXCoord.GetEncoded(),
                    Y = PublicBCKeyParms.Q.AffineYCoord.GetEncoded()
                }
            };

            return ECDsa.Create(parms);
        }
    }
}