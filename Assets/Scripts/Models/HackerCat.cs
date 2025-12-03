using UnityEngine;

public class HackerCat : Tower
{
    protected override void Start()
    {
        towerNameID = "Hacker-Cat WiFi";
        damage = 15f;         // Düşük Hasar
        range = 3.5f;        // İyi Menzil
        fireRate = 2f;       // Orta Hız (2 saniyede 1)
        cost = 70;
    }

    public override void AtesEt()
    {
        if (hedef == null) return;

        // 1. Hasar Ver
        float netHasar = MathHelper.NetHasarHesapla(damage, hedef.Armor);
        hedef.HasarAl(netHasar);

        // 2. Yavaşlatma Efekti Uygula
        hedef.HizDusur(0.5f, 3.0f); // %50 Hız, 3 Saniye
        GameManager.Instance.GunlukYaz($"Kule '{towerNameID}' -> '{hedef.NameID}' hacklendi. Net Hasar: {netHasar}, Yavaşlatma uygulandı.");
    }
}