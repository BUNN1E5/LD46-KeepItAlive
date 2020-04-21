using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public bool isLightRed = false;
    public Vector3[] intersectionDirections;

    public int ChooseDirectionFromIntersection(){
        return Random.Range(0, intersectionDirections.Length);
    }

}
