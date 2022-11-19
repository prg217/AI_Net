using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheChaser : MonoBehaviour
{
    //추격자
    public GameObject attackBox; //스타트할 때 서치해서 넣어줘야함

    // Start is called before the first frame update
    void Start()
    {
        attackBox = transform.Find("AttackBox").gameObject; //AttackBox라는 이름을 가진 자식 오브젝트만 찾아서 넣어줌
    }

    // Update is called once per frame
    void Update()
    {
        //스페이스바를 눌렀을 때 전방 얼마 안의 AI나 플레이어를 죽인다. 근데 AI면 체력이 깎인다.
        //박스 콜라이더를 써서 그 안에 있는 AI나 플레이어를 판단해서 죽이자(근데 그러면 박스 콜라이더 안의 여러명이 죽는다.)
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //상호작용 키
        {
            //누르면 박스 콜라이더 안의 얘들 죽임
            //
        }
    }
}
