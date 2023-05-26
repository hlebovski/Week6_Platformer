using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject HitEffect;

    private void Start() {
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter(Collision collision) {
        Instantiate(HitEffect, transform.position - Vector3.forward/2, Quaternion.identity);
        Destroy(gameObject);
    }
}
