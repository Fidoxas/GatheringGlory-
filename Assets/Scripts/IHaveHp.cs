public interface IHaveHp
{
    public void TakeDamage(int amount,Player player);
    public void ReloadHp();
    public bool IsAlive();
}