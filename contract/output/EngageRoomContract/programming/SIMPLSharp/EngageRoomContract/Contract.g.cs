using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract
{
    /// <summary>
    /// Common Interface for Root Contracts.
    /// </summary>
    public interface IContract
    {
        object UserObject { get; set; }
        void AddDevice(BasicTriListWithSmartObject device);
        void RemoveDevice(BasicTriListWithSmartObject device);
    }

    public class Contract : IContract, IDisposable
    {
        #region Components

        private ComponentMediator ComponentMediator { get; set; }

        public EngageRoomContract.IcontrolPages controlPages { get { return (EngageRoomContract.IcontrolPages)InternalcontrolPages; } }
        private EngageRoomContract.controlPages InternalcontrolPages { get; set; }

        public EngageRoomContract.IselectLanguage selectLanguage { get { return (EngageRoomContract.IselectLanguage)InternalselectLanguage; } }
        private EngageRoomContract.selectLanguage InternalselectLanguage { get; set; }

        public EngageRoomContract.IlocalUse localUse { get { return (EngageRoomContract.IlocalUse)InternallocalUse; } }
        private EngageRoomContract.localUse InternallocalUse { get; set; }

        public EngageRoomContract.IledControl ledControl { get { return (EngageRoomContract.IledControl)InternalledControl; } }
        private EngageRoomContract.ledControl InternalledControl { get; set; }

        public EngageRoomContract.ImonitorControl monitorControl { get { return (EngageRoomContract.ImonitorControl)InternalmonitorControl; } }
        private EngageRoomContract.monitorControl InternalmonitorControl { get; set; }

        public EngageRoomContract.IcameraControl cameraControl { get { return (EngageRoomContract.IcameraControl)InternalcameraControl; } }
        private EngageRoomContract.cameraControl InternalcameraControl { get; set; }

        public EngageRoomContract.ImeetingControl meetingControl { get { return (EngageRoomContract.ImeetingControl)InternalmeetingControl; } }
        private EngageRoomContract.meetingControl InternalmeetingControl { get; set; }

        #endregion

        #region Construction and Initialization

        public Contract()
            : this(new List<BasicTriListWithSmartObject>().ToArray())
        {
        }

        public Contract(BasicTriListWithSmartObject device)
            : this(new [] { device })
        {
        }

        public Contract(BasicTriListWithSmartObject[] devices)
        {
            if (devices == null)
                throw new ArgumentNullException("Devices is null");

            ComponentMediator = new ComponentMediator();

            InternalcontrolPages = new EngageRoomContract.controlPages(ComponentMediator, 1);
            InternalselectLanguage = new EngageRoomContract.selectLanguage(ComponentMediator, 2);
            InternallocalUse = new EngageRoomContract.localUse(ComponentMediator, 3);
            InternalledControl = new EngageRoomContract.ledControl(ComponentMediator, 4);
            InternalmonitorControl = new EngageRoomContract.monitorControl(ComponentMediator, 5);
            InternalcameraControl = new EngageRoomContract.cameraControl(ComponentMediator, 6);
            InternalmeetingControl = new EngageRoomContract.meetingControl(ComponentMediator, 7);

            for (int index = 0; index < devices.Length; index++)
            {
                AddDevice(devices[index]);
            }
        }

        #endregion

        #region Standard Contract Members

        public object UserObject { get; set; }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            InternalcontrolPages.AddDevice(device);
            InternalselectLanguage.AddDevice(device);
            InternallocalUse.AddDevice(device);
            InternalledControl.AddDevice(device);
            InternalmonitorControl.AddDevice(device);
            InternalcameraControl.AddDevice(device);
            InternalmeetingControl.AddDevice(device);
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            InternalcontrolPages.RemoveDevice(device);
            InternalselectLanguage.RemoveDevice(device);
            InternallocalUse.RemoveDevice(device);
            InternalledControl.RemoveDevice(device);
            InternalmonitorControl.RemoveDevice(device);
            InternalcameraControl.RemoveDevice(device);
            InternalmeetingControl.RemoveDevice(device);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            InternalcontrolPages.Dispose();
            InternalselectLanguage.Dispose();
            InternallocalUse.Dispose();
            InternalledControl.Dispose();
            InternalmonitorControl.Dispose();
            InternalcameraControl.Dispose();
            InternalmeetingControl.Dispose();
            ComponentMediator.Dispose(); 
        }

        #endregion

    }
}
