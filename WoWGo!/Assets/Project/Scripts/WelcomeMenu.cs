using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Controls the user interface in the main menu.
/// </summary>
public class WelcomeMenu : MonoBehaviour {

    /// <summary>
    /// The loading menu game object.
    /// </summary>
    public GameObject loadingMenu;

    /// <summary>
    /// The home menu game object.
    /// </summary>
    public GameObject homeMenu;

    /// <summary>
    /// The error panel game object.
    /// </summary>
    public GameObject errorPanel;

    /// <summary>
    /// Slider for loading screen.
    /// </summary>
    public Slider slider;
    
    /// <summary>
    /// Shows the home menu.
    /// </summary>
    private void Awake() {
        homeMenu.SetActive(true);
    }

    /// <summary>
    /// Closes the error panel.
    /// </summary>
    public void CloseErrorPanel() {
        errorPanel.SetActive(false);
    }

    /// <summary>
    /// Sends the username to the server and loads the map scene and loading menu.
    /// Error panel shows if no username was entered.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void PlayGame(int sceneIndex) {
        if (ClientSendData.instance.SendLogin()) {
            ClientSendData.instance.SendGPS(Gps.instance.latitude, Gps.instance.longitude);

            homeMenu.SetActive(false);
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
        errorPanel.SetActive(true);
    }

    /// <summary>
    /// Loads the map scene and updates loading bar.
    /// </summary>
    /// <param name="sceneIndex">Integer index of the scene to be loaded.</param>
    /// <returns></returns>
    IEnumerator LoadAsynchronously(int sceneIndex) {
        loadingMenu.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
