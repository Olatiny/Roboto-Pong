using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDeathBox : MonoBehaviour
{
    [SerializeField]
    private int playerId;

    [SerializeField]
    private int myScore = 1;

    // Only used to check for score, game manager handles the rest.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.AddScore(myScore, playerId);
            Destroy(collision.gameObject);
        }
    }
}
