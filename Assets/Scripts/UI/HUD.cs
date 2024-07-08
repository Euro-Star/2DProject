using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HUD;

public class HUD : UIBase
{
    public delegate void Dele_UpdateCooltime(int index, float cooltime);
    public Dele_UpdateCooltime dele_UpdateCooltime;
    public static HUD instance;

    private Button[] buttons_Skill;
    private TextMeshProUGUI[] texts_Cooltime;
    private TextMeshProUGUI text_money;
    private Scrollbar bar_Exp;

    private Inventory inventory;
    private SkillComponent SkillComponent;
    private AbilityComponent abilityComponent;

    // 테스트 변수
    int Test_MaxExp = 1000;

    enum Buttons
    {
        Button_Skill_0,
        Button_Skill_1,
    }

    enum Texts
    {
        Text_Money,
        Text_CoolTime_0,
        Text_CoolTime_1
    }

    enum Scrollbars
    {
        Bar_Exp
    }


    private void Awake()
    {
        dele_UpdateCooltime = UpdateCoolTime;

        instance = this;

        buttons_Skill = new Button[2];
        texts_Cooltime = new TextMeshProUGUI[2];

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Scrollbar>(typeof(Scrollbars));

        buttons_Skill[0] = Get<Button>((int)Buttons.Button_Skill_0);
        buttons_Skill[1] = Get<Button>((int)Buttons.Button_Skill_1);

        texts_Cooltime[0] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_0);
        texts_Cooltime[1] = Get<TextMeshProUGUI>((int)Texts.Text_CoolTime_1);

        text_money = Get<TextMeshProUGUI>((int)Texts.Text_Money);
        bar_Exp = Get<Scrollbar>((int)Scrollbars.Bar_Exp);
    }

    private void OnEnable()
    {
        SkillComponent = Player.player.skillComponent;
        abilityComponent = Player.player.abilityComponent;
        inventory = Player.player.inventory;

        abilityComponent.ExpChangeEvent += UpdateExp;
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
    }


    private void OnClicked_Skill(int index)
    {
        SkillComponent.Skill(index);
    }

    private void UpdateMoneyText(object sender, EventArgs eventArgs)
    {
        text_money.text = Convert.ToString(inventory.GetMoney());
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
        bar_Exp.size = (float)abilityComponent.GetExp()/Test_MaxExp;
    }
}
