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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //konekcioni string sa bazom
            Podaci.MojaKonekcija = new SqlConnection("Data Source=DESKTOP-839UVL6;Database=Studenti;Integrated Security = SSPI");
            //txtNazivServera.Text = "DESKTOP-839UVL6";
            

        }

        private void btnProfesori_Click(object sender, RoutedEventArgs e)
        {
            Window Profesori = new Profesori();
            this.Close();
            Profesori.ShowDialog();
        }

        private void btnStudenti_Click(object sender, RoutedEventArgs e)
        {
            Window StudentiW = new StudentiW();
            this.Close();
            StudentiW.ShowDialog();
        }


        private void txtNazivIspita_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnIspit_Click(object sender, RoutedEventArgs e)
        {
            Window Ispit = new Ispit();
            this.Close();
            Ispit.ShowDialog();
        }

        private void btnRok_Click(object sender, RoutedEventArgs e)
        {
            Window Rok = new Rok();
            this.Close();
            Rok.ShowDialog();
        }

        private void btnRezultati_Click(object sender, RoutedEventArgs e)
        {
            Window Rezultati = new Rezultati();
            this.Close();
            Rezultati.ShowDialog();
        }

        private void btnBiblioteka_Click(object sender, RoutedEventArgs e)
        {
            Window Biblioteka = new Biblioteka();
            this.Close();
            Biblioteka.ShowDialog();
        }
    }
    
}
