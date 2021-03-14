using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using ToolsLib.Crypt;

namespace ToolsLib.Model
{
    public class User
    {
        [JsonProperty("publicKey")]
        private readonly string _publicKey;
        [JsonProperty("userDirectory")]
        private UDirectory _userDirectory;
        [JsonProperty("publicKeyRSA")]
        private RSAParameters _publicKeyRSA;

        [JsonProperty("friends")]
        public List<string> Friends { get; set; }

        public User()
        {
            _publicKey = GeneratePublicKey();
            Friends = new List<string>();
            _userDirectory = new UDirectory();
        }

        public User(string publicKey)
        {
            _publicKey = publicKey;
            Friends = new List<string>();
            _userDirectory = new UDirectory();
        }

        public User(string publicKey, UDirectory dir)
        {
            _publicKey = publicKey;
            Friends = new List<string>();
            _userDirectory = dir;
        }

        public User(string publicKey, List<string> friends)
        {
            _publicKey = publicKey;
            Friends = friends;
            _userDirectory = new UDirectory();
        }

        public User(string publicKey, List<string> friends, UDirectory dir)
        {
            _publicKey = publicKey;
            Friends = friends;
            _userDirectory = dir;
        }

        private string GeneratePublicKey()
        {
            var rsa = Cryptographer.GetRSA();
            _publicKeyRSA = rsa.ExportParameters(false);

            return JsonConvert.SerializeObject(new RSAPublicKeyParameters(_publicKeyRSA));
        }

        public string PublicKey
        {
            get
            {
                return _publicKey;
            }
        }

        public UDirectory UserDirectory
        {
            get
            {
                return _userDirectory;
            }
            set
            {
                _userDirectory = value;
            }
        }
    }
}
