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
    /// Interaction logic for Ispit.xaml
    /// </summary>
    public partial class Ispit : Window
    {
        public static SqlConnection MojaKonekcija;
        public Ispit()
        {
            InitializeComponent();
      //      MojaKonekcija = new SqlConnection(
      //@"Server=DESKTOP-839UVL6;Database=Studenti;Trusted_Connection=True;");

      //      MojaKonekcija.Open();
        }
        private int dgIzabraniRed = -1;
        private DataTable dt;
        private DataTable dtNaziv;
        private DataTable dtNazivIspita;


        private void UcitajIzvestaj()
        {

           
            
                SqlConnection con = new SqlConnection(
                    @"Server=DESKTOP-839UVL6;Database=Studenti;Trusted_Connection=True;");

                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter("IspitSelect", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);

                reportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource rds = new ReportDataSource("DataSet3", dt);
                reportViewer1.LocalReport.DataSources.Add(rds);

                reportViewer1.LocalReport.ReportPath =
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IspitIzvestaj.rdlc");

                ReportParameter rp = new ReportParameter("Datum", DateTime.Now.ToString("dd.MM.yyyy"));
                reportViewer1.LocalReport.SetParameters(rp);

                reportViewer1.RefreshReport();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (dgIspit.SelectedIndex == -1)
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
            KreirajDtsStudenti();
            KreirajDtpProfesori();
            PopuniKomboSektori();
            PopuniKomboProfesori();
            dt = new DataTable("dt");
            Metode.PopuniDataTabeluSelectom("IspitSelect", dt);

            Metode.PunjenjeGridaDataTabelom(dgIspit, dt);
            CiscenjeTxtBoxova();
            cboProfesori.SelectedValuePath = "ID";
            cboStudenti.SelectedValuePath = "ID";

        }
       
        private void KreirajDtsStudenti()
        {
            dtStudenti = new DataTable("dtStudenti");
            dtStudenti.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            dtStudenti.Columns.Add(new DataColumn("Ime", Type.GetType("System.String")));
            dtStudenti.Columns.Add(new DataColumn("Prezime", Type.GetType("System.String")));
            dtStudenti.Columns.Add(new DataColumn("Prosek", Type.GetType("System.Int32")));
            
        }

        private void KreirajDtpProfesori()
        {
            dtProfesori = new DataTable("dtProfesori");
            dtProfesori.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            dtProfesori.Columns.Add(new DataColumn("Ime", Type.GetType("System.String")));
            dtProfesori.Columns.Add(new DataColumn("Prezime", Type.GetType("System.String")));
            dtProfesori.Columns.Add(new DataColumn("Predmet", Type.GetType("System.String"))); 
            
        }
        private void PopuniKomboSektori()
        {
            dtStudenti = new DataTable("dts");
            Metode.PopuniDataTabeluSelectom("StudentiSelect", dtStudenti);
            Metode.PunjenjeKomboBoksaDataTabelom(cboStudenti, dtStudenti, "Ime");
        }
        DataTable dts;
        DataTable dtStudenti;
        private void PopuniKomboProfesori()
        {
            dtProfesori = new DataTable("dtp");
            Metode.PopuniDataTabeluSelectom("ProfesoriSelect", dtProfesori);
            Metode.PunjenjeKomboBoksaDataTabelom(cboProfesori, dtProfesori, "Ime");

        }
        DataTable dtp;
        DataTable dtProfesori;

        //private void PopuniComboBoxe()
        //{
        //    //STUDENTI
        //   dtStudenti = new DataTable("dtStudenti");
        //    Metode.PopuniDataTabeluSelectom("StudentiSelect", dtStudenti);
        //    Metode.PunjenjeKomboBoksaDataTabelom(cboStudenti, dtStudenti, "Ime");

        //    cboStudenti.SelectedValuePath = "Id";
        //    cboStudenti.DisplayMemberPath = "Ime";


        //    //PROFESORI
        //    DataTable dtProfesori = new DataTable();
        //    Metode.PopuniDataTabeluSelectom("ProfesoriSelect", dtProfesori);
        //    cboProfesori.ItemsSource = dtProfesori.DefaultView;
        //    cboProfesori.SelectedValuePath = "Id";
        //    cboProfesori.DisplayMemberPath = "Ime";


        //}
        private void PopuniListBox()
        {
            dtNaziv = new DataTable("dtNaziv");
            Metode.PopuniDataTabeluSelectom("NazivSelect", dtNaziv);
            Metode.PopunjavanjeListBoxa(lstNaziv, dtNaziv, "Id");
        }
        private void txtNazivPosla_TextChanged(object sender, TextChangedEventArgs e)
        {
            OmogucavanjeUnosa();
        }
        
        private void dgIspit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgIspit.SelectedItem != null)
            {
                //dt = new DataTable("dt");
                DataRowView drv = (DataRowView)dgIspit.SelectedItem;
                Podaci.IdTekucegReda = (int)drv[0];
                txtNazivIspita.Text = drv[1].ToString();
                int StudentID = Convert.ToInt32(drv["StudentID"]);
                for (int i = 0; i < dtStudenti.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtStudenti.Rows[i]["ID"]) == StudentID) 
                    {
                        cboStudenti.SelectedIndex = i;
                        break;
                    }
                }
                btnUnesi.IsEnabled = false;
                btnIzmeni.IsEnabled = true;
                btnObrisi.IsEnabled = true;
                btnOdustani.IsEnabled = true;
            }
            //if (dgIspit.SelectedItem != null)
            //{
            //    dt = new DataTable("dt");
            //    DataRowView drv = (DataRowView)dgIspit.SelectedItem;
            //    Podaci.IdTekucegReda = (int)drv[0];

            //    txtNazivIspita.Text = drv[1].ToString();

            //    cboProfesori.SelectedValue = drv[2];
            //    cboStudenti.SelectedValue = drv[3];

            //    btnUnesi.IsEnabled = false;
            //    btnIzmeni.IsEnabled = true;
            //    btnObrisi.IsEnabled = true;
            //    btnOdustani.IsEnabled = true;
            //}
        }
        private void dgPoslovi_AutoGeneratedColumns(object sender, EventArgs e)
        {
            //dgPoslovi.Columns[0].Visibility = System.Windows.Visibility.Hidden;
            //dgPoslovi.Columns[2].Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (dgIspit.SelectedItem == null)
            {
                MessageBox.Show("Izaberi red!");
                return;
            }
            DataRowView drv = (DataRowView)dgIspit.SelectedItem;
            dgIzabraniRed = dgIspit.SelectedIndex;
            dt = new DataTable("dt");
            string[] ParNazivi = { "@Id", "@ProfesorID", "@StudentID", "@Naziv", "@Ocena" };
            DbType[] ParTipovi = { DbType.Int32, DbType.Int32, DbType.Int32, DbType.String, DbType.Int32 };
            int profesorID = Convert.ToInt32(cboProfesori.SelectedValue);
            int studentID = Convert.ToInt32(cboStudenti.SelectedValue);
            int ocena;
            if (!int.TryParse(txtOcena.Text.Trim(), out ocena))
            {
                MessageBox.Show("Ocena mora biti broj!");
                return;
            }
            int Id = Convert.ToInt32(drv["Id"]);
            object[] ParVrednosti = { Id, profesorID, studentID, txtNazivIspita.Text.Trim(), ocena };
            
            SqlCommand cmd = Metode.KreiranjeKomande("IspitUpdate", ParNazivi, ParTipovi, ParVrednosti);
            Metode.IzvrsiKomandu(cmd);
           
            Metode.PopuniDataTabeluSelectom("IspitSelect", dt);
                    Metode.PunjenjeGridaDataTabelom(dgIspit, dt);
                    CiscenjeTxtBoxova();
                    txtNazivIspita.Focus();


                
          
           

        }
       
        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {
            dt = new DataTable("dt");
            if (cboProfesori.SelectedItem == null || cboStudenti.SelectedItem == null)
            {
                MessageBox.Show("Izaberi profesora i studenta!");
                return;
            }
            int ocena;
            if (!int.TryParse(txtOcena.Text.Trim(), out ocena))
            {
                MessageBox.Show("Ocena mora biti broj!");
                return;
            }
            
            string[] ParNazivi = { "@ProfesorID", "@StudentID", "@Naziv", "@Ocena" };
            DbType[] ParTipovi = { DbType.Int32, DbType.Int32, DbType.String, DbType.Int32 };
            object[] ParVrednosti = { cboProfesori.SelectedValue, cboStudenti.SelectedValue, txtNazivIspita.Text, ocena};
            SqlCommand cmd = Metode.KreiranjeKomande("IspitInsert", ParNazivi, ParTipovi, ParVrednosti);
            Metode.IzvrsiKomandu(cmd);
         
            Metode.PopuniDataTabeluSelectom("IspitSelect", dt);
                Metode.PunjenjeGridaDataTabelom(dgIspit, dt);
                CiscenjeTxtBoxova();
                txtNazivIspita.Focus();


           
            
            
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            dtNazivIspita = new DataTable("dtNazivIspita");
            string[] ParNazivi2 = { "@IdIspita" };
            DbType[] ParTipovi2 = { DbType.Int32 };
            object[] ParVrednosti2 = { Podaci.IdTekucegReda };
            Metode.PuniDtReaderom("IspitDelete", ParNazivi2, ParTipovi2, ParVrednosti2, dtNazivIspita);
            if (dtNazivIspita.Rows.Count > 0)
            {
                MessageBox.Show("Ne možete obrisati ispit, jer je zakazan.");
            }
            else
            {
                dt = new DataTable("dt");
                string[] ParNazivi = { "@Id" };
                DbType[] ParTipovi = { DbType.Int32 };
                object[] ParVrednosti = { Podaci.IdTekucegReda };
                SqlCommand cmd = Metode.KreiranjeKomande("IspitDelete", ParNazivi, ParTipovi, ParVrednosti);
                Metode.IzvrsiKomandu(cmd);
            Metode.PopuniDataTabeluSelectom("IspitSelect", dt);
            Metode.PunjenjeGridaDataTabelom(dgIspit, dt);
                CiscenjeTxtBoxova();
                txtNazivIspita.Focus();

            }
        }

        private void btnOdustani_Click(object sender, RoutedEventArgs e)
        {
            CiscenjeTxtBoxova();
        }
        private void CiscenjeTxtBoxova()
        {
            txtNazivIspita.Focus();
            txtNazivIspita.Text = "";
            dgIspit.SelectedIndex = -1;
            btnIzmeni.IsEnabled = false;
            btnUnesi.IsEnabled = false;
            cboStudenti.SelectedIndex = 0;
           
        }

        private void btnIzlaz_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            this.Close();
            MainWindow.ShowDialog();
        }

        private void dgIspit_AutoGeneratedColumns(object sender, EventArgs e)
        {

        }

        private void lstNaziv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmogucavanjeUnosa();
        }
        private void OmogucavanjeUnosa()
        {
            if (txtNazivIspita.Text != "" && cboStudenti.SelectedIndex > -1 && cboProfesori.SelectedIndex > -1 && txtOcena.Text != "")
            {
                btnUnesi.IsEnabled = true;
            }
            else
            {
                btnUnesi.IsEnabled = false;
            }
        }

        private void txtNazivIspita_TextChanged(object sender, TextChangedEventArgs e)
        {
            OmogucavanjeUnosa();
        }

        private void cboStudenti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmogucavanjeUnosa();
        }

        private void cboProfesori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OmogucavanjeUnosa();
            if (cboProfesori.SelectedValue != null)
            {
                int a = Convert.ToInt32(cboProfesori.SelectedValue);


            }
        }

        private void btnIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            UcitajIzvestaj();
        }
    }
   
}
