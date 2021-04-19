using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DövizHesaplama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DovizKur();
        }


        public double Euro = 0.0;
        public double Dolar = 0.0;
        public double TL = 0.0;
        public double KDV = 0.0;
        public double TLKDV = 0.0;
        public double baz = 0.0;
        public double iskonto = 0.0;
        public DataSet dsDovizKur;


        public bool InternetKontrol()
        {
            try
            {
                System.Net.Sockets.TcpClient kontrol_client = new System.Net.Sockets.TcpClient("www.google.com.tr", 80);
                kontrol_client.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            bool kontrol = InternetKontrol(); // Kontrol fonksiyonumuzu çağırdık
                                              // Eğer internet varsa true yoksa false değeri gelecek. Bunu if ile kontrol edelim
            if (kontrol == true)
            {
                //label1.Text = "İnternet Bağlantın Var !";
                //MessageBox.Show("İnternet Bağlantın Var !");
                //label1.ForeColor = Color.Green;
            }
            else
            {
                //label1.Text = "İnternet'e Bağlanılamıyor.";
                MessageBox.Show("İnternet Bağlantın Yok !");
                label1.ForeColor = Color.Red;
            }

            timer1.Interval = 1000;  // 1sn de bir yeniler
            timer1.Start();
            timer2.Interval = 1000 * 60 * 10;  // 10 dakikada bir yeniler
            timer2.Start();
            this.Left = (Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width);  // formu ekranın sağında başlatır
                                                                                     // this.Right = (Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height);  // formu ekranın sağında başlatır
            radioButton1.Checked = true;
            label13.Text = "$";
        }


        private void timer1_Tick(object sender, EventArgs e)  // Tarih ve saat güncellemesi (1sn)
        {
            tarih.Text = DateTime.Now.ToLongDateString();
            saat.Text = DateTime.Now.ToLongTimeString();
        }

        private void timer2_Tick(object sender, EventArgs e)  // döviz kuru güncellemesi (10dk)
        {
            DovizKur();
        }


        private void DovizKur()  // Döviz kurlarını merkez bankası sisteminde çekiyoruz.
        {
            try
            {
                dsDovizKur = new DataSet();
                dsDovizKur.ReadXml(@"http://www.tcmb.gov.tr/kurlar/today.xml");
                DataRow dr = dsDovizKur.Tables[1].Rows[0];

                Dolar = Convert.ToDouble(dr[4].ToString().Replace('.', ','));
                lb_dolar.Text = dr[4].ToString().Replace('.', ',');
                dr = dsDovizKur.Tables[1].Rows[3];
                Euro = Convert.ToDouble(dr[4].ToString().Replace('.', ','));
                lb_euro.Text = dr[4].ToString().Replace('.', ',');
            }
            catch 
            {
                MessageBox.Show("Döviz kuru bilgisi alınamadı.", "Hata!");

            }

        }


        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DovizKur();
        }

        private void temizleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Text = "0";
            radioButton1.Checked = true;
            label13.Text = "$";
            label1.Text = "***";
            label5.Text = "***";
            label7.Text = "***";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("excel.exe");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("winword.exe");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void islemler()  // Hesaplamaları burada yapıyoruz
        {
            try
            {
                iskonto = Convert.ToDouble(textBox2.Text);

                if (radioButton1.Checked == true)
                {
                    baz = Dolar * Convert.ToDouble(textBox1.Text);
                    TL = baz - (baz * iskonto / 100);
                    label1.Text = baz.ToString();
                }

                else if (radioButton2.Checked == true)
                {
                    baz = Euro * Convert.ToDouble(textBox1.Text);
                    TL = baz - (baz * iskonto / 100);
                    label1.Text = baz.ToString("0.###");
                }
                else if (radioButton3.Checked == true)
                {
                    baz = Convert.ToDouble(textBox1.Text);
                    TL = baz - (baz * iskonto / 100);
                    label1.Text = baz.ToString("0.###");
                }
                else
                {
                    MessageBox.Show("Döviz türü seçmedin.", "Hata!");
                }

                KDV = TL * 18 / 100;
                TLKDV = TL + KDV;
                label14.Text = KDV.ToString("0.###");
                label5.Text = TLKDV.ToString("0.###");
                label7.Text = TL.ToString("0.###");
            }
            catch (Exception)
            {

                // throw;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            islemler();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            islemler();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label13.Text = "$";
            islemler();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label13.Text = "€";
            islemler();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.zcaner.com");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label13.Text = "₺";
            islemler();
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fr = new Form2();
            fr.Show();
        }
    }
}
