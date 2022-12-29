using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBird.Platform.BML
{
    class SignInt
    {
        public static long Bit10ToInt64(byte[] input)
        {
            long baseFor = 0;

            bool negtive = input[0] is 1;
            bool isFirstNonZero = true;

            for (int i = 1; i < 10; i++)
            {
                if (input[i] != 0 && isFirstNonZero)
                {
                    if (i is 9)
                    {
                        baseFor = input[i];
                        break;
                    }
                    baseFor = input[i] * 255;
                    isFirstNonZero = false;
                }
                else if (input[i] != 0 && i != 9)
                {
                    baseFor += input[i];
                    baseFor *= 255;
                }
                else if (input[i] != 0 && i == 9)
                {
                    baseFor += input[i];
                    break;
                }
            }

            if (negtive)
                return -baseFor;
            return baseFor;
        }

        public static byte[] Int64ToBit10(Int64 value)
        {
            bool negtive = value < 0;

            Int64 a = negtive ? -value : value;
            var result = new byte[10];
            for (int i = 0; i < 10; i++)
            {
                var done = a / 255;
                var o = a % 255;

                result[4 - i] = (byte)o;
                if (done <= 255)
                {
                    result[4 - i - 1] = (byte)done;
                    break;
                }
                else
                    a = done;
            }
            if (negtive)
                result[0] = (byte)1;
            return result;
        }

        public static Int32 Bit5ToInt32(byte[] input)
        {
            int baseFor = 0;

            bool negtive = input[0] is 1;
            bool isFirstNonZero = true;

            for (int i = 1; i < 5; i++)
            {
                if (input[i] != 0 && isFirstNonZero)
                {
                    if(i is 4)
                    {
                        baseFor = input[i];
                        break;
                    }
                    baseFor = input[i] * 255;
                    isFirstNonZero = false;
                }
                else if (input[i] != 0 && i != 4)
                {
                    baseFor += input[i];
                    baseFor *= 255;
                }
                else if (input[i] != 0 && i == 4)
                {
                    baseFor += input[i];
                    break;
                }
            }

            if (negtive)
                return -baseFor;
            return baseFor;
        }

        public static byte[] Int32ToBit5(Int32 value)
        {
            bool negtive = value < 0;

            int a = negtive? -value : value;
            var result = new byte[5];
            for (int i = 0; i < 5; i++)
            {
                var done = a / 255;
                var o = a % 255;

                result[4 - i] = (byte)o;
                if (done <= 255)
                {
                    result[4 - i - 1] = (byte)done;
                    break;
                }
                else
                    a = done;
            }
            if (negtive)
                result[0] = (byte) 1;
            return result;
        }
    }
}
