using System;
using System.IO;
using System.Text;

namespace JYU_PDFWaterMark_Sample
{
    internal class Program
    {
        public static string testHtmlContent = "<p style='margin: 0 0 0 40px;text-indent: -33px; text-align: justify;text-justify: inter-ideograph;; vertical-align: top; line-height: 22pt; font-size: 12pt; font-family: DFKai-sb;'>PDF 文字內容</p>";
        public static string outPutFileName = "TestWaterMark.pdf";
        public static string outPutDir = "data";
        public static string textWaterMark = "文字浮水印測試";

        private static void Main(string[] args)
        {
            // 為.net core補足缺少編碼元件
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PDFProcess PDF = new PDFProcess();

            // 取得轉換成PDF內容
            byte[] getPDFInstance = PDF.RunHtmlTextContentToPDF(testHtmlContent);

            if (getPDFInstance.Length != 0)
            {
                // 插入文字浮水印
                byte[] tempPDFContent = PDF.InsertWaterMark(getPDFInstance, textWaterMark);

                if (tempPDFContent.Length == 0) { throw new NullReferenceException("PDF內容為空"); }

                // 產生輸出目錄位置
                string genOutputPath = Path.Combine(Directory.GetCurrentDirectory(), outPutDir);

                if (!Directory.Exists(genOutputPath))
                {
                    Directory.CreateDirectory(genOutputPath);
                }

                // 儲存PDF檔案
                PDF.SavePDF(genOutputPath, outPutFileName, tempPDFContent);
            }
        }
    }
}