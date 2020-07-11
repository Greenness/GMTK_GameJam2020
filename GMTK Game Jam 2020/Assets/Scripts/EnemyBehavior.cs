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
    GameObject target;

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
        if (target == null || target.activeSelf == false)
        {
            target = FindTarget();
        }
        if (target != null) {
            movement = (target.transform.position - transform.position).normalized * speed;
        } else
        {
            movement = (new Vector3(0f, 0f, 0f) - transform.position).normalized * speed;
        }
    }

    GameObject FindTarget()
    {
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(transform.position, 10.0f);
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;

            if (detectedObject.tag == "Player")
            {
                return detectedObject;
            }
        }
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;

            if (detectedObject.tag == "Bot")
            {
                return detectedObject;
            }
        }
        return null;
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
