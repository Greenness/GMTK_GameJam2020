using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public GameObject botPrefab;
    ObjectPooler botPooler;
    public float botSpawnDist = 0.5f;
    Vector3 pos;
    Rigidbody2D rb;
    Vector2 movement;
    
    //Sprites and Animator
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    Animator anim;

    //Directions
    public enum Direction { up, down, left, right};
    public Direction myDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        botPooler = new ObjectPooler(botPrefab);
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        myDirection = Direction.down;
        sprites = new Sprite[3];
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        ChangeDirection();
        FlipSprites();
        Animation();

        //Keep player within camera boundaries
        pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.02f, 0.98f);
        pos.y = Mathf.Clamp(pos.y, 0.02f, 0.98f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        if (Input.GetButtonDown("Fire1"))
        {
            CreateBot(BotBehavior.BehaviorType.Red);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            CreateBot(BotBehavior.BehaviorType.Blue);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            CreateBot(BotBehavior.BehaviorType.Green);
        }
    }

    void CreateBot(BotBehavior.BehaviorType bType)
    {
        GameObject newBot = botPooler.GetPooledObject();
        if (newBot != null)
        {
            Vector3 offset = new Vector3(0f, 0f, 0f);
            if (myDirection == Direction.up) { offset.y = botSpawnDist; }
            else if (myDirection == Direction.down) { offset.y = -botSpawnDist; }
            else if (myDirection == Direction.right) { offset.x = botSpawnDist; }
            else if (myDirection == Direction.left) { offset.x = -botSpawnDist; }

            newBot.transform.position = transform.position + offset;
            BotBehavior newBotScript = newBot.GetComponent<BotBehavior>();
            newBotScript.behaviorType = bType;
            newBot.SetActive(true);
        }
    }


    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
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
        float speed = movement.magnitude;
        anim.SetBool("isMoving", speed > 0);
    }
}
