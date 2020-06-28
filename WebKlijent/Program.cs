using Servis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebKlijent
{
    class Program
    {
        public static string ulogovaniKorisnik { get; set; }

        static void Main(string[] args)
        {
            ulogovaniKorisnik = "";
            Console.WriteLine("---------- KNJIZARA ----------");
            Opcije();
        }

        public static bool OpcijaUlogujSe()
        {
            Console.WriteLine(Environment.NewLine + "--- ULOGUJTE SE ---");
            Console.WriteLine("Korisnicko ime: ");
            string username = Console.ReadLine();
            Console.WriteLine("Lozinka: ");
            string password = Console.ReadLine();
            string uspesno = LogIn(username, password);

            if (uspesno.Equals("uspesno"))
            {
                ulogovaniKorisnik = username;
                Console.WriteLine(Environment.NewLine + "Ulogovani ste kao " + ulogovaniKorisnik.ToUpper());
                return true;
            }
            else
            {
                if(uspesno.Equals("Nalog pod ovim korisnickim imenom ne postoji!"))
                {
                    Console.WriteLine(uspesno);
                    Console.WriteLine(Environment.NewLine + "Zelite li da se registrujete? D/N");
                    string odgovor = Console.ReadLine();

                    if (odgovor.ToUpper().Equals("D"))
                    {
                        bool uspeh = OpcijaRegistrujSe();
                        return uspeh;
                    }
                    else
                    {
                        Opcije();
                    }
                }
                else
                {
                    // pogresna lozinka
                    Console.WriteLine(uspesno);
                    Opcije();
                }
            }

            return false;
        }

        public static bool OpcijaRegistrujSe()
        {
            Console.WriteLine(Environment.NewLine + "--- REGISTRACIJA ---");
            Console.WriteLine("Novo korisnicko ime: ");
            string novoKorIme = Console.ReadLine();
            Console.WriteLine("Nova lozinka: ");
            string novaLozinka = Console.ReadLine();
            string uspesnaRegistracija = RegistrujSe(novoKorIme, novaLozinka);

            if (uspesnaRegistracija.Equals("uspesno"))
            {
                Console.WriteLine(Environment.NewLine + "Uspesna registracija!");
                ulogovaniKorisnik = novoKorIme;
                Console.WriteLine(Environment.NewLine + "Ulogovani ste kao " + ulogovaniKorisnik.ToUpper());
                return true;
            }
            else
            {
                // vec postoji korisnicko ime
                Console.WriteLine(uspesnaRegistracija);
                Opcije();
            }

            return false;
        }

        private static void Opcije()
        {
            Console.WriteLine(Environment.NewLine + "Izaberite redni broj:");
            Console.WriteLine("1 - ULOGUJ SE");
            Console.WriteLine("2 - REGISTRACIJA");
            Console.WriteLine("3 - KRAJ");
            string opcija = Console.ReadLine();

            switch (opcija)
            {
                case "1":
                    bool uspesnoUlgovan = OpcijaUlogujSe();
                    if (uspesnoUlgovan)
                    {
                        OpcijeZaUlogovanogKorisnika();
                    }
                    break;
                case "2":
                    bool uspesnoRegistrovan = OpcijaRegistrujSe();
                    if (uspesnoRegistrovan)
                    {
                        OpcijeZaUlogovanogKorisnika();
                    }

                    break;
                case "3":
                    Console.WriteLine(Environment.NewLine + "Kraj programa...");
                    return;
                default:
                    Console.WriteLine(Environment.NewLine + "Niste izabrali nijednu opciju!");
                    Opcije();
                    break;
            }

        }

        private static void OpcijeZaNarucivanje()
        {
            Console.WriteLine(Environment.NewLine + "----- NARUCIVANJE KNJIGA -----");

            // prikaz knjiga
            IEnumerable<Knjiga> lista = VratiSveKnjige();
            foreach (Knjiga k in lista)
            {
                string ispis = k.Id_knjige + " - " + k.Naziv + " (" + k.Autor + "), CENA: " + k.Cena;
                if(k.Popust != 0)
                {
                    ispis += ", POPUST: " + k.Popust + "%";
                }
                Console.WriteLine(ispis);
            }

            Console.WriteLine(Environment.NewLine + "Izaberite redni broj knjige koju zelite da narucite:");

            bool postojiKnjiga = false;
            Knjiga izabrana = new Knjiga();

            string idNaruceneKnjigeString = Console.ReadLine();
            int idNaruceneKnjige = 0;

            if (int.TryParse(idNaruceneKnjigeString, out idNaruceneKnjige))
            {
                // provera da li knjiga postoji
                foreach (Knjiga k in lista)
                {
                    if (k.Id_knjige == idNaruceneKnjige)
                    {
                        postojiKnjiga = true;
                        izabrana = k;
                    }
                }
            }
            else
            {
                // nije unet broj
                Console.WriteLine(Environment.NewLine + "Niste uneli BROJ, pokusajte ponovo...");
                OpcijeZaNarucivanje();
            }

            if (postojiKnjiga)
            {
                Console.WriteLine("Koliko komada knjige " + izabrana.Naziv + " zelite?");

                string kolicinaString = Console.ReadLine();
                int kolicina = 0;

                if (int.TryParse(kolicinaString, out kolicina))
                {
                    // racunanje ukupne cene
                    double ukupnaCena = 0;
                    if (izabrana.Popust != 0)
                    {
                        ukupnaCena = izabrana.Cena - (izabrana.Cena * (izabrana.Popust / 100.0));
                        ukupnaCena = ukupnaCena * kolicina;
                    }
                    else
                    {
                        ukupnaCena = izabrana.Cena * kolicina;
                    }

                    // narucivanje
                    string uspesno = PoruciKnjigu(idNaruceneKnjigeString, ulogovaniKorisnik, kolicinaString);

                    if (uspesno.Equals("uspesno"))
                    {
                        Console.WriteLine(Environment.NewLine + "USPESNO STE NARUCILI KNJIGU " + izabrana.Naziv 
                            + " (" + kolicinaString + " kom)"
                            + Environment.NewLine + "UKUPNO ZA PLACANJE: " + ukupnaCena + " DIN"
                            + Environment.NewLine + "VREME PORUDZBINE: " + DateTime.Now);
                        OpcijeZaUlogovanogKorisnika();

                    }
                    else
                    {
                        Console.WriteLine(Environment.NewLine + "Kupovina nije uspela...");
                        OpcijeZaUlogovanogKorisnika();
                    }
                }
                else
                {
                    // nije unet broj
                    Console.WriteLine(Environment.NewLine + "Niste uneli BROJ, pokusajte ponovo...");
                    OpcijeZaNarucivanje();
                }
            }
            else
            {
                // knjiga sa tim ID ne postoji
                Console.WriteLine(Environment.NewLine + "Knjiga sa unetim rednim brojem ne postoji, pokusajte ponovo...");
                OpcijeZaNarucivanje();
            }

        }

        private static void OpcijeZaUlogovanogKorisnika()
        {
            Console.WriteLine(Environment.NewLine + "Izaberite redni broj:");
            Console.WriteLine("1 - NARUCI KNJIGU");
            Console.WriteLine("2 - ODJAVI SE");
            string opcija = Console.ReadLine();

            switch (opcija)
            {
                case "1":
                    OpcijeZaNarucivanje();
                    break;
                case "2":
                    Console.WriteLine(Environment.NewLine + "Odjava...");
                    Opcije();
                    break;
                default:
                    Console.WriteLine(Environment.NewLine + "Niste izabrali nijednu opciju!");
                    OpcijeZaUlogovanogKorisnika();
                    break;
            }
        }

        // WEB FUNKCIJE 
        public static string LogIn(string username, string password)
        {
            WebClient webClient = new WebClient();

            string serviceURL = string.Format("http://localhost:8100/Servis/Service1/web/login/username={0}&password={1}", username, password);

            MemoryStream mem = new MemoryStream();

            string podaciStr = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

            var rez = webClient.UploadString(serviceURL, "PUT", podaciStr); // rez = \"uspesno\"

            return rez.ToString().Replace('"',' ').TrimStart().TrimEnd(); // rez = uspesno
        }

        public static string RegistrujSe(string username, string password)
        {
            WebClient webClient = new WebClient();

            string serviceURL = string.Format("http://localhost:8100/Servis/Service1/web/registruj/username={0}&password={1}", username, password);

            MemoryStream mem = new MemoryStream();

            string podaciStr = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

            var rez = webClient.UploadString(serviceURL, "PUT", podaciStr); // rez = \"uspesno\"

            return rez.ToString().Replace('"', ' ').TrimStart().TrimEnd(); // rez = uspesno
        }

        public static IEnumerable<Knjiga> VratiSveKnjige()
        {
            WebClient webClient = new WebClient();
            string serviceURL = string.Format("http://localhost:8100/Servis/Service1/web/sveKnjige");
            byte[] data = webClient.DownloadData(serviceURL);

            string jsonStr = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<IEnumerable<Knjiga>>(jsonStr);           
        }

        public static string PoruciKnjigu(string idKnjige, string korisnickoIme, string kolicina)
        {
            WebClient webClient = new WebClient();

            string serviceURL = string.Format("http://localhost:8100/Servis/Service1/web/poruciKnjigu/idKnjige={0}&korisnickoIme={1}&kolicina={2}", idKnjige, korisnickoIme, kolicina);

            MemoryStream mem = new MemoryStream();

            string podaciStr = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);

            var rez = webClient.UploadString(serviceURL, "PUT", podaciStr); // rez = \"uspesno\"

            return rez.ToString().Replace('"', ' ').TrimStart().TrimEnd(); // rez = uspesno
        }
    }
}
