using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        var agent = gameplayManager.ControlledAgent;

        if (Input.GetKeyUp(KeyCode.F1)) {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyUp(KeyCode.A)) {
            gameplayManager.SaveData.level += 1;
            gameplayManager.Save();
        }

        if (Input.GetKeyUp(KeyCode.S)) {
            gameplayManager.SaveData.health -= 15;
            gameplayManager.Save();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            Debug.Log($"health: {gameplayManager.SaveData.health}, level: {gameplayManager.SaveData.level}");
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetMouseButton(0)) {
                agent.Combat.Attack(groundHitPoint);
            }
        }
        else if (Input.GetMouseButton(0)) {
            if (groundHitFound) {
                agent.Movement.SetDestination(groundHitPoint.With(y: 0));
            }
        }
    }
}
