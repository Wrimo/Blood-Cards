using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonCommands : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Arena");
    }
    public void ExitGame()
    {
        Application.Quit(); 
    }

}
