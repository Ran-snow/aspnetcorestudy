using System;
using System.IO;
using System.Text;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.X509;

namespace ConsoleAppRSA
{
    /// <summary>
    /// RSA
    /// </summary>
    /// <remarks>
    /// https://www.cnblogs.com/dj258/p/6049786.html
    /// </remarks>
    public class RSA
    {
        private readonly Encoding EncodingText;

        public RSA(Encoding encoding)
        {
            EncodingText = encoding;
        }

        /// <summary>
        /// KEY 结构体
        /// </summary>
        public struct RSAKEY
        {
            /// <summary>
            /// 公钥
            /// </summary>
            public string PublicKey { get; set; }
            /// <summary>
            /// 私钥
            /// </summary>
            public string PrivateKey { get; set; }
        }

        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <returns></returns>
        public RSAKEY GenerateKeyPair()
        {
            //https://stackoverflow.com/questions/3087049/bouncy-castle-rsa-keypair-generation-using-lightweight-api

            //RSA密钥对的构造器  
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();

            //RSA密钥构造器的参数  
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
                BigInteger.ValueOf(4),
                new SecureRandom(),
                2048,   //密钥长度  
                112);

            //用参数初始化密钥构造器  
            keyGenerator.Init(param);
            //产生密钥对  
            AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();
            //获取公钥和密钥  
            AsymmetricKeyParameter publicKey = keyPair.Public;
            AsymmetricKeyParameter privateKey = keyPair.Private;

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();

            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded("UTF-8");
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            RSAKEY item = new RSAKEY()
            {
                PublicKey = Convert.ToBase64String(publicInfoByte),
                PrivateKey = Convert.ToBase64String(privateInfoByte)
            };

            return item;
        }

        private AsymmetricKeyParameter GetPublicKeyParameter(string keyBase64)
        {
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(GetContent(keyBase64));
            return pubKey;
        }

        private AsymmetricKeyParameter GetPrivateKeyParameter(string keyBase64)
        {
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(GetContent(keyBase64));
            return priKey;
        }

        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="data">加密内容</param>
        /// <param name="privateKey">私钥（Base64后的）</param>
        /// <returns>返回Base64内容 PKCS#1</returns>
        public string EncryptByPrivateKey(string data, string privateKey)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密  
            try
            {
                engine.Init(true, GetPrivateKeyParameter(privateKey));
                byte[] byteData = EncodingText.GetBytes(data);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);

                return Convert.ToBase64String(resultData);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="data">待解密的内容</param>
        /// <param name="privateKey">私钥（Base64编码后的）</param>
        /// <returns>返回明文</returns>
        public string DecryptByPrivateKey(string data, string privateKey)
        {
            data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //解密  
            try
            {
                engine.Init(false, GetPrivateKeyParameter(privateKey));
                byte[] byteData = Convert.FromBase64String(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return EncodingText.GetString(ResultData);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="data">加密内容</param>
        /// <param name="publicKey">公钥（Base64编码后的）</param>
        /// <returns>返回Base64内容 PKCS#1</returns>
        public string EncryptByPublicKey(string data, string publicKey)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密  
            try
            {
                engine.Init(true, GetPublicKeyParameter(publicKey));
                byte[] byteData = EncodingText.GetBytes(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Convert.ToBase64String(ResultData);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="data">待解密的内容</param>
        /// <param name="publicKey">公钥（Base64编码后的）</param>
        /// <returns>返回明文</returns>
        public string DecryptByPublicKey(string data, string publicKey)
        {
            data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());
            //解密  
            try
            {
                engine.Init(false, GetPublicKeyParameter(publicKey));
                byte[] byteData = Convert.FromBase64String(data);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return EncodingText.GetString(resultData);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 验证签名 SHA1WithRSA
        /// </summary>
        /// <param name="publicKey">公钥（Base64编码后的）</param>
        /// <param name="signature">签名（Base64编码后的）</param>
        /// <param name="data">原文</param>
        /// <returns>验证结果</returns>
        public bool VerifySignature(string publicKey, string signature, string data)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(data);
                byte[] sigBytes = Convert.FromBase64String(signature);

                ISigner signer = SignerUtilities.GetSigner("SHA1WithRSA");
                signer.Init(false, GetPublicKeyParameter(publicKey));
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="privateKey">私钥（Base64编码后的）</param>
        /// <param name="data">原文</param>
        /// <param name="algorithm">摘要算法</param>
        /// <returns></returns>
        public string GenerateSignature(string privateKey, string data, string algorithm = "SHA1WithRSA")
        {
            try
            {
                byte[] msgBytes = EncodingText.GetBytes(data);

                ISigner signer = SignerUtilities.GetSigner(algorithm);
                signer.Init(true, GetPrivateKeyParameter(privateKey));
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return Convert.ToBase64String(signer.GenerateSignature());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 解密私钥
        /// </summary>
        /// <param name="password">加密私钥的密码</param>
        /// <param name="key">PKCS8格式的私钥</param>
        /// <returns></returns>
        public string GetPrivateKeyFromCert(string password, string key)
        {
            var keyParams = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.DecryptKey(password.ToCharArray(), GetContent(key));
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyParams);
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            return Convert.ToBase64String(privateInfoByte);
        }

        /// <summary>
        /// Function for reading OpenSSL PEM encoded streams containing X509 certificates, PKCS8 encoded keys and PKCS7 objects.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static byte[] GetContent(string text)
        {
            StringReader stringReader = new StringReader(text);
            PemReader pemReader = new PemReader(stringReader);
            PemObject pem = pemReader.ReadPemObject();

            if (pem == null)
            {
                text = text.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                return Convert.FromBase64String(text);
            }

            return pem.Content;
        }
    }
}
