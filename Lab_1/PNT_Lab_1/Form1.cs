using System;
using System.Windows.Forms;
using ServiceReference2;

namespace PNT_Lab_1
{
    public partial class Form1 : Form
    {
        // Web-service interaction
        ServiceReference2.CalcPipeVolumeServiceSoapClient calculateServiceSoap = 
            new ServiceReference2.CalcPipeVolumeServiceSoapClient(ServiceReference2.CalcPipeVolumeServiceSoapClient.EndpointConfiguration.CalcPipeVolumeServiceSoap);

        private string _logError;
        private string _logDone = "Готово!";

        public Form1()
        {
            InitializeComponent();
        }

        private void ShowLog(double res)
        {
            label6.Text = res == 0 ? _logError : _logDone;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double S, L;
            double calculatedVolume = 0;

            try
            {
                S = double.Parse(textBox1.Text.Replace('.', ','));
                L = double.Parse(textBox2.Text.Replace('.', ','));

                calculatedVolume = calculateServiceSoap.CalculatePipeVolumeAsync(S, L).Result;
                label8.Text = calculatedVolume.ToString();
            }
            catch (FormatException)
            {
                _logError = "Ошибка формата вводимых данных!";
            }

            ShowLog(calculatedVolume);
        }
    }
}
