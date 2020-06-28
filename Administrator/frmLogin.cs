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
    public partial class frmLogin : Form
    {
        LogInClient clientLogin;
        public frmLogin()
        {
            InitializeComponent();
            clientLogin = new LogInClient();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnUloguj_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Nijedno polje ne sme biti prazno", "Greška");
                return;
            }

            if (txtUsername.Text.Equals("admin"))
            {
                if (clientLogin.LogIn(txtUsername.Text, txtPassword.Text))
                {
                    frmAdministrator forma = new frmAdministrator();
                    Hide();
                    forma.ShowDialog();
                    Close();
                }
                else
                {
                    ListaNaloga korisnici = clientLogin.VratiSveNaloge();
                    bool korisnickoImePostoji = false;
                    foreach (Korisnik korisnik in korisnici)
                    {
                        if (txtUsername.Text.Equals(korisnik.Korisnicko_ime))
                        {
                            korisnickoImePostoji = true;
                        }
                    }
                    if (korisnickoImePostoji)
                    {
                        MessageBox.Show("Pogrešna lozinka!", "Neuspešno prijavljivanje");
                    }
                    else
                    {
                        MessageBox.Show("Nalog pod ovim korisničkim imenom ne postoji!", "Neuspešno prijavljivanje");
                    }
                }
            }
            else
            {
                MessageBox.Show("Nemate pravo pristupa ukoliko niste administrator", "Greška");
            }
        }
    }
}
