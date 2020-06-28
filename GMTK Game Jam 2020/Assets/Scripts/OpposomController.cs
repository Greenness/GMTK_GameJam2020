using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpposomController : MonoBehaviour
{
    public Vector2 velocity = new Vector2(-1.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;

        transform.position = newPosition;
	
	Vector3 characterScale = transform.localScale;
	if (velocity.x > 0f)
	{
	    characterScale.x = -5;
	}
	if (velocity.x < 0f)
	{
	    characterScale.x = 5;
	}
	transform.localScale = characterScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Checks if other gameobject has a Tag of Player
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().health -= 1;
        }
        velocity.x *= -1.0f;
    }

}
