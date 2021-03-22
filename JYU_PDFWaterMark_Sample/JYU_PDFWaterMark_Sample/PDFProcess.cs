using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace JYU_PDFWaterMark_Sample
{
    public class PDFProcess
    {
        private readonly UnicodeFontFactory fontFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public PDFProcess()
        {
            fontFactory = new UnicodeFontFactory();
        }

        /// <summary>
        /// 輸出PDF檔案
        /// </summary>
        /// <param name="outPutPath"></param>
        /// <param name="outPutPDFFileName"></param>
        /// <param name="content"></param>
        public void SavePDF(
            string outPutPath,
            string outPutPDF_FileName,
            byte[] content
        )
        {
            if (Directory.Exists(outPutPath))
            {
                string FullPath = Path.Combine(outPutPath, outPutPDF_FileName);

                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                }

                System.IO.File.WriteAllBytes(FullPath, content);
            }
        }

        /// <summary>
        /// 執行將Html字串內容轉換成PDF
        /// </summary>
        /// <param name="htmlTextContent">Html 內容字串</param>
        /// <returns>返回經過轉換後PDF流內容(Byte[])</returns>
        public byte[] RunHtmlTextContentToPDF(
            string htmlTextContent
        )
        {
            // 如果未有內容就返回Null
            if (string.IsNullOrEmpty(htmlTextContent)) { return null; }

            // 建立一個空白記憶體Stream
            MemoryStream outputStream = new MemoryStream();
            // 將Html字串內容轉換成Byte陣列
            byte[] data = Encoding.UTF8.GetBytes(htmlTextContent);

            //要寫PDF的文件，建構子沒填的話預設直式A4
            Document doc = new Document();

            // PDF Writer Factory
            using (
                PdfWriter writer = PdfWriter.GetInstance(
                    doc, // 空白文件
                    outputStream // 文件暫存區塊
                )
            )
            {
                //指定文件預設開檔時的縮放為100%
                PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                // 文件開檔
                doc.Open();

                // 將轉換成Byte陣列Html字串內容放在記憶體Stream
                MemoryStream msInput = new MemoryStream(data);

                //使用XMLWorkerHelper把Html 內容Parse到PDF檔裡
                XMLWorkerHelper.GetInstance().ParseXHtml(
                    writer, // PDF Writer
                    doc,  // 空白文件
                    msInput, // Html內容
                    null,
                    Encoding.UTF8, // 文件編碼
                    fontFactory // 字體提供者
                );

                //將pdfDest設定的資料寫到PDF檔
                PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                writer.SetOpenAction(action);

                // 文件關檔
                doc.Close();
                msInput.Close();
            }

            outputStream.Close();
            return outputStream.ToArray();
        }

    }
}