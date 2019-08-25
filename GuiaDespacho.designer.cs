namespace DTE33
{
    partial class GuiaDespacho
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
            this.cboGuias = new System.Windows.Forms.ComboBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.btnGeneraXml = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbS = new System.Windows.Forms.RadioButton();
            this.rbL = new System.Windows.Forms.RadioButton();
            this.btnSalir = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboGuias
            // 
            this.cboGuias.FormattingEnabled = true;
            this.cboGuias.Location = new System.Drawing.Point(47, 121);
            this.cboGuias.Name = "cboGuias";
            this.cboGuias.Size = new System.Drawing.Size(121, 21);
            this.cboGuias.TabIndex = 50;
            // 
            // dtpFecha
            // 
            this.dtpFecha.CustomFormat = "dd-mm-yy";
            this.dtpFecha.Location = new System.Drawing.Point(59, 22);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(258, 20);
            this.dtpFecha.TabIndex = 42;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            // 
            // btnGeneraXml
            // 
            this.btnGeneraXml.Location = new System.Drawing.Point(354, 17);
            this.btnGeneraXml.Name = "btnGeneraXml";
            this.btnGeneraXml.Size = new System.Drawing.Size(122, 25);
            this.btnGeneraXml.TabIndex = 40;
            this.btnGeneraXml.Text = "Genera XML  Guia";
            this.btnGeneraXml.UseVisualStyleBackColor = true;
            this.btnGeneraXml.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(354, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 23);
            this.button1.TabIndex = 51;
            this.button1.Text = "Exporta MySql";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.rbS);
            this.groupBox1.Controls.Add(this.rbL);
            this.groupBox1.Location = new System.Drawing.Point(47, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 52);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ubicación Datos";
            // 
            // rbS
            // 
            this.rbS.AutoSize = true;
            this.rbS.Location = new System.Drawing.Point(25, 19);
            this.rbS.Name = "rbS";
            this.rbS.Size = new System.Drawing.Size(71, 17);
            this.rbS.TabIndex = 1;
            this.rbS.Text = "c:\\Leyton";
            this.rbS.UseVisualStyleBackColor = true;
            this.rbS.CheckedChanged += new System.EventHandler(this.rbS_CheckedChanged);
            // 
            // rbL
            // 
            this.rbL.AutoSize = true;
            this.rbL.Checked = true;
            this.rbL.Location = new System.Drawing.Point(167, 19);
            this.rbL.Name = "rbL";
            this.rbL.Size = new System.Drawing.Size(84, 17);
            this.rbL.TabIndex = 0;
            this.rbL.TabStop = true;
            this.rbL.Text = "O:\\LEYTON";
            this.rbL.UseVisualStyleBackColor = true;
            this.rbL.CheckedChanged += new System.EventHandler(this.rbL_CheckedChanged);
            // 
            // btnSalir
            // 
            //this.btnSalir.Image = global::DTE33.Properties.Resources._16__Exit_;
            this.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.Location = new System.Drawing.Point(524, 12);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 49;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(369, 86);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 53;
            this.button2.Text = "certificado";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(43, 146);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(602, 178);
            this.textBox1.TabIndex = 54;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(524, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 55;
            this.button3.Text = "pdf417";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // GuiaDespacho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(685, 327);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cboGuias);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.btnGeneraXml);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GuiaDespacho";
            this.Text = "Guia Despacho Bodega";
            this.Load += new System.EventHandler(this.GuiaDespacho_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboGuias;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnGeneraXml;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbS;
        private System.Windows.Forms.RadioButton rbL;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
    }
}