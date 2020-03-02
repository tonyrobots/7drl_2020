using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    GameObject followee;

    // Start is called before the first frame update
    void Start()
    {
        followee = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(followee.transform.position.x, followee.transform.position.y, transform.position.z); 
    }
}
