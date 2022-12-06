using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackBox : MonoBehaviourPun
{
    public GameObject player;
    private bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMovement>().pv.IsMine)
        {
            Action();
        }
    }

    private void Action()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //��ȣ�ۿ� Ű
        {
            player.GetComponent<PlayerMovement>().AttackAni();
            Debug.Log("�����̽���");
            isAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttack)
        {
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject == player)
                {
                    // �ڽ��� ������ ����X
                    return;
                }

                Debug.Log("!");
                other.GetComponent<PlayerMovement>().Damage();
                //photonView.RPC("AttackRPC", RpcTarget.All, other);
            }
            isAttack = false;
        }

    }
}
