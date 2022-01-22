using System;
using System.IO;
using System.Text;

namespace ConsoleAppSM2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //生成 SM2 密钥
            //openssl ecparam -name SM2 -genkey -out sm2_ec_private.key
            //生成公钥
            //openssl ec -in sm2_ec_private.key -pubout -out sm2_ec_public.key

            //SM3 计算文件Hash
            //openssl dgst -SM3 plain.txt

            //用公钥加密文件
            //openssl pkeyutl -encrypt -in plain.txt -inkey sm2_ec_public.key -pubin -out encryptByPublicKeySM2Text.txt
            //用私钥解密公钥加密的文件
            //openssl pkeyutl -decrypt -in encryptByPublicKeySM2Text.txt -inkey sm2_ec_private.key -out plain.txt

            //使用 SM2 签名文件
            //openssl dgst -SM3 -sign sm2_ec_private.key -out digest.sm3 plain.txt
            //使用 SM2 结合 SM3 验签
            //openssl dgst -SM3 -verify sm2_ec_public.key -signature digest.sm3 plain.txt

            SM2 sm2 = new SM2(Encoding.UTF8);
            string baseDir = @"E:\Projects\Test\aspnetcorestudy\ConsoleAppSM2\key\";
            //原文
            string plain = "中文，测试0232000100000012021-07-203.0.0";
            //公钥
            string publicKey = File.ReadAllText(baseDir + "sm2_ec_public.key");
            //私钥
            var privateKey = File.ReadAllText(baseDir + "sm2PriKeyPkcs8.pem");

            //https://github.com/dromara/hutool/blob/v5-master/hutool-crypto/src/main/java/cn/hutool/crypto/SmUtil.java
            //SM2Service sM2Service = new SM2Service("MFowFAYIKoEcz1UBgi0GCCqBHM9VAYItA0IABBaA/lAw8B+EoED5vCaN07OGGyOpTnOnPTg+joCiZNR1Q+/NHrDSENdYfU3KlRySM+4FSDi5J03IhJI0bK+TtZI=", "MIGIAgEAMBQGCCqBHM9VAYItBggqgRzPVQGCLQRtMGsCAQEEIESwXQHkW07QwU8iyn7VKV2VYrfWf/QrepNXf0zLBIVroUQDQgAEFoD+UDDwH4SgQPm8Jo3Ts4YbI6lOc6c9OD6OgKJk1HVD780esNIQ11h9TcqVHJIz7gVIOLknTciEkjRsr5O1kg==", SM2Service.Mode.C1C3C2);
            //var test= sM2Service.Encrypt(File.ReadAllBytes(baseDir + "plain.txt"));

            string encryptByPublicKeySM2Text = sm2.EncryptByPublicKey(plain, publicKey);
            File.WriteAllBytes(baseDir + "encryptByPublicKeySM20Text.txt", Convert.FromBase64String(encryptByPublicKeySM2Text));
            string text2 = sm2.DecryptByPrivateKey(encryptByPublicKeySM2Text, privateKey);

            Console.WriteLine("Hello World!");
        }
    }
}
