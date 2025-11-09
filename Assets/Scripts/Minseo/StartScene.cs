using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Image startSceneImage;
    public GameObject[] buttons;

    

    public void StartGame()
    {
        SoundManager.Instance.PlaySFX(ESfx.SFX_START_BUTTON);
        
        DeckManager.Instance.runCount++;
        foreach (var button in buttons)
        {
            button.SetActive(false);
        }
        startSceneImage.DOFade(0f, 0.5f)
            .OnComplete(() => startSceneImage.gameObject.SetActive(false));
        
    }
}
