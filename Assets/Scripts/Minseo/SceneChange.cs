using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public Image fadeImage;
    private bool _changing = false;

    private void Start()
    {
        fadeImage.DOFade(1, 0);
        fadeImage.DOFade(0,0.5f).OnComplete(()=>fadeImage.gameObject.SetActive(false));
    }
    public void ChangeScene(string sceneName)
    {
        if (_changing) return;
        
        SoundManager.Instance.PlaySFX(ESfx.SFX_START_BUTTON);
        _changing = true;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1f, 1f)
            .OnComplete(()=>SceneManager.LoadScene(sceneName));
    }
}
