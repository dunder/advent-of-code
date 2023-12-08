using System.Linq;

namespace Shared
{
    public static class Maths
    {
        // https://stackoverflow.com/a/20824923
        public static long Gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long LCM(long[] numbers)
        {
            return numbers.Aggregate(Lcm);
        }

        public static long Lcm(long a, long b)
        {
            return (a / Gcf(a, b)) * b;
        }

        public static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

        public static long Mod(long a, long b)
        {
            return (a % b + b) % b;
        }
    }
}
