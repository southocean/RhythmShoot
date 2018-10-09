using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Vector3 offset;
    public Animator anim;
    public TextMeshPro text;

    public bool justFollow = false;

    public bool titleDisappeared = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitForTitle());
        PlayerController2D.Instance.PlayerDie += OnplayerDie;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = PlayerController2D.pos + offset;
	}

    void OnplayerDie()
    {
        if (justFollow)
        {
            return;
        }
        text.text = Enemy.EnemyKilled.ToString();
        anim.SetTrigger("display");
        StartCoroutine(WaitForTitle());
    }

    public void Peak()
    {
        if (titleDisappeared && Enemy.EnemyKilled > 0)
        {
            text.text = Enemy.EnemyKilled.ToString();
            anim.SetTrigger("peak");
        }
    }

    IEnumerator WaitForTitle()
    {
        titleDisappeared = false;
        yield return new WaitForSeconds(3f);
        titleDisappeared = true;
    }
}
