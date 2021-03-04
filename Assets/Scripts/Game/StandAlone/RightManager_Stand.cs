using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightManager_Stand : LeftRightBaseManager_Stand
{
    //比牌
    public override void Compare()
    {
        m_ZjhManager.RightPlayerCompare();
    }
    public override void CompareWin()
    {
        base.CompareWin();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZjhManager.m_CurrentStakesIndex = 2;
        m_ZjhManager.SetNextPlayerStakes();

        m_IsComaring = false;
    }
    public override bool IsWin()
    {
        if (m_ZjhManager.IsRightWin())
        {
            //如果右边玩家胜利，调用右边玩家胜利的方法
            m_ZjhManager.RightWin();
            return true;
        }
        return false;
    }
}
