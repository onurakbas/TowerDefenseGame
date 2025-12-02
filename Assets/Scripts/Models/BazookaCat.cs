using UnityEngine;

public class BazookaCat : Tower
{
    [Header("Alan Hasarı Ayarı")]
    [SerializeField] private float patlamaYaricapi = 2f; // Patlama genişliği

    protected override void Start()
    {
        // KULE AYARLARI
        towerNameID = "Bazooka-Cat Heavy";
        damage = 20f;
        range = 3f;
        fireRate = 3f;
        cost = 75;

        base.Start(); // Artık Tower.cs'de Start metodu olduğu için hata vermeyecek.
    }

    // ÖZEL DURUM: Hedef bulurken Uçan Düşmanları görmezden gelmeli
    protected override void HedefBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");

        Enemy enOncelikliDusman = null;
        int enYuksekWaypointIndex = -1;
        float enKisaMesafeKuleye = Mathf.Infinity;

        foreach (GameObject dusmanObj in dusmanlar)
        {
            // Menzil Kontrolü
            float mesafe = Vector3.Distance(transform.position, dusmanObj.transform.position);
            if (mesafe > range) continue;

            Enemy dusmanScript = dusmanObj.GetComponent<Enemy>();

            // ÖZEL KURAL: Uçan düşmanları atla
            if (dusmanScript is DroneChihuahua) continue;

            // PDF KURALI: Üsse en yakın olanı seç
            if (dusmanScript.WaypointIndex > enYuksekWaypointIndex)
            {
                enYuksekWaypointIndex = dusmanScript.WaypointIndex;
                enOncelikliDusman = dusmanScript;
                enKisaMesafeKuleye = mesafe;
            }
            else if (dusmanScript.WaypointIndex == enYuksekWaypointIndex)
            {
                if (mesafe < enKisaMesafeKuleye)
                {
                    enKisaMesafeKuleye = mesafe;
                    enOncelikliDusman = dusmanScript;
                }
            }
        }

        hedef = enOncelikliDusman;
    }

    public override void AtesEt()
    {
        if (hedef == null) return;

        // ALAN HASARI (Splash Damage) Mantığı
        Collider[] vurulanlar = Physics.OverlapSphere(hedef.transform.position, patlamaYaricapi);

        GameManager.Instance.GunlukYaz($"Kule '{towerNameID}' alan atışı yaptı. Merkez Hedef: {hedef.NameID}");

        foreach (Collider kurban in vurulanlar)
        {
            if (kurban.CompareTag("Enemy"))
            {
                Enemy dusmanScript = kurban.GetComponent<Enemy>();

                if (dusmanScript != null && !(dusmanScript is DroneChihuahua))
                {
                    float netHasar = MathHelper.NetHasarHesapla(damage, dusmanScript.Armor);
                    dusmanScript.HasarAl(netHasar);

                    GameManager.Instance.GunlukYaz($"   -> Alan Hasarı: '{dusmanScript.NameID}' Net Hasar: {netHasar}");
                }
            }
        }
    }

    // Editörde patlama alanını görmek için
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (hedef != null) Gizmos.DrawWireSphere(hedef.transform.position, patlamaYaricapi);
    }
}