namespace The_Heart
{
    partial class MenuPlay
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
            this.btnBackPlay = new System.Windows.Forms.Button();
            this.btnAtacar = new System.Windows.Forms.Button();
            this.btnBloquear = new System.Windows.Forms.Button();
            this.btnEsquivar = new System.Windows.Forms.Button();
            this.btnPociones = new System.Windows.Forms.Button();
            this.HpPlayer = new System.Windows.Forms.ProgressBar();
            this.VidaMonster = new System.Windows.Forms.ProgressBar();
            this.picMonster = new System.Windows.Forms.PictureBox();
            this.HpMounster = new System.Windows.Forms.ProgressBar();
            this.TxtNombreMounstro = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picMonster)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBackPlay
            // 
            this.btnBackPlay.BackColor = System.Drawing.Color.Brown;
            this.btnBackPlay.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackPlay.Location = new System.Drawing.Point(697, 465);
            this.btnBackPlay.Name = "btnBackPlay";
            this.btnBackPlay.Size = new System.Drawing.Size(150, 74);
            this.btnBackPlay.TabIndex = 1;
            this.btnBackPlay.Text = "Suicidio\r\n";
            this.btnBackPlay.UseVisualStyleBackColor = false;
            this.btnBackPlay.Click += new System.EventHandler(this.btnBackPlay_Click);
            // 
            // btnAtacar
            // 
            this.btnAtacar.Location = new System.Drawing.Point(607, 170);
            this.btnAtacar.Name = "btnAtacar";
            this.btnAtacar.Size = new System.Drawing.Size(130, 55);
            this.btnAtacar.TabIndex = 2;
            this.btnAtacar.Text = "Atacar";
            this.btnAtacar.UseVisualStyleBackColor = true;
            this.btnAtacar.Click += new System.EventHandler(this.btnAtacar_Click);
            // 
            // btnBloquear
            // 
            this.btnBloquear.Location = new System.Drawing.Point(799, 170);
            this.btnBloquear.Name = "btnBloquear";
            this.btnBloquear.Size = new System.Drawing.Size(130, 55);
            this.btnBloquear.TabIndex = 3;
            this.btnBloquear.Text = "Bloquear";
            this.btnBloquear.UseVisualStyleBackColor = true;
            this.btnBloquear.Click += new System.EventHandler(this.btnBloquear_Click);
            // 
            // btnEsquivar
            // 
            this.btnEsquivar.Location = new System.Drawing.Point(607, 296);
            this.btnEsquivar.Name = "btnEsquivar";
            this.btnEsquivar.Size = new System.Drawing.Size(130, 55);
            this.btnEsquivar.TabIndex = 4;
            this.btnEsquivar.Text = "Esquivar";
            this.btnEsquivar.UseVisualStyleBackColor = true;
            this.btnEsquivar.Click += new System.EventHandler(this.btnEsquivar_Click);
            // 
            // btnPociones
            // 
            this.btnPociones.Location = new System.Drawing.Point(799, 296);
            this.btnPociones.Name = "btnPociones";
            this.btnPociones.Size = new System.Drawing.Size(130, 55);
            this.btnPociones.TabIndex = 5;
            this.btnPociones.Text = "Curar";
            this.btnPociones.UseVisualStyleBackColor = true;
            this.btnPociones.Click += new System.EventHandler(this.btnPociones_Click);
            // 
            // HpPlayer
            // 
            this.HpPlayer.Location = new System.Drawing.Point(607, 51);
            this.HpPlayer.Name = "HpPlayer";
            this.HpPlayer.Size = new System.Drawing.Size(322, 43);
            this.HpPlayer.TabIndex = 6;
            this.HpPlayer.Click += new System.EventHandler(this.HpPlayer_Click);
            // 
            // VidaMonster
            // 
            this.VidaMonster.Location = new System.Drawing.Point(109, 401);
            this.VidaMonster.Name = "VidaMonster";
            this.VidaMonster.Size = new System.Drawing.Size(288, 43);
            this.VidaMonster.TabIndex = 7;
            this.VidaMonster.Click += new System.EventHandler(this.progressBar2_Click);
            // 
            // picMonster
            // 
            this.picMonster.Location = new System.Drawing.Point(69, 149);
            this.picMonster.Name = "picMonster";
            this.picMonster.Size = new System.Drawing.Size(406, 439);
            this.picMonster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMonster.TabIndex = 8;
            this.picMonster.TabStop = false;
            this.picMonster.Click += new System.EventHandler(this.picMonster_Click);
            // 
            // HpMounster
            // 
            this.HpMounster.Location = new System.Drawing.Point(60, 100);
            this.HpMounster.Name = "HpMounster";
            this.HpMounster.Size = new System.Drawing.Size(415, 43);
            this.HpMounster.TabIndex = 9;
            this.HpMounster.Click += new System.EventHandler(this.HpMounster_Click);
            // 
            // TxtNombreMounstro
            // 
            this.TxtNombreMounstro.AutoSize = true;
            this.TxtNombreMounstro.Location = new System.Drawing.Point(60, 33);
            this.TxtNombreMounstro.Name = "TxtNombreMounstro";
            this.TxtNombreMounstro.Size = new System.Drawing.Size(76, 20);
            this.TxtNombreMounstro.TabIndex = 10;
            this.TxtNombreMounstro.Text = "Mounstro";
            // 
            // MenuPlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1239, 815);
            this.Controls.Add(this.TxtNombreMounstro);
            this.Controls.Add(this.HpMounster);
            this.Controls.Add(this.picMonster);
            this.Controls.Add(this.VidaMonster);
            this.Controls.Add(this.HpPlayer);
            this.Controls.Add(this.btnPociones);
            this.Controls.Add(this.btnEsquivar);
            this.Controls.Add(this.btnBloquear);
            this.Controls.Add(this.btnAtacar);
            this.Controls.Add(this.btnBackPlay);
            this.Name = "MenuPlay";
            this.Text = "MenuPlay";
            ((System.ComponentModel.ISupportInitialize)(this.picMonster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBackPlay;
        private System.Windows.Forms.Button btnAtacar;
        private System.Windows.Forms.Button btnBloquear;
        private System.Windows.Forms.Button btnEsquivar;
        private System.Windows.Forms.Button btnPociones;
        private System.Windows.Forms.ProgressBar HpPlayer;
        private System.Windows.Forms.ProgressBar VidaMonster;
        private System.Windows.Forms.PictureBox picMonster;
        private System.Windows.Forms.ProgressBar HpMounster;
        private System.Windows.Forms.Label TxtNombreMounstro;
    }
}