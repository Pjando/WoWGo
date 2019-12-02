using System.Xml;
using UnityEngine;

/*! 
 *  \author    Sloan Kelly
 */

/// <summary>
/// Represents the boudaries of the map data.
/// </summary>
class OsmBounds : BaseOsm {
    /// <summary>
    /// The minimum latitude found in the map data.
    /// </summary>
    public float minLat { get; private set; }
    /// <summary>
    /// The maximum latitude found in the map data.
    /// </summary>
    public float maxLat { get; private set; }
    /// <summary>
    /// The minimum longitude found in the map data.
    /// </summary>
    public float minLon { get; private set; }
    /// <summary>
    /// The maximum longitude found in the map data.
    /// </summary>
    public float maxLon { get; private set; }
    /// <summary>
    /// The centre of the map data.
    /// </summary>
    public Vector3 centre { get; private set; }
    /// <summary>
    /// Sets the minimum and maximum longitude and latitude values and calculates the centre of the map.
    /// </summary>
    /// <param name="node"></param>
    public OsmBounds(XmlNode node) {
        minLat = GetAttribute<float>("minlat", node.Attributes);
        maxLat = GetAttribute<float>("maxlat", node.Attributes);
        minLon = GetAttribute<float>("minlon", node.Attributes);
        maxLon = GetAttribute<float>("maxlon", node.Attributes);

        float x = (float) ((MercatorProjection.lonToX(maxLon) + MercatorProjection.lonToX(minLon)) / 2);
        float y = (float) ((MercatorProjection.latToY(maxLat) + MercatorProjection.latToY(minLat)) / 2);

        centre = new Vector3(x, 0, y);
    }
}
