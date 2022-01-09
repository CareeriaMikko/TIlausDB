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
    /// Interaction logic for formTilaukset.xaml
    /// </summary>
    public partial class formTilaukset : Window
    {
        TilausDBEntities2 db = new TilausDBEntities2();
        public formTilaukset()
        {
            InitializeComponent();
            HaeTilaukset();
        }

        private void HaeTilaukset()
        {
            List<Tilaukset> lsttilaus = new List<Tilaukset>();

            var tilau = from tilaukset in db.Tilaukset
                        select tilaukset;

            dgTilaukset.ItemsSource = tilau.ToList();
            dgTilaukset.IsReadOnly = true;
        }
        private void DgTilaukset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTilaukset.SelectedIndex >= 0)
            {
                TextBlock TilausID = dgTilaukset.Columns[0].GetCellContent(dgTilaukset.Items[dgTilaukset.SelectedIndex]) as TextBlock;
                if (TilausID != null)
                    txtTilausNumero.Text = TilausID.Text;
                TextBlock AsiakasID = dgTilaukset.Columns[1].GetCellContent(dgTilaukset.Items[dgTilaukset.SelectedIndex]) as TextBlock;
                if (AsiakasID != null)
                    txtAsiakasNumero.Text = AsiakasID.Text;
            }
        }

        private void BtnSuljeTilaukset_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDeleteTilaukset_Click(object sender, RoutedEventArgs e)
        {
            string message = "Oletko varma, että haluat poistaa tilauksen?";
            string caption = "Tilauksen poistaminen";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                Tilaukset tila = db.Tilaukset.Find(int.Parse(txtTilausNumero.Text));
                if (tila != null)
                {
                    db.Tilaukset.Remove(tila);
                    HaeTilaukset();
                }
            }
            else
            {
                MessageBox.Show("Et voi poistaa koko tilausta, koska se sisältää aktiivia tilausrivejä. Poista ensin tilausrivit.");
                HaeTilaukset();
            }
            db.SaveChanges();
        }

        private void BtnTallennaTilaukset_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            MessageBox.Show("Tiedot tallennettu");
            dgTilaukset.IsReadOnly = true;

        }

        private void BtnMuokkaaTilaukset_Click(object sender, RoutedEventArgs e)
        {
            dgTilaukset.IsReadOnly = false;
        }
    }
}

