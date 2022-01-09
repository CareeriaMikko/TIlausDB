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
    /// Interaction logic for formPostitoimiPaikat.xaml
    /// </summary>
    public partial class formPostitoimiPaikat : Window
    {
        TilausDBEntities2 db = new TilausDBEntities2();
        public formPostitoimiPaikat()
        {
            InitializeComponent();
            HaePostitoimipaikat();
        }

        private void HaePostitoimipaikat()
        {
            List<Postitoimipaikat> lstPosTmp = new List<Postitoimipaikat>();

            var postmpt = from posnot in db.Postitoimipaikat
                          select posnot;

            dgPostitoimiPaikat.ItemsSource = postmpt.ToList();
        }

        private void BtnSulje_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Postitoimipaikat poss = db.Postitoimipaikat.Find(txtPoistaPostinumero.Text);
            if (poss != null)
            {
                db.Postitoimipaikat.Remove(poss);
                db.SaveChanges();
            }
            HaePostitoimipaikat();
            TyhjennaPostikentat();
        }


        private void BtnLisaa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Postitoimipaikat poss = new Postitoimipaikat();
                poss.Postinumero = txtPostiNumero.Text;
                poss.Postitoimipaikka = txtPostitoimipaikka.Text;
                db.Postitoimipaikat.Add(poss);
                db.SaveChanges();
                HaePostitoimipaikat();
                TyhjennaPostikentat();

            }
            catch (Exception)
            {

                MessageBox.Show("Tarkista postinumero");
            }



        }

        private void DgPostitoimiPaikat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPostitoimiPaikat.SelectedIndex >= 0)
            {
                TextBlock Postinumero = dgPostitoimiPaikat.Columns[0].GetCellContent(dgPostitoimiPaikat.Items[dgPostitoimiPaikat.SelectedIndex]) as TextBlock;
                if (Postinumero != null)
                    txtPoistaPostinumero.Text = Postinumero.Text;
                TextBlock Postitoimipaikka = dgPostitoimiPaikat.Columns[1].GetCellContent(dgPostitoimiPaikat.Items[dgPostitoimiPaikat.SelectedIndex]) as TextBlock;
                if (Postitoimipaikka != null)
                    txtPoistaPostitoimipaikka.Text = Postitoimipaikka.Text;
            }
        }

        private void BtnTallenna_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void TyhjennaPostikentat()
        {
            txtPostiNumero.Text = "";
            txtPostitoimipaikka.Text = "";
            txtPoistaPostinumero.Text = "";
            txtPoistaPostitoimipaikka.Text = "";
        }

    }
}
