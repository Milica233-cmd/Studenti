using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Studenti
{
    /// <summary>
    /// Interaction logic for Rok.xaml
    /// </summary>
    public partial class Rok : Window
    {
        public Rok()
        {
            InitializeComponent();
            Podaci.MojaKonekcija = new SqlConnection(@"Server=DESKTOP-839UVL6;Database=Studenti;Trusted_Connection=True;");
        }
        private DataTable dt;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (dgRok.SelectedIndex == -1)
            {
                btnIzmeni.IsEnabled = false;
                btnObrisi.IsEnabled = false;
                btnOdustani.IsEnabled = false;

            }
            else
            {
                btnIzmeni.IsEnabled = true;
                btnObrisi.IsEnabled = true;
                btnOdustani.IsEnabled = true;
            }
            dt = new DataTable("dt");
            Metode.PopuniDataTabeluSelectom("IspitSelect", dt);
            cboIspit.ItemsSource = dt.DefaultView;
            cboIspit.DisplayMemberPath = "Naziv";
            cboIspit.SelectedValuePath = "ID";

            dt = new DataTable("dt");
            Metode.PopuniDataTabeluSelectom("RokSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgRok, dt);
        }
        private void OmogucavanjeUnosa()
        {
            btnUnesi.IsEnabled = true;
            btnIzmeni.IsEnabled = true;
            btnObrisi.IsEnabled = true;
        }
        private void cboIspit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmogucavanjeUnosa();
            if (cboIspit.SelectedValue == null)
                return;

            int ispitId = Convert.ToInt32(cboIspit.SelectedValue);
        }

        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {
            bool skola = false;
            if (rbtnOsnovna.IsChecked == true)
            {
                skola = false;
            }
            else
            {
                skola = true;
            }
            dt = new DataTable("dt");
            string[] ParNazivi = {"@IspitID", "@Datum", "@Skola", "@Podatak" };
            DbType[] ParTipovi = { DbType.Int32, DbType.DateTime, DbType.Boolean, DbType.Boolean };
            object[] ParVrednosti = { cboIspit.SelectedValue, dtpDatum.SelectedDate, skola, chkPodatak.IsChecked == true };
            Metode.IzvrsiKomanduSaOUT("RokInsert", ParNazivi, ParTipovi, ParVrednosti);

            Metode.PopuniDataTabeluSelectom("RokSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgRok, dt);

            CiscenjeTxtBoxova();
        }
    

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            bool skola = false;
            if (rbtnOsnovna.IsChecked == true)
            {
                skola = false;
            }
            else
            {
                skola = true;
            }
            dt = new DataTable("dt");
            string[] ParNazivi = { "@ID", "@IspitID", "@Datum", "@Skola", "@Podatak" };
            DbType[] ParTipovi = { DbType.Int32, DbType.Int32, DbType.DateTime, DbType.Boolean, DbType.Boolean};
            object[] ParVrednosti = { Podaci.IdTekucegReda, cboIspit.SelectedValue, dtpDatum.SelectedDate, skola, chkPodatak.IsChecked == true};
            //object[] OutParam = Metode.IzvrsiKomanduSaOUT("RokUpdate", ParNazivi, ParTipovi, ParVrednosti);
            //string poruka = "";

            Metode.IzvrsiKomandu("RokUpdate", ParNazivi, ParTipovi, ParVrednosti);
            //else
            //{
                Metode.PopuniDataTabeluSelectom("RokSelect", dt);
                Metode.PunjenjeGridaDataTabelom(dgRok, dt);

                CiscenjeTxtBoxova();
                //txtIme.Focus();
            //}
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable("dt");
            string[] ParNazivi = { "@ID" };
            DbType[] ParTipovi = { DbType.Int32 };
            object[] ParVrednosti = { Podaci.IdTekucegReda };
            SqlCommand cmd = Metode.KreiranjeKomande("RokDelete", ParNazivi, ParTipovi, ParVrednosti);
            Metode.IzvrsiKomandu(cmd);
            Metode.PopuniDataTabeluSelectom("RokSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgRok, dt);

            CiscenjeTxtBoxova();
            cboIspit.Focus();
        }

        private void btnOdustani_Click(object sender, RoutedEventArgs e)
        {
            CiscenjeTxtBoxova();
        }

        private void CiscenjeTxtBoxova()
        {
            cboIspit.SelectedIndex = -1;
            dtpDatum.SelectedDate = null;
            rbtnOsnovna.IsChecked = false;
            rbtnSrednja.IsChecked = false;
            chkPodatak.IsChecked = false;
            dgRok.SelectedIndex = -1;
            cboIspit.Focus();
        }
        private void btnIzlaz_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            this.Close();
            MainWindow.ShowDialog();
        }

        private void dgRok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRok.SelectedItem != null)
            {
                //dt = new DataTable("dt");
                DataRowView drv = (DataRowView)dgRok.SelectedItem;
                Podaci.IdTekucegReda = (int)drv["ID"];
                //txtOcena.Text = drv["Ocena"].ToString();
                //txtNaziv.Text = drv["Naziv"].ToString();
                cboIspit.SelectedValue = drv["IspitID"];
                dtpDatum.SelectedDate = Convert.ToDateTime(drv["Datum"]);

                if ((bool)drv["Skola"] == true)
                {
                    rbtnOsnovna.IsChecked = true;
                    rbtnSrednja.IsChecked = false;
                }
                else
                {
                    rbtnOsnovna.IsChecked = false;
                    rbtnSrednja.IsChecked = true;
                }
                chkPodatak.IsChecked = (bool)drv["Podatak"];
                btnUnesi.IsEnabled = false;
                btnIzmeni.IsEnabled = true;
                btnObrisi.IsEnabled = true;
                btnOdustani.IsEnabled = true;
            }
        }
    

        private void dgRok_AutoGeneratedColumns(object sender, EventArgs e)
        {
        //dgRok.Columns[0].Visibility = System.Windows.Visibility.Hidden;
        //dgRok.Columns[1].Visibility = System.Windows.Visibility.Hidden;
        //dgRok.Columns[5].Visibility = System.Windows.Visibility.Hidden;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
