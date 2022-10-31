using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab;

    private int maxHealth = 100;
    private int health;
    

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().maxValue = maxHealth;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;
    
        InvokeRepeating("CreateEnemy", 0f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            if(PlayerPrefs.GetInt("UnlockStage") == gameManager.currentStage)
            {
                PlayerPrefs.SetInt("UnlockStage", gameManager.currentStage+1);
            }

            SceneManager.LoadScene("MenuScene");
        }
    }

    public void CreateEnemy()
    {
        int a = Random.Range(0,2);
        if(gameManager.level == 2)
            a += 2;
        GameObject enemy = Instantiate(enemyPrefab[a], transform.position, transform.rotation);
        enemy.GetComponent<Enemy>().StartStat(a);
    }

        public void TakeDamage(int damage)
    {
        health -= damage;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;
    }
}
