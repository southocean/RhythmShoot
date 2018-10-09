using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class PlayerController2D : MonoBehaviour {

    public float speed = 10;

    public static Vector3 pos;
    
    public event Action PlayerDie;

    public static bool isDead = false;

    public EZObjectPool bulletPool;
    public EZObjectPool bulletPool2;

    public static float offset;
    public float offsetIncrease = 2;
    public static float offset2;
    public float offsetIncrease2 = 10;
    public Animator anim;
    public ParticleSystem upgradeEffect;

    public int numberOfBulletMultiplier = 1;

    public int exp = 0;

    PlayerStat stat;

    public static PlayerController2D Instance;

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
    void Start () {
        stat = PlayerStat.Instance;

    }
	
	// Update is called once per frame
	void Update () {
        if (isDead)
        {
            return;
        }
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    offset = (offset + 45*Time.deltaTime) % 360;
        //}

        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");

        transform.position += new Vector3(hMove, vMove) * speed * Time.deltaTime;
        pos = transform.position;
    }

    public void ShootOne()
    {
        if (isDead)
        {
            return;
        }
        GameObject go;
        int noOfBullets = 4;
        offset = (offset + offsetIncrease) % 360;
        for (int i = 0; i < noOfBullets; i++)
        {
            bulletPool.TryGetNextObject(transform.position, Quaternion.identity, out go);
            //go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.Rotate(new Vector3(0, 0, 360 / noOfBullets * i + offset));
        }
    }

    public void ShootTwo()
    {
        if (isDead)
        {
            return;
        }
        GameObject go;
        int noOfBullets = 4 * numberOfBulletMultiplier;
        for (int i = 0; i < noOfBullets; i++)
        {
            bulletPool.TryGetNextObject(transform.position, Quaternion.identity, out go);
            //go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.Rotate(new Vector3(0, 0, 360/noOfBullets * i + offset));
        }
    }

    public void ShootThree()
    {
        if (isDead)
        {
            return;
        }
        GameObject go;
        int noOfBullets = 8 * numberOfBulletMultiplier;
        for (int i = 0; i < noOfBullets; i++)
        {
            bulletPool.TryGetNextObject(transform.position, Quaternion.identity, out go);
            //go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.Rotate(new Vector3(0, 0, 360 / noOfBullets * i));
        }
    }

    public void ShootFour()
    {
        if (isDead)
        {
            return;
        }
        GameObject go;
        int noOfBullets = 2;
        offset2 = (offset2 + offsetIncrease2) % 360;
        for (int i = 0; i < noOfBullets; i++)
        {
            bulletPool2.TryGetNextObject(transform.position, Quaternion.identity, out go);
            //go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.Rotate(new Vector3(0, 0, 360 / noOfBullets * i + offset2));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isDead)
        {
            StopAllCoroutines();
            isDead = true;
            anim.SetTrigger("die");
            anim.SetFloat("scale", 1.0f);
            PlayerDie();
            StartCoroutine(Restart());
        }
        else if (collision.CompareTag("Food") && !isDead)
        {
            anim.SetTrigger("eat");
            stat.EatFood();
            //collision.gameObject.SetActive(false);
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3f);
        isDead = false;
        anim.SetTrigger("respawn");
        //StartCoroutine(SpawnEnemy());
    }

    public void SpeedUp()
    {
        StartCoroutine(SpeedUpDelay(0.2f));
    }

    IEnumerator SpeedUpDelay(float delay)
    {
        speed += 5f;
        Debug.Log("Sped up to " + speed);
        yield return new WaitForSeconds(delay);
        speed -= 5f;
    }
}

