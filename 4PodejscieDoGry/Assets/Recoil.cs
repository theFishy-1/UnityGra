using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private Shooting aiming_script;
    [SerializeField] private Shooting aiming_script2;
    [SerializeField] private Shooting aiming_script3;
    [SerializeField] private Shooting aiming_script4;

    private bool isAiming;

    private Vector3 currentRotation;

    private Vector3 targetRotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;
    [SerializeField] private float aimRecoilZ;
    
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    void Start()
    {
        
    }
    void Update()
    {
        if (aiming_script.isActiveAndEnabled == true)
            isAiming = aiming_script.aiming;
        else if (aiming_script.isActiveAndEnabled == false && aiming_script2.isActiveAndEnabled == true)
            isAiming = aiming_script2.aiming;
        else if (aiming_script.isActiveAndEnabled == false && aiming_script2.isActiveAndEnabled == false && aiming_script3.isActiveAndEnabled == true)
            isAiming = aiming_script3.aiming;
        else if (aiming_script.isActiveAndEnabled == false && aiming_script2.isActiveAndEnabled == false && aiming_script3.isActiveAndEnabled == false && aiming_script4.isActiveAndEnabled == true)
            isAiming = aiming_script4.aiming;

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        if(isAiming) targetRotation += new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ));
        else targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
