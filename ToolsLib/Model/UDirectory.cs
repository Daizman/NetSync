using Newtonsoft.Json;

namespace ToolsLib.Model
{
    public class UDirectory
    {
        [JsonProperty("path")]
        private string _path = "";

        public UDirectory()
        {
        }

        public UDirectory(string path)
        {
            _path = path;
        }

        public string Path
        {
            set
            {
                if (value is null)
                {
                    _path = "";
                }
                else
                {
                    _path = value.Trim();
                }
            }
            get
            {
                return _path;
            }
        }
    }
}
