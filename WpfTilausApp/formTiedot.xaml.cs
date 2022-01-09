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
    /// Interaction logic for formTiedot.xaml
    /// </summary>
    public partial class formTiedot : Window
    {
        public formTiedot()
        {
            InitializeComponent();
        }
        private void BtnTilaukset_Click(object sender, RoutedEventArgs e)
        {
            formTilaukset frmTilaukset = new formTilaukset();
            frmTilaukset.Show();
        }

        private void BtnTilausrivit_Click(object sender, RoutedEventArgs e)
        {
            formTilausrivit frmTilausrivit = new formTilausrivit();
            frmTilausrivit.Show();
        }

        private void BtnAsiakkaat_Click(object sender, RoutedEventArgs e)
        {
            formAsiakkaat frmAsiakkaat = new formAsiakkaat();
            frmAsiakkaat.Show();
        }

        private void BtnKayttajat_Click(object sender, RoutedEventArgs e)
        {
            formKayttajat frmKayttajat = new formKayttajat();
            frmKayttajat.Show();
        }
        private void BtnPostitoimiPaikat_Click(object sender, RoutedEventArgs e)
        {
            formPostitoimiPaikat frmPostmp = new formPostitoimiPaikat();
            frmPostmp.Show();
        }

        private void BtnTuotteet_Click(object sender, RoutedEventArgs e)
        {
            formTuotteet frmTuotteet = new formTuotteet();
            frmTuotteet.Show();
        }
    }
}
