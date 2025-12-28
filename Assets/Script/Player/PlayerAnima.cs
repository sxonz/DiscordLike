using Photon.Pun;
using UnityEngine;
using DG.Tweening;

public class PlayerAnima : MonoBehaviourPun
{
    public Sprite mySprite;
    public Sprite myHandSprite;

    public Sprite otherSprite;
    public Sprite otherHandSprite;

    private SpriteRenderer sr;
    private Sprite normalSprite;

    private Tween hitTween;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        bool isMine = photonView.IsMine;

        normalSprite = isMine ? mySprite : otherSprite;
        sr.sprite = normalSprite;

        SpriteRenderer[] childRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer childSr in childRenderers)
        {
            if (childSr.CompareTag("Hand"))
            {
                childSr.sprite = isMine ? myHandSprite : otherHandSprite;
            }
        }
    }

    // 외부에서 호출하는 진입점
    public void PlayHitEffect(float duration)
    {
        photonView.RPC(nameof(RPC_HitEffect), RpcTarget.All, duration);
    }

    [PunRPC]
    void RPC_HitEffect(float duration)
    {
        hitTween?.Kill();

        sr.DOFade(0.2f, 0.1f)
          .SetLoops(6, LoopType.Yoyo);

        hitTween = DOVirtual.DelayedCall(duration, () =>
        {
            sr.color = Color.white;
        });
    }

    void OnDestroy()
    {
        hitTween?.Kill();
    }
}
