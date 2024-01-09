using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    public interface IlocalUse
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> volumeChangedBrandMusic;
        event EventHandler<UIEventArgs> volumeChangedMicVolume;
        event EventHandler<UIEventArgs> volumeChangedMediaLevel;
        event EventHandler<UIEventArgs> brandMusicVolume;
        event EventHandler<UIEventArgs> micVolume;
        event EventHandler<UIEventArgs> mediaLevel;

        void volumeChangedBrandMusicFb(localUseBoolInputSigDelegate callback);
        void volumeChangedMicVolumeFb(localUseBoolInputSigDelegate callback);
        void volumeChangedMediaLevelFb(localUseBoolInputSigDelegate callback);
        void brandMusicVolumeFb(localUseUShortInputSigDelegate callback);
        void micVolumeFb(localUseUShortInputSigDelegate callback);
        void mediaLevelFb(localUseUShortInputSigDelegate callback);

    }

    public delegate void localUseBoolInputSigDelegate(BoolInputSig boolInputSig, IlocalUse localUse);
    public delegate void localUseUShortInputSigDelegate(UShortInputSig uShortInputSig, IlocalUse localUse);

    internal class localUse : IlocalUse, IDisposable
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
                public const uint volumeChangedBrandMusic = 1;
                public const uint volumeChangedMicVolume = 2;
                public const uint volumeChangedMediaLevel = 3;

                public const uint volumeChangedBrandMusicFb = 1;
                public const uint volumeChangedMicVolumeFb = 2;
                public const uint volumeChangedMediaLevelFb = 3;
            }
            internal static class Numerics
            {
                public const uint brandMusicVolume = 1;
                public const uint micVolume = 2;
                public const uint mediaLevel = 3;

                public const uint brandMusicVolumeFb = 1;
                public const uint micVolumeFb = 2;
                public const uint mediaLevelFb = 3;
            }
        }

        #endregion

        #region Construction and Initialization

        internal localUse(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeChangedBrandMusic, onvolumeChangedBrandMusic);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeChangedMicVolume, onvolumeChangedMicVolume);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeChangedMediaLevel, onvolumeChangedMediaLevel);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.brandMusicVolume, onbrandMusicVolume);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.micVolume, onmicVolume);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.mediaLevel, onmediaLevel);

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

        public event EventHandler<UIEventArgs> volumeChangedBrandMusic;
        private void onvolumeChangedBrandMusic(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeChangedBrandMusic;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> volumeChangedMicVolume;
        private void onvolumeChangedMicVolume(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeChangedMicVolume;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> volumeChangedMediaLevel;
        private void onvolumeChangedMediaLevel(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeChangedMediaLevel;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void volumeChangedBrandMusicFb(localUseBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.volumeChangedBrandMusicFb], this);
            }
        }

        public void volumeChangedMicVolumeFb(localUseBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.volumeChangedMicVolumeFb], this);
            }
        }

        public void volumeChangedMediaLevelFb(localUseBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.volumeChangedMediaLevelFb], this);
            }
        }

        public event EventHandler<UIEventArgs> brandMusicVolume;
        private void onbrandMusicVolume(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = brandMusicVolume;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> micVolume;
        private void onmicVolume(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = micVolume;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> mediaLevel;
        private void onmediaLevel(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = mediaLevel;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void brandMusicVolumeFb(localUseUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.brandMusicVolumeFb], this);
            }
        }

        public void micVolumeFb(localUseUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.micVolumeFb], this);
            }
        }

        public void mediaLevelFb(localUseUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.mediaLevelFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "localUse", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            volumeChangedBrandMusic = null;
            volumeChangedMicVolume = null;
            volumeChangedMediaLevel = null;
            brandMusicVolume = null;
            micVolume = null;
            mediaLevel = null;
        }

        #endregion

    }
}
