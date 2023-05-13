using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject BulletPrefab;
    public Transform Spawn;
    public float BulletSpeed = 10f;
    public float ShotPeriod = 0.2f;

    public PlayerController playerController;
    public GameObject Flash;

    private float _timer;

    private void Update() {

        _timer += Time.deltaTime;
        if(_timer > ShotPeriod) {
            if (playerController.FirePressed) {
                _timer = 0;
                GameObject newBullet = Instantiate(BulletPrefab, Spawn.position, Spawn.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = Spawn.forward * BulletSpeed;

                Flash.SetActive(true);
                Invoke(nameof(MuzzleFlash), 0.1f);
            }
        }
    }

    public void MuzzleFlash() {
        Flash.SetActive(false);
    }

}
