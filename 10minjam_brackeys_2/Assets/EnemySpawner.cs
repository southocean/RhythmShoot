using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
    public float waitingTime = 2f;
    public float degeneratePercent = 0.95f;

    public float foodDrop = 0.1f;

    public static EnemySpawner Instance;
    public PlayerController2D player;

    public EZObjectPool enemyPool;
    public EZObjectPool foodPool;
    public EZObjectPool enemyExplosionPool;
    public EZObjectPool bulletExplosionPool;
    public EZObjectPool foodExplosionPool;
    public AudioManager audioManager;

    public bool isDead = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance!=this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
        player.PlayerDie += OnPlayerDie;
        StartCoroutine(SpawnEnemy());
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    Vector3 GetPos()
    {
        if (Random.Range(0f, 1f) < .5f)
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0, 2), 0));
        }
        return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 2), Random.Range(0f, 1f), 0));
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(waitingTime);
            if (waitingTime > 0.03f)
            {
                waitingTime *= degeneratePercent;
            }
            if (waitingTime < .5f && degeneratePercent > 0.98f)
            {
                degeneratePercent = 0.98f;
            }
            //Debug.Log("waiting time = " + waitingTime);
        }
    }
    
    void Spawn()
    {
        enemyPool.TryGetNextObject(GetPos(), Quaternion.identity);
    }

    void OnPlayerDie()
    {
        audioManager.PlaySound("LevelLost");
        StopAllCoroutines();
        waitingTime = 2f;
        degeneratePercent = 0.99f;
        isDead = true;
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3f);
        Enemy.EnemyKilled = 0;
        Enemy.speedMax = 3f;
        isDead = false;
        foodDrop = .2f;
        StartCoroutine(SpawnEnemy());
    }

    public void SpawnFood(Vector3 enemy, Vector3 bullet)
    {
        if (isDead)
        {
            return;
        }
        if (Random.Range(0f, 1f) < foodDrop)
        {
            foodPool.TryGetNextObject(enemy, Quaternion.identity);
        }
        audioManager.PlaySound("FoodAppear");
        audioManager.PlaySound("Explosion");
        enemyExplosionPool.TryGetNextObject(enemy, Quaternion.identity);
        bulletExplosionPool.TryGetNextObject(bullet, Quaternion.identity);
    }

    public void FoodTaken(Vector3 food)
    {
        if (isDead)
        {
            return;
        }
        audioManager.PlaySound("Food");
        foodExplosionPool.TryGetNextObject(food, Quaternion.identity);
    }
}
