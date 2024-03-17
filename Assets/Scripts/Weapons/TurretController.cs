using Pooling;
using Pooling.Interfaces;
using UnityEngine;

public class TurretController : HandControllerBase, IPoolable
{
    [field: SerializeField] public ObjectType Type { get; set; }
    
    [SerializeField] private LayerMask hittableLayersByEnemy;
    public HealthController HealthController;
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
        if (!HealthController.IsDead && targetInRange)
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

    public void OnSpawn()
    {
        WeaponPosition = new Vector3(0f, 0.497f, 0f);
    }

    public void OnReset() { }
}