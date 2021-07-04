using System;
using System.Collections.Generic;
using System.IO;

using ScramblerWalsh.Interface;

namespace ScramblerWalsh.Model
{
    public enum TypeCrypt : short
    {
        Adamar,
        Peli,
        Kachmazh,
        Kyli,
        Stohas,
    }
    class Encrypting : IEncrypting
    {
        public event Action<int> Progress;
        private List<Action<byte[]>> listActions;
        List<byte> OutputByte = new List<byte>();
        private IKey Key;
        public Encrypting(IKey key)
        {
            Key = key;
            listActions = new List<Action<byte[]>>
            {
                EncryptAdamar, 
                EncryptPeli,  
                EncryptKachmazh, 
                EncryptKyli,
                EncryptStohas,

                DecryptAdamar,
                DecryptPeli,
                DecryptKachmazh,
                DecryptKyli,
                DecryptStohas,
            };
        }
        public void Encrypt(string fileInput, string fileOutput, TypeCrypt type)
        {
            byte[] inputByte = File.ReadAllBytes(fileInput);
            listActions[(int)type].Invoke(inputByte);
            File.WriteAllBytes(fileOutput, OutputByte.ToArray());
        }
        public void Decrypt(string fileInput, string fileOutput , TypeCrypt type)
        {
            byte[] inputByte = File.ReadAllBytes(fileInput);
            listActions[(int)type+5].Invoke(inputByte);
            File.WriteAllBytes(fileOutput, OutputByte.ToArray());
        }
        public void EncryptAdamar(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i++)
            {
                byte[] array = EncryptWalsh(ByteToArray(inputByte[i]));
                OutputByte.AddRange(array);
                SetProgress(i, persent, inputByte.Length);
            }
        }
        public void DecryptAdamar(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i += 8)
            {
                byte[] array = new byte[8];
                int iterator = 0;
                for (int j = i; j < i + 8; j++)
                {
                    array[iterator] = inputByte[j];
                    iterator++;
                }
                array = EncryptWalsh(array);
                OutputByte.Add(ByteArrayToByte(Divide(array)));
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private void EncryptPeli(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i++)
            {
                byte[] array = ByteToArray((byte)~inputByte[i]);
                array = EncryptWalsh(array);
                OutputByte.AddRange(array);
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private void DecryptPeli(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i += 8)
            {
                byte[] array = new byte[8];
                int iterator = 0;
                for (int j = i; j < i + 8; j++)
                {
                    array[iterator] = inputByte[j];
                    iterator++;
                }
                array = EncryptWalsh(array);
                OutputByte.Add((byte)~ByteArrayToByte(Divide(array)));
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private void EncryptKachmazh(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i++)
            {
                byte[] array = EncryptWalsh(ByteToArray(LeftGray(inputByte[i])));
                OutputByte.AddRange(array);
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private void DecryptKachmazh(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i += 8)
            {
                byte[] array = new byte[8];
                int iterator = 0;
                for (int j = i; j < i + 8 && j < inputByte.Length; j++)
                {
                    array[iterator] = inputByte[j];
                    iterator++;
                }
                array = EncryptWalsh(array);
                OutputByte.Add(LeftGrayBack(ByteArrayToByte(Divide(array))));
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private void EncryptKyli(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i++)
            {
                byte[] array = EncryptWalsh(ByteToArray(RightGray(inputByte[i])));
                OutputByte.AddRange(array);
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private void DecryptKyli(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i += 8)
            {
                byte[] array = new byte[8];
                int iterator = 0;
                for (int j = i; j < i + 8; j++)
                {
                    array[iterator] = inputByte[j];
                    iterator++;
                }
                array = EncryptWalsh(array);
                OutputByte.Add(RightGrayBack(ByteArrayToByte(Divide(array))));
                SetProgress(i, persent, inputByte.Length);
            }
        }
        public void DecryptStohas(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i += 32)
            {
                byte[] array = new byte[32];
                int iterator = 0;
                for (int j = i; j < i + 32; j++)
                {
                    array[iterator] = inputByte[j];
                    iterator++;
                }
                array = Divide(EncryptWalsh(array));
                array = new Transposition(Key.Key).Decrypt(array);
                for (int j = 0; j < array.Length; j += 8)
                {
                    byte[] arrayTmp = new byte[8];
                    int iteratorTmp = 0;
                    for (int n = j; n < j + 8; n++)
                    {
                        arrayTmp[iteratorTmp] = array[n];
                        iteratorTmp++;
                    }
                    OutputByte.Add(ByteArrayToByte(arrayTmp));
                }
                SetProgress(i, persent, inputByte.Length);
            }
        }
        public void EncryptStohas(byte[] inputByte)
        {
            int persent = Persent(inputByte.Length);
            for (int i = 0; i < inputByte.Length; i += 4)
            {
                byte[] array = new byte[4];
                int iterator = 0;
                for (int j = i; j < i + 4 && j < inputByte.Length; j++)
                {
                    array[iterator] = inputByte[j];
                    iterator++;
                }
                array = ByteArrayToBitArray(array);
                array = new Transposition(Key.Key).Encrypt(array);
                array = EncryptWalsh(array);
                OutputByte.AddRange(array);
                SetProgress(i, persent, inputByte.Length);
            }
        }
        private byte[] EncryptWalsh(byte[] inputArray)
        {
            for (int j = 1; j <= inputArray.Length / 2; j <<= 1)
            {
                inputArray = BPFAdd(inputArray, j);
            }
            return inputArray;
        }
        private byte[] Divide(byte[] arra)
        {
            for (int i = 0; i < arra.Length; i++)
                arra[i] = (byte)(arra[i] / arra.Length);
            return arra;
        }
        private byte LeftGray(byte value)
        {
            return (byte)(value ^ (value >> 1));
        }
        private byte RightGray(byte value)
        {
            return (byte)(value ^ (value << 1));
        }
        private byte LeftGrayBack(byte value)
        {
            byte result = 0;
            while (value > 0)
            {
                result ^= value;
                value >>= 1;
            }
            return result;
        }
        private byte RightGrayBack(byte value)
        {
            byte result = 0;
            while (value > 0)
            {
                result ^= value;
                value <<= 1;
            }
            return result;
        }
        private byte[] ByteArrayToBitArray(byte[] array)
        {
            long tmp = 0;
            for(int i = 0; i < array.Length; i++)
            {
                tmp <<= 8;
                tmp = tmp | array[i];
            }
            byte[] result = new byte[32];
            for(int i = result.Length-1; i >= 0; i--)
            {
                if (tmp == 0)
                    break;
                result[i] = (byte)(tmp & 1);
                tmp >>= 1;
            }
            return result;
        } 
        private byte[] BPFAdd(byte[] array, int power)
        {
            byte[] result = new byte[array.Length];
            int i = 0;
            while (i < result.Length)
            {
                for(int j = 0; j < power; j++)
                {
                    result[i] = (byte)(array[i] + array[i + power]);
                    i++;
                }
                for (int j = 0; j < power; j++)
                {
                    result[i] = (byte)(array[i-power] - array[i]);
                    i++;
                }
            }
            return result;
        }
        private byte[] ByteToArray(byte value)
        {
            byte[] result = new byte[8];
            for (int i = 7; i >= 0; i--)
            {
                if (value == 0)
                    break;
                result[i] = (byte)(value & 1);
                value >>= 1;
            }
            return result;
        }
        private byte ByteArrayToByte(byte[] array)
        {
            byte res = 0;
            for(int i = 0; i < array.Length; i++)
            {
                res <<= 1;
                res ^= array[i];
            }
            return res;
        }
        private void SetProgress(int iterator, int persent, int length)
        {
            if (iterator % persent == 0)
            {
                int progress = (int)((double)iterator / length * 100);
                Progress.Invoke(progress);
            }
        }
        private int Persent(int length)
        {
            int persent = length / 100 == 0 ? 1 : length / 100;
            persent++;
            return persent;
        }
    }
}

