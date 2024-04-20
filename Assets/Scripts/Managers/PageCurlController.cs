using UnityEngine;
using System.Collections;

public class PageCurlController : MonoBehaviour
{
    public Material pageCurlMaterial;

    private void Start()
    {
        pageCurlMaterial.SetFloat("_Flip", -1f);
    }

    public void FlipPage()
    {
        StartCoroutine(AnimateFlip());
    }

    private IEnumerator AnimateFlip()
    {
        pageCurlMaterial.SetFloat("_Flip", 1f);

        float elapsedTime = 0f;
        float duration = 1f; // アニメーションの所要時間(秒)

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float flip = Mathf.Lerp(1f, -1f, t);
            pageCurlMaterial.SetFloat("_Flip", flip);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pageCurlMaterial.SetFloat("_Flip", -1f);
    }
}