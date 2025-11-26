using UnityEngine;

public class BazookaCat : Tower
{
    [Header("Alan Hasarı Ayarı")]
    [SerializeField] private float patlamaYaricapi = 2f; // Patlama genişliği

    protected override void Start()
    {
        // KULE AYARLARI
        towerNameID = "Bazooka-Cat Heavy";
        damage = 30f;
        range = 3f;
        fireRate = 3f;
        cost = 75;

        base.Start(); // Artık Tower.cs'de Start metodu olduğu için hata vermeyecek.
    }

    // ÖZEL DURUM: Hedef bulurken Uçan Düşmanları görmezden gelmeli
    protected override void HedefBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");
        float enKisaMesafe = Mathf.Infinity;
        GameObject enYakinDusmanObj = null;

        foreach (GameObject dusmanObj in dusmanlar)
        {
            Enemy dusmanScript = dusmanObj.GetComponent<Enemy>();

            // Uçan düşmanları atla
            if (dusmanScript is DroneChihuahua) continue;

            float mesafe = Vector3.Distance(transform.position, dusmanObj.transform.position);
            if (mesafe < enKisaMesafe)
            {
                enKisaMesafe = mesafe;
                enYakinDusmanObj = dusmanObj;
            }
        }

        if (enYakinDusmanObj != null && enKisaMesafe <= range)
        {
            // Enemy component'ini çekip hedef değişkenine atıyoruz.
            hedef = enYakinDusmanObj.GetComponent<Enemy>();
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
        Collider[] vurulanlar = Physics.OverlapSphere(hedef.transform.position, patlamaYaricapi);

        foreach (Collider kurban in vurulanlar)
        {
            if (kurban.CompareTag("Enemy"))
            {
                Enemy dusmanScript = kurban.GetComponent<Enemy>();

                if (dusmanScript != null && !(dusmanScript is DroneChihuahua))
                {
                    float netHasar = MathHelper.NetHasarHesapla(damage, dusmanScript.Armor);
                    dusmanScript.HasarAl(netHasar);
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