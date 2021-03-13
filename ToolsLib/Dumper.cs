using System;
using System.IO;
using System.Security.Cryptography;

namespace ToolsLib
{
    /// <summary>
    /// Класс для дампа и восстановления пользователя
    /// </summary>
    public static class Dumper
    {
        public static void Dump(string encrString, string encrPath)
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            try
            {
                //Create a file stream
                using (var myStream = new FileStream(encrPath, FileMode.OpenOrCreate))
                {
                    //Create a new instance of the default Aes implementation class  
                    // and configure encryption key.  
                    using (var aes = Aes.Create())
                    {
                        aes.Key = key;

                        //Stores IV at the beginning of the file.
                        //This information will be used for decryption.
                        byte[] iv = aes.IV;
                        myStream.Write(iv, 0, iv.Length);

                        //Create a CryptoStream, pass it the FileStream, and encrypt
                        //it with the Aes class.  
                        using (var cryptStream = new CryptoStream(myStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {

                            //Create a StreamWriter for easy writing to the
                            //file stream.  
                            using (var sWriter = new StreamWriter(cryptStream))
                            {
                                //Write to the stream.  
                                sWriter.WriteLine(encrString);
                            }
                        }
                    }
                }
            }
            catch
            {
                //Inform the user that an exception was raised.  
                Console.WriteLine("The encryption failed.");
                throw;
            }
        }

        public static string Restore(string pathToDecr)
        {
            byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

            try
            {
                //Create a file stream.
                using (var myStream = new FileStream(pathToDecr, FileMode.Open))
                {

                    //Create a new instance of the default Aes implementation class
                    using (var aes = Aes.Create())
                    {

                        //Reads IV value from beginning of the file.
                        byte[] iv = new byte[aes.IV.Length];
                        myStream.Read(iv, 0, iv.Length);

                        //Create a CryptoStream, pass it the file stream, and decrypt
                        //it with the Aes class using the key and IV.
                        using (var cryptStream = new CryptoStream(myStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                        {
                            //Read the stream.
                            using (var sReader = new StreamReader(cryptStream))
                            {
                                //Display the message.
                                return sReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("The decryption failed.");
                throw;
            }
        }
    }
}
