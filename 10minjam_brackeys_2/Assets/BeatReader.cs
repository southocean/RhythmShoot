using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class BeatReader : MonoBehaviour {

    public AudioSource source;
    public int numberOfNotes;
    public float beatDuration = 250;
    public float currentPlayHead;
    public float lastPlayHead;
    public bool isBeat;
    public List<int> notesOnBeat;
    public int lastNote;
    public bool noteDetected = false;

    public FollowPlayer score;
    public bool updateScore = false;
    public int scoreSkip = 0;

    public enum PATTERN { ONE, TWO, THREE, FOUR};

    public PATTERN type;

    public bool spawningNote = false;
    public float animTime = 1000;
    int lookAhead;
    public EZObjectPool musicNotePool;
    public Transform SpawnPoint;

    public bool usingEvent = false;
    public bool usingNotesOnBeat = true;
    public event Action BeatDetected;

    public PlayerController2D player;


    public bool printDebug = false;

    void Start () {
        lookAhead = (int)(animTime / beatDuration);
        source = GetComponent<AudioSource>();
        if (!source)
        {
            Debug.Log("Cannot find audio source, need to assign or put on an object with audio source");
        }
        numberOfNotes = 16000 / (int)beatDuration;
    }
	
	void Update () {
        currentPlayHead = source.time * 1000;
        int currentNote = (int)(currentPlayHead / beatDuration);
        isBeat = false;
        if (currentNote > lastNote)
        {
            noteDetected = true;
            lastNote = currentNote;
            if (!usingNotesOnBeat || (usingNotesOnBeat && notesOnBeat.Contains(currentNote)))
            {
                if (printDebug)
                    Debug.Log("Beat here");
                if (usingEvent)
                {
                    BeatDetected();
                }
                PlayerShoot();
            }
            if (!usingNotesOnBeat || (usingNotesOnBeat && notesOnBeat.Contains(currentNote + lookAhead)))
            {
                SpawnNote();
            }
        }
        else if (currentNote == lastNote)
        {
            noteDetected = false;
        }
        else if (currentNote < lastNote) // that means we looped back
        {
            currentNote = 0;
            lastNote = 0;
            noteDetected = true;
            if (!usingNotesOnBeat || (usingNotesOnBeat && notesOnBeat.Contains(currentNote)))
            {
                if (printDebug)
                    Debug.Log("Beat here");
                if (usingEvent)
                {
                    BeatDetected();
                }
                PlayerShoot();
            }
            if (!usingNotesOnBeat || (usingNotesOnBeat && notesOnBeat.Contains(currentNote + lookAhead)))
            {
                SpawnNote();
            }
        }

    }

    public void PlayerShoot()
    {
        switch(type)
        {
            case PATTERN.ONE:
                player.ShootOne();
                break;
            case PATTERN.TWO:
                player.ShootTwo();
                break;
            case PATTERN.THREE:
                player.ShootThree();
                break;
            case PATTERN.FOUR:
                player.ShootFour();
                break;
        }
        if (updateScore)
        {
            scoreSkip = (scoreSkip + 1) % 2;
            if (scoreSkip == 0)
            {
                score.Peak();
            }
        }
    }

    void SpawnNote()
    {

        if (spawningNote)
        {
            StartCoroutine(SpawnNoteDelay());
        }
    }

    IEnumerator SpawnNoteDelay()
    {
        yield return new WaitForSeconds((beatDuration - animTime) / 1000f);
        //Debug.Log("Note spawned at " + SpawnPoint.position);
        musicNotePool.TryGetNextObject(SpawnPoint.position, Quaternion.identity);
    }
}
