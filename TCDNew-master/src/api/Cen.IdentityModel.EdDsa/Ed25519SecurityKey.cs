using System;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC.Rfc8032;

namespace Cen.IdentityModel.EdDsa
{
    public class Ed25519SecurityKey : AsymmetricSecurityKey
    {
        public Ed25519SecurityKey(Ed25519PrivateKeyParameters keyParameters)
        {
            if (keyParameters == null)
                throw new ArgumentNullException(nameof(keyParameters));
            
            KeyParameters = keyParameters;
            Curve = EdDsaSignatureAlgorithms.Curves.Ed25519;
        }

        public Ed25519SecurityKey(Ed25519PublicKeyParameters keyParameters)
        {
            if (keyParameters == null)
                throw new ArgumentNullException(nameof(keyParameters));
            
            KeyParameters = keyParameters;
            Curve = EdDsaSignatureAlgorithms.Curves.Ed25519;
        }
        
        public AsymmetricKeyParameter KeyParameters { get; }
        public string Curve { get; }
        public override int KeySize => Ed25519.SecretKeySize * 8;
        
        [Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus.")]
        public override bool HasPrivateKey => KeyParameters.IsPrivate;

        public override PrivateKeyStatus PrivateKeyStatus
            => KeyParameters.IsPrivate ? PrivateKeyStatus.Exists : PrivateKeyStatus.DoesNotExist;
    }
}