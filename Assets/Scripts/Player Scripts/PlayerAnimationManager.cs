using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    Animator anim;

    public bool isIdle;
    public bool isWalking;
    public bool isJumping;
    public bool isFalling;
    public bool isDrinking;
    public bool specialAttack;
    public bool isHurted;
    public bool AirAttack;
    public int combo;

    void Start()
    {
        anim = GetComponent<Animator>();   
    }
    void Update()
    {
        anim.SetBool("Idle", isIdle);
        anim.SetBool("Walk", isWalking);   
        anim.SetBool("Jump", isJumping);   
        anim.SetBool("Fall", isFalling);
        anim.SetBool("Drink", isDrinking);
        anim.SetBool("SpecialAttack", specialAttack);
        anim.SetBool("Hurt", isHurted);
            
    }

    public void ExitDamage()
    {
        isHurted = false;
    }

    public void ExitJumpAttack()
    {
       
    }
}
