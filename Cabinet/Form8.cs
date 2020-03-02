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
    public partial class Form8 : Form
    {
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";

        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            String q;
            SqlConnection con = new SqlConnection(init);
            con.Open();
            q = "Select Nume, Prenume from Info_Stomatolog";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader read = cmd.ExecuteReader();
           
            while (read.Read())
                listBox1.Items.Add(read["Nume"] + " " + read["Prenume"]);
            
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.Items.Clear();
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "select * from Info_Client I where I.CNP in(select C.CNP from Consultatie C join Info_Stomatolog S on S.ID_Stomatolog = C.ID_Stomatolog where(S.Nume + ' ' + S.Prenume) = '" + listBox1.SelectedItem.ToString() + "')";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                while (read.Read())
                    listBox2.Items.Add("Name: "+read["Nume"] + " " + read["Prenume"] + "   CNP: " + read["CNP"] + "  Blood type: " + read["Grupa_sanguina"]);
                
                con.Close();
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                listBox2.Items.Clear();
                String q,CNP="0";
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "select Info_Client.Nume,Info_Client.Prenume,Info_Client.CNP,Lucrari.Denumire,Lucrari.Cost from Consultatie join Info_Client on Consultatie.CNP = Info_Client.CNP join Info_Stomatolog on Consultatie.ID_Stomatolog = Info_Stomatolog.ID_Stomatolog join Sumar on Sumar.ID_Consultatie = Consultatie.ID_Consultatie join Lucrari on Lucrari.ID_Lucrare = Sumar.ID_Lucrare where Consultatie.ID_Stomatolog = (select ID_Stomatolog from Info_Stomatolog where(Info_Stomatolog.Nume + ' ' + Info_Stomatolog.Prenume) = '" + listBox1.SelectedItem.ToString() + "') Group by Info_Client.CNP,Info_Client.Nume,Info_Client.Prenume,Lucrari.Denumire,Lucrari.Cost";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                while (read.Read())
                {
                    if (CNP.Equals(read["CNP"].ToString()))
                        listBox2.Items.Add("Service: " + read["Denumire"] + "  Price: " + read["Cost"]);
                    
                    else
                    {
                        CNP = read["CNP"].ToString();
                        listBox2.Items.Add(" ");
                        listBox2.Items.Add("        Name: " + read["Nume"] + " " + read["Prenume"] + "   CNP: " + read["CNP"]);
                        listBox2.Items.Add(" ");
                        listBox2.Items.Add("Service: " + read["Denumire"] + "  Price: " + read["Cost"]);
                    }
                }

                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                listBox2.Items.Clear();
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "select Lucrari.Denumire,count(*) as Numar from Lucrari join Sumar on Sumar.ID_Lucrare = Lucrari.ID_Lucrare join Consultatie on Consultatie.ID_Consultatie = Sumar.ID_Consultatie join Info_Stomatolog on Info_Stomatolog.ID_Stomatolog = Consultatie.ID_Stomatolog where Consultatie.ID_Stomatolog = (select ID_Stomatolog from Info_Stomatolog where(Info_Stomatolog.Nume + ' ' + Info_Stomatolog.Prenume) = '" + listBox1.SelectedItem.ToString() + "') Group by Lucrari.Denumire";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
               
                while (read.Read())
                    listBox2.Items.Add(read["Numar"]+" "+read["Denumire"]);
                
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                listBox2.Items.Clear();
                String q;
                int suma = 0;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "select Lucrari.Denumire,Lucrari.Cost,count(*) as Numar ,SUM(Lucrari.Cost) as Suma from Lucrari join Sumar on Sumar.ID_Lucrare = Lucrari.ID_Lucrare join Consultatie on Consultatie.ID_Consultatie = Sumar.ID_Consultatie join Info_Stomatolog on Info_Stomatolog.ID_Stomatolog = Consultatie.ID_Stomatolog where Consultatie.ID_Stomatolog = (select ID_Stomatolog from Info_Stomatolog where(Info_Stomatolog.Nume + ' ' + Info_Stomatolog.Prenume) = '"+listBox1.SelectedItem.ToString()+"') and Consultatie.Data between '2019-"+DateTime.Now.Month.ToString()+"-01' and '2020-"+DateTime.Now.Month.ToString()+"-31' Group by Lucrari.Denumire,Lucrari.Cost ";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
               
                while (read.Read())
                {
                     listBox2.Items.Add(read["Numar"] + " " + read["Denumire"]+" "+read["Suma"]);
                    suma +=System.Convert.ToInt32( read["Suma"].ToString());
                }

                listBox2.Items.Add(" ");
                listBox2.Items.Add(suma);
                con.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex!=-1)
            {
                listBox2.Items.Clear();
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "select count(*) as Numar from Consultatie join Info_Stomatolog on Info_Stomatolog.ID_Stomatolog = Consultatie.ID_Stomatolog where Consultatie.ID_Stomatolog = (select ID_Stomatolog from Info_Stomatolog where(Info_Stomatolog.Nume + ' ' + Info_Stomatolog.Prenume) = '"+listBox1.SelectedItem.ToString()+"')";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                if (read.Read())
                    listBox2.Items.Add("Number of consults:  "+read["Numar"] );
                
                con.Close();
            }
        }
    }
}
