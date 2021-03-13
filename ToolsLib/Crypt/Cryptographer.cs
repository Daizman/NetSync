using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToolsLib.Crypt
{
    public static class Cryptographer
    {
        public static DESCryptoServiceProvider GetDES()
        {
            return new DESCryptoServiceProvider();
        }

        public static DESCryptoServiceProvider GetDES(byte[] IV, byte[] Key)
        {
            return new DESCryptoServiceProvider()
            {
                IV = IV,
                Key = Key,
            };
        }

        public static RSACryptoServiceProvider GetRSA()
        {
            return new RSACryptoServiceProvider();
        }

        public static byte[] RSAEncrypt(byte[] byteEncrypt, RSAParameters RSAInfo, bool isOAEP = false)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAInfo);
                    encryptedData = RSA.Encrypt(byteEncrypt, isOAEP);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public static byte[] GetBytesOfPublicRSA(RSAParameters RSAInfo)
        {
            var parameters = new RSAPublicKeyParameters(RSAInfo);
            var str = JsonConvert.SerializeObject(parameters);
            return Encoding.UTF8.GetBytes(str);
        }


        public static RSAParameters GetRSAFromBytes(byte[] bytes)
        {
            var publicKeyJson = Encoding.UTF8.GetString(bytes);
            var publicKey = JsonConvert.DeserializeObject<RSAPublicKeyParameters>(publicKeyJson);
            var publicKeyParameters = publicKey.GetRSAParameters();
            return publicKeyParameters;
        }

        public static DESCryptoServiceProvider GetDESFromBytes(byte[] body, RSAParameters RSAInfo)
        {
            var desParameters = JsonConvert.DeserializeObject<DESParameters>(Encoding.UTF8.GetString(body));
            var iv = RSADecrypt(desParameters.IV, RSAInfo);
            var key = RSADecrypt(desParameters.Key, RSAInfo);

            return GetDES(iv, key);
        }

        public static byte[] EncryptDESByRSA(RSAParameters publicRSA, DESCryptoServiceProvider des)
        {
            var iv_ = new byte[des.IV.Length];
            var key_ = new byte[des.Key.Length];
            Array.Copy(des.IV, 0, iv_, 0, des.IV.Length);
            Array.Copy(des.Key, 0, key_, 0, des.Key.Length);

            var iv = RSAEncrypt(iv_, publicRSA);
            var key = RSAEncrypt(key_, publicRSA);
            var encrypdedDes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new DESParameters(iv, key)));
            return encrypdedDes;
        }

        public static byte[] RSADecrypt(byte[] byteDecrypt, RSAParameters RSAInfo, bool isOAEP = false)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAInfo);
                    decryptedData = RSA.Decrypt(byteDecrypt, isOAEP);
                }
                return decryptedData;
            }
            catch (CryptographicException)
            {
                return null;
            }
        }

        public static byte[] SymmetricEncrypt(string strText, SymmetricAlgorithm key)
        {
            var ms = new MemoryStream();
            var crypstream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);
            var sw = new StreamWriter(crypstream);
            sw.WriteLine(strText);
            sw.Close();
            crypstream.Close();
            byte[] buffer = ms.ToArray();
            ms.Close();

            return buffer;
        }

        public static string SymmetricDecrypt(byte[] encryptText, SymmetricAlgorithm key)
        {
            var ms = new MemoryStream(encryptText);
            var crypstream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);
            var sr = new StreamReader(crypstream);
            var val = sr.ReadLine();
            sr.Close();
            crypstream.Close();
            ms.Close();

            return val;
        }
    }
}
