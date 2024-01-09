using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    public interface ImonitorControl
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> signage;
        event EventHandler<UIEventArgs> tvPlayer;
        event EventHandler<UIEventArgs> laptopInput;
        event EventHandler<UIEventArgs> ledOn;
        event EventHandler<UIEventArgs> ledOff;
        event EventHandler<UIEventArgs> showChannelsPage;
        event EventHandler<UIEventArgs> channelsAvailable;
        event EventHandler<UIEventArgs> videoCon;
        event EventHandler<UIEventArgs> videoConPermitted;
        event EventHandler<UIEventArgs> selectedChannel;
        event EventHandler<UIEventArgs> channel1;
        event EventHandler<UIEventArgs> channel2;
        event EventHandler<UIEventArgs> channel3;
        event EventHandler<UIEventArgs> channel4;
        event EventHandler<UIEventArgs> channel5;
        event EventHandler<UIEventArgs> channel6;
        event EventHandler<UIEventArgs> channel7;
        event EventHandler<UIEventArgs> channel8;
        event EventHandler<UIEventArgs> channel9;
        event EventHandler<UIEventArgs> channel10;
        event EventHandler<UIEventArgs> channel11;
        event EventHandler<UIEventArgs> channel12;
        event EventHandler<UIEventArgs> channel13;
        event EventHandler<UIEventArgs> channel14;
        event EventHandler<UIEventArgs> channel15;
        event EventHandler<UIEventArgs> channel16;
        event EventHandler<UIEventArgs> channel17;
        event EventHandler<UIEventArgs> channel18;
        event EventHandler<UIEventArgs> channel19;
        event EventHandler<UIEventArgs> channel20;

        void signageFb(monitorControlBoolInputSigDelegate callback);
        void tvPlayerFb(monitorControlBoolInputSigDelegate callback);
        void laptopInputFb(monitorControlBoolInputSigDelegate callback);
        void ledOnFb(monitorControlBoolInputSigDelegate callback);
        void ledOffFb(monitorControlBoolInputSigDelegate callback);
        void showChannelsPageFb(monitorControlBoolInputSigDelegate callback);
        void channelsAvailableFb(monitorControlBoolInputSigDelegate callback);
        void videoConFb(monitorControlBoolInputSigDelegate callback);
        void videoConPermittedFb(monitorControlBoolInputSigDelegate callback);
        void selectedChannelFb(monitorControlStringInputSigDelegate callback);
        void channel1Fb(monitorControlStringInputSigDelegate callback);
        void channel2Fb(monitorControlStringInputSigDelegate callback);
        void channel3Fb(monitorControlStringInputSigDelegate callback);
        void channel4Fb(monitorControlStringInputSigDelegate callback);
        void channel5Fb(monitorControlStringInputSigDelegate callback);
        void channel6Fb(monitorControlStringInputSigDelegate callback);
        void channel7Fb(monitorControlStringInputSigDelegate callback);
        void channel8Fb(monitorControlStringInputSigDelegate callback);
        void channel9Fb(monitorControlStringInputSigDelegate callback);
        void channel10Fb(monitorControlStringInputSigDelegate callback);
        void channel11Fb(monitorControlStringInputSigDelegate callback);
        void channel12Fb(monitorControlStringInputSigDelegate callback);
        void channel13Fb(monitorControlStringInputSigDelegate callback);
        void channel14Fb(monitorControlStringInputSigDelegate callback);
        void channel15Fb(monitorControlStringInputSigDelegate callback);
        void channel16Fb(monitorControlStringInputSigDelegate callback);
        void channel17Fb(monitorControlStringInputSigDelegate callback);
        void channel18Fb(monitorControlStringInputSigDelegate callback);
        void channel19Fb(monitorControlStringInputSigDelegate callback);
        void channel20Fb(monitorControlStringInputSigDelegate callback);

    }

    public delegate void monitorControlBoolInputSigDelegate(BoolInputSig boolInputSig, ImonitorControl monitorControl);
    public delegate void monitorControlStringInputSigDelegate(StringInputSig stringInputSig, ImonitorControl monitorControl);

    internal class monitorControl : ImonitorControl, IDisposable
    {
        #region Standard CH5 Component members

        private ComponentMediator ComponentMediator { get; set; }

        public object UserObject { get; set; }

        public uint ControlJoinId { get; private set; }

        private IList<BasicTriListWithSmartObject> _devices;
        public IList<BasicTriListWithSmartObject> Devices { get { return _devices; } }

        #endregion

        #region Joins

        private static class Joins
        {
            internal static class Booleans
            {
                public const uint signage = 1;
                public const uint tvPlayer = 2;
                public const uint laptopInput = 3;
                public const uint ledOn = 4;
                public const uint ledOff = 5;
                public const uint showChannelsPage = 6;
                public const uint channelsAvailable = 7;
                public const uint videoCon = 8;
                public const uint videoConPermitted = 9;

                public const uint signageFb = 1;
                public const uint tvPlayerFb = 2;
                public const uint laptopInputFb = 3;
                public const uint ledOnFb = 4;
                public const uint ledOffFb = 5;
                public const uint showChannelsPageFb = 6;
                public const uint channelsAvailableFb = 7;
                public const uint videoConFb = 8;
                public const uint videoConPermittedFb = 9;
            }
            internal static class Strings
            {
                public const uint selectedChannel = 1;
                public const uint channel1 = 2;
                public const uint channel2 = 3;
                public const uint channel3 = 4;
                public const uint channel4 = 5;
                public const uint channel5 = 6;
                public const uint channel6 = 7;
                public const uint channel7 = 8;
                public const uint channel8 = 9;
                public const uint channel9 = 10;
                public const uint channel10 = 11;
                public const uint channel11 = 12;
                public const uint channel12 = 13;
                public const uint channel13 = 14;
                public const uint channel14 = 15;
                public const uint channel15 = 16;
                public const uint channel16 = 17;
                public const uint channel17 = 18;
                public const uint channel18 = 19;
                public const uint channel19 = 20;
                public const uint channel20 = 21;

                public const uint selectedChannelFb = 1;
                public const uint channel1Fb = 2;
                public const uint channel2Fb = 3;
                public const uint channel3Fb = 4;
                public const uint channel4Fb = 5;
                public const uint channel5Fb = 6;
                public const uint channel6Fb = 7;
                public const uint channel7Fb = 8;
                public const uint channel8Fb = 9;
                public const uint channel9Fb = 10;
                public const uint channel10Fb = 11;
                public const uint channel11Fb = 12;
                public const uint channel12Fb = 13;
                public const uint channel13Fb = 14;
                public const uint channel14Fb = 15;
                public const uint channel15Fb = 16;
                public const uint channel16Fb = 17;
                public const uint channel17Fb = 18;
                public const uint channel18Fb = 19;
                public const uint channel19Fb = 20;
                public const uint channel20Fb = 21;
            }
        }

        #endregion

        #region Construction and Initialization

        internal monitorControl(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.signage, onsignage);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.tvPlayer, ontvPlayer);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.laptopInput, onlaptopInput);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.ledOn, onledOn);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.ledOff, onledOff);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.showChannelsPage, onshowChannelsPage);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.channelsAvailable, onchannelsAvailable);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.videoCon, onvideoCon);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.videoConPermitted, onvideoConPermitted);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.selectedChannel, onselectedChannel);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel1, onchannel1);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel2, onchannel2);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel3, onchannel3);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel4, onchannel4);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel5, onchannel5);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel6, onchannel6);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel7, onchannel7);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel8, onchannel8);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel9, onchannel9);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel10, onchannel10);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel11, onchannel11);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel12, onchannel12);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel13, onchannel13);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel14, onchannel14);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel15, onchannel15);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel16, onchannel16);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel17, onchannel17);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel18, onchannel18);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel19, onchannel19);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel20, onchannel20);

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
        }

        #endregion

        #region CH5 Contract

        public event EventHandler<UIEventArgs> signage;
        private void onsignage(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = signage;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> tvPlayer;
        private void ontvPlayer(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = tvPlayer;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> laptopInput;
        private void onlaptopInput(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = laptopInput;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> ledOn;
        private void onledOn(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = ledOn;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> ledOff;
        private void onledOff(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = ledOff;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> showChannelsPage;
        private void onshowChannelsPage(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = showChannelsPage;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channelsAvailable;
        private void onchannelsAvailable(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channelsAvailable;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> videoCon;
        private void onvideoCon(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = videoCon;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> videoConPermitted;
        private void onvideoConPermitted(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = videoConPermitted;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void signageFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.signageFb], this);
            }
        }

        public void tvPlayerFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.tvPlayerFb], this);
            }
        }

        public void laptopInputFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.laptopInputFb], this);
            }
        }

        public void ledOnFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.ledOnFb], this);
            }
        }

        public void ledOffFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.ledOffFb], this);
            }
        }

        public void showChannelsPageFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.showChannelsPageFb], this);
            }
        }

        public void channelsAvailableFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.channelsAvailableFb], this);
            }
        }

        public void videoConFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.videoConFb], this);
            }
        }

        public void videoConPermittedFb(monitorControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.videoConPermittedFb], this);
            }
        }

        public event EventHandler<UIEventArgs> selectedChannel;
        private void onselectedChannel(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = selectedChannel;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel1;
        private void onchannel1(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel1;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel2;
        private void onchannel2(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel2;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel3;
        private void onchannel3(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel3;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel4;
        private void onchannel4(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel4;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel5;
        private void onchannel5(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel5;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel6;
        private void onchannel6(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel6;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel7;
        private void onchannel7(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel7;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel8;
        private void onchannel8(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel8;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel9;
        private void onchannel9(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel9;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel10;
        private void onchannel10(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel10;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel11;
        private void onchannel11(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel11;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel12;
        private void onchannel12(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel12;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel13;
        private void onchannel13(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel13;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel14;
        private void onchannel14(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel14;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel15;
        private void onchannel15(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel15;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel16;
        private void onchannel16(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel16;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel17;
        private void onchannel17(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel17;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel18;
        private void onchannel18(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel18;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel19;
        private void onchannel19(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel19;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel20;
        private void onchannel20(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel20;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void selectedChannelFb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.selectedChannelFb], this);
            }
        }

        public void channel1Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel1Fb], this);
            }
        }

        public void channel2Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel2Fb], this);
            }
        }

        public void channel3Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel3Fb], this);
            }
        }

        public void channel4Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel4Fb], this);
            }
        }

        public void channel5Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel5Fb], this);
            }
        }

        public void channel6Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel6Fb], this);
            }
        }

        public void channel7Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel7Fb], this);
            }
        }

        public void channel8Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel8Fb], this);
            }
        }

        public void channel9Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel9Fb], this);
            }
        }

        public void channel10Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel10Fb], this);
            }
        }

        public void channel11Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel11Fb], this);
            }
        }

        public void channel12Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel12Fb], this);
            }
        }

        public void channel13Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel13Fb], this);
            }
        }

        public void channel14Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel14Fb], this);
            }
        }

        public void channel15Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel15Fb], this);
            }
        }

        public void channel16Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel16Fb], this);
            }
        }

        public void channel17Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel17Fb], this);
            }
        }

        public void channel18Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel18Fb], this);
            }
        }

        public void channel19Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel19Fb], this);
            }
        }

        public void channel20Fb(monitorControlStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel20Fb], this);
            }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return (int)ControlJoinId;
        }

        public override string ToString()
        {
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "monitorControl", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            signage = null;
            tvPlayer = null;
            laptopInput = null;
            ledOn = null;
            ledOff = null;
            showChannelsPage = null;
            channelsAvailable = null;
            videoCon = null;
            videoConPermitted = null;
            selectedChannel = null;
            channel1 = null;
            channel2 = null;
            channel3 = null;
            channel4 = null;
            channel5 = null;
            channel6 = null;
            channel7 = null;
            channel8 = null;
            channel9 = null;
            channel10 = null;
            channel11 = null;
            channel12 = null;
            channel13 = null;
            channel14 = null;
            channel15 = null;
            channel16 = null;
            channel17 = null;
            channel18 = null;
            channel19 = null;
            channel20 = null;
        }

        #endregion

    }
}
