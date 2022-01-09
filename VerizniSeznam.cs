using System;
public class VerizniSeznam<T>
{//"Izjavljam, da sem nalogo opravil samostojno in da sem njen avtor. Zavedam se, da v primeru, če izjava prvega stavka ni resnična, kršim disciplinska pravila."
    private Vozel<T> prvi;
    private Vozel<T> zadnji;
    private int velikost;
    public int Velikost { get { return velikost; } }
    public Vozel<T> Prvi { get { return prvi; } }

    public T this[int index]
    {   
        get
        {
            if (index >= velikost) return default(T);
            Vozel<T> t = prvi;
            int i = 0;
            while (t != null && i < index)
            {
                i++;
                t = t.Nasl;
            }
            return t.Vsebina;
        }
        set
        {
            if (index >= velikost) return;
            Vozel<T> t = prvi;
            int i = 0;
            while (t != null && i < index)
            {
                i++;
                t = t.Nasl;
            }
            if (t == null) return;
            t.Vsebina = value;
        }

    }
    public void Dodaj(T podatek)
    {
        if (prvi == null)
        {
            prvi = new Vozel<T>(podatek);
            zadnji = prvi;
        }
        else
        {
            zadnji.Nasl = new Vozel<T>(podatek);
            zadnji = zadnji.Nasl;
        }
        velikost++;
    }
    public void DodajVse(GenericnaZbirka<T> zbirka)
    {
        for (int i = 0; i < zbirka.Velikost; i++)
        {
            Dodaj(zbirka[i]);
        }
    }
    public void Zbrisi(int index)
    {
        ZbrisiRekurzivno(prvi, index);
    }
    private Vozel<T> ZbrisiRekurzivno(Vozel<T> t, int index)
    {
        if (t == null) return null;
        if (index == 0)
        {
            velikost--;
            return t.Nasl;
        }
        t.Nasl = ZbrisiRekurzivno(t.Nasl, index - 1);
        return t;
    }
    public void Izpis()
    {
        if (velikost == 0)
        {
            Console.WriteLine("Seznam je prazen");
            return;
        }
        IzpisiRekurzivno(prvi);
    }
    private void IzpisiRekurzivno(Vozel<T> t)
    {
        if (t == null) return;
        Console.WriteLine(t.Vsebina.ToString());
        IzpisiRekurzivno(t.Nasl);
    }
    public void Pocisti()
    {
        velikost = 0;
        prvi = null;
        zadnji = null;
    }
}
public class Vozel<T>
{
    private T podatek; // zasebno polje, ki hrani podatke generičnega tipa
    private Vozel<T> naslednji; // referenca na naslednjega
    public Vozel() // konstruktor
    {
        this.Vsebina = default(T);
        this.Nasl = null; // kazalec (referenca) na naslednji element
    }
    //dodatni konstruktor
    public Vozel(T podatek)
        : this()//dedujemo bazični konstruktor
    {
        this.Vsebina = podatek;
    }
    public T Vsebina  //lastnost/Property
    {
        get { return this.podatek; }
        set { this.podatek = value; }
    }
    public Vozel<T> Nasl
    {
        get { return this.naslednji; }
        set { this.naslednji = value; }
    }
}

