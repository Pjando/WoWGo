using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

/*! 
 *  \author    Kevin Kaymak
 *  \author    Modified By: Pavan Jando
 */

/// <summary>
/// Main class used to establish connection with the server.
/// </summary>
public class Network : MonoBehaviour {

    /// <summary>
    /// Global Singleton instance of class.
    /// </summary>
    public static Network instance;
    
    /// <summary>
    /// IP of the server.
    /// </summary>
    public string serverIP = "127.0.0.1";
    /// <summary>
    /// Port to connect to on the server.
    /// </summary>
    public int serverPort = 5500;
    /// <summary>
    /// Whether the client has connected to the server.
    /// </summary>
    public bool isConnected;

    /// <summary>
    /// The client socket
    /// </summary>
    public TcpClient playerSocket;
    /// <summary>
    /// Stream used to transmit data read and write data.
    /// </summary>
    public static NetworkStream myStream;
    /// <summary>
    /// Used to read data from the stream.
    /// </summary>
    public StreamReader myReader;
    /// <summary>
    /// Used to write data to the stream.
    /// </summary>
    public StreamWriter myWriter;

    /// <summary>
    /// Stores data read from the server.
    /// </summary>
    private byte[] asyncBuff;
    /// <summary>
    /// Whether there is data to be handled.
    /// </summary>
    public bool shouldHandleData;
    /// <summary>
    /// Copy of the async buff to prevent data being overwritten.
    /// </summary>
    private byte[] myBytes;

    /// <summary>
    /// Initialises Singleton instance.
    /// </summary>
    private void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Starts the connection to the server.
    /// </summary>
    void Start() {
        ConnectGameServer();
    }

    /// <summary>
    /// Establishes a connection with the server and initiates callback methods.
    /// </summary>
    void ConnectGameServer() {
        //Check for disconnect.
        if (playerSocket != null) {

            if (playerSocket.Connected || isConnected) {
                return;
            }
            playerSocket.Close();
            playerSocket = null;
        }

        playerSocket = new TcpClient();
        playerSocket.ReceiveBufferSize = 4096;
        playerSocket.SendBufferSize = 4096;
        playerSocket.NoDelay = false;
        Array.Resize(ref asyncBuff, 8192);
        playerSocket.BeginConnect(serverIP, serverPort, new AsyncCallback(ConnectCallback), playerSocket);
        isConnected = true;
    }

    /// <summary>
    /// Callback method used when connected to the server.
    /// </summary>
    /// <param name="result">The result of the asynchronous operation.</param>
    void ConnectCallback(IAsyncResult result) {
        //Check for disconnect.
        if (playerSocket != null) {
            playerSocket.EndConnect(result);

            if (playerSocket.Connected == false) {
                isConnected = false;
                return;
            } else {
                playerSocket.NoDelay = true;
                myStream = playerSocket.GetStream();
                myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
            }
        }
    }
    
    /// <summary>
    /// Offloads data to ClientHandleData instance if shouldHandleData = true.
    /// </summary>
    private void Update() {
        if (shouldHandleData) {
            shouldHandleData = false;
            ClientHandleData.instance.HandleData(myBytes);
        }
    }

    /// <summary>
    /// Callback used when the server sends data to the client.
    /// </summary>
    /// <param name="result">The result of the asynchronous operation.</param>
    void OnReceive(IAsyncResult result) {
        //Check for disconnect.
        if (playerSocket != null) {
            if (playerSocket == null) {
                return;
            }

            int byteArray = myStream.EndRead(result);
            myBytes = null;
            Array.Resize(ref myBytes, byteArray);
            Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteArray);

            if (byteArray == 0) {
                Debug.Log("You disconnected from the server.");
                playerSocket.Close();
                return;
            }

            shouldHandleData = true;

            if (playerSocket == null) {
                return;
            }
            myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
        }
    }
}
