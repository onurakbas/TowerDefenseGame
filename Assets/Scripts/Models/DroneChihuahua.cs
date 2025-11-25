using UnityEngine;

public class DroneChihuahua : Enemy
{
    protected override void Start()
    {
        // === UÇAN DÜŞMAN AYARLARI ===
        enemyNameID = "Drone-Chihuahua Air";
        maxHealth = 50f; [cite_start]// Standart Can [cite: 60]
        armor = 0f; [cite_start]// Zırhsız [cite: 60]
        speed = 7.5f; [cite_start]// %50 Daha hızlı [cite: 60]
        reward = 15; [cite_start]// Ödül [cite: 62]
        baseDamage = 5; [cite_start]// Üs hasarı [cite: 63]

        base.Start();
    }

    public override void HasarAl(float miktar)
    {
        currentHealth -= miktar;

        if (currentHealth <= 0)
        {
            Ol();
        }
    }

    public override void Ol()
    {
        Debug.Log($"{enemyNameID} sinyali kesildi! Ödül: {reward}");
        Destroy(gameObject);
    }

    public override void UsseSaldir()
    {
        Debug.Log($"{enemyNameID} havadan sızdı! Hasar: {baseDamage}");
        Destroy(gameObject);
    }
}