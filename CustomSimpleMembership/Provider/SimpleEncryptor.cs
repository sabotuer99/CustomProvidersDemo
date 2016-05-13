using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace CustomSimpleMembership.Provider
{
    public class SimpleEncryptor : IEncrypting
    {
        public string Encode(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                byte[] mang = System.Text.Encoding.UTF8.GetBytes(password);

                var myMd5 = new MD5CryptoServiceProvider();

                mang = myMd5.ComputeHash(mang);

                return mang.Aggregate("", (current, b) => current + b.ToString("X2"));
            }

            // TODO: logic code above should be double check again
            return string.Empty;
        }
    }

    public class Encryptor : IEncrypting
    {
        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="password">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string Encode(string password)
        {
            // TODO: need to encrypt here
            return password;
        }
    }
}