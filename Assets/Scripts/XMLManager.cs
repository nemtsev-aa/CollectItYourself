using System;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class XMLManager : MonoBehaviour {

    public string AnswerName;

    [ContextMenu("Test1")]
    public Answer AnswerToArray() {
        int iWagoNumber = 1;
        //string AnswerName = "1_1";
        Answer answer = ScriptableObject.CreateInstance<Answer>();
        answer.name = AnswerName;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(Application.dataPath + "/Resources/Answers.xml");

        if (AnswerName.Substring(0, 1) != "0") {
            XmlNodeList Nodes = XmlDoc.SelectNodes("Answers/Answers_Part/Answers_Variant[@name='" + AnswerName + "']");

            if (Nodes == null) return null;

            foreach (XmlNode iNode in Nodes) {
                // Анализируем текущий узел
                string name = iNode.Attributes.Item(0).Value;
                int WagoCount = iNode.ChildNodes.Count;

                answer.name = name;
                Debug.Log(name);

                foreach (XmlNode iWago in iNode) {
                    // Анализируем дочерние элементы
                    AnswerData answerData = ScriptableObject.CreateInstance<AnswerData>();
                    answerData.name = "Wago_" + iWagoNumber;

                    string WagoName = iWago.Attributes.Item(0).Value; // Название зажима
                    int WagoNumber = WagoName[WagoName.Length - 1];
                    answer.WagoClipNumber = WagoCount;

                    string WagoValue = iWago.Attributes.Item(1).Value; // Значения
                    string[] Contacts = WagoValue.Split(','); // Массив контактов
                    
                    foreach (var iContact in Contacts) {
                        ConnectionData contactData = new ConnectionData();
                        contactData.CompanentName = iContact.Substring(0, 3);
                        Debug.Log("CompanentName " + contactData.CompanentName);

                        switch (iContact.Substring(0, 2)) {
                            case "IN":
                                contactData.CompanentType = CompanentType.Input;
                                contactData.CompanentName = "Input";
                                break;
                            case "OU":
                                contactData.CompanentType = CompanentType.Output;
                                contactData.CompanentName = "Output";
                                break;
                            case "SA":
                                contactData.CompanentType = CompanentType.Selector;
                                break;
                            case "XS":
                                contactData.CompanentType = CompanentType.PowerSocket;
                                break;
                            case "BK":
                                contactData.CompanentType = CompanentType.MotionSensor;
                                contactData.CompanentName = "BK";
                                break;
                            case "EL":
                                contactData.CompanentType = CompanentType.Lamp;
                                break;
                            default:
                                break;
                        }
                        Debug.Log("CompanentType " + contactData.CompanentType);
                        switch (iContact.Substring(iContact.Length-1, 1)) {
                            case "L":
                                contactData.ContactType = ContactType.Line;
                                break;
                            case "N":
                                contactData.ContactType = ContactType.Neutral;
                                break;
                            case "E":
                                contactData.ContactType = ContactType.GroundConductor;
                                break;
                            case "Z":
                                contactData.ContactType = ContactType.Closed;
                                break;
                            case "R":
                                contactData.ContactType = ContactType.Open;
                                break;
                            case "A":
                                contactData.ContactType = ContactType.LineOut;
                                break;
                            default:
                                break;
                        }
                        Debug.Log("ContactType " + contactData.ContactType);
                        answerData.Connections.Add(contactData);
                    }
#if UNITY_EDITOR
                    AssetDatabase.CreateAsset(answerData, CreateFolder("Assets/Resources/Answers/Parts/" + AnswerName + "/") + answerData.name + ".asset");
                    AssetDatabase.SaveAssets();
#endif
                    answer.AnswerDataList.Add(answerData);
                    iWagoNumber++;
                }
            }
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(answer, CreateFolder("Assets/Resources/Answers/Parts/" + AnswerName + "/") + AnswerName + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = answer;
#endif
            return answer;
        } else {
            return null;
        }
    }

    private string CreateFolder(string path) {
        if (!Directory.Exists(path)) { 
            Directory.CreateDirectory(path); // Создание новой папки
            Debug.Log("Folder created: " + path);
        } else {
            Debug.Log("Folder already exists: " + path);
        }
        return path;
    }
}
