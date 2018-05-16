using ExpType;

namespace ServerSocket
{
    class RegisterProtocol : C2S.C2SProtocol
    {
        public RegisterProtocol(byte[] buffer) : base(PROTOCOLTYPE.REGISTER, buffer)
        {

        }
        public override void UnMarshal()
        {
            int uc = GetByte();
            username = GetString(uc);
            int pc = GetByte();
            password = GetString(pc);
        }
        private string username;
        private string password;

        public string Password { get => password; set => password = value; }
        public string Username { get => username; set => username = value; }
    }
}
