using UnityEngine;

public class OnClickInstantiate : MonoBehaviour{
    [SerializeField] private GameObject _prefab;

    public void InstantiatePlayer(){
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        Instantiate(_prefab, position, Quaternion.identity);
    }
}
