using UnityEngine;

public abstract class Controller : MonoBehaviour {

    [SerializeField] private Driver _drivenBy;

    [SerializeField] private float _from = 0f;
    [SerializeField] private float _to = 1f;
    [SerializeField] private AnimationCurve _animCurve = new(new Keyframe(0,0), new Keyframe (1,1));

    public abstract void SetValuesFromCurve(float curveResult);

    public void Awake(){
        AutoFillDriver();
    }

    private void AutoFillDriver(){
        _drivenBy = gameObject.GetComponent<Driver>();
        if(_drivenBy != null) _drivenBy.AutoFillControllers();
    }

    private float ValueFromCurve(float myFloat){
        float curveResult = _animCurve.Evaluate(myFloat);
        return Mathf.LerpUnclamped(_from, _to, curveResult);
    }

    public void SetValues(float result){
        float remap = ValueFromCurve(result);
        SetValuesFromCurve(remap);
    }
}