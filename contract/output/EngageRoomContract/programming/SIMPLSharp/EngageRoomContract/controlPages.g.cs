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

        event EventHandler<UIEventArgs> page;
        event EventHandler<UIEventArgs> previousPage;

        void pageFb(controlPagesUShortInputSigDelegate callback);
        void previousPageFb(controlPagesUShortInputSigDelegate callback);

    }

    public delegate void controlPagesUShortInputSigDelegate(UShortInputSig uShortInputSig, IcontrolPages controlPages);

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
            internal static class Numerics
            {
                public const uint page = 1;
                public const uint previousPage = 2;

                public const uint pageFb = 1;
                public const uint previousPageFb = 2;
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
 
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.page, onpage);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.previousPage, onpreviousPage);

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

        public event EventHandler<UIEventArgs> page;
        private void onpage(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = page;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> previousPage;
        private void onpreviousPage(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = previousPage;
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

        public void previousPageFb(controlPagesUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.previousPageFb], this);
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

            page = null;
            previousPage = null;
        }

        #endregion

    }
}
