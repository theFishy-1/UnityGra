using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    //Other
    public GameObject projectile;
    public Transform Spawnpoint;    
    public Camera cam;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public GameObject bulletHole;

    //Ints
    public int magAmmoCapacity;
    public int bulletsPerShot;

    int bulletsLeft;
    int bulletsShot;

    //Floats
    public float shootForce;
    public float timeBetweenShots;
    public float timeBetweenShooting;
    public float reloadTime;

    //Recoil & Spread
    public float upwardForce;
    public float spreadValue;
    //public Rigidbody playerRb;
    public float recoilForce;

    //Bools
    bool shooting;
    bool readyToShoot;
    bool reloading;
    public bool allowButtonHold;
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magAmmoCapacity;
        readyToShoot = true;
    }

    public void Update()
    {       
        HandleShooting();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerShot + " / " + magAmmoCapacity / bulletsPerShot);        
    }

    void HandleShooting()
    {
        if (allowButtonHold) shooting = Input.GetButton("Fire1");
        else shooting = Input.GetButtonDown("Fire1");

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magAmmoCapacity && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }                  
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;

            if (hit.collider.tag == "Bullet")
            {
                print("Bullet hit a bullet!");
            }
            else
            {
                GameObject obj = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                obj.transform.position += Quaternion.LookRotation(hit.normal) * obj.transform.forward / 1000;
            }
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        //Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        

        Vector3 directionWithoutSpread = targetPoint - Spawnpoint.position;

        float x = Random.Range(-spreadValue, spreadValue);
        float y = Random.Range(-spreadValue, spreadValue);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(projectile, Spawnpoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, Spawnpoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletsPerShot && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

        
    }   

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magAmmoCapacity;
        reloading = false;
    }
}