namespace Prototype.Scripts
{
    public interface IDamageable
    {
        float health { get; set; }

        public void Damage(float dmg);
    }
}

