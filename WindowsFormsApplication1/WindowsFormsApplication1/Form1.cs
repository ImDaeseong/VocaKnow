using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private bool bFirst = false;

        private SQLiteConnection _connection;

        private void createdatabaseDaeseong()
        {
            String databaseName = "IndonesiaDB.db";
            //String databaseName = "OzbekistonDB.db";
            //String databaseName = "ChinaDB.db";
            //String databaseName = "PhilippinesDB.db";
            //String databaseName = "KyrgyzstanDB.db";
            if (File.Exists(databaseName))
            {
                _connection = new SQLiteConnection("Data Source=" + databaseName);
                _connection.Open();
            }
            else
            {
                SQLiteConnection.CreateFile(databaseName);
                var _connection = new SQLiteConnection("Data Source=" + databaseName);
                _connection.Open();
            }
        }

        private void createTableKatas()
        {
            try
            {
                string query = "CREATE TABLE Katas ( " +
                    "rIndex int," +
                    "Part1 varchar(50)," +
                    "Part2 varchar(50)," +
                    "KataKor varchar(200)," +
                    "KataIndo varchar(200)," +
                    "lembutlidah varchar(200));";
                SQLiteCommand cmd = new SQLiteCommand(query, _connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        private void insertKatas(string query)
        {
            try
            {
                SQLiteCommand com = new SQLiteCommand(query, _connection);
                com.ExecuteNonQuery();
            }
            catch
            {
            }
        }

        private SQLiteDataReader reader(string query)
        {
            SQLiteCommand com = new SQLiteCommand(query, _connection);
            return com.ExecuteReader();
        }

        private void LoadKatas()
        {
            try
            {
                SQLiteDataReader r = reader("SELECT * FROM Katas");
                while (r.Read())
                {
                    String s1 = r["rIndex"].ToString();
                    String s2 = r["Part1"].ToString();
                    String s3 = r["Part2"].ToString();
                    String s4 = r["KataKor"].ToString();
                    String s5 = r["KataIndo"].ToString();
                    String s6 = r["lembutlidah"].ToString();
                    //Console.WriteLine(s1, s2, s3, s4, s5, s6 );
                    ListViewItem item = new ListViewItem();
                    item.Text = "";
                    item.SubItems.Add(s1);
                    item.SubItems.Add(s2);
                    item.SubItems.Add(s3);
                    item.SubItems.Add(s4);
                    item.SubItems.Add(s5);
                    item.SubItems.Add(s6);
                    lstView.Items.Add(item);
                }
            }
            catch
            {
            }

        }

        private string NullVal(object src, string Value)
        {
            if (src != null)
                return src.ToString();
            return Value;
        }

        private string QStr(string sValue)
        {
            return "'" + sValue.Replace("'", "''") + "'";
        }

        private void ReadFile()
        {
            StreamReader reader = new StreamReader("kata.txt");
            using (reader)
            {
                string rIndex;
                string Part1;
                string Part2;
                string KataKor;
                string KataIndo;
                string lembutlidah;

                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] value = line.Split('|');
                    rIndex = value[0];
                    
                    try
                    {

                        Part1 = NullVal(value[1], "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }
                    catch { Part1 = ""; }

                    try
                    {
                        Part2 = NullVal(value[2], "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }
                    catch { Part2 = ""; }

                    try
                    {
                        KataKor = NullVal(value[3], "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }
                    catch { KataKor = ""; }

                    try
                    {
                        KataIndo = NullVal(value[4], "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }
                    catch { KataIndo = ""; }                    

                    try
                    {
                        lembutlidah = NullVal(value[5], "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }
                    catch {lembutlidah = "";}
                    
                    //string val = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", rIndex, Part1, Part2, KataKor, KataIndo, lembutlidah);
                    //Console.WriteLine(val);

                    string query = string.Format("INSERT INTO Katas (rIndex, Part1, Part2, KataKor, KataIndo, lembutlidah) VALUES ({0},{1},{2},{3},{4},{5});", rIndex, QStr(Part1), QStr(Part2), QStr(KataKor), QStr(KataIndo), QStr(lembutlidah));
                    insertKatas(query);

                    line = reader.ReadLine();
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            lstView.View = View.Details;
            lstView.GridLines = true;
            lstView.FullRowSelect = true;
            lstView.HeaderStyle = ColumnHeaderStyle.Clickable;
            lstView.CheckBoxes = true;
            lstView.OwnerDraw = true;

            lstView.Columns.Add("", 25, HorizontalAlignment.Left);
            lstView.Columns.Add("번호", 50, HorizontalAlignment.Left);
            lstView.Columns.Add("대분류", 80, HorizontalAlignment.Left);
            lstView.Columns.Add("소분류", 100, HorizontalAlignment.Left);
            lstView.Columns.Add("한국어", 450, HorizontalAlignment.Left);
            lstView.Columns.Add("외국어", 450, HorizontalAlignment.Left);
            lstView.Columns.Add("외국어 발음", 400, HorizontalAlignment.Left);
            
            createdatabaseDaeseong();
                        
            //처음 DB 생성후 데이터 insert(최초 사용시만 사용)
            if (bFirst)
            {
                createTableKatas();
                ReadFile();
            }

            LoadKatas();
        }

        private void InitChkBox()
        {
            for (int i = 0; i < lstView.Items.Count; i++)
                lstView.Items[i].Checked = false;
        }

        private void lstView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.DrawBackground();
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(e.Header.Tag);
                }
                catch (Exception)
                {
                }
                CheckBoxRenderer.DrawCheckBox(e.Graphics,
                    new Point(e.Bounds.Left + 4, e.Bounds.Top + 4),
                    value ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
                    System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void lstView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lstView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lstView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstView.SelectedItems.Count > 0)
            {
                string num = lstView.SelectedItems[0].SubItems[1].Text.ToString();
                string part1 = lstView.SelectedItems[0].SubItems[2].Text.ToString();
                string part2 = lstView.SelectedItems[0].SubItems[3].Text.ToString();
                string katakor = lstView.SelectedItems[0].SubItems[4].Text.ToString();
                string kataindo = lstView.SelectedItems[0].SubItems[5].Text.ToString();
                string katailidah = lstView.SelectedItems[0].SubItems[6].Text.ToString();

                txtPart1.Text = part1;
                txtPart2.Text = part2;
                txtKor.Text = katakor;
                txtIndo.Text = kataindo;
                txtLidah.Text = katailidah;
            }
            else
            {
                txtPart1.Text = "";
                txtPart2.Text = "";
                txtKor.Text = "";
                txtIndo.Text = "";
                txtLidah.Text = "";
            }            
        }
        
        private void lstView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                bool value = false;
                try
                {
                    value = Convert.ToBoolean(lstView.Columns[e.Column].Tag);
                }
                catch (Exception)
                {
                }
                lstView.Columns[e.Column].Tag = !value;
                foreach (ListViewItem item in lstView.Items)
                    item.Checked = !value;

                lstView.Invalidate();
            }
        }

      
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lstView.SelectedItems.Count > 0)
            {
                string num = lstView.SelectedItems[0].SubItems[1].Text.ToString();

                string query = string.Format("UPDATE Katas SET Part1={0}, Part2={1}, KataKor={2}, KataIndo={3}, lembutlidah={4} where rIndex = {5}", QStr(txtPart1.Text), QStr(txtPart2.Text), QStr(txtKor.Text), QStr(txtIndo.Text), QStr(txtLidah.Text), num);
                insertKatas(query);
            }

            //lstView.Items.Clear();
            //LoadKatas();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {            
            for (int i = lstView.Items.Count - 1; i >= 0; i--)
            {
                if (lstView.Items[i].Checked)
                {
                    ListViewItem listViewItem = lstView.Items[i];
                    string num = listViewItem.SubItems[1].Text.ToString();
                    string part1 = listViewItem.SubItems[2].Text.ToString();
                    string part2 = listViewItem.SubItems[3].Text.ToString();
                    string katakor = listViewItem.SubItems[4].Text.ToString();
                    string kataindo = listViewItem.SubItems[5].Text.ToString();
                    string katailidah = listViewItem.SubItems[6].Text.ToString();

                    lstView.Items.RemoveAt(i);
                    string query = string.Format("DELETE FROM Katas where rIndex = {0}", num);
                    insertKatas(query);
                }
            }

        }
    }
}
