using System;
using System.IO;
using System.Text;

namespace ConsoleAppRSA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RSA rsa = new RSA(encoding: Encoding.UTF8);

            //生成RSA私钥(无加密) PKCS#1-PEM
            //openssl genrsa -out rsa_private.key 2048
            //生成RSA私钥(使用aes256加密) PKCS#1-PEM
            //openssl genrsa -aes256 -passout pass:111111 -out rsa_aes_private.key 2048
            //生成RSA公钥
            //openssl rsa -in rsa_aes_private.key -pubout -out rsa_public.key

            //私钥转非加密
            //openssl rsa -in rsa_aes_private.key -passin pass:111111 -out rsa_private.key
            //私钥转加密
            //openssl rsa -in rsa_private.key -aes256 -passout pass:111111 -out rsa_aes_private.key
            //私钥PKCS#1转PKCS#8
            //openssl pkcs8 -topk8 -in rsa_private.key -passout pass:111111 -out pkcs8_private.key

            //用公钥加密文件
            //openssl rsautl -encrypt -in plain.txt -inkey rsa_public.key -pubin -out encryptByPublicKeyText.txt
            //用私钥解密公钥加密的文件
            //openssl rsautl -decrypt -in encryptByPublicKeyText.txt -inkey rsa_aes_private.key -out plain.txt

            //创建摘要
            //openssl dgst -sha1 -sign rsa_aes_private.key -out digest.sha1 plain.txt
            //验证摘要
            //openssl dgst -sha1 -verify rsa_public.key -signature digest.sha1 plain.txt

            string baseDir = @"C:\Users\aaa\Desktop\ConsoleAppRSA\key\";
            //原文
            string plain = "中文，测试0232000100000012021-07-203.0.0";
            //加密过后的私钥
            string encryptedPrivateKey = File.ReadAllText(baseDir + "rsa_aes_private.key");
            //公钥
            string publicKey = File.ReadAllText(baseDir + "rsa_public.key");
            //私钥
            var privateKey = rsa.GetPrivateKeyFromCert("111111", encryptedPrivateKey);

            string sign = rsa.GenerateSignature(privateKey, plain);
            bool result = rsa.VerifySignature(publicKey, sign, plain);

            //string encryptByPrivateKeyText = rsa.EncryptByPrivateKey(plain, privateKey);
            //File.WriteAllBytes(baseDir + "encryptByPrivateKeyText.txt", Convert.FromBase64String(encryptByPrivateKeyText));
            //string text1 = rsa.DecryptByPublicKey(encryptByPrivateKeyText, publicKey);

            string encryptByPublicKeyText = rsa.EncryptByPublicKey(plain, publicKey);
            File.WriteAllBytes(baseDir + "encryptByPublicKeyText.txt", Convert.FromBase64String(encryptByPublicKeyText));
            string text2 = rsa.DecryptByPrivateKey(encryptByPublicKeyText, privateKey);
        }
    }
}
