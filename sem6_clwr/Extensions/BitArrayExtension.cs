using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace sem6_clwr.Extensions
{
    public static class BitArrayExtension
    {
        public static IEnumerable<BitArray> Split(this BitArray bitArray, int chunkLength)
        {
            var list = new List<BitArray>();

            bool[] boolArray = new bool[chunkLength];
            var index = 0;
            for (int i = 0; i < bitArray.Length; i++)
            {
                boolArray[index++] = bitArray[i];
                if (index >= chunkLength)
                {
                    list.Add(new BitArray(boolArray));
                    boolArray = new bool[chunkLength];
                    index = 0;
                }
            }
            return list;
        }

        public static bool[] ToBoolArray(this BitArray bitArray)
        {
            var boolArray = bitArray.Cast<bool>().ToArray().Reverse().ToArray();
            return boolArray;
        }

        public static string ToStr(this BitArray bitArray)
        {
            var result = string.Join("", bitArray.Cast<bool>().Select(Convert.ToInt32));
            return result;
        }


        public static BitArray GetBitArray(int number, int lenght)
        {
            var bitArray = new BitArray(new[] {number});
            bitArray.Length = lenght;
            bitArray = new BitArray(bitArray.ToBoolArray().Reverse().ToArray());
            return bitArray;
        }
    }
}
