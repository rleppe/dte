using System;
using System.Collections.Generic;
using System.Text;


namespace DTE33
{
    public class CambiaNombre
    {
        public static string Nombre(string nombre)
        {
            Encoding utf8 = Encoding.UTF8;

            Encoding Out = Encoding.GetEncoding("iso-8859-1");

            //string intermediate = encodingIn.GetString(input);
            //byte[] result = encodingOut.GetBytes(intermediate);

              nombre = nombre.Replace("&", "&amp;");
              nombre = nombre.Replace("'", "&apos;");
              nombre = nombre.Replace("\"", "&quot;");
              nombre = nombre.Replace(">", "&gt;");
              nombre = nombre.Replace("<", "&lt;");
            // acentos y eñe
              //nombre = nombre.Replace("Ñ", "&Ntilde;");
              //nombre = nombre.Replace("ñ", "&ntilde;");

              //nombre = nombre.Replace("Ñ", "&#209;"); por el hue que no controla la ñe Nelson estuardo
              //nombre = nombre.Replace("ñ", "&#241;");
              nombre = nombre.Replace("Ñ", "N");
              nombre = nombre.Replace("ñ", "n");

              nombre = nombre.Replace(Convert.ToChar(220).ToString(), "&Uuml;");
              nombre = nombre.Replace(Convert.ToChar(252).ToString(), "&uuml;");

              nombre = nombre.Replace("á", "a");
              nombre = nombre.Replace("é", "e");
              nombre = nombre.Replace("í", "i");
              nombre = nombre.Replace("ó", "o"); 
              nombre = nombre.Replace("ú", "u");

              nombre = nombre.Replace("Á", "A");
              nombre = nombre.Replace("É", "E");
              nombre = nombre.Replace("Í", "I");
              nombre = nombre.Replace("Ó", "O");
              nombre = nombre.Replace("Ú", "U");
              //nombre = nombre.Replace(Convert.ToChar(186).ToString(), "&ordm;");
              nombre = nombre.Replace(Convert.ToChar(186).ToString(), " ");
              nombre = nombre.Replace(Convert.ToChar(170).ToString(), "&#170;");
              nombre = nombre.Replace(Convert.ToChar(176).ToString(), "&#176;");
              nombre = nombre.Replace(Convert.ToChar(167).ToString(), "&#167;");
              nombre = nombre.Replace(Convert.ToChar(166).ToString(), "&#166;");
              nombre = nombre.Replace("/", "-");
              return nombre;
        }

    }



}
