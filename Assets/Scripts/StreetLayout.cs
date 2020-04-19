using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;


public class StreetLayout : MonoBehaviour
{

    public Transform[] points;
    List<List<int>> connections;
    private float _pointMaxDistance = 3;
    public float pointMaxDistance{
        get{
            return _pointMaxDistance;
        }

        set{
            _pointMaxDistance = value;
            GenerateConnections(_pointMaxDistance);
        }
    }

    // Start is called before the first frame update
    void GenerateConnections(float maxDist){
        NativeArray<float3> positions = new NativeArray<float3>(points.Length, Allocator.Temp);
        NativeList<int> connections = new NativeList<int>(positions.Length, Allocator.Temp);
        this.connections = new List<List<int>>();
        
        for(int i = 0; i < points.Length; i++){
            positions[i] = points[i].position; 
            this.connections.Add(new List<int>());
        }

        JobHandle handle = new layoutJob(){
            positions = positions,
            connections = connections,
            pointMaxDistance = maxDist
        }.Schedule(positions.Length, 1);
        
        for(int i = 0, j = 0, k = 0; i < connections.Length; i++){
            if(connections[i] == -1){
                k++;
                j = 0;
                continue;
            }
            this.connections[k][j] = connections[i];
        }

        positions.Dispose();
        connections.Dispose();
    }
    
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnDrawGizmos(){
        for(int i = 0; i < points.Length; i++){

        }
    }
}

[BurstCompile]
public struct layoutJob : IJobParallelFor{
    
    public NativeArray<float3> positions;
    public NativeList<int> connections;
    public float pointMaxDistance;

    public void Execute(int index){
        for(int i = 0; i < positions.Length; i++){
            if(math.distance(positions[index], positions[i]) < pointMaxDistance){
                connections.Add(i);
            }
            connections.Add(-1);
        }
    }

}


