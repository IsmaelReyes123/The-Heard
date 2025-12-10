namespace The_Heart
{
    partial class MenuSeleccion
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewHeroes = new System.Windows.Forms.DataGridView();
            this.picHero = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectHero = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHeroes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHero)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewHeroes
            // 
            this.dataGridViewHeroes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHeroes.Location = new System.Drawing.Point(99, 346);
            this.dataGridViewHeroes.Name = "dataGridViewHeroes";
            this.dataGridViewHeroes.RowHeadersWidth = 62;
            this.dataGridViewHeroes.RowTemplate.Height = 28;
            this.dataGridViewHeroes.Size = new System.Drawing.Size(363, 180);
            this.dataGridViewHeroes.TabIndex = 0;
            this.dataGridViewHeroes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewHeroes_CellClick);
            // 
            // picHero
            // 
            this.picHero.Location = new System.Drawing.Point(627, 24);
            this.picHero.Name = "picHero";
            this.picHero.Size = new System.Drawing.Size(416, 590);
            this.picHero.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHero.TabIndex = 1;
            this.picHero.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Algerian", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 73);
            this.label1.TabIndex = 0;
            this.label1.Text = "...name...\r\n       ...your... \r\n    ...Hero...";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Font = new System.Drawing.Font("Algerian", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlayerName.Location = new System.Drawing.Point(162, 178);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(213, 34);
            this.txtPlayerName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Algerian", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(211, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 66);
            this.label2.TabIndex = 3;
            this.label2.Text = "..choose...\r\n   ...your...\r\n ...Vessel...";
            // 
            // btnSelectHero
            // 
            this.btnSelectHero.Font = new System.Drawing.Font("Harrington", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectHero.Location = new System.Drawing.Point(215, 568);
            this.btnSelectHero.Name = "btnSelectHero";
            this.btnSelectHero.Size = new System.Drawing.Size(110, 46);
            this.btnSelectHero.TabIndex = 4;
            this.btnSelectHero.Text = "Begin";
            this.btnSelectHero.UseVisualStyleBackColor = true;
            this.btnSelectHero.Click += new System.EventHandler(this.btnSelectHero_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 644);
            this.Controls.Add(this.btnSelectHero);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picHero);
            this.Controls.Add(this.dataGridViewHeroes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHeroes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHero)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewHeroes;
        private System.Windows.Forms.PictureBox picHero;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectHero;
    }
}

