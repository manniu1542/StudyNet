using StepUdpFinishSync;
namespace Game
{
    public class PlayerDataMsg2 : StepUdpFinishSync.BaseMsgData
    {
        PPPData pppData;
        public override int GetID()
        {
            return 1002;
        }
    }
}
