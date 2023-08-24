using TcpFinishSyc;
using UnityEngine;
using UnityEngine.UI;

namespace StepTcpFinishAsync
{



    public class StepTcpFinishAsync : MonoBehaviour
    {
        public Button btnClose;
        public Button btnLogin;
        public Button btnSend;
        public InputField ifMsg;

        void Start()
        {

            btnClose.onClick.AddListener(() =>
            {

                NetManagerTcpAsync.Ins.Close();
            });

            btnLogin.onClick.AddListener(() =>
            {

                NetManagerTcpAsync.Ins.Connect("127.0.0.1", 8088);
            });



            btnSend.onClick.AddListener(() =>
            {

                if (ifMsg.text.Length > 0)
                {

                    if (ifMsg.text == "1")//退出游戏
                    {


                        NetManagerTcpAsync.Ins.Send(new QuitGameMsg());

                    }
                    else
                    {

                        QuitRoomData data = new QuitRoomData();

                        data.age = 12;
                        data.name = ifMsg.text;//"张望历代";
                        QuitRoomMsg mm = new QuitRoomMsg() { msg = data };
                        NetManagerTcpAsync.Ins.Send(mm);
                    }

                }

            });





        }
        public float HeartClock = 2;
        public void SendHeartMsg()
        {
            if (NetManagerTcpAsync.Ins.IsConnect)
            {
                HeartClock -= Time.deltaTime;

                if (HeartClock <= 0)
                {
                    HeartClock = 2;
                    NetManagerTcpAsync.Ins.Send(new HeartMsg());
                }
            }

        }
        // Update is called once per frame
        void Update()
        {
            SendHeartMsg();


            NetManagerTcpAsync.Ins.ReceiveMsg();
            NetManagerTcpAsync.Ins.SendServer();
        }
    }



}
