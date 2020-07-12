using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Vector2 movement;
    public float lifeSpan;
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
        if (collidedObj.tag == "Enemy" || collidedObj.tag == "CorruptedBot")
        {
            collidedObj.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
