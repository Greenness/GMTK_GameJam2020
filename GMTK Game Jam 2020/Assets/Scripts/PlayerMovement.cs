using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public GameObject scoreKeeperInstance;
    public GameObject botPrefab;
    ObjectPooler botPooler;
    Vector3 pos;
    Rigidbody2D rb;
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        botPooler = new ObjectPooler(botPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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
            Vector3 offset = new Vector3(0.5f, 0.5f, 0f);
            newBot.transform.position = transform.position + offset;
            BotBehavior newBotScript = newBot.GetComponent<BotBehavior>();
            newBotScript.behaviorType = bType;
            newBotScript.scoreKeeperInstance = this.scoreKeeperInstance;
            newBot.SetActive(true);
        }
    }


    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
