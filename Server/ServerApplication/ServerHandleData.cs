using System.Collections.Generic;

/// <summary>
/// Handles incoming data from various clients.
/// </summary>
class ServerHandleData {

    /// <summary>
    /// Global variable.
    /// </summary>
    public static ServerHandleData instance = new ServerHandleData();
    /// <summary>
    /// Delegate that defines the base method for handling data.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="data">Data retrieved from the client.</param>
    private delegate void Packet_(int index, byte[] data);
    /// <summary>
    /// Dictionary mapping integer identifiers for each data handling method.
    /// </summary>
    private Dictionary<int, Packet_> packets;

    /// <summary>
    /// Adds the data handling methods with each a unique integer identifier.
    /// </summary>
    public void InitMessages() {
        packets = new Dictionary<int, Packet_>();
        packets.Add(2, HandleLogin);
        packets.Add(5, HandleGPS);
    }

    /// <summary>
    /// Base data handling method, offloads to other methods based of the integer indentifier.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="data">Data retrieved from the client.</param>
    public void HandleData(int index, byte[] data) {
        int packetnum;
        Packet_ packet;
        ByteBuffer buffer = new ByteBuffer();

        buffer.WriteBytes(data);
        //Identifies which method to invoke
        packetnum = buffer.ReadInteger();
        buffer = null;
        if (packetnum == 0)
            return;
        //If method identifier is in dictionary execute that method
        if (packets.TryGetValue(packetnum, out packet)) {
            packet.Invoke(index, data);
        }
    }

    /// <summary>
    /// Handles the GPS coordinates retrieved from the server.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="data">Data retrieved from the client.</param>
    void HandleGPS(int index, byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packet = buffer.ReadInteger();
        Network.clients[index].latitude = buffer.ReadFloat();
        Network.clients[index].longitude = buffer.ReadFloat();

        List<Enemy> e = new List<Enemy>();
        //Temporary so that enemies spawn near the client.
        Wizard wizard = new Wizard(Network.clients[index].latitude + 0.0001f, Network.clients[index].longitude + 0.0001f);
        Skeleton skeleton = new Skeleton(Network.clients[index].latitude - 0.0001f, Network.clients[index].longitude - 0.0001f);
        e.Add(wizard);
        e.Add(skeleton);

        GameLogic.instance.SetEnemies(e);
    }

    /// <summary>
    /// Handles the username submitted by the user from the client.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="data">Data retrieved from the client.</param>
    void HandleLogin(int index, byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packet = buffer.ReadInteger();
        string username = buffer.ReadString();
        Network.clients[index].username = username;
    }
}

