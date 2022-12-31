using UnityEngine;
using UnityEngine.Events;

public class BulletController : MonoBehaviour
{
    public GameObject bulletOwner { get; private set; }
    public Vector3 initialPosition { get; private set; }
    public Vector3 initialDirection { get; private set; }
    //public Vector3 bulletVelocity { get; private set; }

    public UnityAction delegateonShootAction;

    // the bullet live time 
    public float maxLifeTime = 5f;
    public float speed = 2f;
    private Vector3 _velocity;

    // variables for the collision of the bullet 
    // bullet transform
    public Transform bulletPoint;
    // bullet tip transform
    public Transform bulletTipPoint;
    public float bulletTipRadius = 0.01f;
    // when the bullet tip touch all the layer, the collision happens
    public LayerMask collitableLayers = -1;
    // the initial postion from the last shooting 
    private Vector3 _lastBulletStartPosition;

    // variables for bullet hit special impact 
    public GameObject impactSource;
    public float impactLifeTime = 5f;
    public float impactOffset = 0.1f;

    // define the power of the bullets 
    public float bulletDmg = 20f;

    public void shoot(WeaponController controller)
    {
        // 
        bulletOwner = controller.weaponOwner;
        // the initial position of bullet will be the player's position 
        initialPosition = transform.position;
        initialDirection = transform.forward;
        //bulletVelocity = controller.bulletVelocityFromWeaponController;

        if (delegateonShootAction != null)
            delegateonShootAction.Invoke();
    }

    private void OnEnable()
    {
        // calling the unity action onShoot (delegate function)
        delegateonShootAction += onShootFunc;
        Destroy(gameObject, maxLifeTime);
    }

    private void onShootFunc()
    {
        // assign the last bullet shooting start position
        _lastBulletStartPosition = bulletPoint.position;
        // bullet speed control 
        _velocity += transform.forward * speed;
    }

    private void Update()
    {
        // bullet move 
        transform.position += _velocity * Time.deltaTime;
        // bullet direction 
        transform.forward = _velocity.normalized;
        // bullet hit detection
        RaycastHit closestHit = new RaycastHit();
        closestHit.distance = Mathf.Infinity;
        bool hitFound = false;

        /* Hitting affect*/
        // Physics.SphereCastAll return type: RaycastHit[] 
        // Physics.SphereCast return type: RaycastHit and bool 
        // the direction vector3 need to be normalized
        RaycastHit[] hits = Physics.SphereCastAll(_lastBulletStartPosition, bulletTipRadius, (bulletTipPoint.position - _lastBulletStartPosition).normalized, (bulletTipPoint.position - _lastBulletStartPosition).magnitude, collitableLayers, QueryTriggerInteraction.Collide);
      
        foreach (var hit in hits)
        {
            if (isHitValid(hit) && hit.distance < closestHit.distance)
            {
                hitFound = true;
                closestHit = hit;
            }
        }

        if (hitFound == true)
        {
            // check when the gun touch the wall or not, if the gun is touching the wall. the shooting will be banned 
            if (closestHit.distance <= 0)
            {
                closestHit.point = bulletPoint.position;
                // shooting back
                closestHit.normal = -transform.forward;
            }

            // call the OnHit 
            OnHit(closestHit.point, closestHit.normal, closestHit.collider);
        }
    }

    // if the player hit the enemy
    private void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        // get the damageble component
        Damagable damagable = collider.GetComponent<Damagable>();

        if (damagable != null)
        {
            // passing bulletDmg to inflict damage
            damagable.inflictDamage(bulletDmg);
        }
        
        // the bullet hitting impact 
        if (impactSource != null)
        {
            GameObject impactInstance = Instantiate(impactSource, point + normal * impactOffset, Quaternion.LookRotation(normal));
            if (impactLifeTime > 0)
                Destroy(impactInstance, impactLifeTime);
        }

        Destroy(gameObject);
    }

    private bool isHitValid(RaycastHit hit)
    {
        // it means the bullet did hit anything
        if(hit.collider.isTrigger)
            return false;
        return true;
    }

}
