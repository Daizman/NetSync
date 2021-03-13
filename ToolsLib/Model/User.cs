using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [JsonProperty("privateKeyRSA")]
        private RSAParameters _privateKeyRSA;

        [JsonProperty("friends")]
        public Friends Friends { get; set; }

        public User()
        {
            _publicKey = GeneratePublicKey();
            Friends = new Friends();
            _userDirectory = new UDirectory();
        }

        public User(string publicKey)
        {
            _publicKey = publicKey;
            Friends = new Friends();
            _userDirectory = new UDirectory();
        }

        public User(string publicKey, UDirectory dir)
        {
            _publicKey = publicKey;
            Friends = new Friends();
            _userDirectory = dir;
        }

        public User(string publicKey, Friends friends)
        {
            _publicKey = publicKey;
            Friends = friends;
            _userDirectory = new UDirectory();
        }

        public User(string publicKey, Friends friends, UDirectory dir)
        {
            _publicKey = publicKey;
            Friends = friends;
            _userDirectory = dir;
        }

        private string GeneratePublicKey()
        {
            var rsa = Cryptographer.GetRSA();
            _publicKeyRSA = rsa.ExportParameters(false);
            _privateKeyRSA = rsa.ExportParameters(true);

            return JsonConvert.SerializeObject(new RSAPublicKeyParameters(_publicKeyRSA));
        }

        public string PublicKey
        {
            get
            {
                return _publicKey;
            }
        }

        public string ReciveMessage(string encryptedDESstr, List<byte[]> dataJ)
        {
            try
            {
                var des = GetDES(encryptedDESstr);
                var buffer = new byte[1024];

                var dataSize = dataJ.Sum(x => x.Length);
                var array = new byte[dataSize];
                int index = 0;
                for (int i = 0; i < dataJ.Count; i++)
                {
                    for (int j = 0; j < dataJ[i].Length; j++)
                    {
                        array[index] = dataJ[i][j];
                        index++;
                    }
                }
                var data = Cryptographer.SymmetricDecrypt(array, des);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private DESCryptoServiceProvider GetDES(string encryptedDESstr)
        {
            var encrypdedDesParams = JsonConvert.DeserializeObject<DESParameters>(encryptedDESstr);

            var desIV = Cryptographer.RSADecrypt(encrypdedDesParams.IV, _privateKeyRSA);
            var desKey = Cryptographer.RSADecrypt(encrypdedDesParams.Key, _privateKeyRSA);

            var des = Cryptographer.GetDES(desIV, desKey);


            return des;
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
