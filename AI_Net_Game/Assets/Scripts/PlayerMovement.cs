using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Camera camera;
    CharacterController characterController;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed;
    public bool isRun;

    public bool toggleCameraRotation;
    public float smoothness = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
        animator = this.GetComponent<Animator>();
        camera = Camera.main;
        characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; //둘러보기 활성화
        }
        else
        {
            toggleCameraRotation = false; // 둘러보기 비활성화
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
            Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));
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

        //달리기 애니
        float percent = ((isRun) ? 1 : 0.5f) * moveDirection.magnitude;
        animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    /*void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }*/

    /*void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //GetKey로 하면 누르는 동안 계속 지속됨, GetKeyDown을 해야 한 번만 눌러짐
        {
            if (isJump == false)
            {
                characterController.Move(Vector3.up * jumpPower);
                //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); //그리고 바닥에 닿았을 때라는 조건문을 추가 안해두면 무한 점프가 됨
                Debug.Log("헬로");
                animator.SetBool("isJump",true);
                isJump = true;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane") //Plane에 태그 추가
        {
            isJump = false; //Plane와 플레이어에 각각 콜라이더 추가
            animator.SetBool("isJump", false);
        }
    }*/
}
