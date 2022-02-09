using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class CameraFollow : MonoBehaviour {
        [HideInInspector] private float smoothSpeed = 10f;
        // [HideInInspector] private Vector3 offset = new Vector3(0, 25, -25);

        private GameplayManager gameplayManager;
        private Transform target;

        private Vector3 offset;

        public void Setup(GameplayManager gameplayManager, Transform target) {
            this.gameplayManager = gameplayManager;
            this.target = target;

            var camT = gameplayManager.Cam.transform;
            var height = camT.position.y;
            var side = GetIsometricOffset(camT.eulerAngles.x, height);

            offset = new Vector3(-side, height, -side);
        }

        float GetIsometricOffset(float angle, float height) {
            return height / Mathf.Tan(angle * Mathf.PI / 180) / Mathf.Sqrt(2);
        }

        void LateUpdate() {
            if (!target) return;

            var desiredPos = target.position + offset;
            var smoothedPos = Vector3.Lerp(gameplayManager.Cam.transform.position, desiredPos, smoothSpeed * Time.deltaTime);

            gameplayManager.Cam.transform.position = smoothedPos;
            // Camera.main.transform.position = desiredPos;
        }
    }
}
