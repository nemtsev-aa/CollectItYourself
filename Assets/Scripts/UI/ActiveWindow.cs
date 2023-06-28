using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWindow : MonoBehaviour
{
    public void Show()
    {     
        Debug.Log("ActiveWindow_Show");   
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
