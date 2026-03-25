# Cats vs. Dogs: Cyber-Pet Tower Defense

**Kedi-Köpek / Siber-Evcil Hayvan Temalı 2D Kule Savunma Oyunu Projesi**

![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-black.svg?logo=unity)

Bu proje, Unity oyun motoru kullanılarak geliştirilmiş, oyuncuların bir üssü gelen düşman dalgalarına karşı savunmak için çeşitli kuleler inşa ettiği klasik bir 2D Kule Savunma (Tower Defense) oyunudur.

## 🏗️ Mimari ve Tasarım

Proje geliştirilirken **Model-View-Controller (MVC) Tasarım Deseni** benimsenmiştir. Bu sayede veriler, oyun mantığı (logic) ve arayüz görselleri (view) birbirinden izole edilerek temiz bir kod yapısı oluşturulmuştur.

- `Controllers`: Oyunun çekirdek mekanikleri, girdi yönetimi ve oyun durumu (GameManager, BuildManager).
- `Models`: Oyundaki varlıkların (kuleler, düşmanlar) özellikleri ve davranışları (Tower, Enemy vb.).
- `Views`: Arayüz (UI) güncellemeleri ve görsel bildirimler (HealthBarView, CurrencyView).

## ⚙️ Oynanış ve Temel Mekanikler

- **Ekonomi ve Sağlık Yönetimi:** Oyuna belirli bir bütçe (örn: 200 birim) ve üs sağlığı (örn: 100 birim) ile başlanmaktadır. Kule inşa etmek bakiye harcarken, öldürülen her düşman oyuncuya para kazandırır. Üs sağlığı 0'a düştüğünde oyun kaybedilir ("SİSTEM HACKLENDİ").
- **İnşa Sistemi (Grid/Tile):** Oyuncular `BuildManager` aracılığıyla seçtikleri kuleleri haritada belirlenmiş özel `TowerTile` noktalarına yerleştirebilirler.
- **Dalga (Wave) Sistemi:** Düşmanlar `GameManager` tarafından yönetilen belirli bir dalga yapısında (`Dalga` struct) ve aralıklarla doğarlar (Spawn).
- **Yapay Zeka (AI) Hedefleme:** Kuleler sadece en yakındaki düşmana ateş etmek yerine, yolda en çok ilerlemiş (hedefe en yakın) düşmana öncelik verir. Eşitlik durumunda en yakın mesafedekini seçerler.
- **Simülasyon Logları:** Oyundaki önemli olaylar (kule satın alma, hasar hesaplamaları, dalga tamamlanması) detaylı bir şekilde o anki zaman damgasıyla `savunma_gunlugu.txt` adlı dosyaya kayıt (log) edilir.

## 🐱 Kuleler (Savunmacılar)

Oyunda her biri farklı bir stratejik amaca hizmet eden 3 temel kule (Kedi) bulunmaktadır:

1.  **Sniper-Cat:** Yüksek hasar veren, tek hedefli kule. Zırhlı hedeflere karşı %50 daha az hasar verme dezavantajı vardır.
2.  **Bazooka-Cat:** Alan etkili (AoE) hasar vurur. Saldırıları belli bir çaptaki tüm düşmanlara zarar verir. Ancak uçan (hava) birimlerine (`DroneChihuahua`) saldıramaz ve isabet ettiremez.
3.  **Hacker-Cat:** Hasar vermekten ziyade destek birimidir (Destek/Utility). Hedeflenen düşmana `HizDusur` zayıflatması (debuff) uygulayarak hızlarını belirli bir süreliğine %50 oranında yavaşlatır.

## 🐶 Düşmanlar (Saldırganlar)

Belirli rota noktalarını (Waypoints) takip ederek oyuncu üssüne ulaşmaya çalışan düşman tipleri (Köpekler/Robotlar):

1.  **Robo-Pug:** Standart karasal düşman birimidir. Zırhsızdır, dengeli hız ve sağlığa sahiptir.
2.  **Drone-Chihuahua:** Uçan (Hava) düşman ünitesidir. `RoboPug`'dan daha hızlıdır (%50 daha hızlı) ve `Bazooka-Cat`'in alan hasarlarından etkilenmez.
3.  **Mecha-Bulldog:** Oyunun "Tank" ünitesidir. Sahip olduğu zırh (Armor) değeri sayesinde `Sniper-Cat` gibi birimlerden daha az hasar alır.

## 🛠️ Kullanılan Teknolojiler

Bu proje herhangi bir üçüncü taraf (third-party) harici kütüphane kullanılmadan, tamamen Unity'nin yerleşik paketleriyle ve **C#** ile geliştirilmiştir:

- **Unity 2D Paketleri:** (Tilemap, SpriteShape, PixelPerfect vb.)
- **TextMeshPro:** UI metinleri ve gösterimler için.

---

_Not: Bu proje, kod içi yorumlardan anlaşıldığı üzere ("PDF KURALI", "Proje İsteri" vb.) belirli kurallara ve gereksinimlere dayalı yapılandırılmış akademik/eğitim amaçlı bir tasarıma sahiptir._
