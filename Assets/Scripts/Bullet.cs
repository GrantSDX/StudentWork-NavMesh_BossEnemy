using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyBigBoss enemyBigBoss))
        {
           enemyBigBoss.TakeDamage(_damage);
            
        }
        Destroy(gameObject);
    }

}
