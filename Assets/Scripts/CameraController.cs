using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;

    [Range(0, 6.28f)]
    public float theta, phi = .68f;

    public float distance = 12;

    public float LookSmoothness = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //We always want to look at the player
        Quaternion lookAt = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAt, LookSmoothness * Time.deltaTime);

        float angleDiff = Vector3.Angle(-player.transform.forward, this.transform.forward);


        //The camera is free to move in a circle around the player
        //But the distance should be a fixed distance
        //Also the angle should be relative to the relative back of the vehicle
        //float _theta = theta * Mathf.Deg2Rad;
        //float _phi = phi * Mathf.Deg2Rad;

        theta = player.rotation.eulerAngles.y * Mathf.Deg2Rad;

        float x = Mathf.Cos (theta) * Mathf.Cos (phi);
        float y = Mathf.Sin (phi);
        float z = Mathf.Sin (theta) * Mathf.Cos (phi);

        this.transform.position = player.position + new Vector3(x, y, z) * distance;
        //this.transform.position = player.position;

    }
}
