﻿using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMsgCenter : MonoBehaviour {
    public static NetMsgCenter Instance;
    private ClientPeer client;
    private void Awake()
    {
        Instance = this;
        //连接以后不摧毁连接体，这样进入下一个场景还能保持网络连接
        DontDestroyOnLoad(gameObject);
        client = new ClientPeer();
        client.Connect("127.0.0.1", 6666);
    }
    //每隔一段时间检测一次
    private void FixedUpdate()
    {
        if (client == null) return;
        //如果消息队列不为空，一直执行处理的方法
        while(client.netMsgQueue.Count>0)
        {
            NetMsg msg = client.netMsgQueue.Dequeue();
            ProcessServerSendMsg(msg);
        }
    }
    #region 发送消息
    public void SendMsg(int opCode, int subCode, object value)
    {
        client.SendMsg(opCode, subCode, value);
    }
    public void SendMsg(NetMsg msg)
    {
        client.SendMsg(msg);
    }
    #endregion

    #region 处理服务器发来的消息
    AccountHandler accountHandler = new AccountHandler();
    MatchHandler matchHandler = new MatchHandler();
    ChatHandler chatHandler = new ChatHandler();
    FightHandler fightHandler = new FightHandler();
    //处理服务器发来的消息
    private void ProcessServerSendMsg(NetMsg msg)
    {
        switch (msg.opCode)
        {
            case OpCode.Account:
                accountHandler.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Chat:
                chatHandler.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Match:
                matchHandler.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Fight:
                fightHandler.OnReceive(msg.subCode, msg.value);
                break;
            default:
                break;
        }
    }
    #endregion

}
