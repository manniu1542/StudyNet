namespace Game
{
    public class PlayerDataMsg : StepUdpFinishSync.BaseMsgData
    {
        PlayerData playerData;
        public override int GetID()
        {
            return 1001;
        }
    }
}
