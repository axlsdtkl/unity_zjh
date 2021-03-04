using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//网络消息类
//作用:每次发送消息都发送这个类，接收到消息后需要转换成这个类
public class NetMsg
{
    //操作码
    public int opCode { get; set; }
    //子操作码
    public int subCode { get; set; }
    //传递的参数
    public object value { get; set; }
    public NetMsg()
    {

    }
    public NetMsg(int opCode, int subCode, object value)
    {
        this.opCode = opCode;
        this.subCode = subCode;
        this.value = value;
    }
    public void Change(int opCode, int subCode, object value)
    {
        this.opCode = opCode;
        this.subCode = subCode;
        this.value = value;
    }
}
