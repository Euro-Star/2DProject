using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUIEntry : UIBase
{
    private Image image_Skill;
    private Button button_SkillLevelUP;
    private TextMeshProUGUI text_SkillName;
    private TextMeshProUGUI text_SkillLevel;
    private TextMeshProUGUI text_SkillExplane;
    private TextMeshProUGUI text_NeedMoney;

    private int skillCode;
    private SkillManager skillManager;
    private int Test_SkillLevelMax = 5;

    enum Images
    {
        Image_Skill
    }

    enum Texts
    {
        Text_SkillName,
        Text_SkillLevel,
        Text_SkillExplane,
        Text_NeedMoney
    }

    enum Buttons
    {
        Button_SkillLevelUp
    }

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        text_SkillName = Get<TextMeshProUGUI>((int)Texts.Text_SkillName);
        text_SkillLevel = Get<TextMeshProUGUI>((int)Texts.Text_SkillLevel);
        text_SkillExplane = Get<TextMeshProUGUI>((int)Texts.Text_SkillExplane);
        text_NeedMoney = Get<TextMeshProUGUI>((int)Texts.Text_NeedMoney);

        image_Skill = Get<Image>((int)Images.Image_Skill);

        button_SkillLevelUP = Get<Button>((int)Buttons.Button_SkillLevelUp);
    }

    private void OnEnable()
    {
        if(skillManager == null)
        {
            skillManager = SkillManager.inst;
        }
    }

    private void Start()
    {
        button_SkillLevelUP.onClick.AddListener(OnClicked_SkillLevelUp);
    }

    private void OnClicked_SkillLevelUp()
    {
        if(skillManager.GetSkillData(skillCode).needMoney <= Player.player.inventory.GetMoney())
        {
            Player.player.inventory.UseMoney(skillManager.GetSkillData(skillCode).needMoney);
            skillManager.SkillLevelUp(skillCode);
            UpdateText(skillCode);
        }
    }

    public void Init(int skillCode)
    {
        this.skillCode = skillCode;

        UpdateText(this.skillCode);
    }

    private void UpdateText(int skillCode)
    {
        text_SkillName.text = skillManager.GetSkillData(skillCode).skillName;
        text_SkillExplane.text = skillManager.GetSkillData(skillCode).skillExplane;
        text_NeedMoney.text = skillManager.GetSkillData(skillCode).needMoney.ToString();

        if (skillManager.GetSkillLevel(skillCode) == Test_SkillLevelMax)
        {
            text_SkillLevel.text = "Level : Max";
            button_SkillLevelUP.interactable = false;
        }
        else
        {
            text_SkillLevel.text = "Level :" + skillManager.GetSkillLevel(skillCode).ToString();
        }
    }
}
