using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Veri_Tabani_Ile_Mailing
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-DSQNOEI\SQLEXPRESS03;Initial Catalog=DbMailing;Integrated Security=True");


        public string Numara;

        void GelenKutusu()
        {
            SqlDataAdapter dataAdapter= new SqlDataAdapter("Select MesajId, (Ad + ' ' + Soyad) AS Gonderen, Baslik, Icerik From TblMesajlar inner join TblKisiler on TblMesajlar.Gonderen = TblKisiler.Numara Where Alici = " + Numara, baglanti);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            DgvGelenKutusu.DataSource = dt;
        }

        void GidenKutusu()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("Select MesajId, (Ad+ ' ' +Soyad) AS Alici,Baslik,Icerik From TblMesajlar inner join TblKisiler on TblMesajlar.Alici = TblKisiler.Numara Where Gonderen = " + Numara, baglanti);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            DgvGidenKutusu.DataSource = dt;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            LblNumara.Text = Numara;
            GelenKutusu();
            GidenKutusu();

            //Ad Soyad Getime

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select Ad, Soyad From TblKisiler Where Numara=" + Numara, baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            baglanti.Close();
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TblMesajlar (Gonderen, Alici, Baslik, Icerik) values (@P1,@P2,@P3,@P4)", baglanti);
            komut.Parameters.AddWithValue("@P1", Numara);
            komut.Parameters.AddWithValue("@P2", MskAlici.Text);
            komut.Parameters.AddWithValue("@P3", TxtBaslik.Text);
            komut.Parameters.AddWithValue("@P4", RTxtIcerik.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Mailiniz İletildi!");
            GidenKutusu();
            MskAlici.Clear();
            TxtBaslik.Clear();
            RTxtIcerik.Clear();
        }
    }
}
