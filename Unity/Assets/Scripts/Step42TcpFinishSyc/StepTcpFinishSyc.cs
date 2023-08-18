using LessonStep40;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TcpFinishSyc
{
    class StepTcpFinishSyc : MonoBehaviour
    {

        public Button btnClose;
        public Button btnLogin;
        public Button btnSend;
        public InputField ifMsg;

        void Start()
        {
            MainSynchronizationContext.Instance.Init(Thread.CurrentThread.ManagedThreadId);
            btnClose.onClick.AddListener(() =>
            {

                NetManagerTcpSyc.Ins.Close();
            });

            btnLogin.onClick.AddListener(() =>
            {

                NetManagerTcpSyc.Ins.Connect("127.0.0.1", 8088);
            });



            btnSend.onClick.AddListener(() =>
            {

                if (ifMsg.text.Length > 0)
                {

                    if (ifMsg.text == "1")//退出游戏
                    {


                        NetManagerTcpSyc.Ins.Send(new QuitGameMsg());

                    }
                    else
                    {

                        QuitRoomData data = new QuitRoomData();

                        data.age = 12;
                        data.name = ifMsg.text;//"张望历代";
                        QuitRoomMsg mm = new QuitRoomMsg() { msg = data };
                        NetManagerTcpSyc.Ins.Send(mm);
                    }

                }

            });





        }
        public float HeartClock = 2;
        public void SendHeartMsg()
        {
            if (NetManagerTcpSyc.Ins.IsConnect)
            {
                HeartClock -= Time.deltaTime;

                if (HeartClock <= 0)
                {
                    HeartClock = 2;
                    NetManagerTcpSyc.Ins.Send(new HeartMsg());
                }
            }

        }
        // Update is called once per frame
        void Update()
        {
            SendHeartMsg();




            MainSynchronizationContext.Instance.Update();

            NetManagerTcpSyc.Ins.ReceiveMsg();
        }

    }
}
