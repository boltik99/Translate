using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Translate
{
    public partial class MainWindow : Window
    {
        static int size = 2000;
        static int size_block = 100;

        List<string> block = new List<string>();
        string[] lib = new string[size];

        int index = 1;
        int right = 0;
        int chet = 0;

        bool rb_one = false;
        bool rb_two = false;

        bool start = false;

        public MainWindow()
        {
            InitializeComponent();

            tb1.Text = "Enter проверять, L.Ctrl чистка, R.Shift перевод";
            tb2.Text = "";
        }

        //функция чтения
        public void read (string path, string[] lib)
        {
            StreamReader f = new StreamReader(path);
            int i = 0;
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                if (s != "")
                {
                    lib[i] = s;
                    i++;
                }
            }
            f.Close();
        }

        public void add_50_words_to_block(int number_block)
        {
            if (number_block == 1)
            {
                for (int l = 0; l < size_block; l++)
                {
                    block.Add(lib[l]);
                }
            }
            else
            {
                for (int l = (number_block - 1) * 100; l < (number_block - 1) * 100 + 100; l++)
                {
                    block.Add(lib[l]);
                }
            }
        }

        public int random_index()
        {
            Random r = new Random();
            index = r.Next(0, block.Count);
            if (rb_one) while (index % 2 == 0) { index = r.Next(0, block.Count); }
            if (rb_two) while (index % 2 == 1) { index = r.Next(0, block.Count); }
            return index;
        }
        public void bt1_Click(object sender, RoutedEventArgs e)
        {
            if (rb_two == false && rb_one == false) { MessageBox.Show("Выбери один из вариантов, stupid mf"); return; }
            string path = "library.txt";

            //read
            if (start == false) { read(path, lib); start = true; }

            //проверка пустоты ТБ - четный индекс это англ слово
            int number_block;
            try { number_block = int.Parse(tb3.Text); }
            catch { MessageBox.Show("Введи значение дебил! Сверху в углу! "); return; }

            //Добавляем в список 50 слов
            add_50_words_to_block(number_block);

            //вывод первого слова
            index = random_index(); tb1.Text = block[index];

            rb1.IsEnabled = false;
            rb2.IsEnabled = false;
        }

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (rb_one == true)
            {
                string s = tb2.Text;
                if (e.Key == Key.Enter)
                {
                    if (s == block[index - 1])
                    {
                        chet++;
                        right++;
                        block.RemoveAt(index);
                        block.RemoveAt(index - 1);

                        //exit
                        if (block.Count == 0)
                        {
                            MessageBox.Show("Правильно: " + right.ToString() + " из: " + (chet + 1).ToString());
                            tb1.Text = "";
                            tb2.Text = "";
                            tb3.Text = "";
                            tb4.Text = "";
                            right = 0;
                            chet = 0;
                            return;
                        }

                        //случайный индекс
                        index = random_index();

                        tb1.Text = block[index];
                        tb2.Foreground = Brushes.Green;
                        tb4.Text = "Правильно: " + right.ToString() + " из: " + chet.ToString();
                    }
                    else
                    {
                        tb2.Foreground = Brushes.Red;
                    }
                }

                //черный цвет шрифта
                if (e.Key != Key.Enter || e.Key == Key.Back) { tb2.Foreground = Brushes.Black; }

                //подсказка
                if (e.Key == Key.RightShift) { tb2.Foreground = Brushes.Blue; tb2.Text = block[index - 1]; right--; }

                //читска tb2
                if (e.Key == Key.LeftCtrl) tb2.Text = "";
            }
            else
            {
                string s = tb2.Text;
                if (e.Key == Key.Enter)
                {
                    if (s == block[index + 1])
                    {
                        chet++;
                        right++;
                        block.RemoveAt(index);
                        block.RemoveAt(index);

                        //exit
                        if (block.Count == 0)
                        {
                            MessageBox.Show("Правильно: " + right.ToString() + " из: " + (chet + 1).ToString());
                            tb1.Text = "";
                            tb2.Text = "";
                            tb3.Text = "";
                            tb4.Text = "";
                            right = 0;
                            chet = 0;
                            return;
                        }

                        //случайный индекс
                        index = random_index();

                        tb1.Text = block[index];
                        tb2.Foreground = Brushes.Green;
                        tb4.Text = "Правильно: " + right.ToString() + " из: " + chet.ToString();
                    }
                    else
                    {
                        tb2.Foreground = Brushes.Red;
                    }
                }

                //черный цвет шрифта
                if (e.Key != Key.Enter || e.Key == Key.Back) { tb2.Foreground = Brushes.Black; }

                //подсказка
                if (e.Key == Key.RightShift) { tb2.Foreground = Brushes.Blue; tb2.Text = block[index + 1]; right--; }

                //читска tb2
                if (e.Key == Key.LeftCtrl) tb2.Text = "";
            }
        }

        private void rb1_Checked(object sender, RoutedEventArgs e)
        {
            rb_one = true;
        }

        private void rb2_Checked(object sender, RoutedEventArgs e)
        {
            rb_two = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            tb4.Text = "";
            right = 0;
            chet = 0;
            block.Clear();
            rb1.IsChecked = false; rb_one = false; rb1.IsEnabled = true;
            rb2.IsChecked = false; rb_two = false; rb2.IsEnabled = true;
        }
    }
}
