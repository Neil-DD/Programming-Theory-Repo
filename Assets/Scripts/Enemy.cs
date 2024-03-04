using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;

    private Rigidbody enemyRb;

    private GameObject player;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected void Update()
    {
        Action();
        CheckDropdown();
    }

    protected virtual void Action()//ABSTRACTION
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(lookDirection * speed);
    }

    void CheckDropdown()//ABSTRACTION
    {
        //if emeny drop down, destory them
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
