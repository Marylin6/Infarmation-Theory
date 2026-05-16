using DSA;
using Microsoft.Win32;
using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace DSA
{
    public partial class MainWindow : Window
    {
        private string signFilePath = "";
        private string verifyFilePath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectSignFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                signFilePath = dlg.FileName;
                signFileBox.Text = signFilePath;
            }
        }

        private void SelectVerifyFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                verifyFilePath = dlg.FileName;
                verifyFileBox.Text = verifyFilePath;
            }
        }

        private void GenerateKeys_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BigInteger p = BigInteger.Parse(pBox.Text);
                BigInteger q = BigInteger.Parse(qBox.Text);
                BigInteger h = BigInteger.Parse(hBox.Text);
                BigInteger x = BigInteger.Parse(xBox.Text);

                if (!Validator.ValidateParameters(p, q, h, x))
                    return;

                BigInteger g = DsaSigner.CalculateG(p, q, h);

                if (g <= 1)
                {
                    MessageBox.Show("g must be > 1");
                    return;
                }

                BigInteger y = MathUtils.ModPow(g, x, p);

                gBox.Text = g.ToString();
                yBox.Text = y.ToString();

                logBox.AppendText("Keys generated\n");
                logBox.AppendText($"g = {g}\n");
                logBox.AppendText($"y = {y}\n\n");
            }
            catch
            {
                MessageBox.Show("Invalid input");
            }
        }


        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(signFilePath))
                {
                    MessageBox.Show("Select file");
                    return;
                }

                BigInteger p = BigInteger.Parse(pBox.Text);
                BigInteger q = BigInteger.Parse(qBox.Text);
                BigInteger h = BigInteger.Parse(hBox.Text);
                BigInteger x = BigInteger.Parse(xBox.Text);
                BigInteger k = BigInteger.Parse(kBox.Text);

                if (!Validator.ValidateParameters(p, q, h, x))
                    return;

                if (!Validator.ValidateK(k, q))
                    return;

                BigInteger g = DsaSigner.CalculateG(p, q, h);

                string text = File.ReadAllText(signFilePath, Encoding.UTF8);
                text = text.TrimEnd('\r', '\n');

                SignatureResult result = DsaSigner.Sign(text, p, q, g, x, k);

                if (result.R == 0 || result.S == 0)
                {
                    MessageBox.Show("r or s equals 0. Enter another k.");
                    return;
                }

                hashBox.Text = result.Hash.ToString();
                rBox.Text = result.R.ToString();
                sBox.Text = result.S.ToString();

                SaveFileDialog dlg = new SaveFileDialog();

                if (dlg.ShowDialog() != true)
                    return;

                string path = dlg.FileName;

                using var writer = new StreamWriter(path, false, Encoding.UTF8);
                writer.Write(text);
                if (!text.EndsWith(Environment.NewLine))
    writer.WriteLine();

                writer.WriteLine($"{result.R} {result.S}");

                logBox.AppendText("File signed\n");
                logBox.AppendText($"Hash = {result.Hash}\n");
                logBox.AppendText($"r = {result.R}\n");
                logBox.AppendText($"s = {result.S}\n\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(verifyFilePath))
                {
                    MessageBox.Show("Select file");
                    return;
                }

                BigInteger p = BigInteger.Parse(pBox.Text);
                BigInteger q = BigInteger.Parse(qBox.Text);
                BigInteger h = BigInteger.Parse(hBox.Text);
                BigInteger x = BigInteger.Parse(xBox.Text);

                BigInteger g = DsaSigner.CalculateG(p, q, h);
                BigInteger y = MathUtils.ModPow(g, x, p);

                var fullText = File.ReadAllText(verifyFilePath, Encoding.UTF8);
                fullText = fullText.TrimEnd('\r', '\n');
                int lastNewLine = fullText.LastIndexOf(Environment.NewLine);

                if (lastNewLine < 0)
                {
                    MessageBox.Show("Invalid file format");
                    return;
                }

                string message = fullText.Substring(0, lastNewLine);
                string signatureLine = fullText.Substring(lastNewLine).Trim();

                var parts = signatureLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    MessageBox.Show("Invalid signature format");
                    return;
                }

                BigInteger r = BigInteger.Parse(parts[0]);
                BigInteger s = BigInteger.Parse(parts[1]);

                VerificationResult result =
                    DsaVerifier.Verify(
                        message,
                        r,
                        s,
                        p,
                        q,
                        g,
                        y
                    );

                verificationBox.Clear();

                verificationBox.AppendText($"Hash = {result.Hash}\n");
                verificationBox.AppendText($"w = {result.W}\n");
                verificationBox.AppendText($"u1 = {result.U1}\n");
                verificationBox.AppendText($"u2 = {result.U2}\n");
                verificationBox.AppendText($"v = {result.V}\n");
                verificationBox.AppendText($"r = {r}\n");

                verificationResult.Text =
                    result.IsValid
                    ? "SIGNATURE IS VALID"
                    : "SIGNATURE IS INVALID";

                logBox.AppendText("Verification completed\n\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}