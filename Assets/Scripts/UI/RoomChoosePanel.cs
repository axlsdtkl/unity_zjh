using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum GameType
{
    Net,
    StandAlone,
}

public class RoomChoosePanel : MonoBehaviour
{
    private Button btn_EnterRoom1;
    private Button btn_EnterRoom2;
    private Button btn_EnterRoom3;
    private Button btn_Close;
    private GameType m_GameType;


    private void Awake()
    {
        EventCenter.AddListener<GameType>(EventDefine.ShowRoomChoosePanel, Show);
        Init();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameType>(EventDefine.ShowRoomChoosePanel, Show);
    }
    private void Init()
    {
        btn_EnterRoom1 = transform.Find("Room_1/btn_EnterRoom").GetComponent<Button>();
        btn_EnterRoom1.onClick.AddListener(delegate { EnterRoom(10, 100); });
        btn_EnterRoom2 = transform.Find("Room_2/btn_EnterRoom").GetComponent<Button>();
        btn_EnterRoom2.onClick.AddListener(delegate { EnterRoom(20, 200); });
        btn_EnterRoom3 = transform.Find("Room_3/btn_EnterRoom").GetComponent<Button>();
        btn_EnterRoom3.onClick.AddListener(delegate { EnterRoom(50, 500); });
        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(() =>
        {
            transform.DOScale(Vector3.zero, 0.3f);
        });
    }
    private void Show(GameType gameType)
    {
        m_GameType = gameType;
        transform.DOScale(Vector3.one, 0.3f);
    }
    //进入房间
    private void EnterRoom(int botoomStakes,int topStakes)
    {
        Models.GameModel.BottomStakes = botoomStakes;
        Models.GameModel.TopStakes = topStakes;

        switch(m_GameType)
        {
            case GameType.Net:
                //进入联网TODO
                break;
            case GameType.StandAlone:
                SceneManager.LoadScene("3.StandAlone");
                break;
            default:
                break;
        }
    }
}
