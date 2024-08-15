using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class UISceneService : MonoBehaviour, IService
    {
        [SerializeField] private string _uiSceneName = "UI";

        IEnumerator IService.Init()
        {
            yield return SceneManager.LoadSceneAsync(_uiSceneName, LoadSceneMode.Additive);
        }

        void IService.Deinit()
        {
            SceneManager.UnloadSceneAsync(_uiSceneName);
        }
    }
}
