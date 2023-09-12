using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepUdpFinishSync
{
    public class PlayerData : BaseData
    {
        public int age;
        public string name;
        public bool isMan;
    }
    public class QuitRoomData : BaseData
    {
        public int age;
        public string name;
    }
    public class QuitGameData : BaseData
    {
    }

}
