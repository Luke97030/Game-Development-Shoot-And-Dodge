using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private Rigidbody _rigidbody;
    private NavMeshAgent _navMeshAgent;

    private bool _isChase;

    // enemy animation 
    private Animator _animator;

    // the box collider in front of melee 
    public BoxCollider meleeArea;
    private float _targetRadius;
    private float _targetRange;
    private bool _isAttack;


    // enemy type
    public enum Type { Melee, Range }
    public Type enemyType;
    // using the same bullet game object 
    public GameObject missile;


   // enemy hp
    private Hp _hp;
    public Text whoisITTxt;

    //// enemy AI 
    //[Header("Sensors")]
    //// the length of sensors
    //public float sensorLength = 3f;
    //// from the center of the enemy to the sensor starting point
    ////public float frontSensorPos = 0.5f;
    //public float frontSidesensorPos = 0.2f;
    //public float frontSensorAngle = 30f;
    //Vector3 frontSensorPos = new Vector3(0, 0.2f, 0.5f);
    //private bool avoiding = false;
    //public BoxCollider boxCollider;
            
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _hp = GetComponentInChildren<Hp>();
        Invoke("delayStart", 1);
        //DontDestroyOnLoad(this.gameObject);
    }

    private void delayStart()
    {
        _isChase = true;
        if (_animator)
        {
            _animator.SetBool("isWalk", true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // check the NavMeshAgent is be checked or not under the Melee component
        // we only let enemy turnning towards the player when the enemy is not dead
        // if (_navMeshAgent.enabled && !_hp.isDead)
        if (_navMeshAgent.enabled)
        {
            // if the enemy is IT, enemy run towards player
            if (_hp.enemyIsIT == true && _hp.playerIsIT == false)
            {
                whoisITTxt.text = "Bot is IT"; 
                //Debug.Log();
                Vector3 targetPos = target.position;
                _navMeshAgent.SetDestination(targetPos);
                _navMeshAgent.isStopped = !_isChase;
            }
            else if (_hp.enemyIsIT == false && _hp.playerIsIT == true)          // if player is IT, enemy run away from player
            {
                //Debug.Log("Player is IT in Ememy");
                whoisITTxt.text = "Player is IT";
                Vector3 newPos = new Vector3();

                //Sensors();
                // the distance between enemy and player each frame
                float enemyToPlayerUpdatingDis = (transform.position - target.transform.position).sqrMagnitude;
                //Debug.Log("1   " + enemyToPlayerUpdatingDis);
                // the distance enemy AI need to reach when we got hitted
                float enemyToPlayerGoalDis = (transform.position - target.transform.position).sqrMagnitude +1;
                //Debug.Log("2   " + enemyToPlayerGoalDis);

                // keep updating the distane
                if (enemyToPlayerUpdatingDis < enemyToPlayerGoalDis)
                {
                    //Debug.Log("enemyToPlayerUpdatingDis    " + enemyToPlayerUpdatingDis);
                    //Debug.Log("enemyToPlayerGoalDis    " + enemyToPlayerGoalDis);
                    Vector3 dirToPlayer = transform.position - target.transform.position;
                    //Debug.Log("transform.position    " + transform.position);
                    //Debug.Log("double transform.position   " + transform.position*2);
                    newPos = transform.position + dirToPlayer;
                    // Adding more distance between enemy AI and player and set the distance
                    //Debug.Log("new Pos   " + newPos);
                    _navMeshAgent.SetDestination(newPos);

                    // adding sense to enery, so when it is close to the wall or corner, turning around and find another path
                    
                }

            }
            _navMeshAgent.isStopped = !_isChase;
        }
    }

    private void FixedUpdate()
    {
        if (_isChase)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        Targeting();
    }

    // the distance detection between enemy and player. 
    private void Targeting()
    {
        _targetRadius = 0.5f;
        if (enemyType == Type.Melee)
            // for the Melee enemy the range is 2f 
            _targetRange = 2f;
        else if (enemyType == Type.Range)
            // for the Range enemy t0he range is 40f 
            _targetRange = 40f;

        // The collision happens with player layer
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _targetRadius, transform.forward, _targetRange, LayerMask.GetMask("Player"));

        // enemy can chasing and shoot 
        //   if (hits.Length > 0 && _isAttack == false && !_hp.isDead && whoisITTxt.text == "Bot is IT")
        if (hits.Length > 0 && _isAttack == false && whoisITTxt.text == "Bot is IT")
        {
            //public Coroutine StartCoroutine(IEnumerator routine);
            // xie cheng de fangshi
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        _isChase = false;
        _isAttack = true;
        _animator.SetBool("isAttack", true);

        if (enemyType == Type.Melee)
        {
            meleeArea.enabled = true;
            // wait 2s ensure the attacking animation finish 
            yield return new WaitForSeconds(3.0f);
            // let the box collider area diappear
            meleeArea.enabled = false;
        }
        else if (enemyType == Type.Range)
        {
            // the bullet generate time is 0.5s
            yield return new WaitForSeconds(0.5f);
            Vector3 adjustTransform = transform.position;
            adjustTransform.y -= 0.5f; 
            GameObject newMissle = Instantiate(missile, adjustTransform, transform.rotation);
            // Get the rigid body component under the Bullet game object 
            Rigidbody bulletRb = newMissle.GetComponent<Rigidbody>();
            // bullet speed control
            bulletRb.velocity = transform.forward * 10f;
            yield return new WaitForSeconds(2f);
        }

        //meleeArea.enabled = true;
        //// wait 2s ensure the attacking animation finish 
        //yield return new WaitForSeconds(3.0f);
        //// let the box collider area diappear
        //meleeArea.enabled = false;

        // the attacking animation finished, let the enemy chase player and chage to the walking animation again
        _isChase = true;
        _isAttack = false;
        _animator.SetBool("isAttack", false);

    }

    //private void Sensors() {
    //    RaycastHit hit;
    //    float avoidMultiplier = 0f;
    //    avoiding = false;

    //    Vector3 sensorStartPos = transform.position;
    //    sensorStartPos += transform.forward * frontSensorPos.z;
    //    sensorStartPos += transform.up * frontSensorPos.y;
    //    //sensorStartPos.z += frontSensorPos;
        
    //    // front center sensor
    //    if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
    //    {
    //        if (hit.collider.CompareTag("Wall"))
    //        {
    //            Debug.DrawLine(sensorStartPos, hit.point);
    //            avoiding = true;
    //        }
    //    }
    //    Debug.DrawLine(sensorStartPos, hit.point, Color.red);

    //    // front right sensor
    //    //sensorStartPos.x += frontSidesensorPos;
    //    sensorStartPos += transform.right * frontSidesensorPos;
    //    if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
    //    {
    //        if (hit.collider.CompareTag("Wall"))
    //        {
    //            Debug.DrawLine(sensorStartPos, hit.point);
    //            avoiding = true;
    //            avoidMultiplier -= 1f;
    //        }
    //    }
    //    //Debug.DrawLine(sensorStartPos, hit.point, Color.red);
        

    //    // front rigth angle sensor 
    //    else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
    //    {
    //        if (hit.collider.CompareTag("Wall"))
    //        {
    //            Debug.DrawLine(sensorStartPos, hit.point);
    //            avoiding = true;
    //            avoidMultiplier -= 0.5f;
    //        }
    //        else { 
                
    //        }
    //    }
    //    //Debug.DrawLine(sensorStartPos, hit.point, Color.green);

    //    // front left sensor
    //    sensorStartPos -= 2 * transform.right * frontSidesensorPos;
    //    //sensorStartPos.x -= 2 * frontSidesensorPos;
    //    if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
    //    {
    //        if (hit.collider.CompareTag("Wall"))
    //        {
    //            Debug.DrawLine(sensorStartPos, hit.point);
    //            avoiding = true;
    //            avoidMultiplier += 1f;
    //        }
    //    }
    //    //Debug.DrawLine(sensorStartPos, hit.point, Color.red);


    //    // front left angle sensor 
    //    else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
    //    {
    //        if (hit.collider.CompareTag("Wall"))
    //        {
    //            Debug.DrawLine(sensorStartPos, hit.point);
    //            avoiding = true;
    //            avoidMultiplier += 0.5f;
    //        }
    //    }
    //    //Debug.DrawLine(sensorStartPos, hit.point, Color.green);

    //    if (avoiding)
    //    {
    //        Quaternion wheelRotation = new Quaternion();
    //        //transform.getCollid
    //        boxCollider.GetWorld
    //        //transform.rotation = transform * avoidMultiplier;

    //    }

    //}
}
