using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Translate
{
    public partial class MainWindow : Window
    {


        static int capacity = 2000;
        static int blockAmount = 20;
        static int size_block = 100;

        List<string> block = new List<string>();
        List<string> lib = new List<string>();
        List<string> wrong_list = new List<string>();

        int index = 1;
        int right = 0;
        int chet = 0;

        public MainWindow()
        {
            InitializeComponent();
            read("library.txt");
        }

        //функция чтения
        public void read(string path)
        {
            lib = new List<string>(capacity);
            if (System.IO.File.Exists(path))
            {
                using (var f = new StreamReader(path))
                {
                    string? s;
                    while ((s = f.ReadLine()) != null)
                    {
                        if (s != "")
                            lib.Add(s);
                    }
                }
            }
            else
            {
                throw new Exception($"file {path} does not exist");
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void wr()
        {
            var newList = new List<string>(wrong_list.Distinct());
            Window1 win = new Window1();
            win.tb_list.Text = "";
            for (int i = 0; i < newList.Count; i += 2)
            {
                win.tb_list.Text += newList[i];
                win.tb_list.Text += " -> ";
                win.tb_list.Text += newList[i + 1];
                win.tb_list.Text += "\r\n";
            }
            win.Show();
            wrong_list.Clear();
        }

        public void add_50_words_to_block(int blockIndex)
        {
            for (int l = (blockIndex - 1) * 100; l < blockIndex * size_block; l++)
            {
                block.Add(lib[l]);
            }
        }

        public int random_index()
        {
            Random r = new Random();
            int index = 2 * r.Next(0, block.Count / 2);
            index += rb1.IsChecked.HasValue && rb1.IsChecked.Value ? 1 : 0;
            return index;
        }
        public void bt1_Click(object sender, RoutedEventArgs e)
        {
            //проверка пустоты ТБ - четный индекс это англ слово
            if (int.TryParse(tb3.Text, out int number_block))
            {
                if (number_block < 1 || blockAmount < number_block)
                {
                    MessageBox.Show($"значение должно быть от 1 до {blockAmount}!");
                    return;
                }
            }
            else if (tb3.Text == "")
            {
                var r = new Random();
                number_block = r.Next(blockAmount) + 1;
                tb3.Text = number_block.ToString();
            }

            //Добавляем в список 50 слов
            add_50_words_to_block(number_block);

            //вывод первого слова
            index = random_index();
            tb1.Text = block[index];

            rb1.IsEnabled = false;
            rb2.IsEnabled = false;
        }

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (rb1.IsChecked.HasValue && rb1.IsChecked.Value)
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
                            MessageBox.Show("Правильно: " + right.ToString() + " из: " + chet.ToString());
                            tb1.Text = "";
                            tb2.Text = "";
                            tb3.Text = "";
                            tb4.Text = "";

                            right = 0;
                            chet = 0;
                            wr();
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
                if (e.Key == Key.RightShift) { tb2.Foreground = Brushes.Blue; tb2.Text = block[index - 1]; right--; wrong_list.Add(block[index]); wrong_list.Add(block[index - 1]); }

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
                            MessageBox.Show("Правильно: " + right.ToString() + " из: " + chet.ToString());
                            tb1.Text = "";
                            tb2.Text = "";
                            tb3.Text = "";
                            tb4.Text = "";

                            right = 0;
                            chet = 0;
                            wr();
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
                if (e.Key == Key.RightShift) { tb2.Foreground = Brushes.Blue; tb2.Text = block[index + 1]; right--; wrong_list.Add(block[index]); wrong_list.Add(block[index + 1]); }

                //читска tb2
                if (e.Key == Key.LeftCtrl) tb2.Text = "";
            }
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
            rb1.IsEnabled = true;
            rb2.IsEnabled = true;
        }
    }
}
