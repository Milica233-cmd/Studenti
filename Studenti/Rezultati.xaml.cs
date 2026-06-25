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
    /// Interaction logic for Rezultati.xaml
    /// </summary>
    public partial class Rezultati : Window
    {
        public Rezultati()
        {
            InitializeComponent();
        }
        private DataTable dtIspit;
        private DataTable dt;
        private DataTable dtStudent;

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopuniKomboPredmet();
            //PopuniLstStudent();
            //PopuniListIspit();
            //OmoguciDodavanje();
            //OmoguciUklanjanje();
        }
        private void lstStudenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmoguciUklanjanje();
        }

        private void cboPredmet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmogucavanjeUnosa();
            PopuniLstStudent();
            PopuniListIspit();
        }
        private void OmogucavanjeUnosa()
        {
            if (cboPredmet.SelectedIndex > -1)
            {
                btnDodaj.IsEnabled = true;
                btnUkloni.IsEnabled = true;
            }

        }
        private void PopuniKomboPredmet()
        {
            dtIspit = new DataTable("dtIspit");
            Metode.PopuniDataTabeluSelectom("RezultatiNazivSelect", dtIspit);
            Metode.PunjenjeKomboBoksaDataTabelom(cboPredmet, dtIspit, "Naziv");
        }
        //private void PopuniStudente()
        //{
        //    DataTable dtRadnika = new DataTable("dtStudent");
        //    string[] ParNazivi = { "@IdInt" };
        //    DbType[] ParTipovi = { DbType.Int32 };
        //    object[] ParVrednosti = { cboPredmet.SelectedValue };
        //    Metode.PuniDtReaderom("RezultatiSelect", ParNazivi, ParTipovi, ParVrednosti, dtRadnika);
        //    Metode.PopunjavanjeListBoxa(lstPolozili, dtRadnika, "Ime i prezime radnika");
        //    Razlika();
        //}
        private void PopuniLstStudent()
        {
            DataTable dtStudent = new DataTable("dtStudent");
            string[] ParNazivi = { "@IdInt" };
            DbType[] ParTipovi = { DbType.Int32 };
            object[] ParVrednosti = { cboPredmet.SelectedValue };
            Metode.PuniDtReaderom("RezultatiImeiPrezimeSelect", ParNazivi, ParTipovi, ParVrednosti, dtStudent);
            Metode.PopunjavanjeListBoxa(lstStudenta, dtStudent, "Ime i prezime studenta");
        }
        private void PopuniListIspit()
        {
            dtIspit = new DataTable("dtIspit");
            string[] ParNazivi = { "@IdInt" };
            DbType[] ParTipovi = { DbType.Int32 };
            object[] ParVrednosti = { cboPredmet.SelectedValue };
            Metode.PuniDtReaderom("RezultatiSelect", ParNazivi, ParTipovi, ParVrednosti, dtIspit);
            lstPolozili.ItemsSource = dtIspit.DefaultView;
            lstPolozili.DisplayMemberPath = "ImePrezime";
            lstPolozili.SelectedValuePath = "ID";
        }
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (cboPredmet.SelectedValue == null || lstStudenta.SelectedValue == null)
                if (cboPredmet.SelectedItem == null || lstStudenta.SelectedItem == null)
                {
                    MessageBox.Show("Izaberi predmet i studenta!");
                    return;
                }

            DataRowView predmet = cboPredmet.SelectedItem as DataRowView;
            DataRowView student = lstStudenta.SelectedItem as DataRowView;

            if (predmet == null || student == null)
            {
                MessageBox.Show("Greška u selekciji!");
                return;
            }

            string[] ParNazivi = { "@IspitID", "@StudentID" };
            DbType[] ParTipovi = { DbType.Int32, DbType.Int32 };
            object[] ParVrednosti =
            {
              predmet["ID"],
              student["ID"]
            };
    

            SqlCommand cmd = Metode.KreiranjeKomande("RezultatiInsert", ParNazivi, ParTipovi, ParVrednosti);
            Metode.IzvrsiKomandu(cmd);

            PopuniListIspit();
            PopuniLstStudent();
        }
    

        private void btnUkloni_Click(object sender, RoutedEventArgs e)
        {
            if (lstPolozili.SelectedIndex > -1)
            {
                int ID = Convert.ToInt32(lstPolozili.SelectedValue);
                
                string[] ParNazivi = { "@ID" };
                DbType[] ParTipovi = { DbType.Int32 };
                object[] ParVrednosti = { ID };
                SqlCommand cmd = Metode.KreiranjeKomande("RezultatiDelete", ParNazivi, ParTipovi, ParVrednosti);
                Metode.IzvrsiKomandu(cmd);
                PopuniListIspit();
                PopuniLstStudent();
            }
            else
            {
                MessageBox.Show("Selektujte studenta kojeg želite da uklonite");
            }

        }
        //private void Razlika()
        //{
        //    dt = new DataTable("dt");
        //    string[] ParNazivi = { "@IdInt" };
        //    DbType[] ParTipovi = { DbType.Int32 };
        //    object[] ParVrednosti = { cboPredmet.SelectedValue };
        //    Metode.PuniDtReaderom("RazlikaStudenti", ParNazivi, ParTipovi, ParVrednosti, dt);
        //    Metode.PopunjavanjeListBoxa(lstStudenta, dt, "Ime i prezime studenta");
        //}



        private void lstPolozili_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmoguciUklanjanje();
        }

        private void btnIzlaz_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            this.Close();
            MainWindow.ShowDialog();
        }
        private void OmoguciDodavanje()
        {
            if (lstStudenta.SelectedIndex > -1)
            {
                btnDodaj.IsEnabled = true;

            }
            else
            {
                btnDodaj.IsEnabled = false;

            }
        }
        private void OmoguciUklanjanje()
        {
            if (lstPolozili.SelectedIndex > -1)
            {
                btnUkloni.IsEnabled = true;

            }
            else
            {
                btnUkloni.IsEnabled = false;

            }
        }

    }
}

    
