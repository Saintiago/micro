using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemUtils
{
    public class CharCounter
    {
        private char[] acceptableChars;

        public CharCounter(char[] charList)
        {
            acceptableChars = charList;
        }

        public int Count(string poem)
        {
            int charCount = 0;
            foreach (char c in poem)
            {
                if (IsCharFits(c))
                {
                    ++charCount;
                }
            }
            return charCount;
        }

        private bool IsCharFits(char c)
        {
            return Array.IndexOf(acceptableChars, Char.ToLower(c)) > -1;
        }
    }
}
