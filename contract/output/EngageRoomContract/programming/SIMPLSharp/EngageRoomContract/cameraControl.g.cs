using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    public interface IcameraControl
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> preset01;
        event EventHandler<UIEventArgs> preset02;
        event EventHandler<UIEventArgs> preset03;
        event EventHandler<UIEventArgs> Left;
        event EventHandler<UIEventArgs> Right;
        event EventHandler<UIEventArgs> Up;
        event EventHandler<UIEventArgs> Down;
        event EventHandler<UIEventArgs> Center;
        event EventHandler<UIEventArgs> zoomPlus;
        event EventHandler<UIEventArgs> zoomMinus;
        event EventHandler<UIEventArgs> call;
        event EventHandler<UIEventArgs> setBtn;

        void preset01Fb(cameraControlBoolInputSigDelegate callback);
        void preset02Fb(cameraControlBoolInputSigDelegate callback);
        void preset03Fb(cameraControlBoolInputSigDelegate callback);
        void zoomPlusFb(cameraControlBoolInputSigDelegate callback);
        void zoomMinusFb(cameraControlBoolInputSigDelegate callback);
        void callFb(cameraControlBoolInputSigDelegate callback);
        void setBtnFb(cameraControlBoolInputSigDelegate callback);

    }

    public delegate void cameraControlBoolInputSigDelegate(BoolInputSig boolInputSig, IcameraControl cameraControl);

    internal class cameraControl : IcameraControl, IDisposable
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
                public const uint preset01 = 1;
                public const uint preset02 = 2;
                public const uint preset03 = 3;
                public const uint Left = 4;
                public const uint Right = 5;
                public const uint Up = 6;
                public const uint Down = 7;
                public const uint Center = 8;
                public const uint zoomPlus = 9;
                public const uint zoomMinus = 10;
                public const uint call = 11;
                public const uint setBtn = 12;

                public const uint preset01Fb = 1;
                public const uint preset02Fb = 2;
                public const uint preset03Fb = 3;
                public const uint zoomPlusFb = 9;
                public const uint zoomMinusFb = 10;
                public const uint callFb = 11;
                public const uint setBtnFb = 12;
            }
        }

        #endregion

        #region Construction and Initialization

        internal cameraControl(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.preset01, onpreset01);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.preset02, onpreset02);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.preset03, onpreset03);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.Left, onLeft);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.Right, onRight);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.Up, onUp);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.Down, onDown);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.Center, onCenter);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.zoomPlus, onzoomPlus);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.zoomMinus, onzoomMinus);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.call, oncall);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.setBtn, onsetBtn);

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

        public event EventHandler<UIEventArgs> preset01;
        private void onpreset01(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = preset01;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> preset02;
        private void onpreset02(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = preset02;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> preset03;
        private void onpreset03(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = preset03;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> Left;
        private void onLeft(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = Left;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> Right;
        private void onRight(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = Right;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> Up;
        private void onUp(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = Up;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> Down;
        private void onDown(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = Down;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> Center;
        private void onCenter(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = Center;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> zoomPlus;
        private void onzoomPlus(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = zoomPlus;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> zoomMinus;
        private void onzoomMinus(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = zoomMinus;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> call;
        private void oncall(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = call;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> setBtn;
        private void onsetBtn(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = setBtn;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void preset01Fb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.preset01Fb], this);
            }
        }

        public void preset02Fb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.preset02Fb], this);
            }
        }

        public void preset03Fb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.preset03Fb], this);
            }
        }

        public void zoomPlusFb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.zoomPlusFb], this);
            }
        }

        public void zoomMinusFb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.zoomMinusFb], this);
            }
        }

        public void callFb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.callFb], this);
            }
        }

        public void setBtnFb(cameraControlBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.setBtnFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "cameraControl", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            preset01 = null;
            preset02 = null;
            preset03 = null;
            Left = null;
            Right = null;
            Up = null;
            Down = null;
            Center = null;
            zoomPlus = null;
            zoomMinus = null;
            call = null;
            setBtn = null;
        }

        #endregion

    }
}
