using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAvatar : NetworkTransform
{
    [HideInInspector] private Transform _arCameraTransform;

    // 서버 권한 설정 비활성화로 클리언트 권한으로 설정
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    // 네트워크에서 오브젝트가 생성될 때 호출
    public override void OnNetworkSpawn()
    {
        // 오브젝트의 소유자인 경우
        if (IsOwner)
        {
            // 메인 카메라가 존재하면, AR 카메라의 Transform을 저장
            if (Camera.main)
            {
                _arCameraTransform = Camera.main.transform;
            }
        }

        base.OnNetworkSpawn();
    }

    new void Update()
    {
        if (IsOwner)
        {
            if (_arCameraTransform)
            {
                // Get local AR camera transform
                _arCameraTransform.GetPositionAndRotation(out var pos, out var rot);
                // Since using the ClientNetworkTransform, just update world transform of the cube matching with 
                // the AR Camera's worldTransform. it's local transform will be synced.
                transform.SetPositionAndRotation(pos, rot);
            }
        }

        base.Update();
    }
}
