using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheChaser : MonoBehaviour
{
    //�߰���
    public GameObject attackBox; //��ŸƮ�� �� ��ġ�ؼ� �־������

    // Start is called before the first frame update
    void Start()
    {
        attackBox = transform.Find("AttackBox").gameObject; //AttackBox��� �̸��� ���� �ڽ� ������Ʈ�� ã�Ƽ� �־���
    }

    // Update is called once per frame
    void Update()
    {
        //�����̽��ٸ� ������ �� ���� �� ���� AI�� �÷��̾ ���δ�. �ٵ� AI�� ü���� ���δ�.
        //�ڽ� �ݶ��̴��� �Ἥ �� �ȿ� �ִ� AI�� �÷��̾ �Ǵ��ؼ� ������(�ٵ� �׷��� �ڽ� �ݶ��̴� ���� �������� �״´�.)
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //��ȣ�ۿ� Ű
        {
            //������ �ڽ� �ݶ��̴� ���� ��� ����
            //
        }
    }
}
