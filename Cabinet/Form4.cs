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
    public partial class Form4 : Form
    {
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";
        int exit = 0;

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            listBox1.Items.Clear();
            String q, FN, LN;
            SqlConnection con = new SqlConnection(init);
            con.Open();
            q = "Select Nume,Prenume from Info_Stomatolog";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
                listBox1.Items.Add(read["Nume"] + " " + read["Prenume"]);

            con.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exit == 0)
            {
                var result = MessageBox.Show("All the unsaved progress will be lost", "Warning",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question);
               
                if (result == DialogResult.Yes)
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
            
            else
                exit = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            List<String> Prog=new List<string>();
            List<String> Used = new List<string>();
            Prog.Add("08:00");
            Prog.Add("08:30");
            Prog.Add("09:00");
            Prog.Add("09:30");
            Prog.Add("10:00");
            Prog.Add("10:30");
            Prog.Add("11:00");
            Prog.Add("11:30");
            Prog.Add("12:00"); 
            Prog.Add("12:30");
            Prog.Add("13:00");
            Prog.Add("13:30");
            Prog.Add("14:00");
            Prog.Add("14:30");
            Prog.Add("15:00");
            Prog.Add("15:30");
            listBox2.DataSource = null;
            listBox2.Items.Clear();
            String q;
            q = "select Ora from Consultatie C join Info_Stomatolog S on S.ID_Stomatolog=C.ID_Stomatolog where C.ID_Stomatolog=(select ID_Stomatolog from Info_Stomatolog I where Nume+' '+Prenume='" + listBox1.SelectedItem.ToString() + "') and Data='" + dateTimePicker1.Value.Date.Year.ToString()+"-"+ dateTimePicker1.Value.Date.Month.ToString()+"-"+ dateTimePicker1.Value.Date.Day.ToString()+"'";
            SqlConnection con = new SqlConnection(init);
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();
           
            while (read.Read())
                Used.Add(read["Ora"].ToString().Substring(0,5));
            
            con.Close();
            var result = Prog.Except(Used);
            listBox2.DataSource=(result.ToList());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String q;
            q = "insert into Consultatie(Data,Ora,CNP,ID_Stomatolog) values ('"+ dateTimePicker1.Value.Date.Year.ToString() + "-" + dateTimePicker1.Value.Date.Month.ToString() + "-" + dateTimePicker1.Value.Date.Day.ToString() + "','"+listBox2.SelectedItem.ToString()+"','"+Form2.CNPMaster+"',"+" (select ID_Stomatolog from Info_Stomatolog I where Nume+' ' + Prenume = '" + listBox1.SelectedItem.ToString() + "'))";
            SqlConnection con = new SqlConnection(init);
            con.Open();
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Done");
            exit = 1;
            this.Close();
        }
    }
}
