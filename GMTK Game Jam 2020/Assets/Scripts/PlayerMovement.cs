using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    Vector3 pos;
    Rigidbody2D rb;
    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
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

    }


    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
