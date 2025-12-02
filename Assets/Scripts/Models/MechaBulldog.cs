using UnityEngine;

public class MechaBulldog : Enemy
{
    protected override void Start()
    {
        // === ZIRHLI DÜŞMAN AYARLARI ===
        enemyNameID = "Mecha-Bulldog MK2";
        maxHealth = 75f; // %50 Daha fazla can 
        armor = 100f; // Zırh var! 
        speed = 2.5f; // %50 Daha yavaş 
        reward = 20; // Ödül 
        baseDamage = 10; // Üsse daha çok hasar verir 

        base.Start();
    }

    public override void HasarAl(float miktar)
    {
        // NOT: Zırh formülü burada değil, kule tarafında MathHelper ile hesaplanıp buraya "Net Hasar" olarak gelecek.
        currentHealth -= miktar;

        if (currentHealth <= 0)
        {
            Ol();
        }
    }

    public override void Ol()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ParaEkle(reward);
        }
        Debug.Log($"{enemyNameID} ağır hasarla patladı! Ödül: {reward}");
        Destroy(gameObject);
    }

    public override void UsseSaldir()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.HasarAl(baseDamage);
        }
        Debug.Log($"{enemyNameID} güvenlik duvarını yıktı! Hasar: {baseDamage}");
        Destroy(gameObject);
    }
}