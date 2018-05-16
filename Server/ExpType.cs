namespace ExpType
{
    public enum PROTOCOLTYPE : byte
    {
        USERPWD = 1,
        SIGNATURE = 2,
        LOGINRES = 3,
        REGISTER = 4,
        REGISTER_RET = 5,
    }

    public enum LOGINRESTYPE : byte
    {
        SUCCESS = 0,
        NOUSER = 1,
        PWDERROR = 2,
        ERROR = 3,
    }

    public enum REGISTERTYPE : byte
    {
        SUCCESS = 0,
        EXIST = 1,
        REGISTERERROR,
    }
}
