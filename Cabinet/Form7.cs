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
    public partial class Form7 : Form
    {
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            String q;
            SqlConnection con = new SqlConnection(init);
            con.Open();
            q = "Select Denumire, Cost from Lucrari";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();
           
            while (read.Read())
                listBox1.Items.Add(read["Denumire"] + " " + read["Cost"]);         
            
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "Delete from Lucrari where Denumire='"+listBox1.SelectedItem.ToString().Split(' ')[0]+"'";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
                con.Close();
                listBox1.Items.Clear();
                con.Open();
                q = "Select Denumire, Cost from Lucrari";
                cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                while (read.Read())
                    listBox1.Items.Add(read["Denumire"] + " " + read["Cost"]);
                
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!=null && textBox2.Text!=null)
            {
                SqlConnection con = new SqlConnection(init);
                con.Open();
                String q;
                q = "Insert into Lucrari (Denumire, Cost) values ('"+textBox1.Text.ToString().ToUpper()+"','"+textBox2.Text.ToString()+"')";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
                con.Close();
                listBox1.Items.Clear();
                con.Open();
                q = "Select Denumire, Cost from Lucrari";
                cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                while (read.Read())
                    listBox1.Items.Add(read["Denumire"] + " " + read["Cost"]);
                
                con.Close();
                textBox1.Text = null;
                textBox2.Text = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
