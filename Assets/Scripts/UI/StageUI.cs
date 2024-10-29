using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameUtils;

public class StageUI : UIBase
{
    [SerializeField]
    private GameObject stageUIEntry;

    private RectTransform content;
    private Button button_Close;

    private int maxStage = 5;
    private Dictionary<string, StageData> stageData;

    enum UIObjects
    {
        StageContent
    }

    enum Buttons
    {
        Button_Close
    }

    private void Awake()
    {
        Bind<RectTransform>(typeof(UIObjects));
        Bind<Button>(typeof(Buttons));
       
        content = Get<RectTransform>((int)UIObjects.StageContent);
        button_Close = Get<Button>((int)Buttons.Button_Close);

        stageData = Utils.JsonToDictionary<string, StageData>("StageData");

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        for (int i = 1; i <= maxStage; ++i)
        {
            GameObject obj = Instantiate(stageUIEntry, content);          
            obj.GetComponent<StageUIEntry>().Init(i, stageData["Stage_" + i].openLevel);
        }

        button_Close.onClick.AddListener(OnClicked_Close);
    }

    private void OnClicked_Close()
    {
        UIManager.inst.UIController(GameUI.StageUI, false);
    }
}
