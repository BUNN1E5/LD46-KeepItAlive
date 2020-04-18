using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    public Transform followPoint;

    public float LookSmoothness = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //We always want to look at the player
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookAt(player), LookSmoothness * Time.deltaTime);


    }



    void FixedUpdate(){
        //Cast a spherecast from the camera to the player to see if thery are visible
        //If they are not visible move the camera to a position in which they will be

        //This can be calculated by 
    }
}
