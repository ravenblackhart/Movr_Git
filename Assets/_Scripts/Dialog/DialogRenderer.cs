using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DialogRenderer : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;

    RectTransform rectTransform;

    [SerializeField, TextArea] string text;

    public bool alignedRight;

    public float textPrintSpeed = 25f;

    [SerializeField]
    Color textColor = Color.white;

    [SerializeField]
    Color[] alternateTextColors = { Color.red };

    string textApply;

    string lastText;
    Color lastTextColor;

    [Range(0f, 200f), SerializeField] float textProgress;

    bool textUpdated;

    List<List<CharMod>> charMods = new List<List<CharMod>>();
    List<Color> charColors = new List<Color>();

    public bool dialogRunning;

    double lastEditorTime;

    float lastDialogProgress;

    [HideInInspector]
    public int dialogLineCount;

    [HideInInspector]
    public float textOpacity = 1f;

    [Header("Callbacks")]
    public UnityEvent DialogStartEvent;
    public UnityEvent DialogStopEvent;
    public UnityEvent DialogCharPrintedEvent;

    // Start
    void Start()
    {
        textUpdated = true;
    }

    // Update
    void Update()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        CheckTextUpdate();

        var innitTextReturn = InnitializeText();

        if (innitTextReturn > -1)
            dialogLineCount = innitTextReturn;

        UpdateText();
    }

    // LateUpdate
    void LateUpdate()
    {
        //
    }

    void CheckTextUpdate()
    {
        if (lastText != text || lastTextColor != textColor)
        {
            textUpdated = true;

            lastText = text;
            lastTextColor = textColor;
        }
    }

    int InnitializeText()
    {
        if (!textUpdated)
            return -1;

        charMods.Clear();
        charColors.Clear();

        var wordStart = 0;

        bool[] removalMarks = new bool[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            charMods.Add(new List<CharMod>());
            charColors.Add(textColor);
        }

        for (int i = 0; i + 3 < text.Length; i++)
        {
            if (text[i] == '[' && text[i + 1] == 'c' && text[i + 3] == ']')
            {
                for (int a = i + 3; a + 2 < text.Length; a++)
                {
                    if (text[a] == '[' && text[a + 1] == 'c' && text[a + 2] == ']')
                    {
                        for (int b = i + 3; b < a; b++)
                        {
                            charColors[b] = alternateTextColors[(int)char.GetNumericValue(text[i + 2])];
                        }

                        removalMarks[i] = true;
                        removalMarks[i + 1] = true;
                        removalMarks[i + 2] = true;
                        removalMarks[i + 3] = true;

                        removalMarks[a] = true;
                        removalMarks[a + 1] = true;
                        removalMarks[a + 2] = true;

                        break;
                    }
                }
            }
        }

        for (int i = 0; i + 2 < text.Length; i++)
        {
            if (text[i] == '[' && text[i + 2] == ']' && !removalMarks[i])
            {
                for (int a = i + 3; a + 2 < text.Length; a++)
                {
                    if (text[a] == '[' && text[a + 1] == text[i + 1] && text[a + 2] == ']')
                    {
                        for (int b = i + 3; b < a; b++)
                        {
                            switch (text[i + 1])
                            {
                                case 'n':
                                    charMods[b].Add(CharMod.nextLine);
                                    break;
                                case 'u':
                                    charMods[b].Add(CharMod.up);
                                    break;
                                case 'd':
                                    charMods[b].Add(CharMod.down);
                                    break;
                                case 'w':
                                    charMods[b].Add(CharMod.wobbly);
                                    break;
                                case 's':
                                    charMods[b].Add(CharMod.shaking);
                                    break;
                            }
                        }

                        removalMarks[i] = true;
                        removalMarks[i + 1] = true;
                        removalMarks[i + 2] = true;

                        removalMarks[a] = true;
                        removalMarks[a + 1] = true;
                        removalMarks[a + 2] = true;

                        break;
                    }
                }
            }
        }

        textApply = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (!removalMarks[i])
            {
                textApply += text[i];
            }
        }

        for (int i = text.Length - 1; i >= 0; i--)
        {
            if (removalMarks[i])
            {
                charMods.RemoveAt(i);
                charColors.RemoveAt(i);
            }
        }

        int lineCount = 1;
        int charsThisLine = 0;

        for (int i = 0; i < textApply.Length; i++)
        {
            charsThisLine++;

            if (textApply[i] != ' ')
            {
                for (int a = i; a <= textApply.Length; a++)
                {
                    if (a + 1 < textApply.Length)
                    {
                        //
                    }
                    else
                    {
                        //textApply.Length
                    }
                }
            }

            //charsThisLine++;

            //if (wordStart == -1 && i < textApply.Length)
            //{
            //    if (textApply[i] != ' ')
            //    {
            //        wordStart = i;
            //    }
            //    else
            //    {
            //        continue;
            //    }
            //}

            //bool isWordEnd = false;
            //if (i == textApply.Length)
            //{
            //    isWordEnd = true;
            //}
            //else
            //{
            //    if (textApply[i] == ' ' && i != textApply.Length - 1)
            //    {
            //        isWordEnd = true;
            //    }

            //    if (charMods[i].Contains(CharMod.nextLine))
            //        charsThisLine = 0;
            //}

            //if (isWordEnd)
            //{
            //    var wordEnd = i - 1;

            //    if (charsThisLine > rectTransform.rect.width)
            //    {
            //        charsThisLine = 0;

            //        if (!charMods[wordStart].Contains(CharMod.nextLine))
            //        {
            //            charMods[wordStart].Add(CharMod.nextLine);
            //            lineCount++;
            //        }
            //    }

            //    wordStart = -1;
            //}
        }

        lineCount = SeperateTextLines(textApply, charMods, (int)rectTransform.rect.width);

        for (int i = 0; i < textApply.Length; i++)
        {
            if (i < transform.childCount)
            {
                var dialogChar = transform.GetChild(i).GetComponent<DialogChar>();

                if (dialogChar != null)
                {
                    dialogChar.Innitialize(textApply[i], i);
                }
            }
            else
                InstantiateCharacter(i);
        }

        textUpdated = false;

        return lineCount;
    }

    int SeperateTextLines(string text, List<List<CharMod>> charMods, int maxLineLength, int i = 0, int charsThisLine = 0, int lineCount = 0)
    {
        if (i < text.Length)
        {
            if (charMods[i].Contains(CharMod.nextLine))
            {
                return SeperateTextLines(text, charMods, maxLineLength, i + 1, 0, lineCount + 1);
            }
            else if (text[i] != ' ')
            {
                var wordLength = FindWordEnd(text, i, charMods);

                if (charsThisLine + wordLength > maxLineLength)
                {
                    charMods[i].Add(CharMod.nextLine);

                    return SeperateTextLines(text, charMods, maxLineLength, i + wordLength, wordLength, lineCount + 1);
                }
                else
                {
                    return SeperateTextLines(text, charMods, maxLineLength, i + wordLength, charsThisLine + wordLength, lineCount);
                }
            }
            else
            {
                return SeperateTextLines(text, charMods, maxLineLength, i + 1, charsThisLine + 1, lineCount);
            }
        }
        else
        {
            return lineCount;
        }
    }

    int FindWordEnd(string text, int i, List<List<CharMod>> charMods, int wordLength = 0)
    {
        if (i < text.Length)
        {
            if (text[i] == ' ' || charMods[i].Contains(CharMod.nextLine))
            {
                return wordLength;
            }
            else
            {
                return FindWordEnd(text, i + 1, charMods, wordLength + 1);
            }
        }
        else
        {
            return wordLength;
        }
    }

    void InstantiateCharacter(int i) => Instantiate(characterPrefab, transform).GetComponent<DialogChar>().Innitialize(textApply[i], i);

    void UpdateText()
    {
        var deltaTime = Time.deltaTime;

        #if UNITY_EDITOR
        if (Application.isEditor)
        {
            deltaTime = (float)(EditorApplication.timeSinceStartup - lastEditorTime);

            lastEditorTime = EditorApplication.timeSinceStartup;
        }
        #endif

        if (dialogRunning)
        {
            textProgress += deltaTime * textPrintSpeed;

            if (textProgress >= textApply.Length + 5f)
            {
                StopDialog();

                textProgress = textApply.Length + 5f;
            }

            var ceiledTextProgres = Mathf.CeilToInt(textProgress);

            char currentChar = ceiledTextProgres < textApply.Length ? textApply[ceiledTextProgres] : ' ';

            if (ceiledTextProgres != Mathf.Ceil(lastDialogProgress) && currentChar != ' ' && currentChar != '.')
            {
                DialogCharPrintedEvent.Invoke();
            }

            lastDialogProgress = textProgress;
        }

        int horInd = 0;
        int vertInd = 0;

        Vector2[] textPositions = new Vector2[transform.childCount];

        float textWidth = 0f;
        float textHeight = 0f;

        for (int i = 0; i < textApply.Length; i++)
        {
            if (i < charMods.Count)
                for (int a = 0; a < charMods[i].Count; a++)
                {
                    switch (charMods[i][a])
                    {
                        case CharMod.nextLine:
                            horInd = 0;
                            vertInd--;
                            break;
                    }
                }

            textPositions[i] = new Vector2(horInd, vertInd);

            horInd++;

            if (textApply[i] != ' ')
                textWidth = Mathf.Max(horInd, textWidth);

            textHeight = Mathf.Max(vertInd, textHeight);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            var character = transform.GetChild(i).GetComponent<DialogChar>();

            if (character != null)
            {
                var charColor = i < charColors.Count ? charColors[i] : textColor;

                character.UpdateChar(i < textApply.Length, textProgress, 
                    alignedRight ? textPositions[i].x + (int)rectTransform.rect.width - textWidth : textPositions[i].x, 
                    textPositions[i].y,
                    charColor.ReplaceA(charColor.a * textOpacity),
                    i < charMods.Count ? charMods[i].ToArray() : new CharMod[0]);
            }
        }
    }

    public void DebugStartDialog()
    {
        textProgress = 0f;

        dialogRunning = true;

        #if UNITY_EDITOR
        EditorApplication.update += Update;
        #endif
    }

    public void DebugStartDialog(string text)
    {
        textProgress = 0f;

        dialogRunning = true;

        this.text = text;

        #if UNITY_EDITOR
        EditorApplication.update += Update;
        #endif
    }

    public void DebugStopDialog()
    {
        dialogRunning = false;

        #if UNITY_EDITOR
        EditorApplication.update -= Update;
        #endif
    }

    void OnEnable()
    {
        textUpdated = true;
    }

    void OnDisable()
    {
        #if UNITY_EDITOR
        EditorApplication.update -= Update;
        #endif
    }

    void OnValidate()
    {
        textUpdated = true;
    }

    public void StartDialog()
    {
        textProgress = 0f;

        dialogRunning = true;

        DialogStartEvent.Invoke();
    }

    public void StartDialog(string text)
    {
        this.text = text;

        textProgress = 0f;

        dialogRunning = true;

        DialogStartEvent.Invoke();
    }

    public void StopDialog()
    {
        dialogRunning = false;

        DialogStopEvent.Invoke();
    }

    public void EraseDialog()
    {
        text = "";
    }

    public void HideDialog()
    {
        textProgress = 0f;
    }

    public bool SkipPressed()
    {
        if (dialogRunning)
        {
            textProgress = textApply.Length + 20f;

            StopDialog();

            return false;
        }
        else
        {
            return true;
        }
    }

    public bool CheckRunning()
    {
        if (dialogRunning)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogRenderer))]
public class DialogRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var dialogRenderer = (DialogRenderer)target;

        GUILayout.Space(6f);

        if (dialogRenderer.dialogRunning)
        {
            if (GUILayout.Button("Stop Dialog"))
            {
                dialogRenderer.DebugStopDialog();
            }
        }
        else
        {
            if (GUILayout.Button("Start Dialog"))
            {
                dialogRenderer.DebugStartDialog();
            }
        }
    }
}
#endif

public enum CharMod
{
    nextLine,
    up,
    down,
    wobbly,
    shaking,
}