using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerInputComponent input;
    private Animator animator;
    private int attackHash;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input =GetComponent<PlayerInputComponent>();
        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("attack");

        input.OnAttackPressed += HandleAttack;
    }

    // Update is called once per frame

    private void HandleAttack()
    {
        animator.SetTrigger(attackHash);
    }

    private void OnDestroy()
    {
        if(input!=null)
        {
            input.OnAttackPressed -= HandleAttack;
        }
    }

}
