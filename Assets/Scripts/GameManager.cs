using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] Vector3 sideCameraPosition;
    [SerializeField] Vector3 topCameraPosition;
    private GravityChanger lastGravityChanger;
    public GravityChanger currentGravityChanger;
    public CheckpointController currentCheckpoint;
    [HideInInspector] public Timer timer;
    private IEnumerator LoseCoroutine = null;

    #region UnityEvents
    
    public UnityEvent onUILoad;
    public UnityEvent onTimerEnd;
    public UnityEvent<Transform> onGoalEnd;
    
    #endregion
    
    // public InputActionReference escapeButtonAction;

    #region UI
    
    [HideInInspector] public TextMeshProUGUI scoreText;
    
    public UIManager _UIManager;
    
    // [HideInInspector] public GameObject PauseMenuUI;
    // [HideInInspector] public GameObject LoseMenuUI;
    
    #endregion
    private float totalScore = 0 ;

    #region pauseMenu

    private bool gamePaused = false;
    private bool gameLost => player.HealthPoints <= 0;

    #endregion
    private void Awake()
    {
        if(currentCheckpoint == null)
            throw new Exception("No first checkpoint setted. Please assign one for the player Spawn point.");
        player.transform.position = currentCheckpoint.SpawnPosition;
        lastGravityChanger = currentGravityChanger;
        var newGravityDirection = -currentGravityChanger.nextSpawn.up;
        newGravityDirection = Auxiliar.Round(newGravityDirection);
        GravityController.ChangeGravityDirection(newGravityDirection);
        onUILoad  = new UnityEvent();
        onTimerEnd = new UnityEvent();
        onGoalEnd = new UnityEvent<Transform>();
        onUILoad.AddListener(SetUp);
        onTimerEnd.AddListener(EndGameByTimer);
        onGoalEnd.AddListener(EndLevel);
    }

    private void EndGameByTimer()
    {
        LoseGame();
    }

    private void SetUp()
    {
        scoreText.text = totalScore.ToString("000000");
    }

    public void ChangeToNewGravity()
    {
        currentGravityChanger.isPlayerInside = false;

        var newGravityDirection = -currentGravityChanger.nextSpawn.up;
        newGravityDirection = Auxiliar.Round(newGravityDirection);
        if(newGravityDirection.x + newGravityDirection.y + newGravityDirection.z > 1)
            throw new Exception("The new gravity is not suitable for an isometric view. Two axis must be equal to zero.");

        lastGravityChanger.DisappearHiddableObjects();

        lastGravityChanger = currentGravityChanger;

        currentGravityChanger.Activate();
        
        GravityController.ChangeGravityDirection(newGravityDirection);
        SetConstraints();
        player.Teleport(currentGravityChanger.nextSpawn.position);
        cameraController.MoveAndLookAtNewDir(currentGravityChanger);
    }

    // private void FlipConstraints(Vector3 gravity)
    // {
    //     if(Mathf.Abs(gravity.x) == 1)
    //         if (player.Rb.constraints == RigidbodyConstraints.FreezePositionZ)
    //             player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
    //         else
    //             player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
    //     else if(Mathf.Abs(gravity.y) == 1)
    //         if (player.Rb.constraints == RigidbodyConstraints.FreezePositionZ)
    //             player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
    //         else
    //             player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
    //     else if(Mathf.Abs(gravity.z) == 1)
    //         if (player.Rb.constraints == RigidbodyConstraints.FreezePositionX)
    //             player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
    //         else
    //             player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
    // }

    public void SetConstraints()
    {
        var axisToLock = currentGravityChanger.nextCameraOrientation.transform.forward;
        Debug.DrawRay(player.transform.position, axisToLock, Color.red);
        LayerMask bitMask = new LayerMask();    //XYZ
        bitMask = Mathf.Abs(Mathf.RoundToInt(axisToLock.x)) << 0 |
                  Mathf.Abs(Mathf.RoundToInt(axisToLock.y)) << 1 |
                  Mathf.Abs(Mathf.RoundToInt(axisToLock.z)) << 2;
        switch (bitMask)
        {
            case 1:
                player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
                break;
            case 2:
                player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
                break;
            case 4:
                player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
                break;
            default:
                throw new Exception("Cant constrain more than one rigidbody constraints.");
        }

        player.UpdateMovementDirection(axisToLock, currentGravityChanger.nextCameraOrientation.transform.up, currentGravityChanger.flipMovement);
    }

    public void UpdateScore(int value)
    {
        totalScore += value;
        scoreText.text = totalScore.ToString("000000");
    }

    [ContextMenu("Camera shake")]
    public void DoCameraShake()
    {
        cameraController.CameraShake();
    }

    public void PressEsc(InputAction.CallbackContext ctx)
    {
        if (gamePaused && !gameLost)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        _UIManager.ShowPauseMenu();
        // PauseMenuUI.gameObject.SetActive(true);
    }
    
    public void LoseGame()
    {
        if(LoseCoroutine != null) return;
        LoseCoroutine = LoseGameCoroutine();
        StartCoroutine(LoseCoroutine);
    }

    IEnumerator LoseGameCoroutine()
    {
        //Small player pause before dying
        player.DisablePlayer();
        player.Fade();
        yield return new WaitForSeconds(player.ColorFadeTime + 0.2f);
        
        //slide transition to LoseMenuScene
        _UIManager.TransitionSlider.onLevelUnload.Invoke();
        yield return new WaitForSeconds(_UIManager.TransitionSlider.TotalTransitionTime);
        gamePaused = true;
        Time.timeScale = 0;
        _UIManager.ShowLoseMenu();
        StopCoroutine(LoseCoroutine);
        LoseCoroutine = null;
    }
    
    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        _UIManager.HidePauseMenu();
        _UIManager.HideLoseMenu();
        // PauseMenuUI.gameObject.SetActive(false);
        // LoseMenuUI.gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        gamePaused = false;
        player.DisableInputs();
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
        player.DisableInputs();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        var nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void EndLevel(Transform goal)
    {
        DisablePlayer();
        timer.StopTimer();
        AddTimerScore();
        //deactivate camera lookat
        cameraController.vcam.Follow = goal;
        cameraController.LockToTarget.Damping = 3f;
    }
    
    IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(1f);
        _UIManager.TransitionSlider.onLevelUnload.Invoke();
        yield return new WaitForSeconds(2f);
        //Fadeout negro to next level
        
        //load next level
        LoadNextLevel();
    }

    private void AddTimerScore()
    {
        StartCoroutine(TimerToScore());
    }

    IEnumerator TimerToScore()
    {
        float effectDuration = 0.05f;
        float strength = 2f;
        while (timer.CurrentTimer >= 10)
        {
            timer.ScoreTimeEffect(10, effectDuration, strength);
            UpdateScore(100);   //For each second, 10 points
            scoreText.transform.DOShakePosition(effectDuration, strength);
            yield return new WaitForSeconds(effectDuration);
        }
        UpdateScore(timer.CurrentTimer*10);
        timer.ScoreTimeEffect(timer.CurrentTimer, effectDuration, strength);
        scoreText.transform.DOShakePosition(effectDuration, strength);
        
        //At the end of the time score calculation, the level ends
        StartCoroutine(EndLevelCoroutine());
    }

    public bool CanChangeGravity()
    {
        return currentGravityChanger != null && currentGravityChanger.Active;
    }

    public void DisablePlayer()
    {
        player.DisableMoveInput();
    }
}