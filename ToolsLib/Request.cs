using Newtonsoft.Json;

namespace ToolsLib
{
    public class Request
    {
        [JsonProperty("type")]
        public UserRequestType Type { get; set; }
        [JsonProperty("MainData")]
        public string MainData { get; set; }
        [JsonProperty("AdditionalInfo")]
        public string AdditionalInfo { get; set; }

        public Request(UserRequestType type=UserRequestType.STUB, string mainData="", string additionalInfo="")
        {
            Type = type;
            MainData = mainData;
            AdditionalInfo = additionalInfo;
        }
    }
}
