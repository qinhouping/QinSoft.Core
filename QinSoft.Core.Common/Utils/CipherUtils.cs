using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 加解密工具类
    /// </summary>
    public static class CipherUtils
    {
        /// <summary>
        /// 默认日志
        /// </summary>
        public static ILog DefaultLog { get; set; } = LogManager.GetLogger(typeof(CipherUtils));

        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// HASH
        /// </summary>
        public static byte[] Hash(byte[] data, string hashName)
        {
            try
            {
                HashAlgorithm hash = HashAlgorithm.Create(hashName);
                return hash.ComputeHash(data);
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("hash", e);
                throw e;
            }
        }

        /// <summary>
        /// HASH
        /// </summary>
        public static string Hash(this string text, string hashName, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(text);
            return BitConverter.ToString(Hash(data, hashName)).Replace("-", "");
        }

        /// <summary>
        /// MD5
        /// </summary>
        public static string MD5(this string text, Encoding encoding)
        {
            return Hash(text, "MD5", encoding);
        }

        /// <summary>
        /// MD5
        /// </summary>
        public static string MD5(this string text)
        {
            return Hash(text, "MD5", DefaultEncoding);
        }

        /// <summary>
        /// SHA1
        /// </summary>
        public static string SHA1(this string text)
        {
            return Hash(text, "SHA1", DefaultEncoding);
        }

        /// <summary>
        /// SHA256
        /// </summary>
        public static string SHA256(this string text)
        {
            return Hash(text, "SHA256", DefaultEncoding);
        }

        /// <summary>
        /// SHA256
        /// </summary>
        public static string SHA512(this string text)
        {
            return Hash(text, "SHA512", DefaultEncoding);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        public static byte[] DESDecrypt(byte[] bytes, byte[] key, byte[] iv, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            try
            {
                using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider()
                {
                    Key = key,
                    IV = iv,
                    Mode = mode,
                    Padding = padding
                })
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (CryptoStream crypto = new CryptoStream(memory, provider.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            crypto.Write(bytes, 0, bytes.Length);
                            crypto.FlushFinalBlock();
                        }
                        return memory.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("des decrypt", e);
                throw e;
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        public static string DESDecrypt(this string data, string key, string iv, Encoding encoding)
        {
            byte[] res = DESDecrypt(Convert.FromBase64String(data), encoding.GetBytes(key), encoding.GetBytes(iv));
            return res.ToString(encoding);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        public static string DESDecrypt(this string data, string key, string iv)
        {
            byte[] res = DESDecrypt(Convert.FromBase64String(data), DefaultEncoding.GetBytes(key), DefaultEncoding.GetBytes(iv));
            return res.ToString(DefaultEncoding);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        public static byte[] DESEncrypt(byte[] bytes, byte[] key, byte[] iv, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            try
            {
                using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider()
                {
                    Key = key,
                    IV = iv,
                    Mode = mode,
                    Padding = padding
                })
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (CryptoStream crypto = new CryptoStream(memory, provider.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            crypto.Write(bytes, 0, bytes.Length);
                            crypto.FlushFinalBlock();
                        }
                        return memory.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("des encrypt", e);
                throw e;
            }
        }

        /// <summary>
        /// DES加密
        /// </summary>
        public static string DESEncrypt(string text, string key, string iv, Encoding encoding)
        {
            byte[] res = DESEncrypt(encoding.GetBytes(text), encoding.GetBytes(key), encoding.GetBytes(iv));
            return Convert.ToBase64String(res);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        public static string DESEncrypt(string text, string key, string iv)
        {
            byte[] res = DESEncrypt(DefaultEncoding.GetBytes(text), DefaultEncoding.GetBytes(key), DefaultEncoding.GetBytes(iv));
            return Convert.ToBase64String(res);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public static byte[] AESDecrypt(byte[] bytes, byte[] key, byte[] iv, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            try
            {
                using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider()
                {
                    Key = key,
                    IV = iv,
                    Mode = mode,
                    Padding = padding
                })
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (CryptoStream crypto = new CryptoStream(memory, provider.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            crypto.Write(bytes, 0, bytes.Length);
                            crypto.FlushFinalBlock();
                        }
                        return memory.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("aes decrypt", e);
                throw e;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public static string AESDecrypt(this string data, string key, string iv, Encoding encoding)
        {
            byte[] res = AESDecrypt(Convert.FromBase64String(data), encoding.GetBytes(key), encoding.GetBytes(iv));
            return res.ToString(encoding);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public static string AESDecrypt(this string data, string key, string iv)
        {
            byte[] res = AESDecrypt(Convert.FromBase64String(data), DefaultEncoding.GetBytes(key), DefaultEncoding.GetBytes(iv));
            return res.ToString(DefaultEncoding);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public static byte[] AESEncrypt(byte[] bytes, byte[] key, byte[] iv, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            try
            {
                using (AesCryptoServiceProvider provider = new AesCryptoServiceProvider()
                {
                    Key = key,
                    IV = iv,
                    Mode = mode,
                    Padding = padding
                })
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        using (CryptoStream crypto = new CryptoStream(memory, provider.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            crypto.Write(bytes, 0, bytes.Length);
                            crypto.FlushFinalBlock();
                        }
                        return memory.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("aes encrypt", e);
                throw e;
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public static string AESEncrypt(string text, string key, string iv, Encoding encoding)
        {
            byte[] res = AESEncrypt(encoding.GetBytes(text), encoding.GetBytes(key), encoding.GetBytes(iv));
            return Convert.ToBase64String(res);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public static string AESEncrypt(string text, string key, string iv)
        {
            byte[] res = AESEncrypt(DefaultEncoding.GetBytes(text), DefaultEncoding.GetBytes(key), DefaultEncoding.GetBytes(iv));
            return Convert.ToBase64String(res);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        public static byte[] RSADecrypt(byte[] bytes, string privateKey)
        {
            try
            {
                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlString(privateKey);
                    return provider.Decrypt(bytes, false);
                }
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("ras decrypt", e);
                throw e;
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        public static string RSADecrypt(this string text, string privateKey, Encoding encoding)
        {
            byte[] res = RSADecrypt(Convert.FromBase64String(text), privateKey);
            return res.ToString(encoding);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        public static string RSADecrypt(this string text, string privateKey)
        {
            byte[] res = RSADecrypt(Convert.FromBase64String(text), privateKey);
            return res.ToString(DefaultEncoding);
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        public static byte[] RSAEncrypt(byte[] bytes, string publicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    return rsa.Encrypt(bytes, false);
                }
            }
            catch (Exception e)
            {
                DefaultLog?.Debug("ras encrypt", e);
                throw e;
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        public static string RSAEncrypt(this string text, string publicKey, Encoding encoding)
        {
            byte[] res = RSAEncrypt(encoding.GetBytes(text), publicKey);
            return Convert.ToBase64String(res);
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        public static string RSAEncrypt(this string text, string publicKey)
        {
            byte[] res = RSAEncrypt(DefaultEncoding.GetBytes(text), publicKey);
            return Convert.ToBase64String(res);
        }
    }
}
