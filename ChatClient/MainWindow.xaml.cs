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
    }


}
