using System;
using UnityEngine;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Used to control the camera position in the map scene.
/// </summary>
public class CameraController : MonoBehaviour {

    /// <summary>
    /// The game object of the model.
    /// </summary>
    public GameObject model;

    /// <summary>
    /// The speed at which the camera rotates.
    /// </summary>
    private const float SPEED = 1.5f;

    /// <summary>
    /// The offset vector between the camera and the model.
    /// </summary>
    private Vector3 offset;

    /// <summary>
    /// The angle the camera is to be rotated.
    /// </summary>
    private Quaternion rotation;

    /// <summary>
    /// Event that is triggered when the user taps on an enemy model.
    /// </summary>
    public static event Action<string> Trigger;

    /// <summary>
    /// Calculates the offset between the camera and the model.
    /// </summary>
    private void Awake() {
        offset = transform.position - model.transform.position;
        rotation = transform.rotation;
    }

    /// <summary>
    /// Rotates and moves the camera when the user swipes on screen.
    /// Detects if an enemy has been tapped to initiate battle.
    /// </summary>
    private void LateUpdate() {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved) {
            Quaternion angle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * SPEED, Vector3.up);
            offset = angle * offset;
            rotation = angle * rotation;
        }
        #if UNITY_EDITOR
        if (Input.GetMouseButton(0)) {
            Quaternion angle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * SPEED, Vector3.up);
            offset = angle * offset;
            rotation = angle * rotation;
        }
        #endif

        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Ray raycast = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit)) {
                if (raycastHit.collider.name == "wizard(Clone)" || raycastHit.collider.name == "skeleton(Clone)") {
                    Trigger?.Invoke(raycastHit.collider.name);
                }
            }
        }

        transform.rotation = rotation;
        transform.position = model.transform.position + offset;
    }
}