using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public string sceneToLoad;
    // Start is called before the first frame update
    public void StartGame()
    {
        Application.LoadLevel(0);
    }
    void Start()
    {

    }
    void LoadGameScene()
    {
        // Загружаем сцену
        SceneManager.LoadScene(sceneToLoad);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
