using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace HashCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateAlgorithms();
        }

        private void PopulateAlgorithms()
        {
            cbHashAlgorithm.ItemsSource = new string[]
            {
                "MD5", "SHA1", "SHA256", "SHA384", "SHA512",
                "SHA3_256", "SHA3_384", "SHA3_512", "RIPEMD160"
            };
            cbHashAlgorithm.SelectedIndex = 2; // Default SHA256
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtFilePath.Text = openFileDialog.FileName;
        }

        private async void Calculate_Click(object sender, RoutedEventArgs e)
        {
            string path = txtFilePath.Text;
            string algo = cbHashAlgorithm.SelectedItem.ToString();

            if (!File.Exists(path))
            {
                MessageBox.Show("Vă rugăm selectați un fișier valid.");
                return;
            }

            try
            {
                pbStatus.Visibility = Visibility.Visible;
                txtResult.Text = "Se calculează...";

                // Execuția asincronă a calculului
                string hashResult = await Task.Run(() => ComputeFileHash(path, algo));

                txtResult.Text = hashResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la calcul: {ex.Message}");
                txtResult.Text = string.Empty;
            }
            finally
            {
                pbStatus.Visibility = Visibility.Hidden;
            }
        }

        private string ComputeFileHash(string filePath, string algorithmName)
        {
            using HashAlgorithm hasher = algorithmName switch
            {
                "MD5" => MD5.Create(),
                "SHA1" => SHA1.Create(),
                "SHA256" => SHA256.Create(),
                "SHA384" => SHA384.Create(),
                "SHA512" => SHA512.Create(),
                "SHA3_256" => SHA3_256.Create(),
                "SHA3_384" => SHA3_384.Create(),
                "SHA3_512" => SHA3_512.Create(),
                "RIPEMD160" => (HashAlgorithm)Activator.CreateInstance(Type.GetType("System.Security.Cryptography.RIPEMD160Managed") ?? throw new Exception("Algoritm indisponibil")),
                _ => throw new ArgumentException("Algoritm nesuportat")
            };

            using FileStream stream = File.OpenRead(filePath);
            byte[] hashBytes = hasher.ComputeHash(stream);

            // Convertire în format Hexazecimal
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString().ToUpper();
        }
    }
}