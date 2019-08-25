using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.IO;
using System.Web;
using System.Security.Cryptography.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;

using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;



namespace DTE33
{
    public partial class Facturas : Form
    {
          
        public static string NumeroFacturas = "10";
        public static string Folio = "110";
      
        public static DateTime FechaEmi = DateTime.Today.Date;
        public static string xml = "";
        public static DataTable DTCliente;
        public static DataTable DTFacturas;
        public static DataTable DTDetalle;
        public static DataTable DTNfacturas;
        public static string Fact = "FACT";
        public static string Documento;
        //public static string  Unidad="S:\\";

        public static string FechaEmision;
        public static int largo_item = 0;
        public static Int32 iid;
        public static int Fila = 0;
        public static int columna = 0;
        public static int FilaSii = 0;
        public static int columnaSii = 0;
        public static string DTE_ENVIO = "";
        public static string DTE_ENVIO_SEED = "";
        public static string MNT;
        public static string IT1;
        public static string NameXml { get; set; }
        public static string RutReceptor;
        public static string Xml_alcliente;
        public static string xmlCliente;
        public static string pdfCliente;

        public static string sEmailXML;
        public static string sEmailPDF;
        public static string sEmailPDF1;
        public static string sEmailPDF2;
        public static string sEmailPDF3;
        public static string sEmailPDF4;
        public static string sIdCliente;
        public static string sServicioW;
        public static string sServicioB;
        public static string sServicioE;
        


        public static string Rut;
        public static string sFecha;
        public static string ITEM1;
        public static string respuestaSii = string.Empty;
        public static string Estado = string.Empty;
        public static string TrackID = string.Empty;
        public Facturas()
        {
            InitializeComponent();
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
        
            if (rbS.Checked == true)
            {
                DTE33.Constantes_Variables.Unidad_C_leyton = "s:\\";
            }

            if (rbL.Checked == true)
            {
                DTE33.Constantes_Variables.Unidad_C_leyton = "l:\\almadena";
            }
            carga_facturas();
            asigna_archivo_Facturas();
        }

        private void Facturas_Load(object sender, EventArgs e)
        {
            if (rbS.Checked == true)
            {
                DTE33.Constantes_Variables.Unidad_C_leyton = "s:\\";
            }

            if (rbL.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad_C_leyton = "l:\\almadena";
            }
            carga_facturas();
            asigna_archivo_Facturas();
            btnSii.Enabled = false;

        }
        private void asigna_archivo_Facturas()
        {
            string StrMes;//=Operaciones.DetalleFacturas.StrPeriodo.Substring(3, 2).Trim();
            string StrAno;//=Operaciones.DetalleFacturas.StrPeriodo.Substring(6, 4).Trim();
            int ano = 0;
            StrMes = dtpFecha.Value.Month.ToString();
            ano = dtpFecha.Value.Year;

            StrAno = (ano - 2000).ToString();

            if (dtpFecha.Value.Month < 10)
            {
                StrMes = "0" + StrMes;
            }

            if ((ano - 2000) < 10)
            {
                StrAno = "0" + StrAno;
            }
            Fact = "FACT" + StrAno + StrMes;
        }
        private void carga_facturas()
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            //OleDbDataReader dr;
            string StrOledbDBFIV;
            double dValor = 0;
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad_C_leyton + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsFactura = new DataSet();
                OleDbDataAdapter daCliente = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT codigosii,ct.numdoc, ct.fecha, ct.codcli, cli.nombre,ct.oc,ct.na,CT.TOTAL,cli.codcli,cli.rut,cli.dvrut,cli.nombre,ct.neto,ct.trackid  ";
                StrOledbDBFIV += " FROM CTACTE as ct ";
                StrOledbDBFIV += " INNER JOIN maecli as cli ";
                StrOledbDBFIV += " ON ( ct.codcli = cli.codcli )";
                StrOledbDBFIV += " where  codigosii =33";
                StrOledbDBFIV += " and month(ct.fecha)= " + dtpFecha.Value.Month.ToString();
                StrOledbDBFIV += " and  year(ct.fecha)= " + dtpFecha.Value.Year.ToString();
                StrOledbDBFIV += " and   day(ct.fecha)= " + dtpFecha.Value.Day.ToString();

              
                StrOledbDBFIV += " order by ct.codigosii, ct.numdoc asc ";

                daCliente = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daCliente.Fill(dsFactura);
                dgvFacturas.Rows.Clear();
                dgvSii.Rows.Clear();


                OldbComando.CommandText = StrOledbDBFIV;


                if (dsFactura.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFactura.Tables[0].Rows.Count; i++)
                    {
                        {
                            if (dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "1851")
                            {
                                MessageBox.Show(dsFactura.Tables[0].Rows[i]["OC"].ToString());
                            }

                            if (dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "2050" || dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "2150" || dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "2300" || dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "1848" || dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "1851" || dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString() == "2316")
                            
                             {
                                 if (dsFactura.Tables[0].Rows[i]["OC"].ToString().Length > 0)
                                    {
                                        int renglon = dgvFacturas.Rows.Add();
                                        dgvSii.Rows.Add();
                                        dgvFacturas.Rows[renglon].Cells["numdoc"].Value = dsFactura.Tables[0].Rows[i]["numdoc"].ToString();
                                        dgvFacturas.Rows[renglon].Cells["fecha"].Value = dsFactura.Tables[0].Rows[i]["fecha"].ToString().Substring(0, 10);
                                        dgvFacturas.Rows[renglon].Cells["OC"].Value = dsFactura.Tables[0].Rows[i]["OC"].ToString();
                                        dgvFacturas.Rows[renglon].Cells["na"].Value = dsFactura.Tables[0].Rows[i]["na"].ToString();
                                        dValor = Convert.ToDouble(dsFactura.Tables[0].Rows[i]["TOTAL"].ToString());
                                        dgvFacturas.Rows[renglon].Cells["TOTAL"].Value = string.Format("{0:N0}", dValor);

                                        dgvFacturas.Rows[renglon].Cells["codcli"].Value = dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString();

                                        dgvFacturas.Rows[renglon].Cells[6].Value = dsFactura.Tables[0].Rows[i]["rut"].ToString() + dsFactura.Tables[0].Rows[i]["dvrut"].ToString();
                                        dgvFacturas.Rows[renglon].Cells[7].Value = dsFactura.Tables[0].Rows[i]["nombre"].ToString();

                                        dValor = Convert.ToDouble(dsFactura.Tables[0].Rows[i]["neto"].ToString());
                                        dgvFacturas.Rows[renglon].Cells[8].Value = string.Format("{0:N0}", dValor);
                                        dgvFacturas.Rows[renglon].Cells[9].Value = dsFactura.Tables[0].Rows[i]["trackid"].ToString();


                                        dgvFacturas.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                                        dgvSii.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                                    }
                             }
                             else
                             {
                                 int renglon = dgvFacturas.Rows.Add();
                                 dgvSii.Rows.Add();
                                 dgvFacturas.Rows[renglon].Cells["numdoc"].Value = dsFactura.Tables[0].Rows[i]["numdoc"].ToString();
                                 dgvFacturas.Rows[renglon].Cells["fecha"].Value = dsFactura.Tables[0].Rows[i]["fecha"].ToString().Substring(0, 10);
                                 dgvFacturas.Rows[renglon].Cells["OC"].Value = dsFactura.Tables[0].Rows[i]["OC"].ToString();
                                 dgvFacturas.Rows[renglon].Cells["na"].Value = dsFactura.Tables[0].Rows[i]["na"].ToString();
                                 dValor = Convert.ToDouble(dsFactura.Tables[0].Rows[i]["TOTAL"].ToString());
                                 dgvFacturas.Rows[renglon].Cells["TOTAL"].Value = string.Format("{0:N0}", dValor);

                                 dgvFacturas.Rows[renglon].Cells["codcli"].Value = dsFactura.Tables[0].Rows[i]["ct.codcli"].ToString();

                                 dgvFacturas.Rows[renglon].Cells[6].Value = dsFactura.Tables[0].Rows[i]["rut"].ToString() + dsFactura.Tables[0].Rows[i]["dvrut"].ToString();
                                 dgvFacturas.Rows[renglon].Cells[7].Value = dsFactura.Tables[0].Rows[i]["nombre"].ToString();

                                 dValor = Convert.ToDouble(dsFactura.Tables[0].Rows[i]["neto"].ToString());
                                 dgvFacturas.Rows[renglon].Cells[8].Value = string.Format("{0:N0}", dValor);
                                 dgvFacturas.Rows[renglon].Cells[9].Value = dsFactura.Tables[0].Rows[i]["trackid"].ToString();
                                 dgvFacturas.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                                 dgvSii.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                             }
                        }
                        
                       
                   
                    }
                }
                else
                {
                    MessageBox.Show("Seleccion de registros 0");

                }

                dgvFacturas.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);

           
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error de Selección de Registros DBF " + ex.Message);
            }
        }
  
        private void dgvFacturas_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //Captura el numero de filas del datagridview

                string RowsNumber = (e.RowIndex + 1).ToString();


                while (RowsNumber.Length < dgvFacturas.RowCount.ToString().Length)
                {
                    RowsNumber = "0" + RowsNumber;

                }

                SizeF size = e.Graphics.MeasureString(RowsNumber, this.Font);


                if (dgvFacturas.RowHeadersWidth < Convert.ToInt32(size.Width + 20))
                {
                    dgvFacturas.RowHeadersWidth = Convert.ToInt32(size.Width + 20);

                }

                Brush ob = SystemBrushes.ControlText;

                e.Graphics.DrawString(RowsNumber, this.Font, ob, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnSAlir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFacturas.RowCount; i++)
            {
                if (dgvFacturas.Rows[i].Cells["numdoc"].Value.ToString() != "")
                {
                    dgvSii.Rows[i].Cells["numdocD"].Value = dgvFacturas.Rows[i].Cells["numdoc"].Value;
                  

                    dgvFacturas.Rows[i].Cells["numdoc"].Value = "";
             
                    btnSii.Enabled = true;
                }
            }
        }

        private void dgvFacturas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Fila = e.RowIndex;
            columna = e.ColumnIndex;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            sIdCliente = dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString();
            Rut = dgvFacturas.Rows[Fila].Cells[5].Value.ToString().Trim();
            sFecha = dgvFacturas.Rows[Fila].Cells["fecha"].Value.ToString();
            int largo=Rut.Length;
            int n = largo - 1;
            RutReceptor = Rut.Substring(0, n) + "-" + Rut.Substring(n,1);
            Trae_pdf_automatico_Cliente(dgvFacturas.Rows[Fila].Cells[6].Value.ToString().Trim());
            //+"-" + Rut.Substring(largo, 1);
            if (dgvFacturas.Rows[Fila].Cells["numdoc"].Value != null )
            {
                if (dgvFacturas.Rows[Fila].Cells["numdoc"].Value.ToString() != "")
                {
                    //if (dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "1848"  || dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "1473"|| dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "2316")
                       if (dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "2050" || dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "2150" || dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "1848" || dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "1473" || dgvFacturas.Rows[Fila].Cells["codcli"].Value.ToString() == "2316")
                    {
                        if (dgvFacturas.Rows[Fila].Cells["oc"].Value.ToString().Length > 0  )
                        {
                            dgvSii.Rows[Fila].Cells["numdocD"].Value = dgvFacturas.Rows[Fila].Cells["numdoc"].Value;

                            dgvFacturas.Rows[Fila].Cells["numdoc"].Value = "";

                            btnSii.Enabled = true;

                        }
                        else
                        {
                            MessageBox.Show("Debe contener una orden de compra este DTE, Solo PRefactura","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            if (chkPrefactura.Checked == true)
                            {
                                dgvSii.Rows[Fila].Cells["numdocD"].Value = dgvFacturas.Rows[Fila].Cells["numdoc"].Value;

                                dgvFacturas.Rows[Fila].Cells["numdoc"].Value = "";

                                btnSii.Enabled = true;

                            }
                                
                        }
                    }
                    else
                    {
                        dgvSii.Rows[Fila].Cells["numdocD"].Value = dgvFacturas.Rows[Fila].Cells["numdoc"].Value;

                        dgvFacturas.Rows[Fila].Cells["numdoc"].Value = "";

                        btnSii.Enabled = true;
                    }
                }
            }
        }

        private void dgvSii_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FilaSii = e.RowIndex;
            columnaSii = e.ColumnIndex;
          
        }

        private void btnElimina_Click(object sender, EventArgs e)
        {
            if (dgvSii.Rows[FilaSii].Cells["numdocd"].Value.ToString() != "")
            {
                dgvFacturas.Rows[FilaSii].Cells["numdoc"].Value = dgvSii.Rows[FilaSii].Cells["numdocd"].Value;
            
                dgvSii.Rows[FilaSii].Cells["numdocd"].Value = "";
                
            }
        }

        private void btnNiuno_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <dgvSii.RowCount; i++)
            {
                if (dgvSii.Rows[i].Cells["numdocd"].Value != null)
                {
                    if (dgvSii.Rows[i].Cells["numdocd"].Value.ToString().Length > 0)
                    {
                        dgvFacturas.Rows[i].Cells["numdoc"].Value = dgvSii.Rows[i].Cells["numdocd"].Value;
              

                        dgvSii.Rows[i].Cells["numdocd"].Value = "";
                        
                    }
                }
            }
        }

        private void btnSii_Click(object sender, EventArgs e)
        {
            bool SiHay = false;
            for (int i=0; i < dgvSii.RowCount;i++)
            {
                if (dgvSii.Rows[i].Cells["numdocd"].Value != null)
                {
                    if (dgvSii.Rows[i].Cells["numdocd"].Value.ToString().Length > 0)
                    {
                        SiHay = true;
                    }
                }

            }
            if (SiHay== false)
            {
                MessageBox.Show("No ha asignado facturas");
            }
            else
            {
                for(int i=0;i<dgvSii.RowCount;i++)
                {
                    if (dgvSii.Rows[i].Cells["numdocd"].Value != null)
                    {
                        if (chkSoloXmlCliente.Checked == false)
                        {
                            Factura(i);
                        }
                        else
                        {
                           
                            Folio = dgvSii.Rows[i].Cells["numdocd"].Value.ToString();
                            Xml_alcliente = @"X:\dte33\xmlsii\EnvioDTE_" + @Folio + ".xml";
                            Crea_xml_alCliente(Xml_alcliente);
                            Trae_Email_Cliente();
                                if (sEmailXML.Length > 0)
                                {
                                    Envia_Correo_Xml_alCliente(xmlCliente, sEmailXML);
                                }
                                else
                                {
                                    MessageBox.Show("cliente sin DTE Eamil");
                                }
                          
                            this.Close();
                        }
                    }
                  

                }
            }
        }

       

        private void Factura(int indice)
        {
            XmlDocument DTE = new XmlDocument();
            string FechaEmision;
            string FechaVencimiento;
            string FechaOC;
            string FechaNA;
            string xNombre = "";

            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad_C_leyton + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsFactura = new DataSet();
                OleDbDataAdapter daFactura = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT * FROM ctacte as cta";
                StrOledbDBFIV += " INNER JOIN MAECLI as MAECLI ON (cta.CODCLI = MAECLI.CODCLI )";
                StrOledbDBFIV += " where cta.codigosii <> 0";
                StrOledbDBFIV += " AND cta.numdoc =" + dgvSii.Rows[indice].Cells["numdocd"].Value.ToString();
                StrOledbDBFIV += " and month(cta.fecha)= " + dtpFecha.Value.Month.ToString();
                StrOledbDBFIV += " and  year(cta.fecha)= " + dtpFecha.Value.Year.ToString();
                StrOledbDBFIV += " and   day(cta.fecha)= " + dtpFecha.Value.Day.ToString();
                daFactura = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daFactura.Fill(dsFactura);
               DTFacturas = dsFactura.Tables[0];
                // barra de progreso
                objconnDBFIV.Close();

            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            int n =DTFacturas.Rows.Count;
            double neto =Convert.ToDouble( DTFacturas.Rows[0]["neto"].ToString());
            double iva = Convert.ToDouble(DTFacturas.Rows[0]["iva"].ToString());
            double total = Convert.ToDouble(DTFacturas.Rows[0]["total"].ToString());
          

            {
                RutReceptor = DTFacturas.Rows[0]["MAECLI.rut"].ToString() + "-" + DTFacturas.Rows[0]["dvrut"].ToString();

                FechaEmision =DTFacturas.Rows[0]["fecha"].ToString().Trim().Substring(6, 4) + "-" +
                              DTFacturas.Rows[0]["fecha"].ToString().Trim().Substring(3, 2) + "-" +
                              DTFacturas.Rows[0]["fecha"].ToString().Trim().Substring(0, 2);
                xNombre = DTFacturas.Rows[0]["nombre"].ToString();
                xNombre = xNombre.Replace("&", "y");
                xNombre = CambiaNombre.Nombre(xNombre);
                Folio = dgvSii.Rows[indice].Cells["numdocd"].Value.ToString();
                string fechahora = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                xml = @"<?xml version='1.0' encoding='ISO-8859-1'?>";

                FechaVencimiento= DTFacturas.Rows[0]["fechaven"].ToString().Trim().Substring(6, 4) + "-" +
                           DTFacturas.Rows[0]["fechaven"].ToString().Trim().Substring(3, 2) + "-" +
                           DTFacturas.Rows[0]["fechaven"].ToString().Trim().Substring(0, 2);

                xml += @"<DTE version='1.0'>";
                xml += @"<Documento ID='T33F" + Folio + "'>";
                xml += @"<Encabezado>";
                xml += @"<IdDoc>";
                xml += @"<TipoDTE>33</TipoDTE >";
                xml += @"<Folio>" + Folio + "</Folio>";
                xml += @"<FchEmis>" + FechaEmision + "</FchEmis>";
                xml += @"<MntPagos>";


                xml += @"<FchPago>" + FechaVencimiento + "</FchPago>";
                xml += @"<MntPago>"+ total.ToString() + "</MntPago>";
                xml += @"</MntPagos>";



               
                xml += @"</IdDoc>";
                xml += @"<Emisor>";
                xml += @"<RUTEmisor>93945000-9</RUTEmisor>";
                xml += @"<RznSoc>ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.</RznSoc>";
                xml += @"<GiroEmis>ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES</GiroEmis>";
                xml += @"<Acteco>630200</Acteco>";
                xml += @"<DirOrigen>MONEDA 812 OFICINA 705</DirOrigen>";
                xml += @"<CmnaOrigen>Santiago</CmnaOrigen>";
                xml += @"<CiudadOrigen>Santiago</CiudadOrigen>";
                xml += @"</Emisor>";
                xml += @"<Receptor>";
                xml += @"<RUTRecep>" + DTFacturas.Rows[0]["MAECLI.rut"].ToString() + "-" + DTFacturas.Rows[0]["dvrut"].ToString() + "</RUTRecep>";
                int largo_item = 39;
                if (xNombre.Length < 39)
                    largo_item = xNombre.Length;
                xml += @"<RznSocRecep>" + xNombre.Substring(0, largo_item) + "</RznSocRecep>";
                xml += @"<GiroRecep>" + DTFacturas.Rows[0]["giro"].ToString() + "</GiroRecep>";
                //xml += @"<Contacto>NO</Contacto>";
                string xDomic;
                string xComuna;
                string xCiudad;

                xDomic = DTFacturas.Rows[0]["domic"].ToString();
                xDomic = xDomic.Replace("&", "y");
                xDomic = CambiaNombre.Nombre(xDomic);


                xml += @"<DirRecep>" + xDomic + "</DirRecep>";

                xComuna = DTFacturas.Rows[0]["Comuna"].ToString();
                xComuna = xComuna.Replace("&", "y");
                xComuna = CambiaNombre.Nombre(xComuna);

                xml += @"<CmnaRecep>" + xComuna + "</CmnaRecep>";

                xCiudad = DTFacturas.Rows[0]["Ciudad"].ToString();
                xCiudad = xCiudad.Replace("&", "y");
                xCiudad = CambiaNombre.Nombre(xCiudad);

                xml += @"<CiudadRecep>" + xCiudad + "</CiudadRecep>";
                xml += @"</Receptor>";

                xml += @"<Totales>";
                xml += @"<MntNeto>" + neto.ToString() + "</MntNeto>";
                xml += @"<MntExe>0</MntExe>";
                xml += @"<TasaIVA>19</TasaIVA>";
                xml += @"<IVA>" + iva.ToString() + "</IVA>";
                xml += @"<MntTotal>" + total.ToString() + "</MntTotal>";
                xml += @"</Totales>";
                xml += @"</Encabezado>";

                //detalle de las liena de factura
                Detalle_Factura(Folio);
                //Detalle_Factura(DTFacturas.Rows[0]["numdoc"].ToString().Trim());
                // descuento global

                 Descuento_global(DTFacturas.Rows[0]["numdoc"].ToString().Trim());

                // si conytiene orde de compra como el caso de automotores gildemeister
                 if (DTFacturas.Rows[0][1].ToString() == "2050" || DTFacturas.Rows[0][1].ToString() == "2150" || DTFacturas.Rows[0][1].ToString() == "2300" || DTFacturas.Rows[0][1].ToString() == "1473" || DTFacturas.Rows[0][1].ToString() == "1848"|| DTFacturas.Rows[0][1].ToString() == "2316")
                {

                        if (DTFacturas.Rows[0]["oc"].ToString().Trim().Length > 0 )
                        {
                            FechaOC = DTFacturas.Rows[0]["fechaoc"].ToString().Trim().Substring(6, 4) + "-" +
                                     DTFacturas.Rows[0]["fechaoc"].ToString().Trim().Substring(3, 2) + "-" +
                                     DTFacturas.Rows[0]["fechaoc"].ToString().Trim().Substring(0, 2);
                            xml += @"<Referencia>";
                            xml += @"<NroLinRef>1</NroLinRef>";
                            xml += @"<TpoDocRef>801</TpoDocRef>";
                            xml += @"<FolioRef>" + DTFacturas.Rows[0]["oc"].ToString().Trim()+"</FolioRef>";
                            xml += @"<FchRef>" + FechaOC + "</FchRef>";
                            xml += @"</Referencia>";
                        }
                        else
                        {
                            MessageBox.Show("orden de compra no ");

                        }
                }
                 if (DTFacturas.Rows[0][1].ToString() == "2050" || DTFacturas.Rows[0][1].ToString() == "2150" || DTFacturas.Rows[0][1].ToString() == "2300" || DTFacturas.Rows[0][1].ToString() == "1473"|| DTFacturas.Rows[0][1].ToString() == "2316")
                 {
                     if (DTFacturas.Rows[0]["NA"].ToString().Trim().Length > 0)
                     {
                         FechaNA = DTFacturas.Rows[0]["fechaNA"].ToString().Trim().Substring(6, 4) + "-" +
                                  DTFacturas.Rows[0]["fechaNA"].ToString().Trim().Substring(3, 2) + "-" +
                                  DTFacturas.Rows[0]["fechaNA"].ToString().Trim().Substring(0, 2);
                         xml += @"<Referencia>";
                         xml += @"<NroLinRef>2</NroLinRef>";
                         xml += @"<TpoDocRef>802</TpoDocRef>";
                         xml += @"<FolioRef>" + DTFacturas.Rows[0]["na"].ToString().Trim() + "</FolioRef>";
                         xml += @"<FchRef>" + FechaNA + "</FchRef>";
                         xml += @"</Referencia>";
                     }
                     else
                     {
                         MessageBox.Show("orden de compra no ");

                     }
                 }
                //--------------------------------------------------------------------------------------------
                // comienza el ted
                xml += @"<TED version='1.0'>";
                xml += @"<DD>";
                xml += @"<RE>93945000-9</RE>";
                xml += @"<TD>33</TD>";
                xml += @"<F>" + Folio + "</F>";

                xml += @"<FE>" + FechaEmision + "</FE>";
                xml += @"<RR>" + DTFacturas.Rows[0]["maecli.rut"].ToString() + "-" + DTFacturas.Rows[0]["dvrut"].ToString() + "</RR>";
                largo_item = 39;
                if (xNombre.Length < 39)
                    largo_item = xNombre.Length;

                xml += @"<RSR>" + xNombre.Substring(0, largo_item) + "</RSR>";
                xml += @"<MNT>" + total.ToString() + "</MNT>";
                //xNombreProducto = DTDetalle.Rows[0]["glosa"].ToString().Trim() ;
                largo_item = 39;
                if (ITEM1.Length < 39)
                    largo_item = ITEM1.Length;
               ITEM1 = ITEM1.Replace("&", "y");
               ITEM1 = CambiaNombre.Nombre(ITEM1);


                xml += @"<IT1>" + ITEM1.Substring(0, largo_item) + "</IT1>";
                XmlDocument xDoc = new XmlDocument();
                //La ruta del documento XML permite rutas relativas
                if (!System.IO.Directory.Exists(@"X:\dte33\CAF\33.xml"))
                {
                    XmlDocument caf = new XmlDocument();
                    caf.PreserveWhitespace = true;
                    caf.Load(@"X:\dte33\CAF\33.xml");
                    string str = caf.OuterXml.ToString();
                    string var = "";
                    string var2 = "";
                    string var3 = "";
                    string var4 = "";
                    string var5 = "";
                    string var6 = "";
                    string var7 = "";
                    string var8 = "";
                    XmlNodeList elemList = caf.GetElementsByTagName("TD");
                    XmlNodeList elemList2 = caf.GetElementsByTagName("D");
                    XmlNodeList elemList3 = caf.GetElementsByTagName("H");
                    XmlNodeList elemList4 = caf.GetElementsByTagName("FA");
                    XmlNodeList elemList5 = caf.GetElementsByTagName("M");
                    XmlNodeList elemList6 = caf.GetElementsByTagName("E");
                    XmlNodeList elemList7 = caf.GetElementsByTagName("IDK");
                    XmlNodeList elemList8 = caf.GetElementsByTagName("FRMA");

                    var = elemList[0].InnerXml;
                    var2 = elemList2[0].InnerXml;
                    var3 = elemList3[0].InnerXml;
                    var4 = elemList4[0].InnerXml;
                    var5 = elemList5[0].InnerXml;
                    var6 = elemList6[0].InnerXml;
                    var7 = elemList7[0].InnerXml;
                    var8 = elemList8[0].InnerXml;


                    xml += @"<CAF version='1.0'>";
                    xml += @"<DA>";
                    xml += @"<RE>93945000-9</RE>";
                    xml += @"<RS>ALMADENA, ALMACENES DE DEPOSITOS NACIONA</RS>";
                    xml += @"<TD>" + var + "</TD>";
                    xml += @"<RNG>";
                    xml += @"<D>" + var2 + "</D>";
                    xml += @"<H>" + var3 + "</H>";
                    xml += @"</RNG>";
                    xml += @"<FA>" + var4 + "</FA>";
                    xml += @"<RSAPK><M>" + var5 + "</M><E>" + var6 + "</E></RSAPK>";
                    xml += @"<IDK>" + var7 + "</IDK>";
                    xml += @"</DA>";
                    xml += @"<FRMA algoritmo='SHA1withRSA'>" + var8 + "</FRMA>";
                    xml += @"</CAF>";

                }

                xml += @"<TSTED>" + fechahora + "</TSTED>";
                xml += @"</DD>";

                xml += @"<FRMT algoritmo='SHA1withRSA'>" + "var8" + "</FRMT>";
                xml += @"</TED>";
                xml += @"<TmstFirma>" + fechahora + "</TmstFirma>";
                //
            
                xml += @"</Documento>";

                xml += @"</DTE>";

                var xmldoc = new XmlDocument();


                var uri = @"X:\dte33\XMLSII\";
                Xml_alcliente = @"X:\dte33\xmlcliente\";
                var Caratula = @"X:\dte33\CARATULA\EnvioDTE_33.xml";
                var cn = "Nicolas Gaston";


                uri += @Folio + ".xml";
              
                xmldoc.LoadXml(xml);

                xmldoc.Save(uri);

                AlineaDTE(uri);


                string FRMT = "";
                FRMT = Timbrar_33(uri); //  actuliza el xml incorporando el <FMR>

                DTE.PreserveWhitespace = true;
                DTE.Load(uri);
                XmlElement node = (XmlElement)DTE.SelectSingleNode("DTE/Documento/TED/FRMT");
                XmlElement TodoTED = (XmlElement)DTE.SelectSingleNode("DTE/Documento");
                if (node != null)
                {
                    node.InnerText = FRMT;
                    string sTED = TodoTED.SelectSingleNode("TED").OuterXml;
                    string Nombre = @"X:\DTE33\TIMBRE\" + Folio + ".png";
                    Crea_Imagen_timbre_elctronico(sTED, Nombre);
                    DTE.DocumentElement.SetAttribute("xmlns", "http://www.sii.cl/SiiDte");
                    DTE.Save(uri);

                }
                if (chkSoloXmlCliente.Checked == false)
                {
                    firmarDocumentoDTE(uri, cn);
                    ArmaEnvioDte(Caratula, uri, cn);
                    if (chkbUpload.Checked == true)
                    {
                        Envia_dte(cn, DTE_ENVIO); // UPLOAD 
                        /////////////////////////////////////////////////////////////////////////////////////
                    }
                    if (TrackID.Length > 0)
                    {
                        //Imprime_PDF(Folio, DTE_ENVIO);
                        Imprime_PDF_COPIA(Folio, DTE_ENVIO);
                        Imprime_PDF_Cedible(Folio, DTE_ENVIO);
                        Imprime_PDF(Folio, DTE_ENVIO);
                       
                        if (chkEnviaPDFCliente.Checked == true)
                        {
                            pdfCliente = @"X:\dte33\PDF\pdf_33DTE" + @Folio + ".pdf";
                            Trae_Email_Cliente();
                            if (sEmailPDF.Length > 0)
                            {
                                Envia_Correo_PDF_Cliente(pdfCliente, sEmailPDF, sEmailPDF1, sEmailPDF2, sEmailPDF3, sEmailPDF4);
                            }
                            else
                            {
                                MessageBox.Show("cliente sin pdf Email ");
                            }
                        }
                        // envia xml al cliente
                        Xml_alcliente = @"X:\dte33\xmlsii\EnvioDTE_" + @Folio + ".xml";
                        Xml_Al_Cliente(Xml_alcliente);
                    }
                    else
                    {
                        MessageBox.Show("TRACKiD sin dato=" + TrackID + " ESTADO=" + Estado);

                    }
                }
                else
                {
                    
                     Xml_alcliente = @"X:\dte33\xmlsii\EnvioDTE_" + @Folio + ".xml";
                    if (File.Exists(Xml_alcliente))
                    {
                        Xml_Al_Cliente(Xml_alcliente);
                    }
                    else
                    {
                        MessageBox.Show("No existe XML del Cliente Crerado");
                    }

                }
              
                this.Close();
            }

        }

        private void Trae_pdf_automatico_Cliente(string Rut)
        {
            SqlCommand ObjComando = new SqlCommand();
            string StrSQL;
            //string[] EmailsPDF = new string[6];
            // coneccion sqlserver
            using (SqlConnection objconnSQL = new SqlConnection(Constantes_Variables.ConexionStringW))
            {
                StrSQL = " select distinct cc.envioautomaticoPDF";
                StrSQL += "  FROM clientes AS cl";
                StrSQL += "  Inner join clientes_codigo as cc on cc.rut=cl.rut ";

                StrSQL += "  where cl.rut = '" + Rut + "'";

                try
                {
                    objconnSQL.Open();

                    // consulta la existencia del Clienete y sus Emails


                    DataSet dscliente = new DataSet();
                    SqlDataAdapter dacliente = new SqlDataAdapter();
                    dacliente = new SqlDataAdapter(StrSQL, objconnSQL);
                    dacliente.Fill(dscliente, "cliente");
                    objconnSQL.Close();
                    chkEnviaPDFCliente.Checked = false;

                    
                    if (dscliente.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dscliente.Tables[0].Rows.Count; i++)
                        {

                            if (dscliente.Tables[0].Rows[i]["envioautomaticoPDF"].ToString() == "S")
                            {
                                chkEnviaPDFCliente.Checked = true;

                            }
                        }
                    }
                   
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error Envio Automatico PDF Tabla Clientes");
                }
            }
        }



private void Xml_Al_Cliente(string Xml_alcliente)
        {
            Crea_xml_alCliente( Xml_alcliente);
            //MessageBox.Show("Enviado");
            if (chkEnviaXmlCliente.Checked == true)
            {
                Trae_Email_Cliente();
                if (sEmailXML.Length > 0)
                {
                    Envia_Correo_Xml_alCliente(xmlCliente, sEmailXML);
                }
                else
                {
                    MessageBox.Show("cliente sin DTE Eamil");
                }
            }
        }

      

        private void Envia_Correo_Xml_alCliente(string xmlalCliente,string sEmailXML)
        {
            Correo c = new Correo();
            c.enviarCorreo("dte@almadena.cl", "f15xe", "ALMADENA S.A._93945000_9-EnviodTE", "ALMADENA S.A._93945000_9-EnviodTE",sEmailXML, xmlalCliente);
        }
        //private void Trae_Email_Cliente()
        //{

        //    SqlCommand ObjComando = new SqlCommand();
        //    string StrSQL;
        //    string[] EmailsPDF = new string[6];
        //    // coneccion sqlserver
        //    using (SqlConnection objconnSQL = new SqlConnection(DTE33.Constantes_Variables.ConexionStringW))
        //    {
        //        StrSQL = " select emailXML,emailpdf,emailPDf1,emailpdf2,emailpdf3,emailpdf4 ";
        //        StrSQL += "  FROM clientes ";
        //        StrSQL += "  where rut = '" + RutReceptor.Replace("-", "") + "'";

        //        try
        //        {
        //            objconnSQL.Open();

        //            // consulta la existencia del Clienete y sus Emails


        //            DataSet dscliente = new DataSet();
        //            SqlDataAdapter dacliente = new SqlDataAdapter();
        //            dacliente = new SqlDataAdapter(StrSQL, objconnSQL);
        //            dacliente.Fill(dscliente, "cliente");
        //            objconnSQL.Close();
        //            sEmailXML = "";
        //            sEmailPDF = "";
        //            sEmailPDF1 = "";
        //            sEmailPDF2 = "";
        //            sEmailPDF3 = "";
        //            sEmailPDF4 = "";
        //            if (dscliente.Tables[0].Rows.Count > 0)
        //            {
        //                sEmailXML = dscliente.Tables[0].Rows[0]["EmailXML"].ToString();
        //                sEmailPDF = dscliente.Tables[0].Rows[0]["EmailPDF"].ToString();
        //                sEmailPDF1 = dscliente.Tables[0].Rows[0]["EmailPDF1"].ToString();
        //                sEmailPDF2 = dscliente.Tables[0].Rows[0]["EmailPDF2"].ToString();
        //                sEmailPDF3 = dscliente.Tables[0].Rows[0]["EmailPDF3"].ToString();
        //                sEmailPDF4 = dscliente.Tables[0].Rows[0]["EmailPDF4"].ToString();

        //            }
        //            else
        //            {
        //                MessageBox.Show("Cliente inexistente");
        //            }

        //        }
        //        catch (SqlException ex)
        //        {
        //            MessageBox.Show(ex.Message, "Error Tabla Clientes");
        //        }
        //    }
        //}
        private void Trae_Email_Cliente()
        {

            SqlCommand ObjComando = new SqlCommand();
            string StrSQL;
            string[] EmailsPDF = new string[6];
            // coneccion sqlserver
            using (SqlConnection objconnSQL = new SqlConnection(Constantes_Variables.ConexionStringW))
            {
                StrSQL = " select cl.emailxml,cl.emailpdf,cl.emailPDf1,cl.emailpdf2,cl.emailpdf3,cl.emailpdf4,cc.servicioW,cc.servicioB,cc.servicioE ";
                StrSQL += "  FROM clientes AS cl";
                StrSQL += "  Inner join clientes_codigo as cc on cc.rut=cl.rut ";

                StrSQL += "  where cl.rut = '" + RutReceptor.Replace("-", "") + "'";

                try
                {
                    objconnSQL.Open();

                    // consulta la existencia del Clienete y sus Emails


                    DataSet dscliente = new DataSet();
                    SqlDataAdapter dacliente = new SqlDataAdapter();
                    dacliente = new SqlDataAdapter(StrSQL, objconnSQL);
                    dacliente.Fill(dscliente, "cliente");
                    objconnSQL.Close();
                    sEmailXML = "";
                    sEmailPDF = "";
                    sEmailPDF1 = "";
                    sEmailPDF2 = "";
                    sEmailPDF3 = "";
                    sEmailPDF4 = "";
                    if (dscliente.Tables[0].Rows.Count > 0)
                    {
                        sEmailXML = dscliente.Tables[0].Rows[0]["emailxml"].ToString();
                        sEmailPDF = dscliente.Tables[0].Rows[0]["EmailPDF"].ToString();
                        sEmailPDF1 = dscliente.Tables[0].Rows[0]["EmailPDF1"].ToString();
                        sEmailPDF2 = dscliente.Tables[0].Rows[0]["EmailPDF2"].ToString();
                        sEmailPDF3 = dscliente.Tables[0].Rows[0]["EmailPDF3"].ToString();
                        sEmailPDF4 = dscliente.Tables[0].Rows[0]["EmailPDF4"].ToString();

                        sServicioW = (dscliente.Tables[0].Rows[0]["ServicioW"].ToString());
                        sServicioB = (dscliente.Tables[0].Rows[0]["ServicioB"].ToString());
                        sServicioE = (dscliente.Tables[0].Rows[0]["ServicioE"].ToString());

                    }
                    else
                    {
                        MessageBox.Show("Cliente inexistente");
                    }

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error Tabla Clientes");
                }
            }
        }
        //private void Envia_Correo_PDF_Cliente(string pdfCliente,string emailpdf, string emailpdf1, string emailpdf2, string emailpdf3, string emailpdf4)
        //{
        //    string Body;
        //    string Asunto;
        //    Body = "Adjunto a la presente factura del " + dtpFecha.Value.ToLongDateString() + " por los servicios de Warrants/Bodegaje, Que tenga Usted un Excelente día";
        //    Asunto="Factura Almadena" ;
            
        //    Correo c = new Correo();
        //    c.enviarCorreo_pdf("scoloma@almadena.cl", "f15xe",Body, Asunto,emailpdf,emailpdf1,emailpdf2,emailpdf3,emailpdf4 ,pdfCliente);
        //}

        private void Envia_Correo_PDF_Cliente(string pdfCliente, string emailpdf, string emailpdf1, string emailpdf2, string emailpdf3, string emailpdf4)
        {
            string Body;
            string Asunto;
            Body = "Adjunto a la presente, factura del " + dtpFecha.Value.ToLongDateString() + " por los servicios de ";
            if (sServicioW == "S")
                Body += "Warrants";
            if (sServicioB == "S")
                Body += ", Bodegaje";
            if (sServicioE == "S")
                Body += " y Envasado";




            Body += "<br />" + "<br />" + "Que tenga Usted una Excelente día";

            Body += "<br />" + "Atentamente, <br />";
            Body += "<br />" + "Sara Coloma G. <br />";
            Body += "Almadena S.A. <br />";
            Body += "Fono: 22 3476514 <br />";

            Asunto = "Factura Almadena Archivo PDF";

            Correo c = new Correo();
            c.enviarCorreo_pdf("scoloma@almadena.cl", "f15xe", Body, Asunto, emailpdf, emailpdf1, emailpdf2, emailpdf3, emailpdf4, pdfCliente);


        }
        private void Crea_xml_alCliente(string uricliente)
        {
            XmlDocument Dte = new XmlDocument();
            Dte.PreserveWhitespace = true;
            Dte.Load(uricliente);
            XPathNavigator navigator = Dte.CreateNavigator();
            XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("sii", "http://www.sii.cl/SiiDte");

            foreach (XPathNavigator nav in navigator.Select("//sii:RutReceptor", manager))
            {
                if (nav.Value == "60803000-K")
                {
                    nav.SetValue(RutReceptor);
                }
            }

            xmlCliente = @"X:\dte33\xmlcliente\EnvioDTE_" + @Folio + ".xml";
           
            Dte.Save(xmlCliente);
            
            
        }

      
        private void Detalle_Factura(string numdoc)
        {
            int linea = 1;
            string Cuenta;
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad_C_leyton + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsDetalle = new DataSet();
                OleDbDataAdapter daDetalle = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT glosa,comision,numlindeta,cuenta FROM " + Fact;
                StrOledbDBFIV += " where numfactu=" + numdoc;
                StrOledbDBFIV += " order by numlindeta";

                daDetalle = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daDetalle.Fill(dsDetalle);
                DTDetalle = dsDetalle.Tables[0];
                objconnDBFIV.Close();
                if (dsDetalle.Tables[0].Rows.Count > 0)
                {
                    //<IT1>linea 1 de la factura
                  ITEM1 = DTDetalle.Rows[0]["glosa"].ToString().Trim();
                    for (int i = 0; i < DTDetalle.Rows.Count; i++)
                    {
                        Cuenta = DTDetalle.Rows[i]["cuenta"].ToString().Trim();
                        // si no es descuento
                        if (Cuenta != "5-01-01-005")
                        {
                            string xglosa;
                            xglosa = DTDetalle.Rows[i]["glosa"].ToString().Trim();
                            xglosa = xglosa.Replace("&", "y");
                            xglosa = CambiaNombre.Nombre(xglosa);
                            xml += @"<Detalle>";
                            xml += "<NroLinDet>" + linea.ToString() + "</NroLinDet>";
                            xml += @"<NmbItem>" + xglosa + "</NmbItem>";
                            xml += @"<QtyItem>1</QtyItem>";
                            xml += @"<PrcItem>" + DTDetalle.Rows[i]["comision"].ToString().Trim() + "</PrcItem>";
                            xml += @"<MontoItem>" + DTDetalle.Rows[i]["comision"].ToString().Trim() + "</MontoItem>";
                            xml += @"</Detalle>";

                            linea++;
                        }

                    }
                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Descuento_global(string numdoc)
        {
            //int linea = 1;

            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad_C_leyton + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsDetalle = new DataSet();
                OleDbDataAdapter daDetalle = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT glosa,comision,numlindeta,cuenta FROM " + Fact;
                StrOledbDBFIV += " where numfactu=" + numdoc;
                StrOledbDBFIV += " and cuenta='5-01-01-005'";
                StrOledbDBFIV += " order by numlindeta";

                daDetalle = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daDetalle.Fill(dsDetalle);
                DTDetalle = dsDetalle.Tables[0];
                objconnDBFIV.Close();
                int lineadcto = 1;
                string Descuento = "";
                for (int i = 0; i < DTDetalle.Rows.Count; i++)
                {
                                     
                        string xglosa;
                        xglosa = DTDetalle.Rows[i]["glosa"].ToString().Trim();
                        xglosa = xglosa.Replace("&", "y");
                        xglosa = CambiaNombre.Nombre(xglosa);
                        xml += @"<DscRcgGlobal>";
                        xml += @"<NroLinDR>" + lineadcto.ToString().Trim() + "</NroLinDR>";
                        xml += @"<TpoMov>D</TpoMov>";
                        xml += @"<GlosaDR>" + xglosa + "</GlosaDR>";
                        xml += @"<TpoValor>$</TpoValor>";
                        Descuento = DTDetalle.Rows[i]["comision"].ToString().Trim();
                       
                        Descuento=Descuento.Replace("-", "");
                       
                        xml += @"<ValorDR>" + Descuento.Trim() + "</ValorDR>";
                        xml += @"</DscRcgGlobal>";
                        lineadcto++;
                  
                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void AlineaDTE(string uri)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(uri, System.Text.Encoding.GetEncoding("ISO-8859-1")))
            {
                string linea = sr.ReadLine();
                linea = sr.ReadLine();
                linea = linea.Trim();
                sb.AppendLine(linea);
                while (sr.Peek() != -1)
                {
                    linea = sr.ReadLine();
                    linea = linea.Trim();
                    sb.AppendLine(linea);
                }
            }
            string uri2 = System.IO.Path.ChangeExtension(uri, ".tempXml");
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(uri2, false, System.Text.Encoding.GetEncoding("ISO-8859-1")))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
                sw.WriteLine(sb.ToString());
            }
            System.IO.File.Delete(uri);
            System.IO.File.Copy(uri2, uri);
            System.IO.File.Delete(uri2);
        }
        private string Timbrar_33(string uri)
        {
            XmlDocument DTE = new System.Xml.XmlDocument();


            DTE.PreserveWhitespace = true;
            DTE.Load(uri);

            string sTED = string.Empty;
            string sTodoTED = string.Empty;
            //SoloTED = DTE.SelectSingleNode();

            XmlElement TED = (XmlElement)DTE.SelectSingleNode("DTE/Documento/TED");
            XmlElement TodoTED = (XmlElement)DTE.SelectSingleNode("DTE/Documento");
            if (TodoTED != null)
            {
                sTodoTED = TodoTED.SelectSingleNode("TED").OuterXml;
            }
            sTodoTED = sTodoTED.Replace("\t", string.Empty);
            sTodoTED = sTodoTED.Replace("\r\n", string.Empty);
            sTodoTED = sTodoTED.Replace("\r", string.Empty);
            sTodoTED = sTodoTED.Replace("\n", string.Empty);
            string Nombre = @"X:\dte33\TIMBRE\" + Folio + ".png";
            //Crea_Imagen_timbre_elctronico(sTodoTED, Nombre);


            if (TED != null)
            {
                sTED = TED.SelectSingleNode("DD").OuterXml;
            }
            sTED = sTED.Replace("\t", string.Empty);
            sTED = sTED.Replace("\r\n", string.Empty);
            sTED = sTED.Replace("\r", string.Empty);
            sTED = sTED.Replace("\n", string.Empty);

            //--------------------caf---------------------
            string Caf = @"X:\dte33\CAF\33.xml";
            XmlDocument XmlCaf = new XmlDocument();
            XmlCaf.PreserveWhitespace = true;
            XmlCaf.Load(Caf);
            string Rsask_sin = string.Empty;
            XmlElement RSASK = (XmlElement)XmlCaf.SelectSingleNode("AUTORIZACION/RSASK");
            if (RSASK != null)
            {
                Rsask_sin = RSASK.InnerText;
                Rsask_sin = Rsask_sin.Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty);
                Rsask_sin = Rsask_sin.Replace("-----END RSA PRIVATE KEY-----", string.Empty);
                Rsask_sin = Rsask_sin.Replace("\r\n", string.Empty);
                Rsask_sin = Rsask_sin.Replace("\n", string.Empty);
                Rsask_sin = Rsask_sin.Replace("\r", string.Empty);
            }
            ////
            //// Calcule el hash de los datos a firmar DD
            //// transformando la cadena DD a arreglo de bytes, luego con
            //// el objeto 'SHA1CryptoServiceProvider' creamos el Hash del
            //// arreglo de bytes que representa los datos del DD
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] bytesStrDD = ByteConverter.GetBytes(sTED);
            byte[] HashValue = new SHA1CryptoServiceProvider().ComputeHash(bytesStrDD);
            ////
            //// Cree el objeto Rsa para poder firmar el hashValue creado
            //// en el punto anterior. La clase FuncionesComunes.crearRsaDesdePEM()
            //// Transforma la llave rivada del CAF en formato PEM a el objeto
            //// Rsa necesario para la firma.
            RSACryptoServiceProvider rsa = FuncionesComunes.crearRsaDesdePEM(Rsask_sin);

            ////
            //// Firme el HashValue ( arreglo de bytes representativo de DD )
            //// utilizando el formato de firma SHA1, lo cual regresará un nuevo 
            //// arreglo de bytes.
            byte[] bytesSing = rsa.SignHash(HashValue, "SHA1");

            ////
            //// Recupere la representación en base 64 de la firma, es decir de
            //// el arreglo de bytes
            string frmt = Convert.ToBase64String(bytesSing);
            return frmt;


        }
        public static void firmarDocumentoDTE(string uriDTE, string CN)
        {
            X509Certificate2 certificado = obtenerCertificado(CN);
            XmlDocument DTE = new XmlDocument();
            DTE.PreserveWhitespace = true;
            DTE.Load(uriDTE);
            string strRreference = "#";
            XmlNamespaceManager ns = new XmlNamespaceManager(DTE.NameTable);
            ns.AddNamespace("sii", "http://www.sii.cl/SiiDte");
            XmlElement ID = (XmlElement)DTE.SelectSingleNode("//sii:Documento", ns);
            if (ID != null)
            {
                strRreference += ID.GetAttribute("ID");
            }
            SignedXml signedXml = new SignedXml(DTE);
            signedXml.SigningKey = certificado.PrivateKey;
            Signature XMLSignature = signedXml.Signature;
            Reference reference = new Reference();
            reference.Uri = strRreference;
            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((System.Security.Cryptography.RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            DTE.DocumentElement.AppendChild(DTE.ImportNode(xmlDigitalSignature, true));
            DTE.Save(uriDTE);
        }
    
        private void Crea_Imagen_timbre_elctronico(string sTED, string Nombre)
        {

            string sTodoTED = sTED;

            sTodoTED = sTodoTED.Replace("\t", string.Empty);
            sTodoTED = sTodoTED.Replace("\r\n", string.Empty);
            sTodoTED = sTodoTED.Replace("\r", string.Empty);
            sTodoTED = sTodoTED.Replace("\n", string.Empty);


            Bitmap bm;
            bm = DTE33.BarCode.PDF417(sTodoTED, 1);
            bm.Save(Nombre, System.Drawing.Imaging.ImageFormat.Png);
        }



        public static void ArmaEnvioDte(string uriCartula, string uri, string CN)
        {
            XmlDocument EnvioDte = new XmlDocument();
            EnvioDte.PreserveWhitespace = true;
            EnvioDte.Load(uriCartula);
            XmlDocument Dte = new XmlDocument();
            Dte.PreserveWhitespace = true;
            Dte.Load(uri);
            XmlNamespaceManager ns = new XmlNamespaceManager(EnvioDte.NameTable); //caratula
            XmlNamespaceManager ns2 = new XmlNamespaceManager(Dte.NameTable); // uri

            ns.AddNamespace("sii", "http://www.sii.cl/SiiDte"); // cratula
            ns2.AddNamespace("sii", "http://www.sii.cl/SiiDte"); // xmlcompleto

                                          //caratula          uri           // del dte completo  
            XmlElement node = (XmlElement)EnvioDte.ImportNode(Dte.DocumentElement, true);

            EnvioDte.SelectSingleNode("sii:EnvioDTE/sii:SetDTE", ns).AppendChild(node);
            string xpath = "sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:RUTRecep";
            string sRutReceptor = Dte.SelectSingleNode(xpath, ns2).InnerText;
            string xpath2 = "sii:EnvioDTE/sii:SetDTE/sii:Caratula/sii:TmstFirmaEnv";
            EnvioDte.SelectSingleNode(xpath2, ns).InnerText = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            string newName = "{0}\\EnvioDTE_{1}.XML";
          

            string sPathxml = System.IO.Path.GetDirectoryName(uri);
            string sNamexml = System.IO.Path.GetFileNameWithoutExtension(uri);
            NameXml = string.Format(newName, sPathxml, sNamexml);
            DTE_ENVIO = NameXml;

            DTE_ENVIO_SEED = string.Format(newName, @"X:\dte33\XMLSEMILLA", sNamexml);
            EnvioDte.Save(NameXml);
            Xml_alcliente = NameXml;
            EnvioDte.Save(DTE_ENVIO_SEED);
            string NameXml2 = NameXml;
            firmarEnvioDTE(NameXml2, CN);
        }
        private void Envia_dte(string cn, string uri)
        {
            CrSeedService semilla = new CrSeedService();

            string respuesta = semilla.getSeed();
            //label1.Text = respuesta;
            //label1.Refresh();

            XmlDocument XMLdoc = new XmlDocument();
            XMLdoc.LoadXml(respuesta);
            XmlNodeList elemList = XMLdoc.GetElementsByTagName("SEMILLA");
            FirmarSeed(elemList[0].InnerXml, cn, uri, "93945000-9", "5816975-7");

        }
        private static string FirmarSeed(string seed, string cn, string ar, string a, string b)
        {
            string resultado = string.Empty;
           
            string body = string.Format("<getToken><item><Semilla>{0}</Semilla></item></getToken>", "0" + double.Parse(seed).ToString());


            X509Certificate2 certificado = obtenerCertificado(cn);
            try
            {
                resultado = firmarXMLSemilla(body, certificado, ar, a, b);
            }
            catch (System.Exception)
            {
                resultado = string.Empty;
            }
            return resultado;
        }
        public static string firmarXMLSemilla(string documento, X509Certificate2 certificado, string ar, string a, string b)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.LoadXml(documento);
            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = certificado.PrivateKey;
            Signature XMLSignature = signedXml.Signature;
            Reference reference = new Reference("");
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((System.Security.Cryptography.RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }
            System.Console.Write(doc.InnerXml);
            string signedSeed = doc.InnerXml;
            token.GetTokenFromSeedService gt = new token.GetTokenFromSeedService();
            string valorRespuesta = "";
            while (valorRespuesta.Length == 0)
            {
                valorRespuesta = gt.getToken(signedSeed);
            }
            
         

            //MessageBox.Show("valor respuesta " + valorRespuesta);

            XmlDocument doc2 = new XmlDocument();
            doc2.LoadXml(valorRespuesta);

            XmlNodeList elemList2 = doc2.GetElementsByTagName("TOKEN");
            string token = "";
            for (int i = 0; i < elemList2.Count; i++)
            {
                string token2 = elemList2[i].InnerXml;
                token = token2;
            }

            //MessageBox.Show("Token " + token);
            string rutEmisor = b.Replace("-", string.Empty);
            string rutEmpresa = a.Replace("-", string.Empty);
            string pRutEmisor = rutEmisor.Substring(0, rutEmisor.Length - 1);
            string pDigEmisor = rutEmisor.Substring(rutEmisor.Length - 1);
            string pRutEmpresa = rutEmpresa.Substring(0, rutEmpresa.Length - 1);
            string pDigEmpresa = rutEmpresa.Substring(rutEmpresa.Length - 1);
            string uri = System.Environment.CurrentDirectory;
            System.Text.StringBuilder secuencia = new System.Text.StringBuilder();
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"rutSender\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(pRutEmisor + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"dvSender\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(pDigEmisor + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"rutCompany\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(pRutEmpresa + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"dvCompany\"\r\n");
            secuencia.Append("\r\n");
            secuencia.Append(pDigEmpresa + "\r\n");
            secuencia.Append("--7d23e2a11301c4\r\n");
            secuencia.Append("Content-Disposition: form-data; name=\"archivo\"; filename=\"" + ar + "\"\r\n");
            secuencia.Append("Content-Type: text/xml\r\n");
            secuencia.Append("\r\n");
            XDocument xdocument = XDocument.Load(ar, System.Xml.Linq.LoadOptions.PreserveWhitespace);
            secuencia.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r");
            secuencia.Append(xdocument.ToString(SaveOptions.DisableFormatting));
            secuencia.Append("\r\n");
            secuencia.Append("--7d23e2a11301c4--\r\n");
            string pUrl = "https://palena.sii.cl/cgi_dte/UPL/DTEUpload";
            string pMethod = "POST";
            string pAccept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg,application/vnd.ms-powerpoint, application/ms-excel,application/msword, */*";
            string pReferer = "www.almadena.cl";
            string pToken = "TOKEN={0}";
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(pUrl);
            request.Method = pMethod;
            request.Accept = pAccept;
            request.Referer = pReferer;
            request.ContentType = "multipart/form-data: boundary=7d23e2a11301c4";
            request.ContentLength = (long)secuencia.Length;
            request.Headers.Add("Accept-Language", "es-cl");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Cookie", string.Format(pToken, token));
            request.UserAgent = "Mozilla/4.0 (compatible; PROG 1.0; Windows NT 5.0; YComp 5.0.2.4)";
            request.KeepAlive = true;
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(request.GetRequestStream(), System.Text.Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(secuencia.ToString());
                }
            }
            catch (System.Exception ex)
            {
            }
            try
            {
                string respuestaSii = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        respuestaSii = sr.ReadToEnd().Trim();

                        XmlDocument doc3 = new XmlDocument();
                        doc3.LoadXml(respuestaSii);
                        string minom = ar.Substring(ar.LastIndexOf("\\") + 1);
                        string nombreArchivos = @"X:\dte33\XMLRESPUESTA\" + minom;
                        doc3.Save(nombreArchivos);
                        Valida_respuesta_Sii(nombreArchivos);
                    }
                }
                if (string.IsNullOrEmpty(respuestaSii))
                {
                    throw new System.ArgumentNullException("Respuesta del SII es null");
                }
            }
            catch (System.Exception ex_4DC)
            {
            }
            doc.Save("supertoken.xml");
            return doc.InnerXml;
        }

        private static void Valida_respuesta_Sii(string nombreArchivos)
        {
          
           Estado="";
           TrackID="";
            string uriDTErepta= nombreArchivos;
            using (XmlReader readerRPTA = XmlReader.Create(uriDTErepta))
            {
                while (readerRPTA.Read())
                    if(readerRPTA.IsStartElement())
                    {
                        string text = readerRPTA.Name.ToString();
                        switch (text)
                        {
                            case "STATUS":
                                Estado = readerRPTA.ReadString();
                                break;
                            case "TRACKID":
                                TrackID = readerRPTA.ReadString();
                                break;
                        }
                        if (Estado.Trim().Length > 0 && TrackID.Trim().Length > 0)
                        {
                           //DTE33.Facturas.Actualiza_ctacte(Estado, TrackID);
                            Actualiza_CTA(Estado, TrackID);

                        }


                    }
            }
            if(Estado =="0")
            {

            }

        }

        private static void Actualiza_CTA(string Esta, string Track)
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();

            string StrOledbDBFIV;

            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad_C_leyton + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                StrOledbDBFIV = "update ctacte set ";
                StrOledbDBFIV += "status='" + Esta + "'";
                StrOledbDBFIV += ",trackid='" + Track + "'";
                StrOledbDBFIV += " wHERE NUMDOC=" + Folio;

                objconnDBFIV.Open();
                OldbComando = new OleDbCommand(StrOledbDBFIV, objconnDBFIV);
                //OldbComando.CommandText = StrOledbDBFIV;
                OldbComando.ExecuteNonQuery();
                objconnDBFIV.Close();
              //dgvFacturas.Rows[Fila].Cells[9].Value = Track;
            }


            catch (System.Exception ex)
            {

            }
           
           
        }

        public void Actualiza_ctacte(string Est, string Track)
        {
           
        }
        public static void firmarEnvioDTE(string uri, string CN)
        {
            X509Certificate2 certificado = obtenerCertificado(CN);
            XmlDocument DTE = new XmlDocument();
            DTE.PreserveWhitespace = true;
            DTE.Load(uri);
            SignedXml signedXml = new SignedXml(DTE);
            signedXml.SigningKey = certificado.PrivateKey;
            Signature XMLSignature = signedXml.Signature;
            Reference reference = new Reference();
            reference.Uri = "#SetDoc";
            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((System.Security.Cryptography.RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            DTE.DocumentElement.AppendChild(DTE.ImportNode(xmlDigitalSignature, true));
            // hacer copia del xml
            if (!File.Exists(uri))
            {
                DTE.Save(uri);
            }
            else
            {
                MessageBox.Show("xml ya existe : " + uri);
                string Bak=uri +"bak";
                DTE.Save(Bak);
                DTE.Save(uri);

            }
        }
        public static X509Certificate2 obtenerCertificado(string CN)
        {
            X509Certificate2 certificado = null;
            X509Certificate2 result;
            if (string.IsNullOrEmpty(CN) || CN.Length == 0)
            {
                result = certificado;
            }
            else
            {
                try
                {
                    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection Certificados = store.Certificates;
                    X509Certificate2Collection Certificados2 = Certificados.Find(X509FindType.FindByTimeValid, System.DateTime.Now, false);
                    X509Certificate2Collection Certificados3 = Certificados2.Find(X509FindType.FindBySubjectName, CN, false);
                    if (Certificados3 != null && Certificados3.Count != 0)
                    {
                        certificado = Certificados3[0];
                    }
                    store.Close();
                }
                catch (System.Exception)
                {
                    certificado = null;
                }
                result = certificado;
            }
            return result;
        }
        private void Imprime_PDF(string Folio,string DTE_ENVIO)
        {

            string carchivo = DTE_ENVIO;
            string tipo = "33";
            string cnumerox = Folio;
            string erut = "";
            string eimagen = "";
          

            erut = "93.945.000-9";
            eimagen = @"X:\dte33\logo.jpg";
        


            string[] Dia_nombre = new string[7] {"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"};
            int NumeroDia;
            string RUTRecep = "";
            string RznSocRecep = "";
            string GiroRecep = "";
            string DirRecep = "";
            string CmnaRecep = "";
            string CiudadRecep = "";
            string MntNeto = "";
            string IVA = "";
            string MntTotal = "";
            string FchEmis = "";
            string FmaPago = "";

            string Nrolinref = "";
            string TpoDocref ="";
            string FolioRef = "";
            string FchRef = "";

            string Nrolinref1 = "";
            string TpoDocref1 = "";
            string FolioRef1= "";
            string FchRef1 = "";

            string Nrolinref2 = "";
            string TpoDocref2 = "";
            string FolioRef2 = "";
            string FchRef2 = "";



            string[] cantidad = new string[21];
            string[] preciounitario = new string[21];
            string[] totalitem = new string[21];

            string[] operacion = new string[21];
            string[] saldo = new string[21];
            string[] basefac = new string[21];
            string[] pesos = new string[21];
            string[] fdesde = new string[21];
            string[] fhasta = new string[21];
            string[] detalle = new string[21];
            object[] oDetalle = new object[21];





            string[] sNrolinref = new string[3];
            string[] sTpoDocref = new string[3];
            string[] sFolioref = new string[3];
            string[] sFchref = new string[3];

            string sDetalle;
            string sPesos;
            string uriDTE = DTE_ENVIO;
            int i = 0;
            using (XmlReader reader3 = XmlReader.Create(uriDTE))
            {
                while (reader3.Read())
                {
                    if (reader3.IsStartElement())
                    {
                        string text = reader3.Name.ToString();
                        switch (text)
                        {
                            case "FmaPago":
                                FmaPago = reader3.ReadString();
                                break;
                            case "FchEmis":
                                FchEmis = reader3.ReadString();
                                break;
                            case "RUTRecep":
                                RUTRecep = reader3.ReadString();
                                break;
                            case "RznSocRecep":
                                RznSocRecep = reader3.ReadString();
                                break;
                            case "GiroRecep":
                                GiroRecep = reader3.ReadString();
                                break;
                            case "DirRecep":
                                DirRecep = reader3.ReadString();
                                break;
                            case "Contacto":
                                {
                                    string Contacto = reader3.ReadString();
                                    break;
                                }
                            case "CmnaRecep":
                                CmnaRecep = reader3.ReadString();
                                break;
                            case "CiudadRecep":
                                CiudadRecep = reader3.ReadString();
                                break;
                            case "MntNeto":
                                MntNeto = reader3.ReadString();
                                break;
                            case "MntExe":
                                {
                                    string MntExe = reader3.ReadString();
                                    break;
                                }
                            case "TasaIVA":
                                {
                                    string TasaIVA = reader3.ReadString();
                                    break;
                                }
                            case "IVA":
                                IVA = reader3.ReadString();
                                break;
                            case "MntTotal":
                                MntTotal = reader3.ReadString();
                                break;
                            case "VlrCodigo":
                                {
                                    string VlrCodigo = reader3.ReadString();

                                    break;
                                }


                            case "NmbItem":
                                {
                                    sDetalle = reader3.ReadString().Normalize();
                                    string VALE = sDetalle.Substring(0, 6);
                                    int ILargo = sDetalle.Length;
                                    double retNum;

                                    //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

                                    bool noNumerico = true;

                                    if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true )
                                    {
                                        if(Convert.ToDouble(VALE.ToString()) > 500000)
                                            {
                                            ILargo = 71;
                                            
                                            }


                                    }
                                    else
                                    {
                                        noNumerico = false;
                                        ILargo = 65;
                                        // no es numerico
                                    }

                                    if (ILargo > 71 )
                                    {
                                        if (noNumerico == true)
                                        {
                                            operacion[i] = sDetalle.Substring(0, 6).Trim().Replace(".", ",");
                                            saldo[i] = sDetalle.Substring(6, 15).Trim().Replace(".", ",");
                                            basefac[i] = sDetalle.Substring(22, 15).Trim().Replace(".", ",");
                                            sPesos = sDetalle.Substring(38, 16);
                                            sPesos.PadLeft(16);
                                            pesos[i] = sPesos.PadLeft(16);
                                            pesos[i].PadLeft(16);
                                            fdesde[i] = sDetalle.Substring(55, 9);
                                            fhasta[i] = sDetalle.Substring(64, 8);
                                        }
                                    }
                                    detalle[i] =sDetalle;
                                    oDetalle[i] = operacion[i] + saldo[i] + basefac[i] + pesos[i] + " " + fdesde[i] + fhasta[i];
                                    break;

                                }
                            case "QtyItem":
                                {
                                    string QtyItem = reader3.ReadString();
                                    cantidad[i] = QtyItem;
                                    break;
                                }
                            case "UnmdItem":
                                {
                                    string UnmdItem = reader3.ReadString();

                                    break;
                                }
                             case "PrcItem":
                                {
                                    preciounitario[i] = "0";
                                    string PrcItem = reader3.ReadString();
                                    if (PrcItem != "\n")
                                    {
                                        preciounitario[i] = PrcItem;
                                    }
                                    else
                                    {
                                        preciounitario[i] = "0";
                                    }
                                    break;
                                }
                            case "MontoItem":
                                {
                                    totalitem[i] = "0";
                                    string MontoItem = reader3.ReadString();
                                    if (MontoItem != "\n")
                                    {
                                        totalitem[i] = MontoItem;
                                    }
                                    else
                                    {
                                        totalitem[i] = "0";
                                    }
                                   
                                    i++;
                                    break;
                                }
                                // descuentos  por almacen
                                case "GlosaDR":
                                {
                                    sDetalle = reader3.ReadString().Normalize();
                                    
                                    detalle[i] = sDetalle;
                                    oDetalle[i] = sDetalle;
                                    break;
                                }

                                case "ValorDR":
                                {
                                    string MontoItem = reader3.ReadString();
                                    if (MontoItem.IndexOf("_") != -1)
                                    {
                                        totalitem[i] = "-" + MontoItem;
                                    }
                                    else
                                    {
                                        totalitem[i] =  MontoItem;
                                    }
                                    
                                    i++;
                                    break;
                                    
                                }
                               

                            case "NroLinRef":
                                {
                                   Nrolinref= reader3.ReadString();
                                   if(Nrolinref=="1")
                                   {
                                       Nrolinref1 = Nrolinref;
                                    }
                                   if (Nrolinref == "2")
                                   {
                                       Nrolinref2 = Nrolinref;
                                   }
                                

                                   break;
                                }
                            case "TpoDocRef":
                                {
                                   TpoDocref = reader3.ReadString();
                                   
                                   if (Nrolinref == "1")
                                   {
                                       TpoDocref1 = TpoDocref;
                                   }
                                   if (Nrolinref == "2")
                                   {
                                       TpoDocref2 = TpoDocref;
                                   }
                                    break;
                                }
                            case "FolioRef":
                                {
                                   FolioRef = reader3.ReadString();
                                   if (Nrolinref == "1")
                                   {
                                       FolioRef1 = FolioRef;
                                   }
                                   if (Nrolinref == "2")
                                   {
                                       FolioRef2 = FolioRef;
                                   }
                                    break;
                                }
                            case "FchRef":
                                {
                                   FchRef = reader3.ReadString();
                                   if (Nrolinref == "1")
                                   {
                                       FchRef1 = FchRef;
                                   }
                                   if (Nrolinref == "2")
                                   {
                                       FchRef2 = FchRef;
                                   }

                                    break;
                                }
                        }
                    }
                }
            }

            string telefono = "";
            DateTime Emision;
          
            Document doc = new Document(PageSize.LETTER);
            PdfWriter writer;
           

            if (chkPrefactura.Checked == false)
            {
                 writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[] { @"X:\dte33\PDF\pdf_", tipo, "DTE", cnumerox.Trim(), ".pdf" }), System.IO.FileMode.Create));
            }
            else
            {
                 writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[] { @"X:\dte33\PDF\PreFacturaPDF_", tipo, "DTE", cnumerox.Trim(), ".pdf" }), System.IO.FileMode.Create));
            }
            doc.AddTitle("Factura electrónica");
            doc.AddCreator("Ricardo Leppe");

            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0, BaseColor.BLACK);
            PdfContentByte cb = writer.DirectContent;

            // PAGINA 1

            cb.BeginText();

            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\Arialmt.ttf", "Cp1252", false);

            //cb.SetFontAndSize(f_cn, 8f);
            // logo de almadena
            cb.SetFontAndSize(f_cn, 10f);
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(eimagen);
            //jpg.ScaleToFit(100f, 100f);
            jpg.ScaleToFit(75f, 750f);
            jpg.SpacingBefore = 0f;
            jpg.SpacingAfter = 5f;
            jpg.Alignment = 0;
            doc.Add(jpg);
            cb.EndText();


            //marco rojo
            if (chkPrefactura.Checked == false)
            {
                cb.SetColorStroke(BaseColor.RED.Darker());
                cb.SetLineWidth(3);
                cb.Rectangle(390f, 680f, 190f, 90f);

                cb.Stroke();
                cb.SetLineWidth(1);
            }
            cb.BeginText();
            BaseFont bf_qty12345 = BaseFont.CreateFont("Times-Roman", "Cp1257", false);
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.CP1257, false);

            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(baseFont, 9f);


            cb.SetTextMatrix(110f, 750f);
            cb.ShowText("ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.");
            // razon social


            cb.SetTextMatrix(110f, 740f);
            cb.ShowText("Giro:ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES");
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(baseFont, 9f);
            cb.SetTextMatrix(120f, 710f);
            cb.ShowText("Casa Matriz:Moneda 812 Of.705,Santiago");
            cb.SetTextMatrix(120f, 700f);
            cb.ShowText("Fono:22347 6500");

            cb.SetTextMatrix(120f, 690f);
            cb.ShowText("Bodega:Camino Lo Sierra 04460 San Bernardo, Santiago");
            cb.SetTextMatrix(35f, 660f);
            cb.ShowText("www.almadena.cl - Email:almadena@almadena.cl");

            if (chkPrefactura.Checked == false)
            {
                iTextSharp.text.Font newFont = new iTextSharp.text.Font(baseFont, 16f, 0, iTextSharp.text.BaseColor.BLACK);

                cb.SetColorFill(BaseColor.RED.Darker());

                cb.SetFontAndSize(baseFont, 14f);
                cb.SetTextMatrix(420f, 750f);
                cb.ShowText("R.U.T.:" + erut);


                cb.SetTextMatrix(400f, 730f);
                cb.ShowText("FACTURA ELECTRÓNICA");

                cb.SetTextMatrix(460f, 700f);
                cb.ShowText("N°   " + cnumerox.Trim());

                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("S.I.I.- SANTIAGO CENTRO");

            }
            else
            {
                cb.SetFontAndSize(baseFont, 14f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("PRE FACTURA");


            }
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(baseFont, 10f);
            cb.SetTextMatrix(460f, 760f);
            cb.EndText();

            //marco encabezado

            cb.SetColorStroke(BaseColor.BLACK);
            cb.Rectangle(30f, 560f, 550f, 90f);
            cb.Stroke();

            cb.BeginText();

            // FECHA DE EMISION
            Emision = Convert.ToDateTime(FchEmis);
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("Es-Es");


            cb.SetTextMatrix(35f, 640f);
            cb.ShowText("Fecha Emisión");
            cb.SetTextMatrix(105F, 640f);
            cb.ShowText(":");

            //System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(;

            cb.SetTextMatrix(110f, 640f);
            NumeroDia=Convert.ToInt16(Emision.DayOfWeek);
            if (NumeroDia ==0)
            {
                NumeroDia=6;
            }
            else
            {
                NumeroDia=NumeroDia-1;

            }


            cb.ShowText(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Dia_nombre[NumeroDia].ToString()) + "  " + FchEmis.Substring(8, 2) + " de " + ci.DateTimeFormat.GetMonthName(Emision.Month).ToString() + " de " + FchEmis.Substring(0, 4));
            // FECHA DE EMISION


            iTextSharp.text.pdf.BaseFont Vn_Helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", iTextSharp.text.pdf.BaseFont.EMBEDDED);
            iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(Vn_Helvetica, 12, iTextSharp.text.Font.NORMAL);



            cb.SetFontAndSize(Vn_Helvetica, 10f);

            cb.SetTextMatrix(35f, 630f);
            cb.ShowText("Señor(es)");

            cb.SetTextMatrix(105f, 630f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 630f);
            cb.ShowText(RznSocRecep);

            cb.SetFontAndSize(baseFont, 10f);


                                         cb.SetTextMatrix(400, 640f);
                                        cb.ShowText("R.U.T.");

                                        cb.SetTextMatrix(490f, 640f);
                                        cb.ShowText(":");

                                        cb.SetTextMatrix(500, 640f);
                                        cb.ShowText(RUTRecep);

            cb.SetTextMatrix(35f, 620f);
            cb.ShowText("Giro");
            cb.SetTextMatrix(105f, 620f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 620f);
            cb.ShowText(GiroRecep);
         

            cb.SetTextMatrix(35f, 610f);
            cb.ShowText("Dirección");
            cb.SetTextMatrix(105f, 610f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 610f);
            cb.ShowText(DirRecep);

            cb.SetTextMatrix(35f, 600f);
            cb.ShowText("Comuna");
            cb.SetTextMatrix(105f, 600f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 600f);
            cb.ShowText(CmnaRecep);

            cb.SetTextMatrix(35f, 590f);
            cb.ShowText("Ciudad");
            cb.SetTextMatrix(105f, 590f);
            cb.ShowText(":");


            cb.SetTextMatrix(110f, 590f);
            cb.ShowText(CiudadRecep);

            //cb.SetTextMatrix(400f, 600f);
            //cb.ShowText("Orden");
            //cb.SetTextMatrix(490f, 600f);
            //cb.ShowText(":");

            cb.SetTextMatrix(35f, 580f);
            cb.ShowText("Fono");
            cb.SetTextMatrix(105f, 580f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 580f);
            cb.ShowText(telefono);
            //int pos = 40;
            // referencia....................................................
            if (FolioRef1.Length > 0)
            {
                cb.SetTextMatrix(35f, 570f);
                cb.ShowText("Referencia");
                cb.SetTextMatrix(105f, 570f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 570f);
                cb.ShowText(Nrolinref1 + "" + "ORDEN COMPRA " + " N° " + FolioRef1 + " del " + FchRef1);
            }
            if (FolioRef2.Length > 0)
            {
                cb.SetTextMatrix(350f, 570f);
                cb.ShowText(Nrolinref2 + " Atencion  N° " + FolioRef2 + " del " + FchRef2);
            }
            cb.EndText();
            cb.SetColorFill(BaseColor.GRAY.Darker());
            cb.SetColorStroke(BaseColor.BLACK);
            //marco titulo
            cb.Rectangle(30f, 530f, 550f, 20f);
            cb.Stroke();

            cb.BeginText();
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetTextMatrix(35f, 540f);
            cb.ShowText("Detalle");


            cb.SetTextMatrix(430, 540f);
            cb.ShowText("Cant.");

            cb.SetTextMatrix(460f, 540f);
            cb.ShowText("P.Unitario");

            cb.SetTextMatrix(530f, 540f);
            cb.ShowText("Total item");



            cb.EndText();
            //marco detalle
            cb.SetColorStroke(new CMYKColor(0f, 12f, 20f, 84f));
            cb.Rectangle(30f, 310f, 550f, 220f);
            cb.Stroke();
            cb.BeginText();
            int Y = 21;
            double dValor;

            for (int di = 0; di < 21; di++)
            {
                if (detalle[di] != null)
                {
                    string VALE = detalle[di].Substring(0, 6);
                    int ILargo = detalle[di].Length;
                    double retNum;

                    //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);


                   
                    if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true)
                    {
                        if (Convert.ToDouble(VALE.ToString()) > 500000)
                        {
                            ILargo = 71;
                            
                        }
                        

                    }
                    else
                        {
                          
                          ILargo = 65;
                        }
                        

                    if (ILargo > 71)
                    {
                        
                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        cb.ShowText(operacion[di]);

                        dValor = System.Convert.ToDouble(saldo[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 160f, (float)(540 - Y), 0);

                        dValor = System.Convert.ToDouble(basefac[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 260f, (float)(540 - Y), 0);
                   
                        dValor = System.Convert.ToDouble(pesos[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 340f, (float)(540 - Y), 0);
                       
                        cb.SetTextMatrix(345f, (float)(540 - Y));
                        cb.ShowText(fdesde[di]);

                        cb.SetTextMatrix(390f, (float)(540 - Y));
                        cb.ShowText(fhasta[di]);
                    }
                    else
                    {
                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        int xLargo = detalle[di].Length;
                        if (xLargo < ILargo)
                        {
                            ILargo = xLargo;
                        }
                        cb.ShowText(detalle[di].Substring(0,ILargo));

                    }

                    if (detalle[di] != "DESCUENTO ARRIENDO ALMACEN")
                    {
                        cb.SetTextMatrix(430f, (float)(540 - Y));
                        dValor = System.Convert.ToDouble(cantidad[di]);
                        if (preciounitario[di] != "0")
                            {
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 450f, (float)(540 - Y), 0);
                            dValor = System.Convert.ToDouble(preciounitario[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 510f, (float)(540 - Y), 0);
                        }
                    }
                    if (preciounitario[di] != "0")
                    {
                        dValor = System.Convert.ToDouble(totalitem[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);
                    }

                    Y += 10;
                }
            }

            // aqui va el timbre electronicodel TED

            iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"X:\dte33\TIMBRE\" + Folio + ".Png");

            imagen2.SetAbsolutePosition(60f, 135f);
            imagen2.BorderWidth = 0f;
            imagen2.ScaleToFit(200f, 230f);
            doc.Add(imagen2);
            cb.EndText();

            cb.BeginText();

            cb.SetFontAndSize(baseFont, 9f);
            cb.SetTextMatrix(120f, 120f);
            cb.ShowText("Timbre Electrónico SII");

            cb.SetTextMatrix(120f, 110f);
            cb.ShowText("Resolución 80 de 2014");

            cb.SetTextMatrix(100f, 100f);
            cb.ShowText("Verifique documento:www.sii.cl");

            // pagina 1

            cb.SetFontAndSize(baseFont, 10f);


            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Neto $", 500f, 225F, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Iva(19%) $", 500f, 215F, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total $", 500f, 205F, 0);

            dValor = System.Convert.ToInt32(MntNeto);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 225F, 0);

            dValor = System.Convert.ToInt32(IVA);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 215F, 0);


            dValor = System.Convert.ToInt32(MntTotal);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 205F, 0);

            cb.EndText();
            if (chkPrefactura.Checked == false)
            {
               
                doc.NewPage();
                // aqui va la tercera y ultima pagina carretera
                //
                //
                //
                //---------------------------------------------------------------------------
                cb.BeginText();



                //cb.SetFontAndSize(f_cn, 8f);
                // logo de almadena
                cb.SetFontAndSize(f_cn, 10f);

                //jpg.ScaleToFit(100f, 100f);
                //jpg.ScaleToFit(80f, 80f);
                //jpg.SpacingBefore = 5f;
                //jpg.SpacingAfter = 10f;
                //jpg.Alignment = 0;
                doc.Add(jpg);
                cb.EndText();

                if (chkPrefactura.Checked == false)
                {
                    //marco rojo
                    cb.SetColorStroke(BaseColor.RED.Darker());
                    cb.SetLineWidth(2);
                    cb.Rectangle(390f, 680f, 190f, 90f);

                    cb.Stroke();
                    cb.SetLineWidth(1);

                }

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 9f);


                cb.SetTextMatrix(110f, 750f);
                cb.ShowText("ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.");
                // razon social


                cb.SetTextMatrix(110f, 740f);
                cb.ShowText("Giro:ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES");
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 9f);
                cb.SetTextMatrix(120f, 710f);
                cb.ShowText("Casa Matriz:Moneda 812 Of.705,Santiago");
                cb.SetTextMatrix(120f, 700f);
                cb.ShowText("Fono:22347 6500");

                cb.SetTextMatrix(120f, 690f);
                cb.ShowText("Bodega:Camino Lo Sierra 04460 San Bernardo, Santiago");
                cb.SetTextMatrix(120f, 680f);
                cb.ShowText("Fono:232634455");
                cb.SetTextMatrix(35f, 660f);
                cb.ShowText("www.almadena.cl - Email:almadena@almadena.cl");
               
                if (chkPrefactura.Checked == false)
                {
                    cb.SetColorFill(BaseColor.RED.Darker());

                    cb.SetFontAndSize(baseFont, 14f);
                    cb.SetTextMatrix(420f, 750f);
                    cb.ShowText("R.U.T.:" + erut);


                    cb.SetTextMatrix(400f, 730f);
                    cb.ShowText("FACTURA ELECTRÓNICA");

                    cb.SetTextMatrix(450f, 720f);
                    cb.ShowText("");

                    cb.SetTextMatrix(460f, 700f);
                    cb.ShowText("N°   " + cnumerox.Trim());

                    cb.SetFontAndSize(baseFont, 10f);
                    cb.SetTextMatrix(420f, 665f);
                    cb.ShowText("S.I.I.- SANTIAGO CENTRO");
                }
                else
                {
                    cb.SetFontAndSize(baseFont, 14f);
                    cb.SetTextMatrix(420f, 665f);
                    cb.ShowText("PRE FACTURA");


                }

                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(460f, 760f);
                cb.EndText();

                //marco encabezado

                cb.SetColorStroke(BaseColor.BLACK);
                cb.Rectangle(30f, 560f, 550f, 90f);
                cb.Stroke();

                cb.BeginText();
                // FECHA DE EMISION
                Emision = Convert.ToDateTime(FchEmis);
                //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("Es-Es");


                cb.SetTextMatrix(35f, 640f);
                cb.ShowText("Fecha Emisión");
                cb.SetTextMatrix(105F, 640f);
                cb.ShowText(":");

                //System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(;

                cb.SetTextMatrix(110f, 640f);
                NumeroDia = Convert.ToInt16(Emision.DayOfWeek);
                if (NumeroDia == 0)
                {
                    NumeroDia = 6;
                }
                else
                {
                    NumeroDia = NumeroDia - 1;

                }


                cb.ShowText(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Dia_nombre[NumeroDia].ToString()) + "  " + FchEmis.Substring(8, 2) + " de " + ci.DateTimeFormat.GetMonthName(Emision.Month).ToString() + " de " + FchEmis.Substring(0, 4));
                // FECHA DE EMISION
                // FECHA DE EMISION


                //iTextSharp.text.pdf.BaseFont Vn_Helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", iTextSharp.text.pdf.BaseFont.EMBEDDED);
                //iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(Vn_Helvetica, 12, iTextSharp.text.Font.NORMAL);



                cb.SetFontAndSize(Vn_Helvetica, 10f);

                cb.SetTextMatrix(35f, 630f);
                cb.ShowText("Señor(es)");

                cb.SetTextMatrix(105f, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 630f);
                cb.ShowText(RznSocRecep);

                cb.SetFontAndSize(baseFont, 10f);


                cb.SetTextMatrix(400, 640f);
                cb.ShowText("R.U.T.");

                cb.SetTextMatrix(490f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(500, 640f);
                cb.ShowText(RUTRecep);

                cb.SetTextMatrix(35f, 620f);
                cb.ShowText("Giro");
                cb.SetTextMatrix(105f, 620f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 620f);
                cb.ShowText(GiroRecep);


                cb.SetTextMatrix(35f, 610f);
                cb.ShowText("Dirección");
                cb.SetTextMatrix(105f, 610f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 610f);
                cb.ShowText(DirRecep);

                cb.SetTextMatrix(35f, 600f);
                cb.ShowText("Comuna");
                cb.SetTextMatrix(105f, 600f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 600f);
                cb.ShowText(CmnaRecep);

                cb.SetTextMatrix(35f, 590f);
                cb.ShowText("Ciudad");
                cb.SetTextMatrix(105f, 590f);
                cb.ShowText(":");


                cb.SetTextMatrix(110f, 590f);
                cb.ShowText(CiudadRecep);

                //cb.SetTextMatrix(400f, 600f);
                //cb.ShowText("Orden");
                //cb.SetTextMatrix(490f, 600f);
                //cb.ShowText(":");

                cb.SetTextMatrix(35f, 580f);
                cb.ShowText("Fono");
                cb.SetTextMatrix(105f, 580f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 580f);
                cb.ShowText(telefono);
                if (FolioRef1.Length > 0)
                {
                    cb.SetTextMatrix(35f, 570f);
                    cb.ShowText("Referencia");
                    cb.SetTextMatrix(105f, 570f);
                    cb.ShowText(":");

                    cb.SetTextMatrix(110f, 570f);
                    cb.ShowText(Nrolinref1 + "" + "ORDEN COMPRA " + " N° " + FolioRef1 + " del " + FchRef1);
                }
                if (FolioRef2.Length > 0)
                {
                    cb.SetTextMatrix(350f, 570f);
                    cb.ShowText(Nrolinref2 + " Atencion  N° " + FolioRef2 + " del " + FchRef2);
                }

                cb.EndText();
                cb.SetColorFill(BaseColor.GRAY.Darker());
                cb.SetColorStroke(BaseColor.BLACK);
                //marco titulo
                cb.Rectangle(30f, 530f, 550f, 20f);
                cb.Stroke();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetTextMatrix(35f, 540f);
                cb.ShowText("Detalle");


                cb.SetTextMatrix(410, 540f);
                cb.ShowText("Cant.");

                cb.SetTextMatrix(460f, 540f);
                cb.ShowText("P.Unitario");

                cb.SetTextMatrix(530f, 540f);
                cb.ShowText("Total item");



                cb.EndText();
                //marco detalle
                cb.SetColorStroke(new CMYKColor(0f, 12f, 20f, 84f));
                cb.Rectangle(30f, 310f, 550f, 220f);
                cb.Stroke();
                cb.BeginText();
                Y = 21;
                dValor = 0;

                for (int di = 0; di < 21; di++)
                {
                    if (detalle[di] != null)
                    {
                        string VALE = detalle[di].Substring(0, 6);
                        int ILargo = detalle[di].Length;
                        double retNum;

                        //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);



                        if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true)
                        {
                            if (Convert.ToDouble(VALE.ToString()) > 500000)
                            {
                                ILargo = 71;
                            }


                        }
                        else
                        {

                            ILargo = 65;
                        }


                        if (ILargo > 71)
                        {

                            cb.SetTextMatrix(35f, (float)(540 - Y));
                            cb.ShowText(operacion[di]);

                            dValor = System.Convert.ToDouble(saldo[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 160f, (float)(540 - Y), 0);

                            dValor = System.Convert.ToDouble(basefac[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 260f, (float)(540 - Y), 0);

                            dValor = System.Convert.ToDouble(pesos[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 340f, (float)(540 - Y), 0);

                            cb.SetTextMatrix(345f, (float)(540 - Y));
                            cb.ShowText(fdesde[di]);

                            cb.SetTextMatrix(390f, (float)(540 - Y));
                            cb.ShowText(fhasta[di]);
                        }
                        else
                        {
                            cb.SetTextMatrix(35f, (float)(540 - Y));
                            int xLargo = detalle[di].Length;
                            if (xLargo < ILargo)
                            {
                                ILargo = xLargo;
                            }
                            cb.ShowText(detalle[di].Substring(0, ILargo));

                        }

                        if (detalle[di] != "DESCUENTO ARRIENDO ALMACEN")
                        {
                            cb.SetTextMatrix(430f, (float)(540 - Y));
                            dValor = System.Convert.ToDouble(cantidad[di]);
                            if (preciounitario[di] != "0")
                            {
                                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 450f, (float)(540 - Y), 0);
                                dValor = System.Convert.ToDouble(preciounitario[di]);
                                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 510f, (float)(540 - Y), 0);
                            }
                        }
                        if (preciounitario[di] != "0")
                        {
                            dValor = System.Convert.ToDouble(totalitem[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);
                        }

                        Y += 10;
                    }
                }

                // aqui va el timbre electronicodel TED

                //iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"X:\dte52\TIMBRE\" + Folio + ".Png");

                imagen2.SetAbsolutePosition(60f, 135f);
                imagen2.BorderWidth = 0f;
                imagen2.ScaleToFit(200f, 230f);
                doc.Add(imagen2);
                cb.EndText();

                cb.SetColorStroke(BaseColor.BLACK);
                //cb.Rectangle(275f, 65f, 150f, 180f);
                cb.Rectangle(275f, 130f, 150f, 110f);
                //cb.Rectangle(15f, 30f, 590f, 170f);
                cb.Stroke();

                cb.BeginText();

                cb.SetFontAndSize(baseFont, 9f);
                cb.SetTextMatrix(120f, 120f);
                cb.ShowText("Timbre Electrónico SII");

                cb.SetTextMatrix(120f, 110f);
                cb.ShowText("Resolución 80 de 2014");

                cb.SetTextMatrix(100f, 100f);
                cb.ShowText("Verifique documento:www.sii.cl");



                //cb.SetFontAndSize(baseFont, 8f);
                cb.SetFontAndSize(Vn_Helvetica, 8f);
                cb.SetTextMatrix(310f, 220f);
                cb.ShowText("Acuse de recibo");

                cb.SetTextMatrix(280f, 200f);
                cb.ShowText("Nombre _____________________");

                cb.SetTextMatrix(280f, 185f);
                cb.ShowText("Rut   ______________________");

                cb.SetTextMatrix(280f, 170f);
                cb.ShowText("Fecha ______________________");



                cb.SetTextMatrix(280f, 155f);
                cb.ShowText("Recinto_____________________");

                cb.SetTextMatrix(280f, 140f);
                cb.ShowText("Firma ______________________");



                cb.SetTextMatrix(280f, 120f);
                //cb.ShowTextAligned(PdfContentByte..ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);
                cb.ShowText("El  acuse de recibo que  se  declara  en");

                cb.SetTextMatrix(280f, 110f);
                cb.ShowText("este acto, de acuerdo a lo dispuesto en");


                cb.SetTextMatrix(280f, 100f);
                //cb.ShowTextAligned()
                cb.ShowText("la letra b) del art. 4°, y la letra c) del art.");

                cb.SetTextMatrix(280f, 90f);
                cb.ShowText("5°  de  la  ley  19.983,  acredita  que  la ");


                cb.SetTextMatrix(280f, 80f);
                cb.ShowText("entrega de mercaderías o servicio (s) ");


                cb.SetTextMatrix(280f, 70f);
                cb.ShowText("prestado(s) ha(n) sido recibido(s).");

                cb.SetFontAndSize(baseFont, 11f);
                cb.SetTextMatrix(430f, 70f);
                cb.ShowText("CEDIBLE");

                cb.EndText();


                cb.BeginText();
                cb.SetFontAndSize(baseFont, 10f);


                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Neto $", 500f, 225F, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Iva(19%) $", 500f, 215F, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total $", 500f, 205F, 0);

                dValor = System.Convert.ToInt32(MntNeto);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 225F, 0);

                dValor = System.Convert.ToInt32(IVA);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 215F, 0);


                dValor = System.Convert.ToInt32(MntTotal);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 205F, 0);

                cb.EndText();
            }
                doc.Close();
                writer.Close();
                string pdfPath;
                if (chkPrefactura.Checked == false)
                {
                    pdfPath = string.Concat(new string[] { @"X:\dte33\PDF\pdf_", tipo, "DTE", cnumerox.Trim(), ".pdf" });
                    if (TrackID.Length > 0)
                    {
                        actuliza_cliente(pdfPath, cnumerox, DTE_ENVIO, MntNeto, IVA, MntTotal);
                    }
                    else
                    {
                        MessageBox.Show("no hay respuesta track id");
                    }
                }
             

        }
        private void Imprime_PDF_COPIA(string Folio, string DTE_ENVIO)
        {

            string carchivo = DTE_ENVIO;
            string tipo = "33";
            string cnumerox = Folio;
            string erut = "";
            string eimagen = "";


            erut = "93.945.000-9";
            eimagen = @"X:\dte33\logo.jpg";



            string[] Dia_nombre = new string[7] { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };
            int NumeroDia;
            string RUTRecep = "";
            string RznSocRecep = "";
            string GiroRecep = "";
            string DirRecep = "";
            string CmnaRecep = "";
            string CiudadRecep = "";
            string MntNeto = "";
            string IVA = "";
            string MntTotal = "";
            string FchEmis = "";
            string FmaPago = "";

            string Nrolinref = "";
            string TpoDocref = "";
            string FolioRef = "";
            string FchRef = "";

            string Nrolinref1 = "";
            string TpoDocref1 = "";
            string FolioRef1 = "";
            string FchRef1 = "";

            string Nrolinref2 = "";
            string TpoDocref2 = "";
            string FolioRef2 = "";
            string FchRef2 = "";



            string[] cantidad = new string[21];
            string[] preciounitario = new string[21];
            string[] totalitem = new string[21];

            string[] operacion = new string[21];
            string[] saldo = new string[21];
            string[] basefac = new string[21];
            string[] pesos = new string[21];
            string[] fdesde = new string[21];
            string[] fhasta = new string[21];
            string[] detalle = new string[21];
            object[] oDetalle = new object[21];





            string[] sNrolinref = new string[3];
            string[] sTpoDocref = new string[3];
            string[] sFolioref = new string[3];
            string[] sFchref = new string[3];

            string sDetalle;
            string sPesos;
            string uriDTE = DTE_ENVIO;
            int i = 0;
            using (XmlReader reader3 = XmlReader.Create(uriDTE))
            {
                while (reader3.Read())
                {
                    if (reader3.IsStartElement())
                    {
                        string text = reader3.Name.ToString();
                        switch (text)
                        {
                            case "FmaPago":
                                FmaPago = reader3.ReadString();
                                break;
                            case "FchEmis":
                                FchEmis = reader3.ReadString();
                                break;
                            case "RUTRecep":
                                RUTRecep = reader3.ReadString();
                                break;
                            case "RznSocRecep":
                                RznSocRecep = reader3.ReadString();
                                break;
                            case "GiroRecep":
                                GiroRecep = reader3.ReadString();
                                break;
                            case "DirRecep":
                                DirRecep = reader3.ReadString();
                                break;
                            case "Contacto":
                                {
                                    string Contacto = reader3.ReadString();
                                    break;
                                }
                            case "CmnaRecep":
                                CmnaRecep = reader3.ReadString();
                                break;
                            case "CiudadRecep":
                                CiudadRecep = reader3.ReadString();
                                break;
                            case "MntNeto":
                                MntNeto = reader3.ReadString();
                                break;
                            case "MntExe":
                                {
                                    string MntExe = reader3.ReadString();
                                    break;
                                }
                            case "TasaIVA":
                                {
                                    string TasaIVA = reader3.ReadString();
                                    break;
                                }
                            case "IVA":
                                IVA = reader3.ReadString();
                                break;
                            case "MntTotal":
                                MntTotal = reader3.ReadString();
                                break;
                            case "VlrCodigo":
                                {
                                    string VlrCodigo = reader3.ReadString();

                                    break;
                                }


                            case "NmbItem":
                                {
                                    sDetalle = reader3.ReadString().Normalize();
                                    string VALE = sDetalle.Substring(0, 6);
                                    int ILargo = sDetalle.Length;
                                    double retNum;

                                    //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

                                    bool noNumerico = true;

                                    if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true)
                                    {
                                        if (Convert.ToDouble(VALE.ToString()) > 500000)
                                        {
                                            ILargo = 71;

                                        }


                                    }
                                    else
                                    {
                                        noNumerico = false;
                                        ILargo = 65;
                                        // no es numerico
                                    }

                                    if (ILargo > 71)
                                    {
                                        if (noNumerico == true)
                                        {
                                            operacion[i] = sDetalle.Substring(0, 6).Trim().Replace(".", ",");
                                            saldo[i] = sDetalle.Substring(6, 15).Trim().Replace(".", ",");
                                            basefac[i] = sDetalle.Substring(22, 15).Trim().Replace(".", ",");
                                            sPesos = sDetalle.Substring(38, 16);
                                            sPesos.PadLeft(16);
                                            pesos[i] = sPesos.PadLeft(16);
                                            pesos[i].PadLeft(16);
                                            fdesde[i] = sDetalle.Substring(55, 9);
                                            fhasta[i] = sDetalle.Substring(64, 8);
                                        }
                                    }
                                    detalle[i] = sDetalle;
                                    oDetalle[i] = operacion[i] + saldo[i] + basefac[i] + pesos[i] + " " + fdesde[i] + fhasta[i];
                                    break;

                                }
                            case "QtyItem":
                                {
                                    string QtyItem = reader3.ReadString();
                                    cantidad[i] = QtyItem;
                                    break;
                                }
                            case "UnmdItem":
                                {
                                    string UnmdItem = reader3.ReadString();

                                    break;
                                }
                            case "PrcItem":
                                {
                                    preciounitario[i] = "0";
                                    string PrcItem = reader3.ReadString();
                                    if (PrcItem != "\n")
                                    {
                                        preciounitario[i] = PrcItem;
                                    }
                                    else
                                    {
                                        preciounitario[i] = "0";
                                    }
                                    break;
                                }
                            case "MontoItem":
                                {
                                    totalitem[i] = "0";
                                    string MontoItem = reader3.ReadString();
                                    if (MontoItem != "\n")
                                    {
                                        totalitem[i] = MontoItem;
                                    }
                                    else
                                    {
                                        totalitem[i] = "0";
                                    }

                                    i++;
                                    break;
                                }
                            // descuentos  por almacen
                            case "GlosaDR":
                                {
                                    sDetalle = reader3.ReadString().Normalize();

                                    detalle[i] = sDetalle;
                                    oDetalle[i] = sDetalle;
                                    break;
                                }

                            case "ValorDR":
                                {
                                    string MontoItem = reader3.ReadString();
                                    if (MontoItem.IndexOf("_") != -1)
                                    {
                                        totalitem[i] = "-" + MontoItem;
                                    }
                                    else
                                    {
                                        totalitem[i] = MontoItem;
                                    }

                                    i++;
                                    break;

                                }


                            case "NroLinRef":
                                {
                                    Nrolinref = reader3.ReadString();
                                    if (Nrolinref == "1")
                                    {
                                        Nrolinref1 = Nrolinref;
                                    }
                                    if (Nrolinref == "2")
                                    {
                                        Nrolinref2 = Nrolinref;
                                    }


                                    break;
                                }
                            case "TpoDocRef":
                                {
                                    TpoDocref = reader3.ReadString();

                                    if (Nrolinref == "1")
                                    {
                                        TpoDocref1 = TpoDocref;
                                    }
                                    if (Nrolinref == "2")
                                    {
                                        TpoDocref2 = TpoDocref;
                                    }
                                    break;
                                }
                            case "FolioRef":
                                {
                                    FolioRef = reader3.ReadString();
                                    if (Nrolinref == "1")
                                    {
                                        FolioRef1 = FolioRef;
                                    }
                                    if (Nrolinref == "2")
                                    {
                                        FolioRef2 = FolioRef;
                                    }
                                    break;
                                }
                            case "FchRef":
                                {
                                    FchRef = reader3.ReadString();
                                    if (Nrolinref == "1")
                                    {
                                        FchRef1 = FchRef;
                                    }
                                    if (Nrolinref == "2")
                                    {
                                        FchRef2 = FchRef;
                                    }

                                    break;
                                }
                        }
                    }
                }
            }

            string telefono = "";
            DateTime Emision;

            Document doc = new Document(PageSize.LETTER);
            PdfWriter writer;


            if (chkPrefactura.Checked == false)
            {
                writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[] { @"X:\dte33\PDF\pdf_", tipo, "COPIA", cnumerox.Trim(), ".pdf" }), System.IO.FileMode.Create));
            }
            else
            {
                writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[] { @"X:\dte33\PDF\PreFacturaPDF_", tipo, "DTE", cnumerox.Trim(), ".pdf" }), System.IO.FileMode.Create));
            }
            doc.AddTitle("Factura electrónica");
            doc.AddCreator("Ricardo Leppe");

            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0, BaseColor.BLACK);
            PdfContentByte cb = writer.DirectContent;

            // PAGINA 1

            cb.BeginText();

            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\Arialmt.ttf", "Cp1252", false);

            //cb.SetFontAndSize(f_cn, 8f);
            // logo de almadena
            cb.SetFontAndSize(f_cn, 10f);
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(eimagen);
            //jpg.ScaleToFit(100f, 100f);
            jpg.ScaleToFit(75f, 750f);
            jpg.SpacingBefore = 0f;
            jpg.SpacingAfter = 5f;
            jpg.Alignment = 0;
            doc.Add(jpg);
            cb.EndText();


            //marco rojo
            if (chkPrefactura.Checked == false)
            {
                cb.SetColorStroke(BaseColor.RED.Darker());
                cb.SetLineWidth(3);
                cb.Rectangle(390f, 680f, 190f, 90f);

                cb.Stroke();
                cb.SetLineWidth(1);
            }
            cb.BeginText();
            BaseFont bf_qty12345 = BaseFont.CreateFont("Times-Roman", "Cp1257", false);
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.CP1257, false);

            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(baseFont, 9f);


            cb.SetTextMatrix(110f, 750f);
            cb.ShowText("ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.");
            // razon social


            cb.SetTextMatrix(110f, 740f);
            cb.ShowText("Giro:ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES");
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(baseFont, 9f);
            cb.SetTextMatrix(120f, 710f);
            cb.ShowText("Casa Matriz:Moneda 812 Of.705,Santiago");
            cb.SetTextMatrix(120f, 700f);
            cb.ShowText("Fono:22347 6500");

            cb.SetTextMatrix(120f, 690f);
            cb.ShowText("Bodega:Camino Lo Sierra 04460 San Bernardo, Santiago");
            cb.SetTextMatrix(35f, 660f);
            cb.ShowText("www.almadena.cl - Email:almadena@almadena.cl");

            if (chkPrefactura.Checked == false)
            {
                iTextSharp.text.Font newFont = new iTextSharp.text.Font(baseFont, 16f, 0, iTextSharp.text.BaseColor.BLACK);

                cb.SetColorFill(BaseColor.RED.Darker());

                cb.SetFontAndSize(baseFont, 14f);
                cb.SetTextMatrix(420f, 750f);
                cb.ShowText("R.U.T.:" + erut);


                cb.SetTextMatrix(400f, 730f);
                cb.ShowText("FACTURA ELECTRÓNICA");

                cb.SetTextMatrix(460f, 700f);
                cb.ShowText("N°   " + cnumerox.Trim());

                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("S.I.I.- SANTIAGO CENTRO");

            }
            else
            {
                cb.SetFontAndSize(baseFont, 14f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("PRE FACTURA");


            }
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(baseFont, 10f);
            cb.SetTextMatrix(460f, 760f);
            cb.EndText();

            //marco encabezado

            cb.SetColorStroke(BaseColor.BLACK);
            cb.Rectangle(30f, 560f, 550f, 90f);
            cb.Stroke();

            cb.BeginText();

            // FECHA DE EMISION
            Emision = Convert.ToDateTime(FchEmis);
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("Es-Es");


            cb.SetTextMatrix(35f, 640f);
            cb.ShowText("Fecha Emisión");
            cb.SetTextMatrix(105F, 640f);
            cb.ShowText(":");

            //System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(;

            cb.SetTextMatrix(110f, 640f);
            NumeroDia = Convert.ToInt16(Emision.DayOfWeek);
            if (NumeroDia == 0)
            {
                NumeroDia = 6;
            }
            else
            {
                NumeroDia = NumeroDia - 1;

            }


            cb.ShowText(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Dia_nombre[NumeroDia].ToString()) + "  " + FchEmis.Substring(8, 2) + " de " + ci.DateTimeFormat.GetMonthName(Emision.Month).ToString() + " de " + FchEmis.Substring(0, 4));
            // FECHA DE EMISION


            iTextSharp.text.pdf.BaseFont Vn_Helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", iTextSharp.text.pdf.BaseFont.EMBEDDED);
            iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(Vn_Helvetica, 12, iTextSharp.text.Font.NORMAL);



            cb.SetFontAndSize(Vn_Helvetica, 10f);

            cb.SetTextMatrix(35f, 630f);
            cb.ShowText("Señor(es)");

            cb.SetTextMatrix(105f, 630f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 630f);
            cb.ShowText(RznSocRecep);

            cb.SetFontAndSize(baseFont, 10f);


            cb.SetTextMatrix(400, 640f);
            cb.ShowText("R.U.T.");

            cb.SetTextMatrix(490f, 640f);
            cb.ShowText(":");

            cb.SetTextMatrix(500, 640f);
            cb.ShowText(RUTRecep);

            cb.SetTextMatrix(35f, 620f);
            cb.ShowText("Giro");
            cb.SetTextMatrix(105f, 620f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 620f);
            cb.ShowText(GiroRecep);


            cb.SetTextMatrix(35f, 610f);
            cb.ShowText("Dirección");
            cb.SetTextMatrix(105f, 610f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 610f);
            cb.ShowText(DirRecep);

            cb.SetTextMatrix(35f, 600f);
            cb.ShowText("Comuna");
            cb.SetTextMatrix(105f, 600f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 600f);
            cb.ShowText(CmnaRecep);

            cb.SetTextMatrix(35f, 590f);
            cb.ShowText("Ciudad");
            cb.SetTextMatrix(105f, 590f);
            cb.ShowText(":");


            cb.SetTextMatrix(110f, 590f);
            cb.ShowText(CiudadRecep);

            //cb.SetTextMatrix(400f, 600f);
            //cb.ShowText("Orden");
            //cb.SetTextMatrix(490f, 600f);
            //cb.ShowText(":");

            cb.SetTextMatrix(35f, 580f);
            cb.ShowText("Fono");
            cb.SetTextMatrix(105f, 580f);
            cb.ShowText(":");

            cb.SetTextMatrix(110f, 580f);
            cb.ShowText(telefono);
            //int pos = 40;
            // referencia....................................................
            if (FolioRef1.Length > 0)
            {
                cb.SetTextMatrix(35f, 570f);
                cb.ShowText("Referencia");
                cb.SetTextMatrix(105f, 570f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 570f);
                cb.ShowText(Nrolinref1 + "" + "ORDEN COMPRA " + " N° " + FolioRef1 + " del " + FchRef1);
            }
            if (FolioRef2.Length > 0)
            {
                cb.SetTextMatrix(350f, 570f);
                cb.ShowText(Nrolinref2 + " Atencion  N° " + FolioRef2 + " del " + FchRef2);
            }
            cb.EndText();
            cb.SetColorFill(BaseColor.GRAY.Darker());
            cb.SetColorStroke(BaseColor.BLACK);
            //marco titulo
            cb.Rectangle(30f, 530f, 550f, 20f);
            cb.Stroke();

            cb.BeginText();
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetTextMatrix(35f, 540f);
            cb.ShowText("Detalle");


            cb.SetTextMatrix(430, 540f);
            cb.ShowText("Cant.");

            cb.SetTextMatrix(460f, 540f);
            cb.ShowText("P.Unitario");

            cb.SetTextMatrix(530f, 540f);
            cb.ShowText("Total item");



            cb.EndText();
            //marco detalle
            cb.SetColorStroke(new CMYKColor(0f, 12f, 20f, 84f));
            cb.Rectangle(30f, 310f, 550f, 220f);
            cb.Stroke();
            cb.BeginText();
            int Y = 21;
            double dValor;

            for (int di = 0; di < 21; di++)
            {
                if (detalle[di] != null)
                {
                    string VALE = detalle[di].Substring(0, 6);
                    int ILargo = detalle[di].Length;
                    double retNum;

                    //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);



                    if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true)
                    {
                        if (Convert.ToDouble(VALE.ToString()) > 500000)
                        {
                            ILargo = 71;

                        }


                    }
                    else
                    {

                        ILargo = 65;
                    }


                    if (ILargo > 71)
                    {

                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        cb.ShowText(operacion[di]);

                        dValor = System.Convert.ToDouble(saldo[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 160f, (float)(540 - Y), 0);

                        dValor = System.Convert.ToDouble(basefac[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 260f, (float)(540 - Y), 0);

                        dValor = System.Convert.ToDouble(pesos[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 340f, (float)(540 - Y), 0);

                        cb.SetTextMatrix(345f, (float)(540 - Y));
                        cb.ShowText(fdesde[di]);

                        cb.SetTextMatrix(390f, (float)(540 - Y));
                        cb.ShowText(fhasta[di]);
                    }
                    else
                    {
                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        int xLargo = detalle[di].Length;
                        if (xLargo < ILargo)
                        {
                            ILargo = xLargo;
                        }
                        cb.ShowText(detalle[di].Substring(0, ILargo));

                    }

                    if (detalle[di] != "DESCUENTO ARRIENDO ALMACEN")
                    {
                        cb.SetTextMatrix(430f, (float)(540 - Y));
                        dValor = System.Convert.ToDouble(cantidad[di]);
                        if (preciounitario[di] != "0")
                        {
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 450f, (float)(540 - Y), 0);
                            dValor = System.Convert.ToDouble(preciounitario[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 510f, (float)(540 - Y), 0);
                        }
                    }
                    if (preciounitario[di] != "0")
                    {
                        dValor = System.Convert.ToDouble(totalitem[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);
                    }

                    Y += 10;
                }
            }

            // aqui va el timbre electronicodel TED

            iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"X:\dte33\TIMBRE\" + Folio + ".Png");

            imagen2.SetAbsolutePosition(60f, 135f);
            imagen2.BorderWidth = 0f;
            imagen2.ScaleToFit(200f, 230f);
            doc.Add(imagen2);
            cb.EndText();

            cb.BeginText();

            cb.SetFontAndSize(baseFont, 9f);
            cb.SetTextMatrix(120f, 120f);
            cb.ShowText("Timbre Electrónico SII");

            cb.SetTextMatrix(120f, 110f);
            cb.ShowText("Resolución 80 de 2014");

            cb.SetTextMatrix(100f, 100f);
            cb.ShowText("Verifique documento:www.sii.cl");

            // pagina 1

            cb.SetFontAndSize(baseFont, 10f);


            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Neto $", 500f, 225F, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Iva(19%) $", 500f, 215F, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total $", 500f, 205F, 0);

            dValor = System.Convert.ToInt32(MntNeto);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 225F, 0);

            dValor = System.Convert.ToInt32(IVA);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 215F, 0);


            dValor = System.Convert.ToInt32(MntTotal);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 205F, 0);

            cb.EndText();
        
            
            doc.Close();
            writer.Close();
            string pdfPath;
            if (chkPrefactura.Checked == false)
            {
                pdfPath = string.Concat(new string[] { @"X:\dte33\PDF\pdf_", tipo, "COPIA", cnumerox.Trim(), ".pdf" });
                if (TrackID.Length > 0)
                {
                    //actuliza_cliente(pdfPath, cnumerox, DTE_ENVIO, MntNeto, IVA, MntTotal);
                }
                else
                {
                    MessageBox.Show("no hay respuesta track id");
                }
            }


        }

     
        private void actuliza_cliente(string pdfPath, string cnumerox, string DTEXML, string Neto, string IVA, string Total)
        {
            XmlReader LEERXML = XmlReader.Create(DTEXML);
            XmlDocument DTE = new System.Xml.XmlDocument();


            DTE.PreserveWhitespace = true;
            DTE.Load(LEERXML);
             string StrSQL;
              SqlCommand ObjComando = new SqlCommand();
              SqlTransaction Transsql = null;
              using (SqlConnection objconnSQL = new SqlConnection(Constantes_Variables.ConexionStringW))
              {
                  try
                  {
                         StrSQL = " select factura ";
                         StrSQL += "  FROM facturasPDF ";
                         StrSQL += "  where factura=" + cnumerox.Trim();
           
                       
                            objconnSQL.Open();

                            // consulta la existencia de factura pdf
                            DataSet dsFactudapdf = new DataSet();
                            SqlDataAdapter daFacturapdf = new SqlDataAdapter();
                            daFacturapdf = new SqlDataAdapter(StrSQL, objconnSQL);
                            daFacturapdf.Fill(dsFactudapdf);
                            objconnSQL.Close();


                            if(dsFactudapdf.Tables[0].Rows.Count ==0)
                            {

                     
                                  StrSQL = "insert into facturasPDF (rut,ultimoarchivopdf,factura,fecha,Neto,Exento,Iva,Total,TipoDTE,trackid,status,codigosii)";
                                  StrSQL += " values(" ;
                                  StrSQL += "'" + DTFacturas.Rows[0]["MAECLI.rut"].ToString()  + DTFacturas.Rows[0]["dvrut"].ToString() +"',";
                                  StrSQL += "'" + pdfPath + "',";
                                  StrSQL += cnumerox.Trim();
                                  StrSQL += ",'" + sFecha +"'";
                                  //StrSQL += "," +DTE.InnerXml;
                                   StrSQL += "," + Neto;
                                   StrSQL += ",0" ;
                                   StrSQL += "," + IVA;
                                   StrSQL += "," + Total;
                                   StrSQL += ",33";
                                   StrSQL += ","+ TrackID;
                                   StrSQL += ","+  Estado;
                                   StrSQL += ",33";

                               

                                  StrSQL += ")";

                                  objconnSQL.Open();
                                  Transsql = objconnSQL.BeginTransaction();
                                  ObjComando.Transaction = Transsql;

                                  ObjComando.Connection = objconnSQL;
                                  ObjComando.CommandText = StrSQL;
                                  ObjComando.ExecuteNonQuery();
                                  Transsql.Commit();
                                  objconnSQL.Close();
                            }
                  }

                  catch (SqlException ex)
                  {
                      MessageBox.Show(ex.Message);
                      Transsql.Rollback();

                  }
                  finally
                  {
                      
                      Transsql = null;
                  }
              }
        }

        private void Facturas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
           
        }

        private void dgvFacturas_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Fila = e.RowIndex;
            if (dgvFacturas.Rows[Fila].Cells["numdoc"].Value != null)
            {
                if (dgvFacturas.Rows[Fila].Cells["numdoc"].Value.ToString() != "")
                {
                    dgvSii.Rows[Fila].Cells["numdocD"].Value = dgvFacturas.Rows[Fila].Cells["numdoc"].Value;
                    //dgvSii.Rows[Fila].Cells["fechaD"].Value = dgvFacturas.Rows[Fila].Cells["fecha"].Value;
                    //dgvSii.Rows[Fila].Cells["TOTALD"].Value = dgvFacturas.Rows[Fila].Cells["Total"].Value;
                    //dgvSii.Rows[Fila].Cells["ocD"].Value = dgvFacturas.Rows[Fila].Cells["oc"].Value;

                    dgvFacturas.Rows[Fila].Cells["numdoc"].Value = "";
                    //dgvFacturas.Rows[Fila].Cells["fecha"].Value = "";
                    //dgvFacturas.Rows[Fila].Cells["Total"].Value = "";
                    //dgvFacturas.Rows[Fila].Cells["oc"].Value = "";
                    btnSii.Enabled = true;
                 

                    //int selectedindex =dgvFacturas.SelectedRows[0].Index;

                     dgvFacturas.ClearSelection();

                     dgvFacturas.Rows[Fila + 1].Selected = true;



                }
            }


        }

        private void dgvFacturas_Enter(object sender, EventArgs e)
        {
           
        }

        private void dgvFacturas_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvFacturas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                Fila = dgvFacturas.CurrentRow.Index;
                if (dgvFacturas.Rows[Fila].Cells["numdoc"].Value != null)
                {
                    if (dgvFacturas.Rows[Fila].Cells["numdoc"].Value.ToString() != "")
                    {
                        dgvSii.Rows[Fila].Cells["numdocD"].Value = dgvFacturas.Rows[Fila].Cells["numdoc"].Value;
                        //dgvSii.Rows[Fila].Cells["fechaD"].Value = dgvFacturas.Rows[Fila].Cells["fecha"].Value;
                        //dgvSii.Rows[Fila].Cells["TOTALD"].Value = dgvFacturas.Rows[Fila].Cells["Total"].Value;
                        //dgvSii.Rows[Fila].Cells["ocD"].Value = dgvFacturas.Rows[Fila].Cells["oc"].Value;

                        dgvFacturas.Rows[Fila].Cells["numdoc"].Value = "";
                        //dgvFacturas.Rows[Fila].Cells["fecha"].Value = "";
                        //dgvFacturas.Rows[Fila].Cells["Total"].Value = "";
                        //dgvFacturas.Rows[Fila].Cells["oc"].Value = "";
                        btnSii.Enabled = true;


                        //int selectedindex =dgvFacturas.SelectedRows[0].Index;

                        dgvFacturas.ClearSelection();
                        if (dgvFacturas.RowCount < Fila)
                            Fila++;
                        dgvFacturas.Rows[Fila ].Selected = true;



                    }
                }
            }
        }

        private void btnPDFS_Click(object sender, EventArgs e)
        {
            //for (int i=0; i  <dgvSii.RowCount ; i++ )
            {
                //Fila = e.RowIndex;
                //columna = e.ColumnIndex;

                Folio=dgvFacturas.Rows[Fila].Cells[2].Value.ToString().Trim();
                sIdCliente = dgvFacturas.Rows[Fila].Cells[5].Value.ToString().Trim();
                Rut = dgvFacturas.Rows[Fila].Cells[6].Value.ToString().Trim();
                if (Folio.Length > 0)
                {
                    DTE_ENVIO = @"X:\dte33\XMLSII\EnvioDTE_" + Folio + ".XML";
                    Imprime_PDF_Cedible(Folio, DTE_ENVIO); 
                    MessageBox.Show("FIN CREACION DE PDF cedible ");
                }

            }
           
        }

        private void Imprime_PDF_Cedible(string Folio, string DTE_ENVIO)
        {
            

            string carchivo = DTE_ENVIO;
            string tipo = "33";
            string cnumerox = Folio;
            string erut = "";
            string eimagen = "";
          

            erut = "93.945.000-9";
            eimagen = @"X:\dte33\logo.jpg";
        


            string[] Dia_nombre = new string[7] {"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"};
            int NumeroDia;
            string RUTRecep = "";
            string RznSocRecep = "";
            string GiroRecep = "";
            string DirRecep = "";
            string CmnaRecep = "";
            string CiudadRecep = "";
            string MntNeto = "";
            string IVA = "";
            string MntTotal = "";
            string FchEmis = "";
            string FmaPago = "";

            string Nrolinref = "";
            string TpoDocref ="";
            string FolioRef = "";
            string FchRef = "";

            string Nrolinref1 = "";
            string TpoDocref1 = "";
            string FolioRef1= "";
            string FchRef1 = "";

            string Nrolinref2 = "";
            string TpoDocref2 = "";
            string FolioRef2 = "";
            string FchRef2 = "";



            string[] cantidad = new string[21];
            string[] preciounitario = new string[21];
            string[] totalitem = new string[21];

            string[] operacion = new string[21];
            string[] saldo = new string[21];
            string[] basefac = new string[21];
            string[] pesos = new string[21];
            string[] fdesde = new string[21];
            string[] fhasta = new string[21];
            string[] detalle = new string[21];
            object[] oDetalle = new object[21];





            string[] sNrolinref = new string[3];
            string[] sTpoDocref = new string[3];
            string[] sFolioref = new string[3];
            string[] sFchref = new string[3];

            string sDetalle;
            string sPesos;
            string uriDTE = DTE_ENVIO;
            int i = 0;
            using (XmlReader reader3 = XmlReader.Create(uriDTE))
            {
                while (reader3.Read())
                {
                    if (reader3.IsStartElement())
                    {
                        string text = reader3.Name.ToString();
                        switch (text)
                        {
                            case "FmaPago":
                                FmaPago = reader3.ReadString();
                                break;
                            case "FchEmis":
                                FchEmis = reader3.ReadString();
                                break;
                            case "RUTRecep":
                                RUTRecep = reader3.ReadString();
                                break;
                            case "RznSocRecep":
                                RznSocRecep = reader3.ReadString();
                                break;
                            case "GiroRecep":
                                GiroRecep = reader3.ReadString();
                                break;
                            case "DirRecep":
                                DirRecep = reader3.ReadString();
                                break;
                            case "Contacto":
                                {
                                    string Contacto = reader3.ReadString();
                                    break;
                                }
                            case "CmnaRecep":
                                CmnaRecep = reader3.ReadString();
                                break;
                            case "CiudadRecep":
                                CiudadRecep = reader3.ReadString();
                                break;
                            case "MntNeto":
                                MntNeto = reader3.ReadString();
                                break;
                            case "MntExe":
                                {
                                    string MntExe = reader3.ReadString();
                                    break;
                                }
                            case "TasaIVA":
                                {
                                    string TasaIVA = reader3.ReadString();
                                    break;
                                }
                            case "IVA":
                                IVA = reader3.ReadString();
                                break;
                            case "MntTotal":
                                MntTotal = reader3.ReadString();
                                break;
                            case "VlrCodigo":
                                {
                                    string VlrCodigo = reader3.ReadString();

                                    break;
                                }


                            case "NmbItem":
                                {
                                    sDetalle = reader3.ReadString().Normalize();
                                    string VALE = sDetalle.Substring(0, 6);
                                    int ILargo = sDetalle.Length;
                                    double retNum;

                                    //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

                                    bool noNumerico = true;

                                    if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true )
                                    {
                                        if(Convert.ToDouble(VALE.ToString()) > 500000)
                                            {
                                            ILargo = 71;
                                            
                                            }


                                    }
                                    else
                                    {
                                        noNumerico = false;
                                        ILargo = 65;
                                        // no es numerico
                                    }

                                    if (ILargo > 71 )
                                    {
                                        if (noNumerico == true)
                                        {
                                            operacion[i] = sDetalle.Substring(0, 6).Trim().Replace(".", ",");
                                            saldo[i] = sDetalle.Substring(6, 15).Trim().Replace(".", ",");
                                            basefac[i] = sDetalle.Substring(22, 15).Trim().Replace(".", ",");
                                            sPesos = sDetalle.Substring(38, 16);
                                            sPesos.PadLeft(16);
                                            pesos[i] = sPesos.PadLeft(16);
                                            pesos[i].PadLeft(16);
                                            fdesde[i] = sDetalle.Substring(55, 9);
                                            fhasta[i] = sDetalle.Substring(64, 8);
                                        }
                                    }
                                    detalle[i] =sDetalle;
                                    oDetalle[i] = operacion[i] + saldo[i] + basefac[i] + pesos[i] + " " + fdesde[i] + fhasta[i];
                                    break;

                                }
                            case "QtyItem":
                                {
                                    string QtyItem = reader3.ReadString();
                                    cantidad[i] = QtyItem;
                                    break;
                                }
                            case "UnmdItem":
                                {
                                    string UnmdItem = reader3.ReadString();

                                    break;
                                }
                             case "PrcItem":
                                {
                                    preciounitario[i] = "0";
                                    string PrcItem = reader3.ReadString();
                                    if (PrcItem != "\n")
                                    {
                                        preciounitario[i] = PrcItem;
                                    }
                                    else
                                    {
                                        preciounitario[i] = "0";
                                    }
                                    break;
                                }
                            case "MontoItem":
                                {
                                    totalitem[i] = "0";
                                    string MontoItem = reader3.ReadString();
                                    if (MontoItem != "\n")
                                    {
                                        totalitem[i] = MontoItem;
                                    }
                                    else
                                    {
                                        totalitem[i] = "0";
                                    }
                                   
                                    i++;
                                    break;
                                }
                                // descuentos  por almacen
                                case "GlosaDR":
                                {
                                    sDetalle = reader3.ReadString().Normalize();
                                    
                                    detalle[i] = sDetalle;
                                    oDetalle[i] = sDetalle;
                                    break;
                                }

                                case "ValorDR":
                                {
                                    string MontoItem = reader3.ReadString();
                                    if (MontoItem.IndexOf("_") != -1)
                                    {
                                        totalitem[i] = "-" + MontoItem;
                                    }
                                    else
                                    {
                                        totalitem[i] =  MontoItem;
                                    }
                                    
                                    i++;
                                    break;
                                    
                                }
                               

                            case "NroLinRef":
                                {
                                   Nrolinref= reader3.ReadString();
                                   if(Nrolinref=="1")
                                   {
                                       Nrolinref1 = Nrolinref;
                                    }
                                   if (Nrolinref == "2")
                                   {
                                       Nrolinref2 = Nrolinref;
                                   }
                                

                                   break;
                                }
                            case "TpoDocRef":
                                {
                                   TpoDocref = reader3.ReadString();
                                   
                                   if (Nrolinref == "1")
                                   {
                                       TpoDocref1 = TpoDocref;
                                   }
                                   if (Nrolinref == "2")
                                   {
                                       TpoDocref2 = TpoDocref;
                                   }
                                    break;
                                }
                            case "FolioRef":
                                {
                                   FolioRef = reader3.ReadString();
                                   if (Nrolinref == "1")
                                   {
                                       FolioRef1 = FolioRef;
                                   }
                                   if (Nrolinref == "2")
                                   {
                                       FolioRef2 = FolioRef;
                                   }
                                    break;
                                }
                            case "FchRef":
                                {
                                   FchRef = reader3.ReadString();
                                   if (Nrolinref == "1")
                                   {
                                       FchRef1 = FchRef;
                                   }
                                   if (Nrolinref == "2")
                                   {
                                       FchRef2 = FchRef;
                                   }

                                    break;
                                }
                        }
                    }
                }
            }

            string telefono = "";
            DateTime Emision;
          
            Document doc = new Document(PageSize.LETTER);
            PdfWriter writer;
           

            if (chkPrefactura.Checked == false)
            {
                 writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[] { @"X:\dte33\PDF\pdf_", tipo, "DTE_Cedible", cnumerox.Trim(), ".pdf" }), System.IO.FileMode.Create));
            }
            else
            {
                 writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[] { @"X:\dte33\PDF\PreFacturaPDF_", tipo, "DTE", cnumerox.Trim(), ".pdf" }), System.IO.FileMode.Create));
            }
            doc.AddTitle("Factura electrónica");
            doc.AddCreator("Ricardo Leppe");

            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0, BaseColor.BLACK);
            PdfContentByte cb = writer.DirectContent;



            if (chkPrefactura.Checked == false)
            {
               
                doc.NewPage();
                // aqui va la tercera y ultima pagina carretera
                //
                //
                //
                //---------------------------------------------------------------------------
                cb.BeginText();


                BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\Arialmt.ttf", "Cp1252", false);
                //cb.SetFontAndSize(f_cn, 8f);
                // logo de almadena
                cb.SetFontAndSize(f_cn, 10f);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(eimagen);
                jpg.ScaleToFit(100f, 100f);
                jpg.ScaleToFit(80f, 80f);
                jpg.SpacingBefore = 5f;
                jpg.SpacingAfter = 10f;
                jpg.Alignment = 0;
                doc.Add(jpg);
                cb.EndText();
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.CP1257, false);
                if (chkPrefactura.Checked == false)
                {
                    //marco rojo
                    cb.SetColorStroke(BaseColor.RED.Darker());
                    cb.SetLineWidth(2);
                    cb.Rectangle(390f, 680f, 190f, 90f);

                    cb.Stroke();
                    cb.SetLineWidth(1);

                }

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 9f);


                cb.SetTextMatrix(110f, 750f);
                cb.ShowText("ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.");
                // razon social


                cb.SetTextMatrix(110f, 740f);
                cb.ShowText("Giro:ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES");
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 9f);
                cb.SetTextMatrix(120f, 710f);
                cb.ShowText("Casa Matriz:Moneda 812 Of.705,Santiago");
                cb.SetTextMatrix(120f, 700f);
                cb.ShowText("Fono:22347 6500");

                cb.SetTextMatrix(120f, 690f);
                cb.ShowText("Bodega:Camino Lo Sierra 04460 San Bernardo, Santiago");
                cb.SetTextMatrix(120f, 680f);
                cb.ShowText("Fono:232634455");
                cb.SetTextMatrix(35f, 660f);
                cb.ShowText("www.almadena.cl - Email:almadena@almadena.cl");
               
                if (chkPrefactura.Checked == false)
                {
                    cb.SetColorFill(BaseColor.RED.Darker());

                    cb.SetFontAndSize(baseFont, 14f);
                    cb.SetTextMatrix(420f, 750f);
                    cb.ShowText("R.U.T.:" + erut);


                    cb.SetTextMatrix(400f, 730f);
                    cb.ShowText("FACTURA ELECTRÓNICA");

                    cb.SetTextMatrix(450f, 720f);
                    cb.ShowText("");

                    cb.SetTextMatrix(460f, 700f);
                    cb.ShowText("N°   " + cnumerox.Trim());

                    cb.SetFontAndSize(baseFont, 10f);
                    cb.SetTextMatrix(420f, 665f);
                    cb.ShowText("S.I.I.- SANTIAGO CENTRO");
                }
                else
                {
                    cb.SetFontAndSize(baseFont, 14f);
                    cb.SetTextMatrix(420f, 665f);
                    cb.ShowText("PRE FACTURA");


                }

                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(460f, 760f);
                cb.EndText();

                //marco encabezado

                cb.SetColorStroke(BaseColor.BLACK);
                cb.Rectangle(30f, 560f, 550f, 90f);
                cb.Stroke();

                cb.BeginText();
                // FECHA DE EMISION
                Emision = Convert.ToDateTime(FchEmis);
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("Es-Es");


                cb.SetTextMatrix(35f, 640f);
                cb.ShowText("Fecha Emisión");
                cb.SetTextMatrix(105F, 640f);
                cb.ShowText(":");

                //System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(;

                cb.SetTextMatrix(110f, 640f);
                NumeroDia = Convert.ToInt16(Emision.DayOfWeek);
                if (NumeroDia == 0)
                {
                    NumeroDia = 6;
                }
                else
                {
                    NumeroDia = NumeroDia - 1;

                }


                cb.ShowText(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Dia_nombre[NumeroDia].ToString()) + "  " + FchEmis.Substring(8, 2) + " de " + ci.DateTimeFormat.GetMonthName(Emision.Month).ToString() + " de " + FchEmis.Substring(0, 4));
                // FECHA DE EMISION
                // FECHA DE EMISION


                iTextSharp.text.pdf.BaseFont Vn_Helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", iTextSharp.text.pdf.BaseFont.EMBEDDED);
                iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(Vn_Helvetica, 12, iTextSharp.text.Font.NORMAL);



                cb.SetFontAndSize(Vn_Helvetica, 10f);

                cb.SetTextMatrix(35f, 630f);
                cb.ShowText("Señor(es)");

                cb.SetTextMatrix(105f, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 630f);
                cb.ShowText(RznSocRecep);

                cb.SetFontAndSize(baseFont, 10f);


                cb.SetTextMatrix(400, 640f);
                cb.ShowText("R.U.T.");

                cb.SetTextMatrix(490f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(500, 640f);
                cb.ShowText(RUTRecep);

                cb.SetTextMatrix(35f, 620f);
                cb.ShowText("Giro");
                cb.SetTextMatrix(105f, 620f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 620f);
                cb.ShowText(GiroRecep);


                cb.SetTextMatrix(35f, 610f);
                cb.ShowText("Dirección");
                cb.SetTextMatrix(105f, 610f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 610f);
                cb.ShowText(DirRecep);

                cb.SetTextMatrix(35f, 600f);
                cb.ShowText("Comuna");
                cb.SetTextMatrix(105f, 600f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 600f);
                cb.ShowText(CmnaRecep);

                cb.SetTextMatrix(35f, 590f);
                cb.ShowText("Ciudad");
                cb.SetTextMatrix(105f, 590f);
                cb.ShowText(":");


                cb.SetTextMatrix(110f, 590f);
                cb.ShowText(CiudadRecep);

                //cb.SetTextMatrix(400f, 600f);
                //cb.ShowText("Orden");
                //cb.SetTextMatrix(490f, 600f);
                //cb.ShowText(":");

                cb.SetTextMatrix(35f, 580f);
                cb.ShowText("Fono");
                cb.SetTextMatrix(105f, 580f);
                cb.ShowText(":");

                cb.SetTextMatrix(110f, 580f);
                cb.ShowText(telefono);
                if (FolioRef1.Length > 0)
                {
                    cb.SetTextMatrix(35f, 570f);
                    cb.ShowText("Referencia");
                    cb.SetTextMatrix(105f, 570f);
                    cb.ShowText(":");

                    cb.SetTextMatrix(110f, 570f);
                    cb.ShowText(Nrolinref1 + "" + "ORDEN COMPRA " + " N° " + FolioRef1 + " del " + FchRef1);
                }
                if (FolioRef2.Length > 0)
                {
                    cb.SetTextMatrix(350f, 570f);
                    cb.ShowText(Nrolinref2 + " Atencion  N° " + FolioRef2 + " del " + FchRef2);
                }

                cb.EndText();
                cb.SetColorFill(BaseColor.GRAY.Darker());
                cb.SetColorStroke(BaseColor.BLACK);
                //marco titulo
                cb.Rectangle(30f, 530f, 550f, 20f);
                cb.Stroke();

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);
                cb.SetTextMatrix(35f, 540f);
                cb.ShowText("Detalle");


                cb.SetTextMatrix(410, 540f);
                cb.ShowText("Cant.");

                cb.SetTextMatrix(460f, 540f);
                cb.ShowText("P.Unitario");

                cb.SetTextMatrix(530f, 540f);
                cb.ShowText("Total item");



                cb.EndText();
                //marco detalle
                cb.SetColorStroke(new CMYKColor(0f, 12f, 20f, 84f));
                cb.Rectangle(30f, 310f, 550f, 220f);
                cb.Stroke();
                cb.BeginText();
                int Y = 21;
                double dValor;
                Y = 21;
                dValor = 0;

                for (int di = 0; di < 21; di++)
                {
                    if (detalle[di] != null)
                    {
                        string VALE = detalle[di].Substring(0, 6);
                        int ILargo = detalle[di].Length;
                        double retNum;

                        //isNum = Double.TryParse(Convert.ToString( VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);



                        if (Double.TryParse(Convert.ToString(VALE), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum) == true)
                        {
                            if (Convert.ToDouble(VALE.ToString()) > 500000)
                            {
                                ILargo = 71;
                            }


                        }
                        else
                        {

                            ILargo = 65;
                        }


                        if (ILargo > 71)
                        {

                            cb.SetTextMatrix(35f, (float)(540 - Y));
                            cb.ShowText(operacion[di]);

                            dValor = System.Convert.ToDouble(saldo[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 160f, (float)(540 - Y), 0);

                            dValor = System.Convert.ToDouble(basefac[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N2}", dValor), 260f, (float)(540 - Y), 0);

                            dValor = System.Convert.ToDouble(pesos[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 340f, (float)(540 - Y), 0);

                            cb.SetTextMatrix(345f, (float)(540 - Y));
                            cb.ShowText(fdesde[di]);

                            cb.SetTextMatrix(390f, (float)(540 - Y));
                            cb.ShowText(fhasta[di]);
                        }
                        else
                        {
                            cb.SetTextMatrix(35f, (float)(540 - Y));
                            int xLargo = detalle[di].Length;
                            if (xLargo < ILargo)
                            {
                                ILargo = xLargo;
                            }
                            cb.ShowText(detalle[di].Substring(0, ILargo));

                        }

                        if (detalle[di] != "DESCUENTO ARRIENDO ALMACEN")
                        {
                            cb.SetTextMatrix(430f, (float)(540 - Y));
                            dValor = System.Convert.ToDouble(cantidad[di]);
                            if (preciounitario[di] != "0")
                            {
                                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 450f, (float)(540 - Y), 0);
                                dValor = System.Convert.ToDouble(preciounitario[di]);
                                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 510f, (float)(540 - Y), 0);
                            }
                        }
                        if (preciounitario[di] != "0")
                        {
                            dValor = System.Convert.ToDouble(totalitem[di]);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);
                        }

                        Y += 10;
                    }
                }

                // aqui va el timbre electronicodel TED

               iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"X:\dte52\TIMBRE\" + Folio + ".Png");

                imagen2.SetAbsolutePosition(60f, 135f);
                imagen2.BorderWidth = 0f;
                imagen2.ScaleToFit(200f, 230f);
                doc.Add(imagen2);
                cb.EndText();

                cb.SetColorStroke(BaseColor.BLACK);
                //cb.Rectangle(275f, 65f, 150f, 180f);
                cb.Rectangle(275f, 130f, 150f, 110f);
                //cb.Rectangle(15f, 30f, 590f, 170f);
                cb.Stroke();

                cb.BeginText();

                cb.SetFontAndSize(baseFont, 9f);
                cb.SetTextMatrix(120f, 120f);
                cb.ShowText("Timbre Electrónico SII");

                cb.SetTextMatrix(120f, 110f);
                cb.ShowText("Resolución 80 de 2014");

                cb.SetTextMatrix(100f, 100f);
                cb.ShowText("Verifique documento:www.sii.cl");



                //cb.SetFontAndSize(baseFont, 8f);
                cb.SetFontAndSize(Vn_Helvetica, 8f);
                cb.SetTextMatrix(310f, 220f);
                cb.ShowText("Acuse de recibo");

                cb.SetTextMatrix(280f, 200f);
                cb.ShowText("Nombre _____________________");

                cb.SetTextMatrix(280f, 185f);
                cb.ShowText("Rut   ______________________");

                cb.SetTextMatrix(280f, 170f);
                cb.ShowText("Fecha ______________________");



                cb.SetTextMatrix(280f, 155f);
                cb.ShowText("Recinto_____________________");

                cb.SetTextMatrix(280f, 140f);
                cb.ShowText("Firma ______________________");



                cb.SetTextMatrix(280f, 120f);
                //cb.ShowTextAligned(PdfContentByte..ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);
                cb.ShowText("El  acuse de recibo que  se  declara  en");

                cb.SetTextMatrix(280f, 110f);
                cb.ShowText("este acto, de acuerdo a lo dispuesto en");


                cb.SetTextMatrix(280f, 100f);
                //cb.ShowTextAligned()
                cb.ShowText("la letra b) del art. 4°, y la letra c) del art.");

                cb.SetTextMatrix(280f, 90f);
                cb.ShowText("5°  de  la  ley  19.983,  acredita  que  la ");


                cb.SetTextMatrix(280f, 80f);
                cb.ShowText("entrega de mercaderías o servicio (s) ");


                cb.SetTextMatrix(280f, 70f);
                cb.ShowText("prestado(s) ha(n) sido recibido(s).");

                cb.SetFontAndSize(baseFont, 11f);
                cb.SetTextMatrix(430f, 70f);
                cb.ShowText("CEDIBLE");

                cb.EndText();


                cb.BeginText();
                cb.SetFontAndSize(baseFont, 10f);


                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Neto $", 500f, 225F, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Iva(19%) $", 500f, 215F, 0);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total $", 500f, 205F, 0);

                dValor = System.Convert.ToInt32(MntNeto);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 225F, 0);

                dValor = System.Convert.ToInt32(IVA);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 215F, 0);


                dValor = System.Convert.ToInt32(MntTotal);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 205F, 0);

                cb.EndText();
            }
                doc.Close();
                writer.Close();
                string pdfPath;
                if (chkPrefactura.Checked == false)
                {
                     //@"X:\dte33\PDF\pdf_", tipo, "DTE_Cedible", cnumerox.Trim(), ".pdf" 
                    
                    pdfPath = string.Concat(new string[] { @"X:\dte33\PDF\pdf_", tipo, "DTE_CEDIBLE", cnumerox.Trim(), ".pdf" });
                    if (TrackID.Length > 0)
                    {
                        actuliza_cliente(pdfPath, cnumerox, DTE_ENVIO, MntNeto, IVA, MntTotal);
                    }
                    else
                    {
                        MessageBox.Show("no hay respuesta track id");
                    }
                }
             

        
        }

        private void chkbUpload_CheckedChanged(object sender, EventArgs e)
        {
            chkEnviaXmlCliente.Checked = true;
        }

        private void chkPrefactura_CheckedChanged(object sender, EventArgs e)
        {
            chkEnviaXmlCliente.Enabled = false;
            chkbUpload.Enabled = false;
        }

        private void chkSoloXmlCliente_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbL_CheckedChanged(object sender, EventArgs e)
        {

        }

        public static object Track { get; set; }

        private void chkEnviaPDFCliente_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

