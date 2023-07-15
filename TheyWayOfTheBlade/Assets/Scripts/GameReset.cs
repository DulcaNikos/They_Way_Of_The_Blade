using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{
    public GameObject[] enemies;

    void Update()
    {
        if (!enemies[0].activeInHierarchy && !enemies[1].activeInHierarchy && !enemies[2].activeInHierarchy)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
