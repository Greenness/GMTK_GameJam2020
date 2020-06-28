using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    private GameObject[] gameOverObjects;
    // Start is called before the first frame update
    void Start()
    {
        health = 1; // Only 1 life for now
        gameOverObjects = GameObject.FindGameObjectsWithTag("OnGameOver");
        foreach (GameObject obj in gameOverObjects)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            print("He ded");
            transform.position = new Vector2(-20.0f, -20.0f);
            this.gameObject.SetActive(false);
            foreach (GameObject obj in gameOverObjects)
            {
                obj.SetActive(true);
            }
            health = -1;
        }
    }
}
