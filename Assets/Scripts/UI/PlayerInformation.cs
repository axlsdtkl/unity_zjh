using DG.Tweening;
using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour {

    private Button btn_Close;
    private Text txt_UserName;
    private Text txt_CoinCount;
    private InputField input_Password;
    private InputField input_PasswordAgain;
    private Button btn_ModifyPwd;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowPlayerInformationPanel, Show);
        Init();
    }
    private void Init()
    {
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("txt_CoinCount").GetComponent<Text>();
        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        input_Password = transform.Find("input_Pwd").GetComponent<InputField>();
        input_PasswordAgain = transform.Find("input_PwdAgain").GetComponent<InputField>();
        btn_ModifyPwd = transform.Find("Button_ModifyPwd").GetComponent<Button>();

        btn_Close.onClick.AddListener(() =>
        {
            transform.DOScale(Vector3.zero, 0.3f);
        });
        txt_UserName.text = Models.GameModel.userDto.UserName;
        txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
        btn_ModifyPwd.onClick.AddListener(OnModifyPwdButton);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowPlayerInformationPanel, Show);
    }
    private void Show()
    {
        transform.DOScale(Vector3.one, 0.3f);

    }
    private void OnModifyPwdButton()
    {
        if (input_Password.text == null || input_Password.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入密码");
            //Debug.Log("请输入用户名");
            return;
        }
        if (input_PasswordAgain.text == null || input_PasswordAgain.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请再次输入密码");
            //Debug.Log("请输入密码");
            return;
        }
        //Debug.Log(input_Password.text);
        //Debug.Log(input_PasswordAgain.text);
        if (input_Password.text!=input_PasswordAgain.text)
        {
            EventCenter.Broadcast(EventDefine.Hint, "两次密码输入不一致");
            return;
        }
        AccountDto dto = new AccountDto(Models.GameModel.userDto.UserName, input_Password.text);
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.ModifyPwd_CREQ, dto);
    }
}
