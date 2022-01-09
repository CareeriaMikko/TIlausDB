using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace WpfTilausApp
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var username = txtKayttajanimi.Text;
            var password = txtSalasana.Text;
            using (TilausDBEntities2 context = new TilausDBEntities2())
            {
                var user = context.Logins.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    if (user.PassWord == password)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Käyttäjätunnus tai salasana virheellinen");
                }
            }

  




        }
    }
}
