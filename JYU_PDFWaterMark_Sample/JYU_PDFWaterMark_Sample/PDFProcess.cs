using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Linq;
using System.Text;

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

            string tempHtmlText = htmlTextContent;

            #region 字體檢查

            var unicodefy = new UnicodeFontFactory();
            foreach (var ch in tempHtmlText.Distinct())
            {
                // 檢查字元是否不可被字體識別
                if (!unicodefy.KAIUFont.CharExists(ch))
                {
                    // 幫字體加上html字型替換為思源黑體
                    tempHtmlText = tempHtmlText.Replace(ch.ToString(), "<span style='font-family: NotoSerif'>" + ch + "</span>");
                }
            }

            #endregion 字體檢查

            // 建立一個空白記憶體Stream
            MemoryStream outputStream = new MemoryStream();
            // 將Html字串內容轉換成Byte陣列
            byte[] data = Encoding.UTF8.GetBytes(tempHtmlText);

            //要寫PDF的文件，建構子沒填參數的話預設直式A4
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

                //使用XMLWorkerHelper把Html內容Parse成PDF內容
                XMLWorkerHelper.GetInstance().ParseXHtml(
                    writer, // PDF Writer
                    doc,  // 空白文件
                    msInput, // Html內容
                    null,
                    Encoding.UTF8, // 文件編碼
                    fontFactory // 字體提供者
                );

                //將pdfDest設定的資料寫到PDF內容
                PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                writer.SetOpenAction(action);

                // 文件關檔
                doc.Close();
                msInput.Close();
            }

            outputStream.Close();
            return outputStream.ToArray();
        }

        /// <summary>
        /// 插入文字浮水印
        /// </summary>
        /// <param name="getPDFContent">PDF內容</param>
        /// <param name="textWaterMark">文字浮水印內容</param>
        /// <returns></returns>
        public byte[] InsertWaterMark(
            byte[] getPDFContent,
            string textWaterMark
        )
        {
            MemoryStream msInput = new MemoryStream();
            byte[] result = new byte[] { };

            using (PdfReader pdfReader = new PdfReader(getPDFContent))
            {
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, msInput))
                {
                    int pageTotal = pdfReader.NumberOfPages + 1;
                    PdfContentByte content;
                    PdfGState gs = new PdfGState();
                    var unicodefy = new UnicodeFontFactory();

                    // 第一頁索引起始是1
                    for (int i = 1; i < pageTotal; i++)
                    {
                        // 取得當前頁面Size
                        Rectangle psize = pdfReader.GetPageSize(1);

                        // 取得Page 寬與高
                        float width = psize.Width;
                        float height = psize.Height;

                        content = pdfStamper.GetUnderContent(i); //在内容上方加水印

                        // 設置透明度
                        gs.FillOpacity = 0.3f;
                        content.SetGState(gs);

                        //寫入內容
                        content.BeginText();
                        content.SetColorFill(BaseColor.BLACK);
                        content.SetFontAndSize(unicodefy.KAIUFont, 30);
                        content.SetTextMatrix(0, 0);
                        content.ShowTextAligned(
                            Element.ALIGN_CENTER, // 對齊
                            textWaterMark, // 內容
                            width / 2, // X軸
                            height - 200,  // Y軸
                            5.56f // 旋轉幅度
                        );
                        content.EndText();
                    }
                }

                msInput.Close();
                result = msInput.ToArray();
            }

            return result;
        }
    }
}