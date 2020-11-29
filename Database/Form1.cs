using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace Database {

    public partial class Form1 : Form {

        public Form1() {
            InitializeComponent();
            
        }
        public class Album {
            public int ID { get; set; }
            public int release_year { get; set; }
            public string album_title { get; set; }
            public string artist { get; set; }
            public string origin { get; set; }
            public double price { get; set; }
            public double rating { get; set; }
            public bool isAvailable { get; set; }

        }
        
        public int id = 1;
        private Stopwatch sw = new Stopwatch();

        List<Album> albums = new List<Album>();


        public static bool isValid(string text, int mode) {
            string strRegex;
            Regex re;
            switch (mode) {
                case 1: // rok
                    strRegex = @"(^(1|2)[0-9]{3}$)";
                    break;

                case 2: //tytul, wykonawca
                    strRegex = @"(^\w+$)";
                    break;

                case 3: //pochodzenie
                    strRegex = @"(^[A-Z][a-z]*$)";
                    break;

                case 4: //cena
                    strRegex = @"(^\d+,?\d*$)";
                    break;

                case 5: //ocena
                    strRegex = @"(^[0-9][,]?[0-9]*$)";
                    break;

                case 6: //id
                    strRegex = @"(^[0-9]+$)";
                    break;

                default:
                    strRegex = @"(*)";
                    break;
            }
            re = new Regex(strRegex);
            if (re.IsMatch(text)) {
                return true;
            } else {
                return false;
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
            int flaga = 0;
            if (!isValid(i_rokwydania.Text, 1)) {
                label7.Visible = true;
                flaga = 1;
            } else {
                label7.Visible = false;
            }
            if (!isValid(i_tytulalbumu.Text, 2)) {
                label8.Visible = true;
                flaga = 1;
            } else {
                label8.Visible = false;
            }
            if (!isValid(i_wykonawca.Text, 2)) {
                label9.Visible = true;
                flaga = 1;
            } else {
                label9.Visible = false;
            }
            if (!isValid(i_pochodzenie.Text, 3)) {
                label10.Visible = true;
                flaga = 1;
            } else {
                label10.Visible = false;
            }
            if (!isValid(i_cena.Text, 4)) {
                label11.Visible = true;
                flaga = 1;
            } else {
                label11.Visible = false;
            }
            if (!isValid(i_ocena.Text, 5)) {
                label12.Visible = true;
                flaga = 1;
            } else {
                label11.Visible = false;
            }
            if (flaga == 0) {
                albums.Add(new Album { ID = id, album_title = i_tytulalbumu.Text, artist = i_wykonawca.Text, isAvailable = i_nastanie.Checked, origin = i_pochodzenie.Text, price = Convert.ToDouble(i_cena.Text), rating = Convert.ToDouble(i_ocena.Text), release_year = Convert.ToInt32(i_rokwydania.Text) });
                //dataGridView1.Rows.Add(id, i_rokwydania.Text, i_tytulalbumu.Text, i_wykonawca.Text, i_pochodzenie.Text, i_cena.Text, i_ocena.Text, i_nastanie.Checked);
                id++;
                var bindingList = new BindingList<Album>(albums);
                var source = new BindingSource(bindingList, null);
                dataGridView1.DataSource = source;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
            int flaga = 0;
            if (!isValid(i_id.Text, 6)) {
                flaga = 1;
                label14.Visible = true;
            } else {
                label14.Visible = false;
            }
            if (flaga == 0) {
                foreach (Album album in albums) {
                    if (album.ID == Convert.ToInt32(i_id.Text)) {
                        albums.Remove(album);
                        break;
                    }
                }
            }
            var bindingList = new BindingList<Album>(albums);
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;
        }

        private void button4_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
            using (TextWriter tw = new StreamWriter("baza.txt")) {
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++) {
                        tw.Write($"{dataGridView1.Rows[i].Cells[j].Value.ToString()}");

                        if (!(j == dataGridView1.Columns.Count - 1)) {
                            tw.Write("_");
                        }
                    }
                    tw.WriteLine();
                }
                MessageBox.Show("Pomyślnie wyeksportowano bazę");
            }
        }

        private int flaga_importu = 0;

        private void button3_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
            if (flaga_importu == 0) {
                var lines = File.ReadAllLines("baza.txt");
                if (lines.Count() > 0) {
                    foreach (var cellValues in lines.Skip(0)) {
                        var cellArray = cellValues.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (cellArray.Length == dataGridView1.Columns.Count) {
                            dataGridView1.Rows.Add(cellArray);
                            id++;
                        }
                    }
                }
                MessageBox.Show("Pomyślnie zaimportowano bazę");
                flaga_importu++;
            } else {
                MessageBox.Show("Baza została już wczytana");
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int flaga = 0;
            if (!isValid(i_iloscrekordow.Text, 6)) {
                flaga = 1;
                label16.Visible = true;
            } else {
                label16.Visible = false;
            }
            if (flaga == 0) {
                int n = Convert.ToInt32(i_iloscrekordow.Text);
                sw.Start();
                for (int i = 0; i < n; i++) {
                    losowanie(id);
                    id++;
                }
                var bindingList = new BindingList<Album>(albums);
                var source = new BindingSource(bindingList, null);
                dataGridView1.DataSource = source;
                sw.Stop();
                MessageBox.Show("Czas losowania: " + sw.ElapsedMilliseconds.ToString() + " ms");

            }
        }

        public void losowanie(int currentid) {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            bool nastanie;
            if (rnd.Next(0, 2) == 0) {
                nastanie = false;
            } else {
                nastanie = true;
            }
            string[] wykonawcy = new string[] { "Abba", "Metallica", "Ich troje", "Band", "BBH", "Ye" };
            string[] tytuly = new string[] { "YTZ", "XYZ Odyssey", "KKKX", "WKXA", "PXA", "GMD" };
            string[] pochodzenie = new string[] { "Polska", "Niemcy", "Stany Zjednoczone", "Kostaryka", "Watykan", "Islandia" };
            int wIndex = rnd.Next(wykonawcy.Length);
            int tIndex = rnd.Next(tytuly.Length);
            int pIndex = rnd.Next(pochodzenie.Length);
            int rok = rnd.Next(1900, 2020);
            double cena = rnd.Next(20, 200);
            double ocena = Math.Round(rnd.NextDouble() * 10, 1);
            int id = currentid;
            //string[] row = new string[] { id.ToString(), rok.ToString(), tytuly[tIndex], wykonawcy[wIndex], pochodzenie[pIndex], cena.ToString(), ocena.ToString() };
            albums.Add(new Album { ID = id, album_title = tytuly[tIndex], artist = wykonawcy[wIndex], isAvailable = nastanie, origin = pochodzenie[pIndex], price = cena, rating = ocena, release_year = rok });
        }


        private void w_liniowe_Click(object sender, EventArgs e) {
            wyszukiwanieGrid.Rows.Clear();
            string value = w_teks.Text;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                        if (dataGridView1.Rows[i].Cells[comboBox1.SelectedIndex].Value.ToString() == value) {
                            wyszukiwanieGrid.Rows.Add(
                                dataGridView1.Rows[i].Cells[0].Value,
                                dataGridView1.Rows[i].Cells[1].Value,
                                dataGridView1.Rows[i].Cells[2].Value,
                                dataGridView1.Rows[i].Cells[3].Value,
                                dataGridView1.Rows[i].Cells[4].Value,
                                dataGridView1.Rows[i].Cells[5].Value,
                                dataGridView1.Rows[i].Cells[6].Value,
                                dataGridView1.Rows[i].Cells[7].Value
                                );
                        }
                    }
            dataGridView1.Visible = false;
            wyszukiwanieGrid.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
        }
    }
}