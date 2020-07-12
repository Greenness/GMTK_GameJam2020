using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPickup : MonoBehaviour
{
    public GameObject gameControllerInstance;
    public int points;
    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current Position is " + transform.position);
        transform.position = position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        switch (collidedObj.tag)
        {
            case "Player":
                this.gameObject.SetActive(false);
                this.gameControllerInstance.GetComponent<GameController>().AddScore(points);
                break;
            case "Enemy":
                this.gameObject.SetActive(false);
                break;
        }
    }
}
