using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : UIBase
{
    private static HUD instance;
    public static HUD inst { get { return instance; } }

    public delegate void Dele_UpdateCooltime(int index, float cooltime);
    public Dele_UpdateCooltime dele_UpdateCooltime;

    private Button[] buttons_Skill;
    private Button button_Status;
    private Button button_SkillUI;
    private TextMeshProUGUI[] texts_Cooltime;
    private TextMeshProUGUI text_Money;
    private TextMeshProUGUI text_Level;
    private TextMeshProUGUI text_MaxHp;
    private TextMeshProUGUI text_CurrentHp;
    private Scrollbar bar_Exp;
    private Scrollbar bar_Hp;

    private Inventory inventory;
    private SkillComponent SkillComponent;
    private AbilityComponent abilityComponent;
    private HealthComponent healthComponent;
    private UIManager uIManager;

    Button testButton;
    enum Buttons
    {
        Button_Skill_0,
        Button_Skill_1,
        Button_Skill_2,
        Button_Skill_3,
        Button_Status,
        Button_SkillUI,
        Button_TestButton
    }

    enum Texts
    {
        Text_Money,
        Text_Level,
        Text_MaxHp,
        Text_CurrentHp,
        Text_CoolTime_0,
        Text_CoolTime_1,
        Text_CoolTime_2,
        Text_CoolTime_3
    }

    enum Scrollbars
    {
        Bar_Exp,
        Bar_Hp
    }


    private void Awake()
    {
        dele_UpdateCooltime = UpdateCoolTime;

        instance = this;

        buttons_Skill = new Button[4];
        texts_Cooltime = new TextMeshProUGUI[4];

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Scrollbar>(typeof(Scrollbars));

        buttons_Skill[0] = Get<Button>((int)Buttons.Button_Skill_0);
        buttons_Skill[1] = Get<Button>((int)Buttons.Button_Skill_1);
        buttons_Skill[2] = Get<Button>((int)Buttons.Button_Skill_2);
        buttons_Skill[3] = Get<Button>((int)Buttons.Button_Skill_3);
        button_Status = Get<Button>((int)Buttons.Button_Status);
        button_SkillUI = Get<Button>((int)Buttons.Button_SkillUI);

        texts_Cooltime[0] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_0);
        texts_Cooltime[1] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_1);
        texts_Cooltime[2] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_2);
        texts_Cooltime[3] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_3);

        text_Money = Get<TextMeshProUGUI>((int)Texts.Text_Money);
        text_Level = Get<TextMeshProUGUI>((int)Texts.Text_Level);
        text_MaxHp = Get<TextMeshProUGUI>((int)Texts.Text_MaxHp);
        text_CurrentHp = Get<TextMeshProUGUI>((int)Texts.Text_CurrentHp);

        bar_Exp = Get<Scrollbar>((int)Scrollbars.Bar_Exp);
        bar_Hp = Get<Scrollbar>((int)Scrollbars.Bar_Hp);

        testButton = Get<Button>((int)Buttons.Button_TestButton);

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SkillComponent = Player.player.skillComponent;
        abilityComponent = Player.player.abilityComponent;
        healthComponent = Player.player.healthComponent;
        inventory = Player.player.inventory;
        uIManager = UIManager.inst;

        abilityComponent.ExpChangeEvent += UpdateExp;
        abilityComponent.LevelChangeEvent += UpdateLevel;
        healthComponent.HpChangeEvent += UpdateHp;
        inventory.moneyChangeEvent += UpdateMoneyText;

        foreach (TextMeshProUGUI t in texts_Cooltime)
        {
            t.enabled = false;
        }
    }

    private void Start()
    {
        for (int i = 0; i < buttons_Skill.Length; ++i)
        {
            int index = i; // 파라미터 참조 문제 해결
            buttons_Skill[index].onClick.AddListener(() => OnClicked_Skill(index));
        }

        button_Status.onClick.AddListener(OnClicked_Status);
        button_SkillUI.onClick.AddListener(OnClicked_SkillUI);
        testButton.onClick.AddListener(TestFunc);
    }


    private void OnClicked_Skill(int index)
    {
        SkillComponent.Skill(index);
    }

    private void OnClicked_Status()
    {
        uIManager.UIController(GameUI.StatUI, true);
    }

    private void OnClicked_SkillUI()
    {
        uIManager.UIController(GameUI.SkillUI, true);
    }

    private void TestFunc()
    {
        SceneManager.LoadScene("TestScene");
    }

    private void UpdateMoneyText(object sender, EventArgs eventArgs)
    {
        text_Money.text = Convert.ToString(inventory.GetMoney());
    }

    private void UpdateCoolTime(int index, float cooltime)
    {
        if (!texts_Cooltime[index].isActiveAndEnabled)
        {
            texts_Cooltime[index].enabled = true;
        }

        texts_Cooltime[index].text = Convert.ToString(Mathf.Ceil(cooltime));

        if(Mathf.Ceil(cooltime) == 0)
        {
            texts_Cooltime[index].enabled = false;
        }
    }

    private void UpdateExp(object sender, EventArgs eventArgs)
    {
        bar_Exp.size = (float)abilityComponent.GetExp()/abilityComponent.GetTotalExp();
    }

    private void UpdateLevel(object sender, EventArgs eventArgs)
    {
        text_Level.text = abilityComponent.GetLevel().ToString();
    }

    private void UpdateHp(object sender, EventArgs eventArgs)
    {
        int maxHp = abilityComponent.GetMaxHp();
        int currentHp = abilityComponent.GetHp();

        text_MaxHp.text = maxHp.ToString();
        text_CurrentHp.text = currentHp.ToString();

        bar_Hp.size = (float)currentHp / maxHp;
    }
}
