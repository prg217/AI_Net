using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairObj : MonoBehaviour
{
    public Slider waitBar;
    private float maxTime = 7f;
    private float nowTime = 0f;
    private float time = 0f;
    private float saveTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            waitBar.gameObject.SetActive(true);

            saveTime = time;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nowTime = time - saveTime;

            waitBar.value = nowTime / maxTime;

            if (nowTime > maxTime)
            {
                //미션쪽에 수리카운트 올려주기
                other.gameObject.GetComponent<Mission>().repairCount++;
                other.gameObject.GetComponent<Mission>().RepairMission();
                waitBar.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            waitBar.gameObject.SetActive(false);
            if (nowTime > maxTime)
            {
                //미션쪽에 수리카운트 올려주기
                other.gameObject.GetComponent<Mission>().repairCount++;
                other.gameObject.GetComponent<Mission>().RepairMission();
                waitBar.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }

            nowTime = 0f;
        }
    }
}
