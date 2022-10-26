using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SAXON
{
    public partial class Form1 : Form
    {
        public static int mixturesAmount;
        public static int ciclesAmount;
        public static int currentCicle;
        public static int currentMixture;
        private int valuesIterator;
        private int _seconds;
        private static int measurementSeconds;
        private double[] measurementValues = new double[20];
        private double value;
        private double valuesSum;
        Timer timer;
        static TextBox[] MixturesBoxes = new TextBox[5];
        static TextBox[] MixturesAmountBoxes = new TextBox[5];
        private object[] Charts = new object[3];
        static Mixture[] StaticMixtureList;
        static ComboBox _resultsBox;
        public static bool isAuto;

        public Form1()
        {
            InitializeComponent();
            timer = new Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();
            MixturesBoxes[0] = mixturesName1;
            MixturesBoxes[1] = mixturesName2;
            MixturesBoxes[2] = mixturesName3;
            MixturesBoxes[3] = mixturesName4;
            MixturesBoxes[4] = mixturesName5;
            MixturesAmountBoxes[0] = results1;
            MixturesAmountBoxes[1] = results2;
            MixturesAmountBoxes[2] = results3;
            MixturesAmountBoxes[3] = results4;
            MixturesAmountBoxes[4] = results5;
            SetTable();
            _resultsBox = resultsBox;
            Charts[0] = manualChart;
            Charts[1] = autoChart;
        }

        private void ManualMeasurementStart()
        {
            measurementTimer.Enabled = true;
            manualChart.ChartAreas[0].AxisY.Maximum = 1500;
            manualChart.ChartAreas[0].AxisY.Minimum = -100;
            
            manualChart.ChartAreas[0].AxisX.LabelStyle.Format = "H.mm.ss";
            manualChart.Series[0].XValueType = ChartValueType.DateTime;
            manualChart.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            manualChart.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(2).ToOADate();
            manualChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            manualChart.ChartAreas[0].AxisX.Interval = 5;
        }
        

        private void MeasureMixture()
        {
            measurementSeconds = 0;
            progressBar.Maximum = 20;
            progressBar.Value = 0;
            concentrationTimer.Start();
            
            

        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.statusStripDate.Text = DateTime.Now.ToLongDateString();
            this.statusStripTime.Text = DateTime.Now.ToLongTimeString();
        }

        public static void CreateMixturesList(string[] mixtureNames)
        {
            Mixture[] MixtureList = new Mixture[mixturesAmount];
            for (int i = 0; i < mixturesAmount; i++)
            {
                MixtureList[i] = new Mixture(mixtureNames[i], new double[ciclesAmount]);
            }
            StaticMixtureList = MixtureList;
            _resultsBox.DataSource = StaticMixtureList;
            _resultsBox.DisplayMember = "Name";

        }
        public static void SetTable()
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < ciclesAmount)
                {
                    MixturesBoxes[i].Text = "Измерение " + (i + 1);
                    MixturesAmountBoxes[i].Text = "0";
                }
                else
                {
                    MixturesBoxes[i].Text = "";
                    MixturesAmountBoxes[i].Text = "";
                }
            }

        }
        public class Mixture
        {
            public string Name { get; set; }
            public double[] concentrations;
            public double average;
            public Mixture(string name, double[] concentrations)
            {
                Name = name;
                this.concentrations = concentrations;

            }
        }

        private void quitProgram_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void resultsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mixture Mixture = (Mixture)resultsBox.SelectedItem;
            for (int i = 0; i < ciclesAmount; i++)
            {
                MixturesAmountBoxes[i].Text = Mixture.concentrations[i].ToString();
                averageResult.Text = Mixture.average.ToString();
            }
        }

        private void autoMeasurement_Click(object sender, EventArgs e)
        {
            Form2 fillAmounts = new Form2();
            fillAmounts.Show();
            autoMeasurement.Enabled = false;
            ManualMeasurementStart();
            progressBar.Minimum = 0;
            progressBar.Maximum = 400;
            autoChart.ChartAreas[0].AxisY.Maximum = 1500;
            autoChart.ChartAreas[0].AxisY.Minimum = -100;
            autoChart.ChartAreas[0].AxisX.LabelStyle.Format = "H.mm.ss";
            autoChart.Series[0].XValueType = ChartValueType.DateTime;
            autoChart.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            autoChart.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(60).ToOADate();
            autoChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
            autoChart.ChartAreas[0].AxisX.Interval = 10;
            measureButton.Enabled = true;
            isAuto = true;
        }

        public static void StartAutoMeasurement()
        {
            currentCicle = 0;
            currentMixture = 0;
            Form3 NextMixture = new Form3();
            NextMixture.labelText.Text = "Подайте смесь номер:" + StaticMixtureList[0].Name;
            NextMixture.Show();
            
            
            
        }

        private void about_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия программы: 0.0.1 \n Сделано в Vusial Studio Winforms(NETFramework,Version=v4.7.2).",
                "Информация о программе", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void measurementTimer_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            value = random.Next(900, 920);
            manualChart.Series[0].Points.AddXY(DateTime.Now, value);
            if (isAuto == true)
            {
                autoChart.Series[0].Points.AddXY(DateTime.Now, value);
                progressBar.PerformStep();
            }
            _seconds++;
            currentConcentration.Text = value.ToString();
            if(value < 0)
            {
                currentConcentration.ForeColor = Color.Red;
            }
            else
            {
                currentConcentration.ForeColor = Color.Black;
            }

            if (_seconds > 120)
            {
                _seconds = 0;
                manualChart.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
                manualChart.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(2).ToOADate();
                manualChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                manualChart.ChartAreas[0].AxisX.Interval = 5;
            }
            if(progressBar.Value == 400)
            {
                MeasureMixture();
            }
        }

        

        private void manualButton_Click(object sender, EventArgs e)
        {
            ManualMeasurementStart();
        }

        private void concentrationTimer_Tick(object sender, EventArgs e)
        {
            if (measurementSeconds < 20)
            {
                measurementValues[measurementSeconds] = value;
            }
            measurementSeconds++;
            Form3 nextMixture = new Form3();
            if (measurementSeconds > 20)
            {
                if (StaticMixtureList[currentMixture].concentrations[currentCicle] == 0)
                {
                    StaticMixtureList[currentMixture].concentrations[currentCicle] = value;
                    nextMixture.labelText.Text = "Подайте нулевую смесь";
                    currentAction.Text = "Нулевой газ";
                    measureButton.Enabled = true;
                }
                else
                {
                    StaticMixtureList[currentMixture].concentrations[currentCicle] -= value;
                    valuesSum = 0;
                    valuesIterator = 0;
                    foreach(double value in StaticMixtureList[currentMixture].concentrations)
                    {
                        if(value != 0)
                        {
                            valuesSum += value;
                        }
                    }
                    StaticMixtureList[currentMixture].average = valuesSum / valuesIterator;

                    currentMixture++;
                    if (currentMixture >= mixturesAmount)
                    {
                        currentCicle++;
                        currentMixture = 0;
                    }
                    if (currentCicle > (ciclesAmount - 1))
                    {
                        nextMixture.labelText.Text = "Измерения завершены!Ура!";
                        autoMeasurement.Enabled = true;
                        nextMixture.Show();
                        isAuto = false;
                    }
                    else
                    {
                        nextMixture.labelText.Text = "Подайте смесь номер :" + StaticMixtureList[currentMixture].Name;
                        currentAction.Text = "Смесь: " + StaticMixtureList[currentMixture].Name;
                        measureButton.Enabled = true;
                    }
                }
                resultsBox_SelectedIndexChanged(null, null);
                nextMixture.Show();
                concentrationTimer.Enabled = false;
                progressBar.Value = 0;
                progressBar.Maximum = 400;
                

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MeasureMixture();
            measureButton.Enabled = false;
        }
    }
}
