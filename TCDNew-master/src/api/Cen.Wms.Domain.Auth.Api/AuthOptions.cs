using System.IO;
using Cen.Common.Text;
using Org.BouncyCastle.Crypto.Parameters;

namespace Cen.Wms.Domain.Auth.Api
{
    public class AuthOptions
    {
        private string _privateKeyString = null;
        private byte[] _privateKeyBytes = null;
        private Ed25519PrivateKeyParameters _privateKeyParameters;
        
        private string _publicKeyString = null;
        private byte[] _publicKeyBytes = null;
        private Ed25519PublicKeyParameters _publicKeyParameters;
        
        public static string SectionName => "Auth:Options";
        public string Issuer { get; set; }
        public string PrivateKeyFileName { get; set; }
        public string PublicKeyFileName { get; set; }
        public int LifetimeSeconds  { get; set; }

        private void LoadPrivateKey()
        {
            if (_privateKeyString != null) return;
            
            _privateKeyString = (File.ReadAllText(PrivateKeyFileName) ?? string.Empty).Trim();
            _privateKeyBytes = HexStringConverter.HexStringToByteArray(_privateKeyString);
            _privateKeyParameters = new Ed25519PrivateKeyParameters(_privateKeyBytes, 0);
        }
        
        private void LoadPublicKey()
        {
            if (_publicKeyParameters != null) return;
            
            _publicKeyString = (File.ReadAllText(PublicKeyFileName)?? string.Empty).Trim();
            _publicKeyBytes = HexStringConverter.HexStringToByteArray(_publicKeyString);
            _publicKeyParameters = new Ed25519PublicKeyParameters(_publicKeyBytes, 0);
        }

        internal string GetPrivateKeyString()
        {
            LoadPrivateKey();

            return _privateKeyString;
        }
        
        internal Ed25519PrivateKeyParameters GetPrivateKeyParameters()
        {
            LoadPrivateKey();

            return _privateKeyParameters;
        }
        
        public string GetPublicKeyString()
        {
            LoadPublicKey();

            return _publicKeyString;
        }

        public Ed25519PublicKeyParameters GetPublicKeyParameters()
        {
            LoadPublicKey();

            return _publicKeyParameters;
        }
    }
}