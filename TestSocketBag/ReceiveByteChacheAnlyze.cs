using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestSocketBag
{
    public class ReceiveByteChacheAnlyze
    {
        //完整包队列 
        public Queue<byte[]> queueFinish = new Queue<byte[]>();



        byte[] arrChache;//

        public byte[] GetNewByteSlice(ref byte[] arr, int startIdx = 0, int count = 0)
        {
            byte[] arrT = new byte[count];

            Array.Copy(arr, startIdx, arrT, 0, count);

            return arrT;
        }

        //1.查看 分包，粘包的情况。 （先查看缓存中的  然后 结合本次 接受的容器，比较 长度）
        //3.把完整的包 取出，不完整的 留到缓存中 

        public void Analyzi(byte[] arrByte, int reiceiveLen)
        {
            if (arrChache == null)
            {
                if (reiceiveLen > 4) //len长度
                {
                    int len = BitConverter.ToInt32(arrByte, 0);
                    if (len < reiceiveLen) //黏包了
                    {

                        //新包
                        var newBy = GetNewByteSlice(ref arrByte, 0, len);
                        queueFinish.Enqueue(newBy);

                        //剩余的未解析包
                        var recei = GetNewByteSlice(ref arrByte, len, reiceiveLen - len);
                        Analyzi(recei, recei.Length);

                    }
                    else if (len == reiceiveLen)
                    {
                        queueFinish.Enqueue(arrByte);
                    }
                    else //分包了
                    {
                        if (arrByte.Length == reiceiveLen)
                            arrChache = arrByte;
                        else
                        {

                            arrChache = GetNewByteSlice(ref arrByte, 0, reiceiveLen);
                        }

                    }


                }
                else
                {
                    arrChache = GetNewByteSlice(ref arrByte, 0, reiceiveLen);
                }


            }
            else
            {  //包  有剩余 处理


                var arr = GetNewByteSlice(ref arrByte, 0, reiceiveLen);



                var tmp = arrChache.Concat(arr).ToArray();
                arrChache = null;
                Analyzi(tmp, tmp.Length);




            }






        }



    }
}
