using UnityEngine;

public class MechaBulldog : Enemy
{
    protected override void Start()
    {
        // === ZIRHLI DÜŞMAN AYARLARI ===
        enemyNameID = "Mecha-Bulldog MK2";
        maxHealth = 75f; [cite_start]// %50 Daha fazla can [cite: 55]
        armor = 100f; [cite_start]// Zırh var! [cite: 55]
        speed = 2.5f; [cite_start]// %50 Daha yavaş [cite: 55]
        reward = 20; [cite_start]// Ödül [cite: 56]
        baseDamage = 10; [cite_start]// Üsse daha çok hasar verir [cite: 57]

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
        Debug.Log($"{enemyNameID} ağır hasarla patladı! Ödül: {reward}");
        Destroy(gameObject);
    }

    public override void UsseSaldir()
    {
        Debug.Log($"{enemyNameID} güvenlik duvarını yıktı! Hasar: {baseDamage}");
        Destroy(gameObject);
    }
}