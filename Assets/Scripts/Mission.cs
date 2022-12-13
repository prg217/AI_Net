using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Mission : MonoBehaviourPun
{
    private int currentNumber = 0;
    private List<int> randomMission = new List<int>();
    private int missionMax = 3;

    private bool runMission = false;

    private float time = 0f;
    private float firstTime = 0f;
    private bool firstTimeBool = false;
    private float runTime = 0;
    private float saveRunTime = 0;
    private TextMeshProUGUI runMissionText;

    public void Start()
    {
        runMissionText = GameObject.Find("Mission").transform.Find("RunMission").GetComponent<TextMeshProUGUI>();
    }

    public void GetMission()
    {
        /*
        CreateUnDuplicateRandom(0, 7);

        for (int i = 0; i < missionMax; i++)
        {
            switch (randomMission[i])
            {
                case 0:
                    runMission = true;
                    break;
            }
        }*/

        runMission = true;
    }

    public void Update()
    {
        time += Time.deltaTime;

        if (runMission)
        {
            RunMission();
            //런미션 UI보이게 해줌
        }
    }

    public void RunMission()
    {
        if (runTime >= 30f)
        {
            gameObject.GetComponent<PlayerMovement>().missionClearCount++;//플레이어 미션 클리어 카운트 늘리기

            runMission = false;
            //런미션 UI끄기
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))))
            {
                //시간 누적해서 카운트하기

                if (firstTimeBool == false)
                {
                    firstTime = time;
                    firstTimeBool = true;
                }

                runTime = time - firstTime + saveRunTime;
                //saveRunTime += runTime;
                runMissionText.text = "달리기 (" + (int)runTime + "/30)";
            }
            else
            {
                saveRunTime = runTime;
                firstTimeBool = false;
            }
        }
    }

    void CreateUnDuplicateRandom(int min, int max)
    {
        currentNumber = Random.Range(min, max);

        for (int i = 0; i < missionMax;)
        {
            if (randomMission.Contains(currentNumber))
            {
                currentNumber = Random.Range(min, max);
            }
            else
            {
                randomMission.Add(currentNumber);
                i++;
            }
        }
    }
}
