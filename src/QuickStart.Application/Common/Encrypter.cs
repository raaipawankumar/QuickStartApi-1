using System.Security.Cryptography;
using System.Text;


namespace QuickStart.Application.Common;

public static class Encrypter
{
    public static string Encrypt(string text, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = new byte[16];

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(text);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }
    public static string Decrypt(string encryptedText, string key)
    {
        var textBytes = Convert.FromBase64String(encryptedText);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = new byte[16];
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(textBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }
}
