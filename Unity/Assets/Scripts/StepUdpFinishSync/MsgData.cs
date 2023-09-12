using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepUdpFinishSync
{
    public class PlayerDataMsg : BaseMsgData
    {
        public PlayerData msg;
        public override int GetID()
        {
            return HandlerCode.Handler_EnterRoom;
        }


    }
    public class QuitRoomMsg : BaseMsgData
    {
        public QuitRoomData msg;
        public override int GetID()
        {
            return HandlerCode.Handler_QuitRoom;
        }


    }

    public class QuitGameMsg : BaseMsgData
    {

        public override int GetID()
        {
            return HandlerCode.Handler_QuitGame;
        }


    }
    public class HeartMsg : BaseMsgData
    {

        public override int GetID()
        {
            return HandlerCode.Handler_Heart;
        }


    }

}
