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
    public partial class Form3 : Form
    {
        //Declarare legatura DB
        public string init = "Data Source = ASPIREVX15\\SQLEXPRESS; Initial Catalog = Cabinet; Integrated Security = True";
        int select = 0;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            textBox4.Hide();
            checkBox1.Hide();
            button1.Hide();
        }
        
        //Atentionare inchidere
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
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

        //Deschidere meniu ADD USER
        private void addClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "User";
            label2.Text = "Password";
            label3.Text = "Password";
            label1.Show();
            label2.Show();
            label3.Show();
            label4.Hide();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            textBox4.Hide();
            checkBox1.Show();
            textBox2.PasswordChar = '*';
            textBox3.PasswordChar = '*';
            button1.Location=new Point(125,250);
            button1.Show();
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            select = 1;
        }

        //Adaugare USER
        private void button1_Click(object sender, EventArgs e)
        {
            if(select==1)
            {
                String User="User", Pass="Pass", Pass2="Pass2", Admin="0";
                int loop = 1, err = 0;
                
                //Verificare credentiale
                while(loop==1)
                {
                    loop = 0;
                    User = Convert.ToString(textBox1.Text);
                    Pass = Convert.ToString(textBox2.Text);
                    Pass2 = Convert.ToString(textBox3.Text);
                    
                    if (User.Equals(" ") || User.Equals(null))
                    {
                        loop = 1;
                        err = 1;
                    }
                    
                    if (Pass.Equals(" ") || Pass.Equals(null))
                    {
                        loop = 1;
                        err = 2;
                    }
                   
                    if (loop == 1)
                    {
                        if (err == 1)
                        {
                            MessageBox.Show("Invalid user");
                            textBox1.Text = null;
                        }                           
                        
                        else
                            MessageBox.Show("Invalid Password");
                        textBox2.Text = null;
                        textBox3.Text = null;
                    } 
                }

                //Verificare existenta user in DB
                if(Pass.Equals(Pass2))
                {
                    SqlConnection con = new SqlConnection(init);
                    con.Open();
                    if (checkBox1.Checked == true)
                        Admin = "1";
                    else
                        Admin = "0";
                    string q = "select Us from LogIn where Us ='" + User + "'";
                    SqlCommand cmd = new SqlCommand(q, con);
                    SqlDataReader read = cmd.ExecuteReader();
                    err = 0;
                    
                    if (read.Read())
                    {
                        if (User.Equals(read["Us"].ToString()))
                        {
                            textBox1.Text = null;
                            textBox2.Text = null;
                            textBox3.Text = null;
                            checkBox1.Checked = false;
                            MessageBox.Show("Invalid user");                            
                        }
                       
                        err = 3;
                    }
                    con.Close();

                    //Adaugare user in DB
                    if (err==0)
                    {
                        q = "insert into LogIn(Us,Pwd,Super) values('" + User + "','" + Pass + "'," + Admin + ")";
                        cmd = new SqlCommand(q, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Done!");
                        textBox1.Text = null;
                        textBox2.Text = null;
                        textBox3.Text = null;
                        checkBox1.Checked = false;
                        label1.Hide();
                        label2.Hide();
                        label3.Hide();
                        label4.Hide();
                        textBox1.Hide();
                        textBox2.Hide();
                        textBox3.Hide();
                        textBox4.Hide();
                        checkBox1.Hide();
                        button1.Hide();
                    }
                }

                else
                {
                    textBox2.Text = null;
                    textBox3.Text = null;
                    MessageBox.Show("Unmatching password fields");
                }
            }

            if(select==2)
            {
                int err = 0;
                String FN1, FN2, LN1, LN2;
                //Citire N,P
                FN1 = Convert.ToString(textBox1.Text);
                LN1 = Convert.ToString(textBox2.Text);
                FN2 = Convert.ToString(textBox3.Text);
                LN2 = Convert.ToString(textBox1.Text);

                SqlConnection con = new SqlConnection(init);
                con.Open();

                string q = "select Nume, Prenume from Info_Stomatolog where Nume ='" + FN1 + "'and Prenume='"+LN1+"'";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader read = cmd.ExecuteReader();
                
                //Verificare existenta 
                if (read.Read())
                {
                    if (FN1.Equals(read["Nume"].ToString())&& LN1.Equals(read["Prenume"].ToString()))
                    {
                        textBox1.Text = null;
                        textBox2.Text = null;
                        textBox3.Text = null;
                        textBox4.Text = null;
                        MessageBox.Show("Already in system");
                    }

                    err = 1;
                    con.Close();
                }

                //Adaugare in DB
                else
                {
                    con.Close();
                    con.Open();
                    q = "insert into Info_Stomatolog(Nume,Prenume,Nume_Asistent,Prenume_Asistent) values('" + FN1 + "','" + LN1 + "','" + FN2 + "','" + LN2 + "')";
                    cmd = new SqlCommand(q, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox4.Text = null;
                    MessageBox.Show("Done!");
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    checkBox1.Checked = false;
                    label1.Hide();
                    label2.Hide();
                    label3.Hide();
                    label4.Hide();
                    textBox1.Hide();
                    textBox2.Hide();
                    textBox3.Hide();
                    textBox4.Hide();
                    checkBox1.Hide();
                    button1.Hide();
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Stomatologist's first name";
            label2.Text = "Stomatologist's last name";
            label3.Text = "Assistant's first name";
            label4.Text = "Assistant's last name";
            label1.Show();
            label2.Show();
            label3.Show();
            label4.Show();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            textBox4.Show();
            checkBox1.Hide();
            button1.Location = new Point(125, 250);
            button1.Show();
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox2.PasswordChar = '\0';
            textBox3.PasswordChar = '\0';
            select = 2;
        }

        private void optiuniToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            this.Hide();
            f7.ShowDialog();
            this.Show();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            this.Hide();
            f8.ShowDialog();
            this.Show();
        }
    }
}
