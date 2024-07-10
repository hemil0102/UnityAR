using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageColocalizationDemoManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text _statusText;
    [SerializeField] private Button _joinAsHostButton;
    [SerializeField] private Button _joinAsClientButton;
    [Header("SharedAR")]
    [SerializeField] private SharedSpaceManager _sharedSpaceManager;
    [SerializeField] private GameObject _sharedARRootMarker;
    [SerializeField] private Texture2D _targetImage;
    [SerializeField] private float _targetImageSize;

    protected void Start()
    {
        // Hide UI until VPS is in tracking state
        _joinAsHostButton.gameObject.SetActive(false);
        _joinAsClientButton.gameObject.SetActive(false);

        // UI event listeners
        _joinAsHostButton.onClick.AddListener(OnJoinAsHostClicked);
        _joinAsClientButton.onClick.AddListener(OnJoinAsClientClicked);

        // Netcode connection event callback
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        // Set SharedSpaceManager and start it
        _sharedSpaceManager.sharedSpaceManagerStateChanged += OnColocalizationTrackingStateChanged;
        // Set room to join
        var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
            _targetImage, _targetImageSize);
        var roomOptions = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
            "ImageTrackingDemoRoom",
            32, // Max capacity
            "image tracking colocalization demo"
        );

        _sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomOptions);
        _statusText.text = $"Shared Space Started";
    }

    private void OnColocalizationTrackingStateChanged(
        SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs args)
    {
        _statusText.text = $"Tracking State Changed: {args.Tracking}";
        // Show Join UI
        if (args.Tracking)
        {
            // create an origin marker object and set under the sharedAR origin
                Instantiate(_sharedARRootMarker,
                    _sharedSpaceManager.SharedArOriginObject.transform, false);

            _statusText.text = $"Localized";
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
