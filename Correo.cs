using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace DTE33
{
    class Correo :IDisposable
    {
      MailMessage  correos= new MailMessage();
      SmtpClient envios = new SmtpClient();
      //static bool mailSent = false; 
    
        
      //  private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
      //  {
      //      // Get the unique identifier for this asynchronous operation.
      //       String token = (string) e.UserState;
           
      //      if (e.Cancelled)
      //      {
      //           Console.WriteLine("[{0}] Send canceled.", token);
      //      }
      //      if (e.Error != null)
      //      {
      //           Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
      //      } else
      //      {
      //          Console.WriteLine("Message sent.");
      //      }
      //      mailSent = true;
      //  }

      public void enviarCorreo(string emisor, string password, string mensaje, string asunto, string destinatario, string ruta)
      {
          try
          {
            correos.To.Clear();
            correos.Body = "";
            correos.Subject = "";
            correos.Body = mensaje;
            correos.Subject = asunto;
            correos.IsBodyHtml = true;
            correos.To.Add(destinatario.Trim());

            if(ruta.Equals("")==false)
            {
              System.Net.Mail.Attachment archivo = new System.Net.Mail.Attachment(ruta);
               
              correos.Attachments.Add(archivo);
            }

            correos.From = new MailAddress(emisor);
            envios.Credentials = new NetworkCredential(emisor, password);

            //Datos importantes no modificables para tener acceso a las cuentas

            envios.Host = "mail.almadena.cl";
            envios.Port = 25;
            //envios.EnableSsl = false;       

            envios.Send(correos);
            envios.Dispose();
            MessageBox.Show("El mensaje fue enviado correctamente");
          }
          catch(Exception ex)
          {
              MessageBox.Show(ex.Message, "No se envio el correo correctamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
               correos.Dispose();
                envios.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void enviarCorreo_pdf(string emisor, string password, string mensaje, string asunto, string destinatario, string destinatario1, string destinatario2, string destinatario3, string destinatario4, string ruta)
      {
          try
          {
              correos.To.Clear();
             
             
              correos.Body = mensaje;
              correos.Subject = asunto;
              correos.IsBodyHtml = true;
              correos.To.Add(destinatario.Trim());
            


              if (destinatario1.Trim().Length > 0)
              correos.To.Add(destinatario1.Trim());

              if (destinatario2.Trim().Length > 0)
              correos.To.Add(destinatario2.Trim());

              if (destinatario3.Trim().Length > 0)
                  correos.To.Add(destinatario3.Trim());

              if (destinatario4.Trim().Length > 0)
                  correos.To.Add(destinatario4.Trim());

              if (ruta.Equals("") == false)
              {
                  System.Net.Mail.Attachment archivo = new System.Net.Mail.Attachment(ruta);
                  correos.Attachments.Add(archivo);
              }

              correos.From = new MailAddress(emisor);
              envios.Credentials = new NetworkCredential(emisor, password);

              //Datos importantes no modificables para tener acceso a las cuentas

              envios.Host = "mail.almadena.cl";
              envios.Port = 25;
              //envios.EnableSsl = false;       

              envios.Send(correos);
              envios.Dispose();
              MessageBox.Show("El PDF fue enviado a " + destinatario.Trim() + " " + destinatario1.Trim() + " " + destinatario2.Trim() );
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message, "No se envio el correo correctamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
    }
}
