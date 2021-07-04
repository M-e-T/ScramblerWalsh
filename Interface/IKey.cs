using ScramblerWalsh.Model;


namespace ScramblerWalsh.Interface
{
    public interface IKey
    {
        TypeCrypt Matrix { get; }
        int[] Key { get; }
    }
}
