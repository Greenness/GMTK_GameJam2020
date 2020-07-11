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
    ObjectPooler botPooler;
    ObjectPooler enemyPooler;
    ObjectPooler bulletPooler;
    int wave;
    int score;
    int corruptionChance;
    private float waitTime = 5.0f;
    private float timer = 0.0f;
    public TextMeshProUGUI waveText, scoreText, corruptionChanceText;

    // Start is called before the first frame update
    void Start()
    {
        botPooler = new ObjectPooler(botPrefab);
        enemyPooler = new ObjectPooler(enemyPrefab);
        bulletPooler = new ObjectPooler(bulletPrefab);

        GameObject playerObj = (GameObject)Instantiate(playerPrefab);
        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();
        playerScript.gameControllerInstance = this.gameObject;
        wave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            timer = timer - waitTime;
            wave += 1;
            waveText.SetText("Wave: " + wave);
            SpawnEnemies();
        }
        scoreText.SetText("Score: " + score);
        corruptionChanceText.SetText("Corruption Chance: " + corruptionChance + "%");

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
            newBotScript.bullet = null;
            newBot.SetActive(true);
        }
        return newBot;
    }

    public void AddScore(int points)
    {
        score += points;
    }
}
