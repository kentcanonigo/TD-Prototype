public interface IHasHealth {
    int HealthPoints { get; }
    int Armor { get; }
    void TakeDamage(int damage);
    void Kill();
    bool IsDead { get; }
}
