using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LogoAnimation : MonoBehaviour
{

    [SerializeField] private Transform title = null;
    [SerializeField] private Transform ballon = null;
    [SerializeField] private Transform sparks = null;
    [SerializeField] private Transform playText = null;
    [SerializeField] private float animationTime = 0;

    // Start is called before the first frame update
    void Start()
    {   
        Vector3 bufferBallon = ballon.localScale;
        Vector3 bufferSparks = sparks.localScale;
        Vector3 bufferTitle = title.localScale;

        ballon.localScale = Vector3.zero;
        sparks.localScale = Vector3.zero;
        title.localScale = Vector3.zero; 



        sparks.DOScale(bufferSparks, animationTime).OnComplete(()=>{
            sparks.DOScale(sparks.localScale*9/10, 1.5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true); 
        }).SetUpdate(true);   
       
        ballon.DOScale(bufferBallon, animationTime).OnComplete(()=>{
            title.DOScale(bufferTitle, animationTime).SetUpdate(true);
        }).SetUpdate(true);

        playText.DOScale(playText.localScale*0.92f, 2).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
