using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;
using GameUtils;

public class Player : MonoBehaviour
{
    public static Player instance;
    public static Player player { get { return instance; } }
    public Vector2 inputVec;
    public float speed; public float GetSpeed() { return speed; }
    public GameObject defaultAttackSpawnPoint;

    // 플레이어가 사용중인 컴포넌트
    public LockOn lockOn;
    public AutoMode autoMode;
    public Inventory inventory;
    public AbilityComponent abilityComponent;
    public SkillComponent skillComponent;
    public HealthComponent healthComponent;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    // 플레이어 Json 데이터
    private PlayerData playerData;

    // 사용하는 변수
    private Vector2 lastPos;
    private float CurrentVelo;
    private float attackDelay;
    private float defaultAttackSpeed = 0.5f;
    private float attackSpeed = 0.5f;

    public Vector2 targetVec { get; set; } // 적 타겟의 벡터
    public Vector2 targetPos { get; set; } // 적 타겟의 위치

    private void Awake()
    {
        instance = this;
        playerData = Utils.JsonDataParse<PlayerData>("PlayerData");

        DontDestroyOnLoad(this.gameObject);
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

        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    // 물리 프레임마다 업데이트 되는 함수
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
        // 기본 공격 자동 사용
        AutoDefaultAttack();
    }

    //프레임이 종료 되기 전 실행되는 생명주기 함수
    private void LateUpdate()
    {
        anim.SetFloat("Speed", CurrentVelo);
        UpdateRotate(inputVec.x);
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        this.gameObject.transform.position = Vector3.zero;
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

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void AutoDefaultAttack()
    {
        attackDelay += Time.deltaTime;

        if (attackDelay > attackSpeed)
        {
            PoolManager.inst.PoolDefaultAttack(defaultAttackSpawnPoint.transform.position, targetVec);
            attackDelay = 0f;
        }
    }

    // 좌우 캐릭터 회전
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
