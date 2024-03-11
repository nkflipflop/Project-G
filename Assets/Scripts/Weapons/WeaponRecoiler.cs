using UnityEngine;

public class WeaponRecoiler : MonoBehaviour
{
    [SerializeField] private float maxOffsetDistance = 0.2f;
    [SerializeField] private float recoilAcceleration = 50f;
    [SerializeField] private float weaponRecoilStartSpeed = -10f;

    private bool _recoilInEffect = false;
    private bool _weaponHeadedBackToStartPosition = false;

    private Vector3 _offsetPosition = Vector3.zero;
    private Vector3 _recoilSpeed = Vector3.zero;

    public void AddRecoil()
    {
        _recoilInEffect = true;
        _weaponHeadedBackToStartPosition = false;

        _recoilSpeed = transform.right * weaponRecoilStartSpeed;
    }

    private void UpdateRecoil()
    {
        // Set up speed and then position
        _recoilSpeed += (-_offsetPosition.normalized) * recoilAcceleration * Time.deltaTime;
        Vector3 newOffsetPosition = _offsetPosition + _recoilSpeed * Time.deltaTime;
        Vector3 newTransformPosition = transform.position - _offsetPosition;

        if (newOffsetPosition.magnitude > maxOffsetDistance)
        {
            _recoilSpeed = Vector3.zero;
            _weaponHeadedBackToStartPosition = true;
            newOffsetPosition = _offsetPosition.normalized * maxOffsetDistance;
        }
        else if (_weaponHeadedBackToStartPosition == true && newOffsetPosition.magnitude > _offsetPosition.magnitude)
        {
            transform.position -= _offsetPosition;
            _offsetPosition = Vector3.zero;

            // Set up our boolean
            _recoilInEffect = false;
            _weaponHeadedBackToStartPosition = false;

            return;
        }

        transform.position = newTransformPosition + newOffsetPosition;
        _offsetPosition = newOffsetPosition;
    }


    private void Update()
    {
        if (_recoilInEffect)
        {
            UpdateRecoil();
        }
    }
}