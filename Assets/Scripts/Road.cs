using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public bool isIntersection = false;
    public bool isLightRed = false;
    public Vector3[] intersectionDirections;
    public Vector3 roadDirection;

    public int ChooseDirectionFromIntersection(){
        return Random.Range(0, intersectionDirections.Length);
    }

}
