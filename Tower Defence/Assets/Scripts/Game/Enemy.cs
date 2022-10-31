using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private EnemyObjects[] unit;
    [SerializeField] private GameObject bulletPrefab;

    public Transform target;
    private Animation anim;
    private Animator anima;

    public float speed = 1f;

    private int index = 1;
    private float range;
    private float frenchTime = 0.2f;

    public int maxHealth;
    private int health, a, attack;
    public bool isDead;
    private string type;
    private AudioSource audioSource;

    GameManager gameManager;
    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    public void StartStat(int _a)
    {
        a = _a;
        maxHealth = unit[a].health;
        range = unit[a].range;
        attack = unit[a].attack;
        type = unit[a].className;

    }

    void Start()
    {
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();


        // InvokeRepeating("UpdateEnemy", 0f, 0.5f);
        target = WayPointManager.points[index];
        health = maxHealth;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().maxValue = maxHealth;
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (type == "SwordMan")
                anim.Play("death");
            if (type == "Archer" || type == "Knight" || type == "Archer2")
                anim.Play("Death");
            Destroy(gameObject, 1.5f);
            if (!isDead)
                gameManager.GainGold(10);
            isDead = true;
            return;
        }
        UpdateEnemy();
        UpdateHealthBar();

        Vector3 dir = target.position - transform.position;
        Quaternion lookRoate = Quaternion.LookRotation(dir);
        Vector3 rotate = lookRoate.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotate.y, 0);


        transform.Translate(dir.normalized * Time.deltaTime * speed, Space.World);

        if (target == WayPointManager.points[index] && Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            NextPoint();
        }
    }

    private void UpdateHealthBar()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
    }
    private void NextPoint()
    {
        index++;
        target = WayPointManager.points[index];
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Finish")
        {
            col.gameObject.GetComponent<FinishManager>().TakeDamage(attack);
            Destroy(gameObject);
        }

        if (col.tag == "Enemy" && col.tag != "Player")
        {
            //col.gameObject.GetComponent<Enemy>().speed = 0f;
            target = gameObject.transform;
            speed = 0f;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Enemy")
        {
            // col.gameObject.GetComponent<Enemy>().speed = 0f;
            speed = 0;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            target = WayPointManager.points[index];
            speed = 1f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void UpdateEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        float shortesDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            float DistanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (DistanceToEnemy < shortesDistance)
            {
                shortesDistance = DistanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortesDistance <= range)
        {
            speed = 0f;
            target = nearestEnemy.transform;
            //Destroy(nearestEnemy, Random.Range(1,3));           

            frenchTime -= Time.deltaTime;
            if (frenchTime <= 0f)
            {
                if (type == "SwordMan")
                {
                    anim.Play("attack1");
                    StartCoroutine(DelaySwordmanSound());
                    nearestEnemy.gameObject.GetComponent<Player>().PlayerTakeDamage(10);
                }
                if (type == "Knight")
                {
                    anim.Play("AttackMelee1");
                    StartCoroutine(DelayKnightSound());
                    target.gameObject.GetComponent<Player>().PlayerTakeDamage(10);
                }
                if (type == "Archer" || type == "Archer2")
                {
                    anim.Play("AttackRange1");
                    StartCoroutine(DelayArrow());
                }


                frenchTime = 2f;
            }
            IEnumerator DelayKnightSound()
            {
                yield return new WaitForSeconds(0.3f);
                audioSource.Play();

            }

            IEnumerator DelaySwordmanSound()
            {
                yield return new WaitForSeconds(0.2f);
                audioSource.Play();

            }

            IEnumerator DelayArrow()
            {
                yield return new WaitForSeconds(0.4f);
                GameObject arrow = Instantiate(bulletPrefab, transform.GetChild(1).position, transform.GetChild(1).rotation);

                if (arrow != null)
                {
                    arrow.GetComponent<BulletManager>().Seek(target);
                }
            }
        }
        else
        {
            if (type == "SwordMan")
                anim.Play("run");
            if (type == "Archer" || type == "Knight" || type == "Archer2")
                anim.Play("RunFront");

            target = WayPointManager.points[index];
            speed = 1f;
        }

    }

    public void EnemyTakeDamage(int damage)
    {
        if (health > 0)
        {
            if (type == "SwordMan")
                anim.Play("getHit");
            health -= damage;
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;
        }

    }


}
