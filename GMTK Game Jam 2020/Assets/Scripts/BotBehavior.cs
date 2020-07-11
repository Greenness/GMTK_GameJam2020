using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBehavior : MonoBehaviour
{
    public enum BehaviorType
    {
        Red,
        Blue,
        Green
    }

    public BehaviorType behaviorType;
    public bool isCorrupted;
    Rigidbody2D rb;
    Vector2 movement;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        isCorrupted = false;
        speed = 5.0f;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (behaviorType)
        {
            case BehaviorType.Red:
                RedUpdate();
                break;
            case BehaviorType.Blue:
                BlueUpdate();
                break;
            case BehaviorType.Green:
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
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(transform.position, 20.0f);
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;
            if (detectedObject.tag == "Enemy")
            {
                movement = (detected.transform.position - transform.position).normalized * speed;
                break;
            }
        }
    }

    void BlueUpdate()
    {
        // 
    }

    void GreenUpdate()
    {
        // Stay Still
    }
}
