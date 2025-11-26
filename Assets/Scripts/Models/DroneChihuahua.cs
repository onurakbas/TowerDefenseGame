using UnityEngine;

public class DroneChihuahua : Enemy
{
    protected override void Start()
    {
        // === UÇAN DÜŞMAN AYARLARI ===
        enemyNameID = "Drone-Chihuahua Air";
        maxHealth = 50f; // Standart Can 
        armor = 0f; // Zırhsız
        speed = 7.5f; // %50 Daha hızlı
        reward = 15; // Ödül 
        baseDamage = 5; // Üs hasarı 

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