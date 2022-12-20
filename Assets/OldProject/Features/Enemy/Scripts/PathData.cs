using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathWayPoints
{
    public List<Transform> wayPoints;
}

public class PathData : MonoBehaviour
{
    [SerializeField] private PathWayPoints[] paths;

    public PathWayPoints[] Paths => paths;
}
