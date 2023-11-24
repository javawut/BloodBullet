using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaCena : MonoBehaviour
{
   public string nomeDaFase;

    void Start()
    {
        
    }
    void Update()
    {
     
    }
    private void CarregarNovaFase()
    {
        SceneManager.LoadScene(nomeDaFase);
    }
    private void OnTriggerEnter2D(Collider2D other)

    {
        if(other.tag == "Player")
        {
            CarregarNovaFase();
        }
    }
}
