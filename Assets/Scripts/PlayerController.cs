using System;
using System.Collections;
using Art.Shaders;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public GameManager GameManager => gameManager;
    #region InputSystem
    public PlayerInput playerInputSystem;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference interactAction;
    public InputActionReference escapeButtonAction;
    #endregion
        
    #region MovementVariables
    [SerializeField] float maxJumpForce;
    [SerializeField] float maxJumpKeyDownTime;
    [SerializeField] private float Speed;
    private float currentTimeDown = 0;
    private IEnumerator chargeJump;
    private Vector3 moveDirection;
    private Vector3 currentAllowedDirection = Vector3.right;
    private bool isMovementFlipped = false;
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
    public bool IsDead => healthPoints <= 0;

    #endregion

    #region Material

    [SerializeField] private SquashStretch squashStretch;
    private Material playerMaterial;
    [SerializeField] private float colorChangeTime = 2;
    [SerializeField] private float colorFadeTime = 1.5f;
    public float ColorFadeTime => colorFadeTime;
    
    [SerializeField] private TrailRenderer trailRenderer;
    #endregion

    #region Audio

    [SerializeField] private AudioClip damageSfx;
    [SerializeField] private AudioClip chargeJumpSfx;
    [SerializeField] private AudioClip releaseJumpSfx;
    [SerializeField] private AudioSource audioSource;
    

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //playerMaterial = GetComponent<Renderer>().material;
        //playerMaterial = shaderGO.GetComponent<Renderer>().material;
        healthPoints = MaxHealth;
        sphereCollider = GetComponent<SphereCollider>();
        playerInputSystem = GetComponent<PlayerInput>();
        squashStretch.deformationMaterial.SetColor("_BaseColor", Color.blue);
        trailRenderer.endColor = trailRenderer.startColor = Color.blue;
    }

    public void ChangePlayerMaterialRed()
    {
        ChangeMaterialAndTrailColor(Color.red);
        /*
        DOVirtual.Color(playerMaterial.color, Color.red, colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
        DOVirtual.Color(playerMaterial.color, Color.red, colorChangeTime, (value) =>
        {
            trailRenderer.endColor = trailRenderer.startColor = value;
        });
        */
    }
    
    public void ChangePlayerMaterialOrange()
    {
        ChangeMaterialAndTrailColor(new Color(1f, 0.5f, 0f));
        /*
        DOVirtual.Color(playerMaterial.color, new Color(1f, 0.5f, 0f), colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
        DOVirtual.Color(playerMaterial.color, new Color(1f, 0.5f, 0f), colorChangeTime, (value) =>
        {
            trailRenderer.endColor = trailRenderer.startColor = value;
        })*/
    }
    
    public void ChangePlayerMaterialBlue()
    {
        ChangeMaterialAndTrailColor(Color.blue);
        /*DOVirtual.Color(playerMaterial.color, Color.blue, colorChangeTime, (value) =>
        {
            playerMaterial.color = value;
        });
        DOVirtual.Color(playerMaterial.color, Color.blue, colorChangeTime, (value) =>
        {
            trailRenderer.endColor = trailRenderer.startColor = value;
        });*/
    }

    private void ChangeMaterialAndTrailColor(Color color)
    {
        DOVirtual.Color(squashStretch.deformationMaterial.GetColor("_BaseColor"), color, colorChangeTime, (value) =>
        {
            squashStretch.deformationMaterial.SetColor("_BaseColor",value);
        });
        DOVirtual.Color(squashStretch.deformationMaterial.GetColor("_BaseColor"), color, colorChangeTime, (value) =>
        {
            trailRenderer.endColor = trailRenderer.startColor = value;
        });
    }
    

    private void Update()
    {
        var moveInput = moveAction.action.ReadValue<Vector2>();
        //moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        moveDirection = moveInput.x * currentAllowedDirection;
    }

    private void FixedUpdate()
    {
        rb.AddForce(GravityController.Gravity * rb.mass);
        rb.AddForce(moveDirection * Speed);
    }

    /// <summary>
    /// Use only when the player dies
    /// </summary>
    public void DisablePlayer()
    {
        DisableMoveInput();
        rb.isKinematic = true;
        sphereCollider.enabled = false;
    }

    public void DisableMoveInput()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void ChargeJump(InputAction.CallbackContext ctx)
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
        audioSource.clip = chargeJumpSfx;
        audioSource.Play();
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

    

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!jumpCharged)
            return;
        
        jumpCharged = false;
        
        if(audioSource.clip == chargeJumpSfx && audioSource.isPlaying)
            audioSource.Stop();
        audioSource.clip = releaseJumpSfx;
        audioSource.Play();
        
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
    
    private void ChangeGravity(InputAction.CallbackContext ctx)
    {
        
        if (!gameManager.CanChangeGravity())
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
        jumpAction.action.started += ChargeJump;
        jumpAction.action.canceled += Jump;
        jumpAction.action.Enable();
        interactAction.action.started += ChangeGravity;
        interactAction.action.Enable();
        escapeButtonAction.action.started += gameManager.PressEsc;
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    public void DisableInputs()
    {
        jumpAction.action.started -= ChargeJump;
        jumpAction.action.canceled -= Jump;
        jumpAction.action.Disable();
        interactAction.action.started -= ChangeGravity;
        interactAction.action.Disable();
        escapeButtonAction.action.started -= gameManager.PressEsc;
        escapeButtonAction.action.Disable();
    }

    public void Teleport(Vector3 position)
    {
        sphereCollider.enabled = false;
        // ResetPhysics();
        rb.transform.DOMove(position, 1f)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() => { sphereCollider.enabled = true;
                StartCoroutine(ResetPhysics(rb));
            });
    }

    // private void ResetPhysics()
    // {
    //     rb.linearVelocity = Vector3.zero;
    //     rb.angularVelocity = Vector3.zero;
    //     rb.inertiaTensorRotation = Quaternion.identity;
    //     rb.inertiaTensor = Vector3.zero;
    //     rb.automaticInertiaTensor = true;
    //     rb.isKinematic = true;
    //     rb.isKinematic = false;
    // }
    
    IEnumerator ResetPhysics(Rigidbody rB)
    {
        rB.linearVelocity = Vector3.zero;
        rB.angularVelocity = Vector3.zero;

        yield return new WaitForFixedUpdate();
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

        }
        else{
            gameManager.LoseGame();
        }
    }

    public void TakeDamage()
    {
        
        audioSource.PlayOneShot(damageSfx);
        LoseHealth();
        UpdateHealthStatus();
        if (!IsDead)
        {
            Teleport(gameManager.currentCheckpoint.SpawnPosition);
            gameManager.DoCameraShake();
        }
    }

    public void UpdateMovementDirection(Vector3 frozenDirection, Vector3 upDirection, bool flip = false)
    {
        currentAllowedDirection = Vector3.Cross( upDirection, frozenDirection);
        if(flip) currentAllowedDirection *= -1;
    }

    public void Fade()
    {
        Color currentColor = playerMaterial.color;
        Color invisibleColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        DOVirtual.Color(playerMaterial.color, invisibleColor, colorFadeTime, (value) =>
        {
            playerMaterial.color = value;
        });

        DOVirtual.Float(playerMaterial.GetFloat("_Smoothness"), 0, colorFadeTime, (value) =>
        {
            playerMaterial.SetFloat("_Smoothness", value);
        });
        DOVirtual.Color(playerMaterial.color, invisibleColor, colorFadeTime, (value) =>
        {
            trailRenderer.endColor = trailRenderer.startColor = value;
        });
    }
}
