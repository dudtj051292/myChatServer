using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace myChatServer
{
    class ChatServer
    {
        Socket socket = null;

        public ChatServer()
        {
            AsyncServerStart();
        }

        private void AsyncServerStart()
        {
            Console.WriteLine("Server Activate..");
            TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 9999));
            listener.Start();

            while (true)
            {
                socket = listener.AcceptSocket();
                ClientHandler cHandler = new ClientHandler(socket);

                Thread t = new Thread(cHandler.chat);
                t.Start();


            }
        }
        
    }
    
}
