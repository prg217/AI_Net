using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPun
{
    Animator animator;
    CharacterController characterController;
    private Transform tr;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed;
    public bool isRun;

    public bool toggleCameraRotation;
    public float smoothness = 10f;

    public TextMesh playerName;
    private PhotonView pv;

    /*
    �����ʿ��� �߰��ڶ� ������ ������Ʈ�� �ٿ��ָ�...
    */


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;

        animator = this.GetComponent<Animator>();
        if (pv.IsMine)
        {
            Camera.main.GetComponent<CameraMovement>().objectTofollow = tr.Find("FollowCam").gameObject.transform;
        }
        characterController = this.GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; //�ѷ����� Ȱ��ȭ
        }
        else
        {
            toggleCameraRotation = false; // �ѷ����� ��Ȱ��ȭ
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
        InputMovement();

        //Jump();

    }

    private void LateUpdate()
    {
        if(toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(GetComponent<Camera>().transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }
    void InputMovement()
    {
        finalSpeed = (isRun) ? runSpeed : speed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        characterController.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);

        //�޸��� �ִ�
        float percent = ((isRun) ? 1 : 0.5f) * moveDirection.magnitude;
        animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    public void SetPlayerName(string name)
    {
        this.name = name;
        GetComponent<PlayerMovement>().playerName.text = this.name;
    }
}
