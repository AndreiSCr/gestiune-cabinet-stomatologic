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
    public partial class Form5 : Form
    {
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox1.Text = Form2.Consult;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            string[] tokens = textBox1.Text.ToString().Split(' ');
            String q;
            int total = 0;
            SqlConnection con = new SqlConnection(init);
            con.Open();
            q = "select L.Denumire, L.Cost from Lucrari L join Sumar S on L.ID_Lucrare = S.ID_Lucrare where S.ID_Consultatie = (select ID_Consultatie from Consultatie where Data = '"+tokens[0]+ "' and Ora = '" + tokens[1] + "' and CNP = '"+Form2.CNPMaster+"')";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();
            
            while (read.Read())
            {
                listBox1.Items.Add(read["Denumire"] + " " + read["Cost"]);
                total += System.Convert.ToInt32(read["Cost"]);
            }
               
            con.Close();
            textBox2.Text="TOTAL: " + total.ToString();
            q = "select Denumire, Cost from Lucrari";
            con.Open();
            cmd = new SqlCommand(q, con);
            read = cmd.ExecuteReader();
            
            while (read.Read())
                listBox2.Items.Add(read["Denumire"] + " " + read["Cost"]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex!=-1)
            {
                int total = 0;
                string[] tokens = textBox1.Text.ToString().Split(' ');
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q= "Insert into Sumar (ID_Lucrare,ID_Consultatie) values( (select ID_Lucrare from Lucrari where Denumire='"+listBox2.SelectedItem.ToString().Split(' ')[0]+ "'),(select ID_Consultatie from Consultatie where Data = '" + tokens[0] + "' and Ora = '" + tokens[1] + "' and CNP = '" + Form2.CNPMaster + "'))";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
                con.Close();
                listBox1.Items.Clear();
                con.Open();
                q = "select L.Denumire, L.Cost from Lucrari L join Sumar S on L.ID_Lucrare = S.ID_Lucrare where S.ID_Consultatie = (select ID_Consultatie from Consultatie where Data = '" + tokens[0] + "' and Ora = '" + tokens[1] + "' and CNP = '" + Form2.CNPMaster + "')";
                cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                while (read.Read())
                {
                    listBox1.Items.Add(read["Denumire"] + " " + read["Cost"]);
                    total += System.Convert.ToInt32(read["Cost"]);
                }

                con.Close();
                textBox2.Text = "TOTAL: " + total.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int total = 0;
                string[] tokens = textBox1.Text.ToString().Split(' ');
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "Delete Top (1) from Sumar where ID_Lucrare=(select ID_Lucrare from Lucrari where Denumire='" + listBox1.SelectedItem.ToString().Split(' ')[0] + "') and ID_Consultatie=(select ID_Consultatie from Consultatie where Data = '" + tokens[0] + "' and Ora = '" + tokens[1] + "' and CNP = '" + Form2.CNPMaster + "')";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
                con.Close();
                listBox1.Items.Clear();
                con.Open();
                q = "select L.Denumire, L.Cost from Lucrari L join Sumar S on L.ID_Lucrare = S.ID_Lucrare where S.ID_Consultatie = (select ID_Consultatie from Consultatie where Data = '" + tokens[0] + "' and Ora = '" + tokens[1] + "' and CNP = '" + Form2.CNPMaster + "')";
                cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
               
                while (read.Read())
                {
                    listBox1.Items.Add(read["Denumire"] + " " + read["Cost"]);
                    total += System.Convert.ToInt32(read["Cost"]);
                }

                con.Close();
                textBox2.Text = "TOTAL: " + total.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
