using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;
using Microsoft.Win32;
using System.Drawing;
using System.IO;


namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , ServiceChat.IServiceChatCallback
    {
        bool IsConnected = false;
        ServiceChat.ServiceChatClient client;
        int ID;
        public MainWindow()
        {
            InitializeComponent();
        }

        void ConnectUser()
        {
            if(!IsConnected)
            {
  
               
                    ID = client.Connect(tbUserName.Text);
                    IsConnected = true;
                    CB.Content = "Disconnect";
                    tbUserName.IsEnabled = false;
               

            }
            }

        void DisconnectUser()
        {
            if (IsConnected)
            {
                client.Disconnect(ID);
                IsConnected = false;
                CB.Content = "Connect";
                tbUserName.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsConnected)
                ConnectUser();
            else
                DisconnectUser();
        }

        public void MsgCallback(string msg)
        {
            LB_Chat.Items.Add(msg);
            LB_Chat.ScrollIntoView(LB_Chat.Items[LB_Chat.Items.Count - 1]);
            
        }

        public void ImgCallback(byte[] img)
        {
            Image image = new Image();
            var bi = LoadImage(img);
            var targetBitmap = new TransformedBitmap(bi, new ScaleTransform(0.1, 0.1));
            image.Source = targetBitmap;
            LB_Chat.Items.Add(image);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceChat.ServiceChatClient(new System.ServiceModel.InstanceContext(this));

        }

        private void Tb_Msg_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (IsConnected)
                {
                    client.SendMsg(tb_Msg.Text, ID);
                    tb_Msg.Text = string.Empty;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(IsConnected)
            client.Disconnect(ID);
        }

        private void ImgButt_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".jpeg";
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document

                    string filename = dlg.FileName;
                    
                    BitmapImage bi = new BitmapImage(new Uri(filename));



                    byte[] img = getJPGFromImageControl(bi);
                    client.SendImg(img, ID);


                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
        }


        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

    }


}
