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
using System.Data;
using System.Data.SqlClient;

namespace Studenti
{
    /// <summary>
    /// Interaction logic for Biblioteka.xaml
    /// </summary>
    public partial class Biblioteka : Window
    {
        SqlConnection con = new SqlConnection(@"Server=DESKTOP-839UVL6;Database=Studenti;Trusted_Connection=True;");
        DataTable dt = new DataTable();
        public Biblioteka()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            SqlDataAdapter da = new SqlDataAdapter("BibliotekaSelect", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            dt.Clear();
            da.Fill(dt);

            dgBiblioteka.ItemsSource = dt.DefaultView;
        }

        private void dgBiblioteka_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            DataRowView row = (DataRowView)e.Row.Item;

            int id = Convert.ToInt32(row["ID"]);
            bool razduzeno = (bool)((CheckBox)e.EditingElement).IsChecked;

            SqlCommand cmd = new SqlCommand("BibliotekaUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@Razduzeno", razduzeno);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            LoadData();
        }

        private void btnIzlaz_Click(object sender, RoutedEventArgs e)
        {
            Window MainWindow = new MainWindow();
            this.Close();
            MainWindow.ShowDialog();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            SqlCommand cmd1 = new SqlCommand(@"INSERT INTO Studenti(Ime, Prezime, Prosek)
            VALUES(@Ime, @Prezime, @Prosek); SELECT SCOPE_IDENTITY(); ", con);

            cmd1.Parameters.AddWithValue("@Ime", txtIme.Text);
            cmd1.Parameters.AddWithValue("@Prezime", txtPrezime.Text);
            cmd1.Parameters.AddWithValue("@Prosek", 0);

            object result = cmd1.ExecuteScalar();
            int studentId = Convert.ToInt32(result);

            SqlCommand cmd2 = new SqlCommand("BibliotekaInsert", con);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@StudentiID", studentId);
            cmd2.Parameters.AddWithValue("@Razduzeno", false);

            cmd2.ExecuteNonQuery();

            con.Close();

            //MessageBox.Show("Dodato!");
            LoadData();
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (dgBiblioteka.SelectedItem == null)
            {
                MessageBox.Show("Izaberi red!");
                return;
            }

            DataRowView row = (DataRowView)dgBiblioteka.SelectedItem;
            int id = Convert.ToInt32(row["ID"]);

            SqlCommand cmd = new SqlCommand("BibliotekaDelete", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            //MessageBox.Show("Obrisano!");

            LoadData();
        }
    }
}
