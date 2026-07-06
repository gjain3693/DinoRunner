using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public void ShowRewardedAd()
    {
        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowRewardAd();
        }
        else
        {
            Debug.LogError(
                "ContinueButton: AdManager.Instance is null."
            );
        }
    }
}