using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MicroBitListen : MonoBehaviour
{
    float micrograv = 0.00981f;
    public float speed = 1f;
    float[] motion;

    public GameObject bullet;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float rotationSpeed;

    public float timeSinceStart;
    public float timeSinceLastCall;
    public float ammo;
    public float startingBearing;
    bool isFirst;
    void Start()
    {
        
    }
    // Called when a message arrives from the micro:bit
    void OnMessageArrived(string msg)
    {
        

        timeSinceLastCall = Time.realtimeSinceStartup - timeSinceStart;
        timeSinceStart = Time.realtimeSinceStartup;
        msg = msg.Trim(); // Remove any extra whitespace or newline characters
        Debug.Log("Message from micro:bit: " + msg);

        if (isFirst)
        {
            motion = Array.ConvertAll(msg.Split(','), float.Parse);
            startingBearing = motion[5];
            isFirst = false;
        }


        if (msg == "A")
        {
            if(ammo > 0)
            {
                GameObject boolet = Instantiate(bulletPrefab,transform.position, transform.rotation);
                boolet.GetComponent<Rigidbody>().velocity = -transform.forward * bulletSpeed;
            }
        }
        else if (msg == "B")
        {
            ammo = 10;
        }else if (msg != "")
        {
            motion = Array.ConvertAll(msg.Split(','), float.Parse);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(motion[3], motion[5]+startingBearing, motion[4]), rotationSpeed * timeSinceLastCall);

            transform.position += new Vector3(motion[0] * speed * timeSinceLastCall,0,-motion[1] * speed * timeSinceLastCall);
        }
    }
    // Called on connect/disconnect events
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Connected to micro:bit" : "Disconnected from micro:bit");
    }
}