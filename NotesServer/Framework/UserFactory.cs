using FlyBirdCommon.Logger;
using LevelDB;
using StickyNoteCommon.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Logger = FlyBirdCommon.Logger.Logger;

namespace NotesServer.Framework
{
    [AppService("userfactory", Mode = ServiceStartUpMode.StartOnCall)]
    class UserFactory
    {
        private readonly Logger logger = SharedLoggers.GetPlatformLogger();
        private readonly DB DB;

        
    }
}
