using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            const string russianL = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string russianU = russianL.ToUpper();
            int count = 0;
            
            string plaintext = text1.Text;
            StringBuilder res = new StringBuilder();
            StringBuilder key = MakeKey(russianL);

            if (key.Length != 0)
            {
                foreach (char c in plaintext)
                {
                    if (russianL.Contains(c))
                    {
                        res.Append(russianL[(russianL.IndexOf(c) + russianL.IndexOf(key[count++ % key.Length]) + 1) % 33]);
                    }
                    else if (russianU.Contains(c))
                    {
                        res.Append(russianU[(russianU.IndexOf(c) + russianL.IndexOf(key[count++ % key.Length]) + 1) % 33]);
                    }
                    else
                    {
                        res.Append(c);
                    }

                    if (count % key.Length == 0)
                    {
                        for (int i = 0; i < key.Length; i++)
                        {
                            key[i] = russianL[(russianL.IndexOf(key[i]) + 1) % 33];
                        }
                    }
                }
                result.Text = res.ToString();
            }
            else
                result.Text = plaintext;
        }                

        private StringBuilder MakeKey(string russianL)
        {
            string key = text2.Text.ToLower();            
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < key.Length; i++)
            {
                if (russianL.Contains(key[i]))
                {
                    res.Append(key[i]);
                }
            }
            return res; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const string russianL = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string russianU = russianL.ToUpper();
            int count = 0;

            string plaintext = text1.Text;
            StringBuilder res = new StringBuilder();
            StringBuilder key = MakeKey(russianL);

            if (key.Length != 0)
            {
                foreach (char c in plaintext)
                {
                    if (russianL.Contains(c))
                    {
                        res.Append(russianL[(russianL.IndexOf(c) - russianL.IndexOf(key[count++ % key.Length]) - 1 + 33) % 33]);
                    }
                    else if (russianU.Contains(c))
                    {
                        res.Append(russianU[(russianU.IndexOf(c) - russianL.IndexOf(key[count++ % key.Length]) - 1 + 33) % 33]);
                    }
                    else
                    {
                        res.Append(c);
                    }

                    if (count % key.Length == 0)
                    {
                        for (int i = 0; i < key.Length; i++)
                        {
                            key[i] = russianL[(russianL.IndexOf(key[i]) + 1) % 33];
                        }
                    }
                }
                result.Text = res.ToString();
            }
            else
                result.Text = plaintext;
        }

        private void button4_Click(object sender, EventArgs e)
        {            
            StringBuilder key = ProccesText(textBox2.Text), text = ProccesText(textBox3.Text);
            char[] res = new char[text.Length];
            int n = key.Length;

            List<(char c, int idx)> items = SortKey(key);

            if (n > 0)
            {              
                int m = (int)Math.Ceiling((double)text.Length / n);
                char[,] table = new char[m, n];

                int pos = 0;
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n && pos < text.Length; j++)
                    {
                        table[i, j] = text[pos++];
                    }
                }

                pos = 0;
                for (int i = 0; i < n; i++)
                {
                    int col = items[i].idx;

                    for (int j = 0; j < m; j++)
                    {
                        if (table[j, col] != '\0')
                            res[pos++] = table[j, col];
                    }
                }
                textBox1.Text = new string(res);
            }
            else
                textBox1.Text = text.ToString();
        }

        internal StringBuilder ProccesText(string txt)
        {
            string userKey = txt.ToLower();
            StringBuilder key = new StringBuilder();
            int[] alf = new int[26];
            for (int i = 0; i < txt.Length; i++)
            {
                if (userKey[i] >= 'a' && userKey[i] <= 'z')
                {
                    key.Append(userKey[i]);
                }
            }
            return key;
        }

        List<(char c, int idx)> SortKey(StringBuilder key)
        {
            List<(char c, int idx)> items = new List<(char c, int idx)>();
            for (int i = 0; i < key.Length; i++)
                items.Add((key[i], i));

            items.Sort((a, b) => {
                int cmp = a.c.CompareTo(b.c);
                return cmp != 0 ? cmp : a.idx.CompareTo(b.idx);
            });

            return items;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder key = ProccesText(textBox2.Text), text = ProccesText(textBox3.Text);
            char[] res = new char[text.Length];
            int n = key.Length;

            if (n != 0)
            {
                List<(char c, int idx)> items = SortKey(key);

                int m = (int)Math.Ceiling((double)text.Length / n);
                int fullCols = text.Length % n;
                char[,] table = new char[m, n];

                int pos = 0;
                for (int i = 0; i < n; i++)
                {
                    int col = items[i].idx;
                    int rows = m;
                    if (fullCols != 0 && col >= fullCols)
                    {
                        rows--;
                    }
                    for (int j = 0; j < rows && pos < text.Length; j++)
                    {
                        table[j, col] = text[pos++];
                    }
                }

                pos = 0;
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (table[i, j] != '\0')
                            res[pos++] = table[i, j];
                    }
                }
                textBox1.Text = new string(res);
            }
            else
                textBox1.Text = text.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Сохранить результат";
            string fileName = string.IsNullOrWhiteSpace(textBox5.Text) ? "result.txt" : textBox5.Text;

            if (!fileName.EndsWith(".txt"))
                fileName += ".txt";

            saveFileDialog.FileName = fileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, result.Text, Encoding.Unicode);
                MessageBox.Show("Файл успешно сохранён!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Сохранить результат";
            string fileName = string.IsNullOrWhiteSpace(textBox4.Text) ? "result.txt" : textBox4.Text;

            if (!fileName.EndsWith(".txt"))
                fileName += ".txt";

            saveFileDialog.FileName = fileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, textBox1.Text, Encoding.UTF8);
                MessageBox.Show("Файл успешно сохранён!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Выберите файл";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    text1.Text = fileContent;   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения файла: " + ex.Message);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Выберите файл";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    textBox3.Text = fileContent;   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения файла: " + ex.Message);
                }
            }
        }
    }
}
