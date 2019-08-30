
namespace WindowsHelloTest1
{
    using System;
    using System.IO;
    using System.Text;
    using WindowsHello;

    /// <summary>
    /// This class encrypts some data with the Windows Hello API and writes the encrypted data to a file.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Encrypts some data with the Windows Hello API and writes the encrypted data to a file.
        /// </summary>
        public static void Main()
        {
            Console.WriteLine("Starting.");

            var handle = new IntPtr();
            string dataToEncrypt = "This is the text we want to check";
            byte[] data = Encoding.ASCII.GetBytes(dataToEncrypt);
            Console.WriteLine("Decrypted data: " + dataToEncrypt);
            IAuthProvider provider = new WinHelloProvider("Hello", handle);
            var encryptedData = provider.Encrypt(data);
            Console.WriteLine($"Encrypted data: { BitConverter.ToString(encryptedData).Replace("-", " ")}");
            var parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
            if (parentPath == null)
            {
                Console.WriteLine("Some error occurred with the parent path...");
            }

            var path = Path.Combine(parentPath, "test.dat");
            File.WriteAllBytes(path, encryptedData);

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
