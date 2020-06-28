using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Klijent.ServiceReference1;

namespace Klijent
{
    public partial class frmKlijent : Form
    {
        string ulogovaniKorisnik;
        JavniClient klijentJavni;
        LogInClient klijentLogin;
        ListaKnjiga knjige;
        ListaNaloga nalozi;

        public frmKlijent(string KorisnickoIme)
        {
            InitializeComponent();
            this.ulogovaniKorisnik = KorisnickoIme;
            klijentJavni = new JavniClient();
            klijentLogin = new LogInClient();
            knjige = klijentJavni.PrikazKnjiga();
            nalozi = klijentLogin.VratiSveNaloge();          
            this.StartPosition = FormStartPosition.CenterScreen;
            lblUlogovaniKorisnik.Select();
        }

        private void frmKlijent_Load(object sender, EventArgs e)
        {
            lblUlogovaniKorisnik.Text = "Ulogovani ste kao " + this.ulogovaniKorisnik.ToUpper();

            // Dinamicko crtanje elemenata za svaku knjigu
            int pomeraj = 61;
            foreach (Knjiga knjiga in knjige)
            {
                TextBox txtBox = new TextBox();
                txtBox.Text = knjiga.Naziv + " (" + knjiga.Autor + "), CENA: " + knjiga.Cena;
                if (knjiga.Popust != 0)
                {
                    txtBox.Text += ", POPUST: " + knjiga.Popust + "%";
                }
                txtBox.Location = new System.Drawing.Point(34, pomeraj);
                txtBox.ReadOnly = true;
                txtBox.Size = new System.Drawing.Size(350, 100);                
                txtBox.Name = "txtKnjiga"+knjiga.Id_knjige;

                Button btnSmanjiKolicinu = new Button();
                btnSmanjiKolicinu.Text = "-";
                btnSmanjiKolicinu.Font = new Font("Microsoft Sans Serif", 14);
                btnSmanjiKolicinu.Location = new System.Drawing.Point(400, pomeraj);
                btnSmanjiKolicinu.Size = new System.Drawing.Size(50, 25);
                btnSmanjiKolicinu.Name = "btnSmanjiKolicinu" + knjiga.Id_knjige;
                

                TextBox txtBoxKolicina = new TextBox();
                txtBoxKolicina.Text = "1";
                txtBoxKolicina.ReadOnly = true;
                txtBoxKolicina.Location = new System.Drawing.Point(470, pomeraj);
                txtBoxKolicina.Size = new System.Drawing.Size(50, 100);
                txtBoxKolicina.TextAlign = HorizontalAlignment.Center;
                txtBoxKolicina.Name = "txtKolicina"+knjiga.Id_knjige;

                Button btnPovecajKolicinu = new Button();
                btnPovecajKolicinu.Text = "+";
                btnPovecajKolicinu.Font = new Font("Microsoft Sans Serif", 14);
                btnPovecajKolicinu.Location = new System.Drawing.Point(540, pomeraj);
                btnPovecajKolicinu.Size = new System.Drawing.Size(50, 25);
                btnPovecajKolicinu.Name = "btnPovecajKolicinu" + knjiga.Id_knjige;
                btnPovecajKolicinu.Click += new EventHandler(dugmePovecaj_Click);

                Button btnNaruci = new Button();
                btnNaruci.Text = "Naruči";
                btnNaruci.Font = new Font("Microsoft Sans Serif", 10);
                btnNaruci.Location = new System.Drawing.Point(610, pomeraj);
                btnNaruci.Size = new System.Drawing.Size(150, 25);
                btnNaruci.Name = "btnNaruci" + knjiga.Id_knjige;
                btnNaruci.Click += new EventHandler(dugmeNaruci_Click);

                pomeraj += 40;
                btnSmanjiKolicinu.Click += new EventHandler(dugmeSmanji_Click);
                this.Controls.Add(txtBox);
                this.Controls.Add(txtBoxKolicina);
                this.Controls.Add(btnSmanjiKolicinu);
                this.Controls.Add(btnPovecajKolicinu);
                this.Controls.Add(btnNaruci);

            }
        }

        void dugmeNaruci_Click(object sender, EventArgs e)
        {
            Button kliknuto = (Button)sender;
            int idKnjige = int.Parse(kliknuto.Name.Substring(kliknuto.Name.Length - 1));

            Korisnik kupac = new Korisnik();
            foreach (Korisnik k in klijentLogin.VratiSveNaloge())
            {
                if (k.Korisnicko_ime.Equals(ulogovaniKorisnik))
                {
                    kupac = k;
                    break;
                }
            }

            Knjiga kupljena = new Knjiga();
            foreach (Knjiga knjiga in knjige)
            {            
                if(knjiga.Id_knjige == idKnjige)
                {
                    kupljena = knjiga;
                }
            }

            foreach (Control c in Controls)
            {
                if (c is TextBox && c.Name == "txtKolicina" + idKnjige)
                {
                    kupljena.Kolicina = int.Parse(c.Text);
                    break;
                }
            }
            klijentJavni.PoruciKnjigu(kupljena, kupac);
            MessageBox.Show("Uspešno naručena knjiga!");
            kliknuto.Text = "Ponovo naruči";

            Porudzbine p = new Porudzbine();
            p.Id_porudzbine = 0;
            p.Id_korisnika = kupac.Id_korisnika;
            p.Id_knjige = kupljena.Id_knjige;
            p.Kolicina = kupljena.Kolicina;

            string upis = "Knjiga: " + kupljena.Naziv + " (" + kupljena.Autor + "), količina: " + p.Kolicina;

            double ukupnaCena = 0;
            if (kupljena.Popust != 0)
            {
                ukupnaCena = kupljena.Cena - (kupljena.Cena * (kupljena.Popust / 100.0));
                ukupnaCena = ukupnaCena * p.Kolicina;
            }
            else
            {
                ukupnaCena = kupljena.Cena * p.Kolicina;
            }
            upis += ", ukupno: " + ukupnaCena + " din, vreme: " + DateTime.Now;
            lbPorudzbine.Items.Add(upis);
        }

        void dugmeSmanji_Click(object sender, EventArgs e)
        {
            Button kliknuto = (Button)sender;
            int idKnjige = int.Parse(kliknuto.Name.Substring(kliknuto.Name.Length - 1));
            foreach(Control c in Controls)
            {
                if (c is TextBox && c.Name == "txtKolicina" + idKnjige)
                {
                    int trenutno = int.Parse(c.Text);
                    int smanjeno = trenutno - 1;
                    if(smanjeno != 0)
                    {
                        c.Text = smanjeno.ToString();
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Minimum je 1 knjiga.");
                    }
                    
                }
            }      
        }

        void dugmePovecaj_Click(object sender, EventArgs e)
        {
            Button kliknuto = (Button)sender;
            int idKnjige = int.Parse(kliknuto.Name.Substring(kliknuto.Name.Length - 1));
            foreach (Control c in Controls)
            {
                if (c is TextBox && c.Name == "txtKolicina" + idKnjige)
                {
                    int trenutno = int.Parse(c.Text);
                    int smanjeno = trenutno + 1;
                     c.Text = smanjeno.ToString();
                    break;
                }
            }
        }

        private void btnOdjava_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            Hide();
            frm.ShowDialog();
            Close();
        }
    }
}
