using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Mission : MonoBehaviourPun
{
    private int currentNumber = 0;
    private List<int> randomMission = new List<int>();
    private int missionMax = 5;
    private List<GameObject> ui = new List<GameObject>();

    private bool runMission = false;
    //private bool targetMission = false;
    private bool stopMission = false;
    public bool jumpMission = false;

    private float time = 0f;
    private float firstTime = 0f;
    private bool firstTimeBool = false;
    private float runTime = 0;
    private float saveRunTime = 0;
    private TextMeshProUGUI runMissionText;

    public List<GameObject> target = new List<GameObject>();
    public int tagetCount = 0;

    private float stopTime = 0f;
    private float stopTimeSave = 0f;
    private bool stopBool = false;
    private TextMeshProUGUI stopMissionText;

    public int jumpCount = 0;
    private TextMeshProUGUI jumpMissionText;

    public int repairCount = 0;
    private TextMeshProUGUI repairMissionText;
    private List<GameObject> repair = new List<GameObject>();

    private GameObject delivery;

    public void Start()
    {
        ui.Add(GameObject.Find("Mission").transform.Find("RunMission").gameObject);
        ui.Add(GameObject.Find("Mission").transform.Find("TargetMission").gameObject);
        ui.Add(GameObject.Find("Mission").transform.Find("StopMission").gameObject);
        ui.Add(GameObject.Find("Mission").transform.Find("JumpMission").gameObject);
        ui.Add(GameObject.Find("Mission").transform.Find("RepairMission").gameObject);
        ui.Add(GameObject.Find("Mission").transform.Find("DeliveryMission").gameObject);

        runMissionText = GameObject.Find("Mission").transform.Find("RunMission").GetComponent<TextMeshProUGUI>();

        target.Add(GameObject.Find("TargetObj").transform.Find("1").gameObject);
        target.Add(GameObject.Find("TargetObj").transform.Find("2").gameObject);
        target.Add(GameObject.Find("TargetObj").transform.Find("3").gameObject);
        target.Add(GameObject.Find("TargetObj").transform.Find("4").gameObject);

        stopMissionText = GameObject.Find("Mission").transform.Find("StopMission").GetComponent<TextMeshProUGUI>();
        jumpMissionText = GameObject.Find("Mission").transform.Find("JumpMission").GetComponent<TextMeshProUGUI>();

        repairMissionText = GameObject.Find("Mission").transform.Find("RepairMission").GetComponent<TextMeshProUGUI>();

        repair.Add(GameObject.Find("RepairObj").transform.Find("1").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("2").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("3").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("4").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("5").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("6").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("7").gameObject);
        repair.Add(GameObject.Find("RepairObj").transform.Find("8").gameObject);

        delivery = GameObject.Find("DeliveryObj").transform.Find("1").gameObject;
    }

    public void GetMission()
    {
        CreateUnDuplicateRandom(0, 6);

        for (int i = 0; i < missionMax; i++)
        {
            switch (randomMission[i])
            {
                case 0:
                    runMission = true;
                    ui[0].SetActive(true);
                    break;
                case 1:
                    TargetMission();
                    ui[1].SetActive(true);
                    break;
                case 2:
                    stopMission = true;
                    ui[2].SetActive(true);
                    break;
                case 3:
                    jumpMission = true;
                    ui[3].SetActive(true);
                    break;
                case 4:
                    RepairStart();
                    ui[4].SetActive(true);
                    break;
                case 5:
                    DeliveryMission();
                    ui[5].SetActive(true);
                    break;
            }
        }
        /*
        runMission = true;
        //���̼� UI���̰� ����

        //targetMission = true;
        TargetMission();

        stopMission = true;

        jumpMission = true;

        RepairStart();

        DeliveryMission();
        */
    }

    public void Update()
    {
        time += Time.deltaTime;

        if (runMission)
        {
            RunMission();
        }
        if (stopMission)
        {
            StopMission();
        }
        if (jumpMission)
        {
            JumpMission();
        }
    }

    public void RunMission()
    {
        if (runTime >= 20f)
        {
            gameObject.GetComponent<PlayerScript>().MissionCountUp();//�÷��̾� �̼� Ŭ���� ī��Ʈ �ø���

            runMission = false;
            ui[0].SetActive(false);
            //���̼� UI����
            //���� ���� ������ ����
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
                
                runMissionText.text = "�޸��� (" + (int)runTime + "/20)";
            }
            else
            {
                saveRunTime = runTime;
                firstTimeBool = false;
            }
        }
    }

    public void TargetMission()
    {
        //������ �� �Դ� �����ϱ�

        for (int i = 0; i < target.Count; i++)
        {
            target[i].SetActive(true);
        }
    }

    public void TargetMissionClear()
    {
        //targetMission = false;
        gameObject.GetComponent<PlayerScript>().MissionCountUp();
        ui[1].SetActive(false);
    }

    public void StopMission()
    {
        //�� �ڸ��� �� �ʰ� ������ �ֱ�(�����̸� �ʱ�ȭ)
        if (stopTime >= 30)
        {
            stopMission = false;
            gameObject.GetComponent<PlayerScript>().MissionCountUp();//�÷��̾� �̼� Ŭ���� ī��Ʈ �ø���
            ui[2].SetActive(false);
        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.Space))
            {
                stopBool = false;
            }
            else
            {
                if (stopBool == false)
                {
                    stopTimeSave = time;
                    stopBool = true;
                }

                stopTime = time - stopTimeSave;
                stopMissionText.text = "���߱� (" + (int)stopTime + "/30)";
            }
        }
        
    }

    public void JumpMission()
    {
        //���� �� �� �ٱ�
        if (jumpCount >= 30)
        {
            jumpMission = false;
            gameObject.GetComponent<PlayerScript>().MissionCountUp();
            ui[3].SetActive(false);
            //���� UI����
        }
        else
        {
            jumpMissionText.text = "���� (" + jumpCount + "/30)";
        }
    }

    //���ε� ����
    public void RepairStart()
    {
        for (int i = 0; i < repair.Count; i++)
        {
            repair[i].SetActive(true);
        }
    }

    public void RepairMission()
    {
        repairMissionText.text = "���ε� ���� (" + repairCount + "/4)";

        if (repairCount >= 4)
        {
            gameObject.GetComponent<PlayerScript>().MissionCountUp();

            for (int i = 0; i < repair.Count; i++)
            {
                repair[i].SetActive(false);
            }

            ui[4].SetActive(false);
            //UI�����
        }
    }

    //���� ���� ���
    public void DeliveryMission()
    {
        delivery.SetActive(true);
    }

    public void DeliveryClear()
    {
        //UI�����
        gameObject.GetComponent<PlayerScript>().MissionCountUp();
        ui[5].SetActive(false);
    }

    //���� ����

    //���� ĳ�� ��ǰ�ϱ�

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
