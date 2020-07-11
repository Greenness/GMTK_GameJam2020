using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public GameObject enemyPrefab;
    ObjectPooler enemyPooler;
    public GameObject bulletPrefab;
    ObjectPooler bulletPooler;
    int wave;
    private float waitTime = 5.0f;
    private float timer = 0.0f;
    TextMeshProUGUI waveText;

    // Start is called before the first frame update
    void Start()
    {
        enemyPooler = new ObjectPooler(enemyPrefab);
        bulletPooler = new ObjectPooler(bulletPrefab);
        wave = 0;
        waveText = this.gameObject.GetComponent<TextMeshProUGUI>();
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

    }

    void SpawnEnemies()
    {
        if (wave == 1)
        {
            Vector3 initialLocation = new Vector3(-3f, -1f);
            SpawnEnemy(initialLocation);
        } else if (wave % 2 == 0)
        {
            int numEnemies = (int)Mathf.Log(wave, 2f) + 1;
            for (int i = 0; i < numEnemies; i++)
            {
                float angle = 2.0f * Mathf.PI * Random.Range(0f, 1f);
                Vector3 initialLocation = new Vector3(3 * Mathf.Cos(angle), 3 * Mathf.Sin(angle));
                SpawnEnemy(initialLocation);
            }
        } else
        {
            int numEnemies = wave / 2;
            for (int i = 0; i < numEnemies; i++)
            {
                float angle = 2.0f * Mathf.PI * Random.Range(0f, 1f);
                Vector3 initialLocation = new Vector3(3 * Mathf.Cos(angle), 3 * Mathf.Sin(angle));
                SpawnEnemy(initialLocation);
            }

        }
    }

    void SpawnEnemy(Vector3 startLocation)
    {
        GameObject newEnemy = enemyPooler.GetPooledObject();
        if (newEnemy != null)
        {
            newEnemy.transform.position = startLocation;
            EnemyBehavior newEnemyScript = newEnemy.GetComponent<EnemyBehavior>();
            newEnemyScript.behaviorType = EnemyBehavior.EnemyType.Red;
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
}
