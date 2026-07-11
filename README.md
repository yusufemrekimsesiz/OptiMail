# OptiMail

Gmail gelen kutusundaki okunmamış e-postaları yapay zeka ile analiz edip otomatik olarak kategorize eden bir .NET MAUI mobil uygulaması.

## Ne Yapar?

- Gmail hesabındaki okunmamış e-postaları IMAP üzerinden çeker
- Her e-postayı Google Gemini API ile analiz eder:
  - **Kategori başlığı** (örn. "Fatura Bildirimi", "İş Toplantısı")
  - **Özet** (2-3 cümlelik içerik özeti)
  - **Aciliyet seviyesi** (Düşük / Orta / Yüksek)
  - **Aksiyon maddeleri** (mailden çıkan yapılacaklar listesi)
- Analiz edilen mailleri kategoriye göre gruplayarak modern bir arayüzde gösterir
- Aynı mailin tekrar tekrar analiz edilmesini önlemek için yerel bir "işlendi" takibi tutar (Gmail'deki okundu/okunmadı durumuna dokunmadan)

## Kullanılan Teknolojiler

- **.NET MAUI** — çapraz platform mobil uygulama
- **MVVM** mimarisi + **CommunityToolkit.Mvvm**
- **MailKit** — IMAP üzerinden Gmail'e bağlanma
- **Google Gemini API** (`gemini-2.5-flash`) — e-posta analizi
- **SecureStorage** — Gmail ve API kimlik bilgilerinin cihazda şifreli saklanması
- **Preferences** — işlenmiş mail ID'lerinin yerel takibi

## Mimari

```
OptiMail/
├── Models/            → EmailMessage, EmailAnalysisResult, EmailGroup
├── Services/          → IMAP, Gemini API, SecureStorage ve yerel takip servisleri
├── ViewModels/         → MainViewModel, SettingsViewModel (MVVM)
├── Converters/         → XAML veri dönüştürücüleri
└── (MainPage / SettingsPage)  → Kullanıcı arayüzü
```

## Kurulum

### 1. Gerekli NuGet Paketleri

```
dotnet add package MailKit
dotnet add package CommunityToolkit.Mvvm
```

### 2. Gmail Uygulama Şifresi

1. Google Hesabı → **Güvenlik** → **2 Adımlı Doğrulama**'yı aç
2. **Uygulama Şifreleri** sayfasından yeni bir şifre oluştur
3. 16 haneli kodu not al

### 3. Gemini API Anahtarı

[aistudio.google.com/apikey](https://aistudio.google.com/apikey) adresinden ücretsiz bir API anahtarı oluştur.

### 4. Uygulama İçinde Ayarları Gir

Uygulamayı çalıştır → sağ üstteki **Ayarlar**'a git → Gmail adresini, uygulama şifresini ve Gemini API anahtarını gir → **Kaydet**.

Bu bilgiler cihazda `SecureStorage` ile şifreli olarak saklanır, koda gömülmez ve GitHub'a gönderilmez.

## Bilinen Sınırlamalar

- Gemini API'nin ücretsiz katmanı dakikada 5 istekle sınırlıdır; bu yüzden uygulama bir seferde en fazla 5 mail işler ve limit aşılırsa otomatik olarak bekleyip tekrar dener
- Şu an her kullanıcı kendi Gemini API anahtarını girmek zorundadır (merkezi bir backend proxy yoktur)
