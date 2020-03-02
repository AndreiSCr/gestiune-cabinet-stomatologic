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
    public partial class Form6 : Form
    {
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("-----Dentexpert-----");
            listBox1.Items.Add(" ");
            listBox1.Items.Add("Appointment date/time: " + Form2.Consult.Split(' ')[0] +" "+ Form2.Consult.Split(' ')[1]);
            listBox1.Items.Add("Stomatologist: " + Form2.Consult.Substring(Form2.Consult.Split(' ')[0].Length+ Form2.Consult.Split(' ')[1].Length+1));
            listBox1.Items.Add(" ");
            SqlConnection con = new SqlConnection(init);
            String q;
            int total = 0;
            con.Open();
            string[] tokens = Form2.Consult.Split(' ');
            q = "select L.Denumire, L.Cost from Lucrari L join Sumar S on L.ID_Lucrare = S.ID_Lucrare where S.ID_Consultatie = (select ID_Consultatie from Consultatie where Data = '" + tokens[0] + "' and Ora = '" + tokens[1] + "' and CNP = '" + Form2.CNPMaster + "')";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();
            
            while (read.Read())
            {
                listBox1.Items.Add(read["Denumire"]);
                listBox1.Items.Add(read["Cost"]);
                total += System.Convert.ToInt32(read["Cost"]);
            }

            con.Close();
            listBox1.Items.Add(" ");
            listBox1.Items.Add("TOTAL: " + total.ToString());
            listBox1.Items.Add(" ");
            listBox1.Items.Add(" ");
            listBox1.Items.Add("Billing date:" + DateTime.Now.ToString());
        }
    }
}
