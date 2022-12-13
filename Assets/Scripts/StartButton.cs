using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviourPun
{
    private bool isStart = false;
    public List<GameObject> playerList; //�ڽ� �ݶ��̴��� ���� �÷��̾ ���
    private int chaserCount = 0;
    private int random = 0;

    private int AICount = 15; //AI����
    private float randomX = 0;
    private float randomZ = 0;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    public int fugitive = 0;

    public void GameStart()
    {
        Debug.Log("GameStart!");
        //��ư�� ���� ��� ȣ��Ʈ �������� �÷��̾�� ���� �������ֱ�
        //�±װ� Player�� ����� ã�Ƽ�...�ұ�?

        //�÷��̾���� ������ ���� ��ġ�ϰ� �ϱ�
        //ȣ��Ʈ �������� AI ������ ���� ��ġ
        if (PhotonNetwork.IsMasterClient) //ȣ��Ʈ �� ���
        {
            isStart = true; //�÷��̾� ��� �׸��ϰ�
            
            //�� ������ �ľ� �� �� ���̳Ŀ� ���� �߰��� ���� �޶����� �ϱ�
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

            //�߰��� ���� �� ��ġ ������ �����ؼ� �ڽ��� �ƴ� �ٸ� ������� �� ������ �� �ȵǴ� �� ���� ���� �ʿ�
            //�ڽſ��Դ� ���� ������ ���� �Ǵµ�, ��� Ŭ���̾�Ʈ���� ������ �ȵ�

            //�߰��� �� ��ŭ �߰��� �̱� ���� ������ ���� �� ���� �ǵ���...
            for (int i = 0; i < chaserCount; i++)
            {
                random = Random.Range(0, playerList.Count + 1);

                if (playerList[random].GetComponent<PlayerMovement>().isChaser == false)
                {
                    playerList[random].GetComponent<PlayerMovement>().SetChaser();
                }
                else
                {
                    //������ ���Դ� ���ڴ� �������� �ؾ��ϱ� ���� �߰��ڰ� �ƴ� ��쿡�� �߰��ڷ� ������ �� �ְ� ���ְ�
                    //�߰����� ��� �ٽ� �̰�
                    i--;
                }
            }

            GameObject.Find("Canvas").transform.Find("StartButton").gameObject.SetActive(false); //��ŸƮ ��ư ��Ȱ��ȭ
            
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].GetComponent<PlayerMovement>().SetPlayerPosition();
                playerList[i].GetComponent<PlayerMovement>().UIOpen();
                playerList[i].GetComponent<PlayerMovement>().StartSet();
            }
            //�� ���� ��ŸƮ ��ư�� �����ϰ� �÷��̾�� �� ���� �ȿ� ���� �����ǰ� �ϰ�
            
            for (int i = 0; i < AICount; i++)
            {
                randomX = Random.Range(-20, 20);
                randomZ = Random.Range(-20, 20);
                random = Random.Range(0, 181); //�����̼� y��

                GameObject AI = PhotonNetwork.Instantiate("AI", new Vector3(randomX, 0, randomZ), Quaternion.Euler(0, random, 0));
            }
            //AI�鵵 �����ϰ�...
            
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
            playerList[i].GetComponent<PlayerMovement>().ChaserWin(); //�ٵ� �÷��̾ �̹� �׾ �����Ǿ �ߵ� �� �� ���ɼ���
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
