using General;
using NaughtyAttributes;
using Pooling;
using Pooling.Interfaces;
using UnityEngine;

public class Turret : HandControllerBase, IPoolable, IHealthInteractable
{
    [field: SerializeField] public ObjectType Type { get; set; }
    
    [SerializeField] private LayerMask hittableLayersByEnemy;
    private bool targetInRange;

    protected override void SpecialStart()
    {
        CurrentWeapon = transform.GetChild(0).GetComponent<WeaponBase>();
        CurrentWeapon.transform.localPosition = WeaponPosition;
        AimDeviation = 2f;
    }

    protected override void SpecialUpdate()
    {
        CheckPlayerInRange();
    }

    protected override void UseWeapon()
    {
        if (!(this as IHealthInteractable).IsDead && targetInRange)
        {
            CurrentWeapon.Trigger();
        }
        else
        {
            CurrentWeapon.ReleaseTrigger();
        }
    }

    private void CheckPlayerInRange()
    {
        if (TargetObject != null)
        {
            Vector3 direction = TargetObject.transform.position - transform.position;
            direction.y -= .23f;
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, 4f, hittableLayersByEnemy);

            //Debug.DrawRay(transform.position, direction, Color.blue, .1f);
            targetInRange = hitInfo.collider != null &&
                             (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Shield"));
        }
        else
        {
            targetInRange = false;
        }
    }

    #region Pooling
    
    public void OnSpawn()
    {
        CurrentHealth = MaxHealth;
        WeaponPosition = new Vector3(0f, 0.497f, 0f);
        DissolveEffect.Reset();
    }

    public void OnReset() { }
    
    #endregion
    
    #region Health Operations
    
    [field: SerializeField, Foldout("Health")] public int CurrentHealth { get; set; }
    [field: SerializeField, Foldout("Health")] public int MaxHealth { get; set; }
    [field: SerializeField, Foldout("Health")] public Dissolve DissolveEffect { get; set; }
    [field: SerializeField, Foldout("Health")] public SpriteRenderer HealthEffectRenderer { get; set; }
    [field: SerializeField, Foldout("Health")] public CapsuleCollider2D HitBoxCollider { get; set; }
    [field: SerializeField, Foldout("Health")] public SoundManager.Sound HitSound { get; set; }
    
    #endregion
}