using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class Boss : MonoBehaviour {

    public bool isDead = false;
    public EZObjectPool bossBullet;

    public static int offset;
    public int offsetIncrement = 3;

    public float delayBullet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void ShootThree()
    {
        if (isDead)
        {
            return;
        }
        GameObject go;
        int noOfBullets = 16;
        for (int i = 0; i < noOfBullets; i++)
        {
            bossBullet.TryGetNextObject(transform.position, Quaternion.identity, out go);
            //go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.Rotate(new Vector3(0, 0, 360 / noOfBullets * i));
        }
    }

    public void ShootOne()
    {
        if (isDead)
        {
            return;
        }
        GameObject go;
        bossBullet.TryGetNextObject(transform.position, Quaternion.identity, out go);
        go.transform.Rotate(new Vector3(0, 0, offset));
    }
}
