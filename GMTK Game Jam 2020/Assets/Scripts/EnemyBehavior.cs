using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyType
    {
        Red,
        Blue,
        Green
    }

    public EnemyType behaviorType;
    public Vector2 movement;
    public float speed = 5.0f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (behaviorType)
        {
            case EnemyType.Red:
                RedUpdate();
                break;
            case EnemyType.Blue:
                BlueUpdate();
                break;
            case EnemyType.Green:
                GreenUpdate();
                break;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.deltaTime);
    }

    void RedUpdate()
    {
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(transform.position, 10.0f);
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;

            if (detectedObject.tag == "Player")
            {
                movement = (detected.transform.position - transform.position).normalized * speed;
                break;
            }
        }
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;

            if (detectedObject.tag == "Bot")
            {
                movement = (detected.transform.position - transform.position).normalized * speed;
                break;
            }
        }
    }

    void BlueUpdate()
    {

    }

    void GreenUpdate()
    {
        // Stay Still
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        switch(collidedObj.tag) { 
            case "Bot":
            case "Player":
                collidedObj.SetActive(false);
                break;
            case "Bullet":
                this.gameObject.SetActive(false);
                collidedObj.SetActive(false);
                break;
        }
    }
}
