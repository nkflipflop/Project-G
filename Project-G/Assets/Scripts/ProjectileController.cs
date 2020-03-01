using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public float speed;
    public float lifeTime;
    public int damage;
    public GameObject projectile;

  //  public GameObject destroyEffect;

    private void Start() {
        // TO DESTROY WHEN HIT
        Invoke("DestroyProjectile", lifeTime);
    }

    private void Update() {
        // RAY COLLIDER
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector3.right, .1f);
        // WHEN RAY COLLIDE WITH ANOTHER COLLIDER
        if (hitInfo.collider != null){
            // IF ENEMY
            if (hitInfo.collider.CompareTag("Enemy")){
                Debug.Log("ENEMYYYY!!");
                hitInfo.collider.GetComponent<EnemyController>().TakeDamage(damage);
            }
            DestroyProjectile();
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void DestroyProjectile() {
    //    Instantiate(destroyEffect, transform.position, Quaternion.identity);
                Instantiate(projectile, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
