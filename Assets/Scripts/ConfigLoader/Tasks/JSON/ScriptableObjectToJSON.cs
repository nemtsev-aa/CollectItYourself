using System.IO;
using UnityEngine;

public class ScriptableObjectToJSON : MonoBehaviour {
    public ScriptableObject scriptableObject; // �������� �� ��� ScriptableObject

    private void Start() {
        SaveScriptableObjectToJson();
    }

    public void SaveScriptableObjectToJson() {
        string jsonFilePath = Path.Combine(Application.dataPath, "scriptableObject.json"); // ���� � ����� JSON

        string json = JsonUtility.ToJson(scriptableObject, true); // �������������� ScriptableObject � JSON � ���������������

        File.WriteAllText(jsonFilePath, json); // ������ JSON � ����

        Debug.Log("ScriptableObject �������� ��� JSON: " + jsonFilePath);
    }
}
