using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Vector2 movement;
    public float lifeSpan;
    public bool isCorrupted = false;
    float duration;

    // Start is called before the first frame update
    void Start()
    {
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
        transform.position = transform.position + (Vector3)movement * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        if (!isCorrupted && (collidedObj.tag == "Enemy" || collidedObj.tag == "CorruptedBot"))
        {
            collidedObj.SetActive(false);
        }
        if (isCorrupted && collidedObj.tag == "Bot")
        {
            collidedObj.SetActive(false);
        }
        duration = 0;
        this.gameObject.SetActive(false);
    }
}
