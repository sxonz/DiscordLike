using Photon.Pun;
using UnityEngine;

public class PlayerAnima : MonoBehaviourPun
{
    public Sprite mySprite;
    public Sprite myHandSprite;

    public Sprite otherSprite;
    public Sprite otherHandSprite;

    SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bool isMine = photonView.IsMine;
        if (isMine)
        {
            sr.sprite = mySprite;
        }
        else
        {
            sr.sprite = otherSprite;
        }

        SpriteRenderer[] childRenderers =
        GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer childSr in childRenderers)
        {
            if (childSr.CompareTag("Hand"))
            {
                if (isMine)
                {
                    childSr.sprite = myHandSprite;
                }
                else
                {
                    childSr.sprite = otherHandSprite;
                }
            }
        }
    }
}
