using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Signers;

namespace Cen.IdentityModel.EdDsa
{
    internal class Ed25519SignatureProvider : SignatureProvider
    {
        private readonly Ed25519SecurityKey _ed25519SecurityKey;

        public Ed25519SignatureProvider(Ed25519SecurityKey key, string algorithm)
            : base(key, algorithm)
        {
            _ed25519SecurityKey = key;
        }

        public override byte[] Sign(byte[] input)
        {
            var signer = new Ed25519Signer();
            signer.Init(true, _ed25519SecurityKey.KeyParameters);
            signer.BlockUpdate(input, 0, input.Length);

            return signer.GenerateSignature();
        }

        public override bool Verify(byte[] input, byte[] signature)
        {
            var validator = new Ed25519Signer();
            validator.Init(false, _ed25519SecurityKey.KeyParameters);
            validator.BlockUpdate(input, 0, input.Length);

            return validator.VerifySignature(signature);
        }
        
        protected override void Dispose(bool disposing) { }
    }
}