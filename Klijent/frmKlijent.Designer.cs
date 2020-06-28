namespace Klijent
{
    partial class frmKlijent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblUlogovaniKorisnik = new System.Windows.Forms.Label();
            this.lbPorudzbine = new System.Windows.Forms.ListBox();
            this.btnOdjava = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Odaberite knjige koje želite da naručite: ";
            // 
            // lblUlogovaniKorisnik
            // 
            this.lblUlogovaniKorisnik.AutoSize = true;
            this.lblUlogovaniKorisnik.Location = new System.Drawing.Point(17, 680);
            this.lblUlogovaniKorisnik.Name = "lblUlogovaniKorisnik";
            this.lblUlogovaniKorisnik.Size = new System.Drawing.Size(53, 20);
            this.lblUlogovaniKorisnik.TabIndex = 2;
            this.lblUlogovaniKorisnik.Text = "label2";
            // 
            // lbPorudzbine
            // 
            this.lbPorudzbine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbPorudzbine.FormattingEnabled = true;
            this.lbPorudzbine.ItemHeight = 25;
            this.lbPorudzbine.Location = new System.Drawing.Point(21, 549);
            this.lbPorudzbine.Name = "lbPorudzbine";
            this.lbPorudzbine.ScrollAlwaysVisible = true;
            this.lbPorudzbine.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbPorudzbine.Size = new System.Drawing.Size(834, 104);
            this.lbPorudzbine.TabIndex = 3;
            // 
            // btnOdjava
            // 
            this.btnOdjava.Location = new System.Drawing.Point(746, 673);
            this.btnOdjava.Name = "btnOdjava";
            this.btnOdjava.Size = new System.Drawing.Size(109, 35);
            this.btnOdjava.TabIndex = 4;
            this.btnOdjava.Text = "Odjava";
            this.btnOdjava.UseVisualStyleBackColor = true;
            this.btnOdjava.Click += new System.EventHandler(this.btnOdjava_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(17, 522);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "MOJE PORUDŽBINE:";
            // 
            // frmKlijent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(877, 715);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOdjava);
            this.Controls.Add(this.lbPorudzbine);
            this.Controls.Add(this.lblUlogovaniKorisnik);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ForeColor = System.Drawing.Color.DarkGreen;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmKlijent";
            this.Text = "NARUČIVANJE KNJIGA";
            this.Load += new System.EventHandler(this.frmKlijent_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUlogovaniKorisnik;
        private System.Windows.Forms.ListBox lbPorudzbine;
        private System.Windows.Forms.Button btnOdjava;
        private System.Windows.Forms.Label label2;
    }
}