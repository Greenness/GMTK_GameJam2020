using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject botPrefab;
    public GameObject enemyPrefab;
    public GameObject bulletPrefab;
    public GameObject pointsPrefab;
    ObjectPooler botPooler;
    ObjectPooler enemyPooler;
    ObjectPooler bulletPooler;
    ObjectPooler pointsPooler;
    int wave;
    int score;
    int corruptionChance;
    private float waveTime = 5.0f;
    private float pointsTime = 7.0f;
    private float waveTimer = 0.0f;
    private float pointsTimer = 0.0f;
    public TextMeshProUGUI waveText, scoreText, corruptionChanceText;
    System.Random random = new System.Random();
    

    // Start is called before the first frame update
    void Start()
    {
        botPooler = new ObjectPooler(botPrefab);
        enemyPooler = new ObjectPooler(enemyPrefab);
        bulletPooler = new ObjectPooler(bulletPrefab);
        pointsPooler = new ObjectPooler(pointsPrefab);
        wave = 0;
        corruptionChance = 0;

        GameObject playerObj = (GameObject)Instantiate(playerPrefab);
        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();
        playerScript.gameControllerInstance = this.gameObject;
        wave = 1;
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        waveTimer += Time.deltaTime;
        pointsTimer += Time.deltaTime;

        if (waveTimer > waveTime)
        {
            waveTimer = waveTimer - waveTime;
            wave += 1;
            corruptionChance += 5;
            corruptBotsByChance();
            waveText.SetText("Wave: " + wave);
            corruptionChanceText.SetText("Corruption Chance: " + corruptionChance + "%");
            SpawnEnemies();
        }
        if (pointsTimer > pointsTime)
        {
            Debug.Log("Adding points");
            pointsTimer = pointsTimer - pointsTime;
            SpawnPointsPickup();
            pointsTime = Random.Range(4f, 10f);
        }
        scoreText.SetText("Score: " + score);
        

    }

    void SpawnEnemies()
    {
        if (wave == 1)
        {
            Vector3 initialLocation = new Vector3(-3f, -1f);
            SpawnEnemy(initialLocation, EnemyBehavior.EnemyType.Red);
        } else if (wave % 2 == 0)
        {
            int numEnemies = (int)Mathf.Log(wave, 2f) + 1;
            for (int i = 0; i < numEnemies; i++)
            {
                Vector3 initialLocation = 3 * Random.onUnitSphere;
                SpawnEnemy(initialLocation, (EnemyBehavior.EnemyType)Random.Range(0f, (float)EnemyBehavior.EnemyType.NumEnemyTypes));
            }
        } else
        {
            int numEnemies = wave / 2;
            for (int i = 0; i < numEnemies; i++)
            {
                Vector3 initialLocation = 3 * Random.onUnitSphere;
                SpawnEnemy(initialLocation, (EnemyBehavior.EnemyType)Random.Range(0f, (float)EnemyBehavior.EnemyType.NumEnemyTypes));
            }

        }
    }

    void SpawnEnemy(Vector3 startLocation, EnemyBehavior.EnemyType bType)
    {
        GameObject newEnemy = enemyPooler.GetPooledObject();
        if (newEnemy != null)
        {
            newEnemy.transform.position = startLocation;
            EnemyBehavior newEnemyScript = newEnemy.GetComponent<EnemyBehavior>();
            newEnemyScript.behaviorType = bType;
            newEnemyScript.movement = Random.onUnitSphere;
            newEnemyScript.gameControllerInstance = this.gameObject;
            newEnemy.SetActive(true);
        }
    }

    void SpawnPointsPickup()
    {
        GameObject newPickup = pointsPooler.GetPooledObject();
        if (newPickup != null)
        {
            PointsPickup newPickupScript = newPickup.GetComponent<PointsPickup>();
            newPickupScript.gameControllerInstance = this.gameObject;
            newPickupScript.points = Random.Range(1, 5);
            newPickupScript.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-2f, 2f), 0f);
            newPickup.SetActive(true);
        }
    }

    public GameObject GetNewBullet(Vector3 bulletPosition, Vector2 bulletSpeed, float lifeSpan)
    {

        GameObject newBullet = bulletPooler.GetPooledObject();
        if (newBullet != null)
        {
            newBullet.transform.position = bulletPosition;
            BulletBehavior newBulletScript = newBullet.GetComponent<BulletBehavior>();
            newBulletScript.movement = bulletSpeed;
            newBulletScript.lifeSpan = lifeSpan;
            newBullet.SetActive(true);
        }
        return newBullet;
    }

    public GameObject GetNewBot(Vector3 botPosition, BotBehavior.BehaviorType bType)
    {
        GameObject newBot = botPooler.GetPooledObject();
        if (newBot != null)
        {
            newBot.transform.position = botPosition;
            BotBehavior newBotScript = newBot.GetComponent<BotBehavior>();
            newBotScript.behaviorType = bType;
            newBotScript.gameControllerInstance = this.gameObject;
            newBotScript.Corrupt(false);
            newBotScript.bullet = null;
            newBot.SetActive(true);
        }
        return newBot;
    }

    public void AddScore(int points)
    {
        score += points;
    }
    public void corruptBotsByChance()
    {
        List<GameObject> botsList = botPooler.getAllPooledObjects();
        int curr;
        for (int i = 0; i < botsList.Count; i++) 
        {
            curr = random.Next(1, 101);
            if (botsList[i].activeInHierarchy && curr <= corruptionChance) {
                botsList[i].GetComponent<BotBehavior>().Corrupt(true);
            }
        }
    }


  


}
