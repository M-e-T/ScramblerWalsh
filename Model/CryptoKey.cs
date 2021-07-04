using ScramblerWalsh.Interface;

namespace ScramblerWalsh.Model
{
    public class CryptoKey : IKey
    {
        public TypeCrypt Matrix { get; }
        public int[] Key { get; }
        public CryptoKey(int[] key, TypeCrypt matrix)
        {
            Key = key;
            Matrix = matrix;
        }
    }
}
