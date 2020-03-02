using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    public float speed;
    public int damage;
    public Sprite snappedArrow;
    
    private SpriteRenderer _renderer;
    private float _lifeTime = 50;
    
  //  public GameObject destroyEffect;

    private void Start() {
        _renderer = GetComponent<SpriteRenderer>();
        // Destroying when hit
        StartCoroutine(DestroyArrow());
    }

    private void Update() {
        // Ray collider controlling
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector3.right, .1f);
        
        // Range controlling for arrow
        if (_lifeTime > 0){
            _lifeTime -= 1;
            // When ray collides with another collider
            if (hitInfo.collider != null){
                // If it is enemy
                if (hitInfo.collider.CompareTag("Enemy")){
                    Debug.Log("ENEMYYYY!!");
                    hitInfo.collider.GetComponent<EnemyController>().TakeDamage(damage);
                }
                _lifeTime = 0;
            }
            else {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        else if (_lifeTime == 0){
            _lifeTime -= 1;
            _renderer.sprite = snappedArrow;
            //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
    
    // Waits and destroys the object
    IEnumerator DestroyArrow() {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
