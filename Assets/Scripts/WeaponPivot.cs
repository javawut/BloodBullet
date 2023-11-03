using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }
    [SerializeField] private Vector2 pointerPosition;
    private int direction;

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
            scale.y = -1;
        }
        else
        {
            if (direction.x > 0)
            {
                scale.y = 1;
            }
        }

        transform.localScale = scale;
    }
}
