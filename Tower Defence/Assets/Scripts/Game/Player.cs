using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public Transform target;

    private Animation anim;
    private AudioSource audioSource;

    public float speed = 1f;

    private int index;
    private float range;
    private float frenchTime = 0.2f;

    public int maxHealth;
    private int health, a, attack;
    private string type;

    public void PlayerStat(int health, int attack, float range, string _type)
    {
        maxHealth = health;
        this.attack = attack;
        this.range = range;
        type = _type;

    }

    void Start()
    {
        index = WayPointManager.childCount - 1;
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();

        target = WayPointManager.points[index];
        //InvokeRepeating("UpdatePlayer", 0f, 0.5f);
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
            {
                anim.Play("death");
                Destroy(gameObject, 1f);
            }
            if (type == "Archer" || type == "Knight" || type == "Archer2")
            {
                anim.Play("Death");
                Destroy(gameObject, 1f);
            }
            return;
        }

        UpdatePlayer();
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
        index--;
        target = WayPointManager.points[index];

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Start")
        {
            col.gameObject.GetComponent<StartManager>().TakeDamage(attack);
            Destroy(gameObject);
        }

        if (col.tag == "Player")
        {
            speed = 0f;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && col.tag != "Enemy")
        {
            //col.gameObject.GetComponent<Player>().speed = 0f;
            target = gameObject.transform;
            speed = 0;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
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

    private void UpdatePlayer()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortesDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (var enemy in enemies)
        {
            float DistanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            bool isDead = enemy.GetComponent<Enemy>().isDead;
            if (DistanceToEnemy < shortesDistance && !isDead)
            {
                shortesDistance = DistanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortesDistance <= range)
        {
            speed = 0f;
            target = nearestEnemy.transform;


            frenchTime -= Time.deltaTime;
            if (frenchTime <= 0f)
            {
                if (type == "SwordMan")
                {
                    anim.Play("attack1");
                    StartCoroutine(DelaySwordmanSound());
                    target.gameObject.GetComponent<Enemy>().EnemyTakeDamage(10);
                    frenchTime = 2f;
                }
                if (type == "Knight")
                {
                    anim.Play("AttackMelee1");
                    StartCoroutine(DelayKnightSound());
                    target.gameObject.GetComponent<Enemy>().EnemyTakeDamage(10);
                    frenchTime = 2f;
                }
                if (type == "Archer" || type == "Archer2")
                {
                    anim.Play("AttackRange1");
                    //target.gameObject.GetComponent<Enemy>().EnemyTakeDamage(10);
                    StartCoroutine(DelayArrow());

                    frenchTime = 2f;
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
        }
        else
        {
            if (type == "SwordMan")
            {
                anim.Play("run");
                target = WayPointManager.points[index];
                speed = 1f;
            }
            if (type == "Archer" || type == "Knight" || type == "Archer2")
            {
                anim.Play("RunFront");
                target = WayPointManager.points[index];
                speed = 1f;
            }

        }

    }

    IEnumerator DelayKnightSound()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource.Play();

    }

        IEnumerator DelaySwordmanSound()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource.Play();

    }

    public void PlayerTakeDamage(int damage)
    {
        if (health > 0)
        {
            if (type == "SwordMan")
            {
                anim.Play("getHit");
                health -= damage;
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;

            }
            if (type == "Archer" || type == "Knight" || type == "Archer2")
            {
                // Archer take Animation
                health -= damage;
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>().value = health;
            }

        }
    }


}
