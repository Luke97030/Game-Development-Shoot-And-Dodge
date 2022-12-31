using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _playerController;
    private CharacterController _characterController;
    //public WeaponController weaponController;

    public Camera camera;
    public float gravity = 15f;
    public float moveSpeed = 7.5f;

    //private float _playerHeight = 1.8f;

    Vector3 playerVelocity { get; set; }

    // variables for camera rotation
    public float rotationSpeed = 2f;
    // controll camera looking up and down
    private float _cameraVertialAngle = 0f;

    // player/It's health 
    public float maxHealth = 3f;
    //private float _currentHealth;

    public Hp _hp;
    public Animator playerAnimator;
    public GameObject bullet;

    // game manager for input text at the end of game and other text control
    public GameManager gameManager;

    // when the program start, the playerController instance be created 
    private void Awake()
    {
        _playerController = this;
        // initialize the currentHealth
        //_currentHealth = maxHealth;
    }

    private void Start()
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _playerController = GetComponent<PlayerController>();
        // the default character controller be we added the Character Component into the Play component
        _characterController = GetComponent<CharacterController>();
        // recovery if the collision overlap happens
        _characterController.enableOverlapRecovery = true;
    }

    private void Update()
    {
        // Camera horizontal rotation
        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * rotationSpeed, 0), Space.Self);

        // Camera horizontal rotation
        _cameraVertialAngle += Input.GetAxisRaw("Mouse Y") * rotationSpeed;
        // set up the min vertical look range for camera 
        _cameraVertialAngle = Mathf.Clamp(_cameraVertialAngle, -89, 89);
        camera.transform.localEulerAngles = new Vector3(-_cameraVertialAngle, 0, 0);

        //// player movement
        // player movement animation
        PlayerMovementAnimationControl();

        // player attacking animtation and fire a projectile
        PlayerAttack();
        // player movement
        if (!_characterController.isGrounded)
        {
            playerVelocity += Vector3.down * gravity * Time.deltaTime;
        }

        _characterController.Move(playerVelocity * Time.deltaTime);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        _characterController.Move(move * moveSpeed * Time.deltaTime); //Move framerate independant


    }

    private void PlayerMovementAnimationControl()
    {
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            //Run forward
            playerAnimator.SetBool("idle", false);
            playerAnimator.SetBool("runForward", true);
            playerAnimator.SetBool("runBackward", false);
            playerAnimator.SetBool("runLeft", false);
            playerAnimator.SetBool("runRight", false);
            //playerAnimator.SetBool("shooting", false);


        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)))
        {
            //Run towards left
            playerAnimator.SetBool("idle", false);
            playerAnimator.SetBool("runForward", false);
            playerAnimator.SetBool("runBackward", false);
            playerAnimator.SetBool("runLeft", true);
            playerAnimator.SetBool("runRight", false);
            //playerAnimator.SetBool("shooting", false);


        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S)))
        {
            //Run towards right
            playerAnimator.SetBool("idle", false);
            playerAnimator.SetBool("runForward", false);
            playerAnimator.SetBool("runBackward", false);
            playerAnimator.SetBool("runLeft", false);
            playerAnimator.SetBool("runRight", true);
            //playerAnimator.SetBool("shooting", false);

        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            //Run backward
            playerAnimator.SetBool("idle", false);
            playerAnimator.SetBool("runForward", false);
            playerAnimator.SetBool("runBackward", true);
            playerAnimator.SetBool("runLeft", false);
            playerAnimator.SetBool("runRight", false);
            //playerAnimator.SetBool("shooting", false);
        }
        else
        {
            //Run idle
            playerAnimator.SetBool("idle", true);
            playerAnimator.SetBool("runForward", false);
            playerAnimator.SetBool("runRight", false);
            playerAnimator.SetBool("runLeft", false);
            playerAnimator.SetBool("runBackward", false);
            //playerAnimator.SetBool("shooting", false);

        }
    }

    void PlayerAttack()
    {
        AnimatorStateInfo animStateInfo;
        float NTime;
        bool animationFinished = false;
        animStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        NTime = animStateInfo.normalizedTime;

        if (NTime > 1.0f) animationFinished = true;
        if (_hp.playerIsIT && Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("shooting", true);

        }

        // only change back to idle when the shooting animation finish
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shoot_SingleShot_AR") && playerAnimator.GetBool("shooting") && animationFinished != false)
        { 
            playerAnimator.SetBool("shooting", false);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee Area")
        {
            //_currentHealth--;
            print("Play be attacked");
            // we can use yield return
            //StartCoroutine(onDamage());
        }

        // if the Range enemy hit the player,
        if (other.tag == "RangeEnemyBullet")
        {
            Debug.Log("Player is It");
            _hp.playerIsIT = true;
            _hp.enemyIsIT = false;
            gameManager.GetComponent<GameManager>().playerITCount += 1;
            gameManager.playerItCountText.text = "Player IT Count: " + gameManager.GetComponent<GameManager>().playerITCount.ToString();
            //Debug.Log("Range Enemy IT count is: " + gameManager.GetComponent<GameManager>().enemyITCount);
            //Bullet enemyBullet = other.GetComponent<Bullet>();
            //_currentHealth --;

            //StartCoroutine(onDamage());

            // if the rigitbody of the enemy bullet is exisiting, we need to destory it
            if (other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);


        }
    }
}
