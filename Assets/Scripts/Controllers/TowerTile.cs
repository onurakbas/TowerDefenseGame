using UnityEngine;
using UnityEngine.EventSystems; // UI'a tıklayınca haritaya tıklamayı engellemek için

public class TowerTile : MonoBehaviour
{
	[Header("Görsel Ayarlar")]
	public Color hoverRengi = Color.gray; // Mouse üzerine gelince renk değişsin
	private Color baslangicRengi;
	private Renderer rend;

	private Tower insaEdilenKule; // Burada zaten bir kule var mı?

	private void Start()
	{
		rend = GetComponent<Renderer>();
		baslangicRengi = rend.material.color;
	}

	// Unity Event: Mouse üzerine gelince
	private void OnMouseEnter()
	{
		// Eğer bir butona (UI) denk geliyorsa rengi değiştirme
		if (EventSystem.current.IsPointerOverGameObject()) return;

		rend.material.color = hoverRengi;
	}

	// Unity Event: Mouse üzerinden gidince
	private void OnMouseExit()
	{
		rend.material.color = baslangicRengi;
	}

	// Unity Event: Tıklayınca
	private void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;

		// EĞER KULE VARSA ÇIK
		if (insaEdilenKule != null)
		{
			Debug.Log("Burada zaten kule var!");
			return;
		}

		// BUILD MANAGER'DAN SEÇİLİ KULEYİ AL
		Tower secilen = BuildManager.Instance.GetSecilenKule();
		if (secilen == null) return; // Kule seçmemiş

		// İNŞA ET
		KuleyiYerlestir(secilen);
	}

	// === TÜRKÇE FONKSİYON ===
	// Bu fonksiyonu "GameManager" veya UI Butonu çağıracak
	public void KuleyiYerlestir(Tower kulePrefab)
	{
		// Dolu mu kontrol et
		if (insaEdilenKule != null)
		{
			Debug.Log("Yer dolu!");
			return;
		}

		// GameManager üzerinden inşaat yap (Para kontrolü orada)
		// Not: KuleInsaEt fonksiyonunu bool döndürecek şekilde güncellersek daha iyi olur
		// ama şimdilik doğrudan çağırıyoruz.
		GameManager.Instance.KuleInsaEt(kulePrefab, transform.position);

		// Basitçe burayı dolu işaretle (Prefab referansı tutuyoruz şimdilik)
		insaEdilenKule = kulePrefab;
	}
}