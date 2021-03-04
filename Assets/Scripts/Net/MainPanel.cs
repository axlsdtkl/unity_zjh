using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {
    private Text txt_UserName;
    private Text txt_CoinCount;
    private Image headIcon;
    private Button btn_Rank;
    private Button btn_Bank;
    private Button btn_Stand;
    private Button btn_Online;
    private Button btn_Logout;

    private void Awake()
    {
        EventCenter.AddListener<int>(EventDefine.UpdateCoinCount, UpdateCoinCount);
        Init();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.UpdateCoinCount, UpdateCoinCount);
    }
    private void Init()
    {
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("txt_CoinCount").GetComponent<Text>();
        headIcon = transform.Find("Mask/headIcon").GetComponent<Image>();
        btn_Rank = transform.Find("btn_Rank").GetComponent<Button>();
        btn_Logout = transform.Find("btn_Logout").GetComponent<Button>();
        btn_Logout.onClick.AddListener(OnLogoutButtonClick);

        btn_Rank.onClick.AddListener(()=>
        {
            //向服务器发送获取排行榜的请求
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.GetRankList_CREQ, null);
            //广播显示排行榜界面的事件码
            EventCenter.Broadcast(EventDefine.ShowRankListPanel); 
        });
        btn_Bank = transform.Find("btn_Bank").GetComponent<Button>();
        btn_Bank.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.ShowRechargePanel);
        });

        btn_Stand = transform.Find("btn_Stand").GetComponent<Button>();
        btn_Stand.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.ShowRoomChoosePanel,GameType.StandAlone);
        });

        btn_Online = transform.Find("btn_Online").GetComponent<Button>();
        btn_Online.onClick.AddListener(() =>
        {
            EventCenter.Broadcast(EventDefine.ShowRoomChoosePanel, GameType.Net);
        });


        txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
        Debug.Log(txt_CoinCount.text);
        txt_UserName.text = Models.GameModel.userDto.UserName;
        headIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
    }
    //金币的更新
    private void UpdateCoinCount(int value)
    {
        txt_CoinCount.text = value.ToString();
    }
    private void OnLogoutButtonClick()
    {
        //退出按钮点击
        //TODO
        //退出UI过程
        SceneManager.LoadScene("1.Start");
        //EventCenter.Broadcast(EventDefine.Logout);
    }
}
