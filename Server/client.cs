using System;
using System.Net;
using System.Net.Sockets;

class Client {

    static void Main(String[] args) {
        int port = 11000;
        string ip = "127.0.0.1";
        Socket client = new Socket(AddressFamily.InterNetwork, 
            SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
        client.Connect(ep);
        Console.WriteLine("Client is connected");

        string clientMessage = "Hello World!";
        byte[] message = System.Text.Encoding.ASCII.GetBytes(clientMessage);
        
        client.Send(message, 0, clientMessage.Length, SocketFlags.None);

        int size = -1;
        byte[] serverMessage = new byte[1024];
        while((size = client.Receive(serverMessage)) != 0) {
            Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(serverMessage, 0, size));
        }
    }
}