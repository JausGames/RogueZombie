using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    [SerializeField]  bool[] connectors = new bool[4];

    public void SetConnectors(bool[] value)
    {
        if (value.Length != 4) return;
        connectors = value;
    }
    public bool[] GetConnectors()
    {
        return connectors;
    }

    public bool[] RotateConnectors()
    {
        bool[] con = new bool[4];
        con[0] = connectors[1];
        con[1] = connectors[2];
        con[2] = connectors[3];
        con[3] = connectors[0];
        connectors = con;
        return connectors;
    }
}
