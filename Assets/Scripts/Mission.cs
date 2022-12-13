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
            //���̼� UI���̰� ����
        }
    }

    public void RunMission()
    {
        if (runTime >= 30f)
        {
            gameObject.GetComponent<PlayerMovement>().missionClearCount++;//�÷��̾� �̼� Ŭ���� ī��Ʈ �ø���

            runMission = false;
            //���̼� UI����
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))))
            {
                //�ð� �����ؼ� ī��Ʈ�ϱ�

                if (firstTimeBool == false)
                {
                    firstTime = time;
                    firstTimeBool = true;
                }

                runTime = time - firstTime + saveRunTime;
                //saveRunTime += runTime;
                runMissionText.text = "�޸��� (" + (int)runTime + "/30)";
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
