namespace NotesServer.Protocol.Net.Handler
{
    enum ResultStatus
    {
        LOGIN = 0,
        SYNC = 1,
        CHAT = 2,
        UPLOAD = 3,
        DOWNLOAD = 4,
        CHANGEPASSWORD = 5, //Used
        PUBLICNOTE = 6, // Used
        MESSAGE = 7 // Used
    }
}
