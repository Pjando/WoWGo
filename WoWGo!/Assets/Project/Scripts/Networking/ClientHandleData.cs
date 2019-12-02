using System;
using System.Collections.Generic;
using UnityEngine;

/*! 
 *  \author    Kevin Kaymak
 *  \author    Modified By: Pavan Jando
 */

/// <summary>
/// Handles incoming data from the server.
/// </summary>
public class ClientHandleData : MonoBehaviour {
    
    /// <summary>
    /// Event that is triggered when new enemies are received from the server.
    /// </summary>
    public static event Action<List<Enemy>> OnNewEnemies;

    /// <summary>
    /// Global Singleton instance of class.
    /// </summary>
    public static ClientHandleData instance;
    /// <summary>
    /// List of type Enemy that is received from the server.
    /// </summary>
    public List<Enemy> enemies;

    /// <summary>
    /// Initialises Singleton instance.
    /// </summary>
    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Decides which method to execute based on the packetNum value.
    /// </summary>
    /// <param name="packetNum">Number used to identify the data.</param>
    /// <param name="data">Byte array sent from the server.</param>
    void HandleMessages(int packetNum, byte[] data) {
        switch (packetNum) {
            case 2:
                HandleEnemies(packetNum, data);
                break;
            case 3:
                HandleMessage(packetNum, data);
                break;
        }
    }

    /// <summary>
    /// Extracts the identifier for the data from the byte array
    /// and calls the HandleMessages() to call the right method.
    /// </summary>
    /// <param name="data">Byte array sent from the server.</param>
    public void HandleData(byte[] data) {
        int packetNum;
        ByteBuffer buffer = new ByteBuffer();

        buffer.WriteBytes(data);
        packetNum = buffer.ReadInteger();

        if (packetNum == 0) { return; }

        HandleMessages(packetNum, buffer.ToArray());
        buffer = null;
    }

    /// <summary>
    /// Deserialises the data using Deserialise() and invokes OnNewEnemies event.
    /// </summary>
    /// <param name="packetNum">Number used to identify the data.</param>
    /// <param name="data">Byte array sent from the server.<</param>
    private void HandleEnemies(int packetNum, byte[] data) {
        Desserialise(data);
        OnNewEnemies?.Invoke(enemies);
    }

    /// <summary>
    /// Writes data to a buffer and reads a string message from the server.
    /// </summary>
    /// <param name="packetNum">Number used to identify the data.</param>
    /// <param name="data">Byte array sent from the server.<</param>
    public void HandleMessage(int packetNum, byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetnum = buffer.ReadInteger();
        string message = buffer.ReadString();
        Debug.Log(message);
    }

    /// <summary>
    /// Deserialises the byte array and converts the data into enemy object.
    /// </summary>
    /// <param name="data">Byte array sent from the server.</param>
    private void Desserialise(byte[] data) {
        enemies = new List<Enemy>();
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetNum = buffer.ReadInteger();

        //Identifies the enemy to be created.
        int count = buffer.ReadInteger();

        for (int i = 0; i < count; i++) {
            int id = buffer.ReadInteger();
            switch (id) {
                case 1:
                    Wizard wiz = new Wizard();
                    wiz.id = id;
                    wiz.name = buffer.ReadString();
                    wiz.lat = buffer.ReadFloat();
                    wiz.lon = buffer.ReadFloat();
                    enemies.Add(wiz);
                    break;
                case 2:
                    Skeleton skel = new Skeleton();
                    skel.id = id;
                    skel.name = buffer.ReadString();
                    skel.lat = buffer.ReadFloat();
                    skel.lon = buffer.ReadFloat();
                    enemies.Add(skel);
                    break;
            }
        }
    }
}
