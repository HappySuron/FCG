using UnityEngine;

public class BlockerForViewCard : MonoBehaviour
{
    public void OnClick(){
    if (ViewManager.Instance != null)
        {
            ViewManager.Instance.CloseTooltip();
        }
        else    {
            Debug.LogWarning("TooltipManager не найден на сцене!");
        }
    }
}
