using UnityEngine;
using Cinemachine;
using OknaaEXTENSIONS;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's X and Y coordinates
/// </summary>
[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CustomLockCameraXY : CinemachineExtension {
    [Tooltip("Lock the camera's X position to this value")]
    [SerializeField] private float xPosition = 0;
    [Tooltip("Lock the camera's Y position to this value")]
    [SerializeField] private float yPosition = 0;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if (stage != CinemachineCore.Stage.Finalize) return;
        
        state.RawPosition.SetXY(xPosition, yPosition);
    }
}