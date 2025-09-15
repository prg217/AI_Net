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
        if (Input.GetMouseButtonDown(0)) //상호작용 키
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
        //바닥을 보고 휘두르면 바닥이 인식되어서 잘 안됨
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
                    // 자신의 공격은 반응X
                    return;
                }

                Debug.Log("공격받음");
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
