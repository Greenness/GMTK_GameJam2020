using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyPrefab;
    ObjectPooler enemyPooler;
    void Start()
    {
        enemyPooler.objectToPool = enemyPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
