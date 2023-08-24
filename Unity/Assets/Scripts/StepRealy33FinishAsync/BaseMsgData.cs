using System;

namespace StepTcpFinishAsync
{
    public abstract class BaseMsgData : BaseData
    {

        public abstract int GetID();

        public override byte[] ToByte()
        {
            int len = 4 + 4 + base.GetLen();
            int handlerId = this.GetID();

            byte[] arrMsg = new byte[len];



            BitConverter.GetBytes(len).CopyTo(arrMsg, 0);
            BitConverter.GetBytes(handlerId).CopyTo(arrMsg, 4);

            if (len > 8)
            {
                byte[] tmp = base.ToByte();
                tmp.CopyTo(arrMsg, 8);
            }








            return arrMsg;
        }

        public override int DataByByte(byte[] arrByte, int curIdx = 0)
        {


            curIdx = base.DataByByte(arrByte, curIdx + 8);


            return curIdx;
        }



    }
}
