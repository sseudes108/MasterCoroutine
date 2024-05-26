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

    private IEnumerator StarStateMachine(){
        while(AllStarsCopy.Count != 0 ){
            yield return SortStartsByDistance(AllStarsCopy);
            Star nearestStar = FindNearestStar();

            yield return GoToNearestPosition(nearestStar.transform.position, 4);
            yield return null;
        }
        yield return null;
    }
}