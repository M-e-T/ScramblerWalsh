using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using ScramblerWalsh.Interface;
using ScramblerWalsh.Model;

namespace ScramblerWalsh
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TypeCrypt TypeCrypt;
        private int[] key;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            string input = TextBoxInputFile.Text;
            string output = TextBoxOutputFile.Text;

            if (IsValidPath(input) == false || IsValidPath(output) == false)
            {
                MessageBox.Show("Невірно вказаний шлях файлу!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            DateTime timeStart = DateTime.Now;

            IEncrypting encrypting = new Encrypting(new CryptoKey(key, TypeCrypt));
            encrypting.Progress += (val) => {
                Dispatcher.Invoke(() =>
                {
                    ProgressBarProgress.Value = val;
                    Label_progres.Content = val + "%";
                });
            };

            if (RadiobuttonEncrypt.IsChecked == true)
            {
                await Task.Run(() =>
                {
                    encrypting.Encrypt(input, output, TypeCrypt);
                });
                LabelResult.Content = "Файл зашифровано";
                LabelResult.Foreground = Brushes.Green;
            }
            else
            {
                try
                {
                    await Task.Run(() =>
                    {
                        encrypting.Decrypt(input, output, TypeCrypt);
                    });
                    LabelResult.Content = "Файл розшифровано";
                    LabelResult.Foreground = Brushes.Green;
                }
                catch (Exception)
                {
                    MessageBox.Show("Неможливо розшифрувати файл!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Information);
                    LabelResult.Content = "Помилка!";
                    LabelResult.Foreground = Brushes.Red;
                }
            }
            ProgressBarProgress.Value = 0;
            Label_progres.Content = "";
            Label_time.Content = (DateTime.Now - timeStart).ToString(@"hh\:mm\:ss\:fff");
        }
        private void ButtonInputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "";
            dlg.Filter = "(*.txt)|*.txt|(*.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                TextBoxInputFile.Text = dlg.FileName;
                TextBoxOutputFile.Text = dlg.FileName;
            }
        }
        private void ButtonOutputFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "";
            dlg.Filter = "(*.txt)|*.txt|(*.docx)|*.docx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                TextBoxOutputFile.Text = dlg.FileName;
            }
        }
        private void TextBoxInputFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox texBox = sender as TextBox;
            if (IsValidPath(texBox.Text) == false)
            {
                texBox.BorderBrush = Brushes.Red;
                LabelResult.Content = "Невірний шлях до файлу!";
                LabelResult.Foreground = Brushes.Red;
            }
            else
            {
                LabelResult.Content = "";
                LabelResult.Foreground = Brushes.Green;
                texBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0078D7");
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            TypeCrypt = (TypeCrypt)Convert.ToInt16(rb.Tag);
            if (rb.Tag.ToString() == "4")
                GroupBoxKey.Visibility = Visibility.Visible;
            else
                GroupBoxKey.Visibility = Visibility.Hidden;
        }
        private bool IsValidPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;
            bool isValidPath = filePath.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            bool isValidDirecroty = Directory.Exists(Path.GetDirectoryName(filePath));
            return isValidPath & isValidDirecroty;
        }

        private void ButtonGenerateKey_Click(object sender, RoutedEventArgs e)
        {
            key = new KeyPermutation().Key;
            TextBoxKey.Text = string.Join(" ",key);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RadioButtonAdamar.IsChecked = true;
        }
        private void Window_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LabelResult.Content = "";
        }
    }
}
