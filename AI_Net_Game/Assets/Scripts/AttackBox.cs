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
        //�����̽��ٸ� ������ �� ���� �� ���� AI�� �÷��̾ ���δ�. �ٵ� AI�� ü���� ���δ�.
        //�ڽ� �ݶ��̴��� �Ἥ �� �ȿ� �ִ� AI�� �÷��̾ �Ǵ��ؼ� ������(�ٵ� �׷��� �ڽ� �ݶ��̴� ���� �������� �״´�.)
        AttackKey();
    }

    private void AttackKey()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
