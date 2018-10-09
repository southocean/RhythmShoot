using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public bool speedDecrease = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (speedDecrease)
        {
            if (speed > 1)
            {
                speed -= Time.deltaTime;
            }
        }
        transform.position += speed * transform.right * Time.deltaTime;
	}

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
