using StepUdpFinishSync;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityNet.UnityWebRequset
{
    using BaseMsgData = StepUdpFinishSync.BaseMsgData;

    public class DownloadHandlerCustomMsg : DownloadHandlerScript
    {

        byte[] arrBytes;
        int curReceiveIndex;
        BaseMsgData msgData;
        public DownloadHandlerCustomMsg() : base()
        {

        }

        protected override byte[] GetData()
        {

            return arrBytes;
        }


        /// <summary>
        /// 得到的 字节
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        protected override bool ReceiveData(byte[] data, int dataLength)
        {

            data.CopyTo(arrBytes, curReceiveIndex);
            curReceiveIndex += dataLength;
            return true;
        }


        /// <summary>
        /// 得到总下载字节数
        /// </summary>
        /// <param name="contentLength"></param>
        protected override void ReceiveContentLengthHeader(ulong contentLength)
        {
            curReceiveIndex = 0;
            arrBytes = new byte[contentLength];
        }
        /// <summary>
        /// 下载完成
        /// </summary>
        protected override void CompleteContent()
        {
       
            int handlerType = BitConverter.ToInt32(arrBytes, 4);

            switch (handlerType)
            {
                case HandlerCode.Handler_EnterRoom:
                    msgData = new PlayerDataMsg();
                    msgData.DataByByte(arrBytes);
                   

                    break;
                case HandlerCode.Handler_QuitGame:
                    Debug.Log("<Handler_QuitGame>");

                    break;


                default:
                    Debug.LogError("没有找到消息id：" + handlerType);
                    break;
            }

        }
   

        public T GetMsgData<T>() where T : BaseMsgData
        {

            return msgData != null ? msgData as T : null;
        }
        public object GetMsgData(Type type)
        {
            if (type.IsSubclassOf(typeof(BaseMsgData)))
            {
                if (msgData != null && msgData.GetType() == type)
                {
                    return msgData;
                }
            }

            return null;
        }
    }
}
