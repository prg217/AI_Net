using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartButton : MonoBehaviourPun
{
    private bool isStart = false;
    public List<GameObject> playerList; //박스 콜라이더에 들어온 플레이어를 등록
    private int chaserCount = 0;
    private int random = 0;
    private int randomX = 0;
    private int randomZ = 0;

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

            //추격자 수 만큼 추격자 뽑기 랜덤 돌려서 랜덤 몇 명에게 되도록...
            for (int i = 0; i < chaserCount; i++)
            {
                random = Random.Range(0, playerList.Count + 1);

                if (playerList[random].GetComponent<PlayerMovement>().isChaser == false)
                {
                    playerList[random].GetComponent<PlayerMovement>().isChaser = true;
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
                randomX = Random.Range(-20, 20);
                randomZ = Random.Range(-20, 20);
                //랜덤 x, z좌표 넣어주기
                //지형이 울퉁불퉁할 경우에는 스폰 지점 좌표...설정해서 해주기
                playerList[i].transform.position = new Vector3(randomX, playerList[i].transform.position.y, randomZ);
            }
            //이 이후 스타트 버튼을 삭제하고 플레이어들 맵 범위 안에 랜덤 스폰되게 하고
            
            //AI들도 스폰하고...
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
