using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public Image startSceneImage;
    public Image fadeImage;
    public GameObject[] buttons;

    private void OnEnable()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(EBgm.BGM_MAIN);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySFX(ESfx.SFX_START_BUTTON);
        
        DeckManager.Instance.runCount++;
        DeckManager.Instance.InitDeck();
        Hand.Instance.InitHand();
        DialogueManager.Instance.OnStartAction?.Invoke();
        
        foreach (var button in buttons)
        {
            button.SetActive(false);
        }
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1f, 0.5f)
            .OnComplete(() =>
            {
                startSceneImage.gameObject.SetActive(false);
                fadeImage.DOFade(0f, 0.5f)
                    .OnComplete(() => { fadeImage.gameObject.SetActive(false); });
            });
        
    }
}
