using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelfManager_Stand : BaseManager_Stand
{
    public AudioClip clip_Start;
    public AudioClip clip_GiveUp;
    private GameObject go_BottomButton;
    private Text txt_UserName;
    private Text txt_CoinCount;

    private Button btn_Ready;
    private GameObject txt_GiveUp;

    private Button btn_LookCard;
    private Button btn_FollowStakes;
    private Button btn_AddStakes;
    private Button btn_ComapreCard;
    private Button btn_GiveUp;
    private Toggle tog_2;
    private Toggle tog_5;
    private Toggle tog_10;
    private GameObject go_CompareBtns;
    private Button btn_CompareLeft;
    private Button btn_CompareRight;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        //监听更新金币数量
        EventCenter.AddListener<int>(EventDefine.UpdateCoinCount, UpdateCoinCount);
        Init();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.UpdateCoinCount, UpdateCoinCount);
    }
    private void FixedUpdate()
    {
        if(tog_2.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.gray;
            tog_5.GetComponent<Image>().color = Color.white;
            tog_10.GetComponent<Image>().color = Color.white;
        }
        if (tog_5.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.white;
            tog_5.GetComponent<Image>().color = Color.gray;
            tog_10.GetComponent<Image>().color = Color.white;
        }
        if (tog_10.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.white;
            tog_5.GetComponent<Image>().color = Color.white;
            tog_10.GetComponent<Image>().color = Color.gray;
        }
        if (m_IsStartStakes)
        {
            //判断自身玩家是否胜利
            if(m_ZjhManager.IsSelfWin())
            {
                m_ZjhManager.SelfWin();
                m_IsStartStakes = false;
                return;
            }
            if(m_Time<=0)
            {
                //倒计时结束
                //默认当作跟注处理TODO
                OnFollowButtonClick();
                m_Time = 60;
            }
            m_Timer += Time.deltaTime;
            if(m_Timer>=1)
            {
                m_Timer = 0;
                m_Time--;
                txt_CountDown.text = m_Time.ToString();
            }
        }
    }
    private void Init()
    {
        m_AudioSource = GetComponent<AudioSource>();
        go_CompareBtns = transform.Find("CompareBtns").gameObject;
        //与左边比牌
        btn_CompareLeft = go_CompareBtns.transform.Find("btn_CompareLeft").GetComponent<Button>();
        btn_CompareLeft.onClick.AddListener(OnCompareLeftButtonClick);
        //与右边比牌
        btn_CompareRight = go_CompareBtns.transform.Find("btn_CompareRight").GetComponent<Button>();
        btn_CompareRight.onClick.AddListener(OnCompareRightButtonClick);

        m_StakesCountHint = transform.Find("StakesCountHint").GetComponent<StakesCountHint>();
        m_ZjhManager = GetComponentInParent<ZjhManager_Stand>();
        go_BottomButton = transform.Find("BottomButton").gameObject;
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("Coin/txt_CoinCount").GetComponent<Text>();
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        txt_StakesSum = transform.Find("StakesSum/txt_StakesSum").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();

        //准备按钮
        btn_Ready = transform.Find("btn_Ready").GetComponent<Button>();
        btn_Ready.onClick.AddListener(OnReadyButtonClick);
        txt_GiveUp = transform.Find("txt_GiveUp").gameObject;

        CardPoints = transform.Find("CardPoints");

        btn_LookCard = go_BottomButton.transform.Find("btn_LookCard").GetComponent<Button>();
        btn_LookCard.onClick.AddListener(OnLookCardButtonClick);

        btn_FollowStakes = go_BottomButton.transform.Find("btn_FollowStakes").GetComponent<Button>();
        btn_FollowStakes.onClick.AddListener(OnFollowButtonClick);

        btn_AddStakes = go_BottomButton.transform.Find("btn_AddStakes").GetComponent<Button>();
        btn_AddStakes.onClick.AddListener(OnAddButtonClick);
        btn_ComapreCard = go_BottomButton.transform.Find("btn_ComapreCard").GetComponent<Button>();
        btn_ComapreCard.onClick.AddListener(OnCopareButtonClick);
        btn_GiveUp = go_BottomButton.transform.Find("btn_GiveUp").GetComponent<Button>();
        btn_GiveUp.onClick.AddListener(OnGiveUpCardButtonClick);

        tog_2 = go_BottomButton.transform.Find("tog_2").GetComponent<Toggle>();
        tog_5 = go_BottomButton.transform.Find("tog_5").GetComponent<Toggle>();
        tog_10 = go_BottomButton.transform.Find("tog_10").GetComponent<Toggle>();

        //以下为了确定点击底栏每个框是精准的，不然就是长方形
        btn_LookCard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_FollowStakes.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_AddStakes.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_ComapreCard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_GiveUp.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_2.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_5.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_10.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;


        go_BottomButton.SetActive(false);
        img_Banker.gameObject.SetActive(false);
        txt_GiveUp.SetActive(false);
        go_CountDown.SetActive(false);
        go_CompareBtns.SetActive(false);

        txt_StakesSum.text = "0";

        //如果用户有登录，则有登录信息
        if(Models.GameModel.userDto!=null)
        {
            img_HeadIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
            txt_UserName.text = Models.GameModel.userDto.UserName;
            txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
        }

    }
    //与左边玩家比牌按钮点击
    private void OnCompareLeftButtonClick()
    {
        go_CompareBtns.SetActive(false);
        m_ZjhManager.SelfCompareLeft();
        SetBottomButtonInteractable(false);
        StakesAfter(m_ZjhManager.Stakes(0), "看看");
    }
    //与右边玩家比牌按钮点击
    private void OnCompareRightButtonClick()
    {
        go_CompareBtns.SetActive(false);
        m_ZjhManager.SelfCompareRight();
        SetBottomButtonInteractable(false);
        StakesAfter(m_ZjhManager.Stakes(0), "看看");
    }
    //比牌按钮点击
    private void OnCopareButtonClick()
    {
        go_CompareBtns.SetActive(true);
        if(m_ZjhManager.LeftIsGiveUp)
        {
            btn_CompareLeft.gameObject.SetActive(false);
        }
        if(m_ZjhManager.RightIsGiveUp)
        {
            btn_CompareRight.gameObject.SetActive(false);
        }
    }
    //加注按钮点击
    private void OnAddButtonClick()
    {
        if(tog_2.isOn)
        {
            StakesAfter(m_ZjhManager.Stakes(m_ZjhManager.Stakes(0) * 1),"不看");
        }
        if (tog_5.isOn)
        {
            StakesAfter(m_ZjhManager.Stakes(m_ZjhManager.Stakes(0) * 4), "不看");
        }
        if (tog_10.isOn)
        {
            StakesAfter(m_ZjhManager.Stakes(m_ZjhManager.Stakes(0) * 9), "不看");
        }
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        SetBottomButtonInteractable(false);
        m_ZjhManager.SetNextPlayerStakes();
        go_CompareBtns.SetActive(false);
    }
    //弃牌按钮点击
    private void OnGiveUpCardButtonClick()
    {
        //播放弃牌音效
        m_AudioSource.clip = clip_GiveUp;
        m_AudioSource.Play();

        go_CompareBtns.SetActive(false);
        m_IsStartStakes = false;
        go_BottomButton.SetActive(false);
        go_CountDown.SetActive(false);
        m_IsGiveUpCard = true;
        txt_GiveUp.SetActive(true);

        foreach (var item in go_SpawnCardList)
        {
            Destroy(item);
        }
        m_ZjhManager.SetNextPlayerStakes();
    }
    //跟注按钮点击
    private void OnFollowButtonClick()
    {
        go_CompareBtns.SetActive(false);
        int Stakes=m_ZjhManager.Stakes(0);
        m_ZjhManager.SetNextPlayerStakes();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        SetBottomButtonInteractable(false);
        StakesAfter(Stakes, "不看");
    }
    protected override void StakesAfter(int count, string str)
    {
        base.StakesAfter(count, str);
        if (NetMsgCenter.Instance != null)
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, -count);
    }
    //看牌按钮点击
    private void OnLookCardButtonClick()
    {
        btn_LookCard.interactable = false;
        for(int i=0;i<m_CardList.Count;i++)
        {
            string cardName = "card_" + m_CardList[i].Color.ToString() + "_" + m_CardList[i].Weight.ToString();
            go_SpawnCardList[i].GetComponent<Image>().sprite = ResourcesManager.LoadCardSprite(cardName);
        }
    }
    //准备按钮点击
    private void OnReadyButtonClick()
    {
        //播放开始游戏音效
        m_AudioSource.clip = clip_Start;
        m_AudioSource.Play();
        //更新总下注信息显示
        m_StakesSum += Models.GameModel.BottomStakes;
        txt_StakesSum.text = m_StakesSum.ToString();

        if(NetMsgCenter.Instance!=null)
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, -Models.GameModel.BottomStakes);

        //准备按钮消失
        btn_Ready.gameObject.SetActive(false);
        m_ZjhManager.ChooseBanker();

    }
    //设置底部按钮是否可以交互
    private void SetBottomButtonInteractable(bool value)
    {
        btn_FollowStakes.interactable = value;
        btn_AddStakes.interactable = value;
        btn_ComapreCard.interactable = value;
        btn_GiveUp.interactable = value;
        tog_2.interactable = value;
        tog_5.interactable = value;
        tog_10.interactable = value;
    }
    //金币数量更新后的调用
    private void UpdateCoinCount(int value)
    {
        txt_CoinCount.text = value.ToString();
    }
    
    //发牌结束
    public void DealCardFinished()
    {
        SetBottomButtonInteractable(false);
        go_BottomButton.SetActive(true);

        SortCard();
        GetCardType();
        print("自身玩家牌型: " + m_CardType);
    }
    //开始下注
    public override void StartStakes()
    {
        base.StartStakes();
        SetBottomButtonInteractable(true);
    }

    public override void CompareLose()
    {
        OnGiveUpCardButtonClick();
    }

    public override void CompareWin()
    {
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZjhManager.m_CurrentStakesIndex = 0;
        m_ZjhManager.SetNextPlayerStakes();
    }
}
