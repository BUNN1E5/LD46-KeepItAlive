using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;

[ExecuteInEditMode]
public class StreetLayout : MonoBehaviour
{

    public Transform[] points;
    List<List<int>> connections;    
    public float pointDistance = 3;

    [ContextMenu("Generate Connections")]
    void GenerateConnections(){
        points = this.gameObject.GetComponentsInChildren<Transform>();

        NativeArray<float3> positions = new NativeArray<float3>(points.Length, Allocator.Temp);
        this.connections = new List<List<int>>();
        
        for(int i = 0; i < points.Length; i++){
            positions[i] = points[i].position; 
            this.connections.Add(new List<int>());
        }

        for(int i = 0; i < positions.Length; i++){
            for(int j = 0; j < positions.Length; j++){
                if(i == j)
                    continue;

                if(math.distance(positions[i], positions[j]) <= pointDistance){
                    connections[i].Add(j);
                }
            }
        }
        positions.Dispose();
    }
    
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        GenerateConnections();
    }

    void OnDrawGizmos(){

        Gizmos.color = Color.white;
        for(int i = 0; i < points.Length; i++){
            Gizmos.DrawSphere(points[i].position, .1f);
            for(int j = 0; j < connections[i].Count; j++){
                Gizmos.DrawLine(points[i].position, points[connections[i][j]].position);
            }
        }
    }
}



