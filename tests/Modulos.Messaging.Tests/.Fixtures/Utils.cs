using System;
using System.IO;
using System.Text;

namespace Modulos.Messaging.Tests.Fixtures
{
    internal static class Utils
    {   
        private static readonly Random random = new Random();

        public static string RandomString(int length)
        {
            // ReSharper disable once StringLiteralTypo
            const string pool = "aaaaaaaaaabbbbbbbbbbbaasdasccc";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }

        public static string EvilPngPath => Path.Combine(Directory.GetCurrentDirectory(), 
            ".Fixtures", "Resources", "evil.png");
    }
}