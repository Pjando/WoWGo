using UnityEngine;

/*! 
 *  \author    Pavan Jando
 */

/// <summary>
/// Controls the models location on the map.
/// </summary>
public class ModelController : MonoBehaviour {

    /// <summary>
    /// x and y based on GPS coordinates.
    /// </summary>
    private float x, y;

    /// <summary>
    /// Current velocity of the model.
    /// </summary>
    private float v;

    /// <summary>
    /// The amount the models velocity increases by.
    /// </summary>
    private float time = 0f;

    /// <summary>
    /// The new position of the model.
    /// </summary>
    private Vector3 newPos;

    /// <summary>
    /// MapReader component.
    /// </summary>
    MapReader map;

    /// <summary>
    /// Animator for the player model.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Sets the model to the clients GPS location.
    /// </summary>
    private void Start() {

        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        anim.SetBool("Grounded", true);

        map = GameObject.Find("Map").GetComponent<MapReader>();

        x = (float) MercatorProjection.lonToX(Gps.instance.longitude);
        y = (float) MercatorProjection.latToY(Gps.instance.latitude);

        newPos = new Vector3(x, 0f, y);
        transform.position = newPos - map.bounds.centre;
    }

    /// <summary>
    /// Updates the models position using GPS coordinates.
    /// Also creates smooth walking animation between the old model
    /// position and new model position.
    /// </summary>
    void Update() {
        x = (float) MercatorProjection.lonToX(Gps.instance.longitude);
        y = (float) MercatorProjection.latToY(Gps.instance.latitude);

        newPos = new Vector3(x, 0f, y);
        newPos = newPos - map.bounds.centre;
        
        float t = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(1 / 25f, 1f));
        //Linearly adjusts rotation and position.
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newPos), 0.15f);
        transform.position = Vector3.Lerp(transform.position, newPos, t);

        float distance = Vector3.Distance(transform.position, newPos);

        if (distance < 2) {
            anim.SetFloat("MoveSpeed", Mathf.SmoothStep(v, 0f, time));
        } else {
            v = Mathf.SmoothStep(0f, 1f, time);
            anim.SetFloat("MoveSpeed", v);
        }

        time += 0.5f * Time.deltaTime;

        if (time > 1.0f) {
            time = 1.0f;
        }
    }
}
