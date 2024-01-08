using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityNet.UnityWebRequset
{

    public class DownloadHandlerCustomABLoad : DownloadHandlerScript
    {
        public UnityEngine.Object Obj;
        byte[] arrBytes;
        int curReceiveIndex;
        string resName;
        public DownloadHandlerCustomABLoad(string resName) : base()
        {
            this.resName = resName;
        }
        public DownloadHandlerCustomABLoad(string resName, byte[] arrBytes) : base(arrBytes)
        {
            this.resName = resName;
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
            var res = AssetBundle.LoadFromMemory(arrBytes);
            Obj = res.LoadAsset(resName);



        }


    }
}
