using UnityEngine;
using UnityEngine.UI;

public enum AnimImageType{
    FillImage,
    FadeImage
}

public class ControllerImage : Controller{
    
    [SerializeField] private AnimImageType _animType;
    [SerializeField] private Image _myImage;

    public override void SetValuesFromCurve(float curveResult){
        switch (_animType){
            case AnimImageType.FillImage: SetImageFill(curveResult); break;
            case AnimImageType.FadeImage: SetImageFade(curveResult); break;
        }
    }

    private void SetImageFill(float myFloat){
        _myImage.fillAmount = myFloat;
    }

    private void SetImageFade(float myFloat){
        Color newColor = _myImage.color;
        newColor.a = myFloat;
        _myImage.color = newColor;
    }
}