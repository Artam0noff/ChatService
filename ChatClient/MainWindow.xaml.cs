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

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , ServiceChat.IServiceChatCallback
    {
        bool IsConnected = false;
        ServiceChat.ServiceChatClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        void ConnectUser()
        {
            if(!IsConnected)
            {
                IsConnected = true;
                CB.Content = "Disconnect";
                tbUserName.IsEnabled = false;
            }
        }

        void DisconnectUser()
        {
            if (IsConnected)
            {
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
            throw new NotImplementedException();
        }
    }


}
