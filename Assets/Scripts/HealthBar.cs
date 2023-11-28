using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Sprite[] barSprites;
    [SerializeField] private Image fill;
    private Image spriteBar;
    private float playerMaxHealth;
    // Start is called before the first frame update
    void Start()
    {
        spriteBar = transform.Find("Bar/Fill").gameObject.GetComponent<Image>();
        playerMaxHealth = PlayerController.InstanciaPlayerController.GetPlayerMaxHealth();
        InitHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitHealthBar()
    {
        slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = playerMaxHealth;

        SetHealth((int) playerMaxHealth);
    }

    public void SetHealth(int health)
    {
        float indexFloat = (health / playerMaxHealth) * 10f;
        int index = (int)(indexFloat);
        spriteBar.sprite = barSprites[index];

    }
}
