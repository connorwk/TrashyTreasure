using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrashyTreasure
{
    //Currently all hard-coded values. Not enough implemented yet but easy to fix later.
    public class SplashController : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(LoadSceneDelay(2));
        }

        IEnumerator LoadSceneDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(1);
        }
    }
}
