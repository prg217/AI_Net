using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�����̽��ٸ� ������ �� ���� �� ���� AI�� �÷��̾ ���δ�. �ٵ� AI�� ü���� ���δ�.
        //�ڽ� �ݶ��̴��� �Ἥ �� �ȿ� �ִ� AI�� �÷��̾ �Ǵ��ؼ� ������(�ٵ� �׷��� �ڽ� �ݶ��̴� ���� �������� �״´�.)
        AttackKey();
    }

    private void AttackKey()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //��ȣ�ۿ� Ű
        {
            //������ �ڽ� �ݶ��̴� ���� ��� ����

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
        if (other.gameObject.tag == "AI")
        {

        }
    }
}
