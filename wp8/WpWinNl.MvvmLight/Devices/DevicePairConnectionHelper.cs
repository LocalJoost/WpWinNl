using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using GalaSoft.MvvmLight.Messaging;

#if NETFX_CORE || PORTABLE
using Windows.Devices.Bluetooth.Rfcomm;
#endif


namespace WpWinNl.Devices
{
  public class DevicePairConnectionHelper
  {
    private ConnectMethod connectMode;

    public DevicePairConnectionHelper(string crossPlatformServiceUid = null)
    {
      RfcommServiceUuid = crossPlatformServiceUid;
      Messenger.Default.Register<NavigationMessage>(this, ProcessNavigationMessage);
      PeerFinder.TriggeredConnectionStateChanged += PeerFinderTriggeredConnectionStateChanged;
      PeerFinder.ConnectionRequested += PeerFinderConnectionRequested;
    }

    public bool SupportsTap
    {
      get
      {
        return (PeerFinder.SupportedDiscoveryTypes &
           PeerDiscoveryTypes.Triggered) == PeerDiscoveryTypes.Triggered;
      }
    }

    public bool SupportsBrowse
    {
      get
      {
        return (PeerFinder.SupportedDiscoveryTypes &
          PeerDiscoveryTypes.Browse) == PeerDiscoveryTypes.Browse;
      }
    }

    public async Task Start(ConnectMethod connectMethod = ConnectMethod.Tap, string displayAdvertiseName = null)
    {
      Reset();
      connectMode = connectMethod;
      if (!string.IsNullOrEmpty(displayAdvertiseName))
      {
        PeerFinder.DisplayName = displayAdvertiseName;
      }

      try
      {
        PeerFinder.Start();
      }
      catch (Exception)
      {
        Debug.WriteLine("Peerfinder error");
      }

      // Enable browse
      if (connectMode == ConnectMethod.Browse)
      {
        if (ConnectCrossPlatform)
        {
          await InitBrowseWpToWin();
        }

        await PeerFinder.FindAllPeersAsync().AsTask().ContinueWith(p =>
        {
          if (!p.IsFaulted)
          {
            FirePeersFound(p.Result);
          }
        });
      }
    }


    private void ProcessNavigationMessage(NavigationMessage message)
    {
      Debug.WriteLine("PhonePairConnectionHelper.ProcessNavigationMessage " + message.NavigationEvent.Uri);

      if (message.IsStartedByNfcRequest)
      {
        Start();
      }
    }

    public void Connect(PeerInformation peerInformation)
    {
      DoConnect(peerInformation);
    }

    private void PeerFinderConnectionRequested(object sender, ConnectionRequestedEventArgs args)
    {
      if (connectMode == ConnectMethod.Browse)
      {
        DoConnect(args.PeerInformation);
      }
    }

    private void DoConnect(HostName hostname, string serviceName)
    {
      var streamSocket = new StreamSocket();
      streamSocket.ConnectAsync(hostname, serviceName).AsTask().ContinueWith((p) => DoConnect(streamSocket));
    }

    private void DoConnect(PeerInformation peerInformation)
    {
#if WINDOWS_PHONE
      if (peerInformation.HostName != null)
      {
        DoConnect(peerInformation.HostName, peerInformation.ServiceName);
        return;
      }
#endif

      PeerFinder.ConnectAsync(peerInformation).AsTask().ContinueWith(p =>
      {
        if (!p.IsFaulted)
        {
          DoConnect(p.Result);
        }
        else
        {
          Debug.WriteLine("connection fault");
          FireConnectionStatusChanged(TriggeredConnectState.Failed);
        }
      });
    }

    private void DoConnect(StreamSocket receivedSocket)
    {
      socket = receivedSocket;
      StartListeningForMessages();
      PeerFinder.Stop();
      FireConnectionStatusChanged(TriggeredConnectState.Completed);
    }

    private async void StartListeningForMessages()
    {
      if (socket != null)
      {
        if (!listening)
        {
          listening = true;
          while (listening)
          {
            var message = await GetMessage();
            if (listening)
            {
              if (message != null && MessageReceived != null)
              {
                MessageReceived(this, new ReceivedMessageEventArgs { Message = message });
              }
            }
          }
        }
      }
    }

    private readonly object lockObject = new object();

    public void SendMessage(string message)
    {
      Debug.WriteLine("Send message:" + message);
      if (socket != null)
      {
        try
        {
          lock (lockObject)
          {
            {
              if (dataWriter == null)
              {
                dataWriter = new DataWriter(socket.OutputStream);
              }

              dataWriter.WriteInt32(message.Length);
              dataWriter.StoreAsync();

              dataWriter.WriteString(message);
              dataWriter.StoreAsync();
            }
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine("SendMessage: " + ex.Message);
        }
      }
    }

    public void Reset()
    {
      PeerFinder.Stop();
      StopInitBrowseWpToWin();

      if (dataReader != null)
      {
        try
        {
          listening = false;
          if (dataReader != null)
          {
            dataReader.Dispose();
            dataReader = null;
          }
          if (dataWriter != null)
          {
            dataWriter.Dispose();
            dataWriter = null;
          }
          if (socket != null)
          {
            socket.Dispose();
            socket = null;
          }
        }
        catch (Exception)
        {
        }
      }
    }

    private void PeerFinderTriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs args)
    {
      if (connectMode == ConnectMethod.Tap)
      {
        switch (args.State)
        {
          case TriggeredConnectState.Completed: // Connection completed, get the socket
            DoConnect(args.Socket);
            break;
          default:
            FireConnectionStatusChanged(args.State);
            break;
        }
      }
    }

    private void FireConnectionStatusChanged(TriggeredConnectState args)
    {
      Debug.WriteLine("PhonePairConnectionHelper: " + args);
      if (ConnectionStatusChanged != null)
      {
        ConnectionStatusChanged(this, args);
      }
    }

    private void FirePeersFound(IEnumerable<PeerInformation> args)
    {
      if (PeersFound != null)
      {
        PeersFound(this, args);
      }
    }

    private async Task<string> GetMessage()
    {
      try
      {
        if (dataReader == null) dataReader = new DataReader(socket.InputStream);
        await dataReader.LoadAsync(4);
        var messageLen = (uint)dataReader.ReadInt32();

        await dataReader.LoadAsync(messageLen);
        var message = dataReader.ReadString(messageLen);
        Debug.WriteLine("Message received: " + message);

        return message;
      }
      catch (Exception ex)
      {
        Debug.WriteLine("GetMessage: " + ex.Message);
      }
      return null;
    }

    public event TypedEventHandler<object, TriggeredConnectState> ConnectionStatusChanged;

    public event TypedEventHandler<object, ReceivedMessageEventArgs> MessageReceived;

    public event TypedEventHandler<object, IEnumerable<PeerInformation>> PeersFound;

    private StreamSocket socket;

    private DataReader dataReader;

    private DataWriter dataWriter;

    private bool listening;

    #region Cross platform stuff

    public bool ConnectCrossPlatform { get; set; }

    private Guid rfcommServiceUuid;

    public string RfcommServiceUuid
    {
      get
      {
        return rfcommServiceUuid == Guid.Empty ? null : rfcommServiceUuid.ToString();
      }
      set
      {
        Guid tmpGuid;
        if (Guid.TryParse(value, out tmpGuid))
        {
          rfcommServiceUuid = tmpGuid;
        }
      }
    }

#if WINDOWS_PHONE
    private async Task InitBrowseWpToWin()
    {
      var t = new Task(() =>
      {
        PeerFinder.AlternateIdentities["Bluetooth:SDP"] = rfcommServiceUuid.ToString();
      });
      t.Start();
      await t;
    }

    private void StopInitBrowseWpToWin()
    {
      if (PeerFinder.AlternateIdentities.ContainsKey("Bluetooth:SDP"))
      {
        PeerFinder.AlternateIdentities.Remove("Bluetooth:SDP");
      }
    }
#endif

#if NETFX_CORE || PORTABLE
    // Code in this part largely based on 
    // http://www.silverlightshow.net/items/Windows-8.1-Play-with-Bluetooth-Rfcomm.aspx

    private const uint ServiceVersionAttributeId = 0x0300;
    private const byte ServiceVersionAttributeType = 0x0A;
    private const uint ServiceVersion = 200;

    private RfcommServiceProvider provider;

    private async Task InitBrowseWpToWin()
    {
      provider = await RfcommServiceProvider.CreateAsync(
                       RfcommServiceId.FromUuid(rfcommServiceUuid));

      var listener = new StreamSocketListener();
      listener.ConnectionReceived += HandleConnectionReceived;

      await listener.BindServiceNameAsync(
        provider.ServiceId.AsString(),
        SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

      using (var writer = new DataWriter())
      {
        writer.WriteByte(ServiceVersionAttributeType);
        writer.WriteUInt32(ServiceVersion);

        var data = writer.DetachBuffer();
        provider.SdpRawAttributes.Add(ServiceVersionAttributeId, data);
        provider.StartAdvertising(listener);
      }
    }

    private void HandleConnectionReceived(StreamSocketListener listener,
      StreamSocketListenerConnectionReceivedEventArgs args)
    {
      provider.StopAdvertising();
      listener.Dispose();
      DoConnect(args.Socket);
    }

    private void StopInitBrowseWpToWin()
    {
      if (provider != null)
      {
        try
        {
          provider.StopAdvertising();
          provider = null;
        }
        catch (Exception)
        {
          // Yes, this is bad, but for some reason StopAdvertising crashes the app sometimes.
        }
      }
    }
#endif
    #endregion
  }
}
