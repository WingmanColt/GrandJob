namespace HireMe.Services.Core.Interfaces
{
    public interface ICipherService
    {
        string Encrypt(string input);

        string Decrypt(string cipherText);
    }


}