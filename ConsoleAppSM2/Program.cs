using System;

namespace ConsoleAppSM2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //生成 SM2 密钥
            //openssl ecparam -name SM2 -genkey -out sm2_ec.key 
            //生成公钥
            //openssl ec -in sm2_ec.key -pubout -out sm2_ec.pubkey 
            //SM3 计算文件Hash
            //echo "https://const.net.cn" > sign.data 
            //openssl dgst -SM3 sign.data
            //使用　SM2 签名文件
            //openssl dgst -SM3 -sign sm2_ec.key -out sm2_ec.sig sign.data 
            //使用　SM2 结合　SM3 验签
            //openssl dgst -SM3 -verify sm2_ec.pubkey -signature sm2_ec.sig sign.data 
            Console.WriteLine("Hello World!");
        }
    }
}
