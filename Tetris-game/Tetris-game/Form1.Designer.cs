namespace Tetris_game
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.pb_polje = new System.Windows.Forms.PictureBox();
            this.btn_nova = new System.Windows.Forms.Button();
            this.timer_game = new System.Windows.Forms.Timer(this.components);
            this.btn_top = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_top = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_bodovi = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pb_sljedeci = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_razina = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_linija = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_polje)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_sljedeci)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_polje
            // 
            this.pb_polje.BackColor = System.Drawing.SystemColors.Window;
            this.pb_polje.Location = new System.Drawing.Point(12, 12);
            this.pb_polje.Name = "pb_polje";
            this.pb_polje.Size = new System.Drawing.Size(201, 401);
            this.pb_polje.TabIndex = 0;
            this.pb_polje.TabStop = false;
            // 
            // btn_nova
            // 
            this.btn_nova.Location = new System.Drawing.Point(245, 12);
            this.btn_nova.Name = "btn_nova";
            this.btn_nova.Size = new System.Drawing.Size(75, 23);
            this.btn_nova.TabIndex = 1;
            this.btn_nova.Text = "Nova Igra";
            this.btn_nova.UseVisualStyleBackColor = true;
            this.btn_nova.Click += new System.EventHandler(this.btn_nova_Click);
            // 
            // timer_game
            // 
            this.timer_game.Interval = 1000;
            this.timer_game.Tick += new System.EventHandler(this.timer_game_Tick);
            // 
            // btn_top
            // 
            this.btn_top.Location = new System.Drawing.Point(245, 41);
            this.btn_top.Name = "btn_top";
            this.btn_top.Size = new System.Drawing.Size(75, 23);
            this.btn_top.TabIndex = 2;
            this.btn_top.Text = "Top lista";
            this.btn_top.UseVisualStyleBackColor = true;
            this.btn_top.Click += new System.EventHandler(this.btn_top_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(219, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Top:";
            // 
            // label_top
            // 
            this.label_top.AutoSize = true;
            this.label_top.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_top.Location = new System.Drawing.Point(265, 87);
            this.label_top.Name = "label_top";
            this.label_top.Size = new System.Drawing.Size(34, 20);
            this.label_top.TabIndex = 4;
            this.label_top.Text = "ime";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(241, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bodovi:";
            // 
            // label_bodovi
            // 
            this.label_bodovi.AutoSize = true;
            this.label_bodovi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_bodovi.Location = new System.Drawing.Point(241, 167);
            this.label_bodovi.Name = "label_bodovi";
            this.label_bodovi.Size = new System.Drawing.Size(18, 20);
            this.label_bodovi.TabIndex = 6;
            this.label_bodovi.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(216, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sljedeci element:";
            // 
            // pb_sljedeci
            // 
            this.pb_sljedeci.BackColor = System.Drawing.SystemColors.Window;
            this.pb_sljedeci.Location = new System.Drawing.Point(242, 220);
            this.pb_sljedeci.Name = "pb_sljedeci";
            this.pb_sljedeci.Size = new System.Drawing.Size(81, 81);
            this.pb_sljedeci.TabIndex = 8;
            this.pb_sljedeci.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(216, 341);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Razina:";
            // 
            // label_razina
            // 
            this.label_razina.AutoSize = true;
            this.label_razina.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_razina.Location = new System.Drawing.Point(285, 341);
            this.label_razina.Name = "label_razina";
            this.label_razina.Size = new System.Drawing.Size(18, 20);
            this.label_razina.TabIndex = 10;
            this.label_razina.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(216, 371);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Linija:";
            // 
            // label_linija
            // 
            this.label_linija.AutoSize = true;
            this.label_linija.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_linija.Location = new System.Drawing.Point(271, 371);
            this.label_linija.Name = "label_linija";
            this.label_linija.Size = new System.Drawing.Size(18, 20);
            this.label_linija.TabIndex = 12;
            this.label_linija.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 424);
            this.Controls.Add(this.label_linija);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label_razina);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pb_sljedeci);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_bodovi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_top);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_top);
            this.Controls.Add(this.btn_nova);
            this.Controls.Add(this.pb_polje);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pb_polje)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_sljedeci)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_polje;
        private System.Windows.Forms.Button btn_nova;
        public System.Windows.Forms.Timer timer_game;
        private System.Windows.Forms.Button btn_top;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label_top;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label_bodovi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pb_sljedeci;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label_razina;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label_linija;
    }
}

