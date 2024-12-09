using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
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
    #endregion
    
    #region PhysicsVariables
    private Rigidbody rb;
    public Rigidbody Rb => rb;
    private Vector3 gyroUp => -GravityController.Gravity.normalized;
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
    }

    [ContextMenu("Change player color - Red")]
    public void ChangePlayerMaterialRed()
    {
        DOVirtual.Color(playerMaterial.color, Color.red, colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
    }
    
    [ContextMenu("Change player color - Orange")]
    public void ChangePlayerMaterialOrange()
    {
        DOVirtual.Color(playerMaterial.color, new Color(1f, 0.5f, 0f), colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
    }
    
    [ContextMenu("Change player color - Blue")]
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
        if (chargeJump == null)
        {
            chargeJump = ChargeJumpCoroutine();
            StartCoroutine(chargeJump);
        }
    }

    IEnumerator ChargeJumpCoroutine()
    {
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
        if (chargeJump != null)
        {
            StopCoroutine(chargeJump);
            chargeJump = null;
        }
        if (currentTimeDown >= maxJumpKeyDownTime)
            currentTimeDown = maxJumpKeyDownTime;
        float jumpForce = Mathf.Lerp(maxJumpForce*0.2f, maxJumpForce, currentTimeDown / maxJumpKeyDownTime);
        rb.AddForce( gyroUp * jumpForce, ForceMode.Impulse);
        currentTimeDown = 0;
    }
    
    private void ChangeGravity()
    {
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
        if (healthPoints > 0)
        {
            //TODO "Death" reset to last checkPoint
        }
        else{
            //TODO Lose POPUP Screen
        }
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
}
