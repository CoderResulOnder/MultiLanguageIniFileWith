//Dil.iniDataGetValue("BilgisayariKapatmakIstediginizdenEminmisiniz", 
//Dil.iniBuyukKucukKontrol.sadece1harfBuyukKalaniKucuk), 
//Dil.iniDataGetValue("Evet", Dil.iniBuyukKucukKontrol.TamamiBuyuk), 
//Dil.iniDataGetValue("Hayir", Dil.iniBuyukKucukKontrol.TamamiBuyuk));



using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNir.Classes
{
    class Dil
    {
        private static string mevcutDil { get; set; }
        public static void DilGuncelle(string yeniDil)
        {
            mevcutDil = yeniDil;
        }

        public static string GetLanguage()
        {
            return mevcutDil;
        }


        public Dil()
        {
            mevcutDil = null;
        }

        #region MultiLanguage
        static FileIniDataParser parser = new FileIniDataParser();
        static IniData inidata;
        public static void iniDosyaislemeleriComboboxtanDilleriDoldur(System.Windows.Forms.ComboBox cmbLanguageSelection)
        {
            try
            {
                //okumak icin:
                inidata = parser.ReadFile("MultiLanguage.ini");
                //string useFullScreenStr = data["General"]["fullscreen"];
                //bool useFullScreen = bool.Parse(useFullScreenStr); //useFullScreenStr = "true"

                int a = inidata.Sections.Count;


               

                cmbLanguageSelection.Items.Clear();
                foreach (SectionData sd in inidata.Sections.ToList())
                {
                    string i = sd.SectionName;

                    //var r = new RegionInfo(CultureInfo.GetCultureInfo(inidata[i.ToUpper()]["CultureName"]).LCID);
                    //var flagName = r.TwoLetterISORegionName + ".gif";
                    

                    cmbLanguageSelection.Items.Add(i.ToUpper());

                  
                }


                if (mevcutDil != null)
                {
                    cmbLanguageSelection.Text = mevcutDil;
                }
                else
                {
                    if (cmbLanguageSelection.Items.Count >= 1)
                    {
                        cmbLanguageSelection.SelectedIndex = 0;
                    }

                }
            }
            catch (Exception w)
            {
            }

        }


        public static void getAllCurrentCulterName()
        {

            try
            {
                string path = @"C:\CulterInfo\AllCulterInfo.txt";
                Directory.CreateDirectory(@"C:\CulterInfo");

                if (File.Exists(path) == false)
                {

                    using (System.IO.StreamWriter fs = new System.IO.StreamWriter(path, true))
                    {
                        // Add some information to the file.

                        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

                        foreach (var culture in cultures)
                        {
                            string CulterInfoTXT = culture.TextInfo.CultureName + "_____" + culture.DisplayName;

                            fs.WriteLine(CulterInfoTXT);
                        }

                        fs.Close();
                    }
                }
            }
            catch (Exception)
            {


            }


        }

        public static string GetDate()
        {
            if (inidata != null && mevcutDil != null && inidata[mevcutDil]["CultureName"] != null)
            {
                DateTimeFormatInfo fmt = (new CultureInfo(inidata[mevcutDil]["CultureName"])).DateTimeFormat;

                string a = String.Format(DateTime.Now.ToString("D", fmt)) + "-" + DateTime.Now.ToLongTimeString();
                return a;
            }
            return DateTime.Now.ToString();

        }

        public enum iniBuyukKucukKontrol
        {
            TamamiBuyuk, TamamiKucuk, IlkHarflerBuyukDigerleriKucuk, sadece1harfBuyukKalaniKucuk, DosyadakiniAynenGetir
        }

        public static string iniDataGetValue(string aranan, iniBuyukKucukKontrol gelen)
        {
            if (inidata == null || inidata[mevcutDil][aranan] == null || inidata[mevcutDil][aranan] == "")
            {
                return aranan;
            }
            else
            {
                switch (gelen)
                {

                    case iniBuyukKucukKontrol.DosyadakiniAynenGetir:

                        return inidata[mevcutDil][aranan].ToString();

                    case iniBuyukKucukKontrol.TamamiBuyuk:
                        if (inidata[mevcutDil]["CultureName"] != null)
                        {
                            return CultureInfo.GetCultureInfo(inidata[mevcutDil]["CultureName"]).TextInfo.ToUpper(inidata[mevcutDil][aranan].ToString());

                        }
                        return CultureInfo.CurrentCulture.TextInfo.ToUpper(inidata[mevcutDil][aranan].ToString());

                    case iniBuyukKucukKontrol.TamamiKucuk:

                        if (inidata[mevcutDil]["CultureName"] != null)
                        {
                            return CultureInfo.GetCultureInfo(inidata[mevcutDil]["CultureName"]).TextInfo.ToLower(inidata[mevcutDil][aranan].ToString());

                        }
                        return CultureInfo.CurrentCulture.TextInfo.ToLower(inidata[mevcutDil][aranan].ToString());


                    case iniBuyukKucukKontrol.IlkHarflerBuyukDigerleriKucuk:


                        if (inidata[mevcutDil]["CultureName"] != null)
                        {
                            return CultureInfo.GetCultureInfo(inidata[mevcutDil]["CultureName"]).TextInfo.ToTitleCase(inidata[mevcutDil][aranan].ToString());

                        }
                        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(inidata[mevcutDil][aranan].ToString());

                    case iniBuyukKucukKontrol.sadece1harfBuyukKalaniKucuk:


                        if (inidata[mevcutDil]["CultureName"] != null)
                        {

                            string aa = CultureInfo.GetCultureInfo(inidata[mevcutDil]["CultureName"]).TextInfo.ToLower(inidata[mevcutDil][aranan].ToString());
                            string aaa = CultureInfo.GetCultureInfo(inidata[mevcutDil]["CultureName"]).TextInfo.ToUpper(aa.ToArray()[0]).ToString();


                            if (aa.Length > 1)
                            {
                                aa = aa.Remove(0, 1);
                                aa = aa.Insert(0, aaa);
                            }
                            return aa;

                        }

                        string a = CultureInfo.CurrentCulture.TextInfo.ToLower(inidata[mevcutDil][aranan].ToString());
                        a.ToArray()[0] = Convert.ToChar(a.ToArray()[0].ToString().ToLower());
                        return a;


                    default:
                        return inidata[mevcutDil][aranan].ToString();

                }
            }
        }



        public static double ConvertToDouble(string gelen)
        {

            double doubleValue = 0;
            double.TryParse(gelen, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo(inidata[mevcutDil]["CultureName"]), out doubleValue);
            return doubleValue;
        }

        public static string GetValue(string aranan, iniBuyukKucukKontrol gelen)
        {
          
            
                switch (gelen)
                {
                    case iniBuyukKucukKontrol.TamamiBuyuk:
                     
                        return CultureInfo.CurrentCulture.TextInfo.ToUpper(aranan);

                    case iniBuyukKucukKontrol.TamamiKucuk:

                      
                        return CultureInfo.CurrentCulture.TextInfo.ToLower(aranan);


                    case iniBuyukKucukKontrol.IlkHarflerBuyukDigerleriKucuk:
                    
                        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(aranan);
                    
                    default:
                        return aranan;

                }
            
        }

        #endregion

    }
}




iniDataGetValue("Analiz",  iniBuyukKucukKontrol.IlkHarflerBuyukDigerleriKucuk)




İni Dosyası nının içerigi:

[TR]
CultureName=tr-TR
Analiz = analiz
Olcum = ölçüm
SurekliOlcum = sürekli ölçüm
Durdur = durdur
Kaydet = kaydet
Rapor = rapor
KutuphaneyeEkle = kütüphaneye ekle
Intensity = intensity(count)
RamanShift = raman shift(cmˉ¹) 
KalitatifAnaliz = kalitatif analiz
XmlYonetimi = xml yönetimi
Grafik = grafik
Veri = veri
KantitatifAnaliz = kantitatif analiz
VeriGoruntule = veri görüntüle
KutuphaneYonetimi = kütüphane yönetimi
OlcumSuresi = ölçüm süresi
Cozunurluk = çözünürlük
OlcumSayisi = ölçüm sayısı
OlcumTuru = ölçüm türü
LazerGucu = lazer gücü
BaselineCapi = baseline çapı
KutuphaneAdi = kütüphane adı
KayitliOrnekAdi = kayitli örnek adı
BenzerlikYuzdesi = benzerlik yuzdesi
FarkDegeri = fark degeri
id = kayit no
ParametreAdi = parametre adı
xmlDosyaAdi = xml dosya adı
Sonuc = sonuç
EnYuksek = en yüksek
Yuksek = yüksek
Orta = orta 
Dusuk = düşük
EnDusuk = en düşük
Karanlik = karanlık
Aydinlik = aydınlık
Fark = fark
Dalgaboyu = dalgaboyu
Normalize = normalize
Baseline = baseline
NormalOlcumSonuclari = standart ölçüm sonuçları
SurekliOlcumSonuclari = sürekli ölçüm sonuçları
Evet =Evet
Hayir=Hayır
UygulamadanCikmayiOnayliyormusunuz=uygulamadan çıkış yapmak üzeresiniz onaylıyormusunuz?


[ENG]
CultureName=en-US
Analiz = analysis
Olcum = measurement
SurekliOlcum = continuous measurement
Durdur = stop
Kaydet = save
Rapor = report
KutuphaneyeEkle = add to library
Intensity = intensity(count)
RamanShift = raman shift(cmˉ¹) 
KalitatifAnaliz = qualitative analysis
XmlYonetimi = xml management
Grafik = graph
Veri = data
KantitatifAnaliz = quantitative analysis
VeriGoruntule = display data
KutuphaneYonetimi = library management
OlcumSuresi=measurement time
Cozunurluk = resolution
OlcumSayisi = measurement count
OlcumTuru = measurement type
LazerGucu = laser power
BaselineCapi = baseline diameter
KutuphaneAdi = library name
KayitliOrnekAdi = registered sample name
BenzerlikYuzdesi = similarity percentage
FarkDegeri = differance value
id = registration number
ParametreAdi = parameter name
xmlDosyaAdi = xml file name
Sonuc = result
EnYuksek = very high
Yuksek = high
Orta = medium 
Dusuk = low
EnDusuk = very low
Karanlik = dark
Aydinlik=light
Fark = difference
Dalgaboyu = wavelength
Normalize = normalize
Baseline = baseline
NormalOlcumSonuclari=standard measurement result
SurekliOlcumSonuclari=continuous measurement result
Evet =ok
Hayir=no
UygulamadanCikmayiOnayliyormusunuz=are you confirming that you will be leaving the application?
[AR]
CultureName=ar-SA
Analiz = تحليل
