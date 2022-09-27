using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Utilities;

namespace Cen.IdentityModel.EdDsa
{
    internal class Ed448SignatureProvider : SignatureProvider
    {
        private readonly Ed448SecurityKey _ed448SecurityKey;

        public Ed448SignatureProvider(Ed448SecurityKey key, string algorithm)
            : base(key, algorithm)
        {
            _ed448SecurityKey = key;
        }

        public override byte[] Sign(byte[] input)
        {
            // https://tools.ietf.org/html/rfc8032#section-8.3
            var signer = new Ed448Signer(Arrays.EmptyBytes);
            signer.Init(true, _ed448SecurityKey.KeyParameters);
            signer.BlockUpdate(input, 0, input.Length);

            return signer.GenerateSignature();
        }

        public override bool Verify(byte[] input, byte[] signature)
        {
            // https://tools.ietf.org/html/rfc8032#section-8.3
            var validator = new Ed448Signer(Arrays.EmptyBytes);
            validator.Init(false, _ed448SecurityKey.KeyParameters);
            validator.BlockUpdate(input, 0, input.Length);

            return validator.VerifySignature(signature);
        }
        
        protected override void Dispose(bool disposing) { }
    }
}