using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScramblerWalsh.Model
{
    public class Transposition
    {
        private int[] key;

        public Transposition(int[] key)
        {
            SetKey(key);
        }
        public void SetKey(int[] _key)
        {
            key = new int[_key.Length];

            for (int i = 0; i < _key.Length; i++)
                key[i] = _key[i];
        }
        public byte[] Encrypt(byte[] input)
        {
            byte[] result = new byte[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[key[i]];
            }

            return result;
        }

        public byte[] Decrypt(byte[] input)
        {
            byte[] result = new byte[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                result[key[i]] = input[i];
            }
            return result;
        }
    }
}
