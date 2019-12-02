using System.Collections.Generic;
using System.Xml;

/*! 
 *  \author    Sloan Kelly
 */
/// <summary>
/// Represents a way which is either a road or building.
/// </summary>
class OsmWay : BaseOsm {
    /// <summary>
    /// Identifier for the way.
    /// </summary>
    public ulong id { get; private set; }
    /// <summary>
    /// Whether the way should be visible/rendered.
    /// </summary>
    public bool visible { get; private set; }
    /// <summary>
    /// List of ids for each point in a way.
    /// </summary>
    public List<ulong> nodeIDs { get; private set; }
    /// <summary>
    /// Whether the way is a boundary.
    /// </summary>
    public bool isBoundary { get; private set; }
    /// <summary>
    /// Whether the way is a building.
    /// </summary>
    public bool isBuilding { get; private set; }
    /// <summary>
    /// The height of the way if it is a building.
    /// </summary>
    public float height { get; private set; }
    /// <summary>
    /// Whether the way is a road.
    /// </summary>
    public bool isRoad { get; private set; }
    /// <summary>
    /// Creates a way and identifies its attributes.
    /// </summary>
    /// <param name="node">The node from which the values are extracted.</param>
    public OsmWay(XmlNode node) {
        nodeIDs = new List<ulong>();
        //Default building height
        height = 3.0f;

        id = GetAttribute<ulong>("id", node.Attributes);
        visible = true;
        //All of the points in a way.
        XmlNodeList nds = node.SelectNodes("nd");
        //For each point get the id and add to the list.
        foreach (XmlNode n in nds) {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            nodeIDs.Add(refNo);
        }
        //If first node equals last node, must be a boundary.
        if (nodeIDs.Count > 1) {
            isBoundary = nodeIDs[0] == nodeIDs[nodeIDs.Count - 1];
        }

        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags) {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "building:levels") {
                height = 3.0f * GetAttribute<float>("v", t.Attributes);
            } else if (key == "height") {
                height = 0.3048f * GetAttribute<float>("v", t.Attributes);
            } else if (key == "building") {
                isBuilding = GetAttribute<string>("v", t.Attributes) == "yes";
            } else if (key == "highway") {
                isRoad = true;
            }
        }
    }
}