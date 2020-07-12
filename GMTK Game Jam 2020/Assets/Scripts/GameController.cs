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
    GameObject playerObj;
    ObjectPooler botPooler;
    ObjectPooler enemyPooler;
    ObjectPooler bulletPooler;
    ObjectPooler pointsPooler;
    int wave;
    int score;
    int highScore;
    int corruptionChance;
    private float waveTime = 5.0f;
    private float pointsTime = 7.0f;
    private float waveTimer = 0.0f;
    private float pointsTimer = 0.0f;
    public TextMeshProUGUI waveText, scoreText, corruptionChanceText, HighScoreText, GameOverText;
    bool isGameOver;

    const float outOfBoundsRadius = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = (GameObject)Instantiate(playerPrefab);
        botPooler = new ObjectPooler(botPrefab);
        enemyPooler = new ObjectPooler(enemyPrefab);
        bulletPooler = new ObjectPooler(bulletPrefab);
        pointsPooler = new ObjectPooler(pointsPrefab);
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"))
            {
                ResetGame();
            }
            return;
        }
        waveTimer += Time.deltaTime;
        pointsTimer += Time.deltaTime;

        if (waveTimer > waveTime)
        {
            waveTimer = waveTimer - waveTime;
            wave += 1;
            corruptionChance += 5;
            corruptBotsByChance();
            waveText.SetText("" + wave);
            corruptionChanceText.SetText("" + corruptionChance + "%");
            SpawnEnemies();
        }
        if (pointsTimer > pointsTime)
        {
            pointsTimer = pointsTimer - pointsTime;
            SpawnPointsPickup();
            pointsTime = Random.Range(4f, 10f);
        }
        scoreText.SetText("" + score);
        if (score > highScore)
        {
            highScore = score;
            HighScoreText.SetText("" + highScore);
        }
        

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
                float angle = 2.0f * Mathf.PI * Random.Range(0f, 1f);
                Vector3 initialLocation = new Vector3(outOfBoundsRadius * Mathf.Cos(angle), outOfBoundsRadius * Mathf.Sin(angle));
                SpawnEnemy(initialLocation, EnemyBehavior.EnemyType.Red);
            }
        } else
        {
            int numEnemies = wave / 2;
            for (int i = 0; i < numEnemies; i++)
            {
                float angle = 2.0f * Mathf.PI * Random.Range(0f, 1f);
                Vector3 initialLocation = new Vector3(outOfBoundsRadius * Mathf.Cos(angle), outOfBoundsRadius * Mathf.Sin(angle));
                SpawnEnemy(initialLocation, EnemyBehavior.EnemyType.Red);
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
            newPickupScript.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-1.5f, 1.5f), 0f);
            newPickup.SetActive(true);
        }
    }

    public GameObject GetNewBullet(Vector3 bulletPosition, Vector2 bulletSpeed, float lifeSpan, bool isCorrupted)
    {

        GameObject newBullet = bulletPooler.GetPooledObject();
        if (newBullet != null)
        {
            newBullet.transform.position = bulletPosition;
            BulletBehavior newBulletScript = newBullet.GetComponent<BulletBehavior>();
            newBulletScript.movement = bulletSpeed;
            newBulletScript.lifeSpan = lifeSpan;
            newBulletScript.isCorrupted = isCorrupted;
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
            curr = Random.Range(1, 101);
            if (botsList[i].activeInHierarchy && curr <= corruptionChance) {
                botsList[i].GetComponent<BotBehavior>().Corrupt(true);
            }
        }
    }

    public void GameOver() {
        GameOverText.gameObject.SetActive(true);
        playerObj.SetActive(false);
        isGameOver = true;
    }

    void ResetGame()
    {
        playerObj.SetActive(true);
        isGameOver = false;
        GameOverText.gameObject.SetActive(false);
        wave = 0;
        score = 0;
        corruptionChance = 0;
        waveTime = 5.0f;
        pointsTime = 7.0f;
        waveTimer = 0.0f;
        pointsTimer = 0.0f;
        
        foreach (GameObject bot in botPooler.getAllPooledObjects())
        {
            bot.SetActive(false);
        }
        foreach (GameObject enemy in enemyPooler.getAllPooledObjects())
        {
            enemy.SetActive(false);
        }
        foreach (GameObject bullet in bulletPooler.getAllPooledObjects())
        {
            bullet.SetActive(false);
        }
        foreach (GameObject points in pointsPooler.getAllPooledObjects())
        {
            points.SetActive(false);
        }

        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();
        playerObj.transform.position.Set(0f, 0f, 0f);
        playerScript.gameControllerInstance = this.gameObject;
        wave = 1;
        SpawnEnemies();
        waveText.SetText("" + wave);
        corruptionChanceText.SetText("" + corruptionChance + "%");
        scoreText.SetText("" + score);
    }

    public bool IsNearBlueBot(Vector2 objectPosition, bool checkCorruptBot)
    {
        float threshold = 1f;
        List<GameObject> botsList = botPooler.getAllPooledObjects();
        for (int i = 0; i < botsList.Count; i++) 
        {
            if (botsList[i].GetComponent<BotBehavior>().behaviorType == BotBehavior.BehaviorType.Blue)
            {
                if (botsList[i].GetComponent<BotBehavior>().isCorrupted == checkCorruptBot && 
                botsList[i].GetComponent<BotBehavior>().transform.position.x - objectPosition.x <= threshold &&
                botsList[i].GetComponent<BotBehavior>().transform.position.y - objectPosition.y <= threshold)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
}
