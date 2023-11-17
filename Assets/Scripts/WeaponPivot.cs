using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPivot : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }
    [SerializeField] private Vector2 pointerPosition;
    private int direction;
    //[SerializeField] private TMP_Text text1;
    //[SerializeField] private TMP_Text text2;

    // Update is called once per frame
    void Update()
    {
        GetMousePosition();
        WeaponRotate();
    }

    private void GetMousePosition()
    {
        Vector3 pointerAux = Input.mousePosition;
        pointerAux.z = 0;
        PointerPosition = Camera.main.ScreenToWorldPoint(pointerAux);
        //text1.text = pointerAux.x + " " + pointerAux.x;
        //text2.text = PointerPosition.x + " " + PointerPosition.y;
    }
    private void WeaponRotate(){
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        FlipSpriteWeapon(direction);
        
    }

    private void FlipSpriteWeapon(Vector2 direction){
        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            
            PlayerController.InstanciaPlayerController.FlipSprite(-1);
            PlayerController.InstanciaPlayerController.SetLocalScale(-1);
            PlayerController.InstanciaPlayerController.SetIsFacingRight(false);
            scale.y = -1;
            scale.x = -1;

        }
        else
        {
            if (direction.x > 0)
            {
                
                PlayerController.InstanciaPlayerController.FlipSprite(1);
                PlayerController.InstanciaPlayerController.SetLocalScale(1);
                PlayerController.InstanciaPlayerController.SetIsFacingRight(true);
                scale.y = 1;
                scale.x = 1;
            }
        }

        transform.localScale = scale;
    }


}
