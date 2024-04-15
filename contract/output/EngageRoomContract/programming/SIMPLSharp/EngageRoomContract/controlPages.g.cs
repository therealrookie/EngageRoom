using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    public interface IcontrolPages
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> isLoading;
        event EventHandler<UIEventArgs> meetingActivated;
        event EventHandler<UIEventArgs> meetingDeactivated;
        event EventHandler<UIEventArgs> camControlActivated;
        event EventHandler<UIEventArgs> camControlDeactivated;
        event EventHandler<UIEventArgs> page;
        event EventHandler<UIEventArgs> codeInput;

        void isLoadingFb(controlPagesBoolInputSigDelegate callback);
        void meetingActivatedFb(controlPagesBoolInputSigDelegate callback);
        void meetingDeactivatedFb(controlPagesBoolInputSigDelegate callback);
        void camControlActivatedFb(controlPagesBoolInputSigDelegate callback);
        void camControlDeactivatedFb(controlPagesBoolInputSigDelegate callback);
        void pageFb(controlPagesUShortInputSigDelegate callback);
        void codeInputFb(controlPagesStringInputSigDelegate callback);

    }

    public delegate void controlPagesBoolInputSigDelegate(BoolInputSig boolInputSig, IcontrolPages controlPages);
    public delegate void controlPagesUShortInputSigDelegate(UShortInputSig uShortInputSig, IcontrolPages controlPages);
    public delegate void controlPagesStringInputSigDelegate(StringInputSig stringInputSig, IcontrolPages controlPages);

    internal class controlPages : IcontrolPages, IDisposable
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
                public const uint isLoading = 1;
                public const uint meetingActivated = 2;
                public const uint meetingDeactivated = 3;
                public const uint camControlActivated = 4;
                public const uint camControlDeactivated = 5;

                public const uint isLoadingFb = 1;
                public const uint meetingActivatedFb = 2;
                public const uint meetingDeactivatedFb = 3;
                public const uint camControlActivatedFb = 4;
                public const uint camControlDeactivatedFb = 5;
            }
            internal static class Numerics
            {
                public const uint page = 1;

                public const uint pageFb = 1;
            }
            internal static class Strings
            {
                public const uint codeInput = 1;

                public const uint codeInputFb = 1;
            }
        }

        #endregion

        #region Construction and Initialization

        internal controlPages(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.isLoading, onisLoading);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.meetingActivated, onmeetingActivated);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.meetingDeactivated, onmeetingDeactivated);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.camControlActivated, oncamControlActivated);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.camControlDeactivated, oncamControlDeactivated);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.page, onpage);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.codeInput, oncodeInput);

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

        public event EventHandler<UIEventArgs> isLoading;
        private void onisLoading(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = isLoading;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> meetingActivated;
        private void onmeetingActivated(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = meetingActivated;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> meetingDeactivated;
        private void onmeetingDeactivated(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = meetingDeactivated;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> camControlActivated;
        private void oncamControlActivated(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = camControlActivated;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> camControlDeactivated;
        private void oncamControlDeactivated(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = camControlDeactivated;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void isLoadingFb(controlPagesBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.isLoadingFb], this);
            }
        }

        public void meetingActivatedFb(controlPagesBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.meetingActivatedFb], this);
            }
        }

        public void meetingDeactivatedFb(controlPagesBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.meetingDeactivatedFb], this);
            }
        }

        public void camControlActivatedFb(controlPagesBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.camControlActivatedFb], this);
            }
        }

        public void camControlDeactivatedFb(controlPagesBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.camControlDeactivatedFb], this);
            }
        }

        public event EventHandler<UIEventArgs> page;
        private void onpage(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = page;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void pageFb(controlPagesUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.pageFb], this);
            }
        }

        public event EventHandler<UIEventArgs> codeInput;
        private void oncodeInput(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = codeInput;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void codeInputFb(controlPagesStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.codeInputFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "controlPages", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            isLoading = null;
            meetingActivated = null;
            meetingDeactivated = null;
            camControlActivated = null;
            camControlDeactivated = null;
            page = null;
            codeInput = null;
        }

        #endregion

    }
}
