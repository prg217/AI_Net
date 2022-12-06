using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

using Random = UnityEngine.Random;
//유니티 랜덤 시스템 랜덤이 겹쳐서 확실하게 해줌

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public bool isChaser = false; //추격자 여부

    Animator animator;
    //CharacterController characterController;
    private Transform tr;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed = 0f;
    public bool isRun;
    public bool isWalk;

    public bool toggleCameraRotation;
    public float smoothness = 10f;

    public TextMesh playerName;
    public PhotonView pv;
    private Vector3 currPos;
    private Quaternion currRot;

    public int hp = 100;
    private bool isDie = false;

    private float h = 0f;
    private float v = 0f;

    // Start is called before the first frame update
    void Start()
    {
        int hp = 100;

        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;

        animator = this.GetComponent<Animator>();
        if (pv.IsMine)
        {
            GameObject.Find("Camera").GetComponent<CameraMovement>().objectTofollow = tr.Find("FollowCam").gameObject.transform;
            GameObject.Find("Camera").GetComponent<CameraMovement>().playerTr = gameObject.transform;
        }
        //characterController = this.GetComponent<CharacterController>();

        if (PhotonNetwork.IsMasterClient) //호스트 일 경우
        {
            GameObject.Find("Canvas").transform.Find("StartButton").gameObject.SetActive(true);
            //게임 스타트 버튼이 보이게 한다.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                toggleCameraRotation = true; //�ѷ����� Ȱ��ȭ
            }
            else
            {
                toggleCameraRotation = false; // �ѷ����� ��Ȱ��ȭ
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRun = true;
                animator.SetBool("isRun", true);
                finalSpeed = 8f;
            }
            else
            {
                isRun = false;
                animator.SetBool("isRun", false);
                finalSpeed = 5f;
            }
            InputMovement();
        }
        else if (!pv.IsMine)
        {
            if (tr.position != currPos)
            {
                animator.SetFloat("Speed", 1.0f);
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                //position이 반영이 안됨
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
            }

            if (tr.rotation != currRot)
            {
                tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }

    }

    void InputMovement()
    {/*
        finalSpeed = (isRun) ? runSpeed : speed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        characterController.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);

        //�޸��� �ִ�
        float percent = ((isRun) ? 1 : 0.5f) * moveDirection.magnitude;
        animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);*/

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            tr.Translate(moveDir.normalized * Time.deltaTime * speed);
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }

        tr.Rotate(Vector3.up * Time.deltaTime * speed * Input.GetAxis("Mouse X"));
        //transform.position += new Vector3(h, 0, v) * speed * Time.deltaTime; //대충 한거라 제대로 동작X 수정바람
    }

    public void SetPlayerName(string name)
    {
        this.name = name;
        GetComponent<PlayerMovement>().playerName.text = this.name;

        //box.name = name;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());

        }
    }

    public void Damage()
    {
        Debug.Log("Damage");
        photonView.RPC("DamageRPC", RpcTarget.All);
    }

    [PunRPC]
    public void DamageRPC()
    {
        Debug.Log("DamageRPC()");

        hp -= 100;

        if (hp <= 0)
        {
            isDie = true; //죽는 것도 펀알피시로?
            Destroy(gameObject);//임시로 일단 삭제해줌 나중에 투명상태가 되어서 이동할 수 있게 하기
        }
    }

    public void AttackAni()
    {
        animator.SetTrigger("Attack");
    }

    public void SetChaser()
    {
        photonView.RPC("SetChaserRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SetChaserRPC()
    {
        isChaser = true;
        transform.Find("AttackBox").gameObject.SetActive(true); //추격자면 공격 박스 켜줌
    }

    public void SetPlayerPosition()
    {
        photonView.RPC("SetPlayerPositionRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SetPlayerPositionRPC()
    {
        if (pv.IsMine)
        {
            float randomX = Random.Range(-20, 20);
            float randomZ = Random.Range(-20, 20);
            //랜덤 x, z좌표 넣어주기
            //지형이 울퉁불퉁할 경우에는 스폰 지점 좌표...설정해서 해주기
            tr.position = new Vector3(randomX, tr.position.y, randomZ);
        }
    }

}
