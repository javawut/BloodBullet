using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterScenes : MonoBehaviour
{
   public void Trocar(string nome)
   {
     SceneManager.LoadScene(nome);
   }
}
