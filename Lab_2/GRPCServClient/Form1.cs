using System;
using System.Windows.Forms;
using PipeVolumeCalcService;
using Grpc.Net.Client;
using System.Net.Http;
using Grpc.Core;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Linq;

namespace GRPCServClient
{
    public partial class Form1 : Form
    {
        private string _address; // "https://127.0.0.1:5001"
        private string _clientLogin;
        private string _clientPassword;
        private string _clientToken;

        private string _logError;
        private string _logAuthDone = "Вы успешно авторизировались!";
        private string _logDone = "Готово!";

        private PipeVolumeCalculation.PipeVolumeCalculationClient client;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("https://127.0.0.1:5001");

            button1.Enabled = false;
        }

        private GrpcChannel GetAuthGrpcChannel(string addr)
        {
            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_clientToken))
                    metadata.Add("Authorization", $"Bearer {_clientToken}");
                
                return Task.CompletedTask;
            });

            HttpClientHandler httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
         
            GrpcChannel channel = GrpcChannel.ForAddress(addr, new GrpcChannelOptions
            {
                HttpHandler = httpHandler,
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;            
        }

        private bool Login(PipeVolumeCalculation.PipeVolumeCalculationClient client, string login, string password)
        {
            try
            {
                var reply = client.AuthorizeWithSession(new AuthRequest { Login = login, Password = password });    //!!!
                _clientToken = reply.SessionToken;

                if (_clientToken != null)
                    return true;

                MessageBox.Show("Ошибка! Токен не был сформирован!");
                return false;
            }
            catch(RpcException e)
            {
                MessageBox.Show("Ошибка! " + e.Message);
                _clientToken = "";
                return false;
            }
        }

        private bool IsValidAuthData()
        {
            return new string[] { _clientLogin, _clientPassword, _address }.All(field => !string.IsNullOrEmpty(field));
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

                HttpClientHandler httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                
                var reply = client.Calculate(new CalcRequest { S = S, L = L });

                label8.Text = reply.V.ToString();
            }
            catch (FormatException)
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

        private void button2_Click(object sender, EventArgs e)
        {
            _clientLogin = textBox3.Text;
            _clientPassword = textBox4.Text;
            _address = comboBox1.SelectedItem.ToString();

            if (!IsValidAuthData())
            {
                MessageBox.Show("Ошибка! Поле или несколько полей авторизации пустые");
                return;
            }

            GrpcChannel channel = GetAuthGrpcChannel(_address);
            client = new PipeVolumeCalculation.PipeVolumeCalculationClient(channel);

            if (!Login(client, _clientLogin, _clientPassword))
                return;

            button1.Enabled = !button1.Enabled;
            MessageBox.Show($"{_logAuthDone}\n Login: {_clientLogin}");
        }
    }
}
