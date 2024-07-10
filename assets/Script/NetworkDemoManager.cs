using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDemoManager : MonoBehaviour
{
    [SerializeField] private Text _statusText;

    [SerializeField] private Button _joinAsHostButton;

    [SerializeField] private Button _joinAsClientButton;

    [SerializeField] private SharedSpaceManager _sharedSpaceManager;

    protected void Start()
    {
        // UI event listeners
        _joinAsHostButton.onClick.AddListener(OnJoinAsHostClicked);
        _joinAsClientButton.onClick.AddListener(OnJoinAsClientClicked);

        // Netcode connection event callback
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        // Set SharedSpaceManager and start it
        _sharedSpaceManager.sharedSpaceManagerStateChanged += OnColocalizationTrackingStateChanged;
        // Set room to join
        var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
        var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
            "ExampleRoom", // use fixed room name
            32, // set capacity to max
            "shared ar demo (mock mode)" // description
        );
        _sharedSpaceManager.StartSharedSpace(mockTrackingArgs, roomArgs);
    }

    private void OnColocalizationTrackingStateChanged(
        SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs args)
    {
        // Show Join UI
        if (args.Tracking)
        {
            _joinAsHostButton.gameObject.SetActive(true);
            _joinAsClientButton.gameObject.SetActive(true);
        }
    }
    private void OnJoinAsHostClicked()
    {
        NetworkManager.Singleton.StartHost();
        HideButtons();
    }

    private void OnJoinAsClientClicked()
    {
        NetworkManager.Singleton.StartClient();
        HideButtons();
    }

    private void HideButtons()
    {
        _joinAsHostButton.gameObject.SetActive(false);
        _joinAsClientButton.gameObject.SetActive(false);
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        _statusText.text = $"Connected: {clientId}";
    }
}
