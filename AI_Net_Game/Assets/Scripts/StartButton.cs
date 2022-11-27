using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartButton : MonoBehaviourPun
{
    private bool isStart = false;
    public List<GameObject> playerList; //�ڽ� �ݶ��̴��� ���� �÷��̾ ���
    private int chaserCount = 0;
    private int random = 0;
    private int randomX = 0;
    private int randomZ = 0;

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

            //�߰��� �� ��ŭ �߰��� �̱� ���� ������ ���� �� ���� �ǵ���...
            for (int i = 0; i < chaserCount; i++)
            {
                random = Random.Range(0, playerList.Count + 1);

                if (playerList[random].GetComponent<PlayerMovement>().isChaser == false)
                {
                    playerList[random].GetComponent<PlayerMovement>().isChaser = true;
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
                randomX = Random.Range(-20, 20);
                randomZ = Random.Range(-20, 20);
                //���� x, z��ǥ �־��ֱ�
                //������ ���������� ��쿡�� ���� ���� ��ǥ...�����ؼ� ���ֱ�
                playerList[i].transform.position = new Vector3(randomX, playerList[i].transform.position.y, randomZ);
            }
            //�� ���� ��ŸƮ ��ư�� �����ϰ� �÷��̾�� �� ���� �ȿ� ���� �����ǰ� �ϰ�
            
            //AI�鵵 �����ϰ�...
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
