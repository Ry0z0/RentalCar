using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RentalCar.Auth
{


    public class EncryptionService
    {
        private readonly string _secretKey;

        public EncryptionService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string Encrypt(string plainText)
        {
            var encrypted = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {
                encrypted.Append((char)(plainText[i] ^ _secretKey[i % _secretKey.Length]));
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypted.ToString()));
        }

        public string Decrypt(string cipherText)
        {
            var decodedBytes = Convert.FromBase64String(cipherText);
            var decodedText = Encoding.UTF8.GetString(decodedBytes);
            var decrypted = new StringBuilder();
            for (int i = 0; i < decodedText.Length; i++)
            {
                decrypted.Append((char)(decodedText[i] ^ _secretKey[i % _secretKey.Length]));
            }
            return decrypted.ToString();
        }



    }

}
