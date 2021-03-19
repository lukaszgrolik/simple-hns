using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private GameplayManager gameplayManager;

    private bool groundHitFound;
    private Vector3 groundHitPoint;

    public void Setup(GameplayManager gameplayManager) {
        this.gameplayManager = gameplayManager;
    }

    void Update() {
        var ray = gameplayManager.Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, 100, gameplayManager.GroundLayerMask)) {
            groundHitFound = true;
            groundHitPoint = hitInfo.point;
        }
        else {
            groundHitFound = false;
        }

        if (Input.GetMouseButton(0)) {
            if (groundHitFound) {
                gameplayManager.ControlledAgent.SetDestination(groundHitPoint.With(y: 0));
            }
        }
    }
}
