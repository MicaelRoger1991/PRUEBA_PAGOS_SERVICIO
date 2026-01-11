using EsApp.CROSS.Encrypt;
using Xunit;

namespace EsApp.Api.Test;

public class EncryptionTests
{
    [Fact]
    public void EncryptAndDecrypt_Roundtrip_ReturnsOriginalValue()
    {
        var encryptor = new SecurityEncrypt("QWERTY0123456789");
        const string plainText = "userdb_esapp";

        var cipherText = encryptor.encrypt(plainText);
        var result = encryptor.dencrypt(cipherText);

        Assert.NotEmpty(cipherText);
        Assert.Equal(plainText, result);
        Console.WriteLine($">>>> Cipher Text: {cipherText}");
    }
}
