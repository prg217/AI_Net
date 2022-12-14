using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;
//유니티 랜덤 시스템 랜덤이 겹쳐서 확실하게 해줌

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public bool isChaser = false; //추격자 여부

    public Animator animator;
    //CharacterController characterController;
    private Transform tr;
    private Rigidbody rigid;

    public float speed = 5f;
    public bool isRun = false;
    private float jumpPower = 3.5f;
    private bool isJump = false;

    public bool toggleCameraRotation;
    public float smoothness = 10f;

    public TextMesh playerName;
    public PhotonView pv;
    private Vector3 currPos;
    private Quaternion currRot;

    public int hp = 100;

    private float h = 0f;
    private float v = 0f;

    public int missionClearCount = 0;

    private bool isClear = false;

    public GameObject doorP1;
    public GameObject doorP2;
    public GameObject doorP3;

    private Vector3 posV;
    private bool isWalk = false;

    // Start is called before the first frame update
    void Start()
    {
        int hp = 100;

        tr = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();

        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;

        //animator = this.GetComponent<Animator>();
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

            photonView.RPC("RunRPC", RpcTarget.All);

            InputMovement();

            if (missionClearCount >= 5 && isClear == false)
            {
                isClear = true;

                GameObject.Find("StartScript").gameObject.GetComponent<StartButton>().EscapeText();
                Escape();
            }
        }
        else if (!pv.IsMine)
        {
            if (tr.position != currPos && isRun == false && isWalk == true)
            {
                /*
                posV = currPos + new Vector3(1, 1, 1);
                if ((tr.position.x <= posV.x && tr.position.y <= posV.y && tr.position.z <= posV.z) || (tr.position.x >= posV.x && tr.position.y >= posV.y && tr.position.z >= posV.z))
                {

                }
                else
                {
                    animator.SetBool("isWalk", true);
                }*/
                animator.SetBool("isWalk", true);
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            else if (tr.position != currPos && isRun == true)
            {
                animator.SetBool("isRun", true);
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            else
            {
                animator.SetBool("isRun", false);
                animator.SetBool("isWalk", false);
            }


            if (tr.rotation != currRot)
            {
                tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }

    }

    [PunRPC]
    void RunRPC()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 8f;
            isRun = true;
            animator.SetBool("isRun", true);
        }
        else
        {
            speed = 5f;
            isRun = false;
            animator.SetBool("isRun", false);
        }
    }

    [PunRPC]
    void WalkRPC()
    {
        isWalk = true;
    }

    [PunRPC]
    void WalkRPC2()
    {
        isWalk = false;
    }

    void InputMovement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            photonView.RPC("WalkRPC", RpcTarget.All);

            tr.Translate(moveDir.normalized * Time.deltaTime * speed);
            animator.SetBool("isWalk", true);
        }
        else
        {
            photonView.RPC("WalkRPC2", RpcTarget.All);

            animator.SetBool("isWalk", false);
        }

        tr.Rotate(Vector3.up * Time.deltaTime * speed * Input.GetAxis("Mouse X"));
        //transform.position += new Vector3(h, 0, v) * speed * Time.deltaTime; //대충 한거라 제대로 동작X 수정바람

        if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
            animator.SetBool("isJump", true);

            if (gameObject.GetComponent<Mission>().jumpMission == true)
            {
                gameObject.GetComponent<Mission>().jumpCount++;
            }
        }
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
            //stream.SendNext(animator);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            //animator = (Animator)stream.ReceiveNext();
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
            if (isChaser == false)
            {
                FugitiveCount();
            }
            photonView.RPC("DeadRPC", RpcTarget.All);

        }
    }

    public void AIDamage()
    {
        photonView.RPC("AIDamageRPC", RpcTarget.All);
    }
    
    [PunRPC]
    public void AIDamageRPC()
    {
        hp -= 10;

        if (hp <= 0)
        {
            photonView.RPC("DeadRPC", RpcTarget.All);
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

    [PunRPC]
    public void DeadRPC()
    {
        transform.Find("Body").gameObject.SetActive(false);
        gameObject.GetComponent<Mission>().enabled = false;
    }

    public void UIOpen()
    {
        photonView.RPC("UIOpenRPC", RpcTarget.All);
    }

    [PunRPC]
    public void UIOpenRPC()
    {
        if (pv.IsMine)
        {
            if (isChaser == true)
            {
                GameObject.Find("Canvas").transform.Find("ChaserText").gameObject.SetActive(true);
            }
            else if (isChaser == false)
            {
                Debug.Log("도망자");
                GameObject.Find("Canvas").transform.Find("FugitiveText").gameObject.SetActive(true);
                //그리고 랜덤하게 플레이어에게 미션이 부여되게 함
                //미션은 7개 미션 카운트만 RPC로 하고 살아있는 모든 플레이어가 미션을 완료하면 탈출구 열림
                //도망자가 한 명이라도 나가면 도망자 승

                //미션은 미션 랜덤하게 부여해주고 UI랜덤...해당되는거 틀어주기
                //달리기 상태 누적 30초 동안 하기 라던가
                //점프 10번 하기 라던가
                //지정된 위치 가기라던가(가고 나서 다른 곳으로 또 가고

                //미션 완료할때마다 파티클로 폭죽 터트리기
                gameObject.GetComponent<Mission>().GetMission();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            gameObject.GetComponent<Mission>().tagetCount++;
            other.gameObject.SetActive(false);

            if (gameObject.GetComponent<Mission>().tagetCount >= 4)
            {
                gameObject.GetComponent<Mission>().TargetMissionClear();
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("isJump", false);
        }
    }

    public void MissionCountUp()
    {
        photonView.RPC("MissionCountUpRPC", RpcTarget.All);
    }

    [PunRPC]
    public void MissionCountUpRPC()
    {
        missionClearCount++;
    }

    public void Escape()
    {
        photonView.RPC("EscapeRPC", RpcTarget.All);
    }

    [PunRPC]
    public void EscapeRPC()
    {
        doorP1 = GameObject.Find("StartScript").gameObject.GetComponent<StartButton>().door1;
        doorP2 = GameObject.Find("StartScript").gameObject.GetComponent<StartButton>().door2;
        doorP3 = GameObject.Find("StartScript").gameObject.GetComponent<StartButton>().door3;

        doorP1.SetActive(false);
        doorP2.SetActive(false);
        doorP3.SetActive(true);
    }

    public void EscapeText()
    {
        photonView.RPC("EscapeTextRPC", RpcTarget.All);
    }

    [PunRPC]
    public void EscapeTextRPC()
    {
        GameObject.Find("Canvas").transform.Find("EscapeText").gameObject.SetActive(true);
    }

    public void FugitiveWin()
    {
        photonView.RPC("FugitiveWinRPC", RpcTarget.All);
    }

    [PunRPC]
    public void FugitiveWinRPC()
    {
        SceneManager.LoadScene("FugitiveWin");
    }

    public void ChaserWin()
    {
        photonView.RPC("ChaserWinRPC", RpcTarget.All);
    }

    [PunRPC]
    public void ChaserWinRPC()
    {
        SceneManager.LoadScene("ChaserWin");
    }

    public void StartSet()
    {
        photonView.RPC("StartSetRPC", RpcTarget.All);
    }

    [PunRPC]
    public void StartSetRPC()
    {
        GameObject.Find("StartScript").GetComponent<StartButton>().SetFugitive(); //도망자 숫자
        GameObject.Find("StartScript").GetComponent<BoxCollider>().enabled = false;
    }

    public void FugitiveCount()
    {
        photonView.RPC("FugitiveCountRPC", RpcTarget.All);
    }

    [PunRPC]
    public void FugitiveCountRPC()
    {
        GameObject.Find("StartScript").GetComponent<StartButton>().fugitive--;
        Debug.Log("도망자 카운트");
        if (GameObject.Find("StartScript").GetComponent<StartButton>().fugitive <= 0)
        {
            GameObject.Find("StartScript").GetComponent<StartButton>().ChaserClear();
        }
    }
}
