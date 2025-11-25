using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Kule Özellikleri")]
    [SerializeField] protected string towerNameID;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float fireRate;
    [SerializeField] protected int cost;

    protected float fireCountdown = 0f;
    protected Transform hedef; 

    public int Cost => cost;

    protected virtual void Update()
    {
        // Hedef yoksa veya menzilden çıktıysa yeni hedef ara
        if (hedef == null || Vector3.Distance(transform.position, hedef.position) > range)
        {
            HedefBul(); 
        }

        if (hedef != null)
        {
            if (fireCountdown <= 0f)
            {
                AtesEt(); 
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    // === SOYUT METOTLAR ===
    // AtesEt: Her kule kendi ateş etme şeklini buraya yazacak
    public abstract void AtesEt();

    // === HEDEF BULMA ===
    // HedefBul: En yakın düşmanı seçer
    protected virtual void HedefBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");
        float enKisaMesafe = Mathf.Infinity;
        GameObject enYakinDusman = null;

        foreach (GameObject dusman in dusmanlar)
        {
            float mesafesi = Vector3.Distance(transform.position, dusman.transform.position);
            if (mesafesi < enKisaMesafe)
            {
                enKisaMesafe = mesafesi;
                enYakinDusman = dusman;
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

    // Menzili sahnede çizdiren yardımcı (Gizmos)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}