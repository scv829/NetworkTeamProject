using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HJS_FadeController : MonoBehaviour
{
    [Header("FadeCanvas")]
    [SerializeField] Image fadeImage;
    [SerializeField] public bool isFadeOver { get; set; }       // 페이드 끝났는지 확인

    /// <summary>
    /// 화면을 밝게 만들어 준다
    /// </summary>
    public void FadeIn() => StartCoroutine(Fade(1, 0, 1));
    /// <summary>
    /// 화면을 어둡게 만들어 준다
    /// </summary>
    public void FadeOut() => StartCoroutine(Fade(0, 1, 1));

    /// <summary>
    /// 화면의 밝기를 조절해주는 코루틴
    /// 투명도를 조절해서 화면의 밝기를 조절
    /// </summary>
    /// <param name="start">시작 투명도</param>
    /// <param name="end">종료 투명도</param>
    /// <param name="time">변환하는데 걸리는 시간</param>
    /// <returns></returns>
    private IEnumerator Fade(float start, float end, float time)
    {
        yield return new WaitForSeconds(1f);

        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        isFadeOver = true;
    }
}
