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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTilausApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime tanaan = DateTime.Today;
        Decimal TilauksenSummaYht = 0;

        public MainWindow()
        {
            InitializeComponent();
            HaeAsiakkaat(); 

            #region
            HaeTuotteet(); //Täytetään Tuote -comboboxin sisältö
            dpTilausPvm.SelectedDate = tanaan;
            dpToimitusPvm.SelectedDate = tanaan.AddDays(14);
            AsetaTilausRiviGridinSarakkeet(); //Asetetaan tilausrivitaulukon sarakkeet
            #endregion
        }

        private void HaeAsiakkaat()
        {
            List<cbPairAsiakas> cbpairAsiakkaat = new List<cbPairAsiakas>(); 

            TilausDBEntities2 entities = new TilausDBEntities2();

            var asiakkaat = from asiak in entities.Asiakkaat
                            select asiak; //Haetaan asiakkaan tietokannasta LINQ -kyselyllä

            foreach (var asiakas in asiakkaat) //Käydään kaikki asiakkaat läpi silmukassa
            {
                cbpairAsiakkaat.Add(new cbPairAsiakas(asiakas.Nimi, asiakas.AsiakasID, asiakas.Osoite, asiakas.Postinumero)); //Viedään asiakkaat listalle
            }
            cbAsiakas.DisplayMemberPath = "asiakasNimi"; //Kerrotaan pudotusvalikolle (ComboBox) mikä kenttä näytetään
            cbAsiakas.SelectedValuePath = "asiakasNro"; //Kerrotaan pudotusvalikolle (ComboBox) missä kentässä on arvotieto (joka siis viedään myöhemmin tkantaan tilaukselle)
            cbAsiakas.SelectedValuePath = "asiakasPostinro";
            cbAsiakas.ItemsSource = cbpairAsiakkaat; //Kerrotaan, että pudotusvalikon tietolähde on cbpairAsiakkaat-lista
        }

        private void HaeTuotteet()
        {
            List<cbPairTuote> cbpairTuotteet = new List<cbPairTuote>(); //Luodaan olemassa olevien asiakasnimi-asiakasnumero -parien lista
            TilausDBEntities2 db = new TilausDBEntities2();

            var tuotteet = from prod in db.Tuotteet
                            orderby prod.Nimi
                            select prod; //Haetaan tuotteet tietokannasta LINQ -kyselyllä

            foreach (var tuote in tuotteet) //Käydään kaikki tuotteet läpi silmukassa
            {
                cbpairTuotteet.Add(new cbPairTuote(tuote.Nimi, tuote.TuoteID, (decimal)tuote.Ahinta)); //Viedään tuotteet listalle
            }
            cbTuote.DisplayMemberPath = "tuoteNimi"; //Kerrotaan pudotusvalikolle (ComboBox) mikä kenttä näytetään
            cbTuote.SelectedValuePath = "tuoteNro"; //Kerrotaan pudotusvalikolle (ComboBox) missä kentässä on arvotieto (joka siis viedään myöhemmin tkantaan tilaukselle)
            cbTuote.ItemsSource = cbpairTuotteet; //Kerrotaan, että pudotusvalikon tietolähde on cbpairTuotteet-lista
        }

        private void CbAsiakas_DropDownClosed(object sender, EventArgs e) //Tämä eventti laukeaa aina, kun pudotusvalikko sulkeutuu
        {
            cbPairAsiakas cbp = (cbPairAsiakas)cbAsiakas.SelectedItem;
            txtAsiakasNumero.Text = cbp.asiakasNro.ToString(); //Asiakasnumero on kannassa int
            txtAsiakasOsoite.Text = cbp.asiakasOsoite; //Asiakkaan osoite lisätty 22.11.2019 JSO
            txtPostinro.Text = cbp.asiakasPostinro; //Asiakkaan postinro lisätty 28.4.2020 JM
        }

        private void BtnTallenna_Click(object sender, RoutedEventArgs e)
        {
            TilausOtsikko Tilaus = new TilausOtsikko();
            Tilaus.AsiakasNumero = int.Parse(txtAsiakasNumero.Text); 
            Tilaus.ToimitusOsoite = txtToimitusOsoite.Text;
            Tilaus.Postinumero = txtToimituksenPostinumero.Text;
            Tilaus.TilausPvm = dpTilausPvm.SelectedDate.Value; 
            Tilaus.ToimitusPvm = dpToimitusPvm.SelectedDate.Value; 

            txtToimitusAika.Text = Tilaus.LaskeToimitusAika();
            txtTilausNumero.Text = VieTilausKantaan(Tilaus); 
        }

        private string VieTilausKantaan(TilausOtsikko Tilaus) //Huomaa, että rutiini palauttaa stringin (uuden tilauksen numeron)
        {
            try
            {
                TilausDBEntities2 entities = new TilausDBEntities2();
                Tilaukset dbItem = new Tilaukset()
                {
                    AsiakasID = Tilaus.AsiakasNumero, //Tilaus on tilausotisikko-tyyppinen olio, jonka tämä rutiini saa parametrinä kutsuvasta ohjelmasta
                    Toimitusosoite = Tilaus.ToimitusOsoite,
                    Postinumero = Tilaus.Postinumero,
                    Tilauspvm = Tilaus.TilausPvm,
                    Toimituspvm = Tilaus.ToimitusPvm
                };

                entities.Tilaukset.Add(dbItem);
                entities.SaveChanges();

                int id = dbItem.TilausID; //Haetaan juuri lisätyn rivin IDENTITEETTIsarakkeen arvo (eli PK)
                return id.ToString(); //Palautetaan onnistuneen lisäyksen merkiksi uuden tilauksen numero
            }
            catch (Exception)
            {
                return "0"; 
            }
        }

        private void CbTuote_DropDownClosed(object sender, EventArgs e)
        {
            cbPairTuote cbp = (cbPairTuote)cbTuote.SelectedItem;

            if(cbp != null)
            {
                txtTuoteNumero.Text = cbp.tuoteNro.ToString();
                txtAHinta.Text = cbp.aHinta.ToString();
            }
            else
            {
                MessageBox.Show("Tuotetta ei valittu");
            }


        }

        private void BtnLisaaRivi_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                    TilausRivi tilausRivi = new TilausRivi();
                    tilausRivi.TilausNumero = int.Parse(txtTilausNumero.Text);
                    tilausRivi.TuoteNumero = int.Parse(txtTuoteNumero.Text);
                    tilausRivi.TuoteNimi = cbTuote.Text;
                    tilausRivi.Maara = int.Parse(txtMaara.Text);
                    tilausRivi.AHinta = Convert.ToDecimal(txtAHinta.Text);
                    tilausRivi.TilausRiviNumero = VieTilausRiviKantaan(tilausRivi);

                    TilauksenSummaYht += tilausRivi.RiviSumma(); //Kuten tämä: TilauksenSummaYht = TilauksenSummaYht + TilausR.RiviSumma();
                    txtTilausSumma.Text = TilauksenSummaYht.ToString();
                    dgTilausRivit.Items.Add(tilausRivi);
            }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message);
            }
            TyhjennnaRivikentat();
        }

        private void TyhjennnaRivikentat()
        {
            txtTuoteNumero.Text = "";
            txtMaara.Text = "";
            txtRiviSumma.Text = "";
            txtMaara.Text = "";
            txtAHinta.Text = "";

        }




        private int VieTilausRiviKantaan(TilausRivi tilausRivi)
        {
            TilausDBEntities2 entities = new TilausDBEntities2();

            Tilausrivit dbItem = new Tilausrivit()
            {
                TilausID = tilausRivi.TilausNumero,
                TuoteID = tilausRivi.TuoteNumero,
                Maara = tilausRivi.Maara,
                Ahinta = tilausRivi.AHinta
            };

            entities.Tilausrivit.Add(dbItem);
            entities.SaveChanges();

            int id = dbItem.TilausriviID; //Haetaan juuri lisätyn rivin IDENTITEETTIsarakkeen arvo (eli PK)
            return id; //Paluuarvo
        }

        private void TxtMaara_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRiviSumma.Text = (decimal.Parse(txtAHinta.Text) * decimal.Parse(txtMaara.Text)).ToString();
        }

        private void AsetaTilausRiviGridinSarakkeet()
        {
            //Luodaan ikäänkuin ilmaan DataGridTextColumn -tyyppisiä olioita
            DataGridTextColumn colTilausRiviNumero = new DataGridTextColumn();
            DataGridTextColumn colTilausNumero = new DataGridTextColumn();
            DataGridTextColumn colTuoteNumero = new DataGridTextColumn();
            DataGridTextColumn colTuoteNimi = new DataGridTextColumn();
            DataGridTextColumn colMaara = new DataGridTextColumn();
            DataGridTextColumn colAHinta = new DataGridTextColumn();
            DataGridTextColumn colRivinSumma = new DataGridTextColumn();            
            //Oliot sidotaan tietyn nimiseen sarakkeeseen < --kohdistuu myöhemmin olion ominaisuuksiin, kunhan olio on ensin viety listalle
            colTilausRiviNumero.Binding = new Binding("TilausRiviNumero");
            colTilausNumero.Binding = new Binding("TilausNumero");
            colTuoteNumero.Binding = new Binding("TuoteNumero");
            colTuoteNimi.Binding = new Binding("TuoteNimi");
            colMaara.Binding = new Binding("Maara");
            colAHinta.Binding = new Binding("AHinta");
            colRivinSumma.Binding = new Binding("Summa");
            //DataGridin otsikot 
            colTilausRiviNumero.Header = "Tilausrivin numero";
            colTilausNumero.Header = "Tilauksen numero";
            colTuoteNumero.Header = "Tuotenumero";
            colTuoteNimi.Header = "Tuotenimi";
            colMaara.Header = "Määrä";
            colAHinta.Header = "A-Hinta";
            colRivinSumma.Header = "Rivin summa";
            //Edellä "ilmaan" luotujen sarakkeiden sijoitus konkreettiseen DataGridiin, joka on luotu formille
            dgTilausRivit.Columns.Add(colTilausRiviNumero);
            dgTilausRivit.Columns.Add(colTilausNumero);
            dgTilausRivit.Columns.Add(colTuoteNumero);
            dgTilausRivit.Columns.Add(colTuoteNimi);
            dgTilausRivit.Columns.Add(colMaara);
            dgTilausRivit.Columns.Add(colAHinta);
            dgTilausRivit.Columns.Add(colRivinSumma);
        }

        #region newWindow
        //https://youtu.be/7vEpc2Vjxo4
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

        #endregion

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

        private void BtnTiedot_Click(object sender, RoutedEventArgs e)
        {
            formTiedot frmTiedot = new formTiedot();
            frmTiedot.Show();

        }
    }
}
