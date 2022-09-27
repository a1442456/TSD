using System.Security.Cryptography;
using System.Text;

namespace Cen.Common.Text
{
    public static class StringExtensions
    {
        private static readonly byte[] ToZeroOffcetFrom1251Table = {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0,40, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0,
            34,35,36,37,38,39,41,42,43,44,45,46,47,48,49,50,
            51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,
            1, 2, 3, 4, 5, 6, 8, 9,10,11,12,13,14,15,16,17,
            18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33};

        private static readonly string[] FromRussianTranscribeTable = {
            "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i",
            "y", "k", "l", "m", "n", "o", "p", "r", "s", "t",
            "u", "f", "h", "ts", "ch", "sh", "sch", "", "y", "",
            "e", "yu", "ya",
            "A", "B", "V", "G", "D", "E", "YO", "ZH", "Z", "I",
            "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T",
            "U", "F", "H", "TS", "CH", "SH", "SCH", "", "Y", "",
            "E", "YU", "YA"};

        public static string Transcribe(this string input)
        {
            var inputBytes = Encoding.Default.GetBytes(input);
            var result = new StringBuilder();

            foreach (var b in inputBytes)
            {
                int k = ToZeroOffcetFrom1251Table[b];
                if (k > 0)
                    result.Append(FromRussianTranscribeTable[k - 1]);
                else
                    result.Append(b);
            }
            return result.ToString();
        }
        
        public static string ToSnakeCase(this string input)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    if (i > 0)
                        sb.Append('_');
                    sb.Append(char.ToLower(c));
                    continue;
                }

                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ToSha256HashString(this string inputString)
        {
            var sb = new StringBuilder();
            foreach (var b in ToSha256Hash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        
        public static byte[] ToSha256Hash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        
        public static string ToMd5HashString(this string inputString)
        {
            var sb = new StringBuilder();
            foreach (var b in ToMd5Hash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        
        public static byte[] ToMd5Hash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}