using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        health = 1; // Only 1 life for now
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            print("He ded");
            transform.position = new Vector2(-20.0f, -20.0f);
            this.gameObject.SetActive(false);
            health = -1;
        }
    }
}
