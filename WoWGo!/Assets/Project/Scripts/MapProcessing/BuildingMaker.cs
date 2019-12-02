using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! 
 *  \author    Sloan Kelly
 *  \author    Modified by Pavan Jando
 */

/// <summary>
/// Renders the buildings on the map.
/// </summary>
class BuildingMaker : InfrastructureBehaviour {
    /// <summary>
    /// The material used for each building.
    /// </summary>
    public Material building;
    /// <summary>
    /// Parent object to append all buildings to.
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// Obtains all ways which are boundaries and procedurally renders each wall for
    /// the building.
    /// </summary>
    /// <returns>Returns null when all buildings are rendered.</returns>
    IEnumerator Start() {
        //Waits for all map data to be read.
        while (!map.IsReady) {
            yield return null;
        }
        //Each way in the list of ways that is a boundary (rather than a road).
        foreach (var way in map.ways.FindAll((w) => w.isBoundary && w.nodeIDs.Count > 1)) {
            //Creates new gameobject for the wall.
            GameObject go = new GameObject(way.id.ToString());
            go.transform.SetParent(parent.transform);
            //Get the centre of building.
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;
            
            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            //Set material for each wall of a building.
            mr.material = building;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();
            //For every point in the way
            for (int i = 1; i < way.nodeIDs.Count; i++) {
                //Setting p1 to the previous point.
                OsmNode p1 = map.nodes[way.nodeIDs[i - 1]];
                //Setting p2 to the next point.
                OsmNode p2 = map.nodes[way.nodeIDs[i]];

                //Create vectors using 2 points with building height
                //Creating a rectangle representing one wall of a building.
                Vector3 v1 = p1 - localOrigin;
                Vector3 v2 = p2 - localOrigin;
                Vector3 v3 = v1 + new Vector3(0, way.height, 0);
                Vector3 v4 = v2 + new Vector3(0, way.height, 0);

                //Add these vectors marking the indicies for the mesh filter.
                vectors.Add(v1);
                vectors.Add(v2);
                vectors.Add(v3);
                vectors.Add(v4);

                //Add 4 normals for each vector.
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);

                //Each idx represents correspoding vector.
                int idx1, idx2, idx3, idx4;
                idx4 = vectors.Count - 1;
                idx3 = vectors.Count - 2;
                idx2 = vectors.Count - 3;
                idx1 = vectors.Count - 4;

                //Draw first triangle with vectors v1, v3, v2.
                indices.Add(idx1);
                indices.Add(idx3);
                indices.Add(idx2);

                //Draw second triangle with vectors v3, v4, v2.
                indices.Add(idx3);
                indices.Add(idx4);
                indices.Add(idx2);

                //Draw third triangle with vectors v2, v3, v1.
                indices.Add(idx2);
                indices.Add(idx3);
                indices.Add(idx1);

                //Draw fourth triangle with vectors v2, v4, v3.
                indices.Add(idx2);
                indices.Add(idx4);
                indices.Add(idx3);
            }
            //Set vectors to outline mesh filter.
            mf.mesh.vertices = vectors.ToArray();
            //Set normals for each vector.
            mf.mesh.normals = normals.ToArray();
            //Set the order of triangles to be rendered.
            mf.mesh.triangles = indices.ToArray();

            yield return null;
        }
    }
}

