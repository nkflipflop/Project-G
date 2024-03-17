using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HandControllerBase : MonoBehaviour
{
    public SpriteRenderer CharacterRenderer;
    public GameObject TargetObject;
    [NonSerialized] public WeaponBase CurrentWeapon;

    protected bool CharacterIsRunning = false;
    protected Vector3 WeaponPosition = new (0, 0, 0);
    protected float AimDeviation; // ability to hit the bull's eye (if 0, you are the best)
    protected Vector3 TargetObjectPosition;

    protected Vector3 AimPosition;
    private float verticalTrigger = 1; // for inverse scaling of the weapon
    private bool canShoot = false;
    private const float FIRE_DELAY = 1f;

    private void Start()
    {
        SpecialStart();
        GivePermissionToFire(FIRE_DELAY);
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
        if (CharacterIsRunning)
        {
            Vector2 mouseDir = AimPosition - transform.parent.position;
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

        CurrentWeapon.SetSortingOrder(sortingOrder);
    }

    // This is default Trigger the weapon fitting to player, you must override for NPCs
    protected virtual void UseWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            CurrentWeapon.Trigger();
        }
        else
        {
            CurrentWeapon.ReleaseTrigger();
        }
    }

    // Aims the weapon
    private void AimWeapon()
    {
        // Flipping hand position and weapon direction
        float verticalAxis = Mathf.Sign(AimPosition.x - transform.position.x);
        bool scale = !(verticalTrigger * verticalAxis > 0);
        verticalTrigger = verticalAxis;
        if (scale) CurrentWeapon.ScaleInverse();

        // Getting mouse position and direction of player to mouse
        Vector3 aimDirection = (AimPosition - CurrentWeapon.transform.position).normalized;

        // Rotating the current weapon
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        CurrentWeapon.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void ControlWeapon()
    {
        if (CurrentWeapon != null)
        {
            AimWeapon();
            AdjustSortingOrder();
            if (canShoot)
            {
                CurrentWeapon.WeaponUpdate();
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
            float distance = Vector3.Distance(AimPosition, transform.position) / 16;

            AimPosition = GetTargetPosition() + UnityEngine.Random.insideUnitSphere *
                UnityEngine.Random.Range(0, distance) * AimDeviation;
            AimPosition.z = 0f;
        }
    }

    private async UniTaskVoid GivePermissionToFire(float waitTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        canShoot = true;
    }
}