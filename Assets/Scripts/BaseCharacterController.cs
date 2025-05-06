using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class BaseCharacterController : MonoBehaviour
{
    private Vector2 movementInput;
    [SerializeField] float movementSpeed;
    [Range(0,1)][SerializeField] float slowedFactor;
    private bool isSlowed;
    private bool isPlayerInBattle;
    private Vector3Int currentPosition;
    private Vector3Int lastEncounterPosition;

    [SerializeField] private string townSceneName;

    public Tilemap tilemap
    {
        get
        {
            if (m_tilemap == null) m_tilemap = FindObjectOfType<Tilemap>();
            return m_tilemap;
        }
    }
    private Tilemap m_tilemap;

    private void Start()
    {
        isSlowed = false;
        isPlayerInBattle = false;
    }

    /// <summary>
    /// Movement is called by the input system when the player moves the joystick or presses the arrow keys.
    /// </summary>
    /// <param name="ctx">Context provided by Unity Input</param>
    public void Movement(CallbackContext ctx)
    {
        //movementInput is set by unity events
        movementInput = ctx.ReadValue<Vector2>(); //comment
    }

    //This is now a FIXEDupdate
    private void FixedUpdate()
    {
        if (isPlayerInBattle) return;
        //var actualMovementSpeed = isSlowed ? movementSpeed * slowedFactor : movementSpeed;
        var actualMovementSpeed = movementSpeed;
        if(isSlowed) actualMovementSpeed *= slowedFactor;

        transform.Translate(new Vector3(movementInput.x, movementInput.y, 0) * Time.deltaTime * actualMovementSpeed);
        currentPosition = tilemap.WorldToCell(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnterTown"))
        {
            SceneManager.LoadScene(townSceneName);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Swamp"))
        {
            isSlowed = true;
        }
        else if (col.gameObject.CompareTag("FightEncounter"))
        {
            if(currentPosition != lastEncounterPosition)
            {
                lastEncounterPosition = currentPosition;
                isPlayerInBattle = FightManager.Instance.CheckForEncounter();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Swamp"))
        {
            isSlowed = false;
        }
    }

    private void CheckForEncounter()
    {
        FightManager.Instance.CheckForEncounter();
    }

}
