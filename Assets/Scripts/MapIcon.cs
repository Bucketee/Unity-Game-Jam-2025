using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject infoBox;
    public List<string> infos;

    public void OnEnable()
    {
        if (HeroStat.Instance.Date == 5 && gameObject.name.Equals("Forest"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if (HeroStat.Instance.dungeunCount == 3 && gameObject.name.Equals("Dungeon"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnDisable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HeroStat.Instance.Date == 5 && gameObject.name.Equals("Forest"))
        {
            infos.Add(@"<size=30>Dark Lord</size>\nAt the heart of the forest lies the Dark Lord’s domain.  He has lingered there since ages past, forever threatening nearby villages.  They say he changes form with each era, wearing the faces of those who challenged him.  Countless heroes have fallen before him, yet he still remains and once again, another hero walks toward that shadow.");
            StartCoroutine(ShowMapIconInfo(infos));
            infos.RemoveAt(infos.Count - 1);
        }
        else if (HeroStat.Instance.dungeunCount == 3 && gameObject.name.Equals("Dungeon"))
        {
            infos.Add(@"<size=30>Dragon</size>\nSaid to have slept deep within the caves since ancient times.  Adventurers and gatherers enter offering prayers, but most never return.  Lately, more claim to have heard its distant roar.  Has it truly awakened or is it only the mountain’s breath?");
            StartCoroutine(ShowMapIconInfo(infos));
            infos.RemoveAt(infos.Count - 1);
        }
        else
        {
            StartCoroutine(ShowMapIconInfo(infos));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("AlertIcon")) continue;
            Destroy(child.gameObject);
        }
    }
    
    public IEnumerator ShowMapIconInfo(List<string> value)
    {
        List<string> newInfos = new List<string>();
        foreach (string info in value)
        {
            newInfos.Add(info);
        }
        
        Vector3 pos = GetComponent<RectTransform>().position + new Vector3(50, 250f, 0);
        foreach (string s in newInfos)
        {
            GameObject g = Instantiate(infoBox, transform);
            g.GetComponent<RectTransform>().position = pos;
            g.GetComponentInChildren<TextMeshProUGUI>().text = s;
            yield return null;
            pos -= new Vector3(0, g.GetComponent<RectTransform>().rect.height, 0);
        }
    }
}
