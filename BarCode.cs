using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.BarcodeCodabar;
//using iTextSharp.text.pdf.BarcodePDF417;
//using iTextSharp.text.pdf.BarcodeDatamatrix;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;






namespace DTE33
{
    class BarCode
    {
        #region "PDF417 (2d)"
        //PDF417 online generator: http://www.bcgen.com/pdf417-barcode-creator.html
        public static Bitmap PDF417(string _code, int Scale = 1)
        {
            if (string.IsNullOrEmpty(_code.Trim()) == true)
            {
                return null;
            }
            else
            {
                BarcodePDF417 barcode = new BarcodePDF417();
                barcode.Options = BarcodePDF417.PDF417_USE_ASPECT_RATIO;
                //barcode.YHeight = 6
                barcode.ErrorLevel = 8;
                barcode.SetText(_code);

                //Dim encoding As New System.Text.UTF8Encoding
                //Dim b() As Byte = encoding.GetBytes(_code)
                //barcode.Text = b
                try
                {
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(barcode.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                    //Return bm
                    if (Scale != 1)
                    {
                        //Dim original As Image = bm
                        int finalW = Convert.ToInt32(bm.Width * Scale);
                        int finalH = Convert.ToInt32(bm.Height * Scale);

                        Bitmap retBitmap = new Bitmap(finalW, finalH);
                        Graphics retgr = Graphics.FromImage(retBitmap);
                        retgr.ScaleTransform(Scale, Scale);
                        retgr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        retgr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        retgr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                        retgr.DrawImage(bm, new Point(0, 0));

                        return retBitmap;
                    }
                    else
                    {
                        return bm;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error generating PDF417 barcode. Desc:" + ex.Message);
                }
            }
        }

        private static bool IsNullOrEmpty(string p)
        {
            throw new NotImplementedException();
        }
        #endregion


      
    }
}
