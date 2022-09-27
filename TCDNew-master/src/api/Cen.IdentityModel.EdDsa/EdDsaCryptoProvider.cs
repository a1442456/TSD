using System;
using Microsoft.IdentityModel.Tokens;

namespace Cen.IdentityModel.EdDsa
{
    public class EdDsaCryptoProvider : ICryptoProvider
    {
        public bool IsSupportedAlgorithm(string algorithm, params object[] args)
        {
            if (algorithm != EdDsaSignatureAlgorithms.EdDsa)
                return false;

            if (args.Length < 1)
                return false;
            
            if (!(args[0] is Ed25519SecurityKey securityKey))
                return false;
            
            return ((securityKey.Curve == EdDsaSignatureAlgorithms.Curves.Ed25519) ||
                    (securityKey.Curve == EdDsaSignatureAlgorithms.Curves.Ed448));
        }

        public object Create(string algorithm, params object[] args)
        {
            if (args != null)
            {
                if (algorithm == EdDsaSignatureAlgorithms.EdDsa && args[0] is Ed25519SecurityKey ed25519Key && ed25519Key.Curve == EdDsaSignatureAlgorithms.Curves.Ed25519)
                {
                    return new Ed25519SignatureProvider(ed25519Key, algorithm);
                }
                if (algorithm == EdDsaSignatureAlgorithms.EdDsa && args[0] is Ed448SecurityKey ed448Key && ed448Key.Curve == EdDsaSignatureAlgorithms.Curves.Ed448)
                {
                    return new Ed448SignatureProvider(ed448Key, algorithm);
                }
            }

            throw new NotSupportedException();
        }

        public void Release(object cryptoInstance)
        {
            if (cryptoInstance is IDisposable disposableObject)
                disposableObject.Dispose();
        }
    }
}