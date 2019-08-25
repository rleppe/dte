using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Web;
using System.Security.Cryptography.Xml;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Xml.XPath;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

using System.Diagnostics;








namespace DTE33
{
    public partial class GuiaDespacho : Form
    {
        public GuiaDespacho()
        {
            InitializeComponent();
        }
        public static string RutCliente = "";
        public static string Razon_Soc_Cliente = "";
        public static string Giro_Cliente = "";
        public static string Dir_Cliente = "";
        public static string Comuna_Cliente = "";
        public static string Ciudad_Cliente = "";
        public static string Fecha_hora_envio = "";
        public static double Monto_Neto = 0;
        public static Double Monto_Iva = 0;
        public static Double Monto_Total = 0;
        public static int Tasa_IVA = 19;
        public static string NumeroGuias = "10";
        public static string Folio;
        public static string Factura_e = "XML_T33";
        public static string NCredito_e = "XML_T31";
        public static string Guia_e = "XML_";
        public static DateTime FechaEmi = DateTime.Today.Date;
        public static string xml = "";
        public static DataTable DTCliente;
        public static DataTable DTGuias;
        public static DataTable DTDetalle;
        public static DataTable DTNguias;
        public static string Guia = "MG";
        public static string Producto = "MP";
        public static int MetrosPrevios = 0;
        public static int MetrosSaldo = 0;
        public static int MetrosMov=0;
        public static string Bodeguero;
        public static string FechaEmision;

        public static int largo_item = 0;
        public static Int32 iidG;
        //public static string  Unidad="S:\\";

        public static string DTE_ENVIO = "";
        public static string DTE_ENVIO_SEED = "";
        public static string MNT;
        public static string IT1;


        private void button1_Click(object sender, EventArgs e)
        {
           
                
               DTE33.Constantes_Variables.Unidad = "c:\\leyton";
               Fecha_hora_envio = DateTime.Now.Date.ToShortDateString() + "T" + DateTime.Now.ToLongTimeString();
               

                FechaEmi = dtpFecha.Value.Date;
                if (rbS.Checked == true)
                {
                   DTE33.Constantes_Variables.Unidad = "c:\\leyton";
                }
                if (rbL.Checked == true)
                {
                   DTE33.Constantes_Variables.Unidad = "O:\\";
                }

                asigna_archivo_Guias(); 
                Carga_Guias();            
            
          
        }


        private void Carga_Guias()
        {
           
            string Rut_caratula = "";
            string xCodcli = "";
            //bool Catatula = false;
          
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            // fecha de la ultima actualizacion

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsGuias = new DataSet();
                OleDbDataAdapter daGuias = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT count(GUI.GUI_NUMERO) as GUIAS,cli.codcli FROM " + Guia + " as GUI INNER JOIN MAECLI ";
                StrOledbDBFIV += "as cli ON (GUI.CODCLI = cli.CODCLI )   ";
                StrOledbDBFIV += " where GUI.GUI_TIPMOV ='S' ";
                StrOledbDBFIV += " and GUI.GUI_NUMERO = " + cboGuias.SelectedValue.ToString();
                StrOledbDBFIV += " group by cli.codcli";
                daGuias = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daGuias.Fill(dsGuias);
                DTNguias = dsGuias.Tables[0];
               


                if (DTNguias.Rows.Count == 0)
                {
                    MessageBox.Show("Seleccion de registros 0");
                }
                objconnDBFIV.Close();

                //Catatula = true;
                for (int i = 0; i <DTNguias.Rows.Count; i++)
                {              
                    xCodcli =DTNguias.Rows[i]["codcli"].ToString();
                    Busca_Rut_Cliente(ref xCodcli, out Rut_caratula);
                    GUIA(i);
                }
               
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error de Selección de Registros DBF " + ex.Message);
            }
        }
        //otra guia en xml
        private void GUIA(int indice)
        {
            XmlDocument DTE = new XmlDocument();
            string FechaEmision;
            string xNombre = "";
           
            string xNombreProducto;
          
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsGuias = new DataSet();
                OleDbDataAdapter daGuias = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT * FROM " + Guia + " as GUIAS";
                StrOledbDBFIV += " INNER JOIN MAECLI as MAECLI ON (GUIAS.CODCLI = MAECLI.CODCLI )";
                StrOledbDBFIV += " where GUIAS.GUI_TIPMOV ='S'";
                StrOledbDBFIV += " and MAECLI.codcli =" + DTNguias.Rows[indice]["codcli"].ToString();
                StrOledbDBFIV += " and guias.gui_numero =" + cboGuias.SelectedValue.ToString();

              




                daGuias = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daGuias.Fill(dsGuias);
                DTGuias = dsGuias.Tables[0];
                // barra de progreso
                objconnDBFIV.Close();

            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
               int n=DTGuias.Rows.Count;
               double neto=0;
               double iva=0;
               double total=0;
               for (int j = 0; j < n; j++)
               {

                   double cantidad = Convert.ToDouble(DTGuias.Rows[j]["GUI_cantid"].ToString());
                   neto+=cantidad;
                  
               }
               iva =Math.Round(neto * 0.19,0);
               total=neto+iva;
               if( n > 0)
                 n=1;

                for (int j = 0; j < n ; j++)
                {

                    FechaEmision =DTGuias.Rows[j]["gui_fecha"].ToString().Trim().Substring(6, 4) + "-" +
                                  DTGuias.Rows[j]["gui_fecha"].ToString().Trim().Substring(3, 2) + "-" +
                                  DTGuias.Rows[j]["gui_fecha"].ToString().Trim().Substring(0, 2);
                    xNombre = DTGuias.Rows[j]["nombre"].ToString();
                    xNombre = xNombre.Replace("&", "y");
                    xNombre = CambiaNombre.Nombre(xNombre);
                    Folio = DTGuias.Rows[j]["GUI_NUMERO"].ToString();
                    string fechahora = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    xml = @"<?xml version='1.0' encoding='ISO-8859-1'?>";



                    xml += @"<DTE version='1.0'>";
                            xml += @"<Documento ID='T52F" + Folio + "'>";
                            xml += @"<Encabezado>";
                            xml += @"<IdDoc>";
                                xml += @"<TipoDTE>52</TipoDTE >";
                                xml += @"<Folio>" + Folio + "</Folio>";
                                xml += @"<FchEmis>" + FechaEmision + "</FchEmis>";
                                xml += @"<TipoDespacho>2</TipoDespacho>";
                                xml += @"<IndTraslado>6</IndTraslado>";
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
                                xml += @"<RUTRecep>" + DTGuias.Rows[j]["rut"].ToString() + "-" + DTGuias.Rows[j]["dvrut"].ToString() + "</RUTRecep>";
                                int  largo_item = 39;
                                if (xNombre.Length < 39)
                                    largo_item = xNombre.Length;
                                xml += @"<RznSocRecep>" + xNombre.Substring(0,largo_item) + "</RznSocRecep>";
                                xml += @"<GiroRecep>" + DTGuias.Rows[j]["giro"].ToString() + "</GiroRecep>";
                                //xml += @"<Contacto>NO</Contacto>";
                                xml += @"<DirRecep>" + DTGuias.Rows[j]["domic"].ToString() + "</DirRecep>";
                                xml += @"<CmnaRecep>" + DTGuias.Rows[j]["comuna"].ToString() + "</CmnaRecep>";
                                xml += @"<CiudadRecep>" + DTGuias.Rows[j]["ciudad"].ToString() + "</CiudadRecep>";
                            xml += @"</Receptor>";

                            xml += @"<Totales>";
                                xml += @"<MntNeto>"+neto.ToString()+"</MntNeto>";
                                xml += @"<MntExe>0</MntExe>";
                                xml += @"<TasaIVA>19</TasaIVA>";
                                xml += @"<IVA>"+ iva.ToString() +"</IVA>";
                                xml += @"<MntTotal>"+ total.ToString() +"</MntTotal>";
                            xml += @"</Totales>";
                            xml += @"</Encabezado>";

                            //detalle de las liena de factura
                            Detalle_guia(DTGuias.Rows[j]["GUI_NUMERO"].ToString().Trim());
                            // signature del dete
                                                            //xml += @"<Referencia>";
                                                            //xml += @"<NroLinRef>1</NroLinRef>";
                                                            //xml += @"<TpoDocRef>52</TpoDocRef>";
                                                            //xml += @"<IndGlobal>1</IndGlobal>";
                                                            //xml += @"<FolioRef>0</FolioRef>";
                                                            //xml += @"<FchRef>" + FechaEmision + "</FchRef>";

                                                            //         xOrden = DTGuias.Rows[j]["gui_orden"].ToString();
                                                            //         xOrden=xOrden.Replace("&", "y");
                                                            //         xOrden  = CambiaNombre.Nombre(xOrden);


                                                            //xml += @"<RazonRef>" + xOrden.Trim() + "</RazonRef>";
                   
                            xml += @"<TED version='1.0'>";
                            xml += @"<DD>";
                                xml += @"<RE>93945000-9</RE>";
                                xml += @"<TD>52</TD>";
                                xml += @"<F>" + Folio + "</F>";

                                xml += @"<FE>" + FechaEmision + "</FE>";
                                xml += @"<RR>" + DTGuias.Rows[j]["rut"].ToString() + "-" + DTGuias.Rows[j]["dvrut"].ToString() + "</RR>";
                               largo_item = 39;
                              if (xNombre.Length < 39)
                                  largo_item = xNombre.Length;

                                xml += @"<RSR>" + xNombre.Substring(0,largo_item) + "</RSR>";
                                xml += @"<MNT>"+ total.ToString() +"</MNT>";
                               xNombreProducto =DTDetalle.Rows[0]["gui_codpro"].ToString() + " " + DTDetalle.Rows[0]["nombre"].ToString();
                               xNombreProducto = xNombreProducto.Replace("&", "y");
                               xNombreProducto = CambiaNombre.Nombre(xNombreProducto);


                                xml += @"<IT1>" + xNombreProducto + "</IT1>";
                                XmlDocument xDoc = new XmlDocument();
                                //La ruta del documento XML permite rutas relativas
                                if (!System.IO.Directory.Exists(@"c:\dte\dte52\CAF\52.xml"))
                                {
                                    XmlDocument caf = new XmlDocument();
                                    caf.PreserveWhitespace = true;
                                    caf.Load(@"c:\dte\dte52\CAF\52.xml");
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

                                xml += @"<TSTED>"+ fechahora+"</TSTED>";
                            xml += @"</DD>";

                
                            xml += @"<FRMT algoritmo='SHA1withRSA'>" + "var8" + "</FRMT>";
                            xml += @"</TED>";
                            xml += @"<TmstFirma>"+ fechahora+"</TmstFirma>";
                            //
                            //busca_metros(DTDetalle.Rows[0]["gui_numop"].ToString().Trim());
                            //busca_bodeguero(DTDetalle.Rows[0]["gui_codbod"].ToString().Trim());

                            xml += @"</Documento>";
                  
                  
                    xml += @"</DTE>";
                    //xml += @"</SetDTE>";
                    //xml += @"</EnvioDTE>";
                    var xmldoc = new XmlDocument();
                   

                    var uri = @"c:\dte\dte52\XMLSII\";
                    var urisemilla = @"c:\dte\dte52\XMLSEMILLA\";
                    var Caratula=@"c:\dte\dte52\CARATULA\DTE_93945000-9_52.xml";
                    var cn="Nicolas Gaston";
                  

                    uri += @Folio + ".xml";

                    xmldoc.LoadXml(xml);
                   
                    xmldoc.Save(uri);

                    NormalizarIzquierdaDTE(uri);
                 

                string FRMT = "";
                FRMT=Timbrar_Ted(uri); //  actuliza el xml incorporando el <FMR>
                
                DTE.PreserveWhitespace = true;
                DTE.Load(uri);
             
                XmlElement node = (XmlElement)DTE.SelectSingleNode("DTE/Documento/TED/FRMT");
                   if (node != null)
                   {
                       node.InnerText = FRMT;
                       DTE.DocumentElement.SetAttribute("xmlns", "http://www.sii.cl/SiiDte");
                       DTE.Save(uri);
                     
                   }
                   firmarDocumentoDTE(uri,cn);
                   ArmaEnvioDte(Caratula,uri,cn);
                   Envia_dte(cn,DTE_ENVIO); // UPLOAD 
                   Imprime_PDF();
                   MessageBox.Show("Enviado");
                }
        
        }

        private void Imprime_PDF()
        {
            
			string carchivo = DTE_ENVIO;
			string crut = "93.945.000-9";
			string tipo = "52";
			string cnumerox =Folio;
			string dirpdf = "";
			
			string erut = "";
			string enombre = "";
			string edireccion = "";
			string egiro = "";
			string eemail = "";
			string eweb = "";
			string etelefono = "";
			string eimagen = "";
			string esii = "";
			
				erut = "93.945.000-9";
				enombre = "ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.";
				edireccion = "Moneda 812 Of.705,Santiago";
				egiro = "ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES";
				eemail = "almadena@almadena.cl";
				eweb = "www.almadena.cl";
				etelefono = "223476500";
				eimagen = @"C:\dte\dte52\logo.jpg";
				esii ="SANTIAGO-CENTRO";
			
			
         
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
			
			string[] cantidad = new string[80];
			string[] preciounitario = new string[80];
			string[] totalitem = new string[80];

            string[] detalle = new string[80];
           

			string uriDTE =DTE_ENVIO;
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
                            //string linea="";
                            detalle[i] = null;
                            string DscItem = reader3.ReadString().ToString().Trim();
                            string[] separador = { "*" };
                            string[] DscItemSeparado = DscItem.Split(separador, StringSplitOptions.RemoveEmptyEntries);


                            detalle[i] = DscItemSeparado[0].ToString().PadLeft(6);
                          

                          
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
							string PrcItem = reader3.ReadString();
							preciounitario[i] = PrcItem;
							break;
						}
						case "MontoItem":
						{
							string MontoItem = reader3.ReadString();
							totalitem[i] = MontoItem;
							i++;
							break;
						}
						}
					}
				}
			}
			string venc = "";
			string telefono = "";
			string vendedor = "";
          
                Document doc = new Document(PageSize.LETTER);
                PdfWriter writer = PdfWriter.GetInstance(doc, new System.IO.FileStream(string.Concat(new string[]
			    {@"C:\dte\dte52\PDF\pdf_",tipo,"DTE",cnumerox.Trim(),".pdf"}), System.IO.FileMode.Create));
                doc.AddTitle("Guia de Despacho electrónica");
                doc.AddCreator("Ricardo Leppe");
               
                doc.Open();
             
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, 0, BaseColor.BLACK);
                PdfContentByte cb = writer.DirectContent;
            
            

                cb.BeginText();

                BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\Arialmt.ttf", "Cp1252", false);

                //cb.SetFontAndSize(f_cn, 8f);
                // logo de almadena
                cb.SetFontAndSize(f_cn, 10f);
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(eimagen);
                //jpg.ScaleToFit(100f, 100f);
                jpg.ScaleToFit(80f, 80f);
                jpg.SpacingBefore = 5f;
                jpg.SpacingAfter = 10f;
                jpg.Alignment = 0;
                doc.Add(jpg);
                cb.EndText();


                //marco rojo
                cb.SetColorStroke(BaseColor.RED.Darker());
                cb.SetLineWidth(2);
                cb.Rectangle(410f, 680f, 170f, 90f);

                cb.Stroke();
                cb.SetLineWidth(1);
                cb.BeginText();
                BaseFont bf_qty12345 = BaseFont.CreateFont("Times-Roman", "Cp1252", false);
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.CP1252, false);


                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);


                cb.SetTextMatrix(150f, 750f);
                cb.ShowText(enombre.Substring(0, 31));
                // razon social
                cb.SetTextMatrix(150f, 740f);
                cb.ShowText(enombre.Substring(31, 16));

                cb.SetTextMatrix(150f, 730f);
                cb.ShowText("Giro:" + egiro.Substring(0, 32));

                cb.SetTextMatrix(150f, 720f);
                //int largox = egiro.Length - 1;
                cb.ShowText(egiro.Substring(32, 12));

                cb.SetTextMatrix(150f, 710f);
                cb.ShowText("Casa Matriz:" + edireccion);
                cb.SetTextMatrix(150f, 700f);
                cb.ShowText("Fono:" + etelefono);
                cb.SetTextMatrix(150f, 690f);
                cb.ShowText("Email:" + eemail);
                cb.SetTextMatrix(150f, 680f);
                cb.ShowText(eweb);
           
                iTextSharp.text.Font newFont = new iTextSharp.text.Font(baseFont, 16f, 0, iTextSharp.text.BaseColor.BLACK);
            
                cb.SetColorFill(BaseColor.RED.Darker());

                cb.SetFontAndSize(baseFont, 12f);
                cb.SetTextMatrix(440f, 750f);
                cb.ShowText("R.U.T.:" + erut);


                cb.SetTextMatrix(435f, 735f);
                cb.ShowText("GUÍA DE DESPACHO");

                cb.SetTextMatrix(450f, 720f);
                cb.ShowText("ELECTRÓNICA");

                cb.SetTextMatrix(460f, 700f);
                cb.ShowText("N°   " + cnumerox.Trim());

                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("S.I.I.- SANTIAGO CENTRO");


                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(460f, 760f);
                cb.EndText();

                //marco encabezado

                cb.SetColorStroke(BaseColor.BLACK);
                cb.Rectangle(30f, 560f, 550f, 90f);
                cb.Stroke();

                cb.BeginText();
                cb.SetTextMatrix(35f, 640f);
                cb.ShowText("Señor(es)");

                cb.SetTextMatrix(85f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 640f);
                cb.ShowText(RznSocRecep);

                cb.SetTextMatrix(400, 640f);
                cb.ShowText("R.U.T.");

                cb.SetTextMatrix(490f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(500, 640f);
                cb.ShowText(RUTRecep);

                cb.SetTextMatrix(35f, 630f);
                cb.ShowText("Giro");
                cb.SetTextMatrix(80f, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 630f);
                cb.ShowText(GiroRecep);


                cb.SetTextMatrix(400f, 630f);
                cb.ShowText("Fecha Emisión");
                cb.SetTextMatrix(490, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(500f, 630f);
                cb.ShowText(FchEmis.Substring(8, 2) + "/" + FchEmis.Substring(5, 2) + "/" + FchEmis.Substring(0, 4));

                cb.SetTextMatrix(35f, 620f);
                cb.ShowText("Dirección");
                cb.SetTextMatrix(85f, 620f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 620f);
                cb.ShowText(DirRecep);

                cb.SetTextMatrix(35f, 610f);
                cb.ShowText("Comuna");
                cb.SetTextMatrix(85f, 610f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 610f);
                cb.ShowText(CmnaRecep);

                cb.SetTextMatrix(35f, 600f);
                cb.ShowText("Ciudad");
                cb.SetTextMatrix(85f, 600f);
                cb.ShowText(":");


                cb.SetTextMatrix(90f, 600f);
                cb.ShowText(CiudadRecep);

                cb.SetTextMatrix(400f, 600f);
                cb.ShowText("Orden");
                cb.SetTextMatrix(490f, 600f);
                cb.ShowText(":");

                cb.SetTextMatrix(35f, 590f);
                cb.ShowText("Fono");
                cb.SetTextMatrix(85f, 590f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 590f);
                cb.ShowText(telefono);

                cb.SetTextMatrix(35f, 580f);
                cb.ShowText("Traslado");
                cb.SetTextMatrix(85f, 580f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 580f);
                cb.ShowText("6=Otros traslado no venta");

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


                cb.SetTextMatrix(450, 540f);
                cb.ShowText("Cant.");

                cb.SetTextMatrix(480f, 540f);
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
                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        cb.ShowText(detalle[di]);

                        cb.SetTextMatrix(430f, (float)(540 - Y));
                        dValor = System.Convert.ToDouble(cantidad[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 470f, (float)(540 - Y), 0);
                        dValor = System.Convert.ToDouble(preciounitario[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 500f, (float)(540 - Y), 0);
                        dValor = System.Convert.ToDouble(totalitem[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);

                        Y += 10;
                    }
                }

                // aqui va el timbre electronicodel TED

                iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"C:\dte\dte52\TIMBRE\" + Folio + ".Png");

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
                cb.ShowText("verifique documento:www.sii.cl");

                 // pagina 1

                cb.SetFontAndSize(baseFont, 10f);


                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Neto $", 500f, 225F, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Iva(19%) $", 500f, 215F, 0);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total $", 500f, 205F, 0);

                dValor = System.Convert.ToInt32(MntNeto);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 225F, 0);

                //dValor = System.Convert.ToInt32(IVA);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 215F, 0);


                //dValor = System.Convert.ToInt32(MntTotal);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, 205F, 0);

                cb.EndText();
                doc.NewPage();
//___________________________________________________________________________________________
// inserta otra pagina 
//
// pagian dos_____________________________________________________________________________
                //cb.Reset();
                cb.BeginText();
                doc.Add(jpg);
               

                //cb.SetFontAndSize(f_cn, 8f);
                // logo de almadena
                cb.SetFontAndSize(f_cn, 10f);
                
                //jpg.ScaleToFit(100f, 100f);
                //doc.Add(jpg);
                cb.EndText();


                //marco rojo
                cb.SetColorStroke(BaseColor.RED.Darker());
                cb.SetLineWidth(2);
                cb.Rectangle(410f, 680f, 170f, 90f);

                cb.Stroke();
                cb.SetLineWidth(1);
                cb.BeginText();
               


                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);


                cb.SetTextMatrix(150f, 750f);
                cb.ShowText(enombre.Substring(0, 31));
                // razon social
                cb.SetTextMatrix(150f, 740f);
                cb.ShowText(enombre.Substring(31, 16));

                cb.SetTextMatrix(150f, 730f);
                cb.ShowText("Giro:" + egiro.Substring(0, 32));

                cb.SetTextMatrix(150f, 720f);
               
                cb.ShowText(egiro.Substring(32, 12));

                cb.SetTextMatrix(150f, 710f);
                cb.ShowText("Casa Matriz:" + edireccion);
                cb.SetTextMatrix(150f, 700f);
                cb.ShowText("Fono:" + etelefono);
                cb.SetTextMatrix(150f, 690f);
                cb.ShowText("Email:" + eemail);
                cb.SetTextMatrix(150f, 680f);
                cb.ShowText(eweb);

               

                cb.SetColorFill(BaseColor.RED.Darker());

                cb.SetFontAndSize(baseFont, 12f);
                cb.SetTextMatrix(440f, 750f);
                cb.ShowText("R.U.T.:" + erut);


                cb.SetTextMatrix(435f, 735f);
                cb.ShowText("GUÍA DE DESPACHO");

                cb.SetTextMatrix(450f, 720f);
                cb.ShowText("ELECTRÓNICA");

                cb.SetTextMatrix(460f, 700f);
                cb.ShowText("N°   " + cnumerox.Trim());

                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("S.I.I.- SANTIAGO CENTRO");


                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(460f, 760f);
                cb.EndText();

                //marco encabezado

                cb.SetColorStroke(BaseColor.BLACK);
                cb.Rectangle(30f, 560f, 550f, 90f);
                cb.Stroke();

                cb.BeginText();
                cb.SetTextMatrix(35f, 640f);
                cb.ShowText("Señor(es)");

                cb.SetTextMatrix(85f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 640f);
                cb.ShowText(RznSocRecep);

                cb.SetTextMatrix(400, 640f);
                cb.ShowText("R.U.T.");

                cb.SetTextMatrix(490f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(500, 640f);
                cb.ShowText(RUTRecep);

                cb.SetTextMatrix(35f, 630f);
                cb.ShowText("Giro");
                cb.SetTextMatrix(80f, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 630f);
                cb.ShowText(GiroRecep);


                cb.SetTextMatrix(400f, 630f);
                cb.ShowText("Fecha Emisión");
                cb.SetTextMatrix(490, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(500f, 630f);
                cb.ShowText(FchEmis.Substring(8, 2) + "/" + FchEmis.Substring(5, 2) + "/" + FchEmis.Substring(0, 4));

                cb.SetTextMatrix(35f, 620f);
                cb.ShowText("Dirección");
                cb.SetTextMatrix(85f, 620f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 620f);
                cb.ShowText(DirRecep);

                cb.SetTextMatrix(35f, 610f);
                cb.ShowText("Comuna");
                cb.SetTextMatrix(85f, 610f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 610f);
                cb.ShowText(CmnaRecep);

                cb.SetTextMatrix(35f, 600f);
                cb.ShowText("Ciudad");
                cb.SetTextMatrix(85f, 600f);
                cb.ShowText(":");


                cb.SetTextMatrix(90f, 600f);
                cb.ShowText(CiudadRecep);

                cb.SetTextMatrix(400f, 600f);
                cb.ShowText("Orden");
                cb.SetTextMatrix(490f, 600f);
                cb.ShowText(":");

                cb.SetTextMatrix(35f, 590f);
                cb.ShowText("Fono");
                cb.SetTextMatrix(85f, 590f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 590f);
                cb.ShowText(telefono);

                cb.SetTextMatrix(35f, 580f);
                cb.ShowText("Traslado");
                cb.SetTextMatrix(85f, 580f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 580f);
                cb.ShowText("6=Otros traslado no venta");

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


                cb.SetTextMatrix(450, 540f);
                cb.ShowText("Cant.");

                cb.SetTextMatrix(480f, 540f);
                cb.ShowText("P.Unitario");

                cb.SetTextMatrix(530f, 540f);
                cb.ShowText("Total item");



                cb.EndText();
                //marco detalle
                cb.SetColorStroke(new CMYKColor(0f, 12f, 20f, 84f));
                cb.Rectangle(30f, 310f, 550f, 220f);
                cb.Stroke();
                cb.BeginText();
                Y= 21;
                dValor=0;

                for (int di = 0; di < 21; di++)
                {
                    if (detalle[di] != null)
                    {
                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        cb.ShowText(detalle[di]);

                        cb.SetTextMatrix(430f, (float)(540 - Y));
                        dValor = System.Convert.ToDouble(cantidad[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 470f, (float)(540 - Y), 0);
                        dValor = System.Convert.ToDouble(preciounitario[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 500f, (float)(540 - Y), 0);
                        dValor = System.Convert.ToDouble(totalitem[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);

                        Y += 10;
                    }
                }

                // aqui va el timbre electronicodel TED

                //iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"C:\dte\dte52\TIMBRE\" + Folio + ".Png");

                imagen2.SetAbsolutePosition(60f, 135f);
                imagen2.BorderWidth = 0f;
                imagen2.ScaleToFit(200f, 230f);
                doc.Add(imagen2);
                cb.EndText();

                //cb.SetColorStroke(BaseColor.BLACK);
                ////cb.Rectangle(275f, 65f, 150f, 180f);
                //cb.Rectangle(275f, 130f, 150f, 110f);
                ////cb.Rectangle(15f, 30f, 590f, 170f);
                //cb.Stroke();

                cb.BeginText();

                cb.SetFontAndSize(baseFont, 9f);
                cb.SetTextMatrix(120f, 120f);
                cb.ShowText("Timbre Electrónico SII");

                cb.SetTextMatrix(120f, 110f);
                cb.ShowText("Resolución 80 de 2014");

                cb.SetTextMatrix(100f, 100f);
                cb.ShowText("verifique documento:www.sii.cl");


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


                //marco rojo
                cb.SetColorStroke(BaseColor.RED.Darker());
                cb.SetLineWidth(2);
                cb.Rectangle(410f, 680f, 170f, 90f);

                cb.Stroke();
                cb.SetLineWidth(1);
                cb.BeginText();



                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);


                cb.SetTextMatrix(150f, 750f);
                cb.ShowText(enombre.Substring(0, 31));
                // razon social
                cb.SetTextMatrix(150f, 740f);
                cb.ShowText(enombre.Substring(31, 16));

                cb.SetTextMatrix(150f, 730f);
                cb.ShowText("Giro:" + egiro.Substring(0, 32));

                cb.SetTextMatrix(150f, 720f);
                
                cb.ShowText(egiro.Substring(32, 12));

                cb.SetTextMatrix(150f, 710f);
                cb.ShowText("Casa Matriz:" + edireccion);
                cb.SetTextMatrix(150f, 700f);
                cb.ShowText("Fono:" + etelefono);
                cb.SetTextMatrix(150f, 690f);
                cb.ShowText("Email:" + eemail);
                cb.SetTextMatrix(150f, 680f);
                cb.ShowText(eweb);



                cb.SetColorFill(BaseColor.RED.Darker());

                cb.SetFontAndSize(baseFont, 12f);
                cb.SetTextMatrix(440f, 750f);
                cb.ShowText("R.U.T.:" + erut);


                cb.SetTextMatrix(435f, 735f);
                cb.ShowText("GUÍA DE DESPACHO");

                cb.SetTextMatrix(450f, 720f);
                cb.ShowText("ELECTRÓNICA");

                cb.SetTextMatrix(460f, 700f);
                cb.ShowText("N°   " + cnumerox.Trim());

                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(420f, 665f);
                cb.ShowText("S.I.I.- SANTIAGO CENTRO");


                cb.SetColorFill(BaseColor.BLACK);
                cb.SetFontAndSize(baseFont, 10f);
                cb.SetTextMatrix(460f, 760f);
                cb.EndText();

                //marco encabezado

                cb.SetColorStroke(BaseColor.BLACK);
                cb.Rectangle(30f, 560f, 550f, 90f);
                cb.Stroke();

                cb.BeginText();
                cb.SetTextMatrix(35f, 640f);
                cb.ShowText("Señor(es)");

                cb.SetTextMatrix(85f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 640f);
                cb.ShowText(RznSocRecep);

                cb.SetTextMatrix(400, 640f);
                cb.ShowText("R.U.T.");

                cb.SetTextMatrix(490f, 640f);
                cb.ShowText(":");

                cb.SetTextMatrix(500, 640f);
                cb.ShowText(RUTRecep);

                cb.SetTextMatrix(35f, 630f);
                cb.ShowText("Giro");
                cb.SetTextMatrix(80f, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 630f);
                cb.ShowText(GiroRecep);


                cb.SetTextMatrix(400f, 630f);
                cb.ShowText("Fecha Emisión");
                cb.SetTextMatrix(490, 630f);
                cb.ShowText(":");

                cb.SetTextMatrix(500f, 630f);
                cb.ShowText(FchEmis.Substring(8, 2) + "/" + FchEmis.Substring(5, 2) + "/" + FchEmis.Substring(0, 4));

                cb.SetTextMatrix(35f, 620f);
                cb.ShowText("Dirección");
                cb.SetTextMatrix(85f, 620f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 620f);
                cb.ShowText(DirRecep);

                cb.SetTextMatrix(35f, 610f);
                cb.ShowText("Comuna");
                cb.SetTextMatrix(85f, 610f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 610f);
                cb.ShowText(CmnaRecep);

                cb.SetTextMatrix(35f, 600f);
                cb.ShowText("Ciudad");
                cb.SetTextMatrix(85f, 600f);
                cb.ShowText(":");


                cb.SetTextMatrix(90f, 600f);
                cb.ShowText(CiudadRecep);

                cb.SetTextMatrix(400f, 600f);
                cb.ShowText("Orden");
                cb.SetTextMatrix(490f, 600f);
                cb.ShowText(":");

                cb.SetTextMatrix(35f, 590f);
                cb.ShowText("Fono");
                cb.SetTextMatrix(85f, 590f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 590f);
                cb.ShowText(telefono);

                cb.SetTextMatrix(35f, 580f);
                cb.ShowText("Traslado");
                cb.SetTextMatrix(85f, 580f);
                cb.ShowText(":");

                cb.SetTextMatrix(90f, 580f);
                cb.ShowText("6=Otros traslado no venta");

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


                cb.SetTextMatrix(450, 540f);
                cb.ShowText("Cant.");

                cb.SetTextMatrix(480f, 540f);
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
                        cb.SetTextMatrix(35f, (float)(540 - Y));
                        cb.ShowText(detalle[di]);

                        cb.SetTextMatrix(430f, (float)(540 - Y));
                        dValor = System.Convert.ToDouble(cantidad[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 470f, (float)(540 - Y), 0);
                        dValor = System.Convert.ToDouble(preciounitario[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 500f, (float)(540 - Y), 0);
                        dValor = System.Convert.ToDouble(totalitem[di]);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, string.Format("{0:N0}", dValor), 575f, (float)(540 - Y), 0);

                        Y += 10;
                    }
                }

                // aqui va el timbre electronicodel TED

                //iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(@"C:\dte\dte52\TIMBRE\" + Folio + ".Png");

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
                cb.ShowText("verifique documento:www.sii.cl");



                cb.SetFontAndSize(baseFont, 8f);
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
                cb.ShowText("CEDIBLE CON SU FACTURA");

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
                doc.Close();
            	writer.Close();

			string pdfPath = string.Concat(new string[]
			{
				@"C:\dte\dte52\PDF\pdf_",
				tipo,
				"DTE",
				cnumerox.Trim(),
				".pdf"
			});
			Process.Start(pdfPath);
		

        }

        private void Envia_dte(string cn, string uri)
        {
            //CrSeedService semilla = new CrSeedService();

            string respuesta = semilla.getSeed();
			XmlDocument XMLdoc = new XmlDocument();
			XMLdoc.LoadXml(respuesta);
			XmlNodeList elemList = XMLdoc.GetElementsByTagName("SEMILLA");
            FirmarSeed(elemList[0].InnerXml, cn, uri, "93945000-9", "5816975-7");
			
        }
        private static string FirmarSeed(string seed, string cn, string ar, string a, string b)
        {
            string resultado = string.Empty;
            string body = string.Format("<getToken><item><Semilla>{0}</Semilla></item></getToken>", "00" + double.Parse(seed).ToString());
          
          
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
           token.GetTokenFromSeedService gt = new  token.GetTokenFromSeedService();
            string valorRespuesta = gt.getToken(signedSeed);
           
            XmlDocument doc2 = new XmlDocument();
            doc2.LoadXml(valorRespuesta);
            
            XmlNodeList elemList2 = doc2.GetElementsByTagName("TOKEN");
            string token = "";
            for (int i = 0; i < elemList2.Count; i++)
            {
                string token2 = elemList2[i].InnerXml;
                token = token2;
            }
          
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
            catch (System.Exception ex_4DC)
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
                        string nombreArchivos = @"C:\dte\dte52\XMLRESPUESTA\" + minom;
                        doc3.Save(nombreArchivos);
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

        public static string RemoveAllXmlNamespace(string xmlData)
        {
           string xmlnsPattern = "\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
           System.Text.RegularExpressions.MatchCollection matchCol = Regex.Matches(xmlData, xmlnsPattern);

            foreach (Match m in matchCol)
            {
                xmlData = xmlData.Replace(m.ToString(), "");
            }
            return xmlData;
        }


        private void NormalizarIzquierdaDTE(string uri)
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
        private string Timbrar_Ted(string uri)
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
            string Nombre = @"C:\dte\dte52\TIMBRE\" + Folio + ".png";
            Crea_Imagen_timbre_elctronico(sTodoTED,Nombre);
              
            
            if (TED != null)
            {
                sTED = TED.SelectSingleNode("DD").OuterXml;
            }
            sTED = sTED.Replace("\t", string.Empty);
            sTED = sTED.Replace("\r\n", string.Empty);
            sTED = sTED.Replace("\r", string.Empty);
            sTED = sTED.Replace("\n", string.Empty);

            //--------------------caf---------------------
            string Caf = @"c:\dte\dte52\CAF\52.xml";
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
            string frmt=Convert.ToBase64String(bytesSing);
            return frmt;

          
        }

        private void Crea_Imagen_timbre_elctronico(string sTodoTED,string Nombre)
        {
            Bitmap bm;
            bm =DTE33.BarCode.PDF417(sTodoTED, 1);
            bm.Save(Nombre, System.Drawing.Imaging.ImageFormat.Png);
        }
        public static void firmarDocumentoDTE(string uriDTE, string CN)
        {
            X509Certificate2 certificado =obtenerCertificado(CN);
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
        public static void ArmaEnvioDte(string uriCartula, string uri, string CN)
        {
            XmlDocument EnvioDte = new XmlDocument();
            EnvioDte.PreserveWhitespace = true;
            EnvioDte.Load(uriCartula);
            XmlDocument Dte = new XmlDocument();
            Dte.PreserveWhitespace = true;
            Dte.Load(uri);
            XmlNamespaceManager ns = new XmlNamespaceManager(EnvioDte.NameTable);
            XmlNamespaceManager ns2 = new XmlNamespaceManager(Dte.NameTable);
            ns.AddNamespace("sii", "http://www.sii.cl/SiiDte");
            ns2.AddNamespace("sii", "http://www.sii.cl/SiiDte");
            XmlElement node = (XmlElement)EnvioDte.ImportNode(Dte.DocumentElement, true);
            EnvioDte.SelectSingleNode("sii:EnvioDTE/sii:SetDTE", ns).AppendChild(node);
            string xpath = "sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:RUTRecep";
            string sRutReceptor = Dte.SelectSingleNode(xpath, ns2).InnerText;
            string xpath2 = "sii:EnvioDTE/sii:SetDTE/sii:Caratula/sii:TmstFirmaEnv";
            EnvioDte.SelectSingleNode(xpath2, ns).InnerText = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
       
            string newName = "{0}\\DTE_93945000-9_52_{1}.XML";

            string sPathxml = System.IO.Path.GetDirectoryName(uri);
            string sNamexml = System.IO.Path.GetFileNameWithoutExtension(uri);
            NameXml = string.Format(newName, sPathxml, sNamexml);
            DTE_ENVIO = NameXml;

            DTE_ENVIO_SEED = string.Format(newName, @"C:\dte\dte52\XMLSEMILLA", sNamexml);
            EnvioDte.Save(NameXml);
            EnvioDte.Save(DTE_ENVIO_SEED);
            string NameXml2 = NameXml;
            firmarEnvioDTE(NameXml2, CN );
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
            reference.Uri ="#SetDoc";
            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((System.Security.Cryptography.RSA)certificado.PrivateKey));
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            DTE.DocumentElement.AppendChild(DTE.ImportNode(xmlDigitalSignature, true));
            DTE.Save(uri);
        }
        private void Detalle_guia(string numerogui)
        {
            int linea = 1;

            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsDetalle = new DataSet();
                OleDbDataAdapter daDetalle = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT GUIAS.GUI_ORDEN,GUIAS.GUI_PATENT,GUIAS.GUI_CODPRO,GUIAS.GUI_CANTID,GUIAS.GUI_NUMOP,GUIAS.GUI_CODBOD";
                StrOledbDBFIV += ", GUIAS.GUI_metros,GUIAS.GUI_NOTA";
                StrOledbDBFIV += ",maepro.nombre";


                StrOledbDBFIV += " from " + Guia + " as GUIAS ";
                StrOledbDBFIV += " INNER JOIN  " + Producto + "  as maepro ";
                StrOledbDBFIV += " ON maepro.codcli = guias.CODCLI and ";
                StrOledbDBFIV += "  maepro.numero=guias.numero";
                StrOledbDBFIV += " where GUIAS.GUI_TIPMOV ='S'";
                StrOledbDBFIV += " and guias.gui_numero =" + numerogui;

                daDetalle = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daDetalle.Fill(dsDetalle);
                DTDetalle = dsDetalle.Tables[0];
                objconnDBFIV.Close();
                string xNombre;
                for (int i = 0; i < DTDetalle.Rows.Count; i++)
                {
                    xNombre = DTDetalle.Rows[i]["gui_codpro"].ToString().Trim() + " " + DTDetalle.Rows[i]["NOMBRE"].ToString().Trim();
                    xNombre = xNombre.Replace("&", "y");
                    xNombre = CambiaNombre.Nombre(xNombre);
                        xml += @"<Detalle>";
                        xml += "<NroLinDet>" + linea.ToString() + "</NroLinDet>";                     
                        xml += @"<NmbItem>" + xNombre+ "</NmbItem>";
                        xml += @"<QtyItem>" + DTDetalle.Rows[i]["GUI_CANTID"].ToString().Trim() + "</QtyItem>";
                        xml += @"<PrcItem>1</PrcItem>";
                        xml += @"<MontoItem>" + DTDetalle.Rows[i]["GUI_CANTID"].ToString().Trim() + "</MontoItem>";
                   xml += @"</Detalle>";
          
                    linea++;

                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void busca_bodeguero(string p)
        {
             OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsbodeguero = new DataSet();
                OleDbDataAdapter daBodeguero = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT nombre from tabbod";
                StrOledbDBFIV += " where tabbod.codbod=" + p;
              

                daBodeguero = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daBodeguero.Fill(dsbodeguero);
              
                objconnDBFIV.Close();
                if (dsbodeguero.Tables[0].Rows.Count > 0)
                {
                     Bodeguero = CambiaNombre.Nombre(dsbodeguero.Tables[0].Rows[0]["nombre"].ToString());
                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        } 
        private void busca_metros(string p)
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsNumope = new DataSet();
                OleDbDataAdapter daNumope = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT m2total from maeope";
                StrOledbDBFIV += " where maeope.numop=" + p;


                daNumope = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daNumope.Fill(dsNumope);
                MetrosSaldo = 0;
                MetrosMov = 0;
                MetrosPrevios =0;
                objconnDBFIV.Close();
                if (dsNumope.Tables[0].Rows.Count > 0)
                // esto por que la guia ya se encuentra grabada y actualkizado los metros
                {

                    if (!string.IsNullOrEmpty(dsNumope.Tables[0].Rows[0]["m2total"].ToString()))
                        MetrosSaldo = Convert.ToInt32(dsNumope.Tables[0].Rows[0]["m2total"].ToString());


                    if (!string.IsNullOrEmpty(DTDetalle.Rows[0]["gui_metros"].ToString()))
                        MetrosMov = Convert.ToInt32(DTDetalle.Rows[0]["gui_metros"].ToString());

                    MetrosPrevios = MetrosSaldo + MetrosMov;
                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        //--------------------------------------------------------------------
        private void Busca_Rut_Cliente(ref string codcli, out string rut)
        {
            rut = "";
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsCliente = new DataSet();
                OleDbDataAdapter daCliente = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT rut,dvrut FROM MAECLI ";
                StrOledbDBFIV += " where codcli=" + codcli;
                daCliente = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daCliente.Fill(dsCliente);

                if (dsCliente.Tables[0].Rows.Count > 0)
                {
                    rut = dsCliente.Tables[0].Rows[0]["Rut"].ToString() + "-" + dsCliente.Tables[0].Rows[0]["dvrut"].ToString();
                }
                objconnDBFIV.Close();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error de Selección de Registros DBF " + ex.Message);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if (rbS.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "c:\\leyton";
            }
            if (rbL.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "O:\\";
            }
          
          
            asigna_archivo_Guias(); 
            carga_Guias_combobox();
        }

        private void asigna_archivo_Guias()
        {
            string StrMes;//=Operaciones.DetalleFacturas.StrPeriodo.Substring(3, 2).Trim();
            string StrAno;//=Operaciones.DetalleFacturas.StrPeriodo.Substring(6, 4).Trim();
            int ano = 0;
            StrMes = dtpFecha.Value.Month.ToString();
            ano = dtpFecha.Value.Year;

            StrAno = ano.ToString();

            if (dtpFecha.Value.Month < 10)
            {
                StrMes = "0" + StrMes;
            }

            if ((ano - 2000) < 10)
            {
                StrAno = "0" + StrAno;
            }
            Guia = "MG" + StrAno + StrMes;
            Producto = "MP" + StrAno + StrMes;
        }

        private void carga_Guias_combobox()
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsGuias = new DataSet();
                OleDbDataAdapter daGuias = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT DISTINCT GUI_NUMERO FROM " + Guia + " AS GUIA" ;
                StrOledbDBFIV += " where GUIA.GUI_TIPMOV ='S'";
                StrOledbDBFIV += " and month(GUIA.GUI_FECHA)= " + dtpFecha.Value.Month.ToString();
                StrOledbDBFIV += " and year(GUIA.GUI_FECHA)= " + dtpFecha.Value.Year.ToString();
           
                StrOledbDBFIV += " order by GUIA.GUI_NUMERO desc ";

                daGuias = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daGuias.Fill(dsGuias);
                if (dsGuias.Tables[0].Rows.Count > 0)
                {
                    cboGuias.DisplayMember = "Gui_numero";
                    cboGuias.ValueMember = "Gui_numero";
                    cboGuias.DataSource = dsGuias.Tables[0].DefaultView;
                    cboGuias.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Seleccion de registros 0");

                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error de Selección de Registros DBF " + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GuiaDespacho_Load(object sender, EventArgs e)
        {
            {
               DTE33.Constantes_Variables.Unidad = "c:\\leyton";
            }
            if (rbL.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "O:\\";
            }
            asigna_archivo_Guias();
            carga_Guias_combobox();
            
        }
        private void btnEditaXml_Click(object sender, EventArgs e)
        {

        }
        // mysql
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {

               DTE33.Constantes_Variables.Unidad = "c:\\leyton";
                Fecha_hora_envio = DateTime.Now.Date.ToShortDateString() + "T" + DateTime.Now.ToLongTimeString();


                FechaEmi = dtpFecha.Value.Date;
                if (rbS.Checked == true)
                {
                   DTE33.Constantes_Variables.Unidad = "c:\\leyton";
                }
                if (rbL.Checked == true)
                {
                   DTE33.Constantes_Variables.Unidad = "O:\\";
                }

                asigna_archivo_Guias();
                Carga_Guias_my();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // para mysqlk
        private void Carga_Guias_my()
        {

            string Rut_caratula = "";
            string xCodcli = "";
            //bool Catatula = false;

            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            // fecha de la ultima actualizacion

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsGuias = new DataSet();
                OleDbDataAdapter daGuias = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT count(GUI.GUI_NUMERO) as GUIAS,cli.codcli FROM " + Guia + " as GUI INNER JOIN MAECLI ";
                StrOledbDBFIV += "as cli ON (GUI.CODCLI = cli.CODCLI )   ";
                StrOledbDBFIV += " where GUI.GUI_TIPMOV ='S' ";
                StrOledbDBFIV += " and GUI.GUI_NUMERO = " + cboGuias.SelectedValue.ToString();
                StrOledbDBFIV += " group by cli.codcli";
                daGuias = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daGuias.Fill(dsGuias);
                DTNguias = dsGuias.Tables[0];



                if (DTNguias.Rows.Count == 0)
                {
                    MessageBox.Show("Seleccion de registros 0");
                }
                objconnDBFIV.Close();


                //Catatula = true;
                for (int i = 0; i < DTNguias.Rows.Count; i++)
                {


                    xCodcli = DTNguias.Rows[i]["codcli"].ToString();
                    Busca_Rut_Cliente(ref xCodcli, out Rut_caratula);

                    GUIA_my(i);


                }

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error de Selección de Registros DBF " + ex.Message);
            }
        }
        private void GUIA_my(int indice)
        {

            string FechaEmision;
            string xNombre = "";

         
          
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsGuias = new DataSet();
                OleDbDataAdapter daGuias = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT top 1 * FROM " + Guia + " as GUIAS INNER JOIN MAECLI as MAECLI";
                StrOledbDBFIV += " ON (GUIAS.CODCLI = MAECLI.CODCLI )   ";
                StrOledbDBFIV += " where GUIAS.GUI_TIPMOV ='S'";
                StrOledbDBFIV += " and MAECLI.codcli =" + DTNguias.Rows[indice]["codcli"].ToString();
                StrOledbDBFIV += " and guias.gui_numero =" + cboGuias.SelectedValue.ToString();
                daGuias = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daGuias.Fill(dsGuias);
                DTGuias = dsGuias.Tables[0];
                // barra de progreso
                objconnDBFIV.Close();

                for (int j = 0; j < DTGuias.Rows.Count; j++)
                {

                    FechaEmision = DTGuias.Rows[j]["gui_fecha"].ToString().Trim().Substring(6, 4) + "-" +
                                  DTGuias.Rows[j]["gui_fecha"].ToString().Trim().Substring(3, 2) + "-" +
                                  DTGuias.Rows[j]["gui_fecha"].ToString().Trim().Substring(0, 2);
                    xNombre = DTGuias.Rows[j]["nombre"].ToString();
                    xNombre = xNombre.Replace("&", "y");
                    xNombre = CambiaNombre.Nombre(xNombre);
                    Folio = DTGuias.Rows[j]["GUI_NUMERO"].ToString();
                    Ecabezado_my(j,Folio);
                    Detalle_guia_my(Folio);



          



                    MessageBox.Show("XML Creado exitosamente");
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Ecabezado_my(int j,string Folio)
        {
            MySqlDataReader rdr = null;
            string xNombre = "";
            string strMysql;
            string xOrden;
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "www.sisalmadena.cl";
                builder.UserID = "csi26126_dte";
                builder.Password = "NSloteria2015";
                builder.Database = "csi26126_dte";
                MySqlConnection con = new MySqlConnection(builder.ToString());
                MySqlCommand cmdinsert = con.CreateCommand();

                FechaEmision = DTGuias.Rows[j]["Gui_fecha"].ToString().Trim().Substring(6, 4) + "-" +
                               DTGuias.Rows[j]["Gui_fecha"].ToString().Trim().Substring(3, 2) + "-" +
                               DTGuias.Rows[j]["Gui_fecha"].ToString().Trim().Substring(0, 2);
                xNombre = DTGuias.Rows[j]["nombre"].ToString();
                xNombre = CambiaNombre.Nombre(xNombre);
                strMysql  = "INSERT INTO documentosgv(numero,fecha,rut,razon,direccion,comuna,ciudad,region,giro,glosa,observacion";
                strMysql += ",total,tipodespacho,indtraslado) VALUES (";
                strMysql +=Folio;
                strMysql += ", '" + FechaEmision + "'";
                strMysql += ", '" + DTGuias.Rows[j]["rut"].ToString() + "-" + DTGuias.Rows[j]["dvrut"].ToString() + "'";
                largo_item = 100;
                if (xNombre.Length < 100)
                    largo_item = xNombre.Length;


                strMysql += ", '" + xNombre.Substring(0, largo_item) + "'";
                strMysql += ", '" + DTGuias.Rows[j]["domic"].ToString() + "'";
                strMysql += ", '" + DTGuias.Rows[j]["comuna"].ToString() + "'";
                strMysql += ", '" + DTGuias.Rows[j]["ciudad"].ToString() + "'";
                strMysql += ", '" + DTGuias.Rows[j]["region"].ToString() + "'";
                strMysql += ", '" + DTGuias.Rows[j]["giro"].ToString() + "'";
                // observacion y glosa
                xOrden = DTGuias.Rows[j]["gui_orden"].ToString();
                xOrden = xOrden.Replace("&", "y");
                xOrden = CambiaNombre.Nombre(xOrden);


                strMysql += ", '" + xOrden +"'";
                strMysql += ", '" + "'";
             
                strMysql += ", 0";
                strMysql += ", " + DTGuias.Rows[j]["tipodespa"].ToString() ;
                strMysql += ", " + DTGuias.Rows[j]["indtras"].ToString() ;


                strMysql += ")";
              
                cmdinsert.CommandText = strMysql;
                con.Open();
                cmdinsert.ExecuteNonQuery();

                string stm = "SELECT id from documentosgv order by id desc limit 1";
                MySqlCommand cmd = new MySqlCommand(stm, con);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    iidG = Convert.ToInt32(rdr.GetValue(0).ToString());
                }


                con.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        

        }
        private void Detalle_guia_my(string numerogui)
        {
            //int linea = 1;
            string xglosa = "";
            string gui_nota;
            string strMysql;
            string xunidad;
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            //mysql
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "www.sisalmadena.cl";

            builder.UserID = "csi26126_dte";
            builder.Password = "NSloteria2015";
            builder.Database = "csi26126_dte";
            MySqlConnection con = new MySqlConnection(builder.ToString());
            MySqlCommand cmdinsert = con.CreateCommand();

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsDetalle = new DataSet();
                OleDbDataAdapter daDetalle = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT GUIAS.GUI_ORDEN,GUIAS.GUI_PATENT,GUIAS.GUI_CODPRO,GUIAS.GUI_CANTID,GUIAS.GUI_NUMOP,GUIAS.GUI_CODBOD";
                StrOledbDBFIV += " , GUIAS.GUI_metros,GUIAS.GUI_NOTA";
                StrOledbDBFIV += " ,maepro.nombre,maepro.coduni";
                StrOledbDBFIV += " from " + Guia + " as GUIAS ";
                StrOledbDBFIV += " INNER JOIN  " + Producto + "  as maepro ";
                StrOledbDBFIV += " ON maepro.codcli = guias.CODCLI and maepro.numero=guias.numero";
                //StrOledbDBFIV += " INNER JOIN  tabuni as tabuni ";
                //StrOledbDBFIV += " ON maepro.coduni = tabuni.coduni ";
             


                StrOledbDBFIV += " where GUIAS.GUI_TIPMOV ='S'";
                StrOledbDBFIV += " and guias.gui_numero =" + numerogui;


                daDetalle = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daDetalle.Fill(dsDetalle);
                DTDetalle = dsDetalle.Tables[0];
                objconnDBFIV.Close();

             
                gui_nota = "";
                for (int i = 0; i < DTDetalle.Rows.Count; i++)
                {

                    gui_nota += DTDetalle.Rows[i]["GUI_nota"].ToString().Trim() + " ";
                    xglosa =  DTDetalle.Rows[i]["NOMBRE"].ToString().Trim();
                    xglosa = CambiaNombre.Nombre(xglosa);
                  

                    strMysql = "INSERT INTO detdocumentosgv(ide,codigo,producto,un,precio,cantidad,total)";
                    strMysql += " VALUES (";

                    strMysql += iidG.ToString();
                    strMysql +=", '" +  DTDetalle.Rows[i]["GUI_CODPRO"].ToString().Trim() + "'";

                    largo_item = 200;
                    if (xglosa.Length < 200)
                        largo_item = xglosa.Length;
                    strMysql += ",'" + xglosa.Substring(0, largo_item) + "'";
                    xunidad = buscar_unidad(DTDetalle.Rows[i]["coduni"].ToString().Trim());
                    strMysql += ",'" + xunidad+ "'"; // unidad
                    strMysql += ",0";
                    strMysql += "," +  DTDetalle.Rows[i]["GUI_CANTID"].ToString().Trim(); // cantidad
                    strMysql += ",0";
                    strMysql += ")";
                    cmdinsert.CommandText = strMysql;
                    con.Open();
                    cmdinsert.ExecuteNonQuery();
                    con.Close();
                }

                if (gui_nota.Length > 1)
                {
                 
                    xglosa =   gui_nota;
                    xglosa = CambiaNombre.Nombre(xglosa);


                    strMysql = "INSERT INTO detdocumentosgv(ide,codigo,producto,un,precio,cantidad,total)";
                    strMysql += " VALUES (";

                    strMysql += iidG.ToString();
                    strMysql += ",''" ;

                    largo_item = 200;
                    if (xglosa.Length < 200)
                        largo_item = xglosa.Length;
                    strMysql += ",'" + xglosa.Substring(0, largo_item) + "'";

                    strMysql += "," + "' '"; // unidad
                    strMysql += ",0";
                    strMysql += ",0";// cantidad
                    strMysql += ",0";
                    strMysql += ")";
                    cmdinsert.CommandText = strMysql;
                    con.Open();
                    cmdinsert.ExecuteNonQuery();
                    con.Close();

                }


            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private string buscar_unidad(string p)
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsunidad = new DataSet();
                OleDbDataAdapter daunidad = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT nombre from tabuni";
                StrOledbDBFIV += " where tabuni.coduni=" + p;


                daunidad = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daunidad.Fill(dsunidad);

                objconnDBFIV.Close();
                return dsunidad.Tables[0].Rows[0]["nombre"].ToString();
               
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
                return " ";
            }
        }
        private void busca_bodeguero_my(string p)
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsbodeguero = new DataSet();
                OleDbDataAdapter daBodeguero = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT nombre from tabbod";
                StrOledbDBFIV += " where tabbod.codbod=" + p;


                daBodeguero = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daBodeguero.Fill(dsbodeguero);

                objconnDBFIV.Close();
                if (dsbodeguero.Tables[0].Rows.Count > 0)
                {
                    Bodeguero = CambiaNombre.Nombre(dsbodeguero.Tables[0].Rows[0]["nombre"].ToString());
                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void busca_metros_my(string p)
        {
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;

            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsNumope = new DataSet();
                OleDbDataAdapter daNumope = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT m2total from maeope";
                StrOledbDBFIV += " where maeope.numop=" + p;


                daNumope = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daNumope.Fill(dsNumope);
                MetrosSaldo = 0;
                MetrosMov = 0;
                MetrosPrevios = 0;
                objconnDBFIV.Close();
                if (dsNumope.Tables[0].Rows.Count > 0)
                // esto por que la guia ya se encuentra grabada y actualkizado los metros
                {

                    if (!string.IsNullOrEmpty(dsNumope.Tables[0].Rows[0]["m2total"].ToString()))
                        MetrosSaldo = Convert.ToInt32(dsNumope.Tables[0].Rows[0]["m2total"].ToString());


                    if (!string.IsNullOrEmpty(DTDetalle.Rows[0]["gui_metros"].ToString()))
                        MetrosMov = Convert.ToInt32(DTDetalle.Rows[0]["gui_metros"].ToString());

                    MetrosPrevios = MetrosSaldo + MetrosMov;
                }
            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void GUIA_xml()
        {
            string xNombre = "";

            string xglosa;
            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsFacturas = new DataSet();
                OleDbDataAdapter daFacturas = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT * FROM CTACTE INNER JOIN MAECLI ";
                StrOledbDBFIV += " ON (CTACTE.CODCLI = MAECLI.CODCLI )   ";
                StrOledbDBFIV += " where ctacte.tipodoc =1 ";
                StrOledbDBFIV += " and month(ctacte.fecha)= " + dtpFecha.Value.Month.ToString();
                StrOledbDBFIV += " and year(ctacte.fecha)= " + dtpFecha.Value.Year.ToString();
                StrOledbDBFIV += " and day(ctacte.fecha)= " + dtpFecha.Value.Day.ToString();
                StrOledbDBFIV += " order by ctacte.numdoc ";

                //StrOledbDBFIV += " and maecli.codcli =" + DTCliente.Rows[indice]["codcli"].ToString();
                daFacturas = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daFacturas.Fill(dsFacturas);
                DTGuias = dsFacturas.Tables[0];
                // barra de progreso
                objconnDBFIV.Close();
                //progressBar1.Maximum = DTGuias.Rows.Count + 10;
                //progressBar1.Minimum = 0;
                //progressBar1.Show();
                //progressBar1.Value = 0;

                // crea archivo plano


                for (int j = 0; j < DTGuias.Rows.Count; j++)
                {
                    //archivo = @"c:\erp2\in\dte33\";
                    //archivo += @"xml_" + DTGuias.Rows[j]["numdoc"].ToString() + ".xml";
                    //StreamWriter XmlTexto = new StreamWriter(archivo);

                    //progressBar1.Visible = true;
                    //progressBar1.Value += 1;
                    //progressBar1.Refresh();

                    //xml = @"<?xml version='1.0' encoding='ISO-8859-1'?>";
                    //xml += @"<DTE version='1.0' >";

                    //xml += @"<Documento ID='xml_" + Folio + ".xml'>";

                    //xml += @"<Encabezado>";
                    //xml += @"<IdDoc>";
                    //xml += @"<TipoDTE>52</TipoDTE >";
                    //xml += @"<Folio>" + Folio + "</Folio>";
                    //xml += @"<FchEmis>" + FechaEmision + "</FchEmis>";
                    //xml += @"<TipoDespacho>2</TipoDespacho>";
                    //xml += @"<IndTraslado>6</IndTraslado>";
                    //xml += @"</IdDoc>";

                    //xml += @"<Emisor>";
                    //xml += @"<RUTEmisor>93945000-9</RUTEmisor>";
                    //xml += @"<RznSoc>ALMADENA,ALMACENES DE DEPOSITOS NACIONALES S.A.</RznSoc>";
                    //xml += @"<GiroEmis>ALMACENES GENERALES DE DEPOSITOS Y BODEGAJES</GiroEmis>";
                    //xml += @"<Acteco>630200</Acteco>";
                    ////"<CdgSIISucur>13101</CdgSIISucur>" +
                    //xml += @"<DirOrigen>MONEDA 812 OFICINA 705</DirOrigen>";
                    //xml += @"<CmnaOrigen>Santiago</CmnaOrigen>";
                    //xml += @"<CiudadOrigen>Santiago</CiudadOrigen>";
                    //xml += @"<CdgVendedor>0</CdgVendedor>";
                    //xml += @"</Emisor>";

                    //xml += @"<Receptor>";
                    //xml += @"<RUTRecep>" + DTGuias.Rows[j]["maecli.rut"].ToString() + "-" + DTGuias.Rows[j]["dvrut"].ToString() + "</RUTRecep>";
                    //int largo_item = 39;
                    //if (xNombre.Length < 39)
                    //    largo_item = xNombre.Length;

                    //xml += @"<RznSocRecep>" + xNombre.Substring(0, largo_item) + "</RznSocRecep>";
                    //xml += @"<GiroRecep>" + DTGuias.Rows[j]["giro"].ToString() + "</GiroRecep>";
                    //xml += @"<Contacto>NO</Contacto>";
                    //xml += @"<DirRecep>" + DTGuias.Rows[j]["domic"].ToString() + "</DirRecep>";
                    //xml += @"<CmnaRecep>" + DTGuias.Rows[j]["comuna"].ToString() + "</CmnaRecep>";
                    //xml += @"<CiudadRecep>" + DTGuias.Rows[j]["ciudad"].ToString() + "</CiudadRecep>";
                    //xml += @"</Receptor>";

                    //xml += @"<Totales>";
                    //xml += @"<MntTotal>0</MntTotal>";
                    //xml += @"</Totales>";
                    //xml += @"</Encabezado>";

                    //detalle de las liena de factura




                    // signature del dete
                    //xml += @"<Referencia>";
                    //xml += @"<NroLinRef>1</NroLinRef>";
                    //xml += @"<TpoDocRef>52</TpoDocRef>";
                    //xml += @"<IndGlobal>1</IndGlobal>";
                    //xml += @"<FolioRef>0</FolioRef>";
                    //xml += @"<FchRef>" + FechaEmision + "</FchRef>";

                    //xOrden = DTGuias.Rows[j]["gui_orden"].ToString();
                    //xOrden = xOrden.Replace("&", "y");
                    //xOrden = CambiaNombre.Nombre(xOrden);


                    //xml += @"<RazonRef>" + xOrden.Trim() + "</RazonRef>";
                    //xml += @"</Referencia>";

                    //xml += @"<TED version='1.0'>";
                    //xml += @"<DD>";
                    //xml += @"<RE>93945000-9</RE>";
                    //xml += @"<TD>52</TD>";
                    //xml += @"<F>" + Folio + "</F>";

                    //xml += @"<FE>" + DTGuias.Rows[j]["GUI_FECHA"].ToString().Trim() + "</FE>";
                    //xml += @"<RR>" + DTGuias.Rows[j]["rut"].ToString() + "-" + DTGuias.Rows[j]["dvrut"].ToString() + "</RR>";
                    //largo_item = 39;
                    //if (xNombre.Length < 39)
                    //    largo_item = xNombre.Length;

                    //xml += @"<RSR>" + xNombre.Substring(0, largo_item) + "</RSR>";
                    //xml += @"<MNT>0</MNT>";
                    //xNombreProducto = DTDetalle.Rows[0]["nombre"].ToString();
                    //xNombreProducto = xNombreProducto.Replace("&", "y");
                    //xNombreProducto = CambiaNombre.Nombre(xNombreProducto);


                    //xml += @"<IT1>" + xNombreProducto + "</IT1>";
                    //XmlDocument xDoc = new XmlDocument();

                    ////La ruta del documento XML permite rutas relativas 
                    ////respecto del ejecutable!

                    //xDoc.Load(@"c:/erp2/caf/caf52/caf52.xml");



                    //xml += @"<CAF version='1.0'>";
                    //xml += @"<DA>";
                    //xml += @"<RE></RE>";
                    //xml += @"<RS></RS>";
                    //xml += @"<TD></TD>";
                    //// AQUI PARA EL JUEVES
                    //xml += @"<RNG><D></D><H></H></RNG>";
                    //xml += @"<FA></FA>";
                    //xml += @"<RSAPK><M></M><E></E></RSAPK>";
                    //xml += @"<IDK></IDK>";
                    //xml += @"</DA>";
                    //xml += @"<FIRMA algoritmo='SHA1withRSA'></FIRMA>";

                    //xml += @"</CAF>";



                    //xml += @"<TSTED/>";
                    //xml += @"</DD>";
                    //xml += @"<FRMT algoritmo='SHA1withRSA'></FRMT>";
                    //xml += @"</TED>";
                    //xml += @"<TmstFirma/>";
                    ////
                    //busca_metros(DTDetalle.Rows[0]["gui_numop"].ToString().Trim());
                    //busca_bodeguero(DTDetalle.Rows[0]["gui_codbod"].ToString().Trim());

                    //xml += @"</Documento>";
                    ////xml += @"<Adicional>";
                    ////xml += @"<NodosA>";
                    ////xml += @"<A1>Camion Patente :" + DTDetalle.Rows[0]["gui_patent"].ToString().Trim() + "</A1>";

                    ////         xOrden = DTGuias.Rows[0]["gui_orden"].ToString();

                    ////         xOrden  = CambiaNombre.Nombre(xOrden);

                    ////xml += @"<A2>Orden:" + xOrden.Trim() + "</A2>";

                    ////xml += @"<A3>Despachado por:" + Bodeguero.Trim() + " </A3>";

                    ////xml += @"<A4>Metros Previos:" + MetrosPrevios.ToString() +" </A4>";
                    ////xml += @"<A5>Metros Despachados:" + MetrosMov.ToString() + " </A5>";
                    ////xml += @"<A6>Metros Saldo:" + MetrosSaldo.ToString() + " </A6>";


                    ////xNota = DTGuias.Rows[DTGuias.Rows.Count -1]["gui_orden"].ToString();
                    ////xNota= xNota.Replace("&", "y");
                    ////xNota  = CambiaNombre.Nombre(xNota);


                    ////xml += @"<A7>Nota:" +xNota.Trim() + " </A7>";


                    ////xml += @"</NodosA>";
                    ////xml += @"</Adicional>";


                    //xml += @"</DTE>";

                    //var xmldoc = new XmlDocument();


                    //var archivo = @"c:\erp2\in\dte52\";
                    //archivo += @"xml_" + Folio + ".xml";

                    //xmldoc.LoadXml(xml);

                    //xmldoc.Save(archivo);

                    //System.IO.StreamWriter file = new System.IO.StreamWriter(archivo);
                    //file.WriteLine(xml);
                    //file.Close();

                    //detalle de las liena de factura
                    //detalle_my(DTGuias.Rows[j]["numdoc"].ToString().Trim());



                }

            }

            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message + " " + xml);
            }
        }
        private void detalle_my(string numdoc)
        {

            OleDbConnection objconnDBFIV = new OleDbConnection();
            OleDbCommand OldbComando = new OleDbCommand();
            string StrOledbDBFIV;
            string xglosa = "";
            string xcomision = "0";
            string strMysql;
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "www.sisalmadena.cl";

            builder.UserID = "csi26126_dte";
            builder.Password = "NSloteria2015";
            builder.Database = "csi26126_dte";
            MySqlConnection con = new MySqlConnection(builder.ToString());
            MySqlCommand cmdinsert = con.CreateCommand();
            // coneccion a tabla dbase
            objconnDBFIV.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                              "Data Source=" + Constantes_Variables.Unidad + ";Extended Properties=dBASE IV;" +
                                                                  "User ID=admin;Password=";
            try
            {
                objconnDBFIV.Open();
                //abre tabla dbf 
                DataSet dsDetalle = new DataSet();
                OleDbDataAdapter daDetalle = new OleDbDataAdapter();
                StrOledbDBFIV = "SELECT glosa,comision FROM " + Guia;
                StrOledbDBFIV += " where numfactu=" + numdoc;
                daDetalle = new OleDbDataAdapter(StrOledbDBFIV, objconnDBFIV);
                daDetalle.Fill(dsDetalle);
                DTDetalle = dsDetalle.Tables[0];
                objconnDBFIV.Close();
                for (int i = 0; i < DTDetalle.Rows.Count; i++)
                {

                    xcomision = DTDetalle.Rows[i]["comision"].ToString().Trim();
                    xglosa = DTDetalle.Rows[i]["glosa"].ToString().Trim();
                    xglosa = CambiaNombre.Nombre(xglosa);
  
                    strMysql = "INSERT INTO detdocumentos(ide,codigo,producto,un,precio,cantidad,total)";
                    strMysql += " VALUES (";

                    strMysql += iidG.ToString();
                    strMysql += ",33";

                    largo_item = 200;
                    if (xglosa.Length < 200)
                        largo_item = xglosa.Length;
                    strMysql += ",'" + xglosa.Substring(0, largo_item) + "'";
                    strMysql += "," + "' '"; // unidad
                    strMysql += "," + xcomision;
                    strMysql += ",1"; // cantidad
                    strMysql += "," + xcomision;
                    strMysql += ")";
                    cmdinsert.CommandText = strMysql;
                    con.Open();
                    cmdinsert.ExecuteNonQuery();
                    con.Close();



                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Carga_clientes_Mysql()
        {
           //DTE_Mysql();
        }

        private void rbL_CheckedChanged(object sender, EventArgs e)
        {
            if (rbS.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "c:\\leyton";
            }
            if (rbL.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "O:\\";
            }


            asigna_archivo_Guias();
            carga_Guias_combobox();
        }

        private void rbS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbS.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "c:\\leyton";
            }
            if (rbL.Checked == true)
            {
               DTE33.Constantes_Variables.Unidad = "O:\\";
            }


            asigna_archivo_Guias();
            carga_Guias_combobox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String atributo, nombre;
            atributo = String.Empty;
            nombre = String.Empty;
            string seed = "";
            var xmldoc = new XmlDocument();
            var archivo = @"c:\erp2\in\dte61\";
            string Sxml;

            try
            {
                //obtiene la semilla del webservice palena
                CrSeedService semilla = new CrSeedService();
                string respuesta = semilla.getSeed();


                textBox1.Text = respuesta;
                //MessageBox.Show(respuesta);

                archivo = "semilla.xml";
                xmldoc.LoadXml(respuesta);
                xmldoc.Save(archivo);

                XmlNodeList elemList = xmldoc.GetElementsByTagName("SEMILLA");
                for (int i = 0; i < elemList.Count; i++)
                {
                    seed = elemList[i].InnerXml.ToString();
                }


                //Sxml = FirmarSeed(seed, "Nicolas Gastosn");
                ////
                //// Suponiendo que el objeto XmlDocument ( XMLDOM ) contenga 
                //// la semilla firmada, esta debería ser la forma de recuperar
                //// el valor string.
                //string signedSeed = Sxml;
                //MessageBox.Show(Sxml);
                //textBox1.Text = Sxml;
                //////
                ////// Luego asigne el valor al metodo GetToken()
                //token.GetTokenFromSeedService gt = new token.GetTokenFromSeedService();
                //string valorRespuesta = gt.getToken(Sxml);



                //textBox1.Text = valorRespuesta;

                //MessageBox.Show(valorRespuesta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        // de aqui a bajo es del blog
        //public static X509Certificate2 RecuperarCertificado(string CN)
        //{
        //    X509Certificate2 certificado = null;
        //    X509Certificate2 result;
        //    if (string.IsNullOrEmpty(CN) || CN.Length == 0)
        //    {
        //        result = null;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //            store.Open(OpenFlags.ReadOnly);
        //            X509Certificate2Collection Certificados = store.Certificates;
        //            X509Certificate2Collection Certificados2 = Certificados.Find(X509FindType.FindByTimeValid, System.DateTime.Now, false);
        //            X509Certificate2Collection Certificados3 = Certificados2.Find(X509FindType.FindBySubjectName, CN, false);
        //            if (Certificados3 != null && Certificados3.Count != 0)
        //            {
        //                certificado = Certificados3[0];
        //            }
        //            store.Close();
        //        }
        //        catch (System.Exception)
        //        {
        //            certificado = null;
        //        }
        //        result = certificado;
        //    }
        //    return result;
        //}



        ///// <summary>
        ///// Firma la semilla para poder validarla en el SII
        ///// </summary>
        //private static string FirmarSemilla(string seed, string cn)
        //{

        //    ////
        //    //// Construya el cuerpo del documento en formato string.
        //    // se saco resultado para ponerlo publico


        //    string resultado = string.Empty;
        //    string body = string.Format("<gettoken><item><Semilla>{0}</Semilla></item></gettoken>", double.Parse(seed).ToString());

        //    ////
        //    //// Recuperar el certificado para firmar el documento.
        //    //// utilizando el nombre del propietario del certificado o CN
        //    X509Certificate2 certificado = obtenerCertificado(cn);

        //    ////
        //    //// Firme la semilla.
        //    try
        //    {
        //        resultado = firmarDocumentoSemilla(body, certificado);

        //    }
        //    catch (Exception)
        //    {
        //        resultado = string.Empty;
        //    }


        //    ////
        //    //// Regrese el valor de retorno
        //    return resultado;


        //}


        /// <summary>
        /// Recupera un determinado certificado para poder firmar un documento
        /// </summary>
        /// <param name="CN">Nombre del certificado que se busca
        /// <returns>X509Certificate2</returns>
        //public static X509Certificate2 obtenerCertificado(string CN)
        //{

        //    ////
        //    //// Respuesta
        //    X509Certificate2 certificado = null;

        //    ////
        //    //// Certificado que se esta buscando
        //    if (string.IsNullOrEmpty(CN) || CN.Length == 0)
        //        return certificado;

        //    ////
        //    //// Inicie la busqueda del certificado
        //    try
        //    {

        //        ////
        //        //// Abra el repositorio de certificados para buscar el indicado
        //        X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //        store.Open(OpenFlags.ReadOnly);
        //        X509Certificate2Collection Certificados1 = (X509Certificate2Collection)store.Certificates;
        //        X509Certificate2Collection Certificados2 = Certificados1.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
        //        X509Certificate2Collection Certificados3 = Certificados2.Find(X509FindType.FindBySubjectName, CN, false);

        //        ////
        //        //// Si hay certificado disponible envíe el primero
        //        if (Certificados3 != null && Certificados3.Count != 0)
        //            certificado = Certificados3[0];

        //        ////
        //        //// Cierre el almacen de sertificados
        //        store.Close();


        //    }
        //    catch (Exception)
        //    {
        //        certificado = null;
        //    }


        //    ////
        //    //// Regrese el valor de retorno 
        //    return certificado;

        //}

        public static string firmarDocumentoSemilla(string documento, X509Certificate2 certificado)
        {

            ////
            //// Cree un nuevo documento xml y defina sus caracteristicas
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.LoadXml(documento);

            ////
            //// Cree el objeto XMLSignature.
            //System.Security.Cryptography.X509Certificates.X509Certificate2 signedXml = new X509Certificate2(doc);
            SignedXml signedXml = new SignedXml(doc);

            ////
            //// Agregue la clave privada al objeto xmlSignature.
            signedXml.SigningKey = certificado.PrivateKey;

            ////
            //// Obtenga el objeto signature desde el objeto SignedXml.
            Signature XMLSignature = signedXml.Signature;

            ////
            //// Cree una referencia al documento que va a firmarse
            //// si la referencia es "" se firmara todo el documento
            Reference reference = new Reference("");

            ////
            //// Representa la transformación de firma con doble cifrado para una firma XML  digital que define W3C.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            ////
            //// Agregue el objeto referenciado al obeto firma.
            XMLSignature.SignedInfo.AddReference(reference);

            ////
            //// Agregue RSAKeyValue KeyInfo  ( requerido para el SII ).
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)certificado.PrivateKey));

            ////
            //// Agregar información del certificado x509
            keyInfo.AddClause(new KeyInfoX509Data(certificado));

            //// 
            //// Agregar KeyInfo al objeto Signature 
            XMLSignature.KeyInfo = keyInfo;

            ////
            //// Cree la firma
            signedXml.ComputeSignature();

            ////
            //// Recupere la representacion xml de la firma
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            ////
            //// Agregue la representacion xml de la firma al documento xml
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            ////
            //// Limpie el documento xml de la declaracion xml ( Opcional, pera para nuestro proceso es valido  )
            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }

            ////
            //// Regrese el valor de retorno
            return doc.InnerXml;


        }



        #region FIRMA DTE

        public static void firmarDocumentoXml(ref XmlDocument xmldocument, X509Certificate2 certificado, string referenciaUri)
        {
            ////
            //// Cree el objeto SignedXml donde xmldocument
            //// representa el documento DTE preparado para
            //// ser firmado. Recuerde que debe ser abierto 
            //// con la propiedad PreserveWhiteSpace = true
            SignedXml signedXml = new SignedXml(xmldocument);

            ////
            //// Agregue la clave privada al objeto signedXml
            signedXml.SigningKey = certificado.PrivateKey;

            ////
            //// Recupere el objeto signature desde signedXml
            Signature XMLSignature = signedXml.Signature;

            ////
            //// Cree la refrerencia al documento DTE
            //// recuerde que la referencia tiene el 
            //// formato '#reference'
            //// ejemplo '#DTE001'
            Reference reference = new Reference();
            reference.Uri = referenciaUri;

            ////
            //// Agregue la referencia al objeto signature
            XMLSignature.SignedInfo.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)certificado.PrivateKey));

            ////
            //// Agregar información del certificado x509
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            XMLSignature.KeyInfo = keyInfo;

            ////
            //// Calcule la firma y recupere la representacion
            //// de la firma en un objeto xmlElement
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            ////
            //// Inserte la firma en el documento DTE
            xmldocument.DocumentElement.AppendChild(xmldocument.ImportNode(xmlDigitalSignature, true));

        }



        #endregion

        //////////////////////////////////////////////////////////////////////
        //// BY: Marcelo Rojas R.
        //// Dt: 16-05-2013
        //// El ejercicio actual representa un ejemplo del SII
        //// Donde se suministra el valor del nodo TED, es decir 
        //// su contenido y posteriormente se calcula el timbre
        //////////////////////////////////////////////////////////////////////
        public static void PruebaTimbreDD()
        {

            ////
            //// Contenido del nodo TED del ejemplo. 
            //// Este es el formato que debe tener los datos
            //// 
            string DD = string.Empty;
            DD += "<DD><RE>97975000-5</RE><TD>33</TD><F>27</F><FE>2003-09-08</FE>";
            DD += "<RR>8414240-9</RR><RSR>JORGE GONZALEZ LTDA</RSR><MNT>502946</M";
            DD += "NT><IT1>Cajon AFECTO</IT1><CAF version=\"1.0\"><DA><RE>97975000-";
            DD += "5</RE><RS>RUT DE PRUEBA</RS><TD>33</TD><RNG><D>1</D><H>200</H>";
            DD += "</RNG><FA>2003-09-04</FA><RSAPK><M>0a4O6Kbx8Qj3K4iWSP4w7KneZYe";
            DD += "J+g/prihYtIEolKt3cykSxl1zO8vSXu397QhTmsX7SBEudTUx++2zDXBhZw==<";
            DD += "/M><E>Aw==</E></RSAPK><IDK>100</IDK></DA><FRMA algoritmo=\"SHA1";
            DD += "withRSA\">g1AQX0sy8NJugX52k2hTJEZAE9Cuul6pqYBdFxj1N17umW7zG/hAa";
            DD += "vCALKByHzdYAfZ3LhGTXCai5zNxOo4lDQ==</FRMA></CAF><TSTED>2003-09";
            DD += "-08T12:28:31</TSTED></DD>";

            ////
            //// Representa la clave privada rescatada desde el CAF que envía el SII
            //// para la prueba propuesta por ellos.
            ////
            string pk = string.Empty;
            pk += "MIIBOwIBAAJBANGuDuim8fEI9yuIlkj+MOyp3mWHifoP6a4oWLSBKJSrd3MpEsZd";
            pk += "czvL0l7t/e0IU5rF+0gRLnU1Mfvtsw1wYWcCAQMCQQCLyV9FxKFLW09yWw7bVCCd";
            pk += "xpRDr7FRX/EexZB4VhsNxm/vtJfDZyYle0Lfy42LlcsXxPm1w6Q6NnjuW+AeBy67";
            pk += "AiEA7iMi5q5xjswqq+49RP55o//jqdZL/pC9rdnUKxsNRMMCIQDhaHdIctErN2hC";
            pk += "IP9knS3+9zra4R+5jSXOvI+3xVhWjQIhAJ7CF0R0S7SIHHKe04NUURf/7RvkMqm1";
            pk += "08k74sdnXi3XAiEAlkWk2vc2HM+a1sCqQxNz/098ketqe7NuidMKeoOQObMCIQCk";
            pk += "FAMS9IcPcMjk7zI2r/4EEW63PSXyN7MFAX7TYe25mw==";


            //// 
            //// Este es el resultado que el SII indica debe obtenerse despues de crear
            //// el timbre sobre los datos expuestos.
            ////
            const string HTIMBRE = "pqjXHHQLJmyFPMRvxScN7tYHvIsty0pqL2LLYaG43jMmnfiZfllLA0wb32lP+HBJ/tf8nziSeorvjlx410ZImw==";


            //// //////////////////////////////////////////////////////////////////
            //// Generar timbre sobre los datos del tag DD utilizando la clave 
            //// privada suministrada por el SII en el archivo CAF
            //// //////////////////////////////////////////////////////////////////

            ////
            //// Calcule el hash de los datos a firmar DD
            //// transformando la cadena DD a arreglo de bytes, luego con
            //// el objeto 'SHA1CryptoServiceProvider' creamos el Hash del
            //// arreglo de bytes que representa los datos del DD
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] bytesStrDD = ByteConverter.GetBytes(DD);
            byte[] HashValue = new SHA1CryptoServiceProvider().ComputeHash(bytesStrDD);

            ////
            //// Cree el objeto Rsa para poder firmar el hashValue creado
            //// en el punto anterior. La clase FuncionesComunes.crearRsaDesdePEM()
            //// Transforma la llave rivada del CAF en formato PEM a el objeto
            //// Rsa necesario para la firma.
            RSACryptoServiceProvider rsa = FuncionesComunes.crearRsaDesdePEM(pk);

            ////
            //// Firme el HashValue ( arreglo de bytes representativo de DD )
            //// utilizando el formato de firma SHA1, lo cual regresará un nuevo 
            //// arreglo de bytes.
            byte[] bytesSing = rsa.SignHash(HashValue, "SHA1");

            ////
            //// Recupere la representación en base 64 de la firma, es decir de
            //// el arreglo de bytes 
            string FRMT1 = Convert.ToBase64String(bytesSing);

            ////
            //// Comprobación del timbre generado por nuestra rutina contra el
            //// valor 
            if (HTIMBRE.Equals(FRMT1))
            {
                Console.WriteLine("Comprobacion OK");
            }
            else
            {
                Console.WriteLine("Comprobacion NOK");
            }

        }
        //#region CrSeedService

        //////
        ////// Crear instancia  
        //CrSeedService maullin = new CrSeedService();
        //string respuesta = maullin.getSeed();


        //#endregion


        //#region Recuperar TOKEN

        //////
        ////// Suponiendo que el objeto XmlDocument ( XMLDOM ) contenga 
        ////// la semilla firmada, esta debería ser la forma de recuperar
        ////// el valor string.
        //string signedSeed = XmlDocument.InnerXml;

        //////
        ////// Luego asigne el valor al metodo GetToken()
        //Proxys.Produccion.GetTokenFromSeedService gt = new Proxys.Produccion.GetTokenFromSeedService();
        //string valorRespuesta = gt.getToken(signedSeed);


        //#endregion



        public static string NameXml { get; set; }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string xml = @"C:\dte\dte52\XMLSII\DTE_93945000-9_52_194.XML";
            //generarPdf417(xml);
        }

        //private void generarPdf417(string uri)
        //{
        //    //XmlNamespaceManager ns = new XmlNamespaceManager(DTE.NameTable);
        //    //ns.AddNamespace("sii", "http://www.sii.cl/SiiDte");
        //    //XmlElement ID = (XmlElement)DTE.SelectSingleNode("//sii:Documento", ns);




        //    string xpathTED = "//sii:TED";
        //    string sTED = string.Empty;
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(uri);
        //    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
        //    namespaceManager.AddNamespace("sii", "http://www.sii.cl/SiiDte");
        



        //    XmlElement xTED = (XmlElement)xmlDoc.SelectSingleNode(xpathTED, namespaceManager);
        //    if (xTED != null)
        //    {
        //        sTED = xTED.InnerXml;
        //    }


        //    string sNamePdf417 = "R{0}T{1}F{2}.PNG";
        //    sNamePdf417 = string.Format(sNamePdf417, "939450009", "52", Folio);
        //    STROKESCRIBECLSLib.StrokeScribeClass ss = (STROKESCRIBECLSLib.StrokeScribeClass)System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(new System.Guid("7E42B8C5-73BE-4806-8904-FF4080A6961C")));
        //    ss.Alphabet = STROKESCRIBECLSLib.enumAlphabet.PDF417;
        //    ss.Text =sTED;

        //    ss.PDF417ErrLevel = 8;
        //    ss.PDF417SymbolAspectRatio = 5f;
        //    int w = ss.BitmapW;
        //    int h = ss.BitmapH;
        //    ss.SavePicture(sNamePdf417, STROKESCRIBECLSLib.enumFormats.GIF, w, h, 0);
        //    if (ss.Error != 0)
        //    {
        //        System.Console.WriteLine(ss.ErrorDescription);
        //    }


        //    BarcodeLib.Barcode.PDF417 barcode = new BarcodeLib.Barcode.PDF417();
        //    barcode.Data = sTED;

        //    //barcode.UOM = UnitOfMeasure.PIXEL;
        //    barcode.BarWidth = 2;
        //    barcode.BarRatio = 0.3333333f;
        //    barcode.LeftMargin = 0;
        //    barcode.RightMargin = 0;
        //    barcode.TopMargin = 0;
        //    barcode.BottomMargin = 0;

        //    barcode.Rows = 3;
        //    barcode.Columns = 18;


        //    barcode.Encoding = BarcodeLib.Barcode.PDF417Encoding.Text;

        //    barcode.ECL = BarcodeLib.Barcode.PDF417ErrorCorrectionLevel.Level_5;

        //    barcode.Compact = false;

        //    barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

        //    // more barcode settings here

        //    // save barcode image into your system
        //    barcode.drawBarcode(sNamePdf417);
        //    sNamePdf417.Substring(4);
        //}
    }
}
