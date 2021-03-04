using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LeftRightBaseManager_Stand : BaseManager_Stand
{
    private GameObject txt_Ready;
    private GameObject txt_GiveUp;
    //随机下注时间
    private float m_RandomWaitStakesTime = 0;

    //是否有下注次数
    private bool m_IsHasStakesNum = false;
    //下注次数
    private int m_StakesNum = 0;
    //是否正在比牌
    protected bool m_IsComaring = false;

    public override void CompareWin()
    {

    }
    public override void CompareLose()
    {
        GiveUpCard();
    }
    private void Awake()
    {
        Init();
    }
    //判断是否胜利
    public abstract bool IsWin();
    private void FixedUpdate()
    {
        if (m_IsStartStakes)
        {
            if(IsWin())
            {
                m_IsStartStakes = false;
                return;
            }
            if (m_RandomWaitStakesTime <= 0)//倒计时结束，下注
            {
                //下注
                PutStakes();
                m_IsStartStakes = false;
                if (m_IsComaring == false)
                {
                    go_CountDown.SetActive(false);
                    m_ZjhManager.SetNextPlayerStakes();
                }
                return;
            }
            m_Timer += Time.deltaTime;
            if (m_Timer >= 1)
            {
                m_RandomWaitStakesTime--;
                m_Timer = 0;
                m_Time--;
                txt_CountDown.text = m_Time.ToString();
            }
        }
    }
    private void Init()
    {
        txt_GiveUp = transform.Find("txt_GiveUp").gameObject;
        m_StakesCountHint = transform.Find("StakesCountHint").GetComponent<StakesCountHint>();
        m_ZjhManager = GetComponentInParent<ZjhManager_Stand>();
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        txt_StakesSum = transform.Find("StakesSum/txt_StakesSum").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        CardPoints = transform.Find("CardPoints");
        txt_Ready = transform.Find("txt_Ready").gameObject;

        txt_GiveUp.SetActive(false);
        txt_StakesSum.text = "0";
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        int ran = Random.Range(0, 19);
        string name = "headIcon_" + ran;
        img_HeadIcon.sprite = ResourcesManager.GetSprite(name);
    }
    //比牌
    public abstract void Compare();
    //下注
    private void PutStakes()
    {
        if (m_IsHasStakesNum)
        {
            m_StakesNum--;
            if (m_StakesNum <= 0)//下注次数用完
            {
                GetPutStakesNum();

                //比牌
                m_IsComaring = true;
                Compare();
                StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "看看");

                return;
            }
            int stakes = m_ZjhManager.Stakes(Random.Range(3, 6));
            StakesAfter(stakes, "不看");
        }
        else if (m_CardType == CardType.Duizi)
        {
            int ran = Random.Range(0, 10);
            if (ran < 5)//跟注
            {
                StakesAfter(m_ZjhManager.Stakes(Random.Range(3, 6)), "不看");
            }
            else//比牌
            {
                m_IsComaring = true;
                Compare();
                StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "看看");
            }
        }
        else if (m_CardType == CardType.Min)
        {
            int ran = Random.Range(0, 15);
            if (ran < 5)//跟注
            {
                StakesAfter(m_ZjhManager.Stakes(Random.Range(3, 6)), "不看");
            }
            else if (ran >= 5 && ran < 10)//比牌
            {
                m_IsComaring = true;
                Compare();
                StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "看看");
            }
            else//弃牌
            {
                GiveUpCard();
            }
        }
        else if (m_CardType == CardType.Baozi || m_CardType == CardType.Max)
        {
            StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "不看");
        }
    }
    //弃牌
    protected void GiveUpCard()
    {
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        txt_GiveUp.SetActive(true);
        m_ZjhManager.SetNextPlayerStakes();
        m_IsGiveUpCard = true;

        foreach (var item in go_SpawnCardList)
        {
            Destroy(item);
        }
    }

    //获得下注次数
    protected void GetPutStakesNum()
    {
        if ((int)m_CardType >= 2 && (int)m_CardType <= 4)
        {
            m_IsHasStakesNum = true;
            m_StakesNum = (int)m_CardType * 6;
        }
    }
    //开始选择庄家
    public void StartChooseBanker()
    {
        m_StakesSum += Models.GameModel.BottomStakes;
        txt_StakesSum.text = m_StakesSum.ToString();
        txt_Ready.SetActive(false);
    }


    //发牌结束
    public void DealCardFinished()
    {
        SortCard();
        GetCardType();
        print("左边玩家牌型: " + m_CardType);
    }
    //开始下注
    public override void StartStakes()
    {
        base.StartStakes();
        m_RandomWaitStakesTime = Random.Range(3, 6);
    }
}
