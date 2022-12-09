using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceReference1;
using System.ServiceModel;
using System.ServiceModel.Channels;


namespace WCFClient
{
    public partial class Form1 : Form
    {
        private PipeVolumeCalculatorClient _client;

        private string _address; // "http://localhost:51085/Service.svc"
        private string _clientLogin;
        private string _clientPassword;
        private string _clientToken;

        private string _logError;
        private string _logAuthDone = "Вы успешно авторизировались!";
        private string _logDone = "Готово!";

        private bool _isAuthorized;

        public Form1()
        {
            InitializeComponent();

            button1.Enabled = false;

            comboBox1.Items.Add("http://localhost:51085/Service.svc");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double S;
            double L;
            double calculatedVolume = 0;

            try
            {
                S = double.Parse(textBox1.Text.Replace('.', ','));
                L = double.Parse(textBox2.Text.Replace('.', ','));

                calculatedVolume = _client.CalculatePipeVolumeAsync(L, S).Result;
                label8.Text = calculatedVolume.ToString();

                _client.CloseAsync();
            }
            catch (Exception ex)
            {
                _logError = "Ошибка формата вводимых данных!";
                MessageBox.Show("Ошибка формата вводимых данных!");
            }

            ShowLog(calculatedVolume);
        }

        private void ShowLog(double res)
        {
            label6.Text = res == 0 ? _logError : _logDone;
        }

        private bool IsValidAuthData()
        {
            return new string[] { _clientLogin, _clientPassword, _address }.All(field => !string.IsNullOrEmpty(field));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _clientLogin = textBox3.Text;
            _clientPassword = textBox4.Text;
            _address = comboBox1.SelectedIndex == -1 ? "" : comboBox1.SelectedItem.ToString();

            if (!IsValidAuthData())
            {
                MessageBox.Show("Ошибка! Поле или несколько полей авторизации пустые");
                return;
            }
            
            _client = new PipeVolumeCalculatorClient(new WSHttpBinding(), new EndpointAddress(_address));

            User user = new User
            {
                _login = _clientLogin,
                _password = _clientPassword
            };

            try
            {
                if (!_client.AuthorizeAsync(user).Result)
                {
                    _client.Abort();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            button1.Enabled = !button1.Enabled;
            MessageBox.Show($"{_logAuthDone}\n Login: {_clientLogin}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
