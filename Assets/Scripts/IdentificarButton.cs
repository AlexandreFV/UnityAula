using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentificarButton : MonoBehaviour
{

    public int next;
    public Minigame minigame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress(int nextlevel)
    {
        minigame.CompararPosicao(nextlevel);
    }
}
