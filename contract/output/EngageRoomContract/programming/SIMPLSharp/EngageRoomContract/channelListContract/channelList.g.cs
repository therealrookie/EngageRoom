using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace EngageRoomContract.channelListContract
{
    public interface IchannelList
    {
        object UserObject { get; set; }

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
        event EventHandler<UIEventArgs> channel21;
        event EventHandler<UIEventArgs> channel22;
        event EventHandler<UIEventArgs> channel23;
        event EventHandler<UIEventArgs> channel24;
        event EventHandler<UIEventArgs> channel25;
        event EventHandler<UIEventArgs> channel26;
        event EventHandler<UIEventArgs> channel27;
        event EventHandler<UIEventArgs> channel28;
        event EventHandler<UIEventArgs> channel29;
        event EventHandler<UIEventArgs> channel30;
        event EventHandler<UIEventArgs> channel31;
        event EventHandler<UIEventArgs> channel32;
        event EventHandler<UIEventArgs> channel33;
        event EventHandler<UIEventArgs> channel34;
        event EventHandler<UIEventArgs> channel35;
        event EventHandler<UIEventArgs> channel36;
        event EventHandler<UIEventArgs> channel37;
        event EventHandler<UIEventArgs> channel38;
        event EventHandler<UIEventArgs> channel39;
        event EventHandler<UIEventArgs> channel40;

        void selectedChannelFb(channelListStringInputSigDelegate callback);
        void channel1Fb(channelListStringInputSigDelegate callback);
        void channel2Fb(channelListStringInputSigDelegate callback);
        void channel3Fb(channelListStringInputSigDelegate callback);
        void channel4Fb(channelListStringInputSigDelegate callback);
        void channel5Fb(channelListStringInputSigDelegate callback);
        void channel6Fb(channelListStringInputSigDelegate callback);
        void channel7Fb(channelListStringInputSigDelegate callback);
        void channel8Fb(channelListStringInputSigDelegate callback);
        void channel9Fb(channelListStringInputSigDelegate callback);
        void channel10Fb(channelListStringInputSigDelegate callback);
        void channel11Fb(channelListStringInputSigDelegate callback);
        void channel12Fb(channelListStringInputSigDelegate callback);
        void channel13Fb(channelListStringInputSigDelegate callback);
        void channel14Fb(channelListStringInputSigDelegate callback);
        void channel15Fb(channelListStringInputSigDelegate callback);
        void channel16Fb(channelListStringInputSigDelegate callback);
        void channel17Fb(channelListStringInputSigDelegate callback);
        void channel18Fb(channelListStringInputSigDelegate callback);
        void channel19Fb(channelListStringInputSigDelegate callback);
        void channel20Fb(channelListStringInputSigDelegate callback);
        void channel21Fb(channelListStringInputSigDelegate callback);
        void channel22Fb(channelListStringInputSigDelegate callback);
        void channel23Fb(channelListStringInputSigDelegate callback);
        void channel24Fb(channelListStringInputSigDelegate callback);
        void channel25Fb(channelListStringInputSigDelegate callback);
        void channel26Fb(channelListStringInputSigDelegate callback);
        void channel27Fb(channelListStringInputSigDelegate callback);
        void channel28Fb(channelListStringInputSigDelegate callback);
        void channel29Fb(channelListStringInputSigDelegate callback);
        void channel30Fb(channelListStringInputSigDelegate callback);
        void channel31Fb(channelListStringInputSigDelegate callback);
        void channel32Fb(channelListStringInputSigDelegate callback);
        void channel33Fb(channelListStringInputSigDelegate callback);
        void channel34Fb(channelListStringInputSigDelegate callback);
        void channel35Fb(channelListStringInputSigDelegate callback);
        void channel36Fb(channelListStringInputSigDelegate callback);
        void channel37Fb(channelListStringInputSigDelegate callback);
        void channel38Fb(channelListStringInputSigDelegate callback);
        void channel39Fb(channelListStringInputSigDelegate callback);
        void channel40Fb(channelListStringInputSigDelegate callback);

    }

    public delegate void channelListStringInputSigDelegate(StringInputSig stringInputSig, IchannelList channelList);

    internal class channelList : IchannelList, IDisposable
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
                public const uint channel21 = 22;
                public const uint channel22 = 23;
                public const uint channel23 = 24;
                public const uint channel24 = 25;
                public const uint channel25 = 26;
                public const uint channel26 = 27;
                public const uint channel27 = 28;
                public const uint channel28 = 29;
                public const uint channel29 = 30;
                public const uint channel30 = 31;
                public const uint channel31 = 32;
                public const uint channel32 = 33;
                public const uint channel33 = 34;
                public const uint channel34 = 35;
                public const uint channel35 = 36;
                public const uint channel36 = 37;
                public const uint channel37 = 38;
                public const uint channel38 = 39;
                public const uint channel39 = 40;
                public const uint channel40 = 41;

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
                public const uint channel21Fb = 22;
                public const uint channel22Fb = 23;
                public const uint channel23Fb = 24;
                public const uint channel24Fb = 25;
                public const uint channel25Fb = 26;
                public const uint channel26Fb = 27;
                public const uint channel27Fb = 28;
                public const uint channel28Fb = 29;
                public const uint channel29Fb = 30;
                public const uint channel30Fb = 31;
                public const uint channel31Fb = 32;
                public const uint channel32Fb = 33;
                public const uint channel33Fb = 34;
                public const uint channel34Fb = 35;
                public const uint channel35Fb = 36;
                public const uint channel36Fb = 37;
                public const uint channel37Fb = 38;
                public const uint channel38Fb = 39;
                public const uint channel39Fb = 40;
                public const uint channel40Fb = 41;
            }
        }

        #endregion

        #region Construction and Initialization

        internal channelList(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
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
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel21, onchannel21);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel22, onchannel22);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel23, onchannel23);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel24, onchannel24);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel25, onchannel25);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel26, onchannel26);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel27, onchannel27);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel28, onchannel28);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel29, onchannel29);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel30, onchannel30);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel31, onchannel31);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel32, onchannel32);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel33, onchannel33);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel34, onchannel34);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel35, onchannel35);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel36, onchannel36);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel37, onchannel37);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel38, onchannel38);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel39, onchannel39);
            ComponentMediator.ConfigureStringEvent(controlJoinId, Joins.Strings.channel40, onchannel40);

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

        public event EventHandler<UIEventArgs> channel21;
        private void onchannel21(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel21;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel22;
        private void onchannel22(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel22;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel23;
        private void onchannel23(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel23;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel24;
        private void onchannel24(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel24;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel25;
        private void onchannel25(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel25;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel26;
        private void onchannel26(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel26;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel27;
        private void onchannel27(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel27;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel28;
        private void onchannel28(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel28;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel29;
        private void onchannel29(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel29;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel30;
        private void onchannel30(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel30;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel31;
        private void onchannel31(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel31;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel32;
        private void onchannel32(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel32;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel33;
        private void onchannel33(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel33;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel34;
        private void onchannel34(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel34;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel35;
        private void onchannel35(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel35;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel36;
        private void onchannel36(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel36;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel37;
        private void onchannel37(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel37;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel38;
        private void onchannel38(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel38;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel39;
        private void onchannel39(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel39;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> channel40;
        private void onchannel40(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = channel40;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void selectedChannelFb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.selectedChannelFb], this);
            }
        }

        public void channel1Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel1Fb], this);
            }
        }

        public void channel2Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel2Fb], this);
            }
        }

        public void channel3Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel3Fb], this);
            }
        }

        public void channel4Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel4Fb], this);
            }
        }

        public void channel5Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel5Fb], this);
            }
        }

        public void channel6Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel6Fb], this);
            }
        }

        public void channel7Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel7Fb], this);
            }
        }

        public void channel8Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel8Fb], this);
            }
        }

        public void channel9Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel9Fb], this);
            }
        }

        public void channel10Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel10Fb], this);
            }
        }

        public void channel11Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel11Fb], this);
            }
        }

        public void channel12Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel12Fb], this);
            }
        }

        public void channel13Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel13Fb], this);
            }
        }

        public void channel14Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel14Fb], this);
            }
        }

        public void channel15Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel15Fb], this);
            }
        }

        public void channel16Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel16Fb], this);
            }
        }

        public void channel17Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel17Fb], this);
            }
        }

        public void channel18Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel18Fb], this);
            }
        }

        public void channel19Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel19Fb], this);
            }
        }

        public void channel20Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel20Fb], this);
            }
        }

        public void channel21Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel21Fb], this);
            }
        }

        public void channel22Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel22Fb], this);
            }
        }

        public void channel23Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel23Fb], this);
            }
        }

        public void channel24Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel24Fb], this);
            }
        }

        public void channel25Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel25Fb], this);
            }
        }

        public void channel26Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel26Fb], this);
            }
        }

        public void channel27Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel27Fb], this);
            }
        }

        public void channel28Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel28Fb], this);
            }
        }

        public void channel29Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel29Fb], this);
            }
        }

        public void channel30Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel30Fb], this);
            }
        }

        public void channel31Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel31Fb], this);
            }
        }

        public void channel32Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel32Fb], this);
            }
        }

        public void channel33Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel33Fb], this);
            }
        }

        public void channel34Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel34Fb], this);
            }
        }

        public void channel35Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel35Fb], this);
            }
        }

        public void channel36Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel36Fb], this);
            }
        }

        public void channel37Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel37Fb], this);
            }
        }

        public void channel38Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel38Fb], this);
            }
        }

        public void channel39Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel39Fb], this);
            }
        }

        public void channel40Fb(channelListStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.channel40Fb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "channelList", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

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
            channel21 = null;
            channel22 = null;
            channel23 = null;
            channel24 = null;
            channel25 = null;
            channel26 = null;
            channel27 = null;
            channel28 = null;
            channel29 = null;
            channel30 = null;
            channel31 = null;
            channel32 = null;
            channel33 = null;
            channel34 = null;
            channel35 = null;
            channel36 = null;
            channel37 = null;
            channel38 = null;
            channel39 = null;
            channel40 = null;
        }

        #endregion

    }
}
