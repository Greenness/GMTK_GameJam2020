using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyType
    {
        Red,
        Blue,
        Green,
        NumEnemyTypes
    }

    public EnemyType behaviorType;
    public Vector2 movement;
    public float speed = 1.0f;
    Rigidbody2D rb;
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
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        myDirection = Direction.down;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        FlipSprites();
        Animation();

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
        // Bounce within Camera
        Vector3 posInCamera = Camera.main.WorldToViewportPoint(transform.position);
        if ((posInCamera.x > 0.98f && movement.x > 0) || (posInCamera.x < 0.02f && movement.x < 0))
        {
            movement.x *= -1;
        }
        if ((posInCamera.y > 0.98f && movement.y > 0) || (posInCamera.y < 0.02f && movement.y < 0))
        {
            movement.y *= -1;
        }
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

        //Is Moving?
        //float speed = movement.magnitude;
        //anim.SetBool("isMoving", speed > 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        switch (collidedObj.tag)
        {
            case "Player":
                collidedObj.SetActive(false);
                break;
            case "Bot":
                collidedObj.SetActive(false);
                break;
            case "Bullet":
                this.gameObject.SetActive(false);
                collidedObj.SetActive(false);
                break;
        }
    }
}
