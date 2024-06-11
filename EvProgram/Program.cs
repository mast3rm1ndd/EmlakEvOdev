using System;
using System.Collections.Generic;
using System.IO;
using EmlakEvOdev;

namespace EvProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Uygulamaya hoş geldiniz. Hangi tür ev üzerinde işlem yapmak istersiniz?\n");
                Console.WriteLine("1- Kiralık Ev");
                Console.WriteLine("2- Satılık Ev");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "1":
                        KiralikEvMenu();
                        break;
                    case "2":
                        SatilikEvMenu();
                        break;
                    default:
                        Console.WriteLine("Lütfen '1' ya da '2' seçeneklerinden birini seçin.");
                        break;
                }
            }
        }

        static void KiralikEvMenu()
        {
            while (true)
            {
                Console.WriteLine("1- Kayıtlı evleri görüntüle");
                Console.WriteLine("2- Yeni ev girişi");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "1":
                        EvleriGoruntule<KiralikEv>("kiralik_evler.txt");
                        break;
                    case "2":
                        YeniEvGirisi<KiralikEv>("kiralik_evler.txt");
                        break;
                    default:
                        Console.WriteLine("Lütfen '1' ya da '2' seçeneklerinden birini seçin.");
                        break;
                }
            }
        }

        static void SatilikEvMenu()
        {
            while (true)
            {
                Console.WriteLine("1- Kayıtlı evleri görüntüle");
                Console.WriteLine("2- Yeni ev girişi");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "1":
                        EvleriGoruntule<SatilikEv>("satilik_evler.txt");
                        break;
                    case "2":
                        YeniEvGirisi<SatilikEv>("satilik_evler.txt");
                        break;
                    default:
                        Console.WriteLine("Lütfen '1' ya da '2' seçeneklerinden birini seçin.");
                        break;
                }
            }
        }

        static List<T>ReadFromFile<T>(string dosyaAdi) where T : Ev, new()
        {
            List<T> evler = new List<T>();

            if (File.Exists(dosyaAdi))
            {
                using (StreamReader sr = new StreamReader(dosyaAdi))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        T ev = new T();
                        ev.OdaSayisi = Convert.ToInt32(parts[0]);
                        ev.KatSayisi = Convert.ToInt32(parts[1]);
                        ev.Alan = Convert.ToDouble(parts[2]);
                        ev.Semt = (parts[4]); // işe yaramadı // YARADI SONUNDA INDEX SAYILARINI DEĞİŞTİRDİM.

                        if (ev is KiralikEv)
                        {
                            KiralikEv kiralikEv = ev as KiralikEv;
                            kiralikEv.Kira = Convert.ToDouble(parts[3]);
                            kiralikEv.Depozito = Convert.ToDouble(parts[5]);
                            evler.Add(ev); 
                        }
                        else if (ev is SatilikEv)
                        {
                            SatilikEv satilikEv = ev as SatilikEv;
                            satilikEv.EvFiyat = Convert.ToDouble(parts[6]); // Burada çözemediğim bir hata var. Kiralık ev kısmı sorunsuz çalışırken satılık evlerde kayıtlı evlerini göstermeye çalıştığımda exception atıyor. Index neden 6 olamıyor anlamadım. 6'dan küçük bir rakam girince kod çalışıyor fakat veriler düzgün olmuyor zira diğer indexler ile çakışıyor.
                            // Bir ara her şey düzgün çalışıyordu. Bi şeyler yapıp bozdum. Kappa
                            evler.Add(ev);
                        }
                    }
                }
            }

            return evler;
        }


        static void EvleriGoruntule<T>(string dosyaAdi) where T : Ev, new()
        {
            List<T> evler = ReadFromFile<T>(dosyaAdi);

            foreach (var ev in evler)
            {
                Console.WriteLine($"Oda Sayısı: {ev.OdaSayisi}, Kat Sayısı: {ev.KatSayisi}, Semt: {ev.Semt}, Alan: {ev.Alan}");

                
                if (ev is KiralikEv)
                {
                    KiralikEv kiralikEv = ev as KiralikEv;
                    Console.WriteLine($"Kira: {kiralikEv.Kira}, Depozito: {kiralikEv.Depozito}");
                }
                
                else if (ev is SatilikEv)
                {
                    SatilikEv satilikEv = ev as SatilikEv;
                    Console.WriteLine($"Fiyat: {satilikEv.EvFiyat}");
                }

                Console.WriteLine(); 
            }
        }



        static void YeniEvGirisi<T>(string dosyaAdi) where T : Ev, new()
        {
            List<T> evler = ReadFromFile<T>(dosyaAdi);

            while (true)
            {
                T yeniEv = new T();

                Console.WriteLine("Ev bilgilerini giriniz:");
                Console.Write("Oda Sayısı: ");
                yeniEv.OdaSayisi = Convert.ToInt32(Console.ReadLine());
                Console.Write("Kat Sayısı: ");
                yeniEv.KatSayisi = Convert.ToInt32(Console.ReadLine());
                Console.Write("Alan: ");
                yeniEv.Alan = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Semt :");
                yeniEv.Semt = Console.ReadLine();

                if (yeniEv is KiralikEv kiralikEv)
                {
                    Console.WriteLine("Kira :");
                    kiralikEv.Kira = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Depozito: ");
                    kiralikEv.Depozito = Convert.ToDouble(Console.ReadLine());
                }
                else if (yeniEv is SatilikEv satilikEv)
                {
                    Console.WriteLine("Satılık evin fiyatı: ");
                    satilikEv.EvFiyat = Convert.ToDouble(Console.ReadLine());
                }

                evler.Add(yeniEv);

                Console.Write("Tamam mı (t) / Devam mı (d): ");
                string devam = Console.ReadLine();
                if (devam.ToLower() != "d")
                {
                    ReadToFile(evler, dosyaAdi);
                    Console.WriteLine("Dosyaya kaydedildi.");
                    Main(new string[0]);
                    return;
                }
            }
        }


        static void ReadToFile<T>(List<T> evler, string dosyaAdi) where T : Ev
        {
            using (StreamWriter sw = new StreamWriter(dosyaAdi))
            {
                foreach (var ev in evler)
                {
                    if (ev is KiralikEv)
                    {
                        KiralikEv kiralikEv = ev as KiralikEv;
                        sw.WriteLine($"{kiralikEv.OdaSayisi},{kiralikEv.KatSayisi},{kiralikEv.Alan},{kiralikEv.Kira},{kiralikEv.Semt},{kiralikEv.Depozito}");
                    }
                    else if (ev is SatilikEv)
                    {
                        SatilikEv satilikEv = ev as SatilikEv;
                        sw.WriteLine($"{satilikEv.OdaSayisi},{satilikEv.KatSayisi},{satilikEv.Alan},{satilikEv.Semt},{satilikEv.EvFiyat}");
                    }
                }
            }
        }
    }
}