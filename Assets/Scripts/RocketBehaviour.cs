using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    private float speed = 15.0f;

    private float rocketStrength = 15f;

    private Transform target;

    private bool homing;

    private float aliverTimer = 5f;

    // Update is called once per frame
    void Update()
    {
        if (homing)
        {
            if (target != null)
                transform.LookAt(target);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void Fire(Transform transform)
    {
        target = transform;
        homing = true;
        Destroy(gameObject, aliverTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
        {
            if (collision.gameObject.CompareTag(target.tag))
            {
                Vector3 awayFromProjectile = -collision.contacts[0].normal;

                target.GetComponent<Rigidbody>().AddForce(awayFromProjectile * rocketStrength, ForceMode.Impulse);

                Destroy(gameObject);
            }
        }

    }

}
