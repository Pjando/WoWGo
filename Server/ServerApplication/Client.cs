using System;
using System.Net.Sockets;

/// <summary>
/// Represents a client connected to the server.
/// </summary>
class Client {
    /// <summary>
    /// The username the client entered.
    /// </summary>
    internal string username;
    /// <summary>
    /// The index of the client in the list of clients.
    /// </summary>
    internal int index;
    /// <summary>
    /// IP address of the client.
    /// </summary>
    internal string ip;
    /// <summary>
    /// The socket that connects to the client.
    /// </summary>
    internal TcpClient socket;
    /// <summary>
    /// The stream used to read and write data between the server and client.
    /// </summary>
    internal NetworkStream myStream;
    /// <summary>
    /// Used to store incoming data from the client.
    /// </summary>
    private byte[] readBuff;
    /// <summary>
    /// Latitude of the client.
    /// </summary>
    internal float latitude;
    /// <summary>
    /// Longitude of the client.
    /// </summary>
    internal float longitude;
    
    /// <summary>
    /// Begins the callback method used to listen for incoming data from the client.
    /// </summary>
    public void Start() {
        socket.SendBufferSize = 4096;
        socket.ReceiveBufferSize = 4096;
        myStream = socket.GetStream();
        Array.Resize(ref readBuff, socket.ReceiveBufferSize);
        myStream.BeginRead(readBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);
    }

    /// <summary>
    /// Closes the connection between server and client.
    /// </summary>
    void CloseConnection() {
        socket.Close();
        socket = null;
        Console.WriteLine("Player disconnected " + ip);
    }

    /// <summary>
    /// Receives data from client and offloads data to ServerHandleData instance.
    /// </summary>
    /// <param name="result">The result of the asynchronous operation.</param>
    void OnReceiveData(IAsyncResult result) {
        try {
            int readBytes = myStream.EndRead(result);
            if (socket == null) {
                return;
            }
            if (readBytes <= 0) {
                CloseConnection();
                return;
            }

            byte[] newBytes = null;
            Array.Resize(ref newBytes, readBytes);
            Buffer.BlockCopy(readBuff, 0, newBytes, 0, readBytes);

            ServerHandleData.instance.HandleData(index, newBytes);

            if (socket == null) {
                return;
            }

            myStream.BeginRead(readBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);

        } catch (Exception e) {
            CloseConnection();
            return;
        }
    }
}

