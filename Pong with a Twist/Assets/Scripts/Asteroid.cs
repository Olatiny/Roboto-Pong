using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public enum Size
    {
        Small = 1,
        Medium = 2,
        Large = 3
    }

    public Size mySize;

    [SerializeField]
    private float mySpeed;

    [SerializeField]
    private Vector2 myDirection;

    [SerializeField]
    private GameObject smallPrefab;

    [SerializeField]
    private GameObject mediumPrefab;

    private Rigidbody2D myRigidBody;

    void Start()
    {
        // Pick a random cardinal direction and rotation.
        // Speed is higher for smaller asteroids

        float xRand = Random.Range(0, 2) == 0 ? -1 : 1;
        float yRand = Random.Range(0, 2) == 0 ? -1 : 1;

        myDirection = new Vector2(xRand, yRand);
        myDirection = myDirection.normalized;

        myRigidBody= GetComponent<Rigidbody2D>();
        myRigidBody.velocity = myDirection * mySpeed * ((int) mySize);

        myRigidBody.angularVelocity = Random.Range(-25f * (int)mySize, 25f * (int)mySize);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect off of walls, similar to the way the ball does it
        Vector3 collisionNormal = collision.contacts[0].normal;

        if (collisionNormal.y != 0)
            myDirection.y = -myDirection.y;
        else if (collisionNormal.x != 0)
            myDirection.x = -myDirection.x;

        myRigidBody.velocity = myDirection * mySpeed;

        // Let the player just destroy them
        if (collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);

        // Everything else only applies to balls
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        // Destroy self and spawn in smaller versoins. 2 if was large, 3 if was medium.
        // Small asteroid just gets destroyed.
        Destroy(gameObject);

        switch (mySize)
        {
            case Size.Medium:
                Instantiate(smallPrefab, transform.position, transform.rotation);
                Instantiate(smallPrefab, transform.position, transform.rotation);
                Instantiate(smallPrefab, transform.position, transform.rotation);
                break;
            case Size.Large:
                Instantiate(mediumPrefab, transform.position, transform.rotation);
                Instantiate(mediumPrefab, transform.position, transform.rotation);
                break;
        }
    }
}
