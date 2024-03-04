using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rdbody;

    public float speed = 5.0f;

    private GameObject focalPoint;

    private bool hasPowerup = false;

    private float powerupStrength = 15f;

    private float smashupStrength = 50f;

    private float smashSpeed = 20f;

    public GameObject powerupIndicator;

    public GameObject rocketPrefab;

    private PowerupType currentPowerupType;

    private Coroutine powerupCountdown;

    private float flootY;

    private float explosionRadius = 150f;

    // Start is called before the first frame update
    void Start()
    {
        rdbody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        rdbody.AddForce(focalPoint.transform.forward * speed * verticalInput);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.45f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currentPowerupType = other.gameObject.GetComponent<Powerup>().powerupType;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);

            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }

            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());

        }
    }

    private bool isOnGround;

    IEnumerator PowerupCountdownRoutine()
    {

        switch (currentPowerupType)
        {
            case PowerupType.Pushback:
                yield return new WaitForSeconds(7);
                break;
            case PowerupType.Rockets:
                float time = Time.time;
                while (Time.time - time < 7)
                {
                    LaunchRocket();
                    yield return new WaitForSeconds(0.5f);
                }
                break;
            case PowerupType.Smash:
                time = Time.time;
                while (Time.time - time < 7)
                {
                    flootY = transform.position.y;
                    //rdbody.AddForce(Vector3.up * 20, ForceMode.Impulse);
                    rdbody.velocity = new Vector3(0, smashSpeed, 0);
                    yield return new WaitForSeconds(0.5f);
                    isOnGround = false;

                    while (transform.position.y > flootY && !isOnGround)
                    {
                        //rdbody.AddForce(Vector3.down * 70, ForceMode.Impulse);
                        rdbody.velocity = new Vector3(0, -smashSpeed * 2, 0);
                        yield return null;
                    }

                    //yield return new WaitForSeconds(1f);
                }
                break;
        }

        hasPowerup = false;
        powerupCountdown = null;
        powerupIndicator.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerupType == PowerupType.Pushback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);

            Debug.Log("Collided with " + collision.gameObject.name);
        }
        else if (collision.gameObject.name.Equals("Island") && currentPowerupType == PowerupType.Smash)
        {
            isOnGround = true;
            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();

                //Vector3 awayFromPlayer = enemy.transform.position - transform.position;
                //float distance = Vector3.Distance(enemy.transform.position, transform.position);
                //enemyRb.AddForce(awayFromPlayer.normalized * (1 - distance / 70) * smashupStrength, ForceMode.Impulse);

                enemyRb.AddExplosionForce(smashupStrength, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);

            }
        }
    }

    private void LaunchRocket()
    {

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            Vector3 lookDirection = (enemy.transform.position - transform.position).normalized;

            var projectile = Instantiate(rocketPrefab, transform.position + lookDirection * 2, Quaternion.LookRotation(lookDirection));

            projectile.GetComponent<RocketBehaviour>().Fire(enemy.gameObject.transform);
        }
    }
}
