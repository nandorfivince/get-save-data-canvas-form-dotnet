using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

using Path = System.IO.Path;

namespace GetAndSaveMyData
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Biztosan kilép?", "Megerősítés", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }

        }

        private void btnHello_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to C#!");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                CopyForm();
            }
            else
            {
                MessageBox.Show("Töltse ki a formot!");
            }
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(txtFirstName.Text) &&
                !string.IsNullOrWhiteSpace(txtLastName.Text) &&
                !string.IsNullOrWhiteSpace(txtEmail.Text) &&
                IsValidEmail_Simple();
        }

        private bool IsValidEmail_Simple()
        {
            //var alma = string.Concat("alma", "alma", "körte");
            //StringBuilder sb = new StringBuilder("alma");
            //sb.Append("alma");
            //sb.Append("körte");
            //var eredmeny = sb.ToString();
            //var interpolacio = $"Gépem neve: {Environment.MachineName}";

            return txtEmail.Text.Count(c => c == '@') == 1;
        }

        private bool IsValidEmail_RegExp()
        {

            throw new NotImplementedException();

        }

        private bool IsValidEmail_SystemNetHttp()
        {

            throw new NotImplementedException();

        }

        private void CopyForm()
        {
            txtEmail2.Text = txtEmail.Text;
            txtLastName2.Text = txtLastName.Text;
            txtFirstName2.Text = txtFirstName.Text;
            btnSaveToFile.IsEnabled = true;
            tabCtrl.SelectedItem = tpMasodik;
        }

        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            var tempPath = Path.GetTempPath();

            var myDirectory = Path.Combine(tempPath, "MyDir");

            var myFileNaem = Path.ChangeExtension(Guid.NewGuid().ToString(), ".txt");

            var savePath = Path.Combine(myDirectory, myFileNaem);

            if (!Directory.Exists(myDirectory))
            {
                try
                {
                    Directory.CreateDirectory(myDirectory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                finally
                {
                    // 
                }
            }

            //File.WriteAllText(savePath, "Hello World!");
            //File.AppendAllText(savePath, "");

            using (var sw = new StringWriter())
            {
                sw.WriteLine("Családnév: {0}", txtLastName.Text);
                sw.WriteLine($"Keresztnév: {txtFirstName.Text}");
                sw.WriteLine(string.Concat("Email", ":", " ", txtEmail.Text));
                File.WriteAllText(savePath, sw.ToString());
            }

            Process.Start("notepad", savePath);

            MyData md = new MyData
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text
            };

            var jsonFile = Path.ChangeExtension(savePath, ".json");
            var jsonString = JsonConvert.SerializeObject(md);
            File.WriteAllText(jsonFile, jsonString);
            Process.Start("notepad", jsonFile);

            // TODO: XML

            // TODO: Binary

            tabCtrl.SelectedItem = tpHarmadik;

            var readData = JsonConvert.DeserializeObject<MyData>(File.ReadAllText(jsonFile));
            txtFirstName3.Text = readData.FirstName;
            txtLastName3.Text = readData.LastName;
            txtEmail3.Text = readData.Email;
        }
    }
}
