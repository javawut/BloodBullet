using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Sprite[] barSprites;
    [SerializeField] private Image fill;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        InitHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitHealthBar()
    {
        slider = GetComponent<Slider>();

        int maxValue = PlayerController.InstanciaPlayerController.GetPlayerMaxHealth();
        slider.minValue = 0;
        slider.maxValue = maxValue;

        SetHealth(maxValue);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

    }

    public void ChangeBarSprite(int playerHealth)
    {
        spriteRenderer.sprite = barSprites[playerHealth];
    }
}
