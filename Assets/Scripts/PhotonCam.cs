using UnityEngine;
using System.Collections;

public class PhotonCam : MonoBehaviour
{

    public float speed = 10;
    public float rayLength = 0.3f;

    public int gizmoReflections = 10;

    public bool realTimeControl = true;
    public float turnSpeed = 0.3f;

    Filter filter;

    void Start ()
    {
        filter = GameObject.FindObjectOfType<Filter>();
    }

    void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Color rayColor = Color.cyan;

        int counter = 0;

        while (counter < gizmoReflections)
        {
            float distance = 100;

            Ray newRay = ray;

            if (GetReflection(ray, out newRay, 10000, out distance))
            {
                Gizmos.color = rayColor;
                Gizmos.DrawRay(ray.origin, ray.direction * distance);
                ray = newRay;
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(newRay.origin, newRay.direction * 100);
                return;
            }

            // koolkolorz(tm)
            rayColor.r += 0.01f;
            rayColor.g -= 0.03f;

            counter++;
        }
    }

    bool GetReflection(Ray ray, out Ray outRay, float maxDistance)
    {
        float x;
        return GetReflection(ray, out outRay, maxDistance, out x);
    }

    bool GetReflection(Ray ray, out Ray outRay, float maxDistance, out float distance)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Vector3 outVector = Vector3.Reflect(ray.direction.normalized, hit.normal);

            outRay = new Ray(hit.point, outVector);
            distance = hit.distance;

            return true;
        }

        distance = 0;
        outRay = ray;
        return false;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.Self);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Ray reflectedRay;

        if (GetReflection(ray, out reflectedRay, rayLength))
        {
            transform.position = reflectedRay.origin; // cause of this reflection is not "seamless", there is a small jump, but it keeps moving along the preview ray
            transform.rotation = Quaternion.LookRotation(reflectedRay.direction, Vector3.up);

            if (filter) 
            {
                filter.InverseX();
            }
        }

        if (realTimeControl)
        {
            // gimbal lock prone, should not be allowed above/below certain degrees?
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed, Space.World);
            transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * turnSpeed, Space.Self);
        }
    }
}
