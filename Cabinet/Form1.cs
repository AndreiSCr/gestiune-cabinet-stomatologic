using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Cabinet
{
    public partial class Form1 : Form
    {
        //Declarare legatura DB
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";

        public Form1()
        {
            InitializeComponent();
        }       

        private void Form1_Load(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            Form2 f2 = new Form2();
            Form4 f4 = new Form4();
            //f2.Show();
            //f3.Show();
            //f4.Show();
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            String User, Pass;

            //Citire USER+PASSWORD
            User = Convert.ToString(textBox1.Text);
            Pass = Convert.ToString(textBox2.Text);

            //Deschidere conexiune DB
            SqlConnection con = new SqlConnection(init);
            con.Open();

            //Selectare User+Parola din DB
            string q = "select Pwd from LogIn where Us ='" + User + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();

            //Verificare credentiale
            if (read.Read())
            {
                if (Pass.Equals(read["Pwd"].ToString()))
                {
                    this.Hide();
                    f2.ShowDialog();
                    textBox1.Text = null;
                    textBox2.Text = null;
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Wrong password");
                    textBox2.Text = null;
                }
            }

            else
            {
                MessageBox.Show("Wrong username");
                textBox1.Text = null;
                textBox2.Text = null;
            }
            con.Close();
        }

        //Actiune enter
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                button1_Click_1(this, new EventArgs());
            }
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            Form3 f3 = new Form3();
            Form2 f2 = new Form2();

            if (MouseButtons== MouseButtons.Right)
            {
                String User, Pass;

                //Citire USER+PASSWORD
                User = Convert.ToString(textBox1.Text);
                Pass = Convert.ToString(textBox2.Text);

                //Deschidere conexiune DB
                SqlConnection con = new SqlConnection(init);
                con.Open();

                //Selectare User+Parola din DB
                string q = "select Pwd, Super from LogIn where Us ='" + User + "'";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();

                //Verificare credentiale
                if (read.Read())
                {
                    if (Pass.Equals(read["Pwd"].ToString()))
                    {
                        if ("1".Equals(read["Super"].ToString()))
                        {
                            this.Hide();
                            f3.ShowDialog();
                            textBox1.Text = null;
                            textBox2.Text = null;
                            this.Show();
                        }
                    }
                }
                con.Close();
            }
        }
    }
}
