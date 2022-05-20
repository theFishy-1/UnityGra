using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public bool shootingEnabled = true;

    [Header("Attach your bullet prefab")]
    public GameObject projectile;

    [Header("Different GameObjects")]
    public Transform Spawnpoint;    
    public Camera cam;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public GameObject bulletHole;
    public AudioSource source;
    public AudioClip clip;
    public GameObject hitmarker;
    public GameObject flash;
    public Recoil Recoil_Script;
    public AudioClip gunShotSound;
    public float pitchRandomization;
    public AudioSource sfx;


    [Header("Weapon Power")]
    public float shootForce;    
    public float upwardForce;
    public float spreadValue;
    public float recoilForce;

    [Header("Weapon Timing")]
    public float reloadTime;
    public float timeBetweenShooting; 
    public float timeBetweenShots;

    [Header("Weapon Customization")]
    public int bulletsPerShot;
    public bool allowButtonHold;
    public bool spreadable;
    
    [Header("Debug")]  
    bool shooting;
    bool readyToShoot;
    bool reloading;
    public bool allowInvoke = true;
    int bulletsShot;
    public bool aiming;
    private Vector3 savedPosition;
    private bool spreadable2;
    private bool turnedOn = false;
    private float minimumSpreadRange = 8f;

    [Header("Magazine")]
    public int bulletsLeft;   
    public int magAmmoCapacity;
    public int currentAmmo;
    public int maxAmmoSize;
      
    private void Awake()
    {       
        bulletsLeft = magAmmoCapacity;
        readyToShoot = true;       
    }

    public void Start() 
    {      
        Recoil_Script = transform.Find("cameraRecoil").GetComponent<Recoil>();
    }

    public void Update()
    {       
        HandleShooting();
        Aiming();

        if(Input.GetKeyDown(KeyCode.G) && turnedOn == false)
        {
            FlashLight();
            turnedOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.G) && turnedOn == true)
        {
            FlashLightOff();
            turnedOn = false;
        }
      
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

            //if (hit.collider.tag == "Bullet")
            //{
                //print("Bullet hit a bullet!");
            //}
            //else
            //{
                //GameObject obj = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                //obj.transform.position += Quaternion.LookRotation(hit.normal) * obj.transform.forward / 1000;
            //}
        }
        else
        {
            targetPoint = ray.GetPoint(100);
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

        //Sound
        sfx.clip = gunShotSound;
        sfx.pitch = 1 - pitchRandomization + Random.Range(-pitchRandomization, pitchRandomization);
        sfx.Play();

        if (currentBullet.GetComponent<CustomProjectiles>().hitTheTarget == true)
        {
            source.PlayOneShot(clip);
            HitActive();
            Invoke("HitDisable", 0.05f);
        }

        Instantiate(muzzleFlash, Spawnpoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
       
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;            
        }

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

        if (hit.distance <= minimumSpreadRange)
        {
            spreadable2 = false;
            if (spreadable)
                spreadValue = 0;
            Debug.Log("Not Spreadable");
        }
        else if (hit.distance >= minimumSpreadRange)
        {
            spreadable2 = true;
        }

        Recoil_Script.RecoilFire();
    }   

    public void Aiming()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            gameObject.transform.localPosition = new Vector3(-0.3f, 0.2f, 0.22f); //* aimTime;
            Camera.main.fieldOfView = 30.0f;
            Movement.mouseSensitivity = 0.2f;
            if (spreadable)
                spreadValue = 0;
            aiming = true;
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(0, 0, 0); //* aimTime;
            Camera.main.fieldOfView = 60.0f;
            Movement.mouseSensitivity = 0.5f;
            if (spreadable == true && spreadable2 == true) 
                spreadValue = 0.5f;
            aiming = false;
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

    private void FlashLight()
    {
        flash.SetActive(true);
    }

    private void FlashLightOff()
    {
        flash.SetActive(false);
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