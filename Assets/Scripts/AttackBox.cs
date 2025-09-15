using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackBox : MonoBehaviourPun
{
    public GameObject player;
    private bool isAttack = false;
    private bool isOther = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerScript>().pv.IsMine)
        {
            Action();
        }
    }

    private void Action()
    {
        if (Input.GetMouseButtonDown(0)) //��ȣ�ۿ� Ű
        {
            player.GetComponent<PlayerScript>().AttackAni();

            if (isOther == true)
            {
                isAttack = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�ٴ��� ���� �ֵθ��� �ٴ��� �νĵǾ �� �ȵ�
        if (other.gameObject.tag == "Player")
        {
            isOther = true;
        }
        else if (other.gameObject.tag == "AI")
        {
            isOther = true;
        }

        if (isAttack)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject == player)
                {
                    isAttack = false;
                    // �ڽ��� ������ ����X
                    return;
                }

                Debug.Log("���ݹ���");
                other.GetComponent<PlayerScript>().Damage();

                //photonView.RPC("AttackRPC", RpcTarget.All, other);
            }
            else if (other.gameObject.tag == "AI")
            {
                Debug.Log("AI");
                player.GetComponent<PlayerScript>().AIDamage();
                Destroy(other.gameObject);
            }
            isAttack = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isOther = false;
    }
}
