using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefs;
    [SerializeField] private GameObject[] towerPreViewPrefs;
    
    public Color hoverColor;

    private Color startColor;
    private Renderer rend;
    private int level;
    private float preTime;
    private GameObject preTower;
    private bool isClicked;

    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }
    private void Update()
    {
        //Debug.Log(currentNode);
        level = gameManager.level -1;
        preTime += Time.deltaTime;
        if(preTime >= 5 && isClicked)
        {
            Destroy(preTower);
            gameManager.TowerPanelOff();
            isClicked = false;
        }
    }

    void OnMouseDown()
    {
        Destroy(gameManager.preTower);

        preTime = 0;
        Vector3 dif = transform.position;
        dif.y += 0.5f;
        gameManager.targetPos = dif;
        gameManager.targetRot = transform.rotation;
        preTower = Instantiate(towerPreViewPrefs[level], gameManager.targetPos, gameManager.targetRot);
        gameManager.preTower = preTower;
        gameManager.TowerPanelON();
        isClicked = true;


    }

    public void TowerBuild(Vector3 _targetPos, Quaternion _targetRot, GameObject _preTower)
    {
        if(gameManager.gold >= 50 && _preTower != null)
        {
            Destroy(_preTower);
            gameManager.SpendGold(50);          
            GameObject tower = Instantiate(towerPrefs[level], _targetPos, _targetRot); 
        }
        else if(gameManager.gold < 50 && _preTower != null)
        {
            Debug.Log("Paranız Yetersiz");
        }
        else if(gameManager.gold >= 50 && _preTower == null)
        {
            Debug.Log("Lütfen Kule Alanı Seçiniz");
        }
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
    
}
