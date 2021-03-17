using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ToolsLib
{
    public class DirectoryFiles
    {
        [JsonProperty("dirFiles")]
        public Dictionary<string, byte[]> DirFiles { get; set; }

        [JsonProperty("basePath")]
        public string BasePath { get; set; }

        public DirectoryFiles()
        {
            DirFiles = new Dictionary<string, byte[]>();
        }

        private bool WaitForFile(string fullPath)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath,
                        FileMode.Open, FileAccess.ReadWrite,
                        FileShare.None, 100))
                    {
                        fs.ReadByte();

                        // If we got this far the file is ready
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (numTries > 10)
                    {
                        return false;
                    }

                    // Wait for the lock to be released
                    Thread.Sleep(500);
                }
            }
            return true;
        }

        public DirectoryFiles(string path)
        {
            BasePath = path;
            DirFiles = new Dictionary<string, byte[]>();
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (WaitForFile(file))
                {
                    using (var f = File.Open(file, FileMode.Open))
                    {
                        if (f.CanRead)
                        {
                            Console.WriteLine("CANREADFILE");
                            f.Close();
                            var fileContent = File.ReadAllBytes(file);
                            DirFiles.Add(file, fileContent);
                        }
                    }
                }
            }
        }
    }
}
