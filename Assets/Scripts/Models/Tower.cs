using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Tower : MonoBehaviour
{
    [Header("Kule Özellikleri")]
    [SerializeField] protected string towerNameID;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float fireRate;
    [SerializeField] protected int cost;

    protected float fireCountdown = 0f;
    protected Enemy hedef;

    public int Cost => cost;
    public string NameID => towerNameID;

    protected virtual void Start()
    {
        // Alt sınıfların (BazookaCat vb.) base.Start() çağırabilmesi için eklendi.
    }

    protected virtual void Update()
    {
        // Hedef yoksa veya menzilden çıktıysa (Pozisyon kontrolü) yeni hedef ara
        if (hedef == null || Vector3.Distance(transform.position, hedef.transform.position) > range)
        {
            HedefBul();
        }

        if (hedef != null)
        {
            if (fireCountdown <= 0f)
            {
                AtesEt();
                // Atış hızına göre sonraki zamanı ayarla (fireRate saniye cinsindense)
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    public abstract void AtesEt();

    // Tower.cs içindeki HedefBul metodu:
    protected virtual void HedefBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");

        Enemy enOncelikliDusman = null;
        // En yüksek index = Üsse en yakın demek (Yolun sonuna yaklaşmış)
        int enYuksekWaypointIndex = -1;
        float enKisaMesafeKuleye = Mathf.Infinity; // Eşitlik durumunda kuleye yakına bakarız

        foreach (GameObject dusmanObj in dusmanlar)
        {
            // Menzil Kontrolü (Kuleye uzaklığı range'den büyükse hesaba katma)
            float mesafe = Vector3.Distance(transform.position, dusmanObj.transform.position);
            if (mesafe > range) continue;

            Enemy dusmanScript = dusmanObj.GetComponent<Enemy>();

            // PDF KURALI: Üsse en yakın olan (Waypoint indexi en büyük olan) önceliklidir.
            if (dusmanScript.WaypointIndex > enYuksekWaypointIndex)
            {
                enYuksekWaypointIndex = dusmanScript.WaypointIndex;
                enOncelikliDusman = dusmanScript;
                enKisaMesafeKuleye = mesafe;
            }
            // Eğer aynı noktadalarsa, kuleye daha yakın olanı seç (İkincil kriter)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}