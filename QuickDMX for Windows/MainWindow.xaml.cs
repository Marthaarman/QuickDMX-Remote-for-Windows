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
using System.Net.Sockets;
using System.Xml;

namespace QuickDMX_for_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Byte[] btn_list_msg =  {
            0x42, 0x55, 0x54, 0x54, 0x4f, 0x4e, 0x5f, 0x4c,
            0x49, 0x53, 0x54, 0x0d, 0x0a
        };

        

        private TcpClient client;
        private NetworkStream stream;
        private bool started = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        public struct buttonData
        {
            public int index;
            public string name;
            public bool pressed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            started = true;
            startProgram();
 
        }


        private void startProgram()
        {
            var ip = ipBox.Text;
            int port = Int32.Parse(portBox.Text);
            connectBtn.Content = "Even geduld...";
            bool setup = setupConnection(ip, port);

            if (setup)
            {
                String message = "HELLO|Live_Mobile|\r\n";
                writeTCP(message);
                String response = readTCP();
                readTCP();

                if (response.Trim() == "HELLO")
                {
                    connectBtn.Content = "Verbinding verbreken";
                    tabButtons.IsEnabled = true;
                    tabButtons.IsSelected = true;
                }




                writeHexTCP(this.btn_list_msg);

                String button_list_xml = readTCP().Replace("BUTTON_LIST|", "");
                XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(button_list_xml));
                reader.Read();

                int x = 0;
                int y = -1;
                int btnCount = 0;
                int txtBlockCount = 0;

                while (!reader.EOF) //load loop
                {
                    if (reader.IsStartElement() && reader.Name == "page")
                    {
                        y++;
                        String pageName = reader.GetAttribute("name");
                        Button btn = new Button();
                        //btn attributes
                        TextBlock tb = new TextBlock();
                        tb.Text = pageName + ":";
                        tb.Name = "TextBlock" + txtBlockCount.ToString();

                        Grid.SetColumn(tb, x);
                        Grid.SetRow(tb, y);
                        dataGridViewTabButtons.Children.Add(tb);
                        y++;
                        txtBlockCount++;

                    }
                    if (reader.IsStartElement() && reader.Name == "button")
                    {
                        //Get button information from QuickDMX, first tag item is the tag opening self (contains index and status)
                        int buttonIndex = Int32.Parse(reader.GetAttribute("index"));
                        bool buttonStatus = reader.GetAttribute("pressed") == "1";
                        reader.Read(); // go to value (between <button> and </button)
                        String buttonName = reader.Value; //read the value

                        //Create a new button for in the grid
                        Button btn = new Button();
                        btn.Click += scene_button_Click;
                        btn.Name = "button_" + btnCount.ToString(); //set name
                        btn.Content = buttonName; //set visible content
                        //add additional data
                        btn.Tag = new buttonData() { index = buttonIndex, name = buttonName, pressed = buttonStatus };
                        if (buttonStatus)
                        {
                            btn.Background = Brushes.DarkGray;
                        }
                        else
                        {
                            btn.Background = Brushes.LightGray;
                        }

                        Grid.SetColumn(btn, x);
                        Grid.SetRow(btn, y);
                        dataGridViewTabButtons.Children.Add(btn);
                        x++;
                        btnCount++;
                    }

                    reader.Read();

                }
                reader.Close();
            }else
            {
                started = false;
                connectBtn.Content = "Verbinden";
            }
        }

        private void closeProgram()
        {
            closeConnection();
        }

        private void scene_button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var data = (buttonData)btn.Tag;
            String message;
            if(data.pressed)
            {
                message = "BUTTON_RELEASE|" + data.index.ToString();
                btn.Background = Brushes.LightGray;
                btn.Tag = new buttonData() { index = data.index, name = data.name, pressed = false };
            }
            else
            {
                message = "BUTTON_PRESS|" + data.index.ToString();
                btn.Background = Brushes.DarkGray;
                btn.Tag = new buttonData() { index = data.index, name = data.name, pressed = true };
            }
            
            writeTCPWithExtra(message);
            
        }

        private Boolean setupConnection(String ip, int port)
        {
            this.client = new TcpClient(ip, port);
            this.stream = this.client.GetStream();
            //return this.client.Connected;*/
            return this.client.Connected == true ? true : false;
            //return true;
        }

        private Boolean connectionOpen()
        {
            return this.client.Connected == true ? true : false;
        }

        private void closeConnection()
        {
            this.stream.Close();
            this.client.Close();
        }

        private void writeTCP(String message)
        {
            if (this.connectionOpen())
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                
                this.stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent {0}", message);
            }else
            {
                Console.WriteLine("No connection");
            }
        }

        private void writeTCPWithExtra(String message)
        {
            if (this.connectionOpen())
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                Byte[] newArray = new byte[data.Length + 2];
                Byte[] extra = {0x0d, 0x0a};

                data.CopyTo(newArray, 0);
                extra.CopyTo(newArray, data.Length);
                this.stream.Write(newArray, 0, newArray.Length);
                Console.WriteLine("Sent {0}", message);
            }
            else
            {
                Console.WriteLine("No connection");
            }
        }

        private void writeHexTCP(Byte[] msg)
        {
            if (this.connectionOpen())
            {
                this.stream.Write(msg, 0, msg.Length);
                String message = Encoding.UTF8.GetString(msg, 0, msg.Length);
                Console.WriteLine("Sent '{0}'", message);
            }
            else
            {
                Console.WriteLine("No connection");
            }
        }

        private String readTCP()
        {
            if (this.connectionOpen())
            {
                Byte[] data = new Byte[2048];
                //String responseData = String.Empty;
                Int32 bytes = this.stream.Read(data, 0, data.Length);
                String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                if (responseData.Contains("ERROR"))
                {
                    closeConnection();
                    Console.WriteLine("Closed");

                }
                return responseData;
            }else
            {
                Console.WriteLine("No connection");
                return "";
            }
        }


        /*
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            String ip = ipBox.Text;
            int port = Int32.Parse(portBox.Text);
            String message = "HELLO|Live_Mobile|\r\n";
            send(ip, port, message);

            message = "BUTTON_LIST";
            //send(ip, port, message);


        }

        private String sendAndReceive(String ip, int port, String message)
        {
            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
            String responseData = String.Empty;

            Byte[] data = new Byte[256];


            data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent: {0}", message);


            data = new Byte[256];
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);




            stream.Close();
            client.Close();
            return responseData;
        }

        private void send(String ip, int port, String message)
        {
            sendAndReceive(ip, port, message);
        }
        */
    }
}
