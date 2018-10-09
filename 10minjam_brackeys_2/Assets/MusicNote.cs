using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicNote : MonoBehaviour {

    public PlayerController2D player;
    public TextMeshProUGUI rhythmIndicator;
    public Animator rhythmAnim;
    public AudioManager audioManager;

    public bool hasDisplayed = false;
    public bool hasPressed = false;
    public bool onTrigger = false;

    private void Start()
    {
        player.PlayerDie += OnPlayerDie;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger enter, ontrigger = " + onTrigger + ", hasdisplayed = " + hasDisplayed);
        hasDisplayed = false;
        hasPressed = false;
        onTrigger = true;
        ResetParam(.5f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exit, ontrigger = " + onTrigger + ", hasdisplayed = " + hasDisplayed);

        onTrigger = false;
        //canSpace = false;
        if (!hasDisplayed)
        {
            //if (rhythmAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !rhythmAnim.IsInTransition(0))
            //{ return; }
            // displaye missed

            if (PlayerController2D.isDead)
            {
                return;
            }
            rhythmIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f)));
            rhythmIndicator.text = "Missed";
            rhythmAnim.SetTrigger("display");
            audioManager.PlaySound("OffBeat");
            hasDisplayed = true;
        }
        //StartCoroutine(ResetParam(.1f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !onTrigger)
        {
            if (PlayerController2D.isDead)
            {
                return;
            }
            rhythmIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f)));
            rhythmIndicator.text = "Missed";
            rhythmAnim.SetTrigger("display");
            audioManager.PlaySound("OffBeat");
            hasDisplayed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Trigger with " + collision.gameObject.name);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ShootThree();
            //collision.GetComponent<Collider2D>().enabled = false;
            hasPressed = true;
            //if (!hasDisplayed)
            //{
                hasDisplayed = true;
            if (PlayerController2D.isDead)
            {
                return;
            }
            rhythmIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f)));
            rhythmIndicator.text = Random.Range(0f, 1f) < .5f ? "Great!" : "Perfect!";
            rhythmAnim.SetTrigger("display");
            audioManager.PlaySound("OnBeat");
            //StartCoroutine(ResetParam(.1f));
            // display Great!
            //}
        }
    }

    IEnumerator ResetParam(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!hasDisplayed)
        {
            // Trigger Exit is not registered

            if (!PlayerController2D.isDead)
            {
                rhythmIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10f, 10f)));
                rhythmIndicator.text = "Missed";
                rhythmAnim.SetTrigger("display");
                audioManager.PlaySound("OffBeat");
                hasDisplayed = true;
            }
        }

        hasDisplayed = false;
        hasPressed = false;
    }

    void OnPlayerDie()
    {
        rhythmAnim.Play("RhythmIdle", -1, 0f);
    }
}
