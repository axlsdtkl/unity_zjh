using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZjhManager_Stand : MonoBehaviour
{
    //发牌动画持续时间
    public float m_DealCardDurationTime = 0.3f;
    private Text txt_BottomStakes;
    private Text txt_TopStakes;
    private Button btn_Back;
    private LeftManager_Stand m_LeftManager;
    private RightManager_Stand m_RightManager;
    private SelfManager_Stand m_SelfManager;
    private AudioSource m_AudioSource;

    //左边玩家是否弃牌
    public bool LeftIsGiveUp { get { return m_LeftManager.m_IsGiveUpCard; } }
    //右边玩家是否弃牌
    public bool RightIsGiveUp { get { return m_RightManager.m_IsGiveUpCard; } }
    //当前发牌的游标
    private int m_CurrentDealCardIndex = 0;
    //当前下注的游标
    public int m_CurrentStakesIndex = 0;
    //牌库
    private List<Card> m_CardList = new List<Card>();

    //发牌的下标
    private int m_DealCardIndex = 0;


    //是否开始下注
    private bool m_IsStartStakes = false;
    //下一位玩家是否可以下注
    private bool m_IsNextPlayerCanStakes = true;

    //设置下一位玩家可以下注
    public void SetNextPlayerStakes()
    {
        m_IsNextPlayerCanStakes = true;
    }
    //上一位玩家下注的数量
    private int m_LastPlayerStakesCount = 0;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_SelfManager = GetComponentInChildren<SelfManager_Stand>();
        m_LeftManager = GetComponentInChildren<LeftManager_Stand>();
        m_RightManager = GetComponentInChildren<RightManager_Stand>();

        txt_BottomStakes = transform.Find("Main/txt_BottomStakes").GetComponent<Text>();
        txt_TopStakes = transform.Find("Main/txt_TopStakes").GetComponent<Text>();
        btn_Back = transform.Find("Main/btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("2.Main");
        });
        txt_BottomStakes.text = Models.GameModel.BottomStakes.ToString();
        txt_TopStakes.text = Models.GameModel.TopStakes.ToString();
        m_LastPlayerStakesCount = Models.GameModel.BottomStakes;

    }
    private void FixedUpdate()
    {
        if (m_IsStartStakes)
        {
            if(m_IsNextPlayerCanStakes)
            {
                //自身玩家下注
                if (m_CurrentStakesIndex%3==0)
                {
                    if(m_SelfManager.m_IsGiveUpCard==false)
                    {
                        m_SelfManager.StartStakes();
                        m_IsNextPlayerCanStakes = false;
                    }

                }
                //左边玩家下注
                if (m_CurrentStakesIndex%3==1)
                {
                    if (m_LeftManager.m_IsGiveUpCard == false)
                    {
                        m_LeftManager.StartStakes();
                        m_IsNextPlayerCanStakes = false;
                    }
                }
                //右边玩家下注
                if (m_CurrentStakesIndex % 3 == 2)
                {
                    if (m_RightManager.m_IsGiveUpCard == false)
                    {
                        m_RightManager.StartStakes();
                        m_IsNextPlayerCanStakes = false;
                    }
                }
                m_CurrentStakesIndex++;
            }
        }
    }
    //判断自身玩家是否胜利
    public bool IsSelfWin()
    {
        if(m_LeftManager.m_IsGiveUpCard&&m_RightManager.m_IsGiveUpCard)
        {
            return true;
        }
        return false;
    }
    //判断左边玩家是否胜利
    public bool IsLeftWin()
    {
        if (m_SelfManager.m_IsGiveUpCard && m_RightManager.m_IsGiveUpCard)
        {
            return true;
        }
        return false;
    }

    //判断右边玩家是否胜利
    public bool IsRightWin()
    {
        if (m_LeftManager.m_IsGiveUpCard && m_SelfManager.m_IsGiveUpCard)
        {
            return true;
        }
        return false;
    }
    //自身玩家胜利
    public void SelfWin()
    {
        EventCenter.Broadcast(EventDefine.GameOver, -m_LeftManager.m_StakesSum,
            m_SelfManager.m_StakesSum, -m_RightManager.m_StakesSum);
    }
    //左边玩家胜利
    public void LeftWin()
    {
        EventCenter.Broadcast(EventDefine.GameOver, m_LeftManager.m_StakesSum,
        -m_SelfManager.m_StakesSum, -m_RightManager.m_StakesSum);
    }
    //右边玩家胜利
    public void RightWin()
    {
        EventCenter.Broadcast(EventDefine.GameOver, -m_LeftManager.m_StakesSum,
        -m_SelfManager.m_StakesSum, m_RightManager.m_StakesSum);
    }
    //右边玩家比牌
    public void RightPlayerCompare()
    {
        if(m_SelfManager.m_IsGiveUpCard)
        {
            //和左边玩家比牌
            EventCenter.Broadcast(EventDefine.VSAI,(BaseManager_Stand) m_RightManager, (BaseManager_Stand)m_LeftManager);
        }
        else
        {
            //和Self玩家比牌
            EventCenter.Broadcast(EventDefine.VSWithSelf, (BaseManager_Stand)m_RightManager, (BaseManager_Stand)m_SelfManager,"右边玩家","我");
        }
    }
    //左边玩家比牌
    public void LeftPlayerCompare()
    {
        if (m_SelfManager.m_IsGiveUpCard)
        {
            //和右边玩家比牌
            EventCenter.Broadcast(EventDefine.VSAI, (BaseManager_Stand)m_LeftManager, (BaseManager_Stand)m_RightManager);
        }
        else
        {
            //和Self玩家比牌
            EventCenter.Broadcast(EventDefine.VSWithSelf, (BaseManager_Stand)m_LeftManager, (BaseManager_Stand)m_SelfManager, "左边玩家", "我");
        }
    }
    //自身与左边玩家比牌
    public void SelfCompareLeft()
    {
        EventCenter.Broadcast(EventDefine.VSWithSelf, (BaseManager_Stand)m_SelfManager, (BaseManager_Stand)m_LeftManager, "我", "左边玩家");

    }
    //自身与右边玩家比牌
    public void SelfCompareRight()
    {
        EventCenter.Broadcast(EventDefine.VSWithSelf, (BaseManager_Stand)m_SelfManager, (BaseManager_Stand)m_RightManager, "我", "右边玩家");

    }
    //下注
    public int Stakes(int count)
    {
        m_LastPlayerStakesCount += count;
        if(m_LastPlayerStakesCount>Models.GameModel.TopStakes)
        {
            m_LastPlayerStakesCount = Models.GameModel.TopStakes;
        }
        return m_LastPlayerStakesCount;
    }
    //选择庄家
    public void ChooseBanker()
    {
        m_LeftManager.StartChooseBanker();
        m_RightManager.StartChooseBanker();

        int ran = Random.Range(0, 3);
        switch(ran)
        {
            case 0://自身成为庄家
                m_SelfManager.BecomeBanker();
                m_CurrentDealCardIndex = 0;
                m_CurrentStakesIndex = 1;
                break;
            case 1://左边成为庄家
                m_LeftManager.BecomeBanker();
                m_CurrentDealCardIndex = 1;
                m_CurrentStakesIndex = 2;
                break;
            case 2://右边成为庄家
                m_RightManager.BecomeBanker();
                m_CurrentDealCardIndex = 2;
                m_CurrentStakesIndex = 0;
                break;
            default:
                break;
        }
        //发牌
        EventCenter.Broadcast(EventDefine.Hint, "开始发牌");
        //发牌TODO
        StartCoroutine(DealCard());
    }
    private IEnumerator DealCard()
    {
        //1.初始化牌
        if(m_CardList.Count==0||m_CardList==null||m_CardList.Count<9)
        {
            InitCard();
            //2.洗牌
            ClearCard();
        }

        for(int i=0;i<9;i++)
        {
            m_AudioSource.Play();
            if (m_CurrentDealCardIndex%3==0)
            {
                //自身发牌
                m_SelfManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(15,286,0));
                m_CardList.RemoveAt(m_DealCardIndex);
                
            }
            //3.发牌
            else if (m_CurrentDealCardIndex % 3 == 1)
            {
                //左边发牌
                m_LeftManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(555,-27,0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            else if (m_CurrentDealCardIndex % 3 == 2)
            {
                //右边发牌
                m_RightManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(-443,-41,0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            yield return new WaitForSeconds(m_DealCardDurationTime);
            m_DealCardIndex++;
            m_CurrentDealCardIndex++;
        }
        //发牌结束
        m_RightManager.DealCardFinished();
        m_SelfManager.DealCardFinished();
        m_LeftManager.DealCardFinished();
        m_IsStartStakes = true;
    }
    //初始化牌
    private void InitCard()
    {
        for(int weight = 2; weight <= 14; weight++)
        {
            for(int color=0;color<=3;color++)
            {
                Card card = new Card(weight, color);
                m_CardList.Add(card);
            }
        }
    }
    //洗牌
    private void ClearCard()
    {
        for(int i=0;i<m_CardList.Count;i++)
        {
            int ran = Random.Range(0, m_CardList.Count);
            Card temp;
            temp = m_CardList[i];
            m_CardList[i] = m_CardList[ran];
            m_CardList[ran] = temp;
        }
    }
}
