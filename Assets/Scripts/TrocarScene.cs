using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Importa o SceneManager

public class TrocarScene : MonoBehaviour
{
    public string nomeSceneIr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(nomeSceneIr);
    }
}
