using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Vector2 movement;
    public float lifeSpan;
    public bool isCorrupted = false;
    float duration;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;

        if (duration > lifeSpan)
        {
            this.gameObject.SetActive(false);
            duration = 0;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        if (!isCorrupted && (collidedObj.tag == "Enemy" || collidedObj.tag == "CorruptedBot"))
        {
            if (collidedObj.tag == "CorruptedBot" && collidedObj.gameObject.GetComponent<BotBehavior>().behaviorType == BotBehavior.BehaviorType.Blue) 
            {
                collidedObj.gameObject.GetComponent<BotBehavior>().radius.GetComponent<Renderer>().enabled = false;
            }
            collidedObj.SetActive(false);
        }
        if (isCorrupted && collidedObj.tag == "Bot")
        {
            if (collidedObj.gameObject.GetComponent<BotBehavior>().behaviorType == BotBehavior.BehaviorType.Blue) 
            {
                collidedObj.gameObject.GetComponent<BotBehavior>().radius.GetComponent<Renderer>().enabled = false;
            }
            collidedObj.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
