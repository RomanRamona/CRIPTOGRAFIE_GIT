using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace CryptoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateCombos();
        }

        private void PopulateCombos()
        {
            cbAlgorithm.ItemsSource = new string[] { "AES", "DES", "TripleDES", "Rijndael", "RC2" };
            cbAlgorithm.SelectedIndex = 0;
            cbMode.ItemsSource = Enum.GetValues(typeof(CipherMode));
            cbMode.SelectedItem = CipherMode.CBC;
            cbPadding.ItemsSource = Enum.GetValues(typeof(PaddingMode));
            cbPadding.SelectedItem = PaddingMode.PKCS7;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtFilePath.Text = openFileDialog.FileName;
        }

        private void GenerateKey_Click(object sender, RoutedEventArgs e) =>
            txtKey.Text = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

        private void GenerateIV_Click(object sender, RoutedEventArgs e) =>
            txtIV.Text = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

        private async void Process_Click(object sender, RoutedEventArgs e)
        {
            bool isEncrypt = (sender == btnEncrypt);
            string filePath = txtFilePath.Text;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Selectați un fișier valid!");
                return;
            }

            try
            {
                progressBar.Visibility = Visibility.Visible;
                progressBar.IsIndeterminate = true;
                lblStatus.Text = "Se procesează...";

                await Task.Run(() => PerformCryptography(filePath, isEncrypt));

                MessageBox.Show($"Operație finalizată cu succes!\nFișier: {filePath}.{(isEncrypt ? "enc" : "dec")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
            finally
            {
                progressBar.Visibility = Visibility.Hidden;
                lblStatus.Text = "";
            }
        }

        private void PerformCryptography(string filePath, bool encrypt)
        {
            // 1. Instanțiere Algoritm dinamic
            using SymmetricAlgorithm algo = cbAlgorithm.SelectedItem.ToString() switch
            {
                "AES" => Aes.Create(),
                "DES" => DES.Create(),
                "TripleDES" => TripleDES.Create(),
                "Rijndael" => Rijndael.Create(),
                "RC2" => RC2.Create(),
                _ => throw new Exception("Algoritm necunoscut")
            };

            // 2. Configurare moduri
            algo.Mode = (CipherMode)cbMode.SelectedItem;
            algo.Padding = (PaddingMode)cbPadding.SelectedItem;

            // Transformăm Key/IV din Base64 în byte[]
            algo.Key = Convert.FromBase64String(txtKey.Text);
            algo.IV = Convert.FromBase64String(txtIV.Text);

            string outputFileName = filePath + (encrypt ? ".enc" : ".dec");
            ICryptoTransform transformer = encrypt ? algo.CreateEncryptor() : algo.CreateDecryptor();

            using FileStream fsInput = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using FileStream fsOutput = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
            using CryptoStream cs = new CryptoStream(fsOutput, transformer, CryptoStreamMode.Write);

            // Buffer pentru fișiere mari
            byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = fsInput.Read(buffer, 0, buffer.Length)) > 0)
            {
                cs.Write(buffer, 0, bytesRead);
            }
        }
    }
}