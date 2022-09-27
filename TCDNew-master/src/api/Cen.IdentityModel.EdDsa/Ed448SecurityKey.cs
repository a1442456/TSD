using System;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC.Rfc8032;

namespace Cen.IdentityModel.EdDsa
{
    public class Ed448SecurityKey : AsymmetricSecurityKey
    {
        public Ed448SecurityKey(Ed448PrivateKeyParameters keyParameters)
        {
            if (keyParameters == null)
                throw new ArgumentNullException(nameof(keyParameters));
            
            KeyParameters = keyParameters;
            Curve = EdDsaSignatureAlgorithms.Curves.Ed448;
        }

        public Ed448SecurityKey(Ed448PublicKeyParameters keyParameters)
        {
            if (keyParameters == null)
                throw new ArgumentNullException(nameof(keyParameters));
            
            KeyParameters = keyParameters;
            Curve = EdDsaSignatureAlgorithms.Curves.Ed448;
        }
        
        public AsymmetricKeyParameter KeyParameters { get; }
        public string Curve { get; }
        public override int KeySize => Ed448.SecretKeySize * 8;
        
        [Obsolete("HasPrivateKey method is deprecated, please use PrivateKeyStatus.")]
        public override bool HasPrivateKey => KeyParameters.IsPrivate;

        public override PrivateKeyStatus PrivateKeyStatus
            => KeyParameters.IsPrivate ? PrivateKeyStatus.Exists : PrivateKeyStatus.DoesNotExist;
    }
}