using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public static float speedMax = 3;

    EnemySpawner spawner;

    public Animator anim;

    public float speed = 3;
    //public bool isDead = false;
    PlayerController2D player;

    public static int EnemyKilled = 0;

    // Use this for initialization
    void Start () {
        spawner = EnemySpawner.Instance;
        player = PlayerController2D.Instance;
        player.PlayerDie += OnPlayerDie;
        //StartCoroutine(CheckAroundDelay());
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = PlayerController2D.pos - transform.position;
        if (dir.magnitude > 0.2f)
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
        } 
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            EnemyKilled++;
            StartCoroutine(DeactivateDelay(.25f));
            player.PlayerDie -= OnPlayerDie;
            spawner.SpawnFood(transform.position, collision.transform.position);
            //gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator DeactivateDelay(float delay)
    {
        anim.SetTrigger("die");
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void OnPlayerDie()
    {
        StartCoroutine(SelfKillDelay());
    }

    IEnumerator SelfKillDelay()
    {
        //if (!isDead)
        //{
            yield return new WaitForSeconds(Random.Range(.1f, 1f));
            //isDead = true;
            player.PlayerDie -= OnPlayerDie;
            anim.SetTrigger("die");
            yield return new WaitForSeconds(.25f);
            gameObject.SetActive(false);
            //Destroy(gameObject);
            //anim.SetBool("explodable", false);
        //}
    }

    private void OnEnable()
    {
        //isDead = false;
        if (player)
        {
            player.PlayerDie += OnPlayerDie;
        }
        speedMax += 0.01f;
        speed = speedMax;
    }
}
