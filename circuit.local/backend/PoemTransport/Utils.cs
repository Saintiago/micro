using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PoemUtils
{
    public class Utils
    {
        public static List<string> GetLinesList(string str)
        {
            List<string> lines = new List<string>(str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None));
            lines.RemoveAll(string.IsNullOrWhiteSpace);
            return lines;
        }

        public static int GetLinesCount(string str)
        {
            return GetLinesList(str).Count();
        }

        /**
         * Using SHA-1 because it does not have trivial biases
         * (http://oldblog.antirez.com/post/redis-presharding.html)
         */
        public static byte[] GetHash(int number)
        {
            byte[] sourceBytes = BitConverter.GetBytes(number);
            return SHA1Managed.Create().ComputeHash(sourceBytes);
        }
    }
}
