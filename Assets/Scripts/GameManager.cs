using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // name input for the game over scene to save record
    public InputField nameInput;

    // when game starts, the player is IT. So the initial count for playerITCount should be 1 
    public int playerITCount = 1;
    public int enemyITCount = 0;
    public Text playerItCountText;
    public Text enemyItCountText;
}
