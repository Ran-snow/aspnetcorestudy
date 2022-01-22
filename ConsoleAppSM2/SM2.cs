using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace ConsoleAppSM2
{
    /// <summary>
    /// SM2
    /// </summary>
    public class SM2
    {
        private readonly Encoding EncodingText;

        public SM2(Encoding encoding)
        {
            EncodingText = encoding;
        }

        private AsymmetricKeyParameter GetPublicKeyParameter(string keyBase64)
        {
            var x9ec = GMNamedCurves.GetByName("SM2P256V1");
            AsymmetricKeyParameter pubKey = new ECPublicKeyParameters(x9ec.Curve.DecodePoint(GetContent(keyBase64)), new ECDomainParameters(x9ec)); ;
            return pubKey;
        }

        private AsymmetricKeyParameter GetPrivateKeyParameter(string keyBase64)
        {
            var x9ec = GMNamedCurves.GetByName("SM2P256V1");
            AsymmetricKeyParameter priKey = new ECPrivateKeyParameters(new BigInteger(1, GetContent(keyBase64)), new ECDomainParameters(x9ec));
            return priKey;
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
            SM2Engine engine = new SM2Engine();

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
        /// 私钥解密
        /// </summary>
        /// <param name="data">待解密的内容</param>
        /// <param name="privateKey">私钥（Base64编码后的）</param>
        /// <returns>返回明文</returns>
        public string DecryptByPrivateKey(string data, string privateKey)
        {
            data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            SM2Engine engine = new SM2Engine();

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
