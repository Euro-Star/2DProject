using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using GameUtils;

public class Player : MonoBehaviour
{
    public static Player player;
    public Vector2 inputVec;
    public float speed; public float GetSpeed() { return speed; }
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
    private Animator anim;

    // �÷��̾� Json ������
    private PlayerData playerData;

    // ����ϴ� ����
    private Vector2 lastPos;
    private float CurrentVelo;
    private float attackDelay;

    public Vector2 targetVec { get; set; } // �� Ÿ���� ����
    public Vector2 targetPos { get; set; } // �� Ÿ���� ��ġ

    private void Awake()
    {
        player = this;
        playerData = Utils.JsonDataParse<PlayerData>("PlayerData");
    }

    private void OnEnable()
    {
        lockOn = GetComponent<LockOn>();
        autoMode = GetComponent<AutoMode>();
        inventory = GetComponent<Inventory>();
        abilityComponent = GetComponent<AbilityComponent>();
        skillComponent = GetComponent<SkillComponent>();
        healthComponent = GetComponent<HealthComponent>();
        
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        lastPos = rigidBody.position;
    }

    private void Start()
    {
        LoadPlayerData();
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
        anim.SetFloat("Speed", CurrentVelo);
        UpdateRotate(inputVec.x);
    }

    public void LoadPlayerData()
    {
        abilityComponent.InitPlayerData(playerData);
        healthComponent.InitHealth(playerData.hp);
        inventory.InitInventory(playerData);
    }

    public void SavePlayerData()
    {
        PlayerData saveData = abilityComponent.GetPlayerData();
        saveData.hp = healthComponent.GetMaxHealth();
        saveData.money = inventory.GetMoney();

        Utils.SaveJsonData("PlayerData", playerData);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void AutoDefaultAttack()
    {
        attackDelay += Time.deltaTime;

        if (attackDelay > 0.5f)
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
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
    
        }
    }
}