using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float mySpeed;

    [SerializeField] 
    private float myWaitTime = 1f;

    [SerializeField]
    private Vector2 myDirection;

    [SerializeField]
    private float mySpeedGained = 0.5f;

    [SerializeField]
    private AudioClip paddleHitSound;

    [SerializeField]
    private AudioClip wallHitSound;

    [SerializeField]
    private AudioClip asteroidHitSound;

    private Rigidbody2D myRigidBody;
    private AudioSource myAudioSource;

    // Get neede components & wait before moving
    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();

        StartCoroutine(MoveAfterDelay());
    }

    // Reflect off of walls and, if collided with player, increase speed a bit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 collisionNormal = collision.contacts[0].normal;

        if (collisionNormal.y != 0)
            myDirection.y = -myDirection.y;
        else if (collisionNormal.x != 0)
            myDirection.x = -myDirection.x;

        myRigidBody.velocity = myDirection * mySpeed;

        if (collision.gameObject.CompareTag("Player"))
            mySpeed += mySpeedGained;

        PlaySound(collision.gameObject);
    }

    // Plays a sound corresponding to what it hits (player, wall, or asteroid)
    private void PlaySound(GameObject collision)
    {
        if (myAudioSource.isPlaying)
            return;

        if (collision.CompareTag("Player"))
            myAudioSource.PlayOneShot(paddleHitSound);
        else if (collision.CompareTag("Wall"))
            myAudioSource.PlayOneShot(wallHitSound);
        else if (collision.CompareTag("Asteroid"))
            myAudioSource.PlayOneShot(asteroidHitSound);
    }

    // Moves the ball after a delay defined in the "myWaitTime" variable
    // Always moves towards the losing player, or random if they are tied. 
    IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(myWaitTime);

        int winningPlayerId = GameManager.Instance.GetWinningPlayerId();
        int coinFlip = Random.Range(0, 2) == 0 ? 1 : -1;

        if (winningPlayerId == 0)
            myDirection = new Vector2(1, coinFlip);
        else if (winningPlayerId == 1)
            myDirection = new Vector2(-1, coinFlip);
        else
            myDirection = new Vector2(Random.Range(0, 2) == 0 ? 1 : -1, coinFlip);

        myRigidBody.velocity = myDirection * mySpeed;
    }
}
