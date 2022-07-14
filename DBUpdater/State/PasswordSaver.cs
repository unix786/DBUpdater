// Contour Enterprise 
// Copyright (c) 2000-2018 Contour Lab
// 2018-11-24 AR

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CECommon;
using CECommon.Extensions;

namespace DBUpdater.State
{
    /// <summary>
    /// Сохраняет и восстанавливает пароль под текущим пользователем (<see cref="DataProtectionScope.CurrentUser"/>).
    /// </summary>
    /// <remarks>
    /// Это модификация CESvydisLTApp.Exports.Equinox.Internal.PasswordSaver.
    /// </remarks>
    internal static class PasswordSaver
    {
        private static class MyEncrypter
        {
            private static void ClearArray(byte[] arr)
            {
                if (arr.Length % minBlockSize == 0) ProtectedMemory.Protect(arr, MemoryProtectionScope.SameProcess);
                else Array.Clear(arr, 0, arr.Length); // Это не гарантирует, что оригинальные данные будут стерты.
            }

            #region PaddingMethod
            private const int minBlockSize = 16; // ProtectedMemory требует "multiple of 16 bytes".
            private const int blockSize = 8 * minBlockSize;
            private static PaddingMode PaddingMode { get { return PaddingMode.ISO10126; } }

            /// <summary>
            /// 256 бит. Это чтобы повторно не вызывать <see cref="Rfc2898DeriveBytes"/>.
            /// </summary>
            private static readonly byte[] key = new byte[]
            {
                0x9f, 0xb0, 0x2d, 0x76,
                0x9f, 0x6e, 0x93, 0x84,
                0x79, 0x4f, 0xc9, 0x5d,
                0xff, 0xb4, 0x64, 0x08,
                0x25, 0x8a, 0xb3, 0x31,
                0xa6, 0xf4, 0xae, 0xff,
                0xf4, 0xae, 0xff, 0x7c,
                0x44, 0xcb, 0x7c, 0x44
            };

            private static AesCryptoServiceProvider CreateCryptoProvider()
            {
                return new AesCryptoServiceProvider
                {
                    BlockSize = blockSize,
                    Key = key,
                    Padding = PaddingMode,
                    Mode = CipherMode.ECB, // нас здесь не интересует строгое шифрование.
                    //Mode = CipherMode.CFB, IV = key.Take(blockSize / 8).ToArray() // тест
                };
            }

            private static void PaddingMethod(ref byte[] bytes, bool isAdd)
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoProvider = CreateCryptoProvider())
                    using (var cryptoStream = new CryptoStream(
                        memoryStream,
                        isAdd ? cryptoProvider.CreateEncryptor() : cryptoProvider.CreateDecryptor(),
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }
                    var buffer = memoryStream.ToArray();
                    ClearArray(bytes);
                    bytes = buffer;
                }
            }
            #endregion

            /// <summary>
            /// Заменяет данный массив зашифрованным. При этом пытается уничтожить первоначальные данные.
            /// </summary>
            /// <remarks>
            /// Ключом является текущий пользователь.
            /// </remarks>
            public static void Encrypt(ref byte[] bytes)
            {
                // Размер зашифрованного массива должен мало зависеть от входящего массива.
                // Здесь в принципе ещё не шифруем, а используем реализацию PaddingMode.
                PaddingMethod(ref bytes, true);
                // В итоге охрану данных обеспечивает ОС. Мы просто превратили их в более подходящий вид.
                var buffer = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
                ClearArray(bytes);
                bytes = buffer;
            }

            public static byte[] Decrypt(byte[] bytes)
            {
                var unprotectedBytes = new byte[bytes.Length];
                bytes.CopyTo(unprotectedBytes, 0);
                unprotectedBytes = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);

                PaddingMethod(ref unprotectedBytes, false);
                return unprotectedBytes;
            }

            private static void Test()
            {
                var bytes = checkValue.ToArray();
                PaddingMethod(ref bytes, true);
                PaddingMethod(ref bytes, false);
                if (!bytes.SequenceEqual(checkValue)) throw new Exception();

                Encrypt(ref bytes);
                bytes = Decrypt(bytes);
                if (!bytes.SequenceEqual(checkValue)) throw new Exception();

                var bytes2 = bytes.ToArray();
                Encrypt(ref bytes);
                Encrypt(ref bytes2);
                /*
                Повторное шифрование даёт разные результаты (когда используется PaddingMode.ISO10126),
                но в принципе они различаются только конечным блоком.
                */
                if (bytes.SequenceEqual(bytes2)) throw new Exception();
            }
        }

        #region Кодирование
        private static Encoding Encoding { get { return Encoding.UTF8; } }
        private const byte version = 0;

        /// <summary>
        /// 16 байт.
        /// </summary>
        private static readonly byte[] checkValue = new byte[]
        {
            0x3e, 0xfb, 0x93, 0x64,
            0x2a, 0x13, 0x9d, 0xb5,
            0x74, 0xbb, 0x95, 0xca,
            0x41, 0x5f, 0x0b, 0xb9
        };

        private static void WriteInternal(string fn, IWriter saver, string value)
        {
            // Metadata можно бы слить с Value, но не хочу на этом уровне делать какие нибудь манипуляции массива с конфиденциальными данными.
            var metaBytes = new byte[checkValue.Length];
            checkValue.CopyTo(metaBytes, 0);
            MyEncrypter.Encrypt(ref metaBytes);
            int metaLen = metaBytes.Length;

            var valueBytes = Encoding.GetBytes(value);
            MyEncrypter.Encrypt(ref valueBytes);
            // ----
            const int metaOffset = 1 + sizeof(int);
            var resBytes = new byte[metaOffset + metaBytes.Length + valueBytes.Length];
            // Version:
            resBytes[0] = version;
            // Metadata:
            BitConverter.GetBytes(metaLen).CopyTo(resBytes, 1);
            metaBytes.CopyTo(resBytes, metaOffset);
            // Value:
            valueBytes.CopyTo(resBytes, metaOffset + metaBytes.Length);
            // ----
            if (saver is RegistryWriter registryWriter) registryWriter.WriteBinary(fn, resBytes);
            else saver.Write(fn, resBytes.ToHexString());
        }

        private static string ReadInternal(string fn, IWriter saver)
        {
            byte[] bytes;
            if (saver is RegistryWriter registryWriter) bytes = registryWriter.ReadBinary(fn);
            else bytes = Str.HexToBytes(saver.Read(fn));
            if (bytes == null) return null;
            // ----
            const int metaOffset = 1 + sizeof(int);
            if (bytes.Length < metaOffset) return null;
            // Version:
            if (bytes[0] != version) return null;
            // Metadata:
            int metaLen = BitConverter.ToInt32(bytes, 1);
            var metaBytes = new byte[metaLen];
            if (bytes.Length <= metaOffset + metaLen) return null;
            Array.Copy(bytes, metaOffset, metaBytes, 0, metaLen);
            metaBytes = MyEncrypter.Decrypt(metaBytes);
            if (!metaBytes.SequenceEqual(checkValue)) return null; // Возможно, что изменился контекст и уже не можем расшифровать.
            // Value:
            int valueOffset = metaOffset + metaLen;
            var valueBytes = new byte[bytes.Length - valueOffset];
            Array.Copy(bytes, valueOffset, valueBytes, 0, valueBytes.Length);
            // ----
            valueBytes = MyEncrypter.Decrypt(valueBytes);
            return Encoding.GetString(valueBytes);
        }
        #endregion

        /// <summary>
        /// Метод должен сохранить значение и её зашифровать.
        /// </summary>
        public static void Save(string fn, IWriter saver, string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                saver.Delete(fn);
            }
            else
            {
                try
                {
                    WriteInternal(fn, saver, value);
                }
                catch (Exception ex)
                {
                    MainTrace.WriteSuppresed(ex);
                    saver.Delete(fn);
                }
            }
        }

        /// <summary>
        /// Должен попытаться восстановить пароль. Метод должен вернуть null если значения нету или его не удаётся расшифровать.
        /// </summary>
        public static string Load(string fn, IWriter saver)
        {
            try
            {
                return ReadInternal(fn, saver);
            }
            catch (Exception ex)
            {
                MainTrace.WriteSuppresed(ex);
                return null;
            }
        }
    }
}
