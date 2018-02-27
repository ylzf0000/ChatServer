using ExpType;

namespace ServerSocket
{
    class UserPwdProtocol : C2S.C2SProtocol
    {
        public UserPwdProtocol(byte[] buffer) : base(PROTOCOLTYPE.USERPWD,buffer)
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

        public string Username
        {
            get
            {
                return username;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }
    }
}
