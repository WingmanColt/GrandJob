using HireMe.Services.Core.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace HireMe.Services.Core
{
    public class CipherService : ICipherService
    {
        private readonly IDataProtector _dataProtectionProvider;
        private const string Key = "my-very-long-key-of-no-exact-size";

        public CipherService(IDataProtectionProvider provider)
        {
            _dataProtectionProvider = provider.CreateProtector(Key);
        }

        public string Encrypt(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;

                string protectedPayload = _dataProtectionProvider.Protect(input);
            return protectedPayload;
        }

        public string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText))
                return null;

            string unprotectedPayload = _dataProtectionProvider.Unprotect(cipherText);

            return unprotectedPayload;
        }
    }
}