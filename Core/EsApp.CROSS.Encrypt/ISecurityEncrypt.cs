using System;

namespace EsApp.CROSS.Encrypt;

public interface ISecurityEncrypt
{
    public string encrypt(string plainText);
    public string dencrypt(string cipherText);
}
