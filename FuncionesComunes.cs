using System;
using System.Security.Cryptography;
using System.IO;

namespace DTE33
{
    public class FuncionesComunes
    {

        static bool verbose = false;



        public static RSACryptoServiceProvider crearRsaDesdePEM(string base64)
        {


            ////

            //// Extraiga de la cadena los header y footer

            base64 = base64.Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty);

            base64 = base64.Replace("-----END RSA PRIVATE KEY-----", string.Empty);


            ////

            //// el resultado que se encuentra en base 64 cambielo a

            //// resultado string

            byte[] arrPK = Convert.FromBase64String(base64);



            ////

            //// obtenga el Rsa object a partir de

            return DecodeRSAPrivateKey(arrPK);



        }


        public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {

            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;


            // --------- Set up stream to decode the asn.1 encoded RSA private key ------

            MemoryStream mem = new MemoryStream(privkey);

            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading

            byte bt = 0;

            ushort twobytes = 0;

            int elems = 0;

            try
            {

                twobytes = binr.ReadUInt16();

                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)

                    binr.ReadByte();    //advance 1 byte

                else if (twobytes == 0x8230)

                    binr.ReadInt16();    //advance 2 bytes

                else

                    return null;


                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0102) //version number

                    return null;

                bt = binr.ReadByte();

                if (bt != 0x00)

                    return null;



                //------ all private key components are Integer sequences ----

                elems = GetIntegerSize(binr);

                MODULUS = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                E = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                D = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                P = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                Q = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                DP = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                DQ = binr.ReadBytes(elems);


                elems = GetIntegerSize(binr);

                IQ = binr.ReadBytes(elems);


                Console.WriteLine("showing components ..");

                if (verbose)
                {

                    showBytes("\nModulus", MODULUS);

                    showBytes("\nExponent", E);

                    showBytes("\nD", D);

                    showBytes("\nP", P);

                    showBytes("\nQ", Q);

                    showBytes("\nDP", DP);

                    showBytes("\nDQ", DQ);

                    showBytes("\nIQ", IQ);

                }


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----

                CspParameters CspParameters = new CspParameters();

                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024, CspParameters);

                RSAParameters RSAparams = new RSAParameters();

                RSAparams.Modulus = MODULUS;

                RSAparams.Exponent = E;

                RSAparams.D = D;

                RSAparams.P = P;

                RSAparams.Q = Q;

                RSAparams.DP = DP;

                RSAparams.DQ = DQ;

                RSAparams.InverseQ = IQ;

                RSA.ImportParameters(RSAparams);

                return RSA;

            }

            catch (Exception ex)
            {

                return null;

            }

            finally
            {

                binr.Close();

            }

        }


        private static int GetIntegerSize(BinaryReader binr)
        {

            byte bt = 0;

            byte lowbyte = 0x00;

            byte highbyte = 0x00;

            int count = 0;

            bt = binr.ReadByte();

            if (bt != 0x02)        //expect integer

                return 0;

            bt = binr.ReadByte();


            if (bt == 0x81)

                count = binr.ReadByte();    // data size in next byte

            else

                if (bt == 0x82)
                {

                    highbyte = binr.ReadByte();    // data size in next 2 bytes

                    lowbyte = binr.ReadByte();

                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };

                    count = BitConverter.ToInt32(modint, 0);

                }

                else
                {

                    count = bt;        // we already have the data size

                }


            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data

                count -= 1;

            }

            binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte

            return count;

        }


        private static void showBytes(String info, byte[] data)
        {

            Console.WriteLine("{0} [{1} bytes]", info, data.Length);

            for (int i = 1; i <= data.Length; i++)
            {

                Console.Write("{0:X2} ", data[i - 1]);

                if (i % 16 == 0)

                    Console.WriteLine();

            }

            Console.WriteLine("\n\n");

        }

    }

      //------------------------------------------------------------------------------

      // <auto-generated>

      //         Este código fue generado por una herramienta.

      //         Versión del motor en tiempo de ejecución:2.0.50727.5456

      //

      //         Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si

      //         se vuelve a generar el código.

      // </auto-generated>

      //------------------------------------------------------------------------------



      //

      // This source code was auto-generated by wsdl, Version=2.0.50727.3038.

      //

      namespace Hefesto.Proxys.Certificacion
    {
        /// <remarks/>

        [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]

          [System.Diagnostics.DebuggerStepThroughAttribute()]

          [System.ComponentModel.DesignerCategoryAttribute("code")]

          [System.Web.Services.WebServiceBindingAttribute(Name = "GetTokenFromSeedSoapBinding", Namespace = "https://palena.sii.cl/DTEWS/GetTokenFromSeed.jws")]

          public partial class GetTokenFromSeedService : System.Web.Services.Protocols.SoapHttpClientProtocol
          {



              private System.Threading.SendOrPostCallback getVersionOperationCompleted;



              private System.Threading.SendOrPostCallback getTokenOperationCompleted;



              /// <remarks/>

              public GetTokenFromSeedService()
              {

                  this.Url = "https://palena.sii.cl/DTEWS/GetTokenFromSeed.jws";

              }



              /// <remarks/>

              public event getVersionCompletedEventHandler getVersionCompleted;



              /// <remarks/>

              public event getTokenCompletedEventHandler getTokenCompleted;



              /// <remarks/>

              [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "https://palena.sii.cl/DTEWS/GetTokenFromSeed.jws", ResponseNamespace = "https://palena.sii.cl/DTEWS/GetTokenFromSeed.jws")]

              [return: System.Xml.Serialization.SoapElementAttribute("getVersionReturn")]

              public string getVersion()
              {

                  object[] results = this.Invoke("getVersion", new object[0]);

                  return ((string)(results[0]));

              }



              /// <remarks/>

              public System.IAsyncResult BegingetVersion(System.AsyncCallback callback, object asyncState)
              {

                  return this.BeginInvoke("getVersion", new object[0], callback, asyncState);

              }



              /// <remarks/>

              public string EndgetVersion(System.IAsyncResult asyncResult)
              {

                  object[] results = this.EndInvoke(asyncResult);

                  return ((string)(results[0]));

              }



              /// <remarks/>

              public void getVersionAsync()
              {

                  this.getVersionAsync(null);

              }



              /// <remarks/>

              public void getVersionAsync(object userState)
              {

                  if ((this.getVersionOperationCompleted == null))
                  {

                      this.getVersionOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetVersionOperationCompleted);

                  }

                  this.InvokeAsync("getVersion", new object[0], this.getVersionOperationCompleted, userState);

              }



              private void OngetVersionOperationCompleted(object arg)
              {

                  if ((this.getVersionCompleted != null))
                  {

                      System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));

                      this.getVersionCompleted(this, new getVersionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));

                  }

              }



              /// <remarks/>

              [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "https://palena.sii.cl/DTEWS/GetTokenFromSeed.jws", ResponseNamespace = "https://palena.sii.cl/DTEWS/GetTokenFromSeed.jws")]

              [return: System.Xml.Serialization.SoapElementAttribute("getTokenReturn")]

              public string getToken(string pszXml)
              {

                  object[] results = this.Invoke("getToken", new object[] {

                            pszXml});

                  return ((string)(results[0]));

              }



              /// <remarks/>

              public System.IAsyncResult BegingetToken(string pszXml, System.AsyncCallback callback, object asyncState)
              {

                  return this.BeginInvoke("getToken", new object[] {

                            pszXml}, callback, asyncState);

              }



              /// <remarks/>

              public string EndgetToken(System.IAsyncResult asyncResult)
              {

                  object[] results = this.EndInvoke(asyncResult);

                  return ((string)(results[0]));

              }



              /// <remarks/>

              public void getTokenAsync(string pszXml)
              {

                  this.getTokenAsync(pszXml, null);

              }



              /// <remarks/>

              public void getTokenAsync(string pszXml, object userState)
              {

                  if ((this.getTokenOperationCompleted == null))
                  {

                      this.getTokenOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetTokenOperationCompleted);

                  }

                  this.InvokeAsync("getToken", new object[] {

                            pszXml}, this.getTokenOperationCompleted, userState);

              }



              private void OngetTokenOperationCompleted(object arg)
              {

                  if ((this.getTokenCompleted != null))
                  {

                      System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));

                      this.getTokenCompleted(this, new getTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));

                  }

              }



              /// <remarks/>

              public new void CancelAsync(object userState)
              {

                  base.CancelAsync(userState);

              }

          }



          /// <remarks/>

          [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]

          public delegate void getVersionCompletedEventHandler(object sender, getVersionCompletedEventArgs e);



          /// <remarks/>

          [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]

          [System.Diagnostics.DebuggerStepThroughAttribute()]

          [System.ComponentModel.DesignerCategoryAttribute("code")]

          public partial class getVersionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
          {



              private object[] results;



              internal getVersionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :

                  base(exception, cancelled, userState)
              {

                  this.results = results;

              }



              /// <remarks/>

              public string Result
              {

                  get
                  {

                      this.RaiseExceptionIfNecessary();

                      return ((string)(this.results[0]));

                  }

              }

          }



          /// <remarks/>

          [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]

          public delegate void getTokenCompletedEventHandler(object sender, getTokenCompletedEventArgs e);



          /// <remarks/>

          [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]

          [System.Diagnostics.DebuggerStepThroughAttribute()]

          [System.ComponentModel.DesignerCategoryAttribute("code")]

          public partial class getTokenCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
          {



              private object[] results;



              internal getTokenCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :

                  base(exception, cancelled, userState)
              {

                  this.results = results;

              }



              /// <remarks/>

              public string Result
              {

                  get
                  {

                      this.RaiseExceptionIfNecessary();

                      return ((string)(this.results[0]));

                  }

              }

          }

      }

}
