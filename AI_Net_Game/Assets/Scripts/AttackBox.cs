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
        if (Input.GetKeyDown(KeyCode.Space)) //상호작용 키
        {
            player.GetComponent<PlayerMovement>().AttackAni();
            Debug.Log("스페이스바");
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
                    // 자신의 공격은 반응X
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
