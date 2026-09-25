using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;
using UnityEditor.Build.Content;

public static class Utils
{
    //TODO: THIS IS HAKY AS FUCK
    public static Camera worldCamera { get; set; }
    public static Mouse Mouse { get; } = Mouse.current;
    public static Keyboard Keyboard { get; } = Keyboard.current;

    public static TextMesh CrateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CrateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CrateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.color = color;
        textMesh.fontSize = fontSize;
        textMesh.text = text;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static GameObject CreateSpriteObject(string name, string spritResourcePath, Vector3 localPosition, float scale, Transform parent = null, int sortingOrder = 5000)
    {
        GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localScale = Vector3.one * scale / 2; //I have know reason why this needs to be halfed...
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();  
        Sprite sprite = Resources.Load<Sprite>(spritResourcePath);
        sr.sprite = sprite;
        sr.sortingOrder = sortingOrder;
        return gameObject;
    }

    public static void CreateFile(string content, string path, string fileName, string fileType = ".txt")
    {
        if (!path.EndsWith("/")) path = $"{path}/";
        if (!fileType.StartsWith(".")) fileType = $".{fileType}";

        File.WriteAllText($"{path}{fileName}{fileType}", content);
    }

    public static void DeleteFile(string path, string fileName, string fileType = ".txt")
    {
        if (!path.EndsWith("/")) path = $"{path}/";
        if (!fileType.StartsWith(".")) fileType = $".{fileType}";

        if (File.Exists($"{path}{fileName}{fileType}"))
            File.Delete($"{path}{fileName}{fileType}");
        else Debug.LogError($"File {path}{fileName}{fileType} does not exists");
    }

    public static T ReadJsonFile<T>(string path, string fileName)
    {
        if (!path.EndsWith("/")) path = $"{path}/";


        if (File.Exists($"{path}{fileName}.json"))
            return JsonConvert.DeserializeObject<T>(File.ReadAllText($"{path}{fileName}.json"));
        else
        {
            Debug.LogError($"File {path}{fileName}.json does not exists");
            return default(T);
        }
    }

    public static string SerializeObjToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static Vector3 GetMouseWorldPostion()
    {
        return worldCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}