using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // variabels and function for displaying weapon
    public GameObject weaponRoot;

    public bool isWeaponActive { get; private set; }
    public GameObject weaponOwner { get; set; }
    public GameObject weaponPrefab { get; set; }

    // variables for controller the weapon fire
    public Transform firePoint;
    //public GameObject firePoint;
    public GameObject firePrefab;
    public BulletController bulletPrefab;
    // one second can shoot 5 bullets 
    private float shootingCD = 0.5f;
    // check the fire interval 
    private float _timer = 0;
    //private AudioSource gunAudio;
    //public AudioClip fireClip;
    //public AudioClip reloadClip;

    public void displayWeapon(bool display)
    {
        weaponRoot.SetActive(display);
        isWeaponActive = display;
    }

    //bool isOverLayer = false;
    private void Update()
    {
        _timer += Time.deltaTime;
    }

    public bool handleShootingInput(bool shot)
    {
        if (shot)
        {
            return tryShoot();
        }
        return false;
    }

    public bool tryShoot()
    {
        if(_timer > shootingCD && Input.GetMouseButton(0))
        {
            // if the firePrefab is assigned, instantiate the prefab
            if (firePrefab != null)
            {
                // Instantiate: Clones the object original and returns the clone
                GameObject firePrefabInstance = Instantiate(firePrefab, firePoint.position, firePoint.rotation, firePoint.transform);
                Destroy(firePrefabInstance, 2);               
            }

            //// if the bulletPrefab is assigned, instantiate the prefab
            if (bulletPrefab != null)
            {
                //Vector3 shootDirection = firePoint.forward;  
                // do not need the bulletpoint, it should be same position, direction and transfer as firepoint
                BulletController bulletPrefabInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bulletPrefabInstance.shoot(this);
            }


            //Debug.Log("Shooting");
            _timer = 0;
            return true;
        }

        return false;
    }
}
