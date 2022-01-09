using System;
using System.Collections.Generic;
public class GenericnaZbirka<T>
{//"Izjavljam, da sem nalogo opravil samostojno in da sem njen avtor. Zavedam se, da v primeru, če izjava prvega stavka ni resnična, kršim disciplinska pravila."
    private T[] elementi;  //tabelarično polje
    private int velikost;   //polje hrani trenutno število podatkov v tabeli    
    public GenericnaZbirka(int n = 0)  //konstruktor
    { elementi = new T[n]; velikost = n; }//začetna dimenzija tabele/zbirke  
    public T this[int indeks]   //indeksiranje 
    {
        get { return elementi[indeks]; } //dostop do posameznih polj
        set { elementi[indeks] = value; }  //prirejanje vrednostim poljem
    }
    //napišimo property, s katerim pridobimo atribut velikost
    public int Velikost
    {
        get { return velikost; }
    }

    //še get metoda za prodobivanje polja velikost
    public int VrneVelikost()
    {
        return velikost;
    }

    public void OdstraniVse()
    {
        elementi = new T[0];
        velikost = 0;
    }
    public void Add(T podatek)  //metoda za dodajanje novega elementa 
    {
        Array.Resize(ref elementi, elementi.Length + 1);
        elementi[velikost] = podatek;  //podatek zapišemo v prvo prosto celico
        velikost = velikost + 1; //število zasedenih celic
    }
    //generična metoda za brisanje celice z določenim indeksom
    public void Brisanje(int indeksCelice)
    {
        if (velikost == 0)
            Console.WriteLine("Zbirka je prazna, brisanje NI možno!");
        //celico brišemo le, če je njen indeks manjši od dimenzije zbirke  
        // if (indeksCelice < elementi.Length && indeksCelice >= 0)
        else if (indeksCelice < elementi.Length)
        {
            T[] zacasna = new T[elementi.Length - 1];
            int j = 0;
            for (int i = 0; i < elementi.Length; i++)
            {
                if (i != indeksCelice)
                {
                    zacasna[j] = elementi[i];
                    j++;
                }
            }
            elementi = zacasna;
            velikost = velikost - 1;//zmanjšamo velikost zbirke
        }
        else Console.WriteLine("Brisanje NI možno, ker indeks št ," + indeksCelice + " NE obstaja!");
    }
    //generična metoda za izpis poljubne zbirke
    public void IzpisZbirke()
    {
        if (velikost == 0)
            Console.WriteLine("Zbirka je prazna!");
        else
        {
            Console.WriteLine("Izpis ZBIRKE: ");
            for (int i = 0; i < velikost; i++)
                Console.WriteLine(elementi[i].ToString() + " ");
            Console.WriteLine();
        }
    }
    public GenericnaZbirka<T> Filter(Func<T, GenericnaZbirka<T>, bool> predicat)
    {
        GenericnaZbirka<T> novazbirka = new GenericnaZbirka<T>();
        for (int i = 0; i < velikost; i++)
        {
            if (predicat(elementi[i], this))
            {
                novazbirka.Add(elementi[i]);
            }
        }
        return novazbirka;
    }
    public void Sort(Comparer<T> comparer)
    {
        Array.Sort(elementi, 0, velikost, comparer);
    }
    public Stack<T> ToStack()
    {
        Stack<T> sklad = new Stack<T>();
        for (int i = 0; i < velikost; i++)
        {
            sklad.Push(elementi[i]);
        }
        return sklad;
    }
    public int IndexOf(T obj, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            if (EqualityComparer<T>.Default.Equals(obj, elementi[i])) //primerjanje generičnig objektov z njihovo Equals metodo
            {
                return i;
            }
        }
        return -1;
    }
    public void OdstraniDuplikate()
    {
        int index = 0;
        while (index < velikost)
        {
            if (IndexOf(elementi[index], index + 1, velikost) != -1)
            {
                Console.WriteLine("Izbrisan duplikat!");
                Brisanje(index);
            }
            else index++;
        }
    }
}