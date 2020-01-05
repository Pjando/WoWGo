using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Outlines the basic logic of the game, mainly enemy spawning.
/// </summary>
class GameLogic {

    /// <summary>
    /// Global variable.
    /// </summary>
    public static GameLogic instance = new GameLogic();
    
    /// <summary>
    /// List of enemies that are currently spawned.
    /// </summary>
    List<Enemy> enemies = new List<Enemy>();

    /// <summary>
    /// Ten second timer.
    /// </summary>
    public Timer timer10seconds;

    /// <summary>
    /// Starts the timer and executes TenSecondFunction every ten seconds.
    /// </summary>
    public void ServerLoop() {       
        timer10seconds = new Timer(TenSecondFunction, null, 0, 10000);
    }

    /// <summary>
    /// Sends the current list of enemies to all connected clients.
    /// </summary>
    /// <param name="o"></param>
    private void TenSecondFunction(Object o) {
        for (int i = 0; i < Network.noOfClients; i++) {
            if (Network.clients[i].socket != null) {
                ServerSendData.instance.SendEnemies(i, enemies);
            }
        }
    }

    /// <summary>
    /// Sets the current list of enemies
    /// </summary>
    /// <param name="e">List of enemies to append.</param>
    public void SetEnemies(List<Enemy> e) {
        enemies.AddRange(e);
    }
}

