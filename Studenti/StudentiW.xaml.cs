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
    /// <summary>
    /// Interaction logic for Studenti.xaml
    /// </summary>
    public partial class StudentiW : Window
    {
        List<Student> lista = new List<Student>();

        public StudentiW()
        {
            InitializeComponent();
        }

        private void UcitajIzvestaj()
        {

            SqlConnection con = new SqlConnection(
         @"Server=DESKTOP-839UVL6;Database=Studenti;Trusted_Connection=True;");

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT Ime, Prezime, Prosek FROM Studenti", con);

            da.Fill(dt);

            MessageBox.Show(dt.Rows.Count.ToString()); // proveri da radi

            reportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource rds = new ReportDataSource("DataSet2", dt);
            reportViewer1.LocalReport.DataSources.Add(rds);

            reportViewer1.LocalReport.ReportPath =
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StudentiIzvestaj.rdlc");

            ReportParameter rp = new ReportParameter("Datum", DateTime.Now.ToString("dd.MM.yyyy"));
            reportViewer1.LocalReport.SetParameters(rp);

            reportViewer1.RefreshReport();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (dgStudenti.SelectedIndex == -1)
            //{
            //    btnIzmeni.IsEnabled = false;
            //    btnUnesi.IsEnabled = false;
            //}
            //else
            //{
            //    btnIzmeni.IsEnabled = true;

            
            dt = new DataTable("dt");
            Metode.PopuniDataTabeluSelectom("StudentiSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgStudenti, dt);
            CiscenjeTxtBoxova();
        }


        //private void Dodaj_Click(object sender, RoutedEventArgs e)
        //{
        //    Student s = new Student();

        //    s.Ime = txtIme.Text;
        //    s.Prezime = txtPrezime.Text;
        //    s.Prosek = int.Parse(txtProsek.Text);

        //    lista.Add(s);

        //    dgStudenti.ItemsSource = null;
        //    dgStudenti.ItemsSource = lista;
        //}





        DataTable dt;
        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {

            decimal prosek;
            if (!decimal.TryParse(txtProsek.Text,out prosek))
            {
                MessageBox.Show("Prosek nije ispravan");
                return;
            }

            dt = new DataTable("dt");
            string[] StudNazivi = { "@Ime", "@Prezime", "@Prosek" };
            DbType[] StudTipovi = { DbType.String, DbType.String, DbType.Int32};
            object[] StudVrednosti = { txtIme.Text, txtPrezime.Text, Convert.ToDecimal(txtProsek.Text)};

            //object[] OutParam = Metode.IzvrsiKomandu("StudentiWInsert", StudNazivi, StudTipovi, StudVrednosti);
            SqlCommand cmd = Metode.KreiranjeKomande("StudentiInsert", StudNazivi, StudTipovi, StudVrednosti);
            Metode.IzvrsiKomandu(cmd);

            //string poruka = "";
            //if (OutParam[0].ToString() != "0") poruka += "Student vec postoji.";
            //if (poruka != "")
            //{
            //    MessageBox.Show(poruka);
            //    CiscenjeTxtBoxova();
            //}
            //else
            //{
            Metode.PopuniDataTabeluSelectom("StudentiSelect", dt);
                Metode.PunjenjeGridaDataTabelom(dgStudenti, dt);
                CiscenjeTxtBoxova();
                txtIme.Focus();
            //}

        }
        private void CiscenjeTxtBoxova()
        {
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtProsek.Text = "";
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable("dt");
            string[] StudNazivi = { "@ID", "@Ime", "@Prezime", "@Prosek" };
            DbType[] StudTipovi = { DbType.Int32, DbType.String, DbType.String, DbType.Int32 };
            object[] StudVrednosti = { Podaci.IdTekucegReda, txtIme.Text, txtPrezime.Text, int.Parse(txtProsek.Text) };
            //object[] OutParam = Metode.IzvrsiKomanduSaOUT("ProfesoriUpdate", ProfNazivi, ProfTipovi, ProfVrednosti);
            //string poruka = "";


            SqlCommand cmd = Metode.KreiranjeKomande("StudentiUpdate", StudNazivi, StudTipovi, StudVrednosti);
            Metode.IzvrsiKomandu(cmd);

            //if (OutParam[0].ToString() != "0") poruka += "Profesor već postoji.";
            //if (poruka != "")
            //{
            //    MessageBox.Show(poruka);
            //    CiscenjeTxtBoxova();
            //}
            //else

            Metode.PopuniDataTabeluSelectom("StudentiSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgStudenti, dt);
            CiscenjeTxtBoxova();
            txtIme.Focus();


        }
        private int dgIzabraniRed = -1;
        private void dgStudenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dgStudenti.SelectedItem != null)
            {
                dt = new DataTable("dt");
                dgIzabraniRed = dgStudenti.SelectedIndex;
                DataRowView drv = (DataRowView)dgStudenti.SelectedItem;
                Podaci.IdTekucegReda = (int)drv[0];
                txtIme.Text = drv[1].ToString();
                txtPrezime.Text = drv[2].ToString();
                txtProsek.Text = drv[3].ToString();

                //txtPrezime.Text= 
            }
        }

        
        private void btnIzmeni_Click_2(object sender, RoutedEventArgs e)
        {
            dt = new DataTable("dt");
            string[] StudNazivi = { "@ID", "@Ime", "@Prezime", "@Prosek" };
            DbType[] StudTipovi = { DbType.Int32, DbType.String, DbType.String, DbType.Int32 };
            object[] StudVrednosti = { Podaci.IdTekucegReda, txtIme.Text, txtPrezime.Text, int.Parse(txtProsek.Text) };
            //object[] OutParam = Metode.IzvrsiKomanduSaOUT("ProfesoriUpdate", ProfNazivi, ProfTipovi, ProfVrednosti);
            //string poruka = "";


            SqlCommand cmd = Metode.KreiranjeKomande("StudentiUpdate", StudNazivi, StudTipovi, StudVrednosti);
            Metode.IzvrsiKomandu(cmd);

            //if (OutParam[0].ToString() != "0") poruka += "Profesor već postoji.";
            //if (poruka != "")
            //{
            //    MessageBox.Show(poruka);
            //    CiscenjeTxtBoxova();
            //}
            //else

            Metode.PopuniDataTabeluSelectom("StudentiSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgStudenti, dt);
            CiscenjeTxtBoxova();
            txtIme.Focus();

        }
        DataTable dtImaLiPodataka;
        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            dtImaLiPodataka = new DataTable("dtImaLiPodataka");
            string[] ParNazivi2 = { "@StudentID" };
            DbType[] ParTipovi2 = { DbType.Int32 };
            object[] ParVrednosti2 = { Podaci.IdTekucegReda };
            Metode.PuniDtReaderom("StudentiPodaciBrisanjeProvera", ParNazivi2, ParTipovi2, ParVrednosti2, dtImaLiPodataka);
            if (dtImaLiPodataka.Rows.Count > 0)
            {
                MessageBox.Show("Ne možete obrisati podatke.");
            }
            else
            {
                dt = new DataTable("dt");
                string[] ParNazivi = { "@Id" };
                DbType[] ParTipovi = { DbType.Int32 };
                object[] ParVrednosti = { Podaci.IdTekucegReda };
                SqlCommand cmd = Metode.KreiranjeKomande("StudentiDelete", ParNazivi, ParTipovi, ParVrednosti);
                Metode.IzvrsiKomandu(cmd);
            }
            Metode.PopuniDataTabeluSelectom("StudentiSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgStudenti, dt);
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

       
    }

    public class Student
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int Prosek { get; set; }
    }
}
