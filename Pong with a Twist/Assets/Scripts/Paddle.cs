using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // ID of this player
    public int playerId = 0;

    // Helper getter to get the velocity from the rigidbody of the paddle
    public Vector2 velocity
    {
        get
        {
            return myRigidBody.velocity;
        }
    }

    [SerializeField]
    private float yBounds = 4;

    // Keycodes serialized to allow for multiplayer by defining controls in inspector
    [SerializeField]
    private KeyCode upKey;

    [SerializeField]
    private KeyCode downKey;

    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidBody;

    // hooks up relevant components (just rigidbody for now)
    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Really only used for moving the player. Moves according to rigidbody velocity so collision can still happen.
    void FixedUpdate()
    {
        if (GameManager.Instance.gameOver)
        {
            myRigidBody.velocity = Vector3.zero;
            return;
        }

        if (Input.GetKey(upKey))
            myRigidBody.velocity = new Vector3(0, speed * Time.fixedDeltaTime, 0);
        else if (Input.GetKey(downKey))
            myRigidBody.velocity = new Vector3(0, -speed * Time.fixedDeltaTime, 0);
        else
            myRigidBody.velocity = Vector3.zero;
    }
}
