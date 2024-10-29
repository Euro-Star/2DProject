using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : UIBase
{
    [SerializeField]
    private GameObject skillUIEntry;

    private RectTransform content;
    private Button button_Close;

    enum UIObjects
    {
        SkillContent
    }

    enum Buttons
    {
        Button_Close
    }

    private void Awake()
    {
        Bind<RectTransform>(typeof(UIObjects));
        Bind<Button>(typeof(Buttons));

        content = Get<RectTransform>((int)UIObjects.SkillContent);
        button_Close = Get<Button>((int)Buttons.Button_Close);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < SkillManager.inst.SkillLength(); ++i)
        {
            GameObject obj = Instantiate(skillUIEntry, content);
            obj.GetComponent<SkillUIEntry>().Init(i);
        }

        button_Close.onClick.AddListener(OnClicked_Close);
    }

    private void OnClicked_Close()
    {
        UIManager.inst.UIController(GameUI.SkillUI, false);
    }
}
