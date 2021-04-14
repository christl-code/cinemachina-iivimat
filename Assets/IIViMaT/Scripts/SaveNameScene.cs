using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveNameScene : MonoBehaviour
{
    public static string nameScene, nameNewScene;

    void Start(){
        if(nameScene == null)
            nameScene  = SceneManager.GetActiveScene().name;

        Debug.Log("nameScene"+nameScene);

    }
    public void Update () {
        nameNewScene  = SceneManager.GetActiveScene().name;
    }

}
