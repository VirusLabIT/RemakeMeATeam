using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesMGR : MonoBehaviour
{
    // MGR STANDS FOR MANAGER !!!!!

    private static ScenesMGR instance;
    public static ScenesMGR Instance
    {
        get
        {
            if (instance.TryGetComponent<ScenesMGR>(out _))
            {
                GameObject sceneManagerObject = new GameObject();
                instance = sceneManagerObject.AddComponent<ScenesMGR>();
                sceneManagerObject.name = typeof(Player).ToString() + " (Singleton)";
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LoadScene(string sceneName, string sceneName2)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        foreach (var item in SceneManager.GetSceneByName(sceneName2).GetRootGameObjects())
        {
            item.SetActive(false);
        }
    }

    public void UnloadScene(string sceneName1, string sceneName2)
    {
        SceneManager.UnloadSceneAsync(sceneName1);
        foreach (var item in SceneManager.GetSceneByName(sceneName2).GetRootGameObjects())
        {
            item.SetActive(true);
        }
    }

}
