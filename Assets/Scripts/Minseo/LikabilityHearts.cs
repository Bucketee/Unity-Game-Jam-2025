using UnityEngine;

public class LikabilityHearts : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform hearts;
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject heart = Instantiate(heartPrefab, hearts);
            heart.GetComponent<LikabilityHeart>().SetInfo(i + 1);
        }
    }

}
