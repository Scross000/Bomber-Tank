using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankBombing : MonoBehaviour {

    public GameObject bombPrefab;              // Bomb Object
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public int m_BombCount = 5;                 // Number of bomb that can be use
    public Text m_BombUI;                       // Show players number of the remaining bombs
    public Transform m_BombTransform;           // A child of the tank where the bombs are spawned.
    private string m_BombButton;                // The input axis that is used for planting bombs.
    private bool m_Bombed;                       // Whether or not the bomb has been planted with this button press.


    // Use this for initialization
    void Start () {
        // The fire axis is based on the player number.
        m_BombButton = "Bomb" + m_PlayerNumber;
        m_BombUI.text = "Bomb: " + m_BombCount;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(m_BombButton) && !m_Bombed)  // Bomb button is pressed
        {
            // Player still have bombs to be planted
            if(m_BombCount != 0)
            {
                DropBomb();
                m_Bombed = true;
                m_BombCount--;
                m_BombUI.text = "Bomb: " + m_BombCount;
            }

            if (m_Bombed && m_BombCount != 0) //player can plant another bomb
            {
                m_Bombed = false;
            }
        }
	}

    void DropBomb()
    {
        Instantiate(bombPrefab, m_BombTransform.position, Quaternion.identity);

    }
}
