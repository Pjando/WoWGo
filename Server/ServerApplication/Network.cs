using System;
using System.Net.Sockets;
using System.Net;

/// <summary>
/// Responsible for establishing connection with the clients.
/// </summary>
class Network {
    /// <summary>
    /// The socket for the server.
    /// </summary>
    public TcpListener serverSocket;
    /// <summary>
    /// Number of clients that can connect to the server.
    /// </summary>
    public static int noOfClients = 100;
    /// <summary>
    /// Global variable.
    /// </summary>
    public static Network instance = new Network();
    /// <summary>
    /// List of all clients.
    /// </summary>
    public static Client[] clients = new Client[noOfClients];

    /// <summary>
    /// Starts the server intialising the server socket.
    /// </summary>
    public void ServerStart() {
        for (int i = 0; i < noOfClients; i++) {
            clients[i] = new Client();
        }

        serverSocket = new TcpListener(IPAddress.Any, 5500);
        serverSocket.Start();
        serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
        
        Console.WriteLine("Server has successfully started.");
    }

    /// <summary>
    /// Callback method used when a client connects to the server.
    /// </summary>
    /// <param name="result">The result of an asynchronous operation.</param>
    void OnClientConnect(IAsyncResult result) {
        //Creates client socket
        TcpClient client = serverSocket.EndAcceptTcpClient(result);
        client.NoDelay = false;
        //Accept other clients.
        serverSocket.BeginAcceptTcpClient(OnClientConnect, null);

        //Set client variables
        for (int i = 0; i < noOfClients; i++) {
            if (clients[i].socket == null) {
                clients[i].socket = client;
                clients[i].index = i;
                clients[i].ip = client.Client.RemoteEndPoint.ToString();
                clients[i].Start();

                Console.WriteLine("Incoming connection from " + clients[i].ip + " || Index: " + i);
                //Send welcome message.
                ServerSendData.instance.SendMessage(i, "Welcome to the Game.");
                return;
            }
        }
    }
}

