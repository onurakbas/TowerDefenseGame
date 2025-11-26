using UnityEngine;

public class BazookaCat : Tower
{
    [Header("Alan Hasarı Ayarı")]
    [SerializeField] private float patlamaYaricapi = 2f; // Patlama genişliği

    private void Start()
    {
        towerNameID = "Bazooka-Cat Heavy";
        damage = 30f;        // Yüksek Hasar
        range = 3f;          // Orta Menzil
        fireRate = 3f;       // Çok Yavaş (3 saniyede 1)
        cost = 75;           // Pahalı
    }

    // ÖZEL DURUM: Hedef bulurken Uçan Düşmanları görmezden gelmeli
    protected override void HedefBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");
        float enKisaMesafe = Mathf.Infinity;
        GameObject enYakinDusman = null;

        foreach (GameObject dusmanObj in dusmanlar)
        {
            // Eğer düşman "DroneChihuahua" (Uçan) ise onu pas geç
            Enemy dusmanScript = dusmanObj.GetComponent<Enemy>();
            if (dusmanScript is DroneChihuahua) continue;

            float mesafe = Vector3.Distance(transform.position, dusmanObj.transform.position);
            if (mesafe < enKisaMesafe)
            {
                enKisaMesafe = mesafe;
                enYakinDusman = dusmanObj;
            }
        }

        if (enYakinDusman != null && enKisaMesafe <= range)
        {
            hedef = enYakinDusman.transform;
        }
        else
        {
            hedef = null;
        }
    }

    public override void AtesEt()
    {
        if (hedef == null) return;

        // ALAN HASARI (Splash Damage) Mantığı
        // Hedefin etrafındaki herkesi bul
        Collider[] vurulanlar = Physics.OverlapSphere(hedef.transform.position, patlamaYaricapi);

        foreach (Collider kurban in vurulanlar)
        {
            // Sadece "Enemy" etiketli olanlara hasar ver
            if (kurban.CompareTag("Enemy"))
            {
                Enemy dusmanScript = kurban.GetComponent<Enemy>();

                // Uçan düşmanlar patlamadan etkilenmez (Proje kuralı)
                if (dusmanScript != null && !(dusmanScript is DroneChihuahua))
                {
                    float netHasar = MathHelper.NetHasarHesapla(damage, dusmanScript.Armor);
                    dusmanScript.HasarAl(netHasar);
                }
            }
        }
        // Debug.Log("BOOM! Alan hasarı verildi.");
    }

    // Editörde patlama alanını görmek için
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (hedef != null) Gizmos.DrawWireSphere(hedef.transform.position, patlamaYaricapi);
    }
}