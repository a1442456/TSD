using System;
using System.Collections;

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Security
{
    /// <remarks>
    ///  Utility class for creating IDigest objects from their names/Oids
    /// </remarks>
    public sealed class DigestUtilities
    {
        private enum DigestAlgorithm {
            KECCAK_224, KECCAK_256, KECCAK_288, KECCAK_384, KECCAK_512,
            NONE,
            SHA_224, SHA_256, SHA_384, SHA_512,
            SHA_512_224, SHA_512_256,
            SHA3_224, SHA3_256, SHA3_384, SHA3_512,
            SHAKE128, SHAKE256,
        };

        private DigestUtilities()
        {
        }

        private static readonly IDictionary algorithms = Platform.CreateHashtable();
        private static readonly IDictionary oids = Platform.CreateHashtable();

        static DigestUtilities()
        {
            // Signal to obfuscation tools not to change enum constants
            ((DigestAlgorithm)Enums.GetArbitraryValue(typeof(DigestAlgorithm))).ToString();

            algorithms["SHA224"] = "SHA-224";
            algorithms["SHA256"] = "SHA-256";
            algorithms["SHA384"] = "SHA-384";
            algorithms["SHA512"] = "SHA-512";
            algorithms["SHA512/224"] = "SHA-512/224";
            algorithms["SHA512/256"] = "SHA-512/256";

            algorithms["KECCAK224"] = "KECCAK-224";
            algorithms["KECCAK256"] = "KECCAK-256";
            algorithms["KECCAK288"] = "KECCAK-288";
            algorithms["KECCAK384"] = "KECCAK-384";
            algorithms["KECCAK512"] = "KECCAK-512";
        }

        public static ICollection Algorithms
        {
            get { return oids.Keys; }
        }

        public static IDigest GetDigest(
            string algorithm)
        {
            string upper = Platform.ToUpperInvariant(algorithm);
            string mechanism = (string) algorithms[upper];

            if (mechanism == null)
            {
                mechanism = upper;
            }

            try
            {
                DigestAlgorithm digestAlgorithm = (DigestAlgorithm)Enums.GetEnumValue(
                    typeof(DigestAlgorithm), mechanism);

                switch (digestAlgorithm)
                {
                    case DigestAlgorithm.KECCAK_224: return new KeccakDigest(224);
                    case DigestAlgorithm.KECCAK_256: return new KeccakDigest(256);
                    case DigestAlgorithm.KECCAK_288: return new KeccakDigest(288);
                    case DigestAlgorithm.KECCAK_384: return new KeccakDigest(384);
                    case DigestAlgorithm.KECCAK_512: return new KeccakDigest(512);
                    case DigestAlgorithm.NONE: return new NullDigest();
                    case DigestAlgorithm.SHA_224: return new Sha224Digest();
                    case DigestAlgorithm.SHA_256: return new Sha256Digest();
                    case DigestAlgorithm.SHA_384: return new Sha384Digest();
                    case DigestAlgorithm.SHA_512: return new Sha512Digest();
                    case DigestAlgorithm.SHA_512_224: return new Sha512tDigest(224);
                    case DigestAlgorithm.SHA_512_256: return new Sha512tDigest(256);
                    case DigestAlgorithm.SHA3_224: return new Sha3Digest(224);
                    case DigestAlgorithm.SHA3_256: return new Sha3Digest(256);
                    case DigestAlgorithm.SHA3_384: return new Sha3Digest(384);
                    case DigestAlgorithm.SHA3_512: return new Sha3Digest(512);
                    case DigestAlgorithm.SHAKE128: return new ShakeDigest(128);
                    case DigestAlgorithm.SHAKE256: return new ShakeDigest(256);
                }
            }
            catch (ArgumentException)
            {
            }

            throw new SecurityUtilityException("Digest " + mechanism + " not recognised.");
        }

        public static byte[] CalculateDigest(string algorithm, byte[] input)
        {
            IDigest digest = GetDigest(algorithm);
            digest.BlockUpdate(input, 0, input.Length);
            return DoFinal(digest);
        }

        public static byte[] DoFinal(
            IDigest digest)
        {
            byte[] b = new byte[digest.GetDigestSize()];
            digest.DoFinal(b, 0);
            return b;
        }

        public static byte[] DoFinal(
            IDigest	digest,
            byte[]	input)
        {
            digest.BlockUpdate(input, 0, input.Length);
            return DoFinal(digest);
        }
    }
}
