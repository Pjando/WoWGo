using System.Xml;
using UnityEngine;

/*! 
 *  \author    Sloan Kelly
 */
/// <summary>
/// Represents a point on the map.
/// </summary>
class OsmNode : BaseOsm {
    /// <summary>
    /// Identifier for the node.
    /// </summary>
    public ulong id { get; private set; }
    /// <summary>
    /// Latitude of the node.
    /// </summary>
    public float latitude { get; private set; }
    /// <summary>
    /// Longitude of the node.
    /// </summary>
    public float longitude { get; private set; }
    /// <summary>
    /// The x value corresponding to the longitude.
    /// </summary>
    public float x { get; private set; }
    /// <summary>
    /// The y value corresponding to the latitude.
    /// </summary>
    public float y { get; private set; }
    /// <summary>
    /// Allows OsmNode objects to be compared and used in calculation
    /// with Vector3 objects by returning a new Vector3 with x and y values.
    /// </summary>
    /// <param name="node">The OsmNode from which to create a Vector3 object.</param>
    public static implicit operator Vector3(OsmNode node) {
        return new Vector3(node.x, 0, node.y);
    }
    /// <summary>
    /// Creates an OsmNode with id, latitude and longitude from the given node.
    /// </summary>
    /// <param name="node">The from which the values are extracted.</param>
    public OsmNode(XmlNode node) {
        id = GetAttribute<ulong>("id", node.Attributes);
        latitude = GetAttribute<float>("lat", node.Attributes);
        longitude = GetAttribute<float>("lon", node.Attributes);

        x = (float) MercatorProjection.lonToX(longitude);
        y = (float) MercatorProjection.latToY(latitude);
    }
}
