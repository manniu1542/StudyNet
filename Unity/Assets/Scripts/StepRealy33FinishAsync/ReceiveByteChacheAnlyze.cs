﻿using System;
using System.Collections.Generic;

namespace StepTcpFinishAsync
{
    public class ReceiveByteChacheAnlyze
    {
        //完整包队列 
        public Queue<BaseMsgData> queueFinish = new Queue<BaseMsgData>();


        byte[] arrChache = new byte[NetManagerTcpAsync.preCount * 3];

        int chacheNum;
        int curIdx;




        //1.查看 分包，粘包的情况。 （先查看缓存中的  然后 结合本次 接受的容器，比较 长度）
        //3.把完整的包 取出，不完整的 留到缓存中 




        public void Analyzi( byte[] arrByte, int reiceiveLen)
        {

            arrByte.CopyTo(arrChache, chacheNum);

            int offsetIdx = curIdx;

            while (true)
            {
                if (offsetIdx + 4 >= chacheNum + reiceiveLen) // 长度不够
                {
                    chacheNum += reiceiveLen;
                    curIdx = offsetIdx;
                    break;
                }
                int len = BitConverter.ToInt32(arrChache, offsetIdx);
                if (offsetIdx + len > chacheNum + reiceiveLen) // 长度不够
                {
                    chacheNum += reiceiveLen;
                    curIdx = offsetIdx;
                    break;
                }
                else
                {

                    int id = BitConverter.ToInt32(arrChache, offsetIdx + 4);

                    switch (id)
                    {
                        case HandlerCode.Handler_QuitRoom:
                            QuitRoomMsg msg = new QuitRoomMsg();
                            offsetIdx = msg.DataByByte(arrChache, offsetIdx);
                            queueFinish.Enqueue(msg);
                            break;
                        case HandlerCode.Handler_EnterRoom:
                            PlayerDataMsg msg2 = new PlayerDataMsg();
                            offsetIdx = msg2.DataByByte(arrChache, offsetIdx);
                            queueFinish.Enqueue(msg2);
                            break;
                        case HandlerCode.Handler_QuitGame:


                            queueFinish.Enqueue(new QuitGameMsg());
                            offsetIdx += 8;
                            break;
                        case HandlerCode.Handler_Heart:


                            queueFinish.Enqueue(new HeartMsg());
                            offsetIdx += 8;
                            break;
                        default:

                            Console.WriteLine($"没有该类型的msg：{id}");
                            return;
                    }

                    if (offsetIdx == chacheNum + reiceiveLen)
                    {
                        chacheNum = 0;
                        curIdx = 0;
                        break;
                    }
                }
            }










        }



    }
}
