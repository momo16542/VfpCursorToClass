using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 類別產生器
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void howtowrite(StringBuilder stringBuilder, string 欄位類型, string 欄位名稱);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(文字.Document.ContentStart, 文字.Document.ContentEnd).Text;
            代碼.Document.Blocks.Clear();
            代碼.Document.Blocks.Add(new Paragraph(new Run(CursorToClass(richText, NormalWrite))));
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(文字.Document.ContentStart, 文字.Document.ContentEnd).Text;
            代碼.Document.Blocks.Clear();
            代碼.Document.Blocks.Add(new Paragraph(new Run(CursorToClass(richText,OnPropertyWrite))));
        }
        private string CursorToClass(string vfpCursorCommand, howtowrite howtowrite)
        {
            //CREATE CURSOR TMPSTOCK4(隸屬倉庫 C(10),倉儲代號 C(10),品別 C(20),品號 C(30),品名 C(200),合計數量 Y,單位成本 Y,合計金額 Y)
            //參數直接用括號內的欄位
            StringBuilder sb = new StringBuilder();
            try
            {
                var 欄位S = vfpCursorCommand.Split(',');
                foreach (var 欄位 in 欄位S)
                {
                    var 欄 = 欄位.Trim().Split(' ');
                    var 欄位名稱 = 欄[0];
                    var 欄位類型 = 欄[1].Substring(0, 1);
                    switch (欄位類型)
                    {
                        case "C":
                        case "M":
                            欄位類型 = "string";
                            break;
                        case "Y":
                        case "B":
                        case "F":
                        case "N":
                            欄位類型 = "decimal";
                            break;
                        case "D":
                        case "T":
                            欄位類型 = "DateTime";
                            break;
                        case "I":
                            欄位類型 = "int";
                            break;

                    }
                    howtowrite(sb, 欄位類型, 欄位名稱);
                }
            }
            catch { }
            return sb.ToString();
        }
        public void NormalWrite(StringBuilder sb, string 欄位類型, string 欄位名稱)
        {
            sb.AppendLine("public " + 欄位類型 + " " + 欄位名稱 + " {get;set;}");
        }
        public void OnPropertyWrite(StringBuilder sb, string 欄位類型, string 欄位名稱)
        {
            sb.AppendLine("public " + 欄位類型 + " " + 欄位名稱 +
                " {get {return _" + 欄位名稱 + "; }set{_" + 欄位名稱 + "=value; \r   OnPropertyChanged();}}"
                + "\r private " + 欄位類型 + " _" + 欄位名稱 + ";");
        }
    }
}
