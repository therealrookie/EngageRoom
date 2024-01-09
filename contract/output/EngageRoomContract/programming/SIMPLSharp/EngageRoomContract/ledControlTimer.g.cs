using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    public interface IledControlTimer
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> monday;
        event EventHandler<UIEventArgs> tuesday;
        event EventHandler<UIEventArgs> wednesday;
        event EventHandler<UIEventArgs> thursday;
        event EventHandler<UIEventArgs> friday;
        event EventHandler<UIEventArgs> saturday;
        event EventHandler<UIEventArgs> sunday;
        event EventHandler<UIEventArgs> signage;
        event EventHandler<UIEventArgs> tvPlayer;
        event EventHandler<UIEventArgs> laptopInput;
        event EventHandler<UIEventArgs> startupWd;
        event EventHandler<UIEventArgs> shutdownWd;
        event EventHandler<UIEventArgs> startupWe;
        event EventHandler<UIEventArgs> shutdownWe;

        void mondayFb(ledControlTimerBoolInputSigDelegate callback);
        void tuesdayFb(ledControlTimerBoolInputSigDelegate callback);
        void wednesdayFb(ledControlTimerBoolInputSigDelegate callback);
        void thursdayFb(ledControlTimerBoolInputSigDelegate callback);
        void fridayFb(ledControlTimerBoolInputSigDelegate callback);
        void saturdayFb(ledControlTimerBoolInputSigDelegate callback);
        void sundayFb(ledControlTimerBoolInputSigDelegate callback);
        void signageFb(ledControlTimerBoolInputSigDelegate callback);
        void tvPlayerFb(ledControlTimerBoolInputSigDelegate callback);
        void laptopInputFb(ledControlTimerBoolInputSigDelegate callback);
        void startupWdFb(ledControlTimerStringInputSigDelegate callback);
        void shutdownWdFb(ledControlTimerStringInputSigDelegate callback);
        void startupWeFb(ledControlTimerStringInputSigDelegate callback);
        void shutdownWeFb(ledControlTimerStringInputSigDelegate callback);

    }

    public delegate void ledControlTimerBoolInputSigDelegate(BoolInputSig boolInputSig, IledControlTimer ledControlTimer);
    public delegate void ledControlTimerStringInputSigDelegate(StringInputSig stringInputSig, IledControlTimer ledControlTimer);

    internal class ledControlTimer : IledControlTimer, IDisposable
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
                public const uint monday = 1;
                public const uint tuesday = 2;
                public const uint wednesday = 3;
                public const uint thursday = 4;
                public const uint friday = 5;
                public const uint saturday = 6;
                public const uint sunday = 7;
                public const uint signage = 8;
                public const uint tvPlayer = 9;
                public const uint laptopInput = 10;

                public const uint mondayFb = 1;
                public const uint tuesdayFb = 2;
                public const uint wednesdayFb = 3;
                public const uint thursdayFb = 4;
                public const uint fridayFb = 5;
                public const uint saturdayFb = 6;
                public const uint sundayFb = 7;
                public const uint signageFb = 8;
                public const uint tvPlayerFb = 9;
                public const uint laptopInputFb = 10;
            }
            internal static class Strings
            {
                public const uint startupWd = 1;
                public const uint shutdownWd = 2;
                public const uint startupWe = 3;
                public const uint shutdownWe = 4;

                public const uint startupWdFb = 1;
                public const uint shutdownWdFb = 2;
                public const uint startupWeFb = 3;
                public const uint shutdownWeFb = 4;
            }
        }

        #endregion

        #region Construction and Initialization

        internal ledControlTimer(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.monday, onmonday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.tuesday, ontuesday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.wednesday, onwednesday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.thursday, onthursday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.friday, onfriday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.saturday, onsaturday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.sunday, onsunday);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.signage, onsignage);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.tvPlayer, ontvPlayer);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.laptopInput, onlaptopInput);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.startupWd, onstartupWd);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.shutdownWd, onshutdownWd);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.startupWe, onstartupWe);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.shutdownWe, onshutdownWe);

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

        public event EventHandler<UIEventArgs> monday;
        private void onmonday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = monday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> tuesday;
        private void ontuesday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = tuesday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> wednesday;
        private void onwednesday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = wednesday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> thursday;
        private void onthursday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = thursday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> friday;
        private void onfriday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = friday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> saturday;
        private void onsaturday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = saturday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> sunday;
        private void onsunday(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = sunday;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

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


        public void mondayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.mondayFb], this);
            }
        }

        public void tuesdayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.tuesdayFb], this);
            }
        }

        public void wednesdayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.wednesdayFb], this);
            }
        }

        public void thursdayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.thursdayFb], this);
            }
        }

        public void fridayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.fridayFb], this);
            }
        }

        public void saturdayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.saturdayFb], this);
            }
        }

        public void sundayFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.sundayFb], this);
            }
        }

        public void signageFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.signageFb], this);
            }
        }

        public void tvPlayerFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.tvPlayerFb], this);
            }
        }

        public void laptopInputFb(ledControlTimerBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.laptopInputFb], this);
            }
        }

        public event EventHandler<UIEventArgs> startupWd;
        private void onstartupWd(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = startupWd;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> shutdownWd;
        private void onshutdownWd(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = shutdownWd;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> startupWe;
        private void onstartupWe(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = startupWe;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> shutdownWe;
        private void onshutdownWe(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = shutdownWe;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void startupWdFb(ledControlTimerStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.startupWdFb], this);
            }
        }

        public void shutdownWdFb(ledControlTimerStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.shutdownWdFb], this);
            }
        }

        public void startupWeFb(ledControlTimerStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.startupWeFb], this);
            }
        }

        public void shutdownWeFb(ledControlTimerStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.shutdownWeFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "ledControlTimer", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            monday = null;
            tuesday = null;
            wednesday = null;
            thursday = null;
            friday = null;
            saturday = null;
            sunday = null;
            signage = null;
            tvPlayer = null;
            laptopInput = null;
            startupWd = null;
            shutdownWd = null;
            startupWe = null;
            shutdownWe = null;
        }

        #endregion

    }
}
