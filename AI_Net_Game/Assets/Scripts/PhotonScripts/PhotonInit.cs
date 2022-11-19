using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    // 싱글턴 패턴을 저굥ㅇ 해야 함
    public static PhotonInit instance;

    public InputField playerInput;
    public Button chattingBtn;
    bool isGameStart = false;
    bool isLoggIn = false;
    bool isReady = false;
    string playerName = "";
    string connectionState = "";
    public string chatMessage ;
    Text chatText;
    ScrollRect scroll_rect = null;
    PhotonView pv;

    Text connectionInfoText;

    [Header("LobbyCanvas")] public GameObject LobbyCanvas;
    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public Toggle PwToggle;
    public GameObject PwPanel;
    public GameObject PwErrorLog;
    public GameObject PwConfirmBtn;
    public GameObject PwPanelCloseBtn;
    public InputField PwCheckIF;
    public bool LockState = false;
    public string privateroom;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomBtn;
    public int hashtablecount;

    // 여러개의 방 리스트를 관리하는 변수
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple, roomnumber;

    private void Awake()
    {
        
        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();

        // 이제 2개의 씬에서 로딩이 되기때문에 UI 처리를 해보자
        if(GameObject.Find("ChatText") != null)
            chatText = GameObject.Find("ChatText").GetComponent<Text>();

        if (GameObject.Find("Scroll View") != null)
            scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

        if (GameObject.Find("ConnectionInfoText") != null)
            connectionInfoText = GameObject.Find("ConnectionInfoText").GetComponent<Text>();
        
        connectionState = "마스터 서버에 접속 중...";

        if(connectionInfoText)
            connectionInfoText.text = connectionState;
        //아래의 함수를 사용하여 씬이 전환 되더라도 선언 되었던 인스턴스가 파괴되지 않는다.

        DontDestroyOnLoad(gameObject);
    }

    public static PhotonInit Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                    Debug.Log("no singleton obj");
            }

            return instance;
        }
    }

    

    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    Debug.Log("Joined Lobby");
    //    //PhotonNetwork.CreateRoom("MyRoom");
    //    PhotonNetwork.JoinRandomRoom();
    //    //PhotonNetwork.JoinRoom("MyRoom");
    //}

    public void Connect()
    {

        if (PhotonNetwork.IsConnected && isReady)
        {
            connectionState = "룸에 접속...";
            if (connectionInfoText)
                connectionInfoText.text = connectionState;

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);
            
            PhotonNetwork.JoinLobby();
            //이제 서버에 연결 되면 로비에 조인! 방을 랜덤하게 들어가는거는 하지 않음!
            //대신 방의 현황을 보여주는 패널을 보여준다
            //PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionState = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도중...";
            if (connectionInfoText)
                connectionInfoText.text = connectionState;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToServer");
        isReady = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionState = "No Room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("No Room");
        //방생성 부분도 주석 처리
        //PhotonNetwork.CreateRoom("MyRoom");
    }

    

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        connectionState = "Finish make a room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("Finish make a room");
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("OnCreateRoomFailed:"+returnCode + "-"+message);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        connectionState = "Joined Lobby";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        myList.Clear();

        Debug.Log("OnJoinedLobby:" );

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("Joined Room");
        isLoggIn = true;
        PlayerPrefs.SetInt("LogIn", 1);

        //SceneManager.LoadScene("SampleScene");
        PhotonNetwork.LoadLevel("SampleScene");

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LogIn", 0);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("LogIn") == 1)
            isLoggIn = true;

        if (isGameStart == false && SceneManager.GetActiveScene().name == "SampleScene" && isLoggIn == true)
        {
            isGameStart = true;
            if (GameObject.Find("ChatText") != null)
                chatText = GameObject.Find("ChatText").GetComponent<Text>();

            if (GameObject.Find("Scroll View") != null)
                scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

            //이제 플레이어 인풋 필드를 대체하자
            if (GameObject.Find("InputFieldChat") != null)
            {
                playerInput = GameObject.Find("InputFieldChat").GetComponent<InputField>();
            }

            if (GameObject.Find("ChattingButton") != null)
            {
                chattingBtn = GameObject.Find("ChattingButton").GetComponent<Button>();
                chattingBtn.onClick.AddListener(SetPlayerName);
            }

            StartCoroutine(CreatePlayer());
        }
    }

    IEnumerator CreatePlayer()
    {
        while(!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GameObject tempPlayer = PhotonNetwork.Instantiate("PlayerDagger",
                                    new Vector3(0, 0, 0),
                                    Quaternion.identity,
                                    0);
        tempPlayer.GetComponent<PlayerMovement>().SetPlayerName(playerName);
        pv = GetComponent<PhotonView>();

        yield return null;
    }

    private void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    public void SetPlayerName()
    {
        Debug.Log(playerInput.text + "를 입력 하셨습니다!");

        if(isGameStart == false && isLoggIn == false)
        {
            playerName = playerInput.text;
            playerInput.text = string.Empty;
            Debug.Log("connect 시도!" + isGameStart + ", " + isLoggIn);
            Connect();
            
        }
        else
        {
            chatMessage = playerInput.text;
            playerInput.text = string.Empty;
            //ShowChat(chatMessage);
            pv.RPC("ChatInfo", RpcTarget.All, chatMessage);
        }
        
    }

    public void ShowChat(string chat)
    {
        chatText.text += chat + "\n";
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    [PunRPC]
    public void ChatInfo(string sChat)
    {
        ShowChat(sChat);
    }

    #region 방 생성 및 접속 관련 메서드
    public void CreateRoomBtnOnClick()
    {
        MakeRoomPanel.SetActive(true);
    }

    public void OKBtnOnClick()
    {
        MakeRoomPanel.SetActive(false);
    }


    public void CreateRoom()
    {
         PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text,
                new RoomOptions { MaxPlayers = 100 });
        LobbyPanel.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "마스터 서버에 접속 중...";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        isGameStart = false;
        isLoggIn = false;
        PlayerPrefs.SetInt("LogIn", 0);
    }


    //새로운 방 만들기
    public void CreateNewRoom()
    {
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 80;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : "*" + RoomInput.text,
                roomOptions);
        }

        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text,
                new RoomOptions { MaxPlayers = 100 });
        }

        MakeRoomPanel.SetActive(false);
        //LobbyCanvas.SetActive(false);
        
        
        
    }


    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
       
        if (num == -2)
        {
            --currentPage;
            MyListRenewal();
        }
        else if (num == -1)
        {
            ++currentPage;
            MyListRenewal();
        }

        else if (myList[multiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();
            
        }
       
    }

    public void RoomPw(int number)
    {
        switch (number)
        {
            case 0:
                roomnumber = 0;
                break;
            case 1:
                roomnumber = 1;
                break;
            case 2:
                roomnumber = 2;
                break;
            case 3:
                roomnumber = 3;
                break;

            default:
                break;
        }
    
    
    }

    public void EnterRoomWithPW()
    {
       if ((string)myList[multiple + roomnumber].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[multiple + roomnumber].Name);
            MyListRenewal();
            PwPanel.SetActive(false);
        }

        else
        {
            StartCoroutine("ShowPwWrongMsg");
        }
        
        
    }

    IEnumerator ShowPwWrongMsg()
    {
        if (!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }

    void MyListRenewal()
    {
       
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0)
            ? myList.Count / CellBtn.Length
            : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text =
                (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count)
                ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers
                : "";
        }
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate:" + roomList.Count);
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }

        MyListRenewal();
        
    }
    #endregion

}
