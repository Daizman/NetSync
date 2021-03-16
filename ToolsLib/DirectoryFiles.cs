using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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

        public DirectoryFiles(string path)
        {
            BasePath = path;
            DirFiles = new Dictionary<string, byte[]>();
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var f = File.Open(file, FileMode.Open);
                if (f.CanRead)
                {
                    //var fileContent = File.ReadAllBytes(file);
                    byte[] fileBuf = new byte[f.Length];
                    var fileContent = f.Read(fileBuf, 0, (int)f.Length);
                    DirFiles.Add(file, fileBuf);
                }
            }
        }
    }
}
