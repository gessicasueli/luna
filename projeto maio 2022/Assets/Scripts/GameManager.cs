using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string guiName;

    [SerializeField] private string levelName;

    [SerializeField] private GameObject playerAndCameraPrefab;




    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(guiName);
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive).completed += operation =>
        {
            Scene levelScene = default;

            for (int i = 0; 1 < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == levelName)
                {
                    levelScene = SceneManager.GetSceneAt(i);
                    break;
                }
            }

            if (levelScene != default) SceneManager.SetActiveScene(levelScene);

            Vector3 playerStartPosition = GameObject.Find("PlayerStart").transform.position;

            Instantiate(playerAndCameraPrefab, playerStartPosition, Quaternion.identity);


        };

    }
}
    
    
    
    
    
    

       
    
