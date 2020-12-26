namespace HireMe.Entities.Enums
{
    public enum MessageStates
    {
        Report,
        Read,
        Stared,
        Important,
        Trash
    }

    public enum MessageClient
    {
        Both = 0,
        Sender = 1,
        Receiver = 2
    }
    public enum ToastMessageState
    {
        Success,
        Info,
        Alert,
        Warning,
        Error
    }
}