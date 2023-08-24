using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepTcpFinishAsync
{
    public static class HandlerCode
    {
        //只能客户端向服务端发送的请求
        public const int Handler_Heart = 9999;
        public const int Handler_QuitGame = 1003;

        //双向
        public const int Handler_EnterRoom = 1000;
        public const int Handler_QuitRoom = 1002;


    }
}