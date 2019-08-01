
using System.Text;

namespace FoxExtensions
{
    public static class FoxExtensions
    {
        public static byte[] ToBytes(this string arg)
        {
            return Encoding.UTF8.GetBytes(arg);
        }
    }
}