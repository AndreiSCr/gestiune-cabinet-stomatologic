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
    public partial class Form2 : Form
    {
        public static String CNPMaster,Consult;

        //Declarare legatura DB
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";

        int val = 0, err = 0;

        public Form2()
        {
            InitializeComponent();      
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Delete");
            comboBox1.Items.Add("Modify");
            comboBox1.Items.Add("New Appointment");
            comboBox1.Items.Add("Delete Future Appointment");
            comboBox1.Items.Add("Complete Appointment");
            comboBox1.Items.Add("Bill Appointment");
            label1.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            radioButton1.Hide();
            radioButton2.Hide();
            radioButton3.Hide();
            radioButton4.Hide();
            button1.Hide();
            comboBox1.Hide();
            listBox1.Hide();
        }

        //Atentionare inchidere
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("All the unsaved progress will be lost", "Warning",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void addClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Hide();
            comboBox1.Hide();
            label1.Text = "First name";
            label2.Text = "Last name";
            label3.Text = "CNP";
            label4.Text = "Blood type";
            button1.Text = "Add";
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton4.Enabled = true;
            label1.Show();
            label2.Show();
            label3.Show();
            label4.Show();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            radioButton1.Show();
            radioButton2.Show();
            radioButton3.Show();
            radioButton4.Show();
            button1.Show();
            val = 1;
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Hide();
            listBox1.Hide();
            textBox1.ReadOnly = false;
            textBox1.Enabled = true;
            label1.Text = "CNP";
            button1.Text = "Search";
            textBox1.Text = null;
            comboBox1.Text = null;
            label1.Show();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            textBox1.Show();
            textBox2.Hide();
            textBox3.Hide();
            radioButton1.Hide();
            radioButton2.Hide();
            radioButton3.Hide();
            radioButton4.Hide();
            button1.Show();
            val = 2;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text.ToString().Equals("Modify"))
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
            }

            else
            {
                textBox2.ReadOnly = true;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
            }

            if (comboBox1.Text.ToString().Equals("Delete Future Appointment"))
            {
                listBox1.Items.Clear();
                listBox1.Show();
                String q;
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "select C.Data, C.Ora, I.Nume, I.Prenume from Consultatie C join Info_Stomatolog I on C.ID_Stomatolog=I.ID_Stomatolog and C.CNP='" + CNPMaster + "' and (C.Data>GETDATE() or (C.Data=GETDATE() and C.Ora>(SELECT CONVERT(TIME,GETDATE())))) order by C.Data";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();

                while(read.Read())
                {
                    listBox1.Items.Add(read["Data"].ToString().Substring(0,read["Data"].ToString().IndexOf(' ')) + " " + read["Ora"].ToString().Substring(0,5) + " " + read["Nume"].ToString() + " " + read["Prenume"].ToString());
                }

                con.Close();
                val = 3;
            }

            else
                if (comboBox1.Text.ToString().Equals("Complete Appointment") || comboBox1.Text.ToString().Equals("Bill Appointment"))
                {
                    listBox1.Items.Clear();
                    listBox1.Show();
                    String q;
                    SqlConnection con = new SqlConnection(init);
                    con.Open();
                    q = "select C.Data, C.Ora, I.Nume, I.Prenume from Consultatie C join Info_Stomatolog I on C.ID_Stomatolog=I.ID_Stomatolog and C.CNP='" + CNPMaster + "'order by C.Data";
                    SqlCommand cmd = new SqlCommand(q, con);
                    SqlDataReader read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        listBox1.Items.Add(read["Data"].ToString().Substring(0, read["Data"].ToString().IndexOf(' ')) + " " + read["Ora"].ToString().Substring(0, 5) + " " + read["Nume"].ToString() + " " + read["Prenume"].ToString());
                    }

                    con.Close();
                    val = 3;
                }

                else
                    listBox1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(val==1)
            {
                //Citire date client
                String q, FN, LN, Gr="01";
                int CNP;
                FN = Convert.ToString(textBox1.Text);
                LN = Convert.ToString(textBox2.Text);
                CNP = Convert.ToInt32(textBox3.Text);

                if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false)
                    MessageBox.Show("Check blood type");
                else
                    if (radioButton1.Checked == true)
                    Gr = Convert.ToString(radioButton1.Text);
                    else
                        if (radioButton2.Checked == true)
                            Gr = Convert.ToString(radioButton2.Text);
                        else
                            if (radioButton3.Checked == true)
                                Gr = Convert.ToString(radioButton3.Text);
                            else
                                Gr = Convert.ToString(radioButton4.Text);

                //Verificare existenta client
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q="Select CNP from Info_Client where CNP ="+CNP;
                
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();

                if (read.Read())
                {
                    con.Close();
                    MessageBox.Show("Client already in DataBase");
                }

                else
                {
                    con.Close();
                    con.Open();
                    q = "insert into Info_Client(CNP, Nume, Prenume, Grupa_sanguina) values('" + CNP + "', '" + FN.ToUpper() + "', '" + LN.ToUpper() + "', '" + Gr + "')";
                    cmd = new SqlCommand(q, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Done!");
                }

                label1.Hide();
                label2.Hide();
                label3.Hide();
                label4.Hide();
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox1.Hide();
                textBox2.Hide();
                textBox3.Hide();
                radioButton1.Hide();
                radioButton2.Hide();
                radioButton3.Hide();
                radioButton4.Hide();
                button1.Hide();
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            }

            if(val==2)
            {
                String q,CNP;
                CNP = Convert.ToString(textBox1.Text);
                CNPMaster= Convert.ToString(textBox1.Text);
                SqlConnection con = new SqlConnection(init);
                con.Open();
                q = "Select * from Info_Client where CNP =" + CNP;

                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();

                if (read.Read())
                {
                    label1.Text = "First name";
                    label2.Text = "Last name";
                    label3.Text = "CNP";
                    label4.Text = "Blood type";
                    button1.Text = "Execute";
                    textBox1.Text = read["Nume"].ToString();
                    textBox2.Text = read["Prenume"].ToString();
                    textBox3.Text = read["CNP"].ToString();
                    textBox3.ReadOnly = true;
                    label2.Show();
                    label3.Show();
                    textBox2.Show();
                    textBox3.Show();
                    radioButton1.Show();
                    radioButton2.Show();
                    radioButton3.Show();
                    radioButton4.Show();
                    comboBox1.Show();
                    if (read["Grupa_sanguina"].ToString().Equals("01"))
                        radioButton1.Checked = true;
                    else
                        if (read["Grupa_sanguina"].ToString().Equals("A2"))
                        radioButton2.Checked = true;
                        else
                            if (read["Grupa_sanguina"].ToString().Equals("B3"))
                                radioButton3.Checked = true;
                            else
                                radioButton4.Checked = true;
                    con.Close();
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    val = 3;
                }

                else
                {
                    MessageBox.Show("Client not found");
                    con.Close();
                    textBox1.Text = null;
                }
            }

            if(val==3)
            {
                if(comboBox1.Text.ToString().Equals("Delete"))
                {
                    String q, CNP;
                    CNP = Convert.ToString(textBox3.Text);
                    SqlConnection con = new SqlConnection(init);
                    con.Open();
                    q = "Delete from Info_Client where CNP =" + CNP;

                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Done!");
                    label1.Hide();
                    label2.Hide();
                    label3.Hide();
                    label4.Hide();
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox1.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    radioButton1.Hide();
                    radioButton2.Hide();
                    radioButton3.Hide();
                    radioButton4.Hide();
                    comboBox1.Hide();
                    button1.Hide();
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                }

                if(comboBox1.Text.ToString().Equals("Modify"))
                {
                    String q, FN, LN, Gr = "01",CNP;
                    SqlConnection con = new SqlConnection(init);
                    con.Open();
                    FN = Convert.ToString(textBox1.Text);
                    LN = Convert.ToString(textBox2.Text);
                    CNP = Convert.ToString(textBox3.Text);

                    if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false)
                        MessageBox.Show("Check blood type");
                    else
                        if (radioButton1.Checked == true)
                            Gr = Convert.ToString(radioButton1.Text);
                        else
                            if (radioButton2.Checked == true)
                                Gr = Convert.ToString(radioButton2.Text);
                            else
                                if (radioButton3.Checked == true)
                                    Gr = Convert.ToString(radioButton3.Text);
                                else
                                    Gr = Convert.ToString(radioButton4.Text);

                    q = "update Info_Client set Nume='" + FN.ToUpper() + "',Prenume='" + LN.ToUpper() + "',Grupa_sanguina='" + Gr + "' where CNP='" + CNP + "'";
                    SqlCommand cmd = new SqlCommand(q, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Done!");
                    label1.Hide();
                    label2.Hide();
                    label3.Hide();
                    label4.Hide();
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox1.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    radioButton1.Hide();
                    radioButton2.Hide();
                    radioButton3.Hide();
                    radioButton4.Hide();
                    comboBox1.Hide();
                    button1.Hide();
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                }

                if(comboBox1.Text.ToString().Equals("New Appointment"))
                {
                    Form4 f4 = new Form4();
                    this.Hide();
                    f4.ShowDialog();
                    this.Show();
                    label1.Hide();
                    label2.Hide();
                    label3.Hide();
                    label4.Hide();
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox1.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    radioButton1.Hide();
                    radioButton2.Hide();
                    radioButton3.Hide();
                    radioButton4.Hide();
                    button1.Hide();
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    comboBox1.Hide();
                }

                if (comboBox1.Text.ToString().Equals("Delete Future Appointment"))
                {
                    if(listBox1.SelectedItem!=null)
                    {
                        String q;
                        string[] tokens = listBox1.SelectedItem.ToString().Split(' ');

                        q = "delete from Consultatie where Data='" + tokens[0] + "' and Ora='" + tokens[1] + "' and CNP='" + CNPMaster + "'";
                        SqlConnection con = new SqlConnection(init);
                        con.Open();
                        SqlCommand cmd = new SqlCommand(q, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        listBox1.Items.Clear();
                        con.Open();
                        q = "select C.Data, C.Ora, I.Nume, I.Prenume from Consultatie C join Info_Stomatolog I on C.ID_Stomatolog=I.ID_Stomatolog and C.CNP='" + CNPMaster + "' and (C.Data>GETDATE() or (C.Data=GETDATE() and C.Ora>(SELECT CONVERT(TIME,GETDATE()))))";
                        cmd = new SqlCommand(q, con);
                        SqlDataReader read = cmd.ExecuteReader();

                        while (read.Read())
                        {
                            listBox1.Items.Add(read["Data"].ToString().Substring(0, read["Data"].ToString().IndexOf(' ')) + " " + read["Ora"].ToString().Substring(0, 5) + " " + read["Nume"].ToString() + " " + read["Prenume"].ToString());
                        }

                        con.Close();
                    }
                }

                if (comboBox1.Text.ToString().Equals("Complete Appointment"))
                {
                    if(listBox1.SelectedIndex!=-1)
                    {
                        Form5 f5 = new Form5();
                        Consult = listBox1.SelectedItem.ToString();
                        this.Hide();
                        f5.ShowDialog();
                        this.Show();
                    }                    
                }

                if (comboBox1.Text.ToString().Equals("Bill Appointment"))
                {
                    if (listBox1.SelectedIndex != -1)
                    {
                        Consult = listBox1.SelectedItem.ToString();
                        Form6 f6 = new Form6();
                        this.Hide();
                        f6.ShowDialog();
                        this.Show();  
                    }                                      
                }
            }
        }
    }
}
