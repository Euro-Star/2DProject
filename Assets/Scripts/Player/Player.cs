using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;
using GameUtils;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public static Player instance;
    public static Player player { get { return instance; } }
    public Player()
    {
        instance = this;
    }

    public Vector2 inputVec;
    public float speed;
    public GameObject defaultAttackSpawnPoint;

    // �÷��̾ ������� ������Ʈ
    public LockOn lockOn;
    public AutoMode autoMode;
    public Inventory inventory;
    public AbilityComponent abilityComponent;
    public SkillComponent skillComponent;
    public HealthComponent healthComponent;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    // �÷��̾� Json ������
    private PlayerData playerData;

    // ����ϴ� ����
    private Vector2 lastPos;
    private float CurrentVelo;
    private float attackDelay;
    private float defaultAttackSpeed = 0.5f;
    private float attackSpeed = 0.5f;
    private bool bDeath = false;

    public Vector2 targetVec { get; set; } // �� Ÿ���� ����
    public Vector2 targetPos { get; set; } // �� Ÿ���� ��ġ
    public Vector2 defaultAttacktarget { get; set; }

    public void SetInputVec(Vector2 input) { inputVec = input; }
    public float GetSpeed() { return speed; }
    public bool IsDeath() { return bDeath; }
    public void SetDeath(bool bDeath) { this.bDeath = bDeath; }
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerData = Utils.JsonDataParse<PlayerData>("PlayerData");

        lockOn = GetComponent<LockOn>();
        autoMode = GetComponent<AutoMode>();
        inventory = GetComponent<Inventory>();
        healthComponent = GetComponent<HealthComponent>();
        abilityComponent = GetComponent<AbilityComponent>();
        skillComponent = GetComponent<SkillComponent>();

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastPos = rigidBody.position;

        DontDestroyOnLoad(this.gameObject);       
    }

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadPlayerData();

        SceneManager.sceneLoaded += LoadSceneEvent;
        healthComponent.DeathEvent += Death;
    }

    // ���� �����Ӹ��� ������Ʈ �Ǵ� �Լ�
    private void FixedUpdate()
    {
        if (inputVec.magnitude > 0.0f)
        {
            Vector2 normalVec = inputVec * speed * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + normalVec);
        }

        CurrentVelo = new Vector2(rigidBody.position.x - lastPos.x, rigidBody.position.y - lastPos.y).magnitude;
        lastPos = rigidBody.position;
    }

    private void Update()
    {
        // �⺻ ���� �ڵ� ���
        AutoDefaultAttack();
    }

    //�������� ���� �Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    private void LateUpdate()
    {
        if (CurrentVelo > 0.0f)
        {
            PlayAnimation((int)CharacterAnim.Run);
        }
        else
        {
            PlayAnimation((int)CharacterAnim.Idle);
        }

        UpdateRotate(inputVec.x);
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        this.gameObject.transform.position = Vector3.zero;
    }

    private void Death(object sender, EventArgs eventArgs)
    {
        anim.SetBool("EditChk", false);
        PlayAnimation((int)CharacterAnim.Death);

        bDeath = true;// ���� ��� ����
        UIManager.inst.UIController(GameUI.DeathUI, true);// ��� UI �ѱ�
    }

    public void DeathAnimBack(bool bchk) 
    {
        anim.SetBool("EditChk", bchk);
    }

    public void AttackSpeedUp(float value)
    {
        attackSpeed /= value;
    }

    public void ReturnAttackSpeed()
    {
        attackSpeed = defaultAttackSpeed;
    }

    public void LoadPlayerData()
    {
        abilityComponent.InitPlayerData(playerData);
        healthComponent.InitHealth(playerData.hp);
        inventory.InitInventory(playerData);
        SkillManager.inst.InitSkillLevel(playerData.skillLevel);
    }

    public void SavePlayerData()
    {
        PlayerData saveData = abilityComponent.GetPlayerData();
        saveData.hp = healthComponent.GetMaxHp();
        saveData.money = inventory.GetMoney();

        Utils.SaveJsonData("PlayerData", playerData);
    }

    void AutoDefaultAttack()
    {
        if(bDeath)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name != "Stage_5")
        {
            if (!PoolManager.inst.IsActiveEnemy())
            {
                return;
            }
        }     

        attackDelay += Time.deltaTime;

        if (attackDelay > attackSpeed)
        {
            PoolManager.inst.PoolDefaultAttack(defaultAttackSpawnPoint.transform.position, targetVec);
            attackDelay = 0f;
        }
    }

    // �¿� ĳ���� ȸ��
    public void UpdateRotate(float x)
    {
        if (x != 0)
        {
            if (x > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
    
        }
    }

    public void PlayAnimation(int num)
    {
        switch (num)
        {
            case 0: //Idle
                anim.SetFloat("RunState", 0f);
                break;

            case 1: //Run
                anim.SetFloat("RunState", 0.5f);
                break;

            case 2: //Death
                anim.SetTrigger("Die");
                break;

            case 3: //Stun
                anim.SetFloat("RunState", 1.0f);
                break;

            case 4: //Attack Sword
                anim.SetTrigger("Attack");
                anim.SetFloat("AttackState", 0.0f);
                anim.SetFloat("NormalState", 0.0f);
                break;

            case 5: //Attack Bow
                anim.SetTrigger("Attack");
                anim.SetFloat("AttackState", 0.0f);
                anim.SetFloat("NormalState", 0.5f);
                break;

            case 6: //Attack Magic
                anim.SetTrigger("Attack");
                anim.SetFloat("AttackState", 0.0f);
                anim.SetFloat("NormalState", 1.0f);
                break;

            case 7: //Skill Sword
                anim.SetTrigger("Attack");
                anim.SetFloat("AttackState", 1.0f);
                anim.SetFloat("NormalState", 0.0f);
                break;

            case 8: //Skill Bow
                anim.SetTrigger("Attack");
                anim.SetFloat("AttackState", 1.0f);
                anim.SetFloat("NormalState", 0.5f);
                break;

            case 9: //Skill Magic
                anim.SetTrigger("Attack");
                anim.SetFloat("AttackState", 1.0f);
                anim.SetFloat("NormalState", 1.0f);
                break;
        }
    }
}
