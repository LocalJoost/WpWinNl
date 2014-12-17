using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Networking.Proximity;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using WpWinNl.Devices;
using WpWinNl.Utilities;

namespace TtsBTUniversalDemo.Viewmodels
{
  public class NfcConnectViewModel : ViewModelBase
  {
    public NfcConnectViewModel()
    {
      Init();
    }

    private DevicePairConnectionHelper connectHelper;

    public ObservableCollection<string> ConnectMessages { get; private set; }
    public ObservableCollection<string> ReceivedMessages { get; private set; }

    private bool canInitiateConnect;

    public bool CanInitiateConnect
    {
      get
      {
        return canInitiateConnect;
      }
      set
      {
        if (canInitiateConnect != value)
        {
          canInitiateConnect = value;
          RaisePropertyChanged(() => CanInitiateConnect);
        }
      }
    }

    private bool isConnecting;

    public bool IsConnecting
    {
      get { return isConnecting; }
      set
      {
        if (isConnecting != value)
        {
          isConnecting = value;
          RaisePropertyChanged(() => IsConnecting);
        }
      }
    }

    private bool canSend;

    public bool CanSend
    {
      get { return canSend; }
      set
      {
        if (canSend != value)
        {
          canSend = value;
          RaisePropertyChanged(() => CanSend);
        }
      }
    }

    private string message;

    public string Message
    {
      get { return message; }
      set
      {
        if (message != value)
        {
          message = value;
          RaisePropertyChanged(() => Message);
        }
      }
    }

    public ICommand StartCommmand
    {
      get
      {
        return new RelayCommand(
          async () =>
          {
            ConnectMessages.Add("Connect started...");
            CanSend = false;
            CanInitiateConnect = false;
            // Changed for Bluetooth.
            if (UseBlueTooth)
            {
              await connectHelper.Start(ConnectMethod.Browse);
            }
            else
            {
             await connectHelper.Start();
            }
          });
      }
    }

    public ICommand ResetCommand
    {
      get
      {
        return new RelayCommand(
          Reset);
      }
    }

    private void Reset()
    {
      CanInitiateConnect = true;
      IsConnecting = true;
      CanSend = false;
    }

    public ICommand SendComand
    {
      get
      {
        return new RelayCommand(
          () =>
          {
            connectHelper.SendMessage(Message);
            Message = string.Empty;
          }
          );
      }
    }

    private void Init()
    {
      useBlueTooth = true;
      Peers = new ObservableCollection<PeerInformation>();

      Messenger.Default.Register<NavigationMessage>(this, ProcessNavigationMessage);

      if (ConnectMessages == null)
      {
        ConnectMessages = new ObservableCollection<string>();
      }

      if (ReceivedMessages == null)
      {
        ReceivedMessages = new ObservableCollection<string>();
      }

      if (!IsInDesignMode)
      {
        if (connectHelper == null)
        {

          // STORE APP
          connectHelper = new DevicePairConnectionHelper("A7EA96BB-4F95-4A91-9FDD-3CE3CFF1D8BD");
          this.ConnectCrossPlatform = true;
          connectHelper.ConnectionStatusChanged += ConnectionStatusChanged;
          connectHelper.MessageReceived += TtsHelperMessageReceived;
          connectHelper.PeersFound += PeersFound; // Added for Bluetooth support
        }
        else
        {
          CanSend = true;
        }
      }
      IsConnecting = true;
    }

    private void ProcessNavigationMessage(NavigationMessage message)
    {
      CanInitiateConnect = !message.IsStartedByNfcRequest;
    }

    private void TtsHelperMessageReceived(object sender,
      ReceivedMessageEventArgs e)
    {
      DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
      {
        ReceivedMessages.Add(e.Message);
      });
    }

    private void ConnectionStatusChanged(object sender,
      TriggeredConnectState e)
    {
      DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
      {
        ConnectMessages.Add(GetMessageForStatus(e));
        if (e == TriggeredConnectState.Completed)
        {
          IsConnecting = false;
          CanSend = true;
        }
        if (e == TriggeredConnectState.Failed)
        {
          Reset();
        }
      });
    }

    private static string GetMessageForStatus(TriggeredConnectState state)
    {
      switch (state)
      {
        case TriggeredConnectState.Listening:
          return "Listening....";

        case TriggeredConnectState.PeerFound:
          return "Opponent found";

        case TriggeredConnectState.Connecting:
          return "Opponent found";

        case TriggeredConnectState.Completed:
          return "Connection succesfull!";

        case TriggeredConnectState.Canceled:
          return "Connection canceled";

        default: //TriggeredConnectState.Failed:    
          return "Connection failed";
      }
    }

    #region Bluetooth stuff

    private bool useBlueTooth;

    public bool UseBlueTooth
    {
      get { return useBlueTooth; }
      set
      {
        if (useBlueTooth != value)
        {
          useBlueTooth = value;
          RaisePropertyChanged(() => UseBlueTooth);
        }
      }
    }


    private PeerInformation selectedPeer;

    public PeerInformation SelectedPeer
    {
      get { return selectedPeer; }
      set
      {
        if (selectedPeer != value)
        {
          selectedPeer = value;
          RaisePropertyChanged(() => SelectedPeer);
        }
      }
    }

    public ObservableCollection<PeerInformation> Peers { get; private set; }

    private void PeersFound(object sender, IEnumerable<PeerInformation> args)
    {
      DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
      {
        Peers.Clear();

        args.ForEach(Peers.Add);
        if (Peers.Count > 0)
        {
          SelectedPeer = Peers.First();
        }
        else
        {
          ConnectMessages.Add("No contacts found");
          Reset();
        }
      });
    }

    public ICommand ConnectBluetoothContactCommand
    {
      get
      {
        return new RelayCommand(() =>
        {
          var peer = SelectedPeer;
          connectHelper.Connect(peer);
          ConnectMessages.Add("Connecting to " + peer.DisplayName);
          Peers.Clear();
        });
      }
    }

    public bool ConnectCrossPlatform
    {
      get { return connectHelper.ConnectCrossPlatform; }
      set
      {
        if (connectHelper.ConnectCrossPlatform != value)
        {
          connectHelper.ConnectCrossPlatform = value;
          RaisePropertyChanged(() => ConnectCrossPlatform);
        }
      }
    }

    #endregion
  }
}

