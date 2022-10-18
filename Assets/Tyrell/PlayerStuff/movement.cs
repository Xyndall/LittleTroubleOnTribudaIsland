using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    #region singleton
    public static movement instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    // Player Movement Terms
    [SerializeField] public CharacterController controller;

    //ShootProjectile script
    public ShootProjectile shootProjectile;

    // Dash
    public float _dashCooldown;
    public bool _isDashing;

    // Movement Inputs
    private Vector3 _moveDirection;

    bool KnockBacked;
    Vector3 knockbackDir;

    // Mouse Position
    public MousePosition mousePos;

    //InventoryHandler
    public InventoryUIHandler inventoryUIHandler;

    //Upgradeables
    public Upgradeables upgrade;


    //analytics stuff
    Vector3 lastPos;
    float DetectMovement;
    public int HowMuchPlayerMoved = 0;
    public int HowMuchPlayerStill = 0;
    float Timer = 0;
    int DelayAmount = 1;


    //Animation
    public Animator _animator;
    bool isShooting = false;
    bool isWalking = false;

    private void Start()
    {
        inventoryUIHandler = FindObjectOfType<InventoryUIHandler>();


    }

    private void Update()
    {
        
        //Input
        GatherInput();
        //Aim
        Aim();
        //Move
        Move();


        //Shoots projectile from shootprojectile script
        if (Input.GetKey(KeyCode.Mouse0))
        {
            isShooting = true;
            shootProjectile.ComponentShoot();
            _animator.SetBool("isWalkOrShooting", true);

        }
        else
        {
            isShooting = false;
            _animator.SetBool("isWalkOrShooting", false);
        }

        if(isShooting && isWalking) _animator.SetFloat("Blend", 0.5f);
        if (isShooting && !isWalking) _animator.SetFloat("Blend", 0);
        if(!isShooting && isWalking) _animator.SetFloat("Blend", 1);



        if (transform.position != lastPos)
        {
            //Player has Moved
            DetectMovement = 1;
            _animator.SetBool("isWalkOrShooting", true);
            _animator.SetBool("isIdle", false);
            isWalking = true;
            
        }
        else
        {
            //Player has not Moved
            DetectMovement = 0;
            _animator.SetBool("isIdle", true);
            if (!isShooting)
            {
                _animator.SetBool("isWalkOrShooting", false);
            }
            
            isWalking = false;

        }

        if (KnockBacked)
        {
            StartCoroutine(IsKnockBacked());
        }

        Timer += Time.deltaTime;

        if (Timer > DelayAmount)
        {
            Timer = 0;
            if (DetectMovement == 1)
            {
                HowMuchPlayerMoved++;

            }
            if (DetectMovement == 0)
            {
                HowMuchPlayerStill++;

            }

        }

    }


    private void FixedUpdate()
    {
        lastPos = transform.position;

    }

    // Inputs for Movement
    private void GatherInput()
    {
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_dashCooldown <= 0)
            {
                _animator.SetTrigger("isDashing");
                
                StartCoroutine(Dash());
                _isDashing = true;
                
            }
        }

        _dashCooldown -= Time.deltaTime;
    }

    // Code for Aim/Mouse
    private void Aim()
    {
        
        // Player looks towards the mouse as it moves
        var direction = mousePos.WorldPosition - transform.position;

        // Ignore the height difference.
        direction.y = 0;

        // Make the transform look in the direction.
        transform.forward = direction;

        
    }

    private void Move()
    {

        controller.Move(_moveDirection.ToIso() * upgrade.playerSpeed * Time.deltaTime);
        
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + upgrade._dashTime)
        {
            if(DetectMovement == 1)
                controller.Move(_moveDirection.ToIso() * upgrade._dashSpeed * Time.deltaTime);
            else
                controller.Move(transform.forward * (upgrade._dashSpeed + 160) * Time.deltaTime);

            _dashCooldown = upgrade._dashCooldownTime;
            _isDashing = false;
            yield return null;
        }
    }

    public void Knockback(Vector3 forward)
    {
        KnockBacked = true;
        knockbackDir = forward;

    }
    
    IEnumerator IsKnockBacked()
    {
        
        float startTime = .5f;
        float KnockbackDistance = 200;

        controller.Move(knockbackDir * KnockbackDistance * Time.deltaTime);

        yield return new WaitForSeconds(startTime);
        Vector3 pos = gameObject.transform.position;
        pos.y = 2;
        gameObject.transform.position = pos; 
        KnockBacked = false;
    }

}


public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 _moveDirection) => _isoMatrix.MultiplyPoint3x4(_moveDirection);
}
