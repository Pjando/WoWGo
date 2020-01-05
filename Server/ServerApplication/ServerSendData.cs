using System.Collections.Generic;

/// <summary>
/// Used to send data to clients.
/// </summary>
class ServerSendData {

    /// <summary>
    /// Global variable.
    /// </summary>
    public static ServerSendData instance = new ServerSendData();

    /// <summary>
    /// Base class for sending byte array data to a client.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="data">Data retrieved from the client.</param>
    public void SendDataTo(int index, byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        Network.clients[index].myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
        buffer = null;
    }

    /// <summary>
    /// Sends the current list of enemies to the specified client.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="enemies">Data retrieved from the client.</param>
    public void SendEnemies(int index, List<Enemy> enemies) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger(2);
        buffer.WriteInteger(enemies.Count);
        foreach(Enemy temp in enemies) {
            buffer.WriteBytes(temp.Serialise());
        }

        SendDataTo(index, buffer.ToArray());
        buffer = null;
    }

    /// <summary>
    /// Sends a string message to the specified client.
    /// </summary>
    /// <param name="index">Integer index of the client from the list of clients.</param>
    /// <param name="message">Data retrieved from the client.</param>
    public void SendMessage(int index, string message) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger(3);
        buffer.WriteString(message);

        SendDataTo(index, buffer.ToArray());
        buffer = null;
    }
}

