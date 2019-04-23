using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using sem6_clwr.Extensions;

namespace sem6_clwr.Models
{
    class CryptoModel
    {
        private readonly Dictionary<String, bool[]> _encryptTable;
        private readonly Dictionary<String, bool[]> _decryptTable;


        public CryptoModel()
        {
            _encryptTable = new Dictionary<string, bool[]>();
            _decryptTable = new Dictionary<string, bool[]>();
        }

        public void CreateBlockReplacementRules(int bitsCount, int seed)
        {
            var itemsCount = (int)Math.Pow(2, bitsCount);
            var rnd = new Random(seed);

            var values = new List<int>(Enumerable.Range(0, (int) Math.Pow(2, bitsCount)));

            for (var i = 0; i < itemsCount; i++)
            {
                var position = rnd.Next(0, values.Count);
                var num = values[position];
                values.RemoveAt(position);

                _encryptTable.Add(BitArrayExtension.GetBitArray(i, bitsCount).ToStr(),
                    BitArrayExtension.GetBitArray(num, bitsCount).ToBoolArray());
                _decryptTable.Add(BitArrayExtension.GetBitArray(num, bitsCount).ToStr(),
                    BitArrayExtension.GetBitArray(i, bitsCount).ToBoolArray());
            }
        }

        public void Encrypt(String path, int blockSize)
        {
            ChangeBlocks(path, blockSize, _encryptTable);
        }

        public void Decrypt(string path, int blockSize)
        {
            ChangeBlocks(path, blockSize, _decryptTable);
        }

        private void ChangeBlocks(string path, int blockSize, Dictionary<string, bool[]> table)
        {
            using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                using (var writer = new BinaryWriter(new FileStream(path + ".tmp", FileMode.OpenOrCreate)))
                {
                    bool fileEndReached = false;
                    while (!fileEndReached)
                    {
                        try
                        {
                            var bytes = reader.ReadBytes(blockSize);

                            if (bytes.Length == 0)
                                break;

                            if (bytes.Length != blockSize)
                            {
                                var correctedBytes = new List<byte>(bytes);
                                while (correctedBytes.Count != blockSize)
                                    correctedBytes.Add(0);
                                bytes = correctedBytes.ToArray();
                                fileEndReached = true;
                            }

                            var bitArray = new BitArray(bytes);

                            var splited = bitArray.Split(blockSize);

                            var encryptedList = new List<bool[]>();
                            foreach (BitArray block in splited)
                            {
                                encryptedList.Add(table[block.ToStr()]);
                            }

                            var result = new List<bool>();
                            foreach (var block in encryptedList)
                            {
                                result.AddRange(block);
                            }

                            var converted = result.ToArray().ToByteArray();

                            writer.Write(converted);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            break;
                        }
                    }
                }
            }
            File.Delete(path);
            File.Move(path + ".tmp", path);
        }
    }
}