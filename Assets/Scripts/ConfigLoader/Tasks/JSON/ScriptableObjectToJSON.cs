using System.IO;
using UnityEngine;

public class ScriptableObjectToJSON : MonoBehaviour {
    public ScriptableObject scriptableObject; // Замените на ваш ScriptableObject

    private void Start() {
        SaveScriptableObjectToJson();
    }

    public void SaveScriptableObjectToJson() {
        string jsonFilePath = Path.Combine(Application.dataPath, "scriptableObject.json"); // Путь к файлу JSON

        string json = JsonUtility.ToJson(scriptableObject, true); // Преобразование ScriptableObject в JSON с форматированием

        File.WriteAllText(jsonFilePath, json); // Запись JSON в файл

        Debug.Log("ScriptableObject сохранен как JSON: " + jsonFilePath);
    }
}
