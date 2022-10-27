/*
 * file: ManualWindow.cs
 * author: DeamonHook
 * feature: Í¼¼ø´°¿Ú
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillWindow : BaseWindow, IPointerClickHandler
{
    public GameObject entryPrefab;

    public static SkillWindow instance { get; private set; }

    private SkillEntry curSelect = null;

    private RectTransform content;
    private Text introText;

    public override void InitWnd()
    {
        base.InitWnd();
        instance = this;
    }

    private void Awake()
    {
        content = transform.Find("Window/ItemList/Viewport/Content").GetComponent<RectTransform>();
        introText = transform.Find("Window/Introduction/Text").GetComponent<Text>();
    }

    public void ShowIntro(string intro)
    {
        introText.text = intro;
    }

    public void Select(SkillEntry skill)
    {
        if (curSelect == skill)
        {
            print($"Use {curSelect.skillName}");
            GameManager.instance.UseSkill(curSelect.skillName);
            UIManager.instance.SwitchWindow("GameWindow");
        }
        else
        {
            if (curSelect != null)
            {
                curSelect.DeselectMode();
            }
            curSelect = skill;
            skill.SelectMode();
            ShowIntro(Skill.SkillIntro[skill.skillName]);
        }
    }

    public void Refresh(IEnumerable<string> skills)
    {
        foreach (var item in skills)
        {
            print($"{item}");
        }
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        foreach (string skill in skills)
        {
            var entry = Instantiate(entryPrefab, content.transform);
            var originalItem = ResServer.instance.GetObject(skill);
            entry.GetComponent<SkillEntry>().Refresh(
                skill,
                originalItem.GetComponent<SpriteRenderer>().sprite
            );
            entry.GetComponent<SkillEntry>().DeselectMode();
        }
    }


    public void OnPointerClick(PointerEventData ped)
    {
        UIManager.instance.SwitchWindow("GameWindow");
    }
}
