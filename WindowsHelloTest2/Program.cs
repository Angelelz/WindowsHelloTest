using System.Security.Cryptography;
namespace WindowsHelloTest2
{
    using System;
    using System.IO;
    using System.Text;
    using WindowsHello;

    /// <summary>
    /// This class decrypts some data with the Windows Hello API from a file.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Decrypts some data with the Windows Hello API from a file.
        /// </summary>
        public static void Main()
        {
            Console.WriteLine("Starting");

            var handle = new IntPtr();
            WinHelloProvider provider = new WinHelloProvider("Hello", handle);
            var parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
            if (parentPath == null)
            {
                Console.WriteLine("Some error occurred with the parent path...");
            }

            var path = Path.Combine(parentPath, "test.dat");
            var encryptedData = File.ReadAllBytes(path);
            //var decryptedData = provider.PromptToDecrypt(encryptedData);

            var ChkResult = WinHelloProvider.NCryptOpenStorageProvider(out var ngcProviderHandle, "Microsoft Passport Key Storage Provider", 0);

            ChkResult = WinHelloProvider.NCryptOpenKey(
                    ngcProviderHandle,
                    out var ngcKeyHandle,
                    WinHelloProvider.CurrentPassportKeyName.Value,
                    0,
                    CngKeyOpenOptions.None);

            var decryptedData = new byte[encryptedData.Length * 2];

            var integ = WinHelloProvider.NCryptDecrypt(ngcKeyHandle, encryptedData, encryptedData.Length, IntPtr.Zero,
                decryptedData, decryptedData.Length, out var pcbResult, WinHelloProvider.NcryptPadPkcs1Flag);

            Array.Resize(ref decryptedData, pcbResult);

            string decryptedString = Encoding.ASCII.GetString(decryptedData);

            Console.WriteLine("Decrypted data: " + decryptedString);

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}