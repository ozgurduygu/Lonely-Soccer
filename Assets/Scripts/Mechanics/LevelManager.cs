using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int CurrentLevel
    {
        get => SceneManager.GetActiveScene().buildIndex;
    }
    
    public static int NextLevel
    {
        get
        {
            var levelCount = SceneManager.sceneCountInBuildSettings;
            var nextLevel = CurrentLevel + 1;
            
            if(nextLevel > levelCount - 1)
            {
                return 0;
            }
            else
            {
                return nextLevel;
            }
        }
    }

    public static void LoadLevel(int level = 0)
    {
        TouchController.active = true;
        SceneManager.LoadScene(level);
    }
}
