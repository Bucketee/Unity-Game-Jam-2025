using System;
using UnityEngine;
using UnityEngine.UI;

public class LikabilityHeart : MonoBehaviour
{
    public int index;

    public Image image;
    public Sprite[] sprites;
    
    public void SetInfo(int idx)
    {
        index = idx;
        
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (HeroStat.Instance.Likeability >= index) image.sprite = sprites[0];
        else image.sprite = sprites[1];
    }
}
