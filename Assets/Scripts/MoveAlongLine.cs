using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongLine : MonoBehaviour {
    public LineRenderer Line;
    public float Speed = 1f;
    public bool MovingForward = true;

    private int currentIndex = 0;


    private void Update() {
        if (MovingForward) {
            transform.position = Vector3.MoveTowards(transform.position, Line.GetPosition(currentIndex), Time.deltaTime * Speed);
            if (transform.position == Line.GetPosition(currentIndex)) {
                if (currentIndex < Line.positionCount - 1) {
                    currentIndex++;
                }
                else {
                    transform.position = Line.GetPosition(0);
                }
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, Line.GetPosition(currentIndex), Time.deltaTime * Speed);
            if (transform.position == Line.GetPosition(currentIndex)) {
                if (currentIndex > 0) {
                    currentIndex--;
                }
                else {
                    MovingForward = true;
                    currentIndex++;
                }
            }
        }
    }
    
}
