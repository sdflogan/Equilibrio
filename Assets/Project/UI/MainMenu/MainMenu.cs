using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> panelLevel;
    [SerializeField] private List<CanvasGroup> endImage;
    [SerializeField] private CanvasGroup arrowR;
    [SerializeField] private CanvasGroup arrowL;
    
    private int countPage = 0;
    private bool clickStade = false;

    public void ClickButton(int count)
    {
        Debug.Log("click");
        Sequence clickButton = DOTween.Sequence();
        clickButton.AppendCallback(() => endImage[count].gameObject.SetActive(true))
            .Append(endImage[count].DOFade(1, 0.25f))
            .AppendCallback(() => SceneLoader.Instance.LoadSceneFade(count));

        clickButton.Play();
    }

    public void ClickRigth()
    {
        if (!clickStade)
        {
            clickStade = true;

            countPage++;

            Sequence clickRigth = DOTween.Sequence();
            clickRigth.AppendCallback(() => panelLevel[countPage].gameObject.SetActive(true))
                .Append(panelLevel[countPage].DOFade(1, 0.25f))
                .AppendCallback(() => clickStade = false);

            clickRigth.Play();

            if (countPage == panelLevel.Count - 1)
            {
                ArrowR();
            }

            else if (countPage == 1)
            {
                ArrowL();
            }
        }
    }

    public void ClickLeft()
    {
        if (!clickStade)
        {
            clickStade = true;

            countPage--;

            Sequence clickLeft = DOTween.Sequence();
            clickLeft.Append(panelLevel[countPage + 1].DOFade(0, 0.25f))
                .AppendCallback(() => panelLevel[countPage + 1].gameObject.SetActive(false))
                .AppendCallback(() => clickStade = false);

            clickLeft.Play();

            if (countPage == panelLevel.Count - 2)
            {
                ArrowR();
            }

            else if (countPage == 0)
            {
                ArrowL();
            }
        }
    }

    private void ArrowR()
    {
        if (arrowR.gameObject.activeInHierarchy)
        {
            Sequence hideArrowR = DOTween.Sequence();
            hideArrowR.Append(arrowR.DOFade(0, 0.25f))
                .AppendCallback(() => arrowR.gameObject.SetActive(false));

            hideArrowR.Play();
        }

        else
        {
            Sequence showArrowR = DOTween.Sequence();
            showArrowR.AppendCallback(() => arrowR.gameObject.SetActive(true))
                .Append(arrowR.DOFade(1, 0.25f));
            
            showArrowR.Play();
        }
    }

    private void ArrowL()
    {
        if (arrowL.gameObject.activeInHierarchy)
        {
            Sequence hideArrowL = DOTween.Sequence();
            hideArrowL.Append(arrowL.DOFade(0, 0.25f))
                .AppendCallback(() => arrowL.gameObject.SetActive(false));

            hideArrowL.Play();
        }

        else
        {
            Sequence showArrowL = DOTween.Sequence();
            showArrowL.AppendCallback(() => arrowL.gameObject.SetActive(true))
                .Append(arrowL.DOFade(1, 0.25f));

            showArrowL.Play();
        }
    }
}
