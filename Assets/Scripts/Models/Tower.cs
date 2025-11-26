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
    // Hedef artık Transform değil, Enemy scripti
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

    // Artık Transform yerine Enemy scriptini bulup atıyor
    protected virtual void HedefBul()
    {
        GameObject[] dusmanlar = GameObject.FindGameObjectsWithTag("Enemy");
        float enKisaMesafe = Mathf.Infinity;
        GameObject enYakinDusmanObj = null;

        foreach (GameObject dusmanObj in dusmanlar)
        {
            float mesafesi = Vector3.Distance(transform.position, dusmanObj.transform.position);
            if (mesafesi < enKisaMesafe)
            {
                enKisaMesafe = mesafesi;
                enYakinDusmanObj = dusmanObj;
            }
        }

        if (enYakinDusmanObj != null && enKisaMesafe <= range)
        {
            // Enemy scriptini çekip hedef değişkenine atıyoruz.
            hedef = enYakinDusmanObj.GetComponent<Enemy>();
        }
        else
        {
            hedef = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}