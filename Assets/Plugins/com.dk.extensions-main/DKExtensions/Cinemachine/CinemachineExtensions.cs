using Cinemachine;
using UnityEngine;

public static partial class CinemachineExtensions
{
    ///<summary>
    ///Return position offset of camera
    ///</summary>
    public static Vector3 PositionOffset(this CinemachineVirtualCamera cam)
    {
        var camTransposer = cam.GetCinemachineComponent<CinemachineTransposer>();
        return camTransposer.m_FollowOffset;
    }

    ///<summary>
    ///Returns look offset of camera
    ///</summary>
    public static Vector3 TrackingOffset(this CinemachineVirtualCamera cam)
    {
        var camComposer = cam.GetCinemachineComponent<CinemachineComposer>();
        return camComposer.m_TrackedObjectOffset;
    }

    public static void SetTarget(this CinemachineVirtualCameraBase cam, Transform followTarget, Transform lookTarget = null)
    {
        cam.Follow = followTarget;
        cam.LookAt = lookTarget == null ? followTarget : lookTarget;
    }

    public static CinemachineTransposer GetTransposer(this CinemachineVirtualCamera cam)
    {
        return cam.GetCinemachineComponent<CinemachineTransposer>();
    }

    public static CinemachineComposer GetComposer(this CinemachineVirtualCamera cam)
    {
        return cam.GetCinemachineComponent<CinemachineComposer>();
    }
}
