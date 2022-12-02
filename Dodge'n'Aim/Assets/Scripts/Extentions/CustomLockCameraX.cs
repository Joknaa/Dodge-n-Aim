using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Y co-ordinate
/// </summary>
[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CustomLockCameraX : CinemachineExtension {
    [Tooltip("Lock the camera's Y position to this value")]
    public float m_XPosition = 0;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if (stage == CinemachineCore.Stage.Finalize) {
            var pos = state.RawPosition;
            pos.x = m_XPosition;
            state.RawPosition = pos;
        }
    }
}