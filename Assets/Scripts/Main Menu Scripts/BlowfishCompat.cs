// I couldn't find an implementation of Blowfish-Compat anywhere, so I just made one myself.

using System;
using System.Text;

namespace EscapistsMapTools.Encryption {
    internal class BlowfishCompat {
        private readonly Blowfish blowfish;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">string key for decryption/encryption</param>
        public BlowfishCompat(string key) {
            blowfish = new Blowfish(Encoding.ASCII.GetBytes(key));
        }

        /// <summary>
        /// Reverses byte order in 4 byte chunks
        /// </summary>
        /// <param name="data">The bytes to reverse</param>
        /// <returns>The reversed bytes</returns>
        private static byte[] ReverseByteOrder(byte[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            int length = data.Length;
            // apply padding, must be multiples of 8
            if (length % 8 != 0) {
                length += 8 - length % 8;
                Array.Resize(ref data, length);
            }
            
            byte[] reversedBytes = new byte[length];
            // taken from https://github.com/otrtool/otrtool/blob/c7891b06616e62331e9230bff017389c65de2051/src/main.c#L96
            for (int i = 0; i < length; i++) {
                reversedBytes[i + 3 - (i % 4 * 2)] = data[i];
            }
            return reversedBytes;
        }

        /// <summary>
        /// Encrypts data with Blowfish-Compat.
        /// </summary>
        /// <param name="data">The unencrypted data to encrypt</param>
        /// <returns>The encrypted data</returns>
        public byte[] Encrypt(byte[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            byte[] encrypted = blowfish.Encrypt_ECB(ReverseByteOrder(data));
            return ReverseByteOrder(encrypted);
        }

        /// <summary>
        /// Decrypts data with Blowfish-Compat
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <returns>The decrypted data</returns>
        public byte[] Decrypt(byte[] data) {
            if (data == null) throw new ArgumentNullException(nameof(data));
            byte[] decrypted = blowfish.Decrypt_ECB(ReverseByteOrder(data));
            byte[] reverseDecrypted = ReverseByteOrder(decrypted);

            // trim padding
            int i = reverseDecrypted.Length - 1;
            while (reverseDecrypted[i] == 0) {
                i--;
            }
            byte[] temp = new byte[i + 1];
            Array.Copy(reverseDecrypted, temp, i + 1);
            return temp;
        }
    }
}