using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TextFade : MonoBehaviour
{
    [SerializeField] private Text text;

    void Start()
    {
        
    }

    void Update()
    {
        ActiveAlert(true);
    }

    

    public void ActiveAlert(bool flag)
    {
        if (flag)
        {
            DOTween.Sequence()
                .Append(text.DOFade(.5f, 1))
                .Append(text.DOFade(0, 1))
                .SetLoops(-1, LoopType.Restart)
                .SetId("warningAlert");
        }
        else
        {
            DOTween.Kill("warningAlert");
            text.DOFade(0, 1);
        }
    }
}
