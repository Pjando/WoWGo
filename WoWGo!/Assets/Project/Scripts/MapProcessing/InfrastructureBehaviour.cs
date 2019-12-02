using UnityEngine;

/*! 
 *  \author    Sloan Kelly
 */
/// <summary>
/// Base class for rendering roads and buildings.
/// </summary>
[RequireComponent(typeof(MapReader))]
abstract class InfrastructureBehaviour : MonoBehaviour {
    /// <summary>
    /// Reference to MapReader component.
    /// </summary>
    protected MapReader map;
    /// <summary>
    /// Initialise the map variable.
    /// </summary>
    void Awake() {
        map = GetComponent<MapReader>();
    }

    /// <summary>
    /// Get the local centre of a way.
    /// </summary>
    /// <param name="way">OsmWay way to find centre of.</param>
    /// <returns>Vector3 of the centre of the way.</returns>
    protected Vector3 GetCentre(OsmWay way) {
        Vector3 total = Vector3.zero;

        foreach (var id in way.nodeIDs) {
            total += map.nodes[id];
        }

        return total / way.nodeIDs.Count;
    }

}