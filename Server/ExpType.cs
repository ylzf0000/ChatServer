namespace ExpType
{
    public enum PROTOCOLTYPE : byte
    {
        USERPWD = 1,
        SIGNATURE = 2,
        LOGINRES
    }

    public enum LOGINRESTYPE : byte
    {
        SUCCESS = 0,
        NOUSER = 1,
        PWDERROR = 2,
        ERROR = 3,
    }
}
