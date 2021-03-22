using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace JYU_PDFWaterMark_Sample
{
    class Program
    {
        public static string testHtmlContent = "<p style='margin: 0 0 0 40px;text-indent: -33px; text-align: justify;text-justify: inter-ideograph;; vertical-align: top; line-height: 22pt; font-size: 12pt; font-family: NotoSerif;'>測試PDF列印浮水印</p>";

        public static string outPutFileName = "TestWaterMark.pdf";
        public static string outPutDir = "data";

        static void Main(string[] args)
        {
            // 為.net core補足缺少編碼元件
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PDFProcess PDF = new PDFProcess();

            // 取得轉換成PDF內容
            byte[] getPDFInstance = PDF.RunHtmlTextContentToPDF(testHtmlContent);

            if (getPDFInstance.Length != 0)
            {
                string genOutputPath = Path.Combine(Directory.GetCurrentDirectory(), outPutDir);

                if (!Directory.Exists(genOutputPath))
                {
                    Directory.CreateDirectory(genOutputPath);
                }

                // 儲存PDF檔案
                PDF.SavePDF(genOutputPath, outPutFileName, getPDFInstance);
            }
        }
    }
}
