using System.Collections.Generic;
using System.Xml;
using UnityEngine;

/*! 
 *  \author    Sloan Kelly
 *  \author    Modified by Pavan Jando
 */
/// <summary>
/// Used to read the map data from the xml map data.
/// </summary>
public class MapReader : MonoBehaviour {
    /// <summary>
    /// Dictionary mapping ids to OsmNodes.
    /// </summary>
    [HideInInspector]
    internal Dictionary<ulong, OsmNode> nodes;
    /// <summary>
    /// The boundaries of the map data.
    /// </summary>
    [HideInInspector]
    internal OsmBounds bounds;
    /// <summary>
    /// List of all the ways from file.
    /// </summary>
    [HideInInspector]
    internal List<OsmWay> ways;
    /// <summary>
    /// Name of the file to be processed.
    /// </summary>
    [Tooltip("This resource file that contains the OSM map data")]
    public string resourceFile;
    /// <summary>
    /// Signifies whether all the data has been read.
    /// </summary>
    public bool IsReady { get; private set; }

    /// <summary>
    /// Loads the resource file and finds the finds the boundaries,
    /// the nodes and ways in the file.
    /// </summary>
    void Awake() {

        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();

        var txtAsset = Resources.Load<TextAsset>(resourceFile);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        GetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));

        IsReady = true;
    }
    /// <summary>
    /// Creates and adds ways to the list from the resource file.
    /// </summary>
    /// <param name="xmlNodeList">Identifies nodes marked "way" to process from the file</param>
    private void GetWays(XmlNodeList xmlNodeList) {
        foreach (XmlNode node in xmlNodeList) {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
    }
    /// <summary>
    /// Creates and adds nodes to the dictionary from the resource file.
    /// </summary>
    /// <param name="xmlNodeList">Identifies nodes marked "node" to process from the file</param>
    private void GetNodes(XmlNodeList xmlNodeList) {
        foreach (XmlNode n in xmlNodeList) {
            OsmNode node = new OsmNode(n);
            nodes[node.id] = node;
        }
    }
    /// <summary>
    /// Creates the bounds of the map data.
    /// </summary>
    /// <param name="xmlNode">Identifies nodes marked "bounds" to process from the file</param>
    private void SetBounds(XmlNode xmlNode) {
        bounds = new OsmBounds(xmlNode);
    }
}
