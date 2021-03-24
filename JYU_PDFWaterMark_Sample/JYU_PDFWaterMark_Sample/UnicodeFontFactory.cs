using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace JYU_PDFWaterMark_Sample
{
    /// <summary>
    /// 字體工廠設置
    /// </summary>
    public class UnicodeFontFactory : FontFactoryImp
    {
        /// <summary>
        /// 思源黑體來源位置設置，依據系統字體環境中抓取
        /// </summary>
        /// <param name=""NotoSerifCJKtc-SemiBold.otf""></param>
        /// <returns></returns>
        private static readonly string NotoFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
              "NotoSerifCJKtc-SemiBold.otf"); // 思源黑體  NotoSerifCJKtc-SemiBold.otf

        /// <summary>
        /// 標楷體，依據系統字體環境中抓取
        /// </summary>
        private static readonly string KAIUFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
              "kaiu.ttf");

        public BaseFont NotoFont;
        public BaseFont KAIUFont;

        public UnicodeFontFactory()
        {
            NotoFont = BaseFont.CreateFont(NotoFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            KAIUFont = BaseFont.CreateFont(KAIUFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public override Font GetFont(
            string fontname, 
            string encoding, 
            bool embedded, 
            float size, 
            int style, 
            BaseColor color,
            bool cached
        )
        {
            //https://stackoverflow.com/questions/1322303/html-to-pdf-some-characters-are-missing-itextsharp
            //可用思源黑體或標楷體，自己選一個

            BaseFont baseFont = null;

            if (fontname == "NotoSerif")
            {
                baseFont = NotoFont;
            }
            else
            {
                baseFont = KAIUFont;
            }

            return new Font(baseFont, size, style, color);
        }
    }
}