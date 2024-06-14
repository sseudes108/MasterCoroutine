using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {
    
    [SerializeField] private List<Star> AllStarsCopy;

    private void Start() {
        AllStarsCopy = Star.AllStars.ToList();
        StartCoroutine(StarStateMachine());
    }

    private IEnumerator SortStartsByDistance(List<Star> starsCopyList){
        float distanceA;
        float distanceB;

        for (int i = 1; i <= starsCopyList.Count; i++){
            for(int j = 0; j < starsCopyList.Count - i; j++){
                distanceA = (starsCopyList[j].transform.position - transform.position).sqrMagnitude;
                distanceB = (starsCopyList[j + 1].transform.position - transform.position).sqrMagnitude;

                if(distanceA > distanceB){
                    Star tempStar = starsCopyList[j]; 
                    starsCopyList[j] = starsCopyList[j + 1]; 
                    starsCopyList[j + 1] = tempStar; 
                }
            }
        }
        yield return null;
    }

    private IEnumerator GoToNearestPosition(Vector3 position, float speed){
        while((position - transform.position).sqrMagnitude > 0.1f){
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, position, step);
            yield return null;
        }
        yield return null;
    }

    private Star FindNearestStar(){
        Star nearestStar = null;
        if(AllStarsCopy[0].occupied){
            AllStarsCopy.RemoveAt(0);
            if(AllStarsCopy.Count != 0){
                nearestStar = FindNearestStar();
            }
        }else{
            AllStarsCopy[0].occupied = true;
            nearestStar = AllStarsCopy[0];
        }
        return nearestStar;
    }

    private IEnumerator EatGameObject(Transform star, float speed){
        while (star.localScale.sqrMagnitude > 0.1f){
            Vector3 oldScale = star.localScale;
            float step = speed * Time.deltaTime;
            star.localScale = Vector3.Lerp(oldScale, Vector3.zero, step);
            yield return null;
        }

        star.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator Explode(){
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        particle.Play();

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator StarStateMachine(){
        while(AllStarsCopy.Count != 0 ){
            yield return SortStartsByDistance(AllStarsCopy);
            Star nearestStar = FindNearestStar();

            if(nearestStar != null){
                yield return GoToNearestPosition(nearestStar.transform.position, 4);
                yield return EatGameObject(nearestStar.transform, 2);
            }
        }
        yield return GoToNearestPosition(Random.insideUnitCircle, 3);

        StartCoroutine(EatGameObject(transform, 5));
        yield return Explode();
        yield return null;
    }
}