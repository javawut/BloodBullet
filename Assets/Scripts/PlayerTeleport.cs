using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{

    [SerializeField] private GameObject pontoDestino;

    private void Start()
    {
        pontoDestino.GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.InstanciaPlayerController.transform.position = pontoDestino.transform.position;
        }
    }
}
