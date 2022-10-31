using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Transform target;
    private float speed = 10f;
 
 public void Seek(Transform _target)
 {
     target = _target;
 }


    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 dir = target.position - transform.position;
        float DistanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= DistanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * DistanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        Destroy(gameObject);
        if(target.tag == "Enemy")
            target.GetComponent<Enemy>().EnemyTakeDamage(25);
        
        if(target.tag == "Player")
            target.GetComponent<Player>().PlayerTakeDamage(25);
    }
}
