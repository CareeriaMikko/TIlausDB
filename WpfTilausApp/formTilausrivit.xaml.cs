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
    /// Interaction logic for formTilausrivit.xaml
    /// </summary>
    public partial class formTilausrivit : Window
    {
        TilausDBEntities2 db = new TilausDBEntities2();
        public formTilausrivit()
        {
            InitializeComponent();
            HaeTilausRivit();
        }

        private void HaeTilausRivit()
        {
            List<Tilausrivit> lstrivit = new List<Tilausrivit>();

            var rivi = from rivit in db.Tilausrivit
                        select rivit;

            dgTilausRivit.ItemsSource = rivi.ToList();
            dgTilausRivit.IsReadOnly = true;
        }
        private void DgTilausRivit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTilausRivit.SelectedIndex >= 0)
            {
                TextBlock TilausriviID = dgTilausRivit.Columns[0].GetCellContent(dgTilausRivit.Items[dgTilausRivit.SelectedIndex]) as TextBlock;
                if (TilausriviID != null)
                    txtTilausRiviID.Text = TilausriviID.Text;
                TextBlock TilausID = dgTilausRivit.Columns[1].GetCellContent(dgTilausRivit.Items[dgTilausRivit.SelectedIndex]) as TextBlock;
                if (TilausID != null)
                    txtTilausID.Text = TilausID.Text;
            }
        }

        private void BtnSuljeTilausRivit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDeleteTilausRivit_Click(object sender, RoutedEventArgs e)
        {
            string message = "Oletko varma, että haluat poistaa tilausrivin?";
            string caption = "Tilausrivin poisto";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                Tilausrivit rivi = db.Tilausrivit.Find(int.Parse(txtTilausRiviID.Text));
                if (rivi != null)
                {
                    db.Tilausrivit.Remove(rivi);
                    db.SaveChanges();
                    HaeTilausRivit();
                }
            }
            else
            {
                HaeTilausRivit();
            }

        }

        private void BtnTallennaTilausRivit_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            MessageBox.Show("Tiedot tallennettu");
            dgTilausRivit.IsReadOnly = true;

        }

        private void BtnMuokkaaTilausRivit_Click(object sender, RoutedEventArgs e)
        {
            dgTilausRivit.IsReadOnly = false;
        }
    }
}

