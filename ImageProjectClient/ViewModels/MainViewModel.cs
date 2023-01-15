using ImageProjectClient.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProjectClient.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand ChooseImageCommand { get; set; }
        public RelayCommand SendToServerCommand { get; set; }

        public bool IsConnected { get; set; }
        public string IP_Address { get; set; }


        private string myImageSource;

        public string MyImageSource
        {
            get { return myImageSource; }
            set { myImageSource = value; OnPropertyChanged(); }
        }

        private Button chooseImageButton;

        public Button ChooseImageButton
        {
            get { return chooseImageButton; }
            set { chooseImageButton = value; OnPropertyChanged(); }
        }

        private Button sendToServer;

        public Button SendToServer
        {
            get { return sendToServer; }
            set { sendToServer = value; OnPropertyChanged(); }
        }

        public byte[] BitmapImagetoByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }


        public MainViewModel()
        {
            ConnectCommand = new RelayCommand(c =>
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                Console.WriteLine(hostName);
                // Get the IP
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                IP_Address = myIP;
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ipAddress = IPAddress.Parse($"{IP_Address}"); // Change IP
                var port = 27001; // doesn't work, use 80;
                var ep = new IPEndPoint(ipAddress, port);
                try
                {
                    MessageBox.Show($"User with this IP : {IP_Address} is connected to server.");
                }
                catch (Exception)
                {
                    MessageBox.Show("Can not connect to server,please try again . . . .");
                }
                ChooseImageButton.IsEnabled = true;
                SendToServer.IsEnabled = true;
                ////Commit2

            });


            SendToServerCommand = new RelayCommand(s =>
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                Console.WriteLine(hostName);
                // Get the IP
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                IP_Address = myIP;
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ipAddress = IPAddress.Parse($"{IP_Address}"); // Change IP
                var port = 27001; // doesn't work, use 80;
                var ep = new IPEndPoint(ipAddress, port);
                try
                {
                    if (IsConnected == false)
                    {
                        socket.Connect(ep);
                        IsConnected = true;
                    }
                    if (IsConnected == true)
                    {
                        Uri imageUri = new Uri(MyImageSource, UriKind.Relative);
                        BitmapImage imageBitmap = new BitmapImage(imageUri);
                        ImageBrush image = new ImageBrush();
                        image.ImageSource = imageBitmap;


                        var bytes = BitmapImagetoByteArray(imageBitmap);
                        socket.Send(bytes);


                    }







                }
                catch (Exception)
                {
                    MessageBox.Show("Can not connect to server,please try again . . . .");
                }
            });
            //n

            ChooseImageCommand = new RelayCommand(c =>
            {
                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

                // Launch OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = openFileDlg.ShowDialog();
                // Get the selected file name and display in a TextBox.
                // Load content of file in a TextBlock
                if (result == true)
                {
                    MyImageSource = openFileDlg.FileName;
                }
            });

        }

    }
}
