using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public GameManager GameManager => gameManager;
    #region InputSystem
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference interactAction;
    #endregion
        
    #region MovementVariables
    [SerializeField] float maxJumpForce;
    [SerializeField] float maxJumpKeyDownTime;
    [SerializeField] private float Speed;
    private float currentTimeDown = 0;
    private IEnumerator chargeJump;
    private Vector3 moveDirection;
    private bool jumpCharged = false;
    private float allowedDistanceToJump = 0.3f;
    private float minJumpPercent = 0.33f;
    #endregion
    
    #region PhysicsVariables
    private Rigidbody rb;
    public Rigidbody Rb => rb;
    private Vector3 gyroUp => -GravityController.Gravity.normalized;
    private SphereCollider sphereCollider;
    #endregion

    #region HealthSystem

    private const int MaxHealth = 3;
    private int healthPoints;
    public int HealthPoints => healthPoints;

    #endregion

    #region Material

    private Material playerMaterial;
    [SerializeField] private float colorChangeTime = 2;
    

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMaterial = GetComponent<Renderer>().material;
        healthPoints = MaxHealth;
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void ChangePlayerMaterialRed()
    {
        DOVirtual.Color(playerMaterial.color, Color.red, colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
    }
    
    public void ChangePlayerMaterialOrange()
    {
        DOVirtual.Color(playerMaterial.color, new Color(1f, 0.5f, 0f), colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
    }
    
    public void ChangePlayerMaterialBlue()
    {
        DOVirtual.Color(playerMaterial.color, Color.blue, colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
    }
    

    private void Update()
    {
        var moveInput = moveAction.action.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }

    private void FixedUpdate()
    {
        rb.AddForce(GravityController.Gravity * rb.mass);
        rb.AddForce(moveDirection * Speed);
    }


    private void ChargeJump()
    {
        Debug.DrawRay(transform.position, GravityController.Gravity.normalized * (sphereCollider.radius + allowedDistanceToJump), Color.red );
        Physics.Raycast(transform.position, GravityController.Gravity.normalized, out RaycastHit hit,
            sphereCollider.radius + allowedDistanceToJump, LayerMask.GetMask("Ground"));
        if (hit.collider == null)
            return;
        if (chargeJump == null)
        {
            chargeJump = ChargeJumpCoroutine();
            StartCoroutine(chargeJump);
        }
    }

    IEnumerator ChargeJumpCoroutine()
    {
        jumpCharged = true;
        while (currentTimeDown < maxJumpKeyDownTime)
        {
            currentTimeDown += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        currentTimeDown = maxJumpKeyDownTime;
        StopCoroutine(chargeJump);
        chargeJump = null;
    }

    

    private void Jump()
    {
        if (!jumpCharged)
            return;
        
        jumpCharged = false;
        if (chargeJump != null)
        {
            StopCoroutine(chargeJump);
            chargeJump = null;
        }
        if (currentTimeDown >= maxJumpKeyDownTime)
            currentTimeDown = maxJumpKeyDownTime;
        float jumpForce = Mathf.Lerp(maxJumpForce*minJumpPercent, maxJumpForce, currentTimeDown / maxJumpKeyDownTime);
        rb.AddForce( gyroUp * jumpForce, ForceMode.Impulse);
        currentTimeDown = 0;
    }
    
    private void ChangeGravity()
    {
        if (!gameManager.currentGravityChanger.Active)
        {
            //tween inactivo además de tener un color distinto
            return;
        }
        if(gameManager.currentGravityChanger.isPlayerInside)
            gameManager.ChangeToNewGravity();
            // if (gameManager.currentGravityChanger.transform.localEulerAngles.x == 270)
            //     gameManager.EnableGravityOnY();
            // else
            //     gameManager.EnableGravityOnZ();
    }

    private void OnEnable()
    {
        jumpAction.action.started += ctx => ChargeJump();
        jumpAction.action.canceled += ctx => Jump();
        interactAction.action.started += ctx => ChangeGravity();
    }

    private void OnDisable()
    {
        jumpAction.action.started -= ctx => ChargeJump();
        jumpAction.action.canceled -= ctx => Jump();
        interactAction.action.started -= ctx => ChangeGravity();
    }

    public void Teleport(Vector3 position)
    {
        transform.DOMove(position, 1f)
            .SetEase(Ease.InOutCubic);
    }

    public void LoseHealth()
    {
        healthPoints--;
    }

    public void GainHealth()
    {
        if (healthPoints == MaxHealth)
            return;
        healthPoints++;
    }

    public void ResetHealthPoints()
    {
        healthPoints = MaxHealth;
    }

    public void UpdateHealthStatus()
    {
        if (healthPoints > 0)
        {
            switch (healthPoints)
            {
                case 1:
                    ChangePlayerMaterialRed();
                    break;
                case 2:
                    ChangePlayerMaterialOrange();
                    break;
                case 3:
                    ChangePlayerMaterialBlue();
                    break;
            }
            //TODO "Death" reset to last checkPoint
        }
        else{
            //TODO Lose POPUP Screen
        }
    }

    public void TakeDamage()
    {
        LoseHealth();
        UpdateHealthStatus();
        Teleport(gameManager.currentCheckpoint.SpawnPosition);
    }
}
