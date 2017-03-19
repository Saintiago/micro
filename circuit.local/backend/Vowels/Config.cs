using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit
{
    class Config
    {
        public const double VOWELS_CONSONANTS_GOOD_RATE = (3.0 / 4.0);
        public const string LINE_INDEX_DELIMITER = "%%";
        public static readonly char[] VOWELS = { 'e', 'y', 'u', 'i', 'o', 'a' };
        public static readonly char[] CONSONANTS = {
            'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm'
        };

        public const int RETRY_COUNT = 3;
        public const int DELAY = 2000; // milliseconds
    }
}
