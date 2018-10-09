using UnityEngine;
using EZObjectPools;
using System.Collections;

[AddComponentMenu("EZ Object Pools/Pooled Objects/Timed Disable")]
public class TimedDisable : PooledObject
{


    float timer = 0;
    public float DisableTime;

    void OnEnable()
    {
        StartCoroutine(DisableAfterPlaying());
    }

    //void Update()
    //{
    //    timer += Time.deltaTime;

    //    if (timer > DisableTime)
    //    {
    //        transform.parent = ParentPool.transform;
    //        gameObject.SetActive(false);
    //    }
    //}


    IEnumerator DisableAfterPlaying()
    {
        yield return new WaitForSeconds(DisableTime);

        gameObject.SetActive(false);
    }
}
