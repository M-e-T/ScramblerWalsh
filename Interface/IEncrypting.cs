using System;
using ScramblerWalsh.Model;

namespace ScramblerWalsh.Interface
{
    public interface IEncrypting
    {
        event Action<int> Progress;
        void Encrypt(string fileInput, string fileOutput, TypeCrypt type);
        void Decrypt(string fileInput, string fileOutput, TypeCrypt type);
    }
}
