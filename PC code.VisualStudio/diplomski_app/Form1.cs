using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace diplomski_app
{
    public partial class Form1 : Form
    {
        public SerialPort a;
        bool kontrola = true;
        int br_taskova=-1;
        int[] taskovi = new int[5];
        int[] vrijeme = new int[5];
        
        public Form1()
        {
            InitializeComponent();
        }
        private void otvori_port()
        {
            for (int i = 0; i < 5; i++)
            {
                taskovi[i] = 0;
            }
            try
            {
                a  = new SerialPort(comboBox1.Text,9600,Parity.None,8,StopBits.One);
                a.Open();
                a.ReadTimeout = 500;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "ERROR");
                kontrola = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            otvori_port();
            if (kontrola == true)
            {
                tabControl1.SelectTab(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            a.Write("C");
            int kontrola = 1;
            try
            {
                br_taskova = Convert.ToInt32(a.ReadChar());
            }
            catch (Exception ex) { 
                MessageBox.Show("Please turn ON PIC, Error:"+ ex.ToString() );
                kontrola = 0;
            }
            if (kontrola == 1)
            {
                if (br_taskova < 1 || br_taskova > 5)
                {
                    MessageBox.Show("Tasks are not loaded! Try again.");
                    tabControl1.SelectTab(1);
                }
                else
                {
                    for (int i = 0; i < br_taskova; i++)
                    {
                        taskovi[i] = 1;
                    }
                    tabControl1.SelectTab(2);

                    if (taskovi[0] == 0) { label14.Text = "No task!"; label14.ForeColor = Color.Red; }
                    if (taskovi[1] == 0) { label15.Text = "No task!"; label15.ForeColor = Color.Red; }
                    if (taskovi[2] == 0) { label16.Text = "No task!"; label16.ForeColor = Color.Red; }
                    if (taskovi[3] == 0) { label17.Text = "No task!"; label17.ForeColor = Color.Red; }
                    if (taskovi[4] == 0) { label18.Text = "No task!"; label18.ForeColor = Color.Red; }
                }
                int[] startovani = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    startovani[i] = Convert.ToInt32(a.ReadChar());
                }
                if (startovani[0] == 1)
                {
                    pictureBox1.Image = Properties.Resources.Box_Green;
                    label20.Text = "Active!";
                }
                if (startovani[1] == 1)
                {
                    pictureBox2.Image = Properties.Resources.Box_Green;
                    label21.Text = "Active!";
                }
                if (startovani[2] == 1)
                {
                    pictureBox3.Image = Properties.Resources.Box_Green;
                    label22.Text = "Active!";
                }
                if (startovani[3] == 1)
                {
                    pictureBox4.Image = Properties.Resources.Box_Green;
                    label23.Text = "Active!";
                }
                if (startovani[4] == 1)
                {
                    pictureBox5.Image = Properties.Resources.Box_Green;
                    label24.Text = "Active!";
                }
                for (int i = 0; i < 5; i++)
                {
                    int broj = 0;
                    int pom;
                    int t = 10000;
                    for (int j = 0; j < 5; j++)
                    {
                        pom = a.ReadChar();
                        broj = broj + pom * t;
                        t = t / 10;
                    }
                    vrijeme[i] = broj;
                    int kon = a.ReadChar();
                }
                label30.Text = Convert.ToString(vrijeme[0]);
                label31.Text = Convert.ToString(vrijeme[1]);
                label32.Text = Convert.ToString(vrijeme[2]);
                label33.Text = Convert.ToString(vrijeme[3]);
                label34.Text = Convert.ToString(vrijeme[4]);
            }
            else {
                return;
            }
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (taskovi[0] == 1)
            {
                a.Write("A");
                a.Write("0");
                pictureBox1.Image = Properties.Resources.Box_Green;
                label20.Text = "Active!";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (taskovi[0] == 1)
            {
                a.Write("D");
                a.Write("0");
                pictureBox1.Image = Properties.Resources.Box_Red;
                label20.Text = "Not active!";
            }
        }
        
        
        private void button5_Click(object sender, EventArgs e)
        {
            if (taskovi[1] == 1)
            {
                a.Write("A");
                a.Write("1");
                pictureBox2.Image = Properties.Resources.Box_Green;
                label21.Text = "Active!";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (taskovi[1] == 1)
            {
                a.Write("D");
                a.Write("1");
                pictureBox2.Image = Properties.Resources.Box_Red;
                label21.Text = "Not active!";
            }
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            if (taskovi[2] == 1)
            {
                a.Write("A");
                a.Write("2");
                pictureBox3.Image = Properties.Resources.Box_Green;
                label22.Text = "Active!";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (taskovi[2] == 1)
            {
                a.Write("D");
                a.Write("2");
                pictureBox3.Image = Properties.Resources.Box_Red;
                label22.Text = "Not active!";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (taskovi[3] == 1)
            {
                a.Write("A");
                a.Write("3");
                pictureBox4.Image = Properties.Resources.Box_Green;
                label23.Text = "Active!";
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (taskovi[3] == 1)
            {
                a.Write("D");
                a.Write("3");
                pictureBox4.Image = Properties.Resources.Box_Red;
                label23.Text = "Not active!";
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (taskovi[4] == 1)
            {
                a.Write("A");
                a.Write("4");
                pictureBox5.Image = Properties.Resources.Box_Green;
                label24.Text = "Active!";
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (taskovi[4] == 1)
            {
                a.Write("D");
                a.Write("4");
                pictureBox5.Image = Properties.Resources.Box_Red;
                label24.Text = "Not active!";
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {

            tabControl1.SelectTab(3);
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }
        void ispisi_broj() {

            char[] buffer ={'0','1','2','3','4','5','6','7','8','9'};
            int aa=Convert.ToInt32(textBox1.Text);
            int p;
            int t=1000;
            for(int i=0;i<4;i++){
                p=(int)(aa/t);
                string cifra = buffer[p].ToString();
                a.Write(cifra);
                aa=aa-p*t;
                t=t/10;
            }
        
        }
        private void update_vremena(int br_taska, int t, int ms_or_sec){
            if (ms_or_sec == 1)
            {
                vrijeme[br_taska] = t * 1000;
                if (br_taska == 0) { label30.Text = Convert.ToString(t * 1000); }
                if (br_taska == 1) { label31.Text = Convert.ToString(t * 1000); }
                if (br_taska == 2) { label32.Text = Convert.ToString(t * 1000); }
                if (br_taska == 3) { label33.Text = Convert.ToString(t * 1000); }
                if (br_taska == 4) { label34.Text = Convert.ToString(t * 1000); }
            }
            else
            {
                vrijeme[br_taska] = t;
                if (br_taska == 0) { label30.Text = Convert.ToString(t); }
                if (br_taska == 1) { label31.Text = Convert.ToString(t); }
                if (br_taska == 2) { label32.Text = Convert.ToString(t); }
                if (br_taska == 3) { label33.Text = Convert.ToString(t); }
                if (br_taska == 4) { label34.Text = Convert.ToString(t); }
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            try {
                if (comboBox2.Text == "" || textBox1.Text == "" || (radioButton1.Checked == false && radioButton2.Checked == false))
            {
                MessageBox.Show("Please enter all parameters!");
            }
            
            else {
                
                    int br=Convert.ToInt32(comboBox2.Text)-1;
                    string task = Convert.ToString(br);
                    a.Write("U");
                    int aa=Convert.ToInt32(textBox1.Text);
                    a.Write(task);
                    ispisi_broj();

                    if (radioButton1.Checked == true) {
                        a.Write("1");
                        update_vremena(br, aa, 1);
                    }
                    if (radioButton2.Checked == true) {
                        a.Write("0");
                        update_vremena(br, aa, 0);
                    }
                    int kon = a.ReadChar();
                    if (kon == 0)
                    {

                        MessageBox.Show("Error to update task");
                        
                    }
                    else {
                        MessageBox.Show("Task is successfully updated");
                        tabControl1.SelectTab(2);
                    }
                    
                    
           }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "ERROR");
                kontrola = false;
            }
            
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if(!Char.IsDigit(ch) && ch != 8 ){
                e.Handled=true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(2);
        }
    }
}
