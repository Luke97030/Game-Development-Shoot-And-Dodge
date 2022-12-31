using UnityEngine;


public class PlayerWeaponManager : MonoBehaviour
{
    public WeaponController weaponController = new WeaponController();
    public Transform weaponParentTransform;
    public Hp _hp;

    //private bool isOverLayer;
    // Start is called before the first frame update
    void Start()
    {
        addWeapon(weaponController);     
    }

    private void Update()
    {
        WeaponController currentWeapon = weaponController;

        // if the currentWeapon is not null
        // player collider when the environment layer, we cannot shoot
        if (currentWeapon && _hp.playerIsIT)
        {
            //Debug.Log("Player Can shoot");
            // when the mouse be lift clicked
            // check the shooting CD with Time by calling the handleShootingInput function in the WeaponController 
            currentWeapon.handleShootingInput(Input.GetMouseButton(0));
        }
    }

    private bool addWeapon(WeaponController newweaponPreb)
    {
        if (weaponController == null)
        {
            return false;
        }

        // create a weapon instance by cloning the transform of parent and attaching with the prefab
        WeaponController weaponInstance = Instantiate(newweaponPreb, weaponParentTransform);
        // initialize the weaponInstance position and rotation
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;
        // this script will be attached with Play Game object, so the gameObject refer to Player
        weaponInstance.weaponOwner = gameObject;
        // the WeaponController script was attached with Weapon Prefab
        weaponInstance.weaponPrefab = newweaponPreb.gameObject;
        weaponInstance.displayWeapon(false);


        return false;

    }
}
