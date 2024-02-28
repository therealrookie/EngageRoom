using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    public interface IledControl
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> signage;
        event EventHandler<UIEventArgs> tvPlayer;
        event EventHandler<UIEventArgs> laptopInput;
        event EventHandler<UIEventArgs> ledOn;
        event EventHandler<UIEventArgs> ledOff;
        event EventHandler<UIEventArgs> videoCon;
        event EventHandler<UIEventArgs> videoConPermitted;

        void signageFb(ledControlBoolInputSigDelegate callback);
        void tvPlayerFb(ledControlBoolInputSigDelegate callback);
        void laptopInputFb(ledControlBoolInputSigDelegate callback);
        void ledOnFb(ledControlBoolInputSigDelegate callback);
        void ledOffFb(ledControlBoolInputSigDelegate callback);
        void videoConFb(ledControlBoolInputSigDelegate callback);
        void videoConPermittedFb(ledControlBoolInputSigDelegate callback);

    }

    public delegate void ledControlBoolInputSigDelegate(BoolInputSig boolInputSig, IledControl ledControl);

    internal class ledControl : IledControl, IDisposable
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
                public const uint videoCon = 6;
                public const uint videoConPermitted = 7;

                public const uint signageFb = 1;
                public const uint tvPlayerFb = 2;
                public const uint laptopInputFb = 3;
                public const uint ledOnFb = 4;
                public const uint ledOffFb = 5;
                public const uint videoConFb = 6;
                public const uint videoConPermittedFb = 7;
            }
        }

        #endregion

        #region Construction and Initialization

        internal ledControl(ComponentMediator componentMediator, uint controlJoinId)
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
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.videoCon, onvideoCon);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.videoConPermitted, onvideoConPermitted);

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


        public void signageFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.signageFb], this);
            }
        }

        public void tvPlayerFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.tvPlayerFb], this);
            }
        }

        public void laptopInputFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.laptopInputFb], this);
            }
        }

        public void ledOnFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.ledOnFb], this);
            }
        }

        public void ledOffFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.ledOffFb], this);
            }
        }

        public void videoConFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.videoConFb], this);
            }
        }

        public void videoConPermittedFb(ledControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.videoConPermittedFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "ledControl", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
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
            videoCon = null;
            videoConPermitted = null;
        }

        #endregion

    }
}
