using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Weapons;

public class HandControllerBase : MonoBehaviour
{
    public SpriteRenderer CharacterRenderer;
    public GameObject TargetObject;
    [NonSerialized] public WeaponBase currentWeapon;

    protected bool characterIsRunning = false;
    protected Vector3 weaponPosition = new (0, 0, 0);
    protected float aimDeviation; // ability to hit the bull's eye (if 0, you are the best)
    protected Vector3 targetObjectPosition;

    protected Vector3 aimPosition;
    private float verticalTrigger = 1; // for inverse scaling of the weapon
    private bool canShoot = false;
    private const float FIRE_DELAY = 1f;

    private void Start()
    {
        SpecialStart();
        _ = GivePermissionToFire(FIRE_DELAY);
    }

    protected virtual void SpecialStart() { }

    private void Update()
    {
        GetInputs();
        SpecialUpdate();
    }

    private void FixedUpdate()
    {
        ControlWeapon();
    }

    protected virtual void SpecialUpdate() { }

    // Adjusts the sorting order of the weapon according to mouse position (player's direction)
    private void AdjustSortingOrder()
    {
        int sortingOrder = CharacterRenderer.sortingOrder;
        if (characterIsRunning)
        {
            Vector2 mouseDir = aimPosition - transform.parent.position;
            float horizontal = mouseDir.x;
            float vertical = mouseDir.y;
            float slope = horizontal / vertical;

            if (-1 < slope && slope < 1 && vertical > 0)
            {
                sortingOrder -= 4;
            }
            else
            {
                sortingOrder += 4;
            }
        }
        else
        {
            sortingOrder += 4;
        }

        currentWeapon.SetSortingOrder(sortingOrder);
    }

    // This is default Trigger the weapon fitting to player, you must override for NPCs
    protected virtual void UseWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            currentWeapon.Trigger(TriggerSource.Player);
        }
        else
        {
            currentWeapon.ReleaseTrigger();
        }
    }

    // Aims the weapon
    private void AimWeapon()
    {
        // Flipping hand position and weapon direction
        float verticalAxis = Mathf.Sign(aimPosition.x - transform.position.x);
        bool scale = !(verticalTrigger * verticalAxis > 0);
        verticalTrigger = verticalAxis;
        if (scale)
        {
            currentWeapon.ScaleInverse();
        }

        // Getting mouse position and direction of player to mouse
        Vector3 aimDirection = (aimPosition - currentWeapon.transform.position).normalized;

        // Rotating the current weapon
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        currentWeapon.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void ControlWeapon()
    {
        if (currentWeapon != null)
        {
            AimWeapon();
            AdjustSortingOrder();
            if (canShoot)
            {
                currentWeapon.WeaponUpdate();
                UseWeapon();
            }
        }
    }

    // Gets position of target by different calculation
    protected virtual Vector3 GetTargetPosition()
    {
        return TargetObject.transform.position;
    }
    
    private void GetInputs()
    {
        // Taking aim position
        if (TargetObject)
        {
            float distance = Vector3.Distance(aimPosition, transform.position) / 16;

            aimPosition = GetTargetPosition() + UnityEngine.Random.insideUnitSphere *
                UnityEngine.Random.Range(0, distance) * aimDeviation;
            aimPosition.z = 0f;
        }
    }

    private async UniTaskVoid GivePermissionToFire(float waitTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        canShoot = true;
    }
}