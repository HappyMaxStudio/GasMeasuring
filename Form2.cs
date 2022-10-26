using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAXON
{
    public partial class Form2 : Form
    {
        TextBox[] cylinderNumbers = new TextBox[5];
        public Form2()
        {
            InitializeComponent();
            cylinderNumbers[0] = mixture1;
            cylinderNumbers[1] = mixture2;
            cylinderNumbers[2] = mixture3;
            cylinderNumbers[3] = mixture4;
            cylinderNumbers[4] = mixture5;

        }
        private void startMeasurement_Click(object sender, EventArgs e)
        {
            Form1.mixturesAmount = Convert.ToInt32(this.mixturesAmountValue.Text);
            Form1.ciclesAmount = Convert.ToInt32(this.ciclesAmountValue.Text);
            string[] MixturesNumbers = new string[Convert.ToInt32(this.mixturesAmountValue.Text)];
            for (int i = 0; i < Convert.ToInt32(this.mixturesAmountValue.Text); i++)
            {
                MixturesNumbers[i] = cylinderNumbers[i].Text;
            }
            Form1.CreateMixturesList(MixturesNumbers);
            Form1.SetTable();
            Form1.StartAutoMeasurement();
            this.Close();
        }


        private void mixturesAmountValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < 5; i++)
            {
                if(i < Convert.ToInt32(mixturesAmountValue.Text))
                {
                    cylinderNumbers[i].Enabled = true;
                }
                else
                {
                    cylinderNumbers[i].Text = "";
                    cylinderNumbers[i].Enabled = false;
                }
            }
        }
    }
}
