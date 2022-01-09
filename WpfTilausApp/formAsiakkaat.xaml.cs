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
    /// Interaction logic for formAsiakkaat.xaml
    /// </summary>
    public partial class formAsiakkaat : Window
    {
        TilausDBEntities2 db = new TilausDBEntities2();
        public formAsiakkaat()
        {
            InitializeComponent();
            HaeAsiakkaat();
        }

        private void HaeAsiakkaat()
        {
            List<Asiakkaat> lstasiak = new List<Asiakkaat>();

            var asiak = from asiakkaat in db.Asiakkaat
                          select asiakkaat;

            dgAsiakkaat.ItemsSource = asiak.ToList();
        }


        private void BtnSuljeAsiakas_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDeleteAsiakas_Click(object sender, RoutedEventArgs e)
        {
            Asiakkaat asi = db.Asiakkaat.Find(int.Parse(txtPoistaAsiakasnumero.Text));
            if (asi != null)
            {
                db.Asiakkaat.Remove(asi);
                db.SaveChanges();
            }
            HaeAsiakkaat();
        }


        private void BtnLisaaAsiakas_Click(object sender, RoutedEventArgs e)
        {
            Asiakkaat asi = new Asiakkaat();
            asi.Nimi = txtAsiakasNimi.Text;
            asi.Osoite = txtOsoite.Text;
            asi.Postinumero = txtPostiNumero.Text;
            db.Asiakkaat.Add(asi);
            db.SaveChanges();
            HaeAsiakkaat();
        }

        private void DgAsiakkaat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAsiakkaat.SelectedIndex >= 0)
            {
                TextBlock AsiakasID = dgAsiakkaat.Columns[0].GetCellContent(dgAsiakkaat.Items[dgAsiakkaat.SelectedIndex]) as TextBlock;
                if (AsiakasID != null)
                    txtPoistaAsiakasnumero.Text = AsiakasID.Text;
                TextBlock AsiakasNimi = dgAsiakkaat.Columns[1].GetCellContent(dgAsiakkaat.Items[dgAsiakkaat.SelectedIndex]) as TextBlock;
                if (AsiakasNimi != null)
                    txtPoistaAsiakasNimi.Text = AsiakasNimi.Text;
            }
        }

        private void BtnTallennaAsiakas_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            MessageBox.Show("Tiedot tallennettu");

        }
    }
}


