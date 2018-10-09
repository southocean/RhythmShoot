using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    public Animator anim;
    EnemySpawner spawner;

	// Use this for initialization
	void Start ()
    {
        spawner = EnemySpawner.Instance;
        PlayerController2D.Instance.PlayerDie += OnPlayerDie;
    }

    public void OnPlayerDie()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SelfKillDelay());
        }
    }

    IEnumerator SelfKillDelay()
    {
        //if (!isDead)
        //{
        yield return new WaitForSeconds(Random.Range(.1f, 1f));
        //isDead = true;
        anim.SetTrigger("taken");
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
        //Destroy(gameObject);
        //anim.SetBool("explodable", false);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DeactivateDelay(.25f));
        }
    }

    IEnumerator DeactivateDelay(float delay)
    {
        anim.SetTrigger("taken");
        spawner.FoodTaken(transform.position);
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
