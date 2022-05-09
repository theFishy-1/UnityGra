using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public bool shootingEnabled = true;

    [Header("Attach your bullet prefab")]
    public GameObject projectile;

    //Other
    public Transform Spawnpoint;    
    public Camera cam;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public GameObject bulletHole;
    public GameObject hitmarker;
    public AudioSource source;
    public AudioClip clip;
   
    //public int magAmmoCapacity2;
    public int bulletsPerShot;
    //private int magAmmoCapacityOriginal;

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

    int bulletsShot;

    public int bulletsLeft;   
    public int magAmmoCapacity = 10;
    public int currentAmmo;
    public int maxAmmoSize = 100;

    private void Awake()
    {
        bulletsLeft = magAmmoCapacity;
        readyToShoot = true;
        hitmarker.SetActive(false);
        //magAmmoCapacityOriginal = magAmmoCapacity;       
    }

    public void Update()
    {       
        HandleShooting();
        
        ammunitionDisplay.SetText(bulletsLeft / bulletsPerShot + " / " + magAmmoCapacity / bulletsPerShot + " | " + currentAmmo / bulletsPerShot + " / " + maxAmmoSize / bulletsPerShot);        
    }

    void HandleShooting()
    {
        if (allowButtonHold) shooting = Input.GetButton("Fire1");
        else shooting = Input.GetButtonDown("Fire1");

        if (Input.GetKeyDown(KeyCode.R) && magAmmoCapacity > 0 && !reloading) Reload(); //bulletsLeft < magAmmoCapacity

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerShot;

            Shoot();
        }                  
    }

    private void Shoot()
    {
        if (!shootingEnabled) return;

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
        float z = Random.Range(-spreadValue, spreadValue);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, z);

        GameObject currentBullet = Instantiate(projectile, Spawnpoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        if (currentBullet.GetComponent<CustomProjectiles>()) currentBullet.GetComponent<CustomProjectiles>().activated = true;

        Instantiate(muzzleFlash, Spawnpoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--; //bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;            
        }

        if (bulletsShot > 0 && bulletsLeft > 0)  //bulletsShot < bulletsPerShot
            Invoke("Shoot", timeBetweenShots);  
        
        if (hit.collider.tag == "Enemy")
        {
            source.PlayOneShot(clip);
            HitActive();            
            Invoke("HitDisable", 0.05f);
        }
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
        //magAmmoCapacityOriginal = magAmmoCapacity2;        
        //magAmmoCapacity -= magAmmoCapacityOriginal - bulletsLeft;
        //bulletsLeft += magAmmoCapacityOriginal - bulletsLeft;

        int reloadAmount = magAmmoCapacity - bulletsLeft;
        reloadAmount = (currentAmmo - reloadAmount) > -0 ? reloadAmount : currentAmmo;
        bulletsLeft += reloadAmount;
        currentAmmo -= reloadAmount;

        reloading = false;
    }

    public void AddAmmo(int ammoAmount)
    {
        currentAmmo += ammoAmount;
        if(currentAmmo > maxAmmoSize)
        {
            //currentAmmo = maxAmmoSize;
        }
    }

    private void HitActive()
    {
        hitmarker.SetActive(true);
    }

    private void HitDisable()
    {
        hitmarker.SetActive(false);
    }

    #region Setters

    public void SetShootForce(float v)
    {
        shootForce = v;
    }
    public void SetUpwardForce(float v)
    {
        upwardForce = v;
    }
    public void SetFireRate(float v)
    {
        float _v = 2 / v;
        timeBetweenShooting = _v;
    }
    public void SetSpread(float v)
    {
        spreadValue = v;
    }
    public void SetMagazinSize(float v)
    {
        int _v = Mathf.RoundToInt(v);
        magAmmoCapacity = _v;
    }
    public void SetBulletsPerTap(float v)
    {
        int _v = Mathf.RoundToInt(v);
        bulletsPerShot = _v;
    }

    #endregion
}