using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefab;

    [SerializeField] private EnemyObjects[] units;

    GameManager gameManager;

    private int maxHealth = 100;
    private int health;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    void Start()
    {
   
       // InvokeRepeating("CreatePlayer", 0f, 5f);
       
       health = maxHealth;
       gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().maxValue = maxHealth;
       gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayer(int a)
    {
        if(gameManager.level == 2)
            a += 2;
        if(gameManager.gold >= 20)
        {
            gameManager.SpendGold(20);
            GameObject player = Instantiate(playerPrefab[a], transform.position, transform.rotation);

            player.GetComponent<Player>().PlayerStat(units[a].health, units[a].attack, units[a].range, units[a].className);
        }
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;
    }

    public void Swordman()
    {
        CreatePlayer(0);
    }

    public void Archer()
    {
        CreatePlayer(1);
    }
}
