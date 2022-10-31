using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text goldText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text swordmanCostText;
    [SerializeField] private Text archerCostText;
    [SerializeField] private Text towerCostText;
    [SerializeField] private Slider levelSlider;
    [SerializeField] private Transform levelManager;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject towerPanel;

    public int gold;
    private float exp;
    private float maxExp = 100;
    public int level = 1;
    public int currentStage;
    public Vector3 targetPos;
    public Quaternion targetRot;
    public GameObject preTower;

    private int towerCount;
    private int towerCost;
    
    LevelManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindObjectOfType<LevelManager>();
    }
    void Start()
    {
        cam.gameObject.SetActive(false);
        currentStage = PlayerPrefs.GetInt("CurrentStage");
        LoadStage();
        gold = 40;
        goldText.text = gold.ToString();
        swordmanCostText.text = "20";
        archerCostText.text = "20";
        towerCostText.text = "50";

        levelSlider.maxValue = maxExp;
        levelSlider.value = exp;

        levelText.text = level.ToString();

        InvokeRepeating("GainSecGold", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        LevelUpdate();
        if(towerCount > 0)
        {
            TowerCostCalculate();
        }
    }

    private void LoadStage()
    {
        stageManager.LevelStart(currentStage);
    }


    private void LevelUpdate()
    {
        if(exp >= maxExp && level == 1)
            level = 2;
        levelText.text = level.ToString();
        exp += Time.deltaTime;
        levelSlider.value = exp;
    }

    private void GainSecGold()
    {
        gold += 5;
        goldText.text = gold.ToString();
    }

    public void GainGold(int much)
    {
        gold += much;
        goldText.text = gold.ToString();
    }

    public void SpendGold(int much)
    {
        gold -= much;
        goldText.text = gold.ToString();

    }

    public void WarriorBtn()
    {
        levelManager.GetChild(0).GetChild(0).GetChild(0).GetComponent<FinishManager>().Swordman();
    }

    public void ArcherBtn()
    {
        levelManager.GetChild(0).GetChild(0).GetChild(0).GetComponent<FinishManager>().Archer();
    }

    public void TowerButton()
    {
        NodeManager nodeManager;
        nodeManager = GameObject.FindObjectOfType<NodeManager>();
        nodeManager.TowerBuild(targetPos, targetRot, preTower);
        towerCount++;
        towerPanel.SetActive(false);
    }

    public void TowerPanelON()
    {
        towerPanel.SetActive(true);
    }
    public void TowerPanelOff()
    {
        towerPanel.SetActive(false);
    }

    private void TowerCostCalculate()
    {
        towerCost = 50+(towerCount * 25);
        towerCostText.text = towerCost.ToString();
    }
}
