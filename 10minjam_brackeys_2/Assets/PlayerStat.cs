using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public int exp;
    public AudioSource audio;
    public BeatReader beatReader;
}

public class PlayerStat : MonoBehaviour {

    public int exp = 0;
    public int maxExp = 10;

    public static PlayerStat Instance;

    public List<Level> levels;
    public int levelIndex = 0;

    public Image expIndicator;

    PlayerController2D player;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
        player = PlayerController2D.Instance;
        maxExp = levels[levelIndex].exp;
        player.PlayerDie += OnPlayerDie;
        //UpdateExpIndicator();
        Restart();
    }

    private void Update()
    {
        float ratio = (float)exp / maxExp;
        if (expIndicator.fillAmount < ratio && ratio > 0f)
        {
            expIndicator.fillAmount += Time.deltaTime * 3f;
        }
    }


    public void EatFood()
    {
        exp++;
        //UpdateExpIndicator();
        if (exp >= maxExp)
        {
            // upgrade
            //some effects
            StartCoroutine(UpgradeDelay());
        }
        //
    }

    public void UpdateExpIndicator()
    {
        expIndicator.fillAmount = (float)exp / maxExp;
    }

    IEnumerator UpgradeDelay()
    {
        yield return new WaitForSeconds(0.15f);
        Upgrade();
    }

    void Upgrade()
    {
        switch (levelIndex)
        {
            case 0:
                StartCoroutine(DrumVolumeUp());
                //levels[1].audio.volume = 0.28f;
                levels[1].beatReader.enabled = true;
                player.anim.SetFloat("scale", 1.2f);
                player.upgradeEffect.Play();
                // more bullets
                break;
            case 1:
                player.numberOfBulletMultiplier = 2;
                player.anim.SetFloat("scale", 1.4f);
                player.upgradeEffect.Play();
                // drum
                break;
            case 2:
                StartCoroutine(SnareVolumeUp());
                levels[2].audio.volume = 0.226f;
                levels[2].beatReader.enabled = true;
                EnemySpawner.Instance.foodDrop = 0;
                EnemySpawner.Instance.degeneratePercent = 0.95f;
                player.anim.SetFloat("scale", 1.7f);
                player.upgradeEffect.Play();
                break;
            case 3:
                EnemySpawner.Instance.foodDrop = 0;
                // win
                break;
            default: break;
        }

        AudioManager.instance.PlaySound("Upgrade");

        levelIndex = Mathf.Clamp(levelIndex + 1, 0, levels.Count - 1);
        Debug.Log("Upgraded to level " + levelIndex);
        exp = 0;
        maxExp = levels[levelIndex].exp;
        UpdateExpIndicator();
    }

    IEnumerator DrumVolumeUp()
    {
        while (levels[1].audio.volume < 0.28f)
        {
            levels[1].audio.volume += Time.deltaTime / 2;
            yield return null;
        }
    }


    IEnumerator SnareVolumeUp()
    {
        while (levels[2].audio.volume < 0.226f)
        {
            levels[2].audio.volume += Time.deltaTime / 2;
            yield return null;
        }
    }

    IEnumerator VolumeDown()
    {
        float vol1Decrease = levels[1].audio.volume / .5f;
        float vol2Decrease = levels[2].audio.volume / .5f;
        while (levels[1].audio.volume > 0 || levels[2].audio.volume > 0)
        {
            levels[1].audio.volume = Mathf.Clamp01(levels[1].audio.volume - vol1Decrease * Time.deltaTime);
            levels[2].audio.volume = Mathf.Clamp01(levels[2].audio.volume - vol2Decrease * Time.deltaTime);
            yield return null;
        }
    }


    public void Restart()
    {
        levelIndex = 0;
        maxExp = levels[levelIndex].exp;
        exp = 0;
        UpdateExpIndicator();
        player.numberOfBulletMultiplier = 1;
        //levels[1].audio.volume = 0;
        levels[1].beatReader.enabled = false;
        //levels[2].audio.volume = 0;
        StartCoroutine(VolumeDown());
        levels[2].beatReader.enabled = false;
    }

    void OnPlayerDie()
    {
        Restart();
    }
}
