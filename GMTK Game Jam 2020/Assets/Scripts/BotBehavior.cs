using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBehavior : MonoBehaviour
{
    public enum BehaviorType
    {
        Red,
        Blue,
        Green,
        NumBotTypes
    }

    public GameObject gameControllerInstance;
    public BehaviorType behaviorType;
    public float speed;
    public GameObject bullet;
    bool isCorrupted;
    Rigidbody2D rb;
    Vector2 movement;
    GameObject target;

    //Directions
    public enum Direction { up, down, left, right };
    public Direction myDirection;

    //Sprite and Animator
    SpriteRenderer spriteRenderer;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        isCorrupted = false;
        bullet = null;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        myDirection = Direction.down;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        anim = this.gameObject.GetComponent<Animator>();

        if (behaviorType == BehaviorType.Blue) {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        FlipSprites();
        Animation();

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
        if (behaviorType == BehaviorType.Red) {
            Vector2 pos = rb.position + movement * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -5f, 5f);
            pos.y = Mathf.Clamp(pos.y, -2f, 2f);
            rb.MovePosition(pos);
        }
    }

    void RedUpdate()
    {
        if (target == null || target.activeSelf == false)
        {
            target = FindTarget();
        }
        if (target != null)
        {
            movement = (target.transform.position - transform.position).normalized * speed;
        } else
        {
            movement.Set(0f, 0f);
        }

        FindAimAndShoot();

    }

    void BlueUpdate()
    {
        //Nothing at the moment. Is a static rigidbody that cannot be moved and blocks enemies 
    }

    void GreenUpdate()
    {
        // Stay Still
        FindAimAndShoot();
    }

    void ChangeDirection()
    {
        float x = movement.x;
        float y = movement.y;
        float absX = Mathf.Abs(x);
        float absY = Mathf.Abs(y);

        if (x > 0 && absX > absY)
        {
            myDirection = Direction.right;
        }
        else if (x < 0 && absX > absY)
        {
            myDirection = Direction.left;
        }
        else if (y > 0 && absY > absX)
        {
            myDirection = Direction.up;
        }
        else if (y < 0 && absY > absX)
        {
            myDirection = Direction.down;
        }

    }

    void FlipSprites()
    {
        Sprite currentSprite = spriteRenderer.sprite;

        if (myDirection == Direction.left)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void Animation()
    {
        //Set Direction
        anim.SetBool("Down", myDirection == Direction.down);
        anim.SetBool("Up", myDirection == Direction.up);
        anim.SetBool("Side", myDirection == Direction.right ^ myDirection == Direction.left);

        //Is Corrupted?
        anim.SetBool("isCorrupted", isCorrupted);
    }

    void FindAimAndShoot()
    {
        float bulletSpeed = behaviorType == BehaviorType.Green ? 2.0f : 5.0f;
        float bulletLifeSpan = behaviorType == BehaviorType.Green ? 10.0f: 5.0f;
        if (this.bullet == null || this.bullet.activeSelf == false)
        {
            Collider2D[] hittableObjs = Physics2D.OverlapCircleAll(transform.position, 5);
            foreach (Collider2D hittable in hittableObjs)
            {
                GameObject hittabledObject = hittable.gameObject;
                if ((isCorrupted && hittabledObject.tag != "Bot") || 
                    (!isCorrupted && (hittabledObject.tag != "Enemy" && hittabledObject.tag != "CorruptedBot")))
                {
                    continue;
                }
                    Vector3 hittableDirection = (hittable.transform.position - transform.position).normalized;
                    Vector2 bulletVelocity = hittableDirection * bulletSpeed;
                    Vector3 bulletStartingPosition = transform.position + 2f * hittableDirection * this.gameObject.GetComponent<BoxCollider2D>().size.magnitude;
                    bullet = gameControllerInstance.GetComponent<GameController>().GetNewBullet(bulletStartingPosition, bulletVelocity, bulletLifeSpan, this.isCorrupted);
                    return;
            }

        }
    }

    GameObject FindTarget()
    {
        float closestDistanceSquared = float.PositiveInfinity;
        GameObject closestTarget = null;
        Collider2D[] detectedObjs = Physics2D.OverlapCircleAll(transform.position, 10.0f);
        foreach (Collider2D detected in detectedObjs)
        {
            GameObject detectedObject = detected.gameObject;
            if ((isCorrupted && detectedObject.tag != "Player") || 
                (!isCorrupted && (detectedObject.tag != "Enemy" && detectedObject.tag != "CorruptedBot")))
            {
                continue;
            }

            float distanceSquared = (transform.position.x - detectedObject.transform.position.x) *
                (transform.position.x - detectedObject.transform.position.x) +
                (transform.position.y - detectedObject.transform.position.y) *
                (transform.position.y - detectedObject.transform.position.y);
            if (distanceSquared < closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestTarget = detectedObject;
            }
        }
        return closestTarget;
    }

    public void Corrupt(bool corruption)
    {
        isCorrupted = corruption;
        target = null;
        this.gameObject.tag = corruption ? "CorruptedBot" : "Bot";
    }
}
