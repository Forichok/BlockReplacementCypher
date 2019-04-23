﻿using System;
using System.Linq;

namespace sem6_clwr.Extensions
{
    static class BoolArrayExtension
    {
        public static byte[] ToByteArray(this bool[] input)
        {
            if (input.Length % 8 != 0)
            {
                throw new ArgumentException("input");
            }
            byte[] ret = new byte[input.Length / 8];
            for (int i = 0; i < input.Length; i += 8)
            {
                int value = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (input[i + j])
                    {
                        value += 1 << (7 - j);
                    }
                }
                ret[i / 8] = (byte)value;
            }
            return ret.Reverse().ToArray();
        }
    }
}
