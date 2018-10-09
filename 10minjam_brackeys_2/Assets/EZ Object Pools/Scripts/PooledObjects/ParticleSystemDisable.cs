using UnityEngine;
using EZObjectPools;
using System.Collections;

[AddComponentMenu("EZ Object Pool/Pooled Objects/Particle System Disable")]
public class ParticleSystemDisable : PooledObject
{
    public ParticleSystem Particles;

    void Awake()
    {
        if (Particles == null)
            Particles = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        if (Particles == null)
        {
            Debug.LogError("ParticleSystemDisable " + gameObject.name + " could not find any particle systems!");
        }
        else
        {
            StartCoroutine(DisableAfterPlaying());
        }
    }

    //void Update()
    //{
    //    if (!Particles.IsAlive())
    //    {
    //        transform.parent = ParentPool.transform;
    //        gameObject.SetActive(false);
    //    }
    //}

    IEnumerator DisableAfterPlaying()
    {
        yield return new WaitForSeconds(Particles.main.duration);

        gameObject.SetActive(false);
    }
}
