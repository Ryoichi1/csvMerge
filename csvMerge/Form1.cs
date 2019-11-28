using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csvMerge
{
    public partial class Form1 : Form
    {
        List<List<string>> Buf = new List<List<string>>();

        string myPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<List<string>> Buf = new List<List<string>>();

            // FolderBrowserDialogクラスのインスタンス生成
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            // ダイアログタイトルを設定
            fbd.Description = "フォルダを選択してください";
            // ルートフォルダの設定（RootFolderに何も指定しなければデスクトップがルートになる）
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            // 初期選択されているフォルダの設定（今回は、ローカルディスクのc:\を指定している）
            // 初期選択フォルダは、RootFolder以下にあるフォルダである必要がある
            fbd.SelectedPath = @"c:\";
            // ユーザーが新しいフォルダを作成できるようにする設定（デフォルトでtrue）
            fbd.ShowNewFolderButton = true;

            //ダイアログを表示する
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                // 選択したフォルダ名を取得
                myPath = fbd.SelectedPath;
                //選択されたフォルダを表示する
                MessageBox.Show(myPath + "が選択されました", "選択結果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //"C:\test"以下のファイルをすべて取得する
                //ワイルドカード"*"は、すべてのファイルを意味する
                string[] files = System.IO.Directory.GetFiles(
                    myPath, "*", System.IO.SearchOption.AllDirectories);

                foreach (var f in files)
                {
                    var buf = new List<string>();
                    // csvファイルを開く
                    using (var sr = new System.IO.StreamReader(f))
                    {
                        var cnt = 0;
                        // ストリームの末尾まで繰り返す
                        while (!sr.EndOfStream)
                        {
                            // ファイルから一行読み込む
                            var line = sr.ReadLine();
                            if (cnt++ < 4)
                                continue;
                            // 読み込んだ一行をカンマ毎に分けて配列に格納する
                            var a = line.Split(',').ToList();
                            if (a.Count() < 3)
                                continue;

                            buf.Add(line.Split(',')[2]);
                        }
                        Buf.Add(buf);
                    }
                }




                var OkDataFilePath = $@"{myPath}\alldata.csv";

                //if (!System.IO.File.Exists(OkDataFilePath))
                //{
                //    //既存検査データがなければ新規作成
                //    File.Copy(Constants.fileName_FormatPass, OkDataFilePath);
                //}

                foreach (var l in Buf)
                {
                    // リストデータをすべてカンマ区切りで連結する
                    string stCsvData = string.Join(",", l);

                    // appendをtrueにすると，既存のファイルに追記
                    // falseにすると，ファイルを新規作成する
                    var append = true;

                    // 出力用のファイルを開く
                    using (var sw = new System.IO.StreamWriter(OkDataFilePath, append, Encoding.GetEncoding("Shift_JIS")))
                    {
                        sw.WriteLine(stCsvData);
                    }
                }




            }
        }
    }
}
