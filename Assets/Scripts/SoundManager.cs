using UnityEngine;

public enum EBgm
{
    BGM_MAIN,
    BGM_END,
}
    
//SFX 종류들
public enum ESfx
{
    SFX_BUTTON,
    SFX_START_BUTTON,
    SFX_DIALOGUE_MOVE_BUTTON,
    SFX_PURCHASE,
    SFX_GIVE_CARD,
    SFX_ENDING_LIST_BUTTON,
    SFX_DRAW_CARD,
    SFX_APPLY
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    //audio clip 담을 수 있는 배열
    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] sfxs;

    //플레이하는 AudioSource
    [SerializeField] AudioSource audioBgm;
    [SerializeField] AudioSource audioSfx;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(this);
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    // EBgm 열거형을 매개변수로 받아 해당하는 배경 음악 클립을 재생
    public void PlayBGM(EBgm bgmIdx)
    {
        //enum int형으로 형변환 가능
        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.Play();
    }

    // 현재 재생 중인 배경 음악 정지
    public void StopBGM()
    {
        audioBgm.Stop();
    }

    // ESfx 열거형을 매개변수로 받아 해당하는 효과음 클립을 재생
    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }
}
