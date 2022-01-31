using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogChar : MonoBehaviour
{
    TextMeshProUGUI text;

    char character;
    int index;

    public void Innitialize(char character, int index)
    {
        this.character = character;
        this.index = index;
    }

    public void UpdateChar(bool enabled, float textProgress, float horInd, float vertInd, Color color, CharMod[] charMods)
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        text.enabled = enabled && index < textProgress;

        if (enabled)
            EnabledUpdate(textProgress, horInd, vertInd, color, charMods);
        else
            DisabledUpdate();
    }

    void EnabledUpdate(float textProgress, float horInd, float vertInd, Color color, CharMod[] charMods)
    {
        text.text = "" + character;

        var progress = Mathf.Clamp01((textProgress - index) * 0.2f);

        var alpha = Mathf.Pow(progress, 2f);

        var movementProgress = 1f - Mathf.Pow(progress - 1f, 2f);

        text.color = new Color(color.r, color.g, color.b, color.a * alpha);

        text.transform.localPosition = new Vector2(horInd, vertInd * 1.5f) + new Vector2(1.2f, -1f) * (1f - movementProgress);

        for (int i = 0; i < charMods.Length; i++)
        {
            switch (charMods[i])
            {
                case CharMod.up:
                    break;
                case CharMod.down:
                    break;
                case CharMod.wobbly:
                    Wobble(progress, horInd, vertInd);
                    break;
                case CharMod.shaking:
                    Shake(progress, horInd, vertInd);
                    break;
            }
        }
    }

    void DisabledUpdate()
    {
        text.text = "";
    }

    public void Disable()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        text.text = "";
    }

    void Wobble(float progress, float horInd, float vertInd)
    {
        text.transform.localPosition += Vector3.up * Mathf.Sin(horInd + Time.time * 3f) * progress * 0.15f;
    }

    void Shake(float progress, float horInd, float vertInd)
    {
        text.transform.localPosition += Vector3.up * Mathf.Sin(horInd + vertInd + Time.time * 60f) * progress * 0.05f + Vector3.right * Mathf.Cos(horInd + Time.time * 30f) * progress * 0.05f;
    }
}
