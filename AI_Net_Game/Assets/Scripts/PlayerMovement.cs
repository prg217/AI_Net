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

        //�޸��� �ִ�
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
        if (Input.GetKeyDown(KeyCode.Space)) //GetKey�� �ϸ� ������ ���� ��� ���ӵ�, GetKeyDown�� �ؾ� �� ���� ������
        {
            if (isJump == false)
            {
                characterController.Move(Vector3.up * jumpPower);
                //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); //�׸��� �ٴڿ� ����� ����� ���ǹ��� �߰� ���صθ� ���� ������ ��
                Debug.Log("���");
                animator.SetBool("isJump",true);
                isJump = true;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane") //Plane�� �±� �߰�
        {
            isJump = false; //Plane�� �÷��̾ ���� �ݶ��̴� �߰�
            animator.SetBool("isJump", false);
        }
    }*/
}
