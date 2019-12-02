using UnityEngine;
using TMPro;

/*! 
 *  \author    Kevin Kaymak
 *  \author    Modified By Pavan Jando
 */

/// <summary>
/// Used to send data from the client to the server.
/// </summary>
public class ClientSendData : MonoBehaviour {

    /// <summary>
    /// Global Singleton instance of class.
    /// </summary>
    public static ClientSendData instance;

    /// <summary>
    /// Input field for user to enter username.
    /// </summary>
    [Header("Login")]
    public TMP_InputField username;

    /// <summary>
    /// Initialises Singleton instance.
    /// </summary>
    void Awake() {
        instance = this;
    }

    /// <summary>
    /// Base method for sending data to server. Sends a byte array data to server.
    /// </summary>
    /// <param name="data">Data to be sent to the server.</param>
    private void SendDataToServer(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        Network.myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
        buffer = null;
    }

    /// <summary>
    /// Sends GPS location to the server.
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lon"></param>
    public void SendGPS(float lat, float lon) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger(5);
        buffer.WriteFloat(lat);
        buffer.WriteFloat(lon);
        SendDataToServer(buffer.ToArray());
        buffer = null;
    }

    /// <summary>
    /// Send username to the server.
    /// </summary>
    /// <returns>Returns true if username was sent, false if username is empty.</returns>
    public bool SendLogin() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger(2);

        if (username.text == string.Empty) {
            return false;
        }

        buffer.WriteString(username.text);

        SendDataToServer(buffer.ToArray());
        buffer = null;
        return true;
    }
}
