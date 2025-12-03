using UnityEngine;
using UnityEngine.EventSystems; // UI engellemesi için gerekli

public class TowerTile : MonoBehaviour
{
    [Header("Görsel Ayarlar")]
    public Color hoverRengi = Color.green; // Mouse üzerine gelince parlayacak renk
    private Color baslangicRengi;          // Oyun başındaki orijinal renk (Hafif şeffaf)
    private Renderer rend;

    private Tower insaEdilenKule; // Bu karede dikili bir kule var mı?

    private void Start()
    {
        rend = GetComponent<Renderer>();
        baslangicRengi = rend.material.color; // Başlangıç rengini hafızaya al
    }

    // Mouse karenin üzerine gelince
    private void OnMouseEnter()
    {
        // 1. Eğer mouse bir butonun (UI) üzerindeyse tepki verme
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 2. Eğer burada zaten kule varsa renk değiştirme (Zaten görünmez olacak ama garanti olsun)
        if (insaEdilenKule != null) return;

        // Rengi değiştir (Parlat)
        rend.material.color = hoverRengi;
    }

    // Mouse karenin üzerinden gidince
    private void OnMouseExit()
    {
        // Eski haline (Sönük haline) dön
        if (insaEdilenKule == null)
        {
            rend.material.color = baslangicRengi;
        }
    }

    // Tıklama Anı
    private void OnMouseDown()
    {
        // UI kontrolü
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Zaten doluysa işlem yapma
        if (insaEdilenKule != null)
        {
            Debug.Log("Burada zaten kule var!");
            return;
        }

        // BuildManager'dan hangi kuleyi seçtiğimizi öğren
        Tower secilen = BuildManager.Instance.GetSecilenKule();
        if (secilen == null) return; // Hiçbir kule seçilmemişse çık

        // İnşaat fonksiyonunu çağır
        KuleyiYerlestir(secilen);
    }

    public void KuleyiYerlestir(Tower kulePrefab)
    {
        // 1. Önce GameManager'a sor: Paramız yetiyor mu?
        // (GameManager.ParaHarcama fonksiyonu true dönerse para düşülmüş demektir)
        if (GameManager.Instance.ParaHarcama(kulePrefab.Cost)) 
        {
            // 2. Kuleyi tam bu karenin üzerine yarat
            Tower yeniKule = Instantiate(kulePrefab, transform.position, Quaternion.identity);
            insaEdilenKule = yeniKule;

            // === KRİTİK DÜZELTME ===
            // Kule dikildiği an bu yeşil/mavi kareyi GÖRÜNMEZ yap.
            // Böylece kedinin altında renk kalmaz.
            rend.enabled = false; 
            
            GameManager.Instance.GunlukYaz($"Kule '{yeniKule.NameID}' inşa edildi.");
        }
        else
        {
            Debug.Log("Para Yetersiz!");
        }
    }
}
