using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public string owner = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //스페이스바를 눌렀을 때 전방 얼마 안의 AI나 플레이어를 죽인다. 근데 AI면 체력이 깎인다.
        //박스 콜라이더를 써서 그 안에 있는 AI나 플레이어를 판단해서 죽이자(근데 그러면 박스 콜라이더 안의 여러명이 죽는다.)
        AttackKey();
    }

    private void AttackKey()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
