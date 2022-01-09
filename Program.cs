using System;
using System.IO;
using System.Collections.Generic; //za omogočanje dinamičnih tabel
using System.Linq;
using System.Data.SqlClient;

namespace Seminarska
{// izjavljam, da sem seminarsko nalogo opravil samostojno in da sem njen avtor. Zavedam se, da v primeru, če izjava prvega stavka ni resnična, kršim disciplinska pravila.

    class Program
    { /*ZAGOVOR BOM UPRAVLJAL PREKO VIDEA NA POVEZAVI: https://youtu.be/4wBzyGM5Ykk */
        //probal sem strniti

        private static GenericnaZbirka<Motor> genmotorji = new GenericnaZbirka<Motor>();
        private static VerizniSeznam<Motor> verizni = new VerizniSeznam<Motor>();
        static void Main(string[] args)
        {
            List<Motor> novmotorji = new List<Motor>();
            if (!File.Exists("motorji.XML"))
            {
                novmotorji.Add(new Motor("kawasaki", "A2", 35,3000,5000));
                novmotorji.Add(new Motor("Yamaha", "AM", 5, 1664, 2500));
                novmotorji.Add(new Motor("Suzuki", "A", 70, 323400, 12390));
                novmotorji.Add(new Motor("Yamaha", "A", 100, 3550, 8000));
                novmotorji.Add(new Motor("kawasaki", "A2", 35, 3000, 5000));//podvojen
                Serializacija<Motor>(novmotorji, "motorji.XML");
            }
            ListvGen();
            Moznosti(genmotorji);
        }
        static void Serializacija<T>(List<T> list, string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            using (var stream = System.IO.File.OpenWrite(fileName))
            {
                serializer.Serialize(stream, list);
            }
        }
        static void Deserializacija<T>(List<T> list, string fileName)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
            using (var stream = System.IO.File.OpenRead(fileName))
            {
                var other = (List<T>)(serializer.Deserialize(stream));
                list.Clear();
                list.AddRange(other);
            }
        }
        public static string dbConn()
        {
            return (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")) + @"motor.mdf;Integrated Security=True");
        }
        public static Motor NovMotor()
        {
            Motor novi = new Motor();
            try
            {
                Console.WriteLine("Dodajanje v tabelo Motor");
                Console.WriteLine("            DODAJTE ZAPIS");
                Console.WriteLine("");
                Console.WriteLine("Znamka: ");
                novi.znamka = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("            DODAJTE ZAPIS");
                Console.WriteLine("");
                Console.WriteLine("barva: ");
                novi.barva = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("            DODAJTE ZAPIS");
                Console.WriteLine("");
                Console.WriteLine("prevozeni: ");
                novi.prevozeni = Convert.ToInt32(Console.ReadLine());

                Console.Clear();
                Console.WriteLine("            DODAJTE ZAPIS");
                Console.WriteLine("");
                Console.WriteLine("cena: ");
                novi.cena = Convert.ToDouble(Console.ReadLine());

                Console.Clear();
                Console.WriteLine("            DODAJTE ZAPIS");
                Console.WriteLine("");
                Console.WriteLine("moc[kw]: ");
                novi.moc = Convert.ToInt32(Console.ReadLine());

                Console.Clear();
                Console.WriteLine("            DODAJTE ZAPIS");
                Console.WriteLine("");
                Console.WriteLine("Kategorija:  ");
                novi.kategorija = Console.ReadLine();
                return novi;
                

            }
            catch { Console.WriteLine("Napaka pri vnasanju podatkov"); }
            return null;
        }
        public static int zapisDB()
        {
            string conn = dbConn();
            Motor motor = NovMotor();
            int fkkat = 1;
            switch(motor.kategorija.ToLower()){
                case "a1":
                    fkkat = 2;
                    break;
                case "a2":
                    fkkat = 3;
                    break;
                case "a":
                    fkkat = 4;
                    break;
            }

            string poizvedba = "INSERT INTO Motor (znamka, barva, prevozeni, cena, moc, id_kategorija) VALUES (@z, @b, @p, @c, @m, @fkkat)";
            Console.WriteLine(poizvedba);
            Console.ReadKey();
            using (SqlConnection dataConnection = new SqlConnection(conn))
            {
                dataConnection.Open();

                using (SqlCommand cmd = new SqlCommand(poizvedba, dataConnection))
                {
                    cmd.Parameters.AddWithValue("@z", motor.znamka);
                    cmd.Parameters.AddWithValue("@b", motor.barva);
                    cmd.Parameters.AddWithValue("@p", motor.prevozeni);
                    cmd.Parameters.AddWithValue("@c", motor.cena);
                    cmd.Parameters.AddWithValue("@m", motor.moc);
                    cmd.Parameters.AddWithValue("@fkkat", fkkat);
                    cmd.ExecuteNonQuery();
                }

                dataConnection.Close();
            }
            return 0;
        }
        public static int izpisbazeMotor()
        {
            string conn = dbConn();

                Console.WriteLine("TABELA Motor");
            Console.WriteLine("Id_motor |znamka | barva | prevozeni | cena | moc |id_kategorija");
            string poizvedba = "SELECT * FROM Motor";
                using (SqlConnection dataConnection = new SqlConnection(conn))
                {
                    dataConnection.Open();
                    using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                    {
                        using (SqlDataReader reader = dataCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            Console.WriteLine(reader.GetValue(0) + " | " + reader.GetValue(1) + " | " + reader.GetValue(2) + " | " + reader.GetValue(3) + " | " + reader.GetValue(4) + " | " + reader.GetValue(5) + " | " + reader.GetValue(6));
                        }
                        }
                    }
                    dataConnection.Close();
                }
                return (0);
            }
        public static int izpisbazeKategorija()
        {
            string conn = dbConn();

            Console.WriteLine("TABELA Kategorija");
            Console.WriteLine("id_kategorija |kategorija");
            string poizvedba = "SELECT * FROM Kategorija";
            using (SqlConnection dataConnection = new SqlConnection(conn))
            {
                dataConnection.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                {
                    using (SqlDataReader reader = dataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetValue(0) + "             | " + reader.GetValue(1));
                        }
                    }
                }
                dataConnection.Close();
            }
            return (0);
        }
        public static int urejanjeVnosa(int id)
        {
            Console.Clear();
            string conn = dbConn();
            string znamka = "", barva = "";
            int prevozeni =0, id_kategorija=0, moc=0;
            double cena=0;


            try
            {
                    using (SqlConnection dataConnection = new SqlConnection(conn))
                    {
                        dataConnection.Open();
                        string pridobitev = "SELECT * FROM Motor WHERE Id_motor=" + id;
                        using (SqlCommand dataCommand = new SqlCommand(pridobitev, dataConnection))
                        {
                            using (SqlDataReader reader = dataCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    znamka = (string)reader.GetValue(1);
                                    barva = (string)reader.GetValue(2);
                                    prevozeni = (int)reader.GetValue(3);
                                    cena = (double)reader.GetValue(4);
                                    moc = (int)reader.GetValue(5);
                                    id_kategorija = (int)reader.GetValue(6);
                                }
                            }
                        }
                    }
                

                    Console.WriteLine("Vpisi novo znamko, trenutna:" + znamka);
                    string z = Console.ReadLine();
                    Console.WriteLine("Vpisi novo barvo, trenutna:" + barva);
                    string b = Console.ReadLine();
                    Console.WriteLine("Vpisi nove prevozene, trenutna:" +prevozeni);
                    int p = int.Parse(Console.ReadLine());
                    Console.WriteLine("Vpisi novo ceno, trenutna:" + cena);
                    double c = double.Parse(Console.ReadLine());
                    Console.WriteLine("Vpisi novo moc, trenutna:" + moc);
                    int m = int.Parse(Console.ReadLine());
                    Console.WriteLine("Vpisi novo kategorijo, trenutna:" + id_kategorija);
                    int fkk = int.Parse(Console.ReadLine());
                    string poizvedba = "UPDATE Motor SET znamka=@z,barva=@b,prevozeni=@p,cena=@c,moc=@m,id_kategorija=@fkk WHERE Id_motor =" +id;

                    using (SqlConnection dataConnection = new SqlConnection(conn))
                    {
                        dataConnection.Open();
                        using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                        {
                            dataCommand.Parameters.AddWithValue("@z", z);
                            dataCommand.Parameters.AddWithValue("@b", b);
                            dataCommand.Parameters.AddWithValue("@p", p);
                            dataCommand.Parameters.AddWithValue("@c", c);
                            dataCommand.Parameters.AddWithValue("@m", m);
                             dataCommand.Parameters.AddWithValue("@fkk", fkk);
                        dataCommand.ExecuteNonQuery();
                        }
                        dataConnection.Close();
                     }
                 }
                catch
                {
                    Console.WriteLine("Napaka pri vnosu");
                    return (1);
                }
          return 0;
        }
        public static int izbiranjeVnosa (int id)
        {
            Console.Clear();
            string conn = dbConn();
            string poizvedba = "DELETE FROM Motor Where Id_motor=" +id;
            try
            {
                using (SqlConnection dataConnection = new SqlConnection(conn))
                {
                    dataConnection.Open();
                    using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                    {
                        dataCommand.ExecuteNonQuery();
                    }
                    dataConnection.Close();
                }
            }
            catch
            {
                Console.WriteLine("Ne dela");
                return 1;
            }
            return 0;

        }
        static void ExecuteReader()
        {
            Console.WriteLine("poizvedbe - execute");
            string conn = dbConn();
            Console.WriteLine("Poizvedba, kjer so prevoženi km motorjev manjši od 10000km: \n");
            Console.WriteLine("ID_motor|znamka|barva|prevozeni|cena|moc|Id_kategorija");
            using (SqlConnection dataConnection = new SqlConnection(conn))
            {
                string poizvedba = "SELECT * FROM Motor WHERE prevozeni < 10000";
                dataConnection.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                {
                    using (SqlDataReader reader = dataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetValue(0) + "       " + reader.GetValue(1) + "" + reader.GetValue(2) + "  " + reader.GetValue(3) + "   " + reader.GetValue(4) + "  " + reader.GetValue(5) + "    " + reader.GetValue(6));
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Poizvedba, vseh kategorij: \n");
            Console.WriteLine("Id_kategorija|kategorija");
            using (SqlConnection dataConnection = new SqlConnection(conn))
            {
                string poizvedba = "SELECT * FROM Kategorija";
                dataConnection.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                {
                    using (SqlDataReader reader = dataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetValue(0) + "               " + reader.GetValue(1));
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Poizvedba, katere motorje je mogoče voziti z katero kategorijo: \n");
            using (SqlConnection dataConnection = new SqlConnection(conn))
            {
                string poizvedba = "SELECT Motor.znamka, Kategorija.kategorija FROM Motor INNER JOIN Kategorija ON Motor.Id_kategorija=Kategorija.Id_kategorija";
                dataConnection.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
                {
                    using (SqlDataReader reader = dataCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetValue(0) + " -> " + reader.GetValue(1));
                        }
                    }
                }
            }

            Console.WriteLine();
        }
        static void ExecuteScalar()
        {
            Console.WriteLine("Poizvedbe - Scalar");
            string conn = dbConn();
            using (SqlConnection dataConn = new SqlConnection(conn))
            {
                string poizvedba = "SELECT COUNT(*) FROM Motor";
                dataConn.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConn))
                {
                    Console.WriteLine("Vseh zapisov v tabeli Motor je: "+dataCommand.ExecuteScalar().ToString());
                }
                dataConn.Close();
            }
            Console.WriteLine();
            using (SqlConnection dataConn = new SqlConnection(conn))
            {
                string poizvedba = "SELECT SUM(cena) FROM Motor";
                dataConn.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConn))
                {
                    Console.WriteLine("Vsi motorji so skupaj vredni: " + dataCommand.ExecuteScalar().ToString() + " eur");
                }
                dataConn.Close();
            }
            Console.WriteLine();
            using (SqlConnection dataConn = new SqlConnection(conn))
            {
                string poizvedba = "SELECT MIN(prevozeni) FROM Motor";
                dataConn.Open();
                using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConn))
                {
                    Console.WriteLine("Motor z najmanj prevoženimi km. ima: "+dataCommand.ExecuteScalar().ToString()+" km");
                }
                dataConn.Close();
            }
            Console.WriteLine("");
        }

        static void ListvGen()
        {
            List<Motor> motorji = new List<Motor>();
            Deserializacija<Motor>(motorji, "motorji.XML");
            foreach(Motor m in motorji)
            {
                genmotorji.Add(m);
            }
        }
        static void GenSerelizacija<T>(GenericnaZbirka<T> zbirka)
        {
            Console.WriteLine("Genericna - XML Serializacija");
            Console.Clear();
            List<T> temp = new List<T>();
            for (int i = 0; i < zbirka.Velikost; i++)
            {
                temp.Add(zbirka[i]);
            }
            Serializacija<T>(temp, "motorji.xml");
            Console.WriteLine("Generična zbirka je bila serializirana!");
        }
        static void Moznosti(GenericnaZbirka<Motor> motorji)
        {
            Console.WriteLine("                         GLAVNI MENI      \n");
            Console.WriteLine("                     1 - BAZA PODATKOV    \n");
            Console.WriteLine("                2 - LASTNA GENERIČNA ZBIRKA    \n");
            Console.WriteLine("           3 - LASTNI ENOSMERNI VERIŽNI SEZNAM  \n");
            Console.WriteLine("                       4 - Izhod  \n");
            Console.WriteLine("                       5 - Vizitka  \n");
            Console.Write("\nVnesi izbiro: ");
            string izbira = Console.ReadLine();
            if (izbira == "4") return;
            if (izbira == "5") Vizitka();
            Izbira(izbira, motorji);
            Console.WriteLine("\nPritisnite gumb za nadaljevanje.\n");
            Console.ReadKey();
            Console.Clear();
            Moznosti(motorji); //rekurzija
        }
        static void Izbira(string izbira, GenericnaZbirka<Motor> motorji)
        {
            try
            {
                switch (izbira)
                {
                    case "1": 
                        Console.WriteLine("                    -BAZA PODATKOV-          ");
                        Console.WriteLine("             1 - Prikaz vsebine ene od tabel         ");
                        Console.WriteLine("             2 - Dodajanje novega zapisa            ");
                        Console.WriteLine("             3 - Urejanje zapisa                  ");
                        Console.WriteLine("             4 - Brisanje zapisa                  ");
                        Console.WriteLine("             5 - Poizvedba tipa ExecuteReader        ");
                        Console.WriteLine("             6 - Poizvedba tipa ExecuteScalar        ");
                        string o = Console.ReadLine();
                        switch (o)
                        {
                            case "1":
                                Console.WriteLine("1 - Tabela Motorji");
                                Console.WriteLine("2 - Tabela Kategorija");
                                string k = Console.ReadLine();
                                switch (k)
                                {
                                    case "1":
                                        izpisbazeMotor();
                                        break;
                                    case "2":
                                        izpisbazeKategorija();
                                        break;
                                }
                                break;
                            case "2":
                                zapisDB();
                                break;
                            case "3":
                                izpisbazeMotor();
                                Console.WriteLine("Vnesite ID atributa, katerega želite urejati: ");
                                int id= Convert.ToInt32(Console.ReadLine());

                                urejanjeVnosa(id);
                                break;
                            case "4":
                                izpisbazeMotor();
                                Console.WriteLine("Vnesite ID atributa, katereg želite izbrisati: ");
                                int del = Convert.ToInt32(Console.ReadLine());
                                izbiranjeVnosa(del);
                                break;
                            case "5":
                                ExecuteReader();
                                break;
                            case "6":
                                ExecuteScalar();
                                break;
                        }
                         break; 
                    case "2": 
                        Console.WriteLine("               -LASTNA GENERIČNA ZBIRKA-     ");
                        Console.WriteLine("             1 - Izpis vsebine zbirke              ");
                        Console.WriteLine("             2 - Filtriraj ");
                        Console.WriteLine("             3 - Brisanje duplikatov ");
                        Console.WriteLine("             4 - Pretvori v sklad");
                        Console.WriteLine("             5 - Uredi");
                        Console.WriteLine("             6 - Serializacija v .XML             ");
                        string u = Console.ReadLine();
                        switch (u)
                        {
                            case "1":
                                genmotorji.IzpisZbirke();
                                break;
                            case "2":
                                Console.WriteLine("Napiši ime atributa preko katerega, želite filtrirati zbirko: ");
                                string atr = (Console.ReadLine().ToLower());
                                Console.WriteLine("Napiši vrednost atributa: ");
                                string vre = (Console.ReadLine().ToLower());
                                switch (atr)
                                {
                                    case "znamka":
                                        genmotorji.Filter((o, Z) => (o.znamka.ToLower() == vre)).IzpisZbirke();
                                        break;
                                    case "barva":
                                        genmotorji.Filter((o, Z) => (o.barva.ToLower() == vre)).IzpisZbirke();
                                        break;
                                    case "prevozeni":
                                        genmotorji.Filter((o, Z) => (o.prevozeni.ToString() == vre)).IzpisZbirke();
                                        break;
                                    case "cena":
                                        genmotorji.Filter((o, Z) => (o.cena.ToString() == vre)).IzpisZbirke();
                                        break;
                                    case "moc":
                                        genmotorji.Filter((o, Z) => (o.moc.ToString() == vre)).IzpisZbirke();
                                        break;
                                    case "kategorija":
                                        genmotorji.Filter((o, Z) => (o.kategorija.ToLower() == vre)).IzpisZbirke();
                                        break;
                                    default: Console.WriteLine("Atribut ne obstaja!"); break;
                                }
                                
                                break;
                            case "3":
                                //ciscenje duplikatov
                                genmotorji.OdstraniDuplikate();
                                Console.WriteLine("Iz zbirke ste izbirsali dublikate");
                                break;
                            case "4":
                                Stack<Motor> novsklad = genmotorji.ToStack();
                                Console.WriteLine("Naredili ste sklad z velkostjo "+ novsklad.Count.ToString());
                                if (novsklad.Count > 0)
                                {
                                    Console.WriteLine("Vrhnji element:\n" + novsklad.Peek().ToString());

                                }
                                break;
                            case "5":
                                Console.WriteLine("Po katerem atributu želite razvrstiti motorje: ");
                                string y = Console.ReadLine();
                                genmotorji.Sort(Motor.GetComparer(y));
                                break;
                            case "6":
                                GenSerelizacija(genmotorji);
                                break;
                        }
                        break;
                    case "3": //Dodajanje
                        Console.WriteLine("                       -VERŽINI SEZNAM-         ");
                        Console.WriteLine("             1 - Izpis verižnega seznama       "); 
                        Console.WriteLine("             2 - Dodaj generično zbirko v verižni seznam     ");
                        Console.WriteLine("             3 - Počisti seznam     ");
                        Console.WriteLine("             4 - Izbriši podatek    ");
                        Console.WriteLine("             5 - Dodaj podatek    ");
                        string d = Console.ReadLine();
                        switch(d){
                            case "1":
                                verizni.Izpis();
                                break;
                            case "2":
                                verizni.DodajVse(genmotorji);
                                break;
                            case "3":
                                verizni.Pocisti();
                                break;
                            case "4":
                                verizni.Izpis();
                                Console.WriteLine("Izberi indeks v seznamu, kjer želite pobrisati podatke: 0 => prvo mesto");
                                int indeks = Convert.ToInt32(Console.ReadLine());
                                verizni.Zbrisi(indeks);
                                Console.WriteLine("Podatek je bil izbrisan");
                                break;
                            case "5":
                                Motor motor = NovMotor();
                                if (motor != null)
                                {
                                    verizni.Dodaj(motor);
                                    Console.WriteLine("Motor je bil dodan");
                                }
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Napačna izbira!");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Pri izbiri objekta je prišlo do izjeme: " + e.Message);
            }
        }
        public static void Vizitka()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\n");
            NaSredino("|->->VIZITKA<-<-|");
            Console.WriteLine("\n\n\n");
            Console.ForegroundColor = ConsoleColor.Green;
            NaSredino("|----Avtor---|");
            Console.ForegroundColor = ConsoleColor.Blue;
            NaSredino("|-Žan Horvat-|");
            Console.ForegroundColor = ConsoleColor.Green;
            NaSredino("|---Inf 2.---|");
            Console.WriteLine("\n");
            NaSredino("-SEMINARSKA NALOGA PRI PREDMETU PROGRAMIRANJE 2-");
            NaSredino("-MOTOR-");
            Console.WriteLine("\n");
            NaSredino(">SOLSKI CENTER KRANJ<");
            Console.WriteLine("\n");
            NaSredino(">Solsko leto<");
            NaSredino(">2021/2022<");
            Console.WriteLine("\n");
            NaSredino(">Ljubljana, januar 2022<");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n\n\n\n\n");
        }
        public static void NaSredino(string a)
        {
            Console.WriteLine(String.Format("{0," + (Console.WindowWidth / 2 + a.Length / 2) + "}", a));
        }
    }
}