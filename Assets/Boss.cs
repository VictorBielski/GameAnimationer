using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public int maxHealth = 5;
    public int currentHealth;

    public Transform player;

    public bool isFlipped = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x < player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x > player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

}
