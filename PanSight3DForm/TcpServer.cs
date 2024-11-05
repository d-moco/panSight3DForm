using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PanSight3DForm
{
   
    public class TcpServer
    {
        public event EventHandler<string> CommandReceived;  // 事件，当收到命令时触发
        public event EventHandler ClientConnected;         // 事件，当有客户端连接时触发
        public event EventHandler ClientDisconnected;      // 事件，当客户端断开连接时触发

        private TcpListener tcpListener;
        private Thread listenThread;

        public TcpServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }


        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                OnClientConnected(EventArgs.Empty);  // 触发客户端连接事件

                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    // 发生错误，可能是连接中断
                    break;
                }

                if (bytesRead == 0)
                {
                    // 客户端已断开连接
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                string command = encoder.GetString(message, 0, bytesRead).Trim();
                OnCommandReceived(command);  // 触发命令接收事件
            }

            tcpClient.Close();
            OnClientDisconnected(EventArgs.Empty);  // 触发客户端断开连接事件
        }

        protected virtual void OnCommandReceived(string command)
        {
            CommandReceived?.Invoke(this, command);
        }

        protected virtual void OnClientConnected(EventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }

        protected virtual void OnClientDisconnected(EventArgs e)
        {
            ClientDisconnected?.Invoke(this, e);
        }
    }
}
