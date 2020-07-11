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
    public GameObject scoreKeeperInstance;
    public bool isCorrupted;
    public float speed;
    Rigidbody2D rb;
    Vector2 movement;
    GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        isCorrupted = false;
        bullet = null;
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
        Vector2 pos = rb.position + movement * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, 0.02f, 0.98f);
        pos.y = Mathf.Clamp(pos.y, 0.02f, 0.98f);
        rb.MovePosition(pos);
    }

    void RedUpdate()
    {
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(transform.position, 10.0f);
        movement.Set(0f, 0f);
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;
            if (detectedObject.tag == "Enemy")
            {
                movement = (detected.transform.position - transform.position).normalized * speed;
                break;
            }
        }

        if (this.bullet == null || this.bullet.activeSelf == false)
        {
            Collider2D[] hittableObjs = Physics2D.OverlapCircleAll(transform.position, 1);
            foreach (Collider2D hittable in hittableObjs)
            {
                GameObject hittabledObject = hittable.gameObject;
                if (hittabledObject.tag == "Enemy")
                {
                    Debug.Log("Found hittable enemy!");
                    Vector3 hittableDirection = (hittable.transform.position - transform.position).normalized;
                    Vector2 bulletSpeed = hittableDirection * 1.0f;
                    Vector3 bulletStartingPosition = transform.position + hittableDirection * this.gameObject.GetComponent<BoxCollider2D>().size.magnitude;
                    bullet = scoreKeeperInstance.GetComponent<ScoreKeeper>().GetNewBullet(bulletStartingPosition, bulletSpeed, 5f);
                    break;
                }
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
