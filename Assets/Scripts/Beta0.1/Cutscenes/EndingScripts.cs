using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScripts : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            toMainMenu();
        }
    }
    private void toMainMenu()
    {        
        SceneManager.LoadScene(5);
    }
}
