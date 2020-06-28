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
    public partial class frmLogin : Form
    {
        LogInClient clientLogin;
        public frmLogin()
        {
            InitializeComponent();
            clientLogin = new LogInClient();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnUlogujSe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Nijedno polje ne sme biti prazno", "Greška");
                return;
            }

            if (clientLogin.LogIn(txtUsername.Text, txtPassword.Text))
            {
                frmKlijent forma = new frmKlijent(txtUsername.Text);
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

        private void btnRegistrujSe_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsernameReg.Text) &&
                !string.IsNullOrEmpty(txtPasswordReg.Text))
            {
                string uspesno = clientLogin.RegistrujSe(txtUsernameReg.Text, txtPasswordReg.Text);
                MessageBox.Show(uspesno);

                if(!uspesno.Equals("Korisničko ime već postoji!"))
                {
                    frmKlijent forma = new frmKlijent(txtUsernameReg.Text);
                    Hide();
                    forma.ShowDialog();
                    Close();
                }
                else
                {
                    txtUsernameReg.Clear();
                    txtPasswordReg.Clear();
                }

            }
            else
            {
                MessageBox.Show("Nijedno polje ne sme ostati prazno!", "Greška");
            }
        }
    }
}
