// Shows upgrade panel when player wins a battle. Player picks a mask from inventory to upgrade.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    static UpgradeUI instance;
    GameObject panel;
    List<Button> upgradeButtons = new List<Button>();

    void Awake()
    {
        instance = this;
    }

    public static void Show()
    {
        if (instance != null) instance.ShowPanel();
    }

    void ShowPanel()
    {
        if (panel != null) { panel.SetActive(true); return; }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var go = new GameObject("Upgrade Canvas");
            canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            go.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            go.AddComponent<GraphicRaycaster>();
        }

        panel = new GameObject("Upgrade Panel");
        panel.transform.SetParent(canvas.transform, false);

        Image panelBg = panel.AddComponent<Image>();
        panelBg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.2f, 0.3f);
        rt.anchorMax = new Vector2(0.8f, 0.7f);
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        var title = new GameObject("Title");
        title.transform.SetParent(panel.transform, false);
        Text titleText = title.AddComponent<Text>();
        titleText.text = "Victory! Choose a mask to upgrade:";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 24;
        titleText.alignment = TextAnchor.MiddleCenter;
        RectTransform titleRt = title.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.1f, 0.7f);
        titleRt.anchorMax = new Vector2(0.9f, 0.9f);
        titleRt.offsetMin = titleRt.offsetMax = Vector2.zero;

        string[] maskNames = { "", "Rock", "Ski", "Bandana", "Hospital", "Spa", "Gas", "Tiki" };

        for (int i = 0; i < 3; i++)
        {
            var btnObj = new GameObject("Upgrade Button " + (i + 1));
            btnObj.transform.SetParent(panel.transform, false);
            Button btn = btnObj.AddComponent<Button>();
            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.3f, 0.6f, 0.3f);

            RectTransform btnRt = btnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.15f + i * 0.25f, 0.25f);
            btnRt.anchorMax = new Vector2(0.35f + i * 0.25f, 0.55f);
            btnRt.offsetMin = btnRt.offsetMax = Vector2.zero;

            var textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            Text btnText = textObj.AddComponent<Text>();
            int slot = i;
            btnText.text = "Slot " + (i + 1);
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.fontSize = 16;
            btnText.alignment = TextAnchor.MiddleCenter;
            RectTransform textRt = textObj.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = textRt.offsetMax = Vector2.zero;

            upgradeButtons.Add(btn);
            int slotCopy = i;
            btn.onClick.AddListener(() => OnUpgradeClicked(slotCopy));
        }
    }

    void OnUpgradeClicked(int slot)
    {
        if (slot >= 0 && slot < player.maskInventory.Count)
        {
            while (player.maskLevels.Count <= slot) player.maskLevels.Add(1);
            player.maskLevels[slot]++;
        }
        panel.SetActive(false);
        game_states.FinishPostBattle();
    }

    void Update()
    {
        if (panel != null && panel.activeSelf && upgradeButtons.Count == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                string label = "Slot " + (i + 1);
                if (i < player.maskInventory.Count)
                {
                    int type = player.maskInventory[i];
                    string[] names = { "", "Rock", "Ski", "Bandana", "Hospital", "Spa", "Gas", "Tiki" };
                    int lvl = i < player.maskLevels.Count ? player.maskLevels[i] : 1;
                    label = names[type] + " Lv." + lvl;
                }
                upgradeButtons[i].GetComponentInChildren<Text>().text = label;
                upgradeButtons[i].interactable = i < player.maskInventory.Count;
            }
        }
    }
}
