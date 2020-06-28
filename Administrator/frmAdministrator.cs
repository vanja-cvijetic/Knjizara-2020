using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Administrator.ServiceReference1;

namespace Administrator
{
    public partial class frmAdministrator : Form
    {
        JavniClient klijentJavni;
        SistemClient klijentSistem;
        LogInClient klijentLogin;
        AdminClient klijentAdmin;
        ListaKnjiga knjige;
        ListaNaloga nalozi;
        ListaPorudzbina porudzbine;

        public frmAdministrator()
        {
            InitializeComponent();
            klijentJavni = new JavniClient();
            klijentSistem = new SistemClient();
            klijentLogin = new LogInClient();
            klijentAdmin = new AdminClient();
            knjige = klijentJavni.PrikazKnjiga();
            nalozi = klijentLogin.VratiSveNaloge();
            porudzbine = klijentSistem.PregledPorudzbina();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmAdministrator_Load(object sender, EventArgs e)
        {
            foreach (Knjiga knjiga in knjige)
            {
                if (!cbKnjigeIzmena.Items.Contains(knjiga))
                {
                    cbKnjigeIzmena.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                }
                if (!cbKnjigeBrisanje.Items.Contains(knjiga))
                {
                    cbKnjigeBrisanje.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                }
            }

            foreach (Korisnik nalog in nalozi)
            {
                if (!cbKorisnici.Items.Contains(nalog))
                {
                    cbKorisnici.Items.Add(nalog.Korisnicko_ime);
                }
                if (!cbNaloziBrisanje.Items.Contains(nalog))
                {
                    cbNaloziBrisanje.Items.Add(nalog.Id_korisnika + " " + nalog.Korisnicko_ime);
                }
            }

            string upis = "";

            foreach (Porudzbine p in porudzbine)
            {
                upis = "ID porudžbine: " + p.Id_porudzbine + ", korisnik: ";

                foreach (Korisnik korisnik in nalozi)
                {
                    if (p.Id_korisnika == korisnik.Id_korisnika)
                    {
                        upis += korisnik.Korisnicko_ime;

                        foreach (Knjiga knjiga in knjige)
                        {
                            if (p.Id_knjige == knjiga.Id_knjige)
                            {
                                upis += ", knjiga: " + knjiga.Naziv + " (" + knjiga.Autor + "), količina: " + p.Kolicina
                                    + ", cena: " + knjiga.Cena;

                                double ukupnaCena = 0;
                                if (knjiga.Popust != 0)
                                {
                                    upis += ", popust: " + knjiga.Popust + "%";
                                    ukupnaCena = knjiga.Cena - (knjiga.Cena * (knjiga.Popust / 100.0));
                                    ukupnaCena = ukupnaCena * p.Kolicina;
                                }
                                else
                                {
                                    ukupnaCena = knjiga.Cena * p.Kolicina;
                                }
                                upis += ", ukupno: " + ukupnaCena + " din";
                                listBoxPorudzbine.Items.Add(upis);
                            }
                        }
                    }
                }

            }
        }

        private void btnUnosNoveKnjige_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNaziv.Text) &&
                !string.IsNullOrEmpty(txtAutor.Text) &&
                !string.IsNullOrEmpty(txtCena.Text) &&
                !string.IsNullOrEmpty(txtPopust.Text))
            {
                Knjiga novaKnjiga = new Knjiga();

                int idPrethodneKnjige = knjige[knjige.Count - 1].Id_knjige;
                int noviId = idPrethodneKnjige + 1;
                string noviNaziv = txtNaziv.Text;
                string noviAutor = txtAutor.Text;

                double novaCena = 0.0;

                char[] niz = txtCena.Text.ToCharArray();
                bool greska = false;
                foreach (char karakter in niz)
                {
                    if (!char.IsDigit(karakter))
                    { // ako jedan od karaktera nije broj
                        greska = true;
                    }
                }
                if (greska)
                {
                    MessageBox.Show("Cenu unesite brojevima, ne slovima!", "Greška");
                    return;
                }
                else
                {
                    novaCena = double.Parse(txtCena.Text);
                }


                int noviPopust = 0;

                char[] niz2 = txtPopust.Text.ToCharArray();
                bool greska2 = false;
                foreach (char karakter in niz2)
                {
                    if (!char.IsDigit(karakter))
                    { // ako jedan od karaktera nije broj
                        greska2 = true;
                    }
                }
                if (greska2)
                {
                    MessageBox.Show("Popust unesite brojevima, ne slovima!", "Greška");
                    return;
                }
                else
                {
                    noviPopust = int.Parse(txtPopust.Text);
                }

                klijentSistem.UnosKnjige(noviId, noviNaziv, noviAutor, novaCena, noviPopust, 0);
                MessageBox.Show("Uspešno uneta nova knjiga!" + Environment.NewLine +
                    "ID: " + noviId + ", " + noviNaziv + " (" + noviAutor + "), " + novaCena + " din");

                txtNaziv.Clear(); txtAutor.Clear(); txtCena.Clear(); txtPopust.Clear();

                cbKnjigeIzmena.Items.Clear();
                cbKnjigeBrisanje.Items.Clear();

                knjige = klijentJavni.PrikazKnjiga();

                foreach (Knjiga knjiga in knjige)
                {
                    if (!cbKnjigeIzmena.Items.Contains(knjiga))
                    {
                        cbKnjigeIzmena.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                    }
                    if (!cbKnjigeBrisanje.Items.Contains(knjiga))
                    {
                        cbKnjigeBrisanje.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                    }
                }

            }
            else
            {
                MessageBox.Show("Nijedno polje ne sme da ostane prazno.", "Greška");
            }
        }

        private void cbKnjigeIzmena_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selektovano = cbKnjigeIzmena.SelectedItem as string;
            int selektovaniID = int.Parse(selektovano.Split(' ')[0]);

            knjige = klijentJavni.PrikazKnjiga();
            foreach (Knjiga k in knjige)
            {
                if (selektovaniID == k.Id_knjige)
                {
                    txtIdKnjige.Text = k.Id_knjige.ToString();
                    txtNazivIzmena.Text = k.Naziv;
                    txtAutorIzmena.Text = k.Autor;
                    txtCenaIzmena.Text = k.Cena.ToString();
                    txtPopustIzmena.Text = k.Popust.ToString();
                    break;
                }
            }
        }

        private void btnIzmenaKnjige_Click(object sender, EventArgs e)
        {
            if (cbKnjigeIzmena.SelectedIndex < 0)
            {
                MessageBox.Show("Niste odabrali knjigu za izmenu", "Greška");
                return;
            }

            string izmenjenNaziv = txtNazivIzmena.Text;
            string izmenjenAutor = txtAutorIzmena.Text;

            double izmenjenaCena = 0.0;

            char[] niz = txtCenaIzmena.Text.ToCharArray();
            bool greska = false;
            foreach (char karakter in niz)
            {
                if (!char.IsDigit(karakter))
                { // ako jedan od karaktera nije broj
                    greska = true;
                }
            }
            if (greska)
            {
                MessageBox.Show("Cenu unesite brojevima, ne slovima!", "Greška");
                return;
            }
            else
            {
                izmenjenaCena = double.Parse(txtCenaIzmena.Text);
            }


            int izmenjenPopust = 0;

            char[] niz2 = txtPopustIzmena.Text.ToCharArray();
            bool greska2 = false;
            foreach (char karakter in niz2)
            {
                if (!char.IsDigit(karakter))
                { // ako jedan od karaktera nije broj
                    greska2 = true;
                }
            }
            if (greska2)
            {
                MessageBox.Show("Popust unesite brojevima, ne slovima!", "Greška");
                return;
            }
            else
            {
                izmenjenPopust = int.Parse(txtPopustIzmena.Text);
            }

            knjige = klijentJavni.PrikazKnjiga();
            foreach (Knjiga k in knjige)
            {
                if (txtIdKnjige.Text.Equals(k.Id_knjige.ToString()))
                {
                    k.Naziv = izmenjenNaziv;
                    k.Autor = izmenjenAutor;
                    k.Cena = izmenjenaCena;
                    k.Popust = izmenjenPopust;
                    klijentSistem.IzmenaKnjige(k);
                    break;
                }
            }

            MessageBox.Show("Knjiga uspešno izmenjena!");

            txtIdKnjige.Clear(); txtNazivIzmena.Clear(); txtAutorIzmena.Clear();
            txtCenaIzmena.Clear(); txtPopustIzmena.Clear(); cbKnjigeIzmena.ResetText();

            cbKnjigeIzmena.Items.Clear();
            cbKnjigeBrisanje.Items.Clear();

            knjige = klijentJavni.PrikazKnjiga();

            foreach (Knjiga knjiga in knjige)
            {
                if (!cbKnjigeIzmena.Items.Contains(knjiga))
                {
                    cbKnjigeIzmena.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                }
                if (!cbKnjigeBrisanje.Items.Contains(knjiga))
                {
                    cbKnjigeBrisanje.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                }
            }
        }

        private void cbKorisnici_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selektovano = cbKorisnici.SelectedItem as string;

            nalozi = klijentLogin.VratiSveNaloge();
            foreach (Korisnik k in nalozi)
            {
                if (selektovano.Equals(k.Korisnicko_ime))
                {
                    txtIdKorisnika.Text = k.Id_korisnika.ToString();
                    txtKorisnickoIme.Text = k.Korisnicko_ime;
                    txtLozinka.Text = k.Lozinka;
                    break;
                }
            }
        }

        private void btnIzmenaNaloga_Click(object sender, EventArgs e)
        {
            if (cbKorisnici.SelectedIndex < 0)
            {
                MessageBox.Show("Niste odabrali knjigu za izmenu", "Greška");
                return;
            }

            if (!string.IsNullOrEmpty(txtKorisnickoIme.Text) &&
                !string.IsNullOrEmpty(txtLozinka.Text))
            {
                foreach (Korisnik korisnik in nalozi)
                {
                    int adminID = 0;
                    if (korisnik.Korisnicko_ime.Equals("admin"))
                    {
                        adminID = korisnik.Id_korisnika;
                    }

                    if (txtIdKorisnika.Text.Equals(adminID.ToString()))
                    {
                        MessageBox.Show("Podaci o administratoru se ne mogu menjati!", "Oprez");
                        return;
                    }

                    int operaterID = 0;
                    if (korisnik.Korisnicko_ime.Equals("operater"))
                    {
                        operaterID = korisnik.Id_korisnika;
                    }

                    if (txtIdKorisnika.Text.Equals(operaterID.ToString()))
                    {
                        MessageBox.Show("Podaci o operateru se ne mogu menjati!", "Oprez");
                        return;
                    }

                    if (txtIdKorisnika.Text.Equals(korisnik.Id_korisnika.ToString()))
                    {
                        klijentSistem.IzmenaKorisnika(korisnik.Id_korisnika, txtKorisnickoIme.Text, txtLozinka.Text);
                        MessageBox.Show("Uspešno promenjeni podaci o korisniku.");
                        break;
                    }
                }

                txtIdKorisnika.Clear(); txtKorisnickoIme.Clear(); txtLozinka.Clear();
                cbKorisnici.ResetText();

                cbKorisnici.Items.Clear();
                cbNaloziBrisanje.Items.Clear();

                nalozi = klijentLogin.VratiSveNaloge();

                foreach (Korisnik k in nalozi)
                {
                    if (!cbKorisnici.Items.Contains(k.Korisnicko_ime))
                    {
                        cbKorisnici.Items.Add(k.Korisnicko_ime);
                    }
                    if (!cbNaloziBrisanje.Items.Contains(k))
                    {
                        cbNaloziBrisanje.Items.Add(k.Id_korisnika + " " + k.Korisnicko_ime);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nijedno polje ne sme ostati prazno", "Greška");
            }
        }

        private void btnRegistracija_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNovoKorIme.Text) &&
                !string.IsNullOrEmpty(txtNovaLozinka.Text))
            {
                string uspesno = klijentLogin.RegistrujSe(txtNovoKorIme.Text, txtNovaLozinka.Text);
                MessageBox.Show(uspesno);

                txtNovoKorIme.Clear(); txtNovaLozinka.Clear();

                cbKorisnici.ResetText();
                cbNaloziBrisanje.ResetText();

                cbKorisnici.Items.Clear();
                cbNaloziBrisanje.Items.Clear();

                nalozi = klijentLogin.VratiSveNaloge();

                foreach (Korisnik k in nalozi)
                {
                    if (!cbKorisnici.Items.Contains(k.Korisnicko_ime))
                    {
                        cbKorisnici.Items.Add(k.Korisnicko_ime);
                    }
                    if (!cbNaloziBrisanje.Items.Contains(k))
                    {
                        cbNaloziBrisanje.Items.Add(k.Id_korisnika + " " + k.Korisnicko_ime);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nijedno polje ne sme ostati prazno!", "Greška");
            }
        }

        private void btnObrisiKnjigu_Click(object sender, EventArgs e)
        {
            if (cbKnjigeBrisanje.SelectedIndex < 0)
            {
                MessageBox.Show("Niste odabrali knjigu za brisanje!", "Greška");
                return;
            }

            string selektovano = cbKnjigeBrisanje.SelectedItem as string;
            int selektovaniID = int.Parse(selektovano.Split(' ')[0]);           

            knjige = klijentJavni.PrikazKnjiga();
            foreach (Knjiga k in knjige)
            {
                if (selektovaniID == k.Id_knjige)
                {
                    klijentAdmin.ObrisiKnjigu(k);
                    MessageBox.Show("Knjiga " + k.Naziv + " je obrisana.", "Obrisano");
                    break;
                }
            }

            txtIdKnjige.Clear(); txtNazivIzmena.Clear(); txtAutorIzmena.Clear();
            txtCenaIzmena.Clear(); txtPopustIzmena.Clear();

            cbKnjigeIzmena.ResetText();
            cbKnjigeBrisanje.ResetText();

            cbKnjigeIzmena.Items.Clear();
            cbKnjigeBrisanje.Items.Clear();

            knjige = klijentJavni.PrikazKnjiga();

            foreach (Knjiga knjiga in knjige)
            {
                if (!cbKnjigeIzmena.Items.Contains(knjiga))
                {
                    cbKnjigeIzmena.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                }
                if (!cbKnjigeBrisanje.Items.Contains(knjiga))
                {
                    cbKnjigeBrisanje.Items.Add(knjiga.Id_knjige + " " + knjiga.Naziv);
                }
            }

        }

        private void btnObrisiNalog_Click(object sender, EventArgs e)
        {
            if (cbNaloziBrisanje.SelectedIndex < 0)
            {
                MessageBox.Show("Niste odabrali nalog za brisanje", "Greška");
                return;
            }

            string selektovano = cbNaloziBrisanje.SelectedItem as string;
            int selektovaniID = int.Parse(selektovano.Split(' ')[0]);

            nalozi = klijentLogin.VratiSveNaloge();
            if (selektovaniID == 100 || selektovaniID == 200)
            {
                MessageBox.Show("Nalog administratora i operatera nije moguće obrisati.");
                return;
            }

            foreach (Korisnik k in nalozi)
            {              
                if (selektovaniID == k.Id_korisnika)
                {
                    klijentAdmin.ObrisiNalog(k);
                    MessageBox.Show("Korisnik " + k.Korisnicko_ime + " je uklonjen.", "Obrisano");
                    break;
                }
            }

            txtIdKorisnika.Clear(); txtKorisnickoIme.Clear(); txtLozinka.Clear();
            cbKorisnici.ResetText();
            cbNaloziBrisanje.ResetText();

            cbKorisnici.Items.Clear();
            cbNaloziBrisanje.Items.Clear();

            nalozi = klijentLogin.VratiSveNaloge();

            foreach (Korisnik k in nalozi)
            {
                if (!cbKorisnici.Items.Contains(k.Korisnicko_ime))
                {
                    cbKorisnici.Items.Add(k.Korisnicko_ime);
                }
                if (!cbNaloziBrisanje.Items.Contains(k))
                {
                    cbNaloziBrisanje.Items.Add(k.Id_korisnika + " " + k.Korisnicko_ime);
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
