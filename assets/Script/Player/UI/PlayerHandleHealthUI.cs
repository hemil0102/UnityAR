using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHandleHealthUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text HealthText;

    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        _mainCamera = GameObject.FindObjectOfType<Camera>();
        AllPlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnPlayerHealthChangedServerRpc;
        InstanceOnOnPlayerHealthChangedServerRpc(GetComponentInParent<NetworkObject>().OwnerClientId);
    }

    public override void OnNetworkDespawn()
    {
        AllPlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnPlayerHealthChangedServerRpc;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InstanceOnOnPlayerHealthChangedServerRpc(ulong id)
    {
        if (GetComponentInParent<NetworkObject>().OwnerClientId == id)
        {
            SetHealthTextClientRpc(id);
        }
    }

    private void Update()
    {
        if (_mainCamera)
        {
            HealthText.transform.LookAt(_mainCamera.transform);
        }
    }

    [ClientRpc]
    private void SetHealthTextClientRpc(ulong id)
    {
        HealthText.text = AllPlayerDataManager.Instance.GetPlayerHealth(id).ToString();
    }
}
