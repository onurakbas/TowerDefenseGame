using UnityEngine;

public class SniperCat : Tower
{
    protected override void Start()
    {
        // === İSTATİSTİKLER ===
        towerNameID = "Sniper-Cat v1";
        damage = 20f;        // Taban Hasar
        range = 4f;          // Uzun Menzil
        fireRate = 1f;       // Saniyede 1 Atış
        cost = 50;           // Maliyet
    }

    // Polimorfizm: AtesEt fonksiyonunu kendine özel yazıyoruz
    public override void AtesEt()
    {
        if (hedef == null) return;

        // 1. KURAL: Zırhlı düşmana (MechaBulldog) %50 daha az vurur
        float uygulanacakHasar = damage;

        if (hedef.Armor > 0) // Eğer düşmanın zırhı varsa
        {
            uygulanacakHasar = damage * 0.5f; // Hasarı yarıya indir
            // Debug.Log("Zırhlı hedef tespit edildi! Hasar düşürüldü.");
        }

        // 2. KURAL: Matematik Motorunu çağırıp Net Hasarı hesapla
        float netHasar = MathHelper.NetHasarHesapla(uygulanacakHasar, hedef.Armor);

        // 3. Hedefe hasar ver
        hedef.HasarAl(netHasar);

        // İleride buraya Lazer Sesi/Efekti eklenecek
    }
}