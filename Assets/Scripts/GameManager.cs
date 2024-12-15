using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] Vector3 sideCameraPosition;
    [SerializeField] Vector3 topCameraPosition;
    public GravityChanger currentGravityChanger;
    public CheckpointController currentCheckpoint;
    
    // public InputActionReference escapeButtonAction;

    #region UI
    
    [SerializeField] private TextMeshProUGUI textoPuntuación;
    
    [SerializeField] private GameObject PauseMenuUI;
    
    #endregion
    private float totalScore = 0 ;

    #region pauseMenu

    private bool gamePaused = false;

    #endregion
    private void Awake()
    {
        if(currentCheckpoint == null)
            throw new Exception("No first checkpoint setted. Please assign one for the player Spawn point.");
        player.transform.position = currentCheckpoint.SpawnPosition;
        textoPuntuación.text = totalScore.ToString("000000");
    }

    // private void Start()
    // {
    //     escapeButtonAction.action.Disable();
    // }

    public void ChangeToNewGravity()
    {
        var newGravityDirection = -currentGravityChanger.nextSpawn.up;
        newGravityDirection = Auxiliar.Round(newGravityDirection);
        if(newGravityDirection.x + newGravityDirection.y + newGravityDirection.z > 1)
            throw new Exception("The new gravity is not suitable for an isometric view. Two axis must be equal to zero.");

        currentGravityChanger.ShowHiddenObjects();
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
        bitMask = Mathf.RoundToInt(axisToLock.x) << 0 |
                  Mathf.RoundToInt(axisToLock.y) << 1 |
                  Mathf.RoundToInt(axisToLock.z) << 2;
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
        }
    }

    public void UpdateScore(int value)
    {
        totalScore += value;
        textoPuntuación.text = totalScore.ToString("000000");
    }

    public void DoCameraShake()
    {
        //TODO
        // 
    }

    public void PressEsc()
    {
        if (gamePaused)
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
        Debug.Log("Pausando desde player");
        Time.timeScale = 0;
        PauseMenuUI.gameObject.SetActive(true);
    }
    
    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        Debug.Log("---Resume desde menu pausa");
        PauseMenuUI.gameObject.SetActive(false);
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
}