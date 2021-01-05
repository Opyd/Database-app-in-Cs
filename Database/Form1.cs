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
            public String album_title { get; set; }
            public String artist { get; set; }
            public String origin { get; set; }
            public double price { get; set; }
            public double rating { get; set; }
            public bool isAvailable { get; set; }

            void showdata() {
                Debug.Write(this.ID + " " + this.release_year + " " + this.album_title + " " + this.artist);
            
            }

        }

        int flaga_losowanie = 0;
        public int id = 1;
        private Stopwatch sw = new Stopwatch();

        List<Album> albums = new List<Album>();

        List<List<List<string>>> OriginsListInverionsSet = new List<List<List<string>>>();
        List<List<List<string>>> OriginsListChainSet = new List<List<List<string>>>();

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
                        if (cellArray.Length == 8) {
                            albums.Add(new Album() { ID = Convert.ToInt32(cellArray[0]), release_year = Convert.ToInt32(cellArray[1]), album_title = cellArray[2], artist = cellArray[3], origin = cellArray[4], price = Convert.ToInt32(cellArray[5]), rating = Convert.ToDouble(cellArray[6]), isAvailable = Convert.ToBoolean(cellArray[7]) });
                            id++;
                        }
                    }
                }
                var bindingList = new BindingList<Album>(albums);
                var source = new BindingSource(bindingList, null);
                dataGridView1.DataSource = source;
                MessageBox.Show("Pomyślnie zaimportowano bazę");
                flaga_importu++;

            } else {
                MessageBox.Show("Baza została już wczytana");
            }
        }

        private List<T> DodajZListy<T>(List<T> ListaZ, int _z, int _do) {
            List<T> listRegion = new List<T>();
            for (int i = _z; i < _do; i++) {
                listRegion.Add(ListaZ[i]);
            }
            return listRegion;
        }

        private List<List<string>> CreateOriginsTableForChainSearch(List<Album> _lista, int property) {
            List<List<string>> ListOfPointer = new List<List<string>>();
            List<string> TempList = new List<string>();

            int counter = 0;
            bool titleAdded = false;

            void CreateTable<T>(int i, T value, T valuePrev) {
                Comparer<T> Comp = Comparer<T>.Default;
                void AddToList(bool a, bool b) {
                    if(!a || !b) { return; }
                    TempList.Insert(2, (counter).ToString());
                    ListOfPointer.Add(DodajZListy(TempList, 0, 3)); titleAdded = false; counter = 0;
                }
                AddToList(i > 0, Comp.Compare(value, valuePrev) > 0);
                if (!titleAdded) {
                    TempList.Insert(0, value.ToString());
                    TempList.Insert(1, i.ToString());
                    titleAdded = true;
                }
                counter++;

                AddToList(i == (_lista.Count - 1), true);
            }
            int I(int i) { return (i > 0) ? i - 1 : i; }
            switch (property) {
                case 0:for(int i = 0; i < _lista.Count; i++) { CreateTable<int>(i, _lista[i].ID, _lista[I(i)].ID); }break;
                case 1:for(int i = 0; i < _lista.Count; i++) { CreateTable<int>(i, _lista[i].release_year, _lista[I(i)].release_year); }break;
                case 2:for(int i = 0; i < _lista.Count; i++) { CreateTable<string>(i, _lista[i].album_title, _lista[I(i)].album_title); } break;
                case 3:for(int i = 0; i < _lista.Count; i++) { CreateTable<string>(i, _lista[i].artist, _lista[I(i)].artist); } break;
                case 4:for(int i = 0; i < _lista.Count; i++) { CreateTable<string>(i, _lista[i].origin, _lista[I(i)].origin); } break;
                case 5:for(int i = 0; i < _lista.Count; i++) { CreateTable<double>(i, _lista[i].price, _lista[I(i)].price); } break;
                case 6:for(int i = 0; i < _lista.Count; i++) { CreateTable<double>(i, _lista[i].rating, _lista[I(i)].rating); } break;
                case 7:for(int i = 0; i < _lista.Count; i++) { CreateTable<bool>(i, _lista[i].isAvailable, _lista[I(i)].isAvailable); } break;
            }
            void show() {
                Debug.WriteLine("[Cecha], [Pierwsze wystąpienie], [Liczba wystąpień]");
                for(int i = 0; i < ListOfPointer.Count; i++) {
                    for(int j = 0; j < ListOfPointer[i].Count; j++) { Debug.Write("[" + ListOfPointer[i][j] + "]"); }
                    Debug.Write("\n");
                }
                Console.Write("_______________________\n");
            }
            show();
            return ListOfPointer;
        }


        private List<List<string>> CreateOriginsTableForInversionListSearch(List<Album> _lista, int property) {

            List<List<string>> ListOfPointer = new List<List<string>>();
            List<string> TempList = new List<string>();
            int counter = 0;
            bool titleAdded = false;

            void CreateTable<T>(int i, T value, T valuePrev, int id) {
                Comparer<T> Comp = Comparer<T>.Default;
                void AddToList(bool a, bool b) {
                    if (!a || !b) { return; }
                    int J = ((TempList.Count - counter) > 0) ? (TempList.Count - counter) : 0;
                    ListOfPointer.Add(DodajZListy(TempList, J, TempList.Count)); titleAdded = false; counter = 0;
                }
                AddToList(i > 0, Comp.Compare(value, valuePrev) > 0);

                if (!titleAdded) { TempList.Add((value).ToString()); titleAdded = true; counter++; }
                TempList.Add((id).ToString()); counter++;

                AddToList(i == (_lista.Count - 1), true);
            }
            int I(int i) { return (i > 0) ? i - 1 : i; }


            switch (property) {
                case 0: for (int i = 0; i < _lista.Count; i++) { CreateTable<int>(i, _lista[i].ID, _lista[I(i)].ID,_lista[i].ID); } break;
                case 1: for (int i = 0; i < _lista.Count; i++) { CreateTable<int>(i, _lista[i].release_year, _lista[I(i)].release_year, _lista[i].ID); } break;
                case 2: for (int i = 0; i < _lista.Count; i++) { CreateTable<string>(i, _lista[i].album_title, _lista[I(i)].album_title, _lista[i].ID); } break;
                case 3: for (int i = 0; i < _lista.Count; i++) { CreateTable<string>(i, _lista[i].artist, _lista[I(i)].artist, _lista[i].ID); } break;
                case 4: for (int i = 0; i < _lista.Count; i++) { CreateTable<string>(i, _lista[i].origin, _lista[I(i)].origin, _lista[i].ID); } break;
                case 5: for (int i = 0; i < _lista.Count; i++) { CreateTable<double>(i, _lista[i].price, _lista[I(i)].price, _lista[i].ID); } break;
                case 6: for (int i = 0; i < _lista.Count; i++) { CreateTable<double>(i, _lista[i].rating, _lista[I(i)].rating, _lista[i].ID); } break;
                case 7: for (int i = 0; i < _lista.Count; i++) { CreateTable<bool>(i, _lista[i].isAvailable, _lista[I(i)].isAvailable, _lista[i].ID); } break;
            }


            void show() {
                Debug.WriteLine("[Cecha], [Indeks..]");
                for (int i = 0; i < ListOfPointer.Count; i++) {
                    for (int j = 0; j < ListOfPointer[i].Count; j++) { Debug.Write("[" + ListOfPointer[i][j] + "]"); }
                    Debug.Write("\n");
                }
                Console.Write("_______________________\n");
            }
            show();

            return ListOfPointer;
        }





        private List<Album> PosortujListePoWlasciwosci(List <Album> _lista, int property) {
            List<Album> sorted = new List<Album>();
            switch (property) {
                case 0: sorted = _lista.OrderBy(o => o.ID).ToList(); break;
                case 1: sorted = _lista.OrderBy(o => o.release_year).ToList();break;
                case 2: sorted = _lista.OrderBy(o => o.album_title).ToList();break;
                case 3: sorted = _lista.OrderBy(o => o.artist).ToList(); break;
                case 4: sorted = _lista.OrderBy(o => o.origin).ToList(); break;
                case 5: sorted = _lista.OrderBy(o => o.price).ToList(); break;
                case 6: sorted = _lista.OrderBy(o => o.rating).ToList(); break;
                case 7: sorted = _lista.OrderBy(o => o.isAvailable).ToList(); break;
             
            }
            return sorted;
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
                

                if(flaga_losowanie == 0) {
                    for(int i = 0; i < 8; i++) {
                        List<Album> AlbumyPosortowane = PosortujListePoWlasciwosci(albums, i);
                        List<List<string>> _inversion = CreateOriginsTableForInversionListSearch(AlbumyPosortowane,i);
                        List<List<string>> _chain = CreateOriginsTableForChainSearch(AlbumyPosortowane,i);

                        OriginsListChainSet.Add(_chain);
                        OriginsListInverionsSet.Add(_inversion);

                        //List<int> chaincon = CreateChainForOriginList(AlbumyPosortowane,i)
                        //OriginsListChainSet.Add(chaincon);
                    }
                    

                    flaga_losowanie = 1;
                }
                

                sw.Stop();
                MessageBox.Show("Czas losowania oraz tworzenia struktur indeksowych: " + sw.ElapsedMilliseconds.ToString() + " ms");
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
            sw.Reset();
            List<Album> Results = new List<Album>();
            wyszukiwanieGrid.Rows.Clear();
            string value = w_teks.Text;
            sw.Start();
            for (int i = 0; i < albums.Count; i++) {
                        if (dataGridView1.Rows[i].Cells[comboBox1.SelectedIndex].Value.ToString() == value) {
                            Results.Add(albums[i]);
                        }
                    }
            sw.Stop();
            var bindingList = new BindingList<Album>(Results);
            var source = new BindingSource(bindingList, null);
            wyszukiwanieGrid.DataSource = source;
            dataGridView1.Visible = false;
            wyszukiwanieGrid.Visible = true;
            label17.Text = "Czas: " + sw.Elapsed.ToString();
        }

        private void button6_Click(object sender, EventArgs e) {
            dataGridView1.Visible = true;
            wyszukiwanieGrid.Visible = false;
        }

        public class IDComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.ID.CompareTo(y.ID);
            }

        }
        public class ArtistComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.artist.CompareTo(y.artist);
            }

        }

        public class YearComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.release_year.CompareTo(y.release_year);
            }

        }
        public class TitleComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.album_title.CompareTo(y.album_title);
            }

        }
        public class OriginComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.origin.CompareTo(y.origin);
            }

        }
        public class PriceComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.price.CompareTo(y.price);
            }

        }
        public class RatingComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.rating.CompareTo(y.rating);
            }

        }
        public class AvabilityComparer : IComparer<Album> {

            public int Compare(Album x, Album y) {
                return x.isAvailable.CompareTo(y.isAvailable);
            }

        }

        private void w_binarne_Click(object sender, EventArgs e) {
            sw.Reset();
            bool fail = false;
            wyszukiwanieGrid.Rows.Clear();
            List<Album> SortedList = new List<Album>();
            List<Album> Results = new List<Album>();
            int property = comboBox1.SelectedIndex;
            int position = 1;
            try {
            switch (property) {
                case 0:
                    SortedList = albums.OrderBy(o => o.ID).ToList();
                    sw.Start();
                    while(position >= 0) {
                        position = SortedList.BinarySearch(new Album { ID = Convert.ToInt32(w_teks.Text) }, new IDComparer());
                        if (position < 0) {
                            fail = true;
                            break;

                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;
                case 1:
                    SortedList = albums.OrderBy(o => o.release_year).ToList();
                    sw.Start();
                    while (position >= 0) {
                        position = SortedList.BinarySearch(new Album { release_year = Convert.ToInt32(w_teks.Text) }, new YearComparer());
                        if (position < 0) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;
                case 2:
                    SortedList = albums.OrderBy(o => o.album_title).ToList();
                    sw.Start();
                    while (position >= 0) {
                        position = SortedList.BinarySearch(new Album { album_title = w_teks.Text }, new TitleComparer());
                        if (position < 0) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;
                case 3:
                    SortedList = albums.OrderBy(o => o.artist).ToList();
                    sw.Start();
                    while(position >= 0) {
                        position = SortedList.BinarySearch(new Album { artist = w_teks.Text }, new ArtistComparer());
                        if(position < 0) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    
                    break;
                case 4:
                    SortedList = albums.OrderBy(o => o.origin).ToList();
                    sw.Start();
                    while (position >= 0) {
                        position = SortedList.BinarySearch(new Album { origin = w_teks.Text }, new OriginComparer());
                        if (position < 0) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;
                case 5:
                    SortedList = albums.OrderBy(o => o.price).ToList();
                    sw.Start();
                    while (position >= 0) {
                        position = SortedList.BinarySearch(new Album { price = Convert.ToDouble(w_teks.Text) }, new PriceComparer());
                        if (position < 0) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;
                case 6:
                    SortedList = albums.OrderBy(o => o.rating).ToList();
                    sw.Start();
                    while (position >= 0) {
                        position = SortedList.BinarySearch(new Album { rating = Convert.ToDouble(w_teks.Text) }, new RatingComparer());
                        if (position < 0) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;
                case 7:
                    SortedList = albums.OrderBy(o => o.isAvailable).ToList();
                    sw.Start();
                    while (position >= 0) {
                        position = SortedList.BinarySearch(new Album { isAvailable = Convert.ToBoolean(w_teks.Text) }, new AvabilityComparer());
                        if (position < 0 ) {
                            fail = true;
                            break;
                        }
                        Results.Add(SortedList.ElementAt(position));
                        SortedList.RemoveAt(position);
                    }
                    sw.Stop();
                    break;   
                }
                var bindingList = new BindingList<Album>(Results);
                var source = new BindingSource(bindingList, null);
                wyszukiwanieGrid.DataSource = source;

                dataGridView1.Visible = false;
                wyszukiwanieGrid.Visible = true;
                label17.Text = "Czas test: " + sw.Elapsed.ToString();
            } catch(FormatException) {
                MessageBox.Show("Wprowadzono zły format");
            }

        }

        private void w_lancuchowe_Click(object sender, EventArgs e) {

        }
    }
}