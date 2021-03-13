namespace ToolsLib.Crypt
{
    public class DESParameters
    {
        public byte[] IV;
        public byte[] Key;

        public DESParameters(byte[] IV, byte[] Key)
        {
            this.IV = IV;
            this.Key = Key;
        }
    }
}
