namespace General
{
    public interface IHealthInteractable
    {
        int CurrentHealth { get; set; }
        int MaxHealth { get; set; }
        bool IsDead => CurrentHealth <= 0;
        void TakeDamage(int amount);
        void GainHealth(int amount);
    }
}