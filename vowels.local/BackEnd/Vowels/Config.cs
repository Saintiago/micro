using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vowels
{
    class Config
    {
        public const float VOWELS_CONSONANTS_GOOD_RATE = 1 / 1;
        public const string BASE_ADDRESS = "http://localhost:8091/";
        public static readonly char[] VOWELS = { 'e', 'y', 'u', 'i', 'o', 'a' };
        public static readonly char[] CONSONANTS = {
            'q', 'w', 'r', 't', 'p', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm'
        };
    }
}
