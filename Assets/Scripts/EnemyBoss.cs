using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy //INHERITANCE
{
    [SerializeField]
    private GameObject minionPrefab;

    private float powerupStrength = 7f;

    // Start is called before the first frame update
    protected override void Start()//POLYMOPHISM
    {
        base.Start();
        SpawnManager manager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        //Release minions
        for (int i = -1; ++i < manager.WaveNumber;)
        {
            Instantiate(minionPrefab, manager.GenerateSpawnPosition(minionPrefab), minionPrefab.transform.rotation);
        }

    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromEnemy = collision.gameObject.transform.position - transform.position;

            playerRb.AddForce(awayFromEnemy.normalized * powerupStrength, ForceMode.Impulse);
        }
    }


}
