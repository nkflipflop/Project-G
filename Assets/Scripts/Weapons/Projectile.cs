using System;
using Cysharp.Threading.Tasks;
using General;
using NaughtyAttributes;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [field: SerializeField] public ObjectType Type { get; set; }

    [Foldout("Components")] public SpriteRenderer spriteRenderer;
    [Foldout("Components")] public GameObject lightObject;
    
    [Foldout("Properties"), SerializeField] private int endurance = 1;
    [Foldout("Properties"), SerializeField] private float speed = 10;
    [Foldout("Properties"), SerializeField] private int damage = 2;
    [Foldout("Properties"), SerializeField] private bool shotByPlayer;
    
    [Foldout("Config"), SerializeField] private LayerMask hittableLayersByPlayer;
    [Foldout("Config"), SerializeField] private LayerMask hittableLayersByEnemy;
    
    private float lifetime = 100;
    private int cachedEndurance;
    private float cachedSpeed;

    private bool isActive;

    private void Start()
    {
        cachedEndurance = endurance;
        cachedSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
        
        // Ray collider controlling
        Vector3 direction = transform.rotation * Vector3.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, .25f,
            shotByPlayer ? hittableLayersByPlayer : hittableLayersByEnemy);
        //Debug.DrawRay(transform.position, direction, Color.green);
        // Range controlling for bullet	
        if (lifetime > 0 && endurance > 0)
        {
            lifetime -= 1;
            // When ray collides with another collider
            if (hitInfo.collider != null)
            {
                endurance--;
                // If it is damageable
                if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Enemy") ||
                    hitInfo.collider.CompareTag("Breakable"))
                {
                    hitInfo.collider.GetComponent<IHealthInteractable>().TakeDamage(damage);
                }

                if (endurance < 1)
                {
                    lifetime = 0;       // Stopping the projectile
                }
                else
                {
                    speed *= 0.6f; // after hitting an enemy or an object, slow down the bullet
                }
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        else if (lifetime == 0)
        {
            lifetime -= 1;
            spriteRenderer.enabled = false;
            lightObject.SetActive(false);

            // Creating After Effect
            PoolFactory.instance.GetObject(ObjectType.HitEffect, transform.position, transform.rotation);
            isActive = false;
            this.ResetObject();
        }
    }
    
    public async UniTaskVoid Activate(bool shotByPlayer)
    {
        this.shotByPlayer = shotByPlayer;
        isActive = true;
        await UniTask.WhenAny(UniTask.Delay(TimeSpan.FromSeconds(5f)), UniTask.WaitUntil(() => !isActive))
            .AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        isActive = false;
    }
    
    public void OnSpawn()
    {
        spriteRenderer.enabled = true;
        lifetime = 100;
        lightObject.SetActive(true);
    }

    public void OnReset()
    {
        endurance = cachedEndurance;
        speed = cachedSpeed;
    }
}