using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toStart : MonoBehaviour
{
    
    // Start is called before the first frame update
    public void SceneChange(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
    
}