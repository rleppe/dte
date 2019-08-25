namespace DTE33
{
    partial class Facturas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.rbS = new System.Windows.Forms.RadioButton();
            this.rbL = new System.Windows.Forms.RadioButton();
            this.dgvFacturas = new System.Windows.Forms.DataGridView();
            this.oc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.na = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numdoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codcli = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.neto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TRACK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnTodos = new System.Windows.Forms.Button();
            this.btnElimina = new System.Windows.Forms.Button();
            this.btnNiuno = new System.Windows.Forms.Button();
            this.dgvSii = new System.Windows.Forms.DataGridView();
            this.ocd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numdocD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSii = new System.Windows.Forms.Button();
            this.btnSAlir = new System.Windows.Forms.Button();
            this.chkbUpload = new System.Windows.Forms.CheckBox();
            this.chkSoloXmlCliente = new System.Windows.Forms.CheckBox();
            this.chkEnviaXmlCliente = new System.Windows.Forms.CheckBox();
            this.chkPrefactura = new System.Windows.Forms.CheckBox();
            this.btnPDFS = new System.Windows.Forms.Button();
            this.chkEnviaPDFCliente = new System.Windows.Forms.CheckBox();
            this.queryEstDteAvService1 = new DTE33.cl.sii.palena.QueryEstDteAvService();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSii)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpFecha
            // 
            this.dtpFecha.CustomFormat = "dd-mm-yy";
            this.dtpFecha.Location = new System.Drawing.Point(23, 6);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(258, 20);
            this.dtpFecha.TabIndex = 43;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            // 
            // rbS
            // 
            this.rbS.AutoSize = true;
            this.rbS.Location = new System.Drawing.Point(389, 12);
            this.rbS.Name = "rbS";
            this.rbS.Size = new System.Drawing.Size(35, 17);
            this.rbS.TabIndex = 1;
            this.rbS.Text = "S:";
            this.rbS.UseVisualStyleBackColor = true;
            // 
            // rbL
            // 
            this.rbL.AutoSize = true;
            this.rbL.Checked = true;
            this.rbL.Location = new System.Drawing.Point(298, 10);
            this.rbL.Name = "rbL";
            this.rbL.Size = new System.Drawing.Size(85, 17);
            this.rbL.TabIndex = 0;
            this.rbL.TabStop = true;
            this.rbL.Text = "L:\\almadena";
            this.rbL.UseVisualStyleBackColor = true;
            this.rbL.CheckedChanged += new System.EventHandler(this.rbL_CheckedChanged);
            // 
            // dgvFacturas
            // 
            this.dgvFacturas.AllowUserToAddRows = false;
            this.dgvFacturas.AllowUserToDeleteRows = false;
            this.dgvFacturas.AllowUserToOrderColumns = true;
            this.dgvFacturas.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFacturas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.oc,
            this.na,
            this.numdoc,
            this.fecha,
            this.TOTAL,
            this.codcli,
            this.Column1,
            this.nombre,
            this.neto,
            this.TRACK});
            this.dgvFacturas.Location = new System.Drawing.Point(3, 49);
            this.dgvFacturas.Name = "dgvFacturas";
            this.dgvFacturas.ReadOnly = true;
            this.dgvFacturas.Size = new System.Drawing.Size(890, 565);
            this.dgvFacturas.TabIndex = 46;
            this.dgvFacturas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFacturas_CellClick);
            this.dgvFacturas.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFacturas_CellMouseDoubleClick);
            this.dgvFacturas.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvFacturas_RowPostPaint);
            this.dgvFacturas.Enter += new System.EventHandler(this.dgvFacturas_Enter);
            this.dgvFacturas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvFacturas_KeyPress);
            // 
            // oc
            // 
            this.oc.FillWeight = 50F;
            this.oc.HeaderText = "O.C";
            this.oc.Name = "oc";
            this.oc.ReadOnly = true;
            this.oc.Width = 50;
            // 
            // na
            // 
            this.na.HeaderText = "na";
            this.na.Name = "na";
            this.na.ReadOnly = true;
            // 
            // numdoc
            // 
            this.numdoc.FillWeight = 50F;
            this.numdoc.HeaderText = "factura";
            this.numdoc.MaxInputLength = 5;
            this.numdoc.Name = "numdoc";
            this.numdoc.ReadOnly = true;
            this.numdoc.Width = 50;
            // 
            // fecha
            // 
            this.fecha.FillWeight = 70F;
            this.fecha.HeaderText = "Fecha";
            this.fecha.MaxInputLength = 10;
            this.fecha.Name = "fecha";
            this.fecha.ReadOnly = true;
            this.fecha.Width = 70;
            // 
            // TOTAL
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            this.TOTAL.DefaultCellStyle = dataGridViewCellStyle1;
            this.TOTAL.FillWeight = 70F;
            this.TOTAL.HeaderText = "TOTAL";
            this.TOTAL.MaxInputLength = 9;
            this.TOTAL.Name = "TOTAL";
            this.TOTAL.ReadOnly = true;
            this.TOTAL.Width = 70;
            // 
            // codcli
            // 
            this.codcli.HeaderText = "codcli";
            this.codcli.Name = "codcli";
            this.codcli.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "rut";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // nombre
            // 
            this.nombre.HeaderText = "nombre";
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            // 
            // neto
            // 
            this.neto.HeaderText = "neto";
            this.neto.Name = "neto";
            this.neto.ReadOnly = true;
            // 
            // TRACK
            // 
            this.TRACK.HeaderText = "TrackID";
            this.TRACK.Name = "TRACK";
            this.TRACK.ReadOnly = true;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Image = global::DTE33.Properties.Resources._1rightarrow;
            this.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAgregar.Location = new System.Drawing.Point(936, 80);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.TabIndex = 49;
            this.btnAgregar.Text = "Agrega";
            this.btnAgregar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnTodos
            // 
            this.btnTodos.Image = global::DTE33.Properties.Resources._2rightarrow;
            this.btnTodos.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTodos.Location = new System.Drawing.Point(936, 49);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(75, 23);
            this.btnTodos.TabIndex = 50;
            this.btnTodos.Text = "Todos";
            this.btnTodos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTodos.UseVisualStyleBackColor = true;
            this.btnTodos.Visible = false;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // btnElimina
            // 
            this.btnElimina.Image = global::DTE33.Properties.Resources._1leftarrow;
            this.btnElimina.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnElimina.Location = new System.Drawing.Point(952, 184);
            this.btnElimina.Name = "btnElimina";
            this.btnElimina.Size = new System.Drawing.Size(75, 23);
            this.btnElimina.TabIndex = 51;
            this.btnElimina.Text = "Elimina";
            this.btnElimina.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnElimina.UseVisualStyleBackColor = true;
            this.btnElimina.Click += new System.EventHandler(this.btnElimina_Click);
            // 
            // btnNiuno
            // 
            this.btnNiuno.Image = global::DTE33.Properties.Resources._2leftarrow;
            this.btnNiuno.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNiuno.Location = new System.Drawing.Point(952, 213);
            this.btnNiuno.Name = "btnNiuno";
            this.btnNiuno.Size = new System.Drawing.Size(75, 23);
            this.btnNiuno.TabIndex = 52;
            this.btnNiuno.Text = "Todos";
            this.btnNiuno.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNiuno.UseVisualStyleBackColor = true;
            this.btnNiuno.Visible = false;
            this.btnNiuno.Click += new System.EventHandler(this.btnNiuno_Click);
            // 
            // dgvSii
            // 
            this.dgvSii.AllowUserToAddRows = false;
            this.dgvSii.AllowUserToDeleteRows = false;
            this.dgvSii.AllowUserToOrderColumns = true;
            this.dgvSii.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSii.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSii.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ocd,
            this.numdocD});
            this.dgvSii.Location = new System.Drawing.Point(1039, 40);
            this.dgvSii.Name = "dgvSii";
            this.dgvSii.Size = new System.Drawing.Size(243, 563);
            this.dgvSii.TabIndex = 55;
            this.dgvSii.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSii_CellClick);
            // 
            // ocd
            // 
            this.ocd.HeaderText = "O.C.";
            this.ocd.Name = "ocd";
            this.ocd.ReadOnly = true;
            // 
            // numdocD
            // 
            this.numdocD.FillWeight = 50F;
            this.numdocD.HeaderText = "factura";
            this.numdocD.Name = "numdocD";
            this.numdocD.ReadOnly = true;
            // 
            // btnSii
            // 
            this.btnSii.Location = new System.Drawing.Point(916, 109);
            this.btnSii.Name = "btnSii";
            this.btnSii.Size = new System.Drawing.Size(117, 55);
            this.btnSii.TabIndex = 56;
            this.btnSii.Text = "Procesa";
            this.btnSii.UseVisualStyleBackColor = true;
            this.btnSii.Click += new System.EventHandler(this.btnSii_Click);
            // 
            // btnSAlir
            // 
            this.btnSAlir.Image = global::DTE33.Properties.Resources._16__Exit_;
            this.btnSAlir.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSAlir.Location = new System.Drawing.Point(952, 242);
            this.btnSAlir.Name = "btnSAlir";
            this.btnSAlir.Size = new System.Drawing.Size(75, 23);
            this.btnSAlir.TabIndex = 57;
            this.btnSAlir.Text = "Salir";
            this.btnSAlir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSAlir.UseVisualStyleBackColor = true;
            this.btnSAlir.Click += new System.EventHandler(this.btnSAlir_Click);
            // 
            // chkbUpload
            // 
            this.chkbUpload.AutoSize = true;
            this.chkbUpload.Checked = true;
            this.chkbUpload.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbUpload.Location = new System.Drawing.Point(952, 620);
            this.chkbUpload.Name = "chkbUpload";
            this.chkbUpload.Size = new System.Drawing.Size(117, 17);
            this.chkbUpload.TabIndex = 58;
            this.chkbUpload.Text = "Envia  Sii UPLOAD";
            this.chkbUpload.UseVisualStyleBackColor = true;
            this.chkbUpload.CheckedChanged += new System.EventHandler(this.chkbUpload_CheckedChanged);
            // 
            // chkSoloXmlCliente
            // 
            this.chkSoloXmlCliente.AutoSize = true;
            this.chkSoloXmlCliente.Location = new System.Drawing.Point(18, 620);
            this.chkSoloXmlCliente.Name = "chkSoloXmlCliente";
            this.chkSoloXmlCliente.Size = new System.Drawing.Size(100, 17);
            this.chkSoloXmlCliente.TabIndex = 59;
            this.chkSoloXmlCliente.Text = "Solo xml Cliente";
            this.chkSoloXmlCliente.UseVisualStyleBackColor = true;
            this.chkSoloXmlCliente.CheckedChanged += new System.EventHandler(this.chkSoloXmlCliente_CheckedChanged);
            // 
            // chkEnviaXmlCliente
            // 
            this.chkEnviaXmlCliente.AutoSize = true;
            this.chkEnviaXmlCliente.Checked = true;
            this.chkEnviaXmlCliente.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnviaXmlCliente.Location = new System.Drawing.Point(827, 620);
            this.chkEnviaXmlCliente.Name = "chkEnviaXmlCliente";
            this.chkEnviaXmlCliente.Size = new System.Drawing.Size(119, 17);
            this.chkEnviaXmlCliente.TabIndex = 60;
            this.chkEnviaXmlCliente.Text = "Envia Xml al Cliente";
            this.chkEnviaXmlCliente.UseVisualStyleBackColor = true;
            // 
            // chkPrefactura
            // 
            this.chkPrefactura.AutoSize = true;
            this.chkPrefactura.Location = new System.Drawing.Point(124, 620);
            this.chkPrefactura.Name = "chkPrefactura";
            this.chkPrefactura.Size = new System.Drawing.Size(78, 17);
            this.chkPrefactura.TabIndex = 61;
            this.chkPrefactura.Text = "Prefactura ";
            this.chkPrefactura.UseVisualStyleBackColor = true;
            this.chkPrefactura.CheckedChanged += new System.EventHandler(this.chkPrefactura_CheckedChanged);
            // 
            // btnPDFS
            // 
            this.btnPDFS.Location = new System.Drawing.Point(456, 624);
            this.btnPDFS.Name = "btnPDFS";
            this.btnPDFS.Size = new System.Drawing.Size(146, 23);
            this.btnPDFS.TabIndex = 62;
            this.btnPDFS.Text = "PDFs Cedible";
            this.btnPDFS.UseVisualStyleBackColor = true;
            this.btnPDFS.Click += new System.EventHandler(this.btnPDFS_Click);
            // 
            // chkEnviaPDFCliente
            // 
            this.chkEnviaPDFCliente.AutoSize = true;
            this.chkEnviaPDFCliente.Location = new System.Drawing.Point(710, 620);
            this.chkEnviaPDFCliente.Name = "chkEnviaPDFCliente";
            this.chkEnviaPDFCliente.Size = new System.Drawing.Size(77, 17);
            this.chkEnviaPDFCliente.TabIndex = 63;
            this.chkEnviaPDFCliente.Text = "Envia PDF";
            this.chkEnviaPDFCliente.UseVisualStyleBackColor = true;
            this.chkEnviaPDFCliente.CheckedChanged += new System.EventHandler(this.chkEnviaPDFCliente_CheckedChanged);
            // 
            // queryEstDteAvService1
            // 
            this.queryEstDteAvService1.Credentials = null;
            this.queryEstDteAvService1.Url = "https://palena.sii.cl/DTEWS/services/QueryEstDteAv";
            this.queryEstDteAvService1.UseDefaultCredentials = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(284, 624);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "label1";
            // 
            // Facturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1324, 750);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkEnviaPDFCliente);
            this.Controls.Add(this.btnPDFS);
            this.Controls.Add(this.chkPrefactura);
            this.Controls.Add(this.chkEnviaXmlCliente);
            this.Controls.Add(this.chkSoloXmlCliente);
            this.Controls.Add(this.chkbUpload);
            this.Controls.Add(this.btnSAlir);
            this.Controls.Add(this.btnSii);
            this.Controls.Add(this.dgvSii);
            this.Controls.Add(this.rbL);
            this.Controls.Add(this.rbS);
            this.Controls.Add(this.btnNiuno);
            this.Controls.Add(this.btnElimina);
            this.Controls.Add(this.btnTodos);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvFacturas);
            this.Controls.Add(this.dtpFecha);
            this.Name = "Facturas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facturación Electronica 18 JUNIO 2019";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Facturas_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Facturas_MouseDoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFacturas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSii)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.RadioButton rbS;
        private System.Windows.Forms.RadioButton rbL;
        private System.Windows.Forms.DataGridView dgvFacturas;
    
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.Button btnElimina;
        private System.Windows.Forms.Button btnNiuno;
        private System.Windows.Forms.DataGridView dgvSii;
        private System.Windows.Forms.Button btnSii;
        private System.Windows.Forms.Button btnSAlir;
        private System.Windows.Forms.DataGridViewTextBoxColumn ocd;
        private System.Windows.Forms.DataGridViewTextBoxColumn numdocD;
        private System.Windows.Forms.CheckBox chkbUpload;
        private System.Windows.Forms.CheckBox chkSoloXmlCliente;
        private System.Windows.Forms.CheckBox chkEnviaXmlCliente;
        private System.Windows.Forms.CheckBox chkPrefactura;
        private System.Windows.Forms.Button btnPDFS;
        private System.Windows.Forms.CheckBox chkEnviaPDFCliente;
        private cl.sii.palena.QueryEstDteAvService queryEstDteAvService1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn oc;
        private System.Windows.Forms.DataGridViewTextBoxColumn na;
        private System.Windows.Forms.DataGridViewTextBoxColumn numdoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn fecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTAL;
        private System.Windows.Forms.DataGridViewTextBoxColumn codcli;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn neto;
        private System.Windows.Forms.DataGridViewTextBoxColumn TRACK;
    }
}

