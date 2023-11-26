using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimation : MonoBehaviour
{
    private Animator animatorWeapon;
    private bool isPlayerAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        animatorWeapon = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetAnimations();
    }

    private void GetAnimations() {
        animatorWeapon.SetBool("Attack", isPlayerAttacking);
    }

    public void SetAttack(bool flag) {
        isPlayerAttacking = flag;
    }
}
