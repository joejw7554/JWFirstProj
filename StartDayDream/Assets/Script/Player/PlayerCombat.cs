using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerInputComponent input;
    private Animator animator;

    private int attackHash;
    private int attackCountHash;
    private int comboIndex=0;
    private int maxComboIndex = 2;


    float timer = 0;

    [Header("Combat")]
    [SerializeField] private float resetTimer=2;


    private void IntializeComponents()
    {
        this.GetRequiredComponent(out input);
        this.GetRequiredComponent(out animator);
    }

    private void InitializeAnimationHashes()
    {
        attackHash = Animator.StringToHash("attack");
        attackCountHash = Animator.StringToHash("attackIndex");
    }
    void Awake()
    {
        IntializeComponents();
        InitializeAnimationHashes();
    }

    void Start()
    {
        input.OnAttackPressed += HandleAttack;
    }

    private void HandleAttack()
    {
        animator.SetTrigger(attackHash);
        animator.SetInteger(attackCountHash, comboIndex);

        if (comboIndex < maxComboIndex) comboIndex++;

        timer = 0f; // 공격 시 타이머 리셋

        Debug.Log($"ComboIndex: {comboIndex}");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"현재 애니메이터 상태: {stateInfo.fullPathHash}");

        if (comboIndex==maxComboIndex) comboIndex = 0;
    }

    void Update()
    {
        if (comboIndex > 0)
        {
            timer += Time.deltaTime;

            if (timer > resetTimer)
            {
                Debug.Log($"timer: {timer}");
                timer = 0f;
                comboIndex = 0;
                Debug.Log("ComboReset");
            }
        }

    }


    private void OnDestroy()
    {
        if (input != null)
        {
            input.OnAttackPressed -= HandleAttack;
        }
    }

}
