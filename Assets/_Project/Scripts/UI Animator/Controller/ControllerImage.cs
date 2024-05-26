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

    public override void GetInitialValue(ControllerEditorPreview controller){
        switch(_animType) {
            case AnimImageType.FillImage: EditorPreview.InitialFloat = _myImage.fillAmount; break;

            case AnimImageType.FadeImage: EditorPreview.InitialFloat = _myImage.color.a; break;
        }
        EditorPreview.initialString = EditorPreview.InitialFloat.ToString();
    }
    public override void SetInitialValue(ControllerEditorPreview controller){
        SetValuesFromCurve(EditorPreview.InitialFloat);
    }
}