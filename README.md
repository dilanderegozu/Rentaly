# 🚗 Rentaly - Kurumsal Araç Kiralama Yönetim Sistemi

**Rentaly**, ASP.NET Core MVC ve N-Tier (Katmanlı) Mimari kullanılarak geliştirilmiş, hem müşteri tarafı hem de kapsamlı bir yönetim paneli barındıran uçtan uca bir araç kiralama platformudur. Proje; katmanlı mimari, ilişkisel veri modelleme ve gerçek iş mantığı (fiyatlandırma, müsaitlik durumu, filo analitiği) üzerine kurgulanmıştır.

---

## 🚀 Teknolojiler ve Mimari Altyapı

### 🛠️ Teknik Yığın (Tech Stack)
* **Framework:** ASP.NET Core MVC (.NET 10)
* **Veritabanı:** MSSQL Server & Entity Framework Core (Code First + Migrations)
* **Validasyon:** FluentValidation
* **Mapping:** AutoMapper
* **Raporlama:** ClosedXML (Excel) & QuestPDF (PDF)
* **Frontend (Admin Panel):** Tailwind CSS, Material Symbols
* **Frontend (Kullanıcı Tarafı):** Özelleştirilmiş HTML/CSS şablonu

### 🏗️ Kurumsal Mimari Yapısı (N-Tier Architecture)
Proje 4 ana katman üzerine inşa edilmiştir:

1. **Rentaly.WebUI:** Sunum katmanı. **Areas (Admin)** yapısı, **ViewComponent** kullanımı ve asenkron Controller'lar ile optimize edilmiştir.
2. **Rentaly.BusinessLayer:** İş mantığının yürütüldüğü katman. `Abstract (Services)` ve `Concreate (Managers)` ayrımıyla servis odaklı çalışır.
3. **Rentaly.DataAccessLayer:** Veri erişim katmanı. **Generic Repository Pattern** ile merkezi ve tekrar kullanılabilir bir veri erişim noktası sağlanmıştır.
4. **Rentaly.EntityLayer:** Veritabanı tablolarına karşılık gelen, framework'ten bağımsız POCO sınıfları barındırır.

## 💎 Uygulanan Design Patterns & Prensipler

* **Generic Repository Pattern:** `IGenericDal<T>` / `GenericRepository<T>` üzerinden merkezi CRUD işlemleri; her entity kendi ihtiyacına özel `Include()` zincirine sahip ek metodlarla genişletilmiştir (örn. `GetCarWithDetailsByIdAsync`).
* **Service (Manager) Katmanı:** Controller'lar veritabanına asla doğrudan erişmez; her işlem `CarManager`, `BookingManager` gibi servisler üzerinden yürütülür.
* **Dependency Injection:** Tüm servis ve repository bağımlılıkları `Program.cs` üzerinden gevşek bağlı (loosely coupled) şekilde yönetilir.
* **ViewModel Kullanımı:** Formlar ve liste sayfaları asla doğrudan Entity'e bağlanmaz; her sayfa kendi ihtiyacına özel bir ViewModel (`CarModelRowViewModel`, `BookingListViewModel`, `ReportsViewModel` vb.) ile beslenir.
* **Alan Bazlı Görselleştirme (Display Helper Pattern):** Araç durumu, aktivite ikonları gibi tekrar eden görsel mantıklar (`CarStatusDisplayHelper`, `ActivityDisplayHelper`) tek bir yerden yönetilir.

---

## 🖼️ Kullanıcı Arayüzü ve Dinamik Yönetim



<img width="1884" height="879" alt="Ekran görüntüsü 2026-07-20 204224" src="https://github.com/user-attachments/assets/773bcb61-112f-450a-8528-0ebca1d958dd" />
<img width="1890" height="865" alt="Ekran görüntüsü 2026-07-20 204238" src="https://github.com/user-attachments/assets/c77471e9-597d-47a6-86dc-d96a635408d5" />
<img width="1890" height="864" alt="Ekran görüntüsü 2026-07-20 204248" src="https://github.com/user-attachments/assets/d6039a4a-528c-4c22-bc67-6f9f4f769729" />
<img width="1894" height="877" alt="Ekran görüntüsü 2026-07-20 204331" src="https://github.com/user-attachments/assets/ee57bd95-6c1c-48f7-8c81-c54a0d0dc6dc" />

### 🏎️ 1. Araç Detay Sayfası ve Galeri
Kullanıcıların seçtikleri araç hakkında tüm teknik detaylara ulaştığı sayfadır.
<img width="1889" height="653" alt="Ekran görüntüsü 2026-07-20 204358" src="https://github.com/user-attachments/assets/87e2c5af-68e4-4a2a-8142-c506ce117097" />
<img width="1886" height="867" alt="Ekran görüntüsü 2026-07-20 204424" src="https://github.com/user-attachments/assets/f8997594-218c-47c5-930a-4bbbddd5973a" />
<img width="1896" height="867" alt="Ekran görüntüsü 2026-07-20 204436" src="https://github.com/user-attachments/assets/d69ab637-f603-4def-824e-efb73e85c603" />


* **Dinamik Görsel Galerisi:** Ayrı bir `CarImage` tablosu üzerinden çoklu görsel desteği — tıklanabilir thumbnail'ler ve ana önizleme görseli. Galeri boşsa aracın tekil kapak görseline otomatik geri döner.
* **Teknik Özellik Tablosu:** Kategori, araç tipi, koltuk/bagaj kapasitesi, yakıt tipi, kilometre, şube ve müsaitlik durumu veritabanından anlık olarak listelenir.
* **"Araç Hakkında" Bölümü:** Aracın tanıtım metni, galeri görselinin hemen altında ayrı bir kart olarak sunulur.

<img width="1896" height="872" alt="Ekran görüntüsü 2026-07-20 204446" src="https://github.com/user-attachments/assets/5b5345b6-adb0-4985-9363-df3f7a229f6a" />
<img width="1882" height="873" alt="Ekran görüntüsü 2026-07-20 204459" src="https://github.com/user-attachments/assets/f48c3179-e328-4a32-aa46-f223aa148e8d" />

### 💳 2. Dinamik Rezervasyon Sistemi ve Otomatik Fiyat Hesaplama
* **Özel Araç Seçim Menüsü:** Native `<select>` yerine, görsel + araç adı + fiyat gösteren özel bir dropdown yapısı.
* **Otomatik Ücret Hesaplama:** Alış ve dönüş tarihleri seçildiği anda `(Gün Sayısı × Günlük Ücret)` formülüyle toplam tutar anlık olarak hesaplanır ve kullanıcıya gösterilir.
* **Sunucu Taraflı Doğrulama:** İstemci tarafında hesaplanan tutara güvenilmez — sunucu, rezervasyon kaydedilmeden önce toplam tutarı veritabanındaki güncel fiyatla **bağımsız olarak yeniden hesaplar**. Bu, istemci tarafı manipülasyonuna karşı bilinçli bir güvenlik önlemidir.
* **Rezervasyon Onay Sayfası:** Rezervasyon numarası, araç özeti, tarih/şube bilgisi ve toplam tutarı içeren bir onay ekranı.

<img width="1892" height="864" alt="Ekran görüntüsü 2026-07-20 204555" src="https://github.com/user-attachments/assets/4233a3fb-b283-4ff3-83a7-6f3702c81c4d" />
<img width="1883" height="862" alt="Ekran görüntüsü 2026-07-20 205339" src="https://github.com/user-attachments/assets/70a65835-dd31-4bfb-8c1e-59a2251af5cb" />
<img width="1889" height="849" alt="Ekran görüntüsü 2026-07-20 205348" src="https://github.com/user-attachments/assets/17af1ff3-d478-41c0-bd76-2aa8194510c6" />


### 📑 3. Filtrelenebilir Araç Listeleme
* **Çok Boyutlu Filtreleme:** Marka, model, kategori ve serbest metin (marka/model/plaka) araması bir arada kullanılabilir.
* **Sunucu Taraflı Sayfalama:** Filtre ve sayfa numarası birlikte URL'de taşınır; sayfa değiştirince aktif filtre kaybolmaz.
* **Durum Rozetleri:** Müsait / Kirada / Bakımda durumları, tek bir merkezi yardımcı sınıf üzerinden tutarlı renk ve etiketle gösterilir.

<img width="1886" height="867" alt="Ekran görüntüsü 2026-07-20 204424" src="https://github.com/user-attachments/assets/3a911b7a-9391-404b-833f-285b61e18449" />

---

## 🛠️ Gelişmiş Admin Yönetim Paneli

### 📈 1. Sistem Panoraması (Dashboard)
* **Canlı İstatistik Kartları:** Toplam araç, toplam rezervasyon ve toplam şube sayısı gerçek verilerden hesaplanır.
* **Filo Büyüme Grafiği:** Harici bir kütüphaneye bağımlı olmadan, el yapımı SVG çizgi/alan grafiği ile aracın sisteme eklenme tarihine (`CreatedDate`) göre aylık kümülatif filo büyümesi gösterilir; 6/12 aylık aralık seçilebilir.
* **Son Aktiviteler:** Bağımsız bir `ViewComponent` olarak çalışan, kendi `Activity` tablosundan beslenen ve "2 dakika önce" gibi göreceli zaman etiketleri üreten bir akış.

<img width="1907" height="939" alt="Ekran görüntüsü 2026-07-21 125544" src="https://github.com/user-attachments/assets/91e2c365-0cbb-442f-a992-efca06d4413f" />


### 🚜 2. Araç ve Filo Yönetimi
* **Akıllı Filtreleme + Sayfalama:** Marka, model, kategori bazlı filtreleme; sayfa başına 5 kayıt gösterimi.
* **Esnek Görsel Yönetimi:** Yeni araç eklerken hem **dosya yükleme** hem de **görsel URL'i yapıştırma** desteklenir, ikisi için de anlık önizleme mevcuttur; hiçbiri girilmezse otomatik bir placeholder görsele düşülür.
* **Durum Yönetimi:** Araç durumu (`Available` / `Rented` / `Maintenance`) bir enum ile tek noktadan yönetilir — iki ayrı boolean alanla karışıklık yaratmak yerine net bir durum makinesi kullanılmıştır.

<img width="1916" height="938" alt="Ekran görüntüsü 2026-07-21 125555" src="https://github.com/user-attachments/assets/e57f31ed-76e6-4744-9460-4efec7f0d785" />
<img width="1912" height="942" alt="Ekran görüntüsü 2026-07-21 125614" src="https://github.com/user-attachments/assets/2e70dbc2-d052-4a2d-bed2-57071f63f5a6" />
<img width="1904" height="914" alt="Ekran görüntüsü 2026-07-21 141326" src="https://github.com/user-attachments/assets/5d276006-8960-42ec-903b-19af75689555" />
<img width="1901" height="913" alt="Ekran görüntüsü 2026-07-21 143132" src="https://github.com/user-attachments/assets/17b77d02-3d6f-4f3e-a83d-fb33eaa82d43" />
<img width="1899" height="903" alt="Ekran görüntüsü 2026-07-21 144433" src="https://github.com/user-attachments/assets/77ef8cb4-a2c9-40ba-af52-bca34b66b2bc" />
<img width="1913" height="903" alt="Ekran görüntüsü 2026-07-21 144458" src="https://github.com/user-attachments/assets/e9792b48-af71-4b7f-a1c5-30aa9a5087f3" />


### 🔖 3. Marka, Model, Kategori ve Şube Yönetimi
* Her modül; **listeleme, ekleme, düzenleme ve silme** işlemlerini tam olarak destekler.
* Listelerdeki metrikler (marka başına araç sayısı, filo payı %, kategori taban fiyatı, şube doluluk oranı) **canlı olarak ilişkisel verilerden hesaplanır**, ayrı bir alanda tekrar saklanmaz.
* Yönetici avatarları, dış bir görsele bağımlı olmadan isim baş harflerinden otomatik üretilir.

<img width="1908" height="913" alt="Ekran görüntüsü 2026-07-21 153305" src="https://github.com/user-attachments/assets/20cba98a-1683-41ca-b6bf-33a2c34f0102" />
<img width="1908" height="914" alt="Ekran görüntüsü 2026-07-21 151640" src="https://github.com/user-attachments/assets/749e22f9-b755-48f9-8f53-1bf06232a713" />
<img width="1911" height="912" alt="Ekran görüntüsü 2026-07-21 153345" src="https://github.com/user-attachments/assets/c33c9209-0142-4f3c-9433-3c4e22d860ce" />
<img width="1907" height="917" alt="Ekran görüntüsü 2026-07-21 160051" src="https://github.com/user-attachments/assets/4fc4f901-bab2-42f7-bf2b-d10b582b47a5" />
<img width="1903" height="902" alt="Ekran görüntüsü 2026-07-21 160103" src="https://github.com/user-attachments/assets/fb6677ef-b48d-435c-89ca-58a3b63466fe" />
<img width="1906" height="915" alt="Ekran görüntüsü 2026-07-21 160126" src="https://github.com/user-attachments/assets/0142872f-6554-4a03-89fd-1ac01fe23d22" />
<img width="1895" height="911" alt="Ekran görüntüsü 2026-07-21 161353" src="https://github.com/user-attachments/assets/00fdf4a9-e034-4185-9467-92591608e5d2" />
<img width="1890" height="913" alt="Ekran görüntüsü 2026-07-21 161405" src="https://github.com/user-attachments/assets/525b4c7f-ed32-4aed-a1d5-8ec5ea499992" />
<img width="1895" height="911" alt="Ekran görüntüsü 2026-07-21 161414" src="https://github.com/user-attachments/assets/6836d672-e655-48da-be8d-d5137b496119" />
<img width="1897" height="905" alt="Ekran görüntüsü 2026-07-21 162118" src="https://github.com/user-attachments/assets/8ec23f99-9a76-4502-b67a-47da07a0ba31" />
<img width="1899" height="904" alt="Ekran görüntüsü 2026-07-21 162130" src="https://github.com/user-attachments/assets/5f238e7d-acb6-4d2e-9444-1fdddbefc335" />
<img width="1887" height="915" alt="Ekran görüntüsü 2026-07-21 162139" src="https://github.com/user-attachments/assets/4052f23b-1e37-4df6-9ca8-7d3ee8514cdf" />


### 👥 4. Rezervasyon Yönetimi
* **Otomatik Durum Belirleme:** Rezervasyon durumu (Onaylandı / Devam Ediyor / Tamamlandı / İptal Edildi) tarihlerden **anlık olarak türetilir** — sadece "İptal Edildi" durumu elle işaretlenir, geri kalanı hiçbir arka plan görevine ihtiyaç duymadan otomatik değişir.
* **Detay Görüntüleme ve İptal:** Her rezervasyon için ayrı bir detay sayfası (araç, müşteri, tarih, şube, ödeme bilgisi) ve onay istenen bir iptal işlemi mevcuttur.
* **Operasyonel İçgörüler:** İptal oranı, aktif/gelecek rezervasyon sayısı, en popüler teslimat noktaları ve anlık filo kullanım oranı.

<img width="1895" height="915" alt="Ekran görüntüsü 2026-07-21 163355" src="https://github.com/user-attachments/assets/ef70b413-c3db-47b2-ad82-fb9d460bf33e" />
<img width="1886" height="916" alt="Ekran görüntüsü 2026-07-21 163405" src="https://github.com/user-attachments/assets/a4a42b36-6949-49fe-8455-c7b357aacdee" />



### 📊 5. Raporlama ve Dışa Aktarma
* **Şube ve durum bazlı filtreleme** ile detaylı araç raporu.
* **Hesaplanmış Metrikler:** Her araç için doluluk oranı (kiralanan gün / filoya katılalı geçen gün) ve toplam kazanç (iptal edilmemiş rezervasyonların toplamı) anlık hesaplanır.
* **Excel Dışa Aktarma (ClosedXML):** Aktif filtreye göre biçimlendirilmiş `.xlsx` dosyası üretir.
* **PDF Dışa Aktarma (QuestPDF):** Native kütüphane bağımlılığı olmadan, tamamen .NET içinde çalışan kod tabanlı PDF üretimi.

<img width="1896" height="907" alt="Ekran görüntüsü 2026-07-21 164421" src="https://github.com/user-attachments/assets/6fbd5514-fcfe-4e3b-b29d-109f86e60d95" />
<img width="1892" height="915" alt="Ekran görüntüsü 2026-07-21 164428" src="https://github.com/user-attachments/assets/fda6a861-db26-48bb-81ad-5eda95094347" />
<img width="1899" height="907" alt="Ekran görüntüsü 2026-07-21 164438" src="https://github.com/user-attachments/assets/e72d2519-b22a-4ab9-8811-4a830d90a4c2" />
<img width="1331" height="826" alt="Ekran görüntüsü 2026-07-21 164821" src="https://github.com/user-attachments/assets/a5cf51d2-0c50-425b-861c-55dddedd1189" />
<img width="1914" height="1037" alt="Ekran görüntüsü 2026-07-21 164840" src="https://github.com/user-attachments/assets/745f80c9-a711-49eb-b59e-9e538a4c1e58" />


---

## 📂 Proje Yapısı

```
Rentaly.EntityLayer/
  Entities/                 → Car, Brand, CarModel, Category, Branch, Booking, CarImage, Activity...

Rentaly.DataAccessLayer/
  Abstract/                 → IGenericDal<T>, ICarDal, IBookingDal, ...
  RepositoryDesignPattern/  → GenericRepository<T>
  EntityFramework/          → EfCarDal, EfBookingDal, ..., RentalyContext

Rentaly.Businesslayer/
  Abstract/                 → ICarService, IBookingService, ...
  Concreate/                → CarManager, BookingManager, ...
  ValidationRules/           → FluentValidation validator'ları

Rentaly.WebUI/
  Controllers/               → Kullanıcı taraflı controller'lar (Car, Booking)
  Views/                     → Kullanıcı taraflı Razor view'ları
  Areas/Admin/
    Controllers/             → CarController, BrandController, CarModelController,
                                CategoryController, BranchController, BookingController,
                                DashboardController, ReportsController
    Models/                  → Admin ViewModel'leri
    Views/                   → Admin Razor view'ları + Shared/_AdminLayout.cshtml
  Helpers/                   → CarStatusDisplayHelper, ActivityDisplayHelper, TimeAgoHelper
```

---

## ⚙️ Kurulum

```bash
# 1. Projeyi klonla
git clone <repo-url>
cd Rentaly

# 2. appsettings.json içindeki bağlantı dizesini güncelle

# 3. Migration'ları uygula
Add-Migration InitialCreate
Update-Database

# 4. Paketleri yükle ve çalıştır
dotnet restore
dotnet run --project Rentaly.WebUI
```

**İlk çalıştırma öncesi:** Araç ekleyebilmek için önce en az bir Marka, Model, Kategori, Araç Tipi ve Şube kaydı oluşturulmalıdır — araç ekleme/düzenleme formlarındaki dropdown'lar bu tablolardan beslenir.

Admin paneline `/Admin/Dashboard` üzerinden erişilir.

---

## 🧠 Mimari Kararlar ve Öğrenilenler

* **Çoklu Cascade Yolu Sorunu:** Bir tabloya birden fazla foreign key yolundan ulaşılabildiğinde (`Booking → Branch` çift FK, `CarModel → Brand` ile `Car → Brand` birlikte) SQL Server `CASCADE` silmeyi reddeder. Bu proje boyunca karşılaşılan bu senaryoların her biri, `OnModelCreating` içinde ilgili ilişkiye açıkça `DeleteBehavior.Restrict` tanımlanarak çözülmüştür.
* **Navigation Property'lerin ModelState Üzerindeki Etkisi:** `Car.Images`, `Brand.Cars`, `Category.Cars` gibi koleksiyon tipi alanlar formdan hiçbir zaman gelmediği için sahte "zorunlu alan" doğrulama hataları üretiyordu. Genel doğrulamayı gevşetmek yerine, ilgili her Create/Edit POST action'ında `ModelState.Remove(...)` ile bu alanlar açıkça hariç tutulmuştur.
* **Liste ve Detay Sorgularının Ayrılması:** Liste görünümleri sadece ihtiyaç duyduğu ilişkileri `Include` eder; detay sayfaları için ayrı, daha kapsamlı Include zincirine sahip metodlar (`GetCarWithDetailsByIdAsync`, `GetDetailsByIdAsync`) tanımlanmıştır — böylece liste sayfaları gereksiz JOIN maliyeti taşımaz.
* **PDF Kütüphanesi Değişimi:** Proje başlangıçta `DinkToPdf` ile geliştirildi, ancak native `libwkhtmltox` bağımlılığı hedef ortamda `DllNotFoundException` hatasına yol açtı. Bu sorun, tamamen .NET içinde çalışan ve native bağımlılık gerektirmeyen **QuestPDF**'e geçilerek kalıcı olarak çözülmüştür.

---

## 🔭 Yol Haritası

* Admin panelinden doğrudan rezervasyon oluşturma akışı henüz eklenmedi.
* Marka seçimine göre model dropdown'unu istemci tarafında filtreleyen kademeli (cascading) seçim yapısı planlanıyor.
* Admin Area için kimlik doğrulama/yetkilendirme (ASP.NET Core Identity + rol bazlı politika) henüz uygulanmadı.
* Raporlardaki doluluk oranı şu an "filoya katılalı geçen gün" bazlı hesaplanıyor; ileride hareketli 30/90 günlük pencereye geçilmesi daha isabetli bir metrik sunacaktır.

---

*ASP.NET Core MVC üzerinde N-Tier mimari, EF Core ilişkisel modelleme ve admin panel UX'i üzerine uygulamalı bir öğrenme projesi olarak geliştirilmiştir.*
