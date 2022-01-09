using System;
using System.IO;
using System.Collections.Generic;

namespace Seminarska
{//"Izjavljam, da sem nalogo opravil samostojno in da sem njen avtor. Zavedam se, da v primeru, če izjava prvega stavka ni resnična, kršim disciplinska pravila."
    public class Motor :IEquatable<Motor>
    {

        public Motor()
        { }
        public string znamka
        { get; set; }
        public string barva
        { get; set; }
        public int prevozeni//KM
        { get; set; }
        public double cena//€
        { get; set; }
        public int moc
        { get; set; }//KW
        public string kategorija
        { get; set; }
        public Motor(string znamka,string kategorija, int moc, int prevozeni, double cena)
        {
            this.znamka = znamka;
            this.prevozeni = prevozeni;
            this.cena = cena;
            this.barva = Barva();
            this.kategorija = kategorija;
            this.moc = moc;
        }
        public override string ToString()
        {
            return "\nZnamka: ".PadLeft(15, ' ') + znamka +
               
                "\nPrevoženi km: ".PadLeft(15, ' ') + prevozeni + " km" +
                "\nCena: ".PadLeft(15, ' ') + cena + " eur" +
                "\nBarva: ".PadLeft(15, ' ') + barva +
                "\nVozen z kategorijo: " .PadLeft(15, ' ') + kategorija +
                "\nMoč: ".PadLeft(15, ' ') + moc + " KW" +
                "\nMoč v konjih: ".PadLeft(15, ' ') + Konji() + " KM";
        }
        public double Konji()
        {
            return this.moc * 1.341;
        }
        public string Barva()
        {
            string x = znamka.ToLower();
            string b;
            switch (x)
            {
                case "honda":
                    b = "Oranžna";
                    break;
                case "yamaha":
                    b = "Modra";
                    break;
                case "kawasaki":
                    b = "Zelena";
                    break;
                case "bmw":
                    b = "Siva";
                    break;
                case "ducati":
                    b = "Rdeča";
                    break;
                case "aprilia":
                    b = "Črna";
                    break;
                default:
                    b = "Bela";
                    break;
            }
            return b;
        }
        public static Comparer<Motor> GetComparer(string atribut)
        {
            switch (atribut.ToLower())
            {
                case "znamka":
                    return Comparer<Motor>.Create((x, y)=> x.znamka.CompareTo(y.znamka));
                case "barva":
                    return Comparer<Motor>.Create((x, y) => x.barva.CompareTo(y.barva));
                case "prevozeni":
                    return Comparer<Motor>.Create((x, y) => x.prevozeni.CompareTo(y.prevozeni));
                case "cena":
                    return Comparer<Motor>.Create((x, y) => x.cena.CompareTo(y.cena));
                case "moc":
                    return Comparer<Motor>.Create((x, y) => x.moc.CompareTo(y.moc));
                case "kategorija":
                    
                default: return Comparer<Motor>.Create((x, y) => x.cena.CompareTo(y.cena));
            }
        }
        public bool Equals(Motor m)
        {
            if (m == null)
            {
                return false;
            }
            return znamka.Equals(m.znamka) && barva.Equals(m.barva) && prevozeni.Equals(m.prevozeni) && cena.Equals(m.cena) && moc.Equals(m.moc) && kategorija.Equals(m.kategorija);
        } //primerjanje abjekotv z drugimi objekti (extenda Defoult.Equals metodo)
    }
}
