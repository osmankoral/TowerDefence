using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private float range = 3f;
    private float attackTime = 1f;
    private string enemyTag = "Enemy";
    public Transform target;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void UpdateTower()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortesDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(var enemy in enemies)
        {
            float DistanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            bool isDead = enemy.GetComponent<Enemy>().isDead;
            if(DistanceToEnemy < shortesDistance && !isDead)
            {
                shortesDistance = DistanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortesDistance <= range)
        {
            bool isDead = nearestEnemy.GetComponent<Enemy>().isDead;
            if(!isDead)
                target = nearestEnemy.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTower();
        if(target == null)
            return;
        bool isDead = target.GetComponent<Enemy>().isDead;
        if(isDead)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;

        transform.rotation = Quaternion.Euler(0, rotation.y, 0);

        Attack();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Attack()
    {
        attackTime += Time.deltaTime;
        if(attackTime >= 2f)
        {
            audioSource.Play();
            GameObject bullet = Instantiate(bulletPrefab, transform.GetChild(0).GetComponent<Transform>().position, transform.GetChild(0).GetComponent<Transform>().rotation);
            BulletManager bulletManager = bullet.GetComponent<BulletManager>();

            if(bullet != null)
            {
                bulletManager.Seek(target);
            }

            attackTime = 0;
        }
    }
}
