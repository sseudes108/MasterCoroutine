using UnityEngine;

public enum AnimType{
    Position,
    Rotation,
    Scale,
    RectPosition
}

public class ControllerTransform : Controller{
    [SerializeField] private AnimType _animType;
    [SerializeField] private Transform _myTransform;

    [SerializeField] private Vector3 _inAxis = Vector3.one;

    private Vector3 ResultInAxis(Vector3 originalVector, float floatResult, Vector3 inAxis){
        float x = originalVector.x;
        float y = originalVector.y;
        float z = originalVector.z;

        if(inAxis.x != 0) x = floatResult * inAxis.x;
        if(inAxis.y != 0) y = floatResult * inAxis.y;
        if(inAxis.z != 0) z = floatResult * inAxis.z;

        return new Vector3(x, y, z);
    }

    private Vector3 GetValuesInAxis(float myFloat){
        return ResultInAxis(GetTransform(), myFloat, _inAxis);
    }

    private Vector3 GetTransform(){
        switch (_animType){
            case AnimType.Position: return _myTransform.position;
            case AnimType.Rotation: return _myTransform.rotation.eulerAngles;
            case AnimType.Scale: return _myTransform.localScale;
            case AnimType.RectPosition: return ((RectTransform)_myTransform).anchoredPosition3D;
            default: return Vector3.zero;
        }
    }


    private void SetTransform(Vector3 resultVector){
        switch (_animType){
            case AnimType.Position: _myTransform.position = resultVector; break;
            case AnimType.Rotation:_myTransform.rotation = Quaternion.Euler(resultVector); break;
            case AnimType.Scale:_myTransform.localScale = resultVector; break;
            case AnimType.RectPosition:((RectTransform)_myTransform).anchoredPosition3D = resultVector; break;
        }
    }

    public override void SetValuesFromCurve(float curveResult){
        Vector3 resultVector = GetValuesInAxis(curveResult);
        SetTransform(resultVector);
    }
}
