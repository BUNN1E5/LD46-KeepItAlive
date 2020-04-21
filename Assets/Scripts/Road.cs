using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public bool isLightRed = false;
    public Vector3[] intersectionDirections;

    public Vector3 ChooseDirectionFromIntersection(int seed){
        Random.InitState(seed);
        return intersectionDirections[Random.Range(0, intersectionDirections.Length)];
    }

}
