using System.Collections;
using UnityEngine;

/*! 
 *  \author    N3K EN
 *  \author    Modified By Pavan Jando
 */

/// <summary>
/// Gets the GPS longitude and latitude of the client.
/// </summary>
public class Gps : MonoBehaviour {

    /// <summary>
    /// Global Singleton instance of class.
    /// </summary>
    public static Gps instance { set; get; }

    /// <summary>
    /// Latitude of the client.
    /// </summary>
    public float latitude { private set; get; }
    /// <summary>
    /// Longitude of the client.
    /// </summary>
    public float longitude { private set; get; }

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
    /// Starts the StartLocationService() coroutine.
    /// </summary>
    private void Start() {
        StartCoroutine(StartLocationService());
    }

    /// <summary>
    /// Starts the location service if the user has authorised it.
    /// </summary>
    /// <returns>Breaks if error occurred in starting location service,
    /// otherwise starts UpdateGPS() coroutine.</returns>
    private IEnumerator StartLocationService() {
        //Check for user enabling GPS
        if (!Input.location.isEnabledByUser) {
            Debug.Log("User not enabled GPS");
            latitude = 51.4293344263613f;
            longitude = -0.570242821658077f;
            yield break;
        }
        //Start location service
        Input.location.Start();

        int maxWait = 20;
        //Wait for service to initialise
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        //If 20 seconds have passed time out
        if (maxWait <= 0) {
            Debug.Log("Timed Out!");
            yield break;
        }
        //If failed break
        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        //Get lat and lon
        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;

        StartCoroutine(UpdateGps());
    }

    /// <summary>
    /// Updates every second and gets the GPS coordinates of the client.
    /// </summary>
    /// <returns>Returns every second</returns>
    private IEnumerator UpdateGps() {
        WaitForSeconds updateTime = new WaitForSeconds(1f);

        while (true) {
            longitude = Input.location.lastData.longitude;
            latitude = Input.location.lastData.latitude;

            yield return updateTime;
        }
    }

    /// <summary>
    /// When the behaviour is disabled stop location service.
    /// </summary>
    private void OnDisable() {
        Input.location.Stop();
        StopCoroutine(UpdateGps());
    }
}