using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏数据
public class GameModel
{
    //用户信息
    public UserDto userDto { get; set; }

    //底
    public int BottomStakes { get; set; }
    //顶
    public int TopStakes { get; set; }
	
}
