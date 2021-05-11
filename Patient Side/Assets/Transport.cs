using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;

public class Transport : MonoBehaviour
{
    int myChannelId;
    int socketID;
    int socketPort = 8888;
    int connectionId;

    //GUI elements
    public Slider amount;
    public Slider size;
    public Button submit;

    // Start is called before the first frame update
    void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        myChannelId = config.AddChannel(QosType.Reliable);
        int maxConnections = 1;
        HostTopology topology = new HostTopology(config, maxConnections);

        socketID = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Open. SocketId: " + socketID);
        //Connect();
    }

    // Update is called once per frame
    void Update()
    {
        int recHostId;
        int recConnectionId;
        int recChannelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recNetworkEvent)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("incoming connection event received");
                break;
            case NetworkEventType.DataEvent:
                Stream stream = new MemoryStream(recBuffer);
                BinaryFormatter formatter = new BinaryFormatter();
                string message = formatter.Deserialize(stream) as string;
                SpiderValues sv = JsonUtility.FromJson<SpiderValues>(message);
                
                //update UI to values and submit
                amount.value = sv.quantity;
                size.value = sv.scale;
                submit.onClick.Invoke();
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("remote client event disconnected");
                break;
        }
    }

    public void Connect()
    {
        byte error;
        connectionId = NetworkTransport.Connect(socketID, "10.50.50.66", socketPort, 0, out error);
        Debug.Log("Connected to server. ConnectionId: " + connectionId);
    }

    [Serializable]
    class SpiderValues
    {
        public int quantity;
        public float scale;

        public SpiderValues(int quantity, float scale)
        {
            this.quantity = quantity;
            this.scale = scale;

        }                
    }

    
}
