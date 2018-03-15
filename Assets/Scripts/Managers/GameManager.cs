using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
    public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
    public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public GameObject m_tank1;             // Reference to the prefab the players will control.
    public GameObject m_tank2;             // Reference to the prefab the players will control.
    public Transform m_SpawnPoint1;
    public Transform m_SpawnPoint2;


    private int m_RoundNumber;                  // Which round the game is currently on.
    private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
    private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
    private bool m_OneTankLeft;
    private GameObject m_GameWinner;
    private TankMovement m_Movement1;
    private TankShooting m_Shooting1;
    private TankMovement m_Movement2;
    private TankShooting m_Shooting2;
    private TankHealth player1health;
    private TankHealth player2health;
    


    private void Start()
    {
        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        m_GameWinner = null;
        m_OneTankLeft = false ;

        m_Movement1 = m_tank1.GetComponent<TankMovement>();
        m_Shooting1 = m_tank1.GetComponent<TankShooting>();

        m_Movement2 = m_tank2.GetComponent<TankMovement>();
        m_Shooting2 = m_tank2.GetComponent<TankShooting>();

        player1health = m_tank1.GetComponent<TankHealth>();
        player2health = m_tank2.GetComponent<TankHealth>();
        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLoop());
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
        if (m_GameWinner != null)
        {
            // If there is a game winner, back to start menu.
            SceneManager.LoadScene("Start");
        }
        else
        {
            // If there isn't a winner yet, restart this coroutine so the loop continues.
            // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        DisableTankControl();

        // Increment the round number and display text showing the players what round it is.
        m_RoundNumber++;
        m_MessageText.text = "Battle Start!";

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the tanks.
        EnableTankControl();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one tank left...
        while (!m_OneTankLeft)
        {
            player1health = m_tank1.GetComponent<TankHealth>();
            player2health = m_tank2.GetComponent<TankHealth>();

            float player1CurrentHealth = player1health.getCurrentHealth();
            float player2CurrentHealth = player2health.getCurrentHealth();

            if (player1CurrentHealth <= 0)
            {
                m_GameWinner = m_tank2;
                m_OneTankLeft = true;
            }
            else if (player2CurrentHealth <= 0)
            {
                m_GameWinner = m_tank1;
                m_OneTankLeft = true;
            }

            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // Stop tanks from moving.
        DisableTankControl();

        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }


    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "DRAW!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";

        // If there is a game winner, change the entire message to reflect that.
        if (m_GameWinner != null)
            message = m_GameWinner.name+ " WINS THE GAME!";

        return message;
    }


    // This function is used to turn all the tanks back on and reset their positions and properties.
    private void ResetAllTanks()
    {
        // Player 1's Tank
        m_tank1.transform.position = m_SpawnPoint1.position;
        m_tank1.transform.rotation = m_SpawnPoint1.rotation;

        m_tank1.SetActive(false);
        m_tank1.SetActive(true);

        // Player 2's Tank
        m_tank2.transform.position = m_SpawnPoint2.position;
        m_tank2.transform.rotation = m_SpawnPoint2.rotation;

        m_tank2.SetActive(false);
        m_tank2.SetActive(true);
    }


    private void EnableTankControl()
    {
        // Player 1's Tank
        m_Movement1.enabled = true;
        m_Shooting1.enabled = true;

        // Player 2's Tank
        m_Movement2.enabled = true;
        m_Shooting2.enabled = true;
    }


    private void DisableTankControl()
    {
        // Player 1's Tank
        m_Movement1.enabled = false;
        m_Shooting1.enabled = false;

        // Player 2's Tank
        m_Movement2.enabled = false;
        m_Shooting2.enabled = false;
    }

    
}