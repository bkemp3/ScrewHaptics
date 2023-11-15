using Android.BLE;
using Android.BLE.Commands;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DeviceButton : MonoBehaviour
{
    private string _deviceUuid = string.Empty;
    private string _deviceName = string.Empty;

    [SerializeField]
    private Text _deviceUuidText;
    [SerializeField]
    private Text _deviceNameText;

    [SerializeField]
    private Image _deviceButtonImage;
    [SerializeField]
    private Text _deviceButtonText;

    [SerializeField]
    private Color _onConnectedColor;
    private Color _previousColor;

    private bool _isConnected = false;

    private ConnectToDevice _connectCommand;
    private SubscribeToCharacteristic _readFromCharacteristic;

    string quaternion = "No data yet";

    private void Update()
    {
        _deviceUuidText.text = quaternion;
    }

    public void Show(string uuid, string name)
    {
        _deviceButtonText.text = "Connect";

        _deviceUuid = uuid;
        _deviceName = name;

        _deviceUuidText.text = uuid;
        _deviceNameText.text = name;
    }

    public void Connect()
    {
        if (!_isConnected)
        {
            _connectCommand = new ConnectToDevice(_deviceUuid, OnConnected, OnDisconnected);
            BleManager.Instance.QueueCommand(_connectCommand);
        }
        else
        {
            _connectCommand.Disconnect();
        }
    }

    public void SubscribeToExampleService()
    {
        //Replace these Characteristics with YOUR device's characteristics
        string service_id = "4fafc2011fb5459e8fccc5c9c331914b";
        string characteristic_id = "beb5483e36e14688b7f5ea07361b26a8";
        _readFromCharacteristic = new SubscribeToCharacteristic(_deviceUuid, service_id, characteristic_id, (byte[] value) =>
        {
            quaternion = Encoding.UTF8.GetString(value);
            Debug.LogWarning(Encoding.UTF8.GetString(value));
            //_deviceUuidText.text = Encoding.UTF8.GetString(value);

        }, true); 
        BleManager.Instance.QueueCommand(_readFromCharacteristic); 
        //_deviceUuidText.text = service_id;
    }

    private void OnConnected(string deviceUuid)
    {
        _deviceUuidText.text = "Connected";
        _previousColor = _deviceButtonImage.color;
        _deviceButtonImage.color = _onConnectedColor;

        _isConnected = true;
        _deviceButtonText.text = "Disconnect";

        SubscribeToExampleService();
    }

    private void OnDisconnected(string deviceUuid)
    {
        _deviceUuidText.text = "Disconnected";
        _deviceButtonImage.color = _previousColor;

        _isConnected = false;
        _deviceButtonText.text = "Connect";
    }
}
