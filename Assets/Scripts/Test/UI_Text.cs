using GameUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Text : UIBase
{
    TextMeshProUGUI t;
    Button b;

    enum Texts
    {
        TestText
    }

    enum Buttons
    {
        TestButton
    }

    private void Start()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        t = Get<TextMeshProUGUI>((int)Texts.TestText);
        b = Get<Button>((int)Buttons.TestButton);

        b.onClick.AddListener(OnClickedEvent);
    }

    public void OnClickedEvent()
    {
        t.text = "abc";
    }
}
