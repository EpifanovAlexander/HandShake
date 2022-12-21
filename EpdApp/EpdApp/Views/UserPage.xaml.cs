using Android.App;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.Util;
using EpdApp.Services;
using EpdApp.Services.UsersService;
using EpdApp.Services.XmlsService;
using EpdApp.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Android.Content;
using EpdApp.Services.DocumentsService;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using EpdApp.Utils;
using Document = EpdApp.Services.DocumentsService.Document;

namespace EpdApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage, NfcReader.AccountCallback
    {
        /// <summary>
        /// Порт, на который будут отправляться документы к инспектору
        /// </summary>
        private int listenPort = 11000;

        /// <summary>
        /// ViewModel пользователя
        /// </summary>
        private UserViewModel userModel;

        /// <summary>
        /// Выбранный документ для отправки инспектору
        /// </summary>
        private XmlDoc SelectedDoc;

        public UserPage(UserRoles role)
        {
            InitializeComponent();
            BindingContext = userModel = new UserViewModel(role);
            Title = (role == UserRoles.Police) ? "Inspector" : "User";
        }

        /// <summary>
        /// Обработчик выхода из аккаунта пользователя
        /// </summary>
        private async void LogOutHandler(object sender, EventArgs e)
        {
            UserService.LogOut();
            await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }

        #region Drvier
        /// <summary>
        /// Оновляет на форме список Xml-документов водителя
        /// </summary>
        private void UpdateXmlDocuments()
        {
            if (userModel.IsDriver)
            {
                userModel.Documents.Clear();
                foreach (var file in DocumentsService.GetDocuments())
                {
                    userModel.Documents.Add(file);
                }
                userModel.UpdateXmlDocumentsIsExist();
            }
        }

        /// <summary>
        /// Оновляется список Xml-документов водителя при каждом появлении страницы
        /// </summary>
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            //UpdateXmlDocuments();
        }

        /// <summary>
        /// Обработчик нажатия на маску на фоне. Скрывает меню со способами предъявления ЭПД
        /// </summary>
        private void MaskTapped(object sender, EventArgs e)
        {
            var isSubMenuShowed = ShareEpd.IsVisible;
            ShareEpd.IsVisible = !isSubMenuShowed;
        }

        /// <summary>
        /// Обработчик выбора xml-документа для отправки
        /// </summary>
        private void XmlDocSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var isSubMenuShowed = ShareEpd.IsVisible;
            ShareEpd.IsVisible = !isSubMenuShowed;
            //ShareEpdLabel.Text = $"Предъявить {(e.SelectedItem as XmlDoc).Name}";
           // SelectedDoc = e.SelectedItem as XmlDoc;
        }

        /// <summary>
        /// Обработчик выбора способа предъявления ЭПД
        /// </summary>
        private void ShareTypeClicked(object sender, EventArgs e)
        {
            var senderStack = sender as StackLayout;
            if (senderStack == WifiType)
            {

                SendOverUDP(new Passport(
                    "6120", "210421",
                    "Ivan", "Dmitrievich",
                    "Marinin", 0,
                    new DateTime(2001, 04, 25)).ToString());
            }
            if (senderStack == BluetoothType)
            {
            }
            if (senderStack == NfcType)
            {
                Activity activity = CurrentActivityUtil.GetCurrentActivity();
                NfcAdapter nfc = NfcAdapter.GetDefaultAdapter(activity);
                if (nfc != null)
                {
                    
                }

                Document document = new Passport(
                    "6120", "210421", 
                    "Ivan", "Dmitrievich", 
                    "Marinin", 0, 
                    new DateTime(2001, 04, 25));

                DocumentsService.Document = document;
            }
            if (senderStack == AutoType)
            {
            }
        }

        /// <summary>
        /// Отправка текста через UDP
        /// </summary>
        /// <param name="message"> Сообщение для отправки </param>
        private async void SendOverUDP(string message)
        {
            try {
                var IpAddress = Dns.GetHostAddresses(Dns.GetHostName());
                string result = "";
                foreach (var ip in IpAddress)
                {
                    var _ip = ip.ToString();
                    if (ip.ToString().IndexOf("192.168.") == 0)
                    {
                        var index = ip.ToString().LastIndexOf(".");
                        result = _ip.Remove(index, _ip.Length - index);
                        result += ".255";
                        break;
                    }
                }

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                {
                    EnableBroadcast = true
                };

                IPAddress broadcast = IPAddress.Parse(result);

                byte[] sendbuf = Encoding.UTF8.GetBytes(message);
                IPEndPoint ep = new IPEndPoint(broadcast, 11000);

                socket.SendTo(sendbuf, ep);

                Console.WriteLine("Message sent to the broadcast address");
                await DisplayAlert("", "Документ отправлен", "ОK");
            } 
            catch(Exception) 
            {
                await DisplayAlert("Ошибка", "Документ не отправлен", "ОK");
            }
        }

        /// <summary>
        /// Обработчик скачивания xml-документа с сервера
        /// </summary>
        private async void GetEpdFromServer(object sender, EventArgs e)
        {
            UpdateXmlDocuments();
            /*
            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri($"http://178.172.227.162:8080/file/{EpdEntry.Text}");
                request.Method = HttpMethod.Get;
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    string path = $"ON_TRNACLGROT_{EpdEntry.Text}";
                    XmlService.Instance.SaveXmlFile(await responseContent.ReadAsStreamAsync(), path);
                    UpdateXmlDocuments();
                }
            }
            catch (Exception) 
            {
                await DisplayAlert("Ошибка", "Связь с сервером оборвалась...", "ОK");
            }*/
        }
        #endregion

        #region Insepctor
        /// <summary>
        /// Прослушивание UDP-пакетов для инспектора
        /// </summary>
        /// 
        public NfcReaderFlags READER_FLAGS = NfcReaderFlags.NfcA | NfcReaderFlags.SkipNdefCheck;
        private NfcReader mNfcReader;
        private void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort) { EnableBroadcast = true };
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            EnableReaderMode();
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    userModel.CurrentDocument = new Passport(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                    userModel.IsVerified = true;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }

        /// <summary>
        /// Обработчик запуска прослушивания UDP-пакетов для инспектора
        /// </summary>
        private void LoadEpdAction(object sender, EventArgs e)
        {
            Thread listenerThread = new Thread(StartListener);
            listenerThread.Start();
            EpdContent.IsVisible = true;
            EnableReaderMode();
        }

        
        private void EnableReaderMode()
        {
            mNfcReader = new NfcReader(new WeakReference<NfcReader.AccountCallback>(this));
            Activity activity = CurrentActivityUtil.GetCurrentActivity();
            NfcAdapter nfc = NfcAdapter.GetDefaultAdapter(activity);
            if (nfc != null)
            {
                nfc.EnableReaderMode(activity, mNfcReader, READER_FLAGS, null);
            }
        }
        #endregion

        #region AccountCallback implementation
        // This callback is run on a background thread, but updates to UI elements must be performed
        // on the UI thread.
        public void OnAccountRecieved(string account)
        {
            userModel.CurrentDocument = new Passport(account);
            userModel.IsVerified = true;
        }

        #endregion
    }
}