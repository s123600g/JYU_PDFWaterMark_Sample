using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

        public BaseFont NotoFont;

        public UnicodeFontFactory()
        {
            NotoFont = BaseFont.CreateFont(NotoFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
    }
}