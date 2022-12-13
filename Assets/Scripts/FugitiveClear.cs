using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FugitiveClear : MonoBehaviour
{
    public GameObject start;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerMovement>().isChaser == false)
            {
                //µµ¸ÁÀÚ ½Â¸®
                start.GetComponent<StartButton>().FugitiveWin();
            }
        }
    }
}
