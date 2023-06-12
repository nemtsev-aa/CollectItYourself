using UnityEngine;

public class ActiveWindow : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
