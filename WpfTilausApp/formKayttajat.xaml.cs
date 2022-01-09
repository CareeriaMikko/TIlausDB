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

namespace WpfTilausApp
{
    /// <summary>
    /// Interaction logic for formKayttajat.xaml
    /// </summary>
    public partial class formKayttajat : Window
    {
        TilausDBEntities2 db = new TilausDBEntities2();
        public formKayttajat()
        {
            InitializeComponent();
            HaeKayttajat();
        }
        private void HaeKayttajat()
        {
            List<Logins> lstkayttajat = new List<Logins>();

            var kayttaja = from kayttajat in db.Logins
                           select kayttajat;

            dgKayttajat.ItemsSource = kayttaja.ToList();
            dgKayttajat.IsReadOnly = true;
        }

        private void dgKayttajat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKayttajat.SelectedIndex >= 0)
            {
                TextBlock LoginId = dgKayttajat.Columns[0].GetCellContent(dgKayttajat.Items[dgKayttajat.SelectedIndex]) as TextBlock;
                if (LoginId != null)
                    txtLoginID.Text = LoginId.Text;
                TextBlock UserName = dgKayttajat.Columns[1].GetCellContent(dgKayttajat.Items[dgKayttajat.SelectedIndex]) as TextBlock;
                if (UserName != null)
                    txtKayttajaNimi.Text = UserName.Text;
            }
        }

        private void btnTallennaKayttajat_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            MessageBox.Show("Tiedot tallennettu");
            dgKayttajat.IsReadOnly = true;
        }

        private void btnDeleteKayttajat_Click(object sender, RoutedEventArgs e)
        {
            string message = "Oletko varma, että haluat poistaa käyttäjän?";
            string caption = "Käyttäjän poistaminen";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                Logins log = db.Logins.Find(int.Parse(txtLoginID.Text));
                if (log != null)
                {
                    db.Logins.Remove(log);
                    db.SaveChanges();
                    HaeKayttajat();
                    TyhjennaKayttajaKentat();
                }
            }
            else
            {
                HaeKayttajat();
            }
        }

        private void btnSuljeKayttajat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMuokkaaKayttajat_Click(object sender, RoutedEventArgs e)
        {
            dgKayttajat.IsReadOnly = false;
        }

        private void btnLisaaKayttajat_Click(object sender, RoutedEventArgs e)
        {
            Logins kay = new Logins();
            kay.UserName = txtKayttajaNimiLisaa.Text;
            kay.PassWord = txtLoginIDLisaa.Text;
            db.Logins.Add(kay);
            db.SaveChanges();
            HaeKayttajat();
            TyhjennaKayttajaKentat();

        }

        private void TyhjennaKayttajaKentat()
        {
            txtKayttajaNimi.Text = "";
            txtKayttajaNimiLisaa.Text = "";
            txtLoginID.Text = "";
            txtLoginIDLisaa.Text = "";
        }

    }      
    
}
