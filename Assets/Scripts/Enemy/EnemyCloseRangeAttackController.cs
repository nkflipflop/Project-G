using General;
using UnityEngine;

public class EnemyCloseRangeAttackController : MonoBehaviour
{
    [SerializeField] private CircleCollider2D attackRangeCollider;
    [SerializeField] private int damage = 3;
    private bool canAttack = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canAttack && other.gameObject.CompareTag("Player"))
        {
            canAttack = false;
            other.gameObject.GetComponent<IHealthInteractable>().TakeDamage(damage);
        }
    }

    public void EnableCollider()
    {
        attackRangeCollider.enabled = true;
    }

    public void DisableCollider()
    {
        attackRangeCollider.enabled = false;
        canAttack = true;
    }
}