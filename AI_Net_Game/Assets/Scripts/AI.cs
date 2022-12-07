using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private int randomState = 0;
    private int randomMove = 0;
    private float speed = 5f;

    enum State
    {
        Waiting,
        Move,
        Rotation,
    }

    enum MoveEnum
    {
        Front,
        Right,
        Left,
    }

    private State state;
    private MoveEnum moveEnum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�������� �ڽ��� ���¸� �����Ѵ�.
        //���, �̵�, ȸ��
        //�̵��� �����Լ��� ��, �� �� �� �ϳ�
        //ȸ���� �ε巴�� ȸ��(Y��)
        StartCoroutine(AIState());

        switch (state)
        {
            case State.Move:
                AIMove();
                break;

            case State.Rotation:
                break;

            case State.Waiting:
                break;

            default:
                break;
        }

    }

    IEnumerator AIState()
    {
        randomState = Random.Range(0, 3);

        switch (randomState)
        {
            case 0:
                state = State.Move;
                randomMove = Random.Range(0, 3);

                switch (randomMove)
                {
                    case 0:
                        moveEnum = MoveEnum.Right;
                        break;

                    case 1:
                        moveEnum = MoveEnum.Left;
                        break;

                    case 2:
                        moveEnum = MoveEnum.Front;
                        break;

                    default:
                        break;
                }
                break;

            case 1:
                state = State.Rotation;
                break;

            case 2:
                state = State.Waiting;
                break;

            default:
                break;
        }

        yield return new WaitForSeconds(3.0f);
    }

    private void AIMove()
    {
        switch (moveEnum)
        {
            case MoveEnum.Front:
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                break;

            case MoveEnum.Left:
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                break;

            case MoveEnum.Right:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                break;
        }
    }
}
