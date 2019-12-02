using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Manages which enemy prefabs to spawn.
/// </summary>
public class EnemyManager : MonoBehaviour {
    
    /// <summary>
    /// Global Singleton instance of class.
    /// </summary>
    public static EnemyManager instance;

    /// <summary>
    /// Wizard prefab to be spawned.
    /// </summary>
    public GameObject wizard;
    
    /// <summary>
    /// Skeleton prefab to be spawned.
    /// </summary>
    public GameObject skeleton;

    /// <summary>
    /// The parent Game object of the prefabs.
    /// </summary>
    public GameObject enemyParent;

    /// <summary>
    /// The enemy name that has be tapped to enter the battle scene.
    /// </summary>
    public string currentEnemy;

    /// <summary>
    /// MapReader component.
    /// </summary>
    MapReader map;

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
    /// Sets the methods to be executed when events are triggered in other classes.
    /// </summary>
    void Start() {
        ClientHandleData.OnNewEnemies += Spawn;
        CameraController.Trigger += BattleScene;
        map = GameObject.Find("Map").GetComponent<MapReader>();
    }

    /// <summary>
    /// Method to be executed when "Trigger" event in CameraController is triggered.
    /// </summary>
    /// <param name="name">String name of the current enemy.</param>
    private void BattleScene(string name) {
        currentEnemy = name;
        StartCoroutine(LoadAsynchronously(name));
    }

    /// <summary>
    /// Loads the battle scene.
    /// </summary>
    /// <param name="name">String name of the current enemy.</param>
    /// <returns>Returns null whilst waiting for scene to load.</returns>
    private IEnumerator LoadAsynchronously(string name) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }
    
    /// <summary>
    /// Spawns new enemies when they are sent from the server.
    /// </summary>
    /// <param name="enemies">List of type Enemy of all the enemies to be spawned.</param>
    void Spawn(List<Enemy> enemies) {
        //Only execute in the Map scene.
        if (SceneManager.GetActiveScene().name == "Map Reader Test") {

            //Destroy any current spawned enemies
            for (int i = enemyParent.transform.childCount; i > 0; i--) {
                Destroy(enemyParent.transform.GetChild(i - 1).gameObject);
            }

            //Instantiate each new enemy.
            foreach (Enemy e in enemies) {
                float x = (float) MercatorProjection.lonToX(e.lon);
                float y = (float) MercatorProjection.latToY(e.lat);
                Vector3 position = new Vector3(x, 0, y);
                position = position - map.bounds.centre;

                if (e.name == "wizard") {
                    var newWizard = Instantiate(wizard, position, transform.rotation);
                    newWizard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    newWizard.transform.SetParent(enemyParent.transform);
                } else if (e.name == "skeleton") {
                    var newSkeleton = Instantiate(skeleton, position, transform.rotation);
                    newSkeleton.transform.SetParent(enemyParent.transform);
                }
            }
        }
    }
}