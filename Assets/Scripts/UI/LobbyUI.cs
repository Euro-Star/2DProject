using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyUI : UIBase, IPointerClickHandler
{
    private TextMeshProUGUI text_TouchToContinue;
    private float alpha = 0f;
    private bool bFade = true;

    enum Texts
    {
        Text_TouchToContinue
    }

    private void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));

        text_TouchToContinue = Get<TextMeshProUGUI>((int)Texts.Text_TouchToContinue);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LoadingSceneManager.LoadScene("Stage_1");
    }

    private void FixedUpdate()
    {
        if(bFade)
        {
            alpha += Time.deltaTime;
            if(alpha >= 1.0f)
            {
                bFade = false;
            }
        }
        else
        {
            alpha -= Time.deltaTime;
            if(alpha <= 0.0f) 
            {
                bFade = true;
            }
        }
        
        text_TouchToContinue.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }
}
