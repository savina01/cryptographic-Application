using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace cryptographicApplication
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        //TODO
        //нов проект само за многоазбучното за курсовата работа

        public void PolyaplhabeticCiper()
        {
            //https://www.youtube.com/watch?v=Tpd_ebRNvf0
            char[] cipher = new char[100];

            string c = "";

            int i;
            int j = 0;

            string p = textBoxPlain.Text;
            char[] plain = p.ToCharArray();
            string k = textBoxKey1.Text;
            char[] key = k.ToCharArray();

            for (i = 0; i < plain.Length; i++)
            {
                //проверка за край на ключа и започване на неговото повтаряне, ако думата е свършила
                if (j == key.Length)
                    j = 0;

                if ((plain[i] + (key[j] - 97)) > 122)
                {
                    //почти не се налага да влизаме тук
                    cipher[i] = Convert.ToChar(plain[i] + ((key[j] - 97) - 26));
                    j++;
                }
                else
                {
                    //намираме числото на c след като сме събрали числото на p с ключа
                    cipher[i] = Convert.ToChar(plain[i] + (key[j] - 97));
                    j++;
                }
            }
            for (i = 0; i < plain.Length; i++)
            {
                c += cipher[i];
            }
            textBoxChiper1.Text = c;
        }
        public int[] GetShiftIndexes(string key)
        {
            //https://www.programmingalgorithms.com/algorithm/transposition-cipher/

            //взимаме индексите на ключа
            int[] indexes =  new int[key.Length];

            //лист от двойки ключ и стойност
            List<KeyValuePair<int,char>> sortedKey = new List<KeyValuePair<int, char>>();
            int i;

            //добавяме всички индекси със стойности в sortedKey
            for(i = 0; i < key.Length; i++)
                sortedKey.Add(new KeyValuePair<int, char> (i, key[i]));

            sortedKey.Sort(
                delegate (KeyValuePair<int, char> pair1, KeyValuePair<int, char> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                });
            for (i = 0; i < key.Length; i++)
                indexes[sortedKey[i].Key] = i;
            return indexes;

        }
        public void TranspositionCiper(string plain, string key, char padChar)
        {
            plain = (plain.Length % key.Length == 0) ? plain :
                plain.PadRight(plain.Length - (plain.Length % key.Length) + key.Length, padChar);

            StringBuilder c = new StringBuilder();
            string res = "";

            int totalChars = plain.Length;
            int totalColumns = key.Length;
            int totalRows = (int)Math.Ceiling((double)totalChars / totalColumns);

            char[,] rowChars = new char[totalRows, totalColumns];
            char[,] colChars = new char[totalColumns, totalRows];
            char[,] sortedColChars = new char[totalColumns, totalRows];
            int currentRow, currentColumn, i, j;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = plain[i];
            }

            for (i = 0; i < totalRows; ++i)
                for (j = 0; j < totalColumns; ++j)
                    colChars[j, i] = rowChars[i, j];

            for (i = 0; i < totalColumns; ++i)
                for (j = 0; j < totalRows; ++j)
                    sortedColChars[shiftIndexes[i], j] = colChars[i, j];

            for(i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                c.Append(sortedColChars[currentRow, currentColumn]);
            }

            res = c.ToString();

            textBoxCiper2.Text = res;
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPlain.Text) &&
                string.IsNullOrEmpty(textBoxKey1.Text))           
                    MessageBox.Show(
                        "Enter in Plain and Key1.",
                        "WARNING",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                
            else
                PolyaplhabeticCiper();

            if (string.IsNullOrEmpty(textBoxChiper1.Text) &&
                string.IsNullOrEmpty(textBoxKey2.Text))
                    MessageBox.Show(
                        "Enter in Key2.",
                        "WARNING",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            else
                TranspositionCiper(textBoxChiper1.Text, textBoxKey2.Text, '-');

        }
    }
}
