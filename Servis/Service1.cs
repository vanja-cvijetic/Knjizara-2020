using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Servis
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service1 : ILogIn, ISistem, IJavni, IAdmin, IWeb
    {
        private ListaKnjiga knjige;
        private ListaNaloga nalozi;
        private ListaPorudzbina svePorudzbine;

        public Service1()
        {
            // vec postojece knjige
            knjige = new ListaKnjiga();
            Knjiga k1 = new Knjiga(1, "Saptac", "Donato Karizi", 600, 0, 0);
            Knjiga k2 = new Knjiga(2, "Igre gladi", "Suzan Kolins", 550, 0, 0);
            Knjiga k3 = new Knjiga(3, "Lovac u zitu", "Dz. D. Selindzer", 999, 10, 0);
            Knjiga k4 = new Knjiga(4, "Hari Poter", "Dz. K. Rouling", 1200, 50, 0);
            Knjiga k5 = new Knjiga(5, "Stranac", "Alber Kami", 400, 2, 0);
            knjige.Add(k1); knjige.Add(k2); knjige.Add(k3); knjige.Add(k4); knjige.Add(k5);

            // vec postojeci nalozi
            nalozi = new ListaNaloga();
            Korisnik admin = new Korisnik(100, "admin", "admin");
            Korisnik operater = new Korisnik(200, "operater", "operater");
            Korisnik user1 = new Korisnik(300, "vanjanrt2917", "vanja123");
            Korisnik user2 = new Korisnik(400, "pera", "pera123");
            nalozi.Add(admin); nalozi.Add(operater); nalozi.Add(user1); nalozi.Add(user2);

            // vec postojece porudzbine
            svePorudzbine = new ListaPorudzbina();
            Porudzbine p1 = new Porudzbine(1000, 2, 300, 2);
            Porudzbine p2 = new Porudzbine(1001, 4, 400, 1);
            svePorudzbine.Add(p1); svePorudzbine.Add(p2);
        }


        // ISistem
        public void IzmenaKnjige(Knjiga izmenjenaKnjiga)
        {
            foreach(Knjiga k in knjige)
            {
                if(k.Id_knjige == izmenjenaKnjiga.Id_knjige)
                {
                    k.Naziv = izmenjenaKnjiga.Naziv;
                    k.Autor = izmenjenaKnjiga.Autor;
                    k.Cena = izmenjenaKnjiga.Cena;
                    k.Popust = izmenjenaKnjiga.Popust;
                    k.Kolicina = izmenjenaKnjiga.Kolicina;
                }
            }
        }      

        public ListaPorudzbina PregledPorudzbina()
        {
            return svePorudzbine;
        }    

        public void UnosKnjige(int id, string naziv, string autor, double cena, int popust, int kolicina)
        {
            Knjiga k = new Knjiga(id, naziv, autor, cena, popust, kolicina);
            knjige.Add(k);
        }

        public void IzmenaKorisnika(int id, string username, string password)
        {
            foreach(Korisnik k in nalozi)
            {
                if (k.Id_korisnika == id)
                {
                    k.Korisnicko_ime = username;
                    k.Lozinka = password;
                }
            }
        }


        // IJavni
        public ListaKnjiga PrikazKnjiga()
        {
            return knjige;
        }

        public void PoruciKnjigu(Knjiga kupljena, Korisnik ulogovanKorisnik)
        {
            // za svaku knjigu se pravi nova porudzbina
            Porudzbine novaPorudzbina = new Porudzbine();
            novaPorudzbina.Id_porudzbine = svePorudzbine.Count + 1000;
            novaPorudzbina.Id_knjige = kupljena.Id_knjige;
            novaPorudzbina.Id_korisnika = ulogovanKorisnik.Id_korisnika;
            novaPorudzbina.Kolicina = kupljena.Kolicina;
            svePorudzbine.Add(novaPorudzbina);
        }


        // ILogIn
        public ListaNaloga VratiSveNaloge()
        {
            return nalozi;
        }

        public bool LogIn(string username, string password)
        {
            foreach (Korisnik kor in nalozi)
            {
                if (kor.Korisnicko_ime.Equals(username) & kor.Lozinka.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }

        public string RegistrujSe(string username, string password)
        {
            foreach(Korisnik nalog in nalozi)
            {
                if (nalog.Korisnicko_ime.Equals(username))
                {
                    return "Korisničko ime već postoji!";
                }
            }
            Korisnik novi = new Korisnik();
            int idPrethodnog = nalozi[nalozi.Count - 1].Id_korisnika;
            novi.Id_korisnika = idPrethodnog + 100;
            novi.Korisnicko_ime = username;
            novi.Lozinka = password;
            nalozi.Add(novi);
            return "Uspešno kreiran nalog!";
        }


        // IAdmin
        public void ObrisiNalog(Korisnik korisnik)
        {
            Korisnik obrisi = new Korisnik();
            foreach(Korisnik nalog in nalozi)
            {
                if(nalog.Id_korisnika == korisnik.Id_korisnika)
                {
                    obrisi = nalog;
                    break;
                }
            }
            nalozi.Remove(obrisi);
        }

        public void ObrisiKnjigu(Knjiga knjiga)
        {
            Knjiga obrisi = new Knjiga();
            foreach (Knjiga k in knjige)
            {
                if (k.Id_knjige == knjiga.Id_knjige)
                {
                    obrisi = k;
                    break;
                }
            }
            knjige.Remove(obrisi);
        }


        // IWeb 
        public string webLogIn(string username, string password)
        {
            bool korisnickoImePostoji = false;
            foreach (Korisnik kor in nalozi)
            {              
                if (kor.Korisnicko_ime.Equals(username) & kor.Lozinka.Equals(password))
                {
                    return "uspesno";
                }
                if (kor.Korisnicko_ime.Equals(username))
                {
                    korisnickoImePostoji = true;
                }
            }
            if (korisnickoImePostoji)
            {
                return "Pogresna lozinka!";
            }
            else
            {
                return "Nalog pod ovim korisnickim imenom ne postoji!";
            }
        }

        public string webRegistrujSe(string username, string password)
        {
            foreach (Korisnik nalog in nalozi)
            {
                if (nalog.Korisnicko_ime.Equals(username))
                {
                    return "Korisnicko ime vec postoji!";
                }
            }
            Korisnik novi = new Korisnik();
            novi.Id_korisnika = (nalozi.Count * 100) + 100;
            novi.Korisnicko_ime = username;
            novi.Lozinka = password;
            nalozi.Add(novi);
            return "uspesno";
        }

        public IEnumerable<Knjiga> webPrikazKnjiga()
        {
            return knjige;
        }

        public string webPoruciKnjigu(string idKnjige, string korisnickoIme, string kolicina)
        {
            // za svaku knjigu se pravi nova porudzbina
            Knjiga knjiga = new Knjiga();
            foreach(Knjiga k in knjige)
            {
                if(k.Id_knjige == int.Parse(idKnjige))
                {
                    knjiga = k;
                }
            }

            Korisnik korisnik = new Korisnik();
            foreach(Korisnik kor in nalozi)
            {
                if (kor.Korisnicko_ime.Equals(korisnickoIme))
                {
                    korisnik = kor;
                }
            }

            Porudzbine novaPorudzbina = new Porudzbine();
            novaPorudzbina.Id_porudzbine = svePorudzbine.Count + 1000;
            novaPorudzbina.Id_knjige = knjiga.Id_knjige;
            novaPorudzbina.Id_korisnika = korisnik.Id_korisnika;
            novaPorudzbina.Kolicina = int.Parse(kolicina);
            svePorudzbine.Add(novaPorudzbina);

            return "uspesno";
        }
    }
}
