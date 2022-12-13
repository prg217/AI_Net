using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviourPun
{
    private bool isStart = false;
    public List<GameObject> playerList; //박스 콜라이더에 들어온 플레이어를 등록
    private int chaserCount = 0;
    private int random = 0;

    private int AICount = 15; //AI개수
    private float randomX = 0;
    private float randomZ = 0;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    public int fugitive = 0;

    public void GameStart()
    {
        Debug.Log("GameStart!");
        //버튼을 누를 경우 호스트 기준으로 플레이어들 역할 배정해주기
        //태그가 Player인 얘들을 찾아서...할까?

        //플레이어들을 랜덤한 곳에 배치하게 하기
        //호스트 기준으로 AI 랜덤한 곳에 배치
        if (PhotonNetwork.IsMasterClient) //호스트 일 경우
        {
            isStart = true; //플레이어 등록 그만하고
            
            //몇 명인지 파악 후 몇 명이냐에 따라 추격자 숫자 달라지게 하기
            if (playerList.Count <= 4)
            {
                chaserCount = 1;
            }
            else if (playerList.Count <= 7)
            {
                chaserCount = 2;
            }
            else if (playerList.Count <= 10)
            {
                chaserCount = 3;
            }

            //추격자 설정 및 위치 설정에 관련해서 자신이 아닌 다른 사람에게 값 전달이 잘 안되는 것 같음 수정 필요
            //자신에게는 변수 변경이 적용 되는데, 상대 클라이언트에는 적용이 안됨

            //추격자 수 만큼 추격자 뽑기 랜덤 돌려서 랜덤 몇 명에게 되도록...
            for (int i = 0; i < chaserCount; i++)
            {
                random = Random.Range(0, playerList.Count + 1);

                if (playerList[random].GetComponent<PlayerMovement>().isChaser == false)
                {
                    playerList[random].GetComponent<PlayerMovement>().SetChaser();
                }
                else
                {
                    //지난번 나왔던 숫자는 못나오게 해야하기 위해 추격자가 아닌 경우에만 추격자로 변경할 수 있게 해주고
                    //추격자인 경우 다시 뽑게
                    i--;
                }
            }

            GameObject.Find("Canvas").transform.Find("StartButton").gameObject.SetActive(false); //스타트 버튼 비활성화
            
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].GetComponent<PlayerMovement>().SetPlayerPosition();
                playerList[i].GetComponent<PlayerMovement>().UIOpen();
                playerList[i].GetComponent<PlayerMovement>().StartSet();
            }
            //이 이후 스타트 버튼을 삭제하고 플레이어들 맵 범위 안에 랜덤 스폰되게 하고
            
            for (int i = 0; i < AICount; i++)
            {
                randomX = Random.Range(-20, 20);
                randomZ = Random.Range(-20, 20);
                random = Random.Range(0, 181); //로테이션 y값

                GameObject AI = PhotonNetwork.Instantiate("AI", new Vector3(randomX, 0, randomZ), Quaternion.Euler(0, random, 0));
            }
            //AI들도 스폰하고...
            
        }
    }

    public void SetFugitive()
    {
        fugitive = playerList.Count - chaserCount;
    }


    public void FugitiveWin()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().FugitiveWin();
        }          
    }

    public void ChaserClear()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().ChaserWin(); //근데 플레이어가 이미 죽어서 삭제되어서 발동 안 할 가능성이
        }
    }

    public void EscapeText()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().EscapeText();
            //playerList[i].GetComponent<PlayerMovement>().Escape();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isStart == false)
        {
            if (other.gameObject.tag == "Player")
            {
                playerList.Add(other.gameObject);
            }
        }
    }
}
