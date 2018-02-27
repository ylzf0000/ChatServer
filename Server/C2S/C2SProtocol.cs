using System.Text;
using ExpType;
namespace ServerSocket.C2S
{
    public abstract class C2SProtocol
    {
        private readonly PROTOCOLTYPE type;
        private byte[] buffer;
        private int cur;

        protected PROTOCOLTYPE Type
        {
            get
            {
                return type;
            }
        }

        public C2SProtocol(PROTOCOLTYPE type, byte[] buffer)
        {
            this.type = type;
            this.buffer = buffer;
            cur = 1;
        }

        public abstract void UnMarshal();

        public byte GetByte()
        {
            return buffer[cur++];
        }

        public string GetString(int count)
        {
            return Encoding.Default.GetString(buffer, cur, count);
        }
    }
}
