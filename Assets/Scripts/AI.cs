using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Animator animator;

    private int randomState = 0;
    private int randomMove = 0;
    private int randomRL = 0;
    private float speed = 5f;
    private float turnSpeed = 10f;

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
        StartCoroutine(AIState());
    }

    // Update is called once per frame
    void Update()
    {
        //랜덤으로 자신의 상태를 결정한다.
        //대기, 이동, 회전
        //이동은 로컬함수로 앞, 옆 둘 중 하나
        //회전은 부드럽게 회전(Y축)

        switch (state)
        {
            case State.Move:
                animator.SetBool("isWalk", true);
                AIMove();
                break;

            case State.Rotation:
                animator.SetBool("isWalk", false);
                this.transform.Rotate(Vector3.up * randomRL * turnSpeed * Time.deltaTime);
                break;

            case State.Waiting:
                animator.SetBool("isWalk", false);
                break;

            default:
                break;
        }

    }

    IEnumerator AIState()
    {
        while (true)
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
                    randomRL = Random.Range(-10, 11);
                    state = State.Rotation;
                    break;

                case 2:
                    state = State.Waiting;
                    break;

                default:
                    break;
            }

            yield return new WaitForSeconds(3.0f); //3초마다 판단하게
        }
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
