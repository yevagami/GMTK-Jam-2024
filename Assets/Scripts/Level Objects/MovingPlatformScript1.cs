using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript1 : MonoBehaviour
{
    [Header("Components")]
    public GameObject pointA;
    public GameObject pointB;
    public GameObject platform;
    Transform targetPoint;
    Rigidbody2D rb2d;

    [Header("Functionality")]
    public float moveSpeed;
    Vector3 outVel;
    public bool triggerActivated = false;
    public bool triggerStatus = false;
    public bool isActive = true;
    float angle = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = platform.GetComponent<Rigidbody2D>();
        targetPoint = pointB.transform;
        platform.transform.position = pointA.transform.position;
    }

    private void Update() {
        if (triggerActivated) {
            isActive = triggerStatus;
        }

        if(isActive) {

            angle += Time.deltaTime * moveSpeed;
            if(angle >= 360.0f) { angle = 0.0f; }

            float t = (Mathf.Cos(angle) + 1) / 2;
            Vector3 resultPos = Vector3.Lerp(pointB.transform.position, pointA.transform.position,  t);
            platform.transform.position = resultPos;

            rb2d.velocity = (resultPos - platform.transform.position) / Time.deltaTime;
            //Vector3.SmoothDamp(platform.transform.position, targetPoint.position, ref outVel, moveTime);
            //rb2d.velocity = outVel;

            //if(platform.transform.position == targetPoint.transform.position) {
            //    if(targetPoint == pointA.transform) {
            //        targetPoint = pointB.transform;
            //    } else {
            //        targetPoint = pointA.transform;
            //    }
            //}

        }

    }

    public void Activate() {
        triggerStatus = true;
    }

    public void Deactivate() {
        triggerStatus = false;
    }
}
