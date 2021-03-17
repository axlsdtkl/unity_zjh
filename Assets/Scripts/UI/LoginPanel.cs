using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

    private InputField input_UserName;
    private InputField input_Password;
    private Button btn_Login;
    private Button btn_Register;

    private void Awake()
    {
        //从注册页面过来的监听
        EventCenter.AddListener(EventDefine.ShowLoginPanel, Show);
        Init();
    }
    private void Init()
    {
        //用户名与密码
        input_UserName = transform.Find("input_UserName").GetComponent<InputField>();
        input_Password = transform.Find("input_Password").GetComponent<InputField>();
        btn_Login = transform.Find("btn_Login").GetComponent<Button>();
        //点击登录
        btn_Login.onClick.AddListener(OnLoginButtonClick);
        btn_Register = transform.Find("btn_Register").GetComponent<Button>();
        //点击注册
        btn_Register.onClick.AddListener(OnRegisterButtonClick);
    }
    private void OnDestroy()
    {
        //移除监听
        EventCenter.RemoveListener(EventDefine.ShowLoginPanel, Show);
    }
    //注册按钮点击
    private void OnRegisterButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowRegisterPanel);
    }
    //登录按钮点击
    private void OnLoginButtonClick()
    {
        if (input_UserName.text == null || input_UserName.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入用户名");
            //Debug.Log("请输入用户名");
            return;
        }
        if (input_Password.text == null || input_Password.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint, "请输入密码");
            //Debug.Log("请输入密码");
            return;
        }
        //向服务器发送登录的请求
        AccountDto dto = new AccountDto(input_UserName.text, input_Password.text);
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.Login_CREQ, dto);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
 