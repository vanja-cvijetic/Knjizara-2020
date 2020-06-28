using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Servis
{
    [ServiceContract]
    public interface IWeb
    {
        [WebInvoke(Method = "PUT", UriTemplate = "login/username={username}&password={password}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        string webLogIn(string username, string password);

        [WebInvoke(Method = "PUT", UriTemplate = "registruj/username={username}&password={password}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        string webRegistrujSe(string username, string password);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "GET", UriTemplate = "sveKnjige")]
        [OperationContract]
        IEnumerable<Knjiga> webPrikazKnjiga();

        [WebInvoke(Method = "PUT", UriTemplate = "poruciKnjigu/idKnjige={idKnjige}&korisnickoIme={korisnickoIme}&kolicina={kolicina}", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [OperationContract]
        string webPoruciKnjigu(string idKnjige, string korisnickoIme, string kolicina);
    }

    [ServiceContract]
    public interface ILogIn
    {
        [OperationContract]
        bool LogIn(string username, string password);

        [OperationContract]
        ListaNaloga VratiSveNaloge();

        [OperationContract]
        string RegistrujSe(string username, string password);
    }

    [ServiceContract]
    public interface IJavni
    {
        [OperationContract]
        ListaKnjiga PrikazKnjiga();

        [OperationContract]
        void PoruciKnjigu(Knjiga kupljena, Korisnik ulogovanKorisnik);
    }

    [ServiceContract]
    public interface ISistem
    {
        [OperationContract]
        void UnosKnjige(int id, string naziv, string autor, double cena, int popust, int kolicina);

        [OperationContract]
        void IzmenaKorisnika(int id, string username, string password);

        [OperationContract]
        void IzmenaKnjige(Knjiga izmenjenaKnjiga);

        [OperationContract]
        ListaPorudzbina PregledPorudzbina();
    }

    [ServiceContract]
    public interface IAdmin
    {
        [OperationContract]
        void ObrisiKnjigu(Knjiga knjiga);

        [OperationContract]
        void ObrisiNalog(Korisnik korisnik);
    }

    [DataContract]
    public class Korisnik
    {
        private int id_korisnika;
        private string korisnicko_ime;
        private string lozinka;

        public Korisnik() { }

        public Korisnik(int id, string korisnicko, string lozinka)
        {
            this.id_korisnika = id;
            this.korisnicko_ime = korisnicko;
            this.lozinka = lozinka;
        }

        [DataMember]
        public int Id_korisnika
        {
            get { return id_korisnika; }
            set { id_korisnika = value; }
        }

        [DataMember]
        public string Korisnicko_ime
        {
            get { return korisnicko_ime; }
            set { korisnicko_ime = value; }
        }

        [DataMember]
        public string Lozinka
        {
            get { return lozinka; }
            set { lozinka = value; }
        }
    }

    [DataContract]
    public class Knjiga
    {
        private int id_knjige;
        private string naziv;
        private string autor;
        private double cena;
        private int popust;
        private int kolicina;

        public Knjiga(int id_knjige, string naziv, string autor, double cena, int popust, int kolicina)
        {
            this.id_knjige = id_knjige;
            this.naziv = naziv;
            this.autor = autor;
            this.cena = cena;
            this.popust = popust;
            this.kolicina = kolicina;
        }

        public Knjiga() { }

        [DataMember]
        public int Id_knjige
        {
            get { return id_knjige; }
            set { id_knjige = value; }
        }

        [DataMember]
        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        [DataMember]
        public string Autor
        {
            get { return autor; }
            set { autor = value; }
        }

        [DataMember]
        public double Cena
        {
            get { return cena; }
            set { cena = value; }
        }

        [DataMember]
        public int Popust
        {
            get { return popust; }
            set { popust = value; }
        }

        [DataMember]
        public int Kolicina
        {
            get { return kolicina; }
            set { kolicina = value; }
        }
    }

    [DataContract]
    public class Porudzbine
    {
        private int id_porudzbine;
        private int id_knjige;
        private int id_korisnika;
        private int kolicina;

        public Porudzbine() { }

        public Porudzbine(int id_porudzbine, int id_knjiga,int id_korisnik, int kolicina) {
            this.id_porudzbine = id_porudzbine;
            this.id_knjige = id_knjiga;
            this.id_korisnika = id_korisnik;
            this.kolicina = kolicina;
        }

        [DataMember]
        public int Id_porudzbine
        {
            get { return id_porudzbine; }
            set { id_porudzbine = value; }
        }

        [DataMember]
        public int Id_knjige
        {
            get { return id_knjige; }
            set { id_knjige = value; }
        }

        [DataMember]
        public int Id_korisnika
        {
            get { return id_korisnika; }
            set { id_korisnika = value; }
        }

        [DataMember]
        public int Kolicina
        {
            get { return kolicina; }
            set { kolicina = value; }
        }
    }

    [CollectionDataContract]
    public class ListaNaloga : List<Korisnik> { }

    [CollectionDataContract]
    public class ListaKnjiga : List<Knjiga>{ }

    [CollectionDataContract]
    public class ListaPorudzbina : List<Porudzbine> { }
}
