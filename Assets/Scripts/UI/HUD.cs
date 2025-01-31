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
    private Button button_StatusUI;
    private Button button_SkillUI;
    private Button button_StageUI;
    private Button button_Auto;
    private Image[] images_SkillCoolTime;
    private TextMeshProUGUI[] texts_Cooltime;
    private TextMeshProUGUI text_Money;
    private TextMeshProUGUI text_Level;
    private TextMeshProUGUI text_MaxHp;
    private TextMeshProUGUI text_CurrentHp;
    private TextMeshProUGUI text_MaxBossHp;
    private TextMeshProUGUI text_CurrentBossHp;
    private Scrollbar bar_Exp;
    private Scrollbar bar_Hp;
    private Scrollbar bar_BossHp;

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
        Button_Stage,
        Button_TestButton,
        Button_Auto
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
        Text_CoolTime_3,
        Text_CurrentBossHp,
        Text_MaxBossHp
    }

    enum Images
    {
        Image_SkillCoolTime_0,
        Image_SkillCoolTime_1,
        Image_SkillCoolTime_2,
        Image_SkillCoolTime_3,
    }


    enum Scrollbars
    {
        Bar_Exp,
        Bar_Hp,
        Bar_BossHp
    }


    private void Awake()
    {
        dele_UpdateCooltime = UpdateCoolTime;

        instance = this;

        buttons_Skill = new Button[4];
        texts_Cooltime = new TextMeshProUGUI[4];
        images_SkillCoolTime = new Image[4];

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Scrollbar>(typeof(Scrollbars));
        Bind<Image>(typeof(Images));

        buttons_Skill[0] = Get<Button>((int)Buttons.Button_Skill_0);
        buttons_Skill[1] = Get<Button>((int)Buttons.Button_Skill_1);
        buttons_Skill[2] = Get<Button>((int)Buttons.Button_Skill_2);
        buttons_Skill[3] = Get<Button>((int)Buttons.Button_Skill_3);
        button_StatusUI = Get<Button>((int)Buttons.Button_Status);
        button_SkillUI = Get<Button>((int)Buttons.Button_SkillUI);
        button_StageUI = Get<Button>((int)Buttons.Button_Stage);
        button_Auto = Get<Button>((int)Buttons.Button_Auto);

        texts_Cooltime[0] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_0);
        texts_Cooltime[1] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_1);
        texts_Cooltime[2] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_2);
        texts_Cooltime[3] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_3);

        text_Money = Get<TextMeshProUGUI>((int)Texts.Text_Money);
        text_Level = Get<TextMeshProUGUI>((int)Texts.Text_Level);
        text_MaxHp = Get<TextMeshProUGUI>((int)Texts.Text_MaxHp);
        text_CurrentHp = Get<TextMeshProUGUI>((int)Texts.Text_CurrentHp);

        text_CurrentBossHp = Get<TextMeshProUGUI>((int)Texts.Text_CurrentBossHp);
        text_MaxBossHp = Get<TextMeshProUGUI>((int)Texts.Text_MaxBossHp);

        images_SkillCoolTime[0] = Get<Image>((int)Images.Image_SkillCoolTime_0);
        images_SkillCoolTime[1] = Get<Image>((int)Images.Image_SkillCoolTime_1);
        images_SkillCoolTime[2] = Get<Image>((int)Images.Image_SkillCoolTime_2);
        images_SkillCoolTime[3] = Get<Image>((int)Images.Image_SkillCoolTime_3);

        bar_Exp = Get<Scrollbar>((int)Scrollbars.Bar_Exp);
        bar_Hp = Get<Scrollbar>((int)Scrollbars.Bar_Hp);
        bar_BossHp = Get<Scrollbar>((int)Scrollbars.Bar_BossHp);

        testButton = Get<Button>((int)Buttons.Button_TestButton);

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        foreach (TextMeshProUGUI t in texts_Cooltime)
        {
            t.enabled = false;
        }
    }

    private void Start()
    {
        Player.player.abilityComponent.ExpChangeEvent += UpdateExp;
        Player.player.abilityComponent.LevelChangeEvent += UpdateLevel;
        Player.player.healthComponent.HpChangeEvent += UpdateHp;
        Player.player.inventory.moneyChangeEvent += UpdateMoneyText;

        for (int i = 0; i < buttons_Skill.Length; ++i)
        {
            int index = i; // 파라미터 참조 문제 해결
            buttons_Skill[index].onClick.AddListener(() => OnClicked_Skill(index));
        }

        button_StatusUI.onClick.AddListener(OnClicked_Status);
        button_SkillUI.onClick.AddListener(OnClicked_SkillUI);
        button_StageUI.onClick.AddListener(OnClicked_Stage);
        button_Auto.onClick.AddListener(OnClicked_Auto);

        SceneManager.sceneLoaded += LoadSceneEvent;
        Player.player.playerDataChangeEvent += InitHUD;

        // 테스트 버튼
        testButton.onClick.AddListener(TestFunc);
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        bar_BossHp.gameObject.SetActive(false);
    }

    private void InitHUD(object sender, EventArgs eventArgs)
    {
        UpdateExp(null, null);
        UpdateLevel(null, null);
        UpdateHp(null, null);
        UpdateMoneyText(null, null);

        for(int i = 0; i < buttons_Skill.Length; ++i)
        {
            texts_Cooltime[i].enabled = false;
            images_SkillCoolTime[i].fillAmount = 0;
        }     
    }

    private void OnClicked_Skill(int index)
    {
        Player.player.skillComponent.Skill(index);
    }

    private void OnClicked_Status()
    {
        UIManager.inst.UIController(GameUI.StatUI, true);
    }

    private void OnClicked_SkillUI()
    {
        UIManager.inst.UIController(GameUI.SkillUI, true);
    }

    private void OnClicked_Stage()
    {
        UIManager.inst.UIController(GameUI.StageUI, true);
    }

    private void OnClicked_Auto()
    {
        if(Player.player.autoMode.IsAutoMode())
        {
            Player.player.autoMode.SetAutoMode(false);
            button_Auto.image.color = new Color(1f, 1f, 1f, 0.25f);
        }
        else
        {
            Player.player.autoMode.SetAutoMode(true);
            button_Auto.image.color = new Color(1f, 1f, 1f, 1f);
        }
        
    }

    private void TestFunc()
    {
        SceneManager.LoadScene("TestScene");
    }

    private void UpdateMoneyText(object sender, EventArgs eventArgs)
    {
        text_Money.text = Convert.ToString(Player.player.inventory.GetMoney());
    }

    private void UpdateCoolTime(int index, float cooltime)
    {
        if (!texts_Cooltime[index].isActiveAndEnabled)
        {
            texts_Cooltime[index].enabled = true;
        }

        texts_Cooltime[index].text = Convert.ToString(Mathf.Ceil(cooltime));
        images_SkillCoolTime[index].fillAmount = cooltime / SkillManager.inst.GetSkillData(index).coolTime;

        if (Mathf.Ceil(cooltime) == 0)
        {
            texts_Cooltime[index].enabled = false;
            images_SkillCoolTime[index].fillAmount = 0;
        }
    }

    private void UpdateExp(object sender, EventArgs eventArgs)
    {
        bar_Exp.size = (float)Player.player.abilityComponent.GetExp()/Player.player.abilityComponent.GetTotalExp();
    }

    private void UpdateLevel(object sender, EventArgs eventArgs)
    {
        text_Level.text = Player.player.abilityComponent.GetLevel().ToString();
    }

    private void UpdateHp(object sender, EventArgs eventArgs)
    {
        int maxHp = Player.player.abilityComponent.GetMaxHp();
        int currentHp = Player.player.abilityComponent.GetHp();

        text_MaxHp.text = maxHp.ToString();
        text_CurrentHp.text = currentHp.ToString();

        bar_Hp.size = (float)currentHp / maxHp;
    }

    public void UpdateBossHP(int maxHp, int currentHp)
    {
        if(!bar_BossHp.IsActive())
        {
            bar_BossHp.gameObject.SetActive(true);
        }

        text_MaxBossHp.text = maxHp.ToString();
        text_CurrentBossHp.text = currentHp.ToString();

        bar_BossHp.size = (float)currentHp / maxHp;
    }
}
