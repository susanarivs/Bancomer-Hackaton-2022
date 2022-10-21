using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace WebSocket
{
     class Program
    {
        static void Main(string[] args)
        {
            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket conexion;
            IPEndPoint connect = new IPEndPoint(IPAddress.Parse("127.0.0.1"),6400);
            listen.Bind(connect);
            listen.Listen(10);
            conexion = listen.Accept();
            Console.WriteLine("Conexion aceptada:");
            byte[] recibir_info = new byte[100];
            String data = "";
            int array_size = 0;
            array_size=conexion.Receive(recibir_info, 0,recibir_info.Length,0);
            Array.Resize(ref recibir_info, array_size);
            data=Encoding.Default.GetString(recibir_info);
            Console.WriteLine("La informacion recibida es{0}",data);
            Console.ReadKey();
        }
    }
}
