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
using Microsoft.Reporting.WinForms;

namespace Studenti
{
    public partial class Profesori : Window
    {
        List<Profesor> lista = new List<Profesor>();

        public Profesori()
        {
            InitializeComponent();
        }
        private void UcitajIzvestaj()
        {

            SqlConnection con = new SqlConnection(
         @"Server=DESKTOP-839UVL6;Database=Studenti;Trusted_Connection=True;");

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT Ime, Prezime, Predmet FROM Profesori", con);

            da.Fill(dt);

            //MessageBox.Show(dt.Rows.Count.ToString()); // proveri da radi

            reportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.LocalReport.ReportPath =
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProfesoriIzvestaj.rdlc");

            ReportParameter rp = new ReportParameter("Datum", DateTime.Now.ToString("dd.MM.yyyy"));
            reportViewer1.LocalReport.SetParameters(rp);

            reportViewer1.RefreshReport();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (dgProfesori.SelectedIndex == -1)
            //{
            //    btnIzmeni.IsEnabled = false;
            //    btnUnesi.IsEnabled = false;
            //}
            //else
            {
                btnIzmeni.IsEnabled = true;
          
            }
            dt = new DataTable("dt");
            Metode.PopuniDataTabeluSelectom("ProfesoriSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgProfesori, dt);
            CiscenjeTxtBoxova();
        }

        
        //private void Dodaj_Click(object sender, RoutedEventArgs e)
        //{
        //    Profesor p = new Profesor();

        //    p.Ime = txtIme.Text;
        //    p.Prezime = txtPrezime.Text;
        //    p.Predmet = txtPredmet.Text;

        //    lista.Add(p);

        //    dgProfesori.ItemsSource = null;
        //    dgProfesori.ItemsSource = lista;
        //}


        DataTable dt;
        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {

            dt = new DataTable("dt");
            string[] ProfNazivi = { "@Ime", "@Prezime", "@Predmet", "@OUTSektorVecPostoji" };
            DbType[] ProfTipovi = { DbType.String, DbType.String, DbType.String, DbType.String };
            object[] ProfVrednosti = { txtIme.Text, txtPrezime.Text, txtPredmet.Text, 0 };

            object[] OutParam = Metode.IzvrsiKomanduSaOUT("ProfesoriInsert", ProfNazivi, ProfTipovi, ProfVrednosti);
            string poruka = "";
            if (OutParam[0].ToString() != "0") poruka += "Sektor vec postoji.";
            if (poruka != "")
            {
                MessageBox.Show(poruka);
                CiscenjeTxtBoxova();
            }
            else
            {
                Metode.PopuniDataTabeluSelectom("ProfesoriSelect", dt);
                Metode.PunjenjeGridaDataTabelom(dgProfesori, dt);
                CiscenjeTxtBoxova();
                txtIme.Focus();
            }

        }

        private void CiscenjeTxtBoxova()
        {
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtPredmet.Text = "";
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable("dt");
            string[] ProfNazivi = { "@ID", "@Ime", "@Prezime", "@Predmet"};
            DbType[] ProfTipovi = { DbType.Int32, DbType.String, DbType.String, DbType.String};
            object[] ProfVrednosti = { Podaci.IdTekucegReda, txtIme.Text, txtPrezime.Text, txtPredmet.Text};
            //object[] OutParam = Metode.IzvrsiKomanduSaOUT("ProfesoriUpdate", ProfNazivi, ProfTipovi, ProfVrednosti);
            //string poruka = "";


            SqlCommand cmd = Metode.KreiranjeKomande("ProfesoriUpdate", ProfNazivi, ProfTipovi, ProfVrednosti);
            Metode.IzvrsiKomandu(cmd);

            //if (OutParam[0].ToString() != "0") poruka += "Profesor već postoji.";
            //if (poruka != "")
            //{
            //    MessageBox.Show(poruka);
            //    CiscenjeTxtBoxova();
            //}
            //else
            
                Metode.PopuniDataTabeluSelectom("ProfesoriSelect", dt);
                Metode.PunjenjeGridaDataTabelom(dgProfesori, dt);
                CiscenjeTxtBoxova();
                txtIme.Focus();
            

        }
        private int dgIzabraniRed = -1;
        private void dgProfesori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dgProfesori.SelectedItem != null)
            {
                dt = new DataTable("dt");
                dgIzabraniRed = dgProfesori.SelectedIndex;
                DataRowView drv = (DataRowView)dgProfesori.SelectedItem;
                Podaci.IdTekucegReda = (int)drv[0];
                txtIme.Text = drv[1].ToString();
                txtPrezime.Text = drv[2].ToString();
                txtPredmet.Text = drv[3].ToString();

                //txtPrezime.Text= 
            }
        }

        public class Profesor
        {
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Predmet { get; set; }
        }

        private void dgProfesori_AutoGeneratedColumns(object sender, EventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
        DataTable dtImaLiPodataka;
        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            dtImaLiPodataka = new DataTable("dtImaLiPodataka");
            string[] ParNazivi2 = { "@ProfesorID" };
            DbType[] ParTipovi2 = { DbType.Int32 };
            object[] ParVrednosti2 = { Podaci.IdTekucegReda };
            Metode.PuniDtReaderom("ProfesoriPodaciBrisanjeProvera", ParNazivi2, ParTipovi2, ParVrednosti2, dtImaLiPodataka);
            if (dtImaLiPodataka.Rows.Count > 0)
            {
                MessageBox.Show("Ne možete obrisati podatke.");
            }
            else
            {
                dt = new DataTable("dt");
                string[] ParNazivi = { "Id" };
                DbType[] ParTipovi = { DbType.Int32 };
                object[] ParVrednosti = { Podaci.IdTekucegReda };
                SqlCommand cmd = Metode.KreiranjeKomande("ProfesoriDelete", ParNazivi, ParTipovi, ParVrednosti);
                Metode.IzvrsiKomandu(cmd);
            }
            Metode.PopuniDataTabeluSelectom("ProfesoriSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgProfesori, dt);
            CiscenjeTxtBoxova();
            txtIme.Focus();
        }

        private void btnIzlaz_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            this.Close();
            MainWindow.ShowDialog();
        }

        private void btnIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            UcitajIzvestaj();
        }

        private void txtIme_TextChanged()
        {

        }
    }

}

