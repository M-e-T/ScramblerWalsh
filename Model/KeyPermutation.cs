using System;
using System.Linq;

namespace ScramblerWalsh.Model
{
    class KeyPermutation
    {
        private const int KeyLength = 32;
        public int[] Key;
        public KeyPermutation()
        {
            Generate();
        }
        private void Generate()
        {
            var random = new Random();
            Key = Enumerable.Range(0, KeyLength).ToArray();
            for (int i = Key.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = Key[j];
                Key[j] = Key[i];
                Key[i] = temp;
            }
        }
    }
}
