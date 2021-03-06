﻿using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountHandler : BaseHandler
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.Register_SRES:
                Register_SRES((int)value);
                break;
            case AccountCode.Login_SRES:
                Login_SRES((int)value);
                break;
            case AccountCode.GetUserInfo_SRES:
                Models.GameModel.userDto = (UserDto)value;
                //跳转到主场景
                SceneManager.LoadScene("2.Main");
                break;
            case AccountCode.GetRankList_SRES:
                EventCenter.Broadcast(EventDefine.SendRankListDto, value as RankListDto);
                break;
            case AccountCode.UpdateCoinCount_SRES:
                Models.GameModel.userDto.CoinCount = (int)value;
                EventCenter.Broadcast(EventDefine.UpdateCoinCount, (int)value);
                break;
            case AccountCode.ModifyPwd_SRES:
                EventCenter.Broadcast(EventDefine.Hint, "密码更新成功");
                break;
            case AccountCode.Logout_SRES:
                if(((int)value)==0)SceneManager.LoadScene("1.Start");
                break;
            default:
                break;
        }
    }
    //登录服务器的响应
    private void Login_SRES(int value)
    {
        if(value==-1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "用户名不存在");
        }else if(value==-2)
        {
            EventCenter.Broadcast(EventDefine.Hint, "密码不正确");
        }else if(value==-3)
        {
            EventCenter.Broadcast(EventDefine.Hint, "该账号已在线");
        }else if (value == 0)
        {
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.GetUserInfo_CREQ, null);
            EventCenter.Broadcast(EventDefine.Hint, "登录成功");
        }
    }
    //注册服务器的响应
    private void Register_SRES(int value)
    {
        if(value==-1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "用户名已被注册");
            return;
        }
        if (value == 0)
        {
            EventCenter.Broadcast(EventDefine.Hint, "注册成功");
        }
    }
}
