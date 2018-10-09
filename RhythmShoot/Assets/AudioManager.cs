using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 1f)] 
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float ranomPitch = 0.1f;

    public bool loop = false;
    public AudioManager.SOUND type = AudioManager.SOUND.EFFECT;

    public AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        //source.volume = Mathf.Clamp01(volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)));
        //source.pitch = Mathf.Clamp(pitch * (1 + Random.Range(-ranomPitch / 2f, ranomPitch / 2f)), .8f, 1.2f);
        if (source == null)
        {
            Debug.Log("Cannot find the source of sound clip named " + name);
            return;
        }
        source.Play();
        //source.GetComponent<AudioSource>().Play();
    }

    public void PlayMute()
    {
        Play();
        source.volume = 0;
        source.pitch = Mathf.Clamp01(pitch * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)));
        source.Play();
    }

    public void DecreaseVolume(float amount)
    {
        source.volume = Mathf.Clamp01(volume - amount);
    }

    public void IncreaseVolume(float amount)
    {
        source.volume = Mathf.Clamp01(volume + amount);
    }

    public void SetVolume(float value)
    {
        //source.volume = Mathf.Clamp01(value);
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour {

    public enum SOUND { MUSIC, EFFECT};

    public static AudioManager instance;

    //private static bool keepFadingIn;
    //private static bool keepFadingOut;
    
    public float fadingSpeed = 0.01f;
    
    
    public float masterVolume;
    public float musicVolume;
    public float effectVolume;

    public List<AudioSource> musics;
    public List<AudioSource> sfxs;

    //public bool isInTitle = false;

    private void Awake()
    {
       

        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField]
    Sound[] sounds;

    private void Start()
    {
        UpdateSoundOptions();
        //PlayerPrefs.SetInt("levelReached", 1);
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            AudioSource _source = _go.AddComponent<AudioSource>();
            sounds[i].SetSource(_source);
            if (sounds[i].type == SOUND.MUSIC)
            {
                musics.Add(_source);
            }
            else
            {
                sfxs.Add(_source);
            }
        }

        UpdateVolumeAllSounds();
        Debug.Log("All sounds are updated in Start()");

        //PlaySound("Music");
        Debug.Log("Playing music");
    }

    public void UpdateSoundOptions()
    {
        //masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.4f);
        //musicVolume = PlayerPrefs.GetFloat("musicVolume", .4f);
        //effectVolume = PlayerPrefs.GetFloat("effectVolume", .5f);
    }

    public void PlaySound(string _name)
    {
        //Debug.Log("Playing sound " + _name);
        int i;
        for (i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                break;
            }
        }

        // Sound not found with _name
        if (i >= sounds.Length)
        {
            Debug.LogWarning("AudioManage: Sound " + _name + " not found in list.");
            return;
        }

        //sounds[i].Play();

        if (sounds[i].type == SOUND.MUSIC)
        {
            sounds[i].Play();
        }
        else if (sounds[i].type == SOUND.EFFECT)
        {
            sounds[i].Play();
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // Sound not found with _name
        Debug.LogWarning("AudioManage: Sound " + _name + " not found in list.");
    }

    AudioSource GetSource(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                return sounds[i].source;
            }
        }

        // Sound not found with _name
        Debug.LogWarning("AudioManage: Sound " + _name + " not found in list.");
        return null;
    }

    public void FadeInOutMusic(string s1, string s2)
    {
        AudioSource a1 = GetSource(s1);
        AudioSource a2 = GetSource(s2);
        if (s1 == null ||s2 == null)
        {
            Debug.Log("Cannot find sound with such names.");
            return;
        }
        StartCoroutine(FadeInOut(a1, a2, 5f));
    }

    IEnumerator FadeInOut(AudioSource a1, AudioSource a2, float duration)
    {
        float t = duration;
        float initialVolume = a1.volume;
        a2.volume = 0;
        a2.Play();
        float delay = 0.05f;
        float decreaseRate = initialVolume / duration / delay; // e.g. decrease 0.7 in 2 sec -> 0.35 per sec, will be scaled down by Time.deltaTime
        while (t > 0)
        {
            t = t - delay;
            a1.volume -= decreaseRate;
            a2.volume += decreaseRate;
            yield return new WaitForSeconds(delay);
        }
        a1.Stop();
    }

    public void FadeOut(string _name, float duration)
    {
        StartCoroutine(FadeOutHelper(GetSource(_name), duration));
    }

    IEnumerator FadeOutHelper(AudioSource a1, float duration)
    {
        float t = duration;
        float initialVolume = a1.volume;
        float delay = 0.1f;
        //float decreaseRate = initialVolume / duration / delay;
        while (t > 0)
        {
            t -= Time.deltaTime;
            a1.volume = Mathf.Lerp(initialVolume, 0, 1 - t / duration);
            yield return new WaitForSeconds(delay);
        }
        a1.Stop();
    }

    public void ChangeMasterVolume(float volume)
    {
        //Debug.Log("Changed master volume from " + masterVolume + " to " + volume);
        masterVolume = volume;
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        UpdateVolumeAllSounds();
        //Debug.Log("Now master volume is " + PlayerPrefs.GetFloat("masterVolume", 1f));
    }

    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        UpdateVolumeAllSounds();
        //Debug.Log("Now musicVolume is " + PlayerPrefs.GetFloat("musicVolume", 1f));
    }

    public void ChangeEffectVolume(float volume)
    {
        effectVolume = volume;
        PlayerPrefs.SetFloat("effectVolume", effectVolume);
        UpdateVolumeAllSounds();
        //Debug.Log("Now effectVolume is " + PlayerPrefs.GetFloat("effectVolume", 1f));
    }


    public void UpdateVolumeAllSounds()
    {
        for (int i = 0; i < musics.Count; i++)
        {
            musics[i].volume = masterVolume * musicVolume;
        }
        for (int i = 0; i < sfxs.Count; i++)
        {
            if (i < 4)
            {
                sfxs[i].volume = masterVolume * effectVolume * 0.5f;
            }
            else
            {

                sfxs[i].volume = masterVolume * effectVolume;
            }
        }
        //foreach (var source in musics)
        //{
        //    source.volume = masterVolume * musicVolume;
        //}

        //foreach (var source in sfxs)
        //{
        //    source.volume = masterVolume * effectVolume;
        //}


        //for (int i = 0; i < instance.sounds.Length; i++)
        //{
        //    if (sounds[i].type == SOUND.EFFECT)
        //    {
        //        sounds[i].SetVolume(masterVolume * effectVolume);
        //    }
        //    else
        //    {
        //        sounds[i].SetVolume(masterVolume * musicVolume);
        //    }
        //}
    }
}
