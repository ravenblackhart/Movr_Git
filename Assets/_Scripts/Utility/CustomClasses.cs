using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomClasses
{
    #region Extentions

    public static Vector2 Abs(this Vector2 a) => new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));

    public static Vector3 Abs(this Vector3 a) => new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));

    public static float AsAngle(this Vector2 a) => Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;

    public static float AsAngle(this Vector3 a) => Mathf.Atan2(a.x, a.z) * Mathf.Rad2Deg;

    public static Vector2 AsVector2(this float a) => new Vector2(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad));

    public static Vector3 AsVector3(this Vector2 a) => new Vector3(a.x, a.y);

    public static Vector2 AsVector2(this Vector3 a) => new Vector2(a.x, a.y);

    public static Vector2 RemoveX(this Vector2 a) => new Vector2(0f, a.y);

    public static Vector2 RemoveY(this Vector2 a) => new Vector2(a.x, 0f);

    public static Vector3 RemoveX(this Vector3 a) => new Vector3(0f, a.y, a.z);

    public static Vector3 RemoveY(this Vector3 a) => new Vector3(a.x, 0f, a.z);

    public static Vector3 RemoveZ(this Vector3 a) => new Vector3(a.x, a.y, 0f);

    public static Vector2 ReplaceX(this Vector2 a, float b) => new Vector2(b, a.y);

    public static Vector2 ReplaceY(this Vector2 a, float b) => new Vector2(a.x, b);

    public static Vector3 ReplaceX(this Vector3 a, float b) => new Vector3(b, a.y, a.z);

    public static Vector3 ReplaceY(this Vector3 a, float b) => new Vector3(a.x, b, a.z);

    public static Vector3 ReplaceZ(this Vector3 a, float b) => new Vector3(a.x, a.y, b);

    public static Color ReplaceR(this Color a, float b) => new Color(b, a.g, a.b, a.a);

    public static Color ReplaceG(this Color a, float b) => new Color(a.r, b, a.b, a.a);

    public static Color ReplaceB(this Color a, float b) => new Color(a.r, a.g, b, a.a);

    public static Color ReplaceA(this Color a, float b) => new Color(a.r, a.g, a.b, b);

    public static Vector2 Make2d(this Vector3 a) => new Vector2(a.x, a.z);

    public static Vector3 Make3d(this Vector2 a) => new Vector3(a.x, 0f, a.y);

    public static Transform[] GetChildAll(this Transform a) => GetAllChildrenRecursive(a).ToArray();

    public static GameObject[] GetChildAll(this GameObject a) => GetAllChildrenRecursive(a).ToArray();

    public static Component[] GetComponentAll(this Transform a, System.Type type) => GetAllComponentsFromList(GetAllChildrenRecursive(a), type).ToArray();

    public static Component[] GetComponentAll(this GameObject a, System.Type type) => GetAllComponentsFromList(GetAllChildrenRecursive(a.transform), type).ToArray();

    public static void Align(this Transform a, Transform b) { a.position = b.position; a.rotation = b.rotation; }

    public static void Align(this Transform[] a, Transform[] b) { for (int i = 0; i < Mathf.Min(a.Length, b.Length); i++) { a[i].Align(b[i]); } }

    public static Vector3 Damp(this Vector3 v, Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static float[] Normalize(this float[] a)
    {
        float total = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            total += a[i];
        }

        for (int i = 0; i < a.Length; i++)
        {
            a[i] /= total;
        }

        return a;
    }

    public static Vector2[] Normalize(this Vector2[] a)
    {
        float total = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            total += a[i].magnitude;
        }

        for (int i = 0; i < a.Length; i++)
        {
            a[i] /= total;
        }

        return a;
    }

    public static Vector3[] Normalize(this Vector3[] a)
    {
        float total = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            total += a[i].magnitude;
        }

        for (int i = 0; i < a.Length; i++)
        {
            a[i] /= total;
        }

        return a;
    }

    public static Rect GetWorldRect(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        // Get the bottom left corner.
        Vector3 position = corners[0];

        Vector2 size = new Vector2(Vector3.Distance(corners[0], corners[3]), Vector3.Distance(corners[0], corners[1]));

        return new Rect(position, size);
    }

    // Testing Needed
    public static Quaternion TransformRotation(this Transform a, Quaternion b) => a.rotation * b;

    // Testing Needed 
    public static Quaternion InverseTransformRotation(this Transform a, Quaternion b) => Quaternion.Inverse(a.rotation) * b;

    #endregion Extentions

    #region Functions

    public static Vector2 DivideScale(Vector2 a, Vector2 b) => new Vector2(DivideSafe(a.x, b.x), DivideSafe(a.y, b.y));

    public static Vector3 DivideScale(Vector3 a, Vector3 b) => new Vector3(DivideSafe(a.x, b.x), DivideSafe(a.y, b.y), DivideSafe(a.z, b.z));

    public static float DivideSafe(float a, float b)
    {
        var output = a / b;

        if (output == float.NaN)
            return 0f;

        return output;
    }

    public static float ClampAbsolute(float value, float max)
    {
        return Mathf.Clamp(value, -Mathf.Abs(max), Mathf.Abs(max));
    }

    public static float MinAbsolute(float a, float b)
    {
        return Mathf.Abs(a) < Mathf.Abs(b) ? a : b;
    }

    public static float MaxAbsolute(float a, float b)
    {
        return Mathf.Abs(a) > Mathf.Abs(b) ? a : b;
    }

    public static float RepeatAbsolute(float value, float max)
    {
        max = Mathf.Abs(max);

        if (value < -max)
        {
            value += max * 2f;
        }
        else if (value > max)
        {
            value -= max * 2f;
        }

        return value;
    }

    public static float Damp(float a, float b, float lambda, float dt)
    {
        return Mathf.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static Vector2 Damp(Vector2 a, Vector2 b, float lambda, float dt)
    {
        return Vector2.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static Vector3 Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static float DampAngle(float a, float b, float lambda, float dt)
    {
        return Mathf.LerpAngle(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

    public static float RemapLerp(float a1, float b1, float a2, float b2, float t)
    {
        return Mathf.Lerp(a1, b1, Mathf.InverseLerp(a2, b2, t));
    }

    public static Vector2 RemapLerp(Vector2 a1, Vector2 b1, float a2, float b2, float t)
    {
        return Vector2.Lerp(a1, b1, Mathf.InverseLerp(a2, b2, t));
    }

    public static Vector3 RemapLerp(Vector3 a1, Vector3 b1, float a2, float b2, float t)
    {
        return Vector3.Lerp(a1, b1, Mathf.InverseLerp(a2, b2, t));
    }

    public static Quaternion RemapLerp(Quaternion a1, Quaternion b1, float a2, float b2, float t)
    {
        return Quaternion.Lerp(a1, b1, Mathf.InverseLerp(a2, b2, t));
    }

    public static Color RemapLerp(Color a1, Color b1, float a2, float b2, float t)
    {
        return Color.Lerp(a1, b1, Mathf.InverseLerp(a2, b2, t));
    }

    private static List<Transform> GetAllChildrenRecursive(Transform a)
    {
        var listOfChildren = new List<Transform>();

        for (int i = 0; i < a.childCount; i++)
        {
            listOfChildren.Add(a.GetChild(i));
            listOfChildren.AddRange(GetAllChildrenRecursive(a.GetChild(i)));
        }

        return listOfChildren;
    }

    private static List<GameObject> GetAllChildrenRecursive(GameObject a)
    {
        var listOfChildren = new List<GameObject>();

        for (int i = 0; i < a.transform.childCount; i++)
        {
            listOfChildren.Add(a.transform.GetChild(i).gameObject);
            listOfChildren.AddRange(GetAllChildrenRecursive(a.transform.GetChild(i).gameObject));
        }

        return listOfChildren;
    }

    private static List<Component> GetAllComponentsFromList(List<Transform> a, System.Type type)
    {
        var comps = new List<Component>();

        for (int i = 0; i < a.Count; i++)
        {
            var comp = a[i].GetComponent(type);

            if (comp != null)
            {
                comps.Add(comp);
            }
        }

        return comps;
    }

    #endregion Functions

    #region Classes

    public struct QueryEvent
    {
        float triggeredFrame;

        bool triggered;

        public bool Query()
        {
            if (triggered)
            {
                triggered = false;

                return true;
            }

            return false;
        }

        public bool Query(int frameCount, bool canDisable = false)
        {
            if (frameCount == triggeredFrame || frameCount - 1 == triggeredFrame)
            {
                if (canDisable)
                {
                    return Query();
                }
                else
                {
                    triggered = false;

                    return true;
                }
            }

            return false;
        }

        public void Invoke()
        {
            triggered = true;

            triggeredFrame = -1;
        }

        public void Invoke(int frameCount)
        {
            triggered = true;

            triggeredFrame = frameCount;
        }
    }

    #endregion Classes

    #region Coroutines

    public static IEnumerator WaitUntilEvent(UnityEngine.Events.UnityEvent unityEvent)
    {
        var trigger = false;
        System.Action action = () => trigger = true;
        unityEvent.AddListener(action.Invoke);
        yield return new WaitUntil(() => trigger);
        unityEvent.RemoveListener(action.Invoke);
    }

    #endregion Coroutines
}