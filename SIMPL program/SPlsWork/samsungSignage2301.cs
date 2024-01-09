using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;

namespace UserModule_SAMSUNGSIGNAGE2301
{
    public class UserModuleClass_SAMSUNGSIGNAGE2301 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput CONNECT;
        Crestron.Logos.SplusObjects.DigitalInput GETPOWERSTATE;
        Crestron.Logos.SplusObjects.DigitalInput POWERON;
        Crestron.Logos.SplusObjects.DigitalInput POWEROFF;
        Crestron.Logos.SplusObjects.DigitalInput GETINPUTSTATE;
        Crestron.Logos.SplusObjects.DigitalInput INPUTDP1;
        Crestron.Logos.SplusObjects.DigitalInput INPUTDP2;
        Crestron.Logos.SplusObjects.DigitalInput INPUTDP3;
        Crestron.Logos.SplusObjects.DigitalInput INPUTHDMI1;
        Crestron.Logos.SplusObjects.DigitalInput INPUTHDMI2;
        Crestron.Logos.SplusObjects.DigitalInput INPUTHDMI3;
        Crestron.Logos.SplusObjects.DigitalInput INPUTDVI;
        Crestron.Logos.SplusObjects.DigitalInput GETAUDIOMUTESTATE;
        Crestron.Logos.SplusObjects.DigitalInput AUDIOMUTEONOFF;
        Crestron.Logos.SplusObjects.DigitalInput AUDIOMUTEON;
        Crestron.Logos.SplusObjects.DigitalInput AUDIOMUTEOFF;
        Crestron.Logos.SplusObjects.DigitalInput GETVIDEOMUTESTATE;
        Crestron.Logos.SplusObjects.DigitalInput VIDEOMUTEONOFF;
        Crestron.Logos.SplusObjects.DigitalInput VIDEOMUTEON;
        Crestron.Logos.SplusObjects.DigitalInput VIDEOMUTEOFF;
        Crestron.Logos.SplusObjects.DigitalInput GETSERIALNUMBER;
        Crestron.Logos.SplusObjects.DigitalInput GETDISPLAYSTATUS;
        Crestron.Logos.SplusObjects.DigitalInput GETSOFTWAREVERSION;
        Crestron.Logos.SplusObjects.DigitalInput GETTEMPERATUREDEV;
        Crestron.Logos.SplusObjects.DigitalInput GETTEMPERATUREEXE;
        Crestron.Logos.SplusObjects.DigitalInput GETTEMPERATURELED;
        Crestron.Logos.SplusObjects.AnalogInput AUDIOVOLUME;
        Crestron.Logos.SplusObjects.DigitalOutput CONNECTFBK;
        Crestron.Logos.SplusObjects.DigitalOutput WARNFBK;
        Crestron.Logos.SplusObjects.DigitalOutput ERRORFBK;
        Crestron.Logos.SplusObjects.DigitalOutput POWERFBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTDP1FBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTDP2FBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTDP3FBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTHDMI1FBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTHDMI2FBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTHDMI3FBK;
        Crestron.Logos.SplusObjects.DigitalOutput INPUTDVIFBK;
        Crestron.Logos.SplusObjects.DigitalOutput AUDIOMUTEFBK;
        Crestron.Logos.SplusObjects.DigitalOutput VIDEOMUTEFBK;
        Crestron.Logos.SplusObjects.DigitalOutput STATUSLAMPERR;
        Crestron.Logos.SplusObjects.DigitalOutput STATUSTEMPERR;
        Crestron.Logos.SplusObjects.DigitalOutput STATUSBRIGHTERR;
        Crestron.Logos.SplusObjects.DigitalOutput STATUSNOSYNCERR;
        Crestron.Logos.SplusObjects.DigitalOutput STATUSFANERR;
        Crestron.Logos.SplusObjects.AnalogOutput AUDIOVOLUMEFBK;
        Crestron.Logos.SplusObjects.AnalogOutput TEMPERATUREDEVFBK;
        Crestron.Logos.SplusObjects.AnalogOutput UPTIMEFBK;
        Crestron.Logos.SplusObjects.AnalogOutput LIFETIMEFBK;
        Crestron.Logos.SplusObjects.StringOutput NAMEFBK;
        Crestron.Logos.SplusObjects.StringOutput MANUFACTURERFBK;
        Crestron.Logos.SplusObjects.StringOutput TYPEFBK;
        Crestron.Logos.SplusObjects.StringOutput SERIALFBK;
        Crestron.Logos.SplusObjects.StringOutput HOSTFBK;
        Crestron.Logos.SplusObjects.StringOutput VERSIONFBK;
        Crestron.Logos.SplusObjects.StringOutput INFOFBK;
        SplusTcpClient TCPCLIENT;
        StringParameter TCPIPADDRESS;
        UShortParameter DISPLAYID;
        UShortParameter OPTIMISTICFEEDBACK;
        ushort TCPPORT = 0;
        ushort REFRESHSTATUSCTR = 0;
        CrestronString TCPRESPONSEOVF;
        CrestronString PERSISTANTFILE;
        uint UPTIMESECONDS = 0;
        uint LIFETIMEINTERNAL = 0;
        private CrestronString PRINTALLCHARS (  SplusExecutionContext __context__, CrestronString IN ) 
            { 
            CrestronString CHARS;
            CHARS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 1, this );
            
            ushort I = 0;
            ushort TMPBYTE = 0;
            ushort LASTCONTROLCHAR = 0;
            
            
            __context__.SourceCodeLine = 281;
            Functions.ResizeString (  ref CHARS , ((Functions.Length( IN ) * 5) + 10), null ) ; 
            __context__.SourceCodeLine = 282;
            CHARS  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 283;
            LASTCONTROLCHAR = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 285;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( IN ); 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 287;
                TMPBYTE = (ushort) ( Byte( IN , (int)( I ) ) ) ; 
                __context__.SourceCodeLine = 288;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( TMPBYTE <= 31 ) ) || Functions.TestForTrue ( Functions.BoolToInt ( TMPBYTE >= 127 ) )) ) ) || Functions.TestForTrue ( 1 )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 289;
                    MakeString ( CHARS , "{0} {1:X2}h", CHARS , Byte( IN , (int)( I ) )) ; 
                    __context__.SourceCodeLine = 290;
                    LASTCONTROLCHAR = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 292;
                    if ( Functions.TestForTrue  ( ( LASTCONTROLCHAR)  ) ) 
                        {
                        __context__.SourceCodeLine = 293;
                        MakeString ( CHARS , "{0} ", CHARS ) ; 
                        }
                    
                    __context__.SourceCodeLine = 294;
                    MakeString ( CHARS , "{0}{1}", CHARS , Functions.Chr ( Byte( IN , (int)( I ) ) ) ) ; 
                    __context__.SourceCodeLine = 295;
                    LASTCONTROLCHAR = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 285;
                } 
            
            __context__.SourceCodeLine = 299;
            return ( CHARS ) ; 
            
            }
            
        private void PRINTDEBUGSTRINT (  SplusExecutionContext __context__, CrestronString FUN , CrestronString MSG , short VAL ) 
            { 
            
            __context__.SourceCodeLine = 304;
            Trace( "SAMSUNG: {0}: {1} >{2:d}<\r\n", FUN , MSG , (short)VAL) ; 
            
            }
            
        private ushort BOUNDVALUE (  SplusExecutionContext __context__, ushort VAL , ushort LO , ushort UP ) 
            { 
            
            __context__.SourceCodeLine = 309;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( VAL > UP ))  ) ) 
                {
                __context__.SourceCodeLine = 310;
                return (ushort)( UP) ; 
                }
            
            __context__.SourceCodeLine = 312;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( VAL < LO ))  ) ) 
                {
                __context__.SourceCodeLine = 313;
                return (ushort)( LO) ; 
                }
            
            __context__.SourceCodeLine = 315;
            return (ushort)( VAL) ; 
            
            }
            
        private ushort HEXTOINT (  SplusExecutionContext __context__, CrestronString STR , ushort START ) 
            { 
            
            __context__.SourceCodeLine = 320;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( STR ) < (START + 1) ))  ) ) 
                { 
                __context__.SourceCodeLine = 322;
                return (ushort)( 0) ; 
                } 
            
            __context__.SourceCodeLine = 325;
            return (ushort)( (((Byte( STR , (int)( START ) ) - 48) * 16) + (Byte( STR , (int)( (START + 1) ) ) - 48))) ; 
            
            }
            
        private void WRITEUPTIME (  SplusExecutionContext __context__ ) 
            { 
            short NFILEHANDLE = 0;
            short NBYTESWRITTEN = 0;
            
            
            __context__.SourceCodeLine = 332;
            StartFileOperations ( ) ; 
            __context__.SourceCodeLine = 334;
            NFILEHANDLE = (short) ( FileOpenShared( PERSISTANTFILE ,(ushort) ((1 | 256) | 32768) ) ) ; 
            __context__.SourceCodeLine = 337;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NFILEHANDLE >= 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 339;
                NBYTESWRITTEN = (short) ( WriteLongInteger( (short)( NFILEHANDLE ) , (uint)( UPTIMESECONDS ) ) ) ; 
                __context__.SourceCodeLine = 342;
                FileClose (  (short) ( NFILEHANDLE ) ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 344;
                PRINTDEBUGSTRINT (  __context__ , "writeUptime", "failed", (short)( NFILEHANDLE )) ; 
                } 
            
            __context__.SourceCodeLine = 347;
            EndFileOperations ( ) ; 
            
            }
            
        private void READUPTIME (  SplusExecutionContext __context__ ) 
            { 
            short NFILEHANDLE = 0;
            short NBYTESREAD = 0;
            
            
            __context__.SourceCodeLine = 354;
            StartFileOperations ( ) ; 
            __context__.SourceCodeLine = 356;
            NFILEHANDLE = (short) ( FileOpenShared( PERSISTANTFILE ,(ushort) ((256 | 0) | 32768) ) ) ; 
            __context__.SourceCodeLine = 359;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NFILEHANDLE >= 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 361;
                NBYTESREAD = (short) ( ReadLongInteger( (short)( NFILEHANDLE ) , ref UPTIMESECONDS ) ) ; 
                __context__.SourceCodeLine = 364;
                FileClose (  (short) ( NFILEHANDLE ) ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 366;
                PRINTDEBUGSTRINT (  __context__ , "readUptime", "failed", (short)( NFILEHANDLE )) ; 
                } 
            
            __context__.SourceCodeLine = 369;
            EndFileOperations ( ) ; 
            
            }
            
        private short TCPCONNECT (  SplusExecutionContext __context__, CrestronString HOST ) 
            { 
            short STATUS = 0;
            
            ushort TIMEOUTCTR = 0;
            
            
            __context__.SourceCodeLine = 377;
            STATUS = (short) ( TCPCLIENT.SocketStatus ) ; 
            __context__.SourceCodeLine = 384;
            TIMEOUTCTR = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 385;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (STATUS != 2) ) && Functions.TestForTrue ( Functions.BoolToInt ( TIMEOUTCTR < 500 ) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 386;
                STATUS = (short) ( TCPCLIENT.SocketStatus ) ; 
                __context__.SourceCodeLine = 387;
                TIMEOUTCTR = (ushort) ( (TIMEOUTCTR + 1) ) ; 
                __context__.SourceCodeLine = 388;
                Functions.Delay (  (int) ( 1 ) ) ; 
                __context__.SourceCodeLine = 385;
                } 
            
            __context__.SourceCodeLine = 391;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (STATUS != 2))  ) ) 
                {
                __context__.SourceCodeLine = 392;
                Trace( "SAMSUNG: tcpConnect: SocketStatus: {0:d}\r\n", (short)STATUS) ; 
                }
            
            __context__.SourceCodeLine = 394;
            return (short)( STATUS) ; 
            
            }
            
        private short TCPDISCONNECT (  SplusExecutionContext __context__ ) 
            { 
            short STATUS = 0;
            
            
            __context__.SourceCodeLine = 400;
            STATUS = (short) ( Functions.SocketDisconnectClient( TCPCLIENT ) ) ; 
            __context__.SourceCodeLine = 401;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUS < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 402;
                Trace( "SAMSUNG: tcpDisConnect: Error disConnecting socket to address {0} on port {1:d}\r\n", TCPIPADDRESS , (short)TCPPORT) ; 
                }
            
            __context__.SourceCodeLine = 406;
            return (short)( STATUS) ; 
            
            }
            
        private short WRITECMD (  SplusExecutionContext __context__, ushort TYPE , CrestronString MSG ) 
            { 
            short STATUS = 0;
            
            CrestronString TMPCMD;
            CrestronString HEADER;
            TMPCMD  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 1, this );
            HEADER  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 7, this );
            
            ushort CHECK = 0;
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 415;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (CONNECT  .Value != 1))  ) ) 
                { 
                __context__.SourceCodeLine = 416;
                Trace( "SAMSUNG: writeCmd: Connect not enabled\r\n") ; 
                __context__.SourceCodeLine = 417;
                return (short)( Functions.ToSignedInteger( -( 1 ) )) ; 
                } 
            
            __context__.SourceCodeLine = 420;
            MakeString ( HEADER , "{0}{1}{2}{3}{4}{5:X}{6:X}", Functions.Chr ( 1 ) , Functions.Chr ( 48 ) , Functions.Chr ( DISPLAYID  .Value ) , Functions.Chr ( 48 ) , Functions.Chr ( TYPE ) , ((Functions.Length( MSG ) >> 4) & 15), (Functions.Length( MSG ) & 15)) ; 
            __context__.SourceCodeLine = 423;
            Functions.ResizeString (  ref TMPCMD , ((Functions.Length( HEADER ) + Functions.Length( MSG )) + 4), null ) ; 
            __context__.SourceCodeLine = 424;
            MakeString ( TMPCMD , "{0}{1}", HEADER , MSG ) ; 
            __context__.SourceCodeLine = 427;
            CHECK = (ushort) ( Byte( TMPCMD , (int)( 2 ) ) ) ; 
            __context__.SourceCodeLine = 428;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 3 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( TMPCMD ); 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                {
                __context__.SourceCodeLine = 429;
                CHECK = (ushort) ( (CHECK ^ Byte( TMPCMD , (int)( I ) )) ) ; 
                __context__.SourceCodeLine = 428;
                }
            
            __context__.SourceCodeLine = 431;
            MakeString ( TMPCMD , "{0}{1}{2}", TMPCMD , Functions.Chr ( (CHECK & 255) ) , Functions.Chr ( 13 ) ) ; 
            __context__.SourceCodeLine = 433;
            TCPCONNECT (  __context__ , TCPIPADDRESS ) ; 
            __context__.SourceCodeLine = 435;
            STATUS = (short) ( Functions.SocketSend( TCPCLIENT , TMPCMD ) ) ; 
            __context__.SourceCodeLine = 436;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUS < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 437;
                Trace( "SAMSUNG: writeCmd: Error transmitting '{0}' to tcpClient, status: {1:d}\r\n", PRINTALLCHARS (  __context__ , TMPCMD) , (short)STATUS) ; 
                }
            
            __context__.SourceCodeLine = 443;
            return (short)( STATUS) ; 
            
            }
            
        private short WRITECMDLEN0 (  SplusExecutionContext __context__, ushort CMD , ushort ID ) 
            { 
            short STATUS = 0;
            
            CrestronString TMPCMD;
            TMPCMD  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 12, this );
            
            ushort CHECK = 0;
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 452;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (CONNECT  .Value != 1))  ) ) 
                { 
                __context__.SourceCodeLine = 453;
                Trace( "SAMSUNG: writeCmdLen1: Connect not enabled\r\n") ; 
                __context__.SourceCodeLine = 454;
                return (short)( Functions.ToSignedInteger( -( 1 ) )) ; 
                } 
            
            __context__.SourceCodeLine = 457;
            CHECK = (ushort) ( (CMD + ID) ) ; 
            __context__.SourceCodeLine = 460;
            MakeString ( TMPCMD , "{0}{1}{2}{3}{4}", Functions.Chr ( 170 ) , Functions.Chr ( CMD ) , Functions.Chr ( ID ) , Functions.Chr ( 0 ) , Functions.Chr ( (CHECK & 255) ) ) ; 
            __context__.SourceCodeLine = 462;
            TCPCONNECT (  __context__ , TCPIPADDRESS ) ; 
            __context__.SourceCodeLine = 464;
            STATUS = (short) ( Functions.SocketSend( TCPCLIENT , TMPCMD ) ) ; 
            __context__.SourceCodeLine = 465;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUS < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 466;
                Trace( "SAMSUNG: writeCmd: Error transmitting '{0}' to tcpClient, status: {1:d}\r\n", PRINTALLCHARS (  __context__ , TMPCMD) , (short)STATUS) ; 
                }
            
            __context__.SourceCodeLine = 472;
            return (short)( STATUS) ; 
            
            }
            
        private short WRITECMDLEN1 (  SplusExecutionContext __context__, ushort CMD , ushort ID , ushort DATA ) 
            { 
            short STATUS = 0;
            
            CrestronString TMPCMD;
            TMPCMD  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 12, this );
            
            ushort CHECK = 0;
            ushort DATALEN = 0;
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 481;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (CONNECT  .Value != 1))  ) ) 
                { 
                __context__.SourceCodeLine = 482;
                Trace( "SAMSUNG: writeCmdLen1: Connect not enabled\r\n") ; 
                __context__.SourceCodeLine = 483;
                return (short)( Functions.ToSignedInteger( -( 1 ) )) ; 
                } 
            
            __context__.SourceCodeLine = 486;
            DATALEN = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 488;
            CHECK = (ushort) ( (((CMD + ID) + DATALEN) + DATA) ) ; 
            __context__.SourceCodeLine = 491;
            MakeString ( TMPCMD , "{0}{1}{2}{3}{4}{5}", Functions.Chr ( 170 ) , Functions.Chr ( CMD ) , Functions.Chr ( ID ) , Functions.Chr ( DATALEN ) , Functions.Chr ( DATA ) , Functions.Chr ( (CHECK & 255) ) ) ; 
            __context__.SourceCodeLine = 493;
            TCPCONNECT (  __context__ , TCPIPADDRESS ) ; 
            __context__.SourceCodeLine = 495;
            STATUS = (short) ( Functions.SocketSend( TCPCLIENT , TMPCMD ) ) ; 
            __context__.SourceCodeLine = 496;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUS < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 497;
                Trace( "SAMSUNG: writeCmd: Error transmitting '{0}' to tcpClient, status: {1:d}\r\n", PRINTALLCHARS (  __context__ , TMPCMD) , (short)STATUS) ; 
                }
            
            __context__.SourceCodeLine = 503;
            return (short)( STATUS) ; 
            
            }
            
        private void DISPLAYGETPOWERSTATE (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 508;
            WRITECMDLEN0 (  __context__ , (ushort)( 17 ), (ushort)( DISPLAYID  .Value )) ; 
            
            }
            
        private void DISPLAYSETPOWER (  SplusExecutionContext __context__, ushort STATE ) 
            { 
            ushort DISPLAYSETPOWERVALUE = 0;
            
            
            __context__.SourceCodeLine = 515;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATE > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 516;
                DISPLAYSETPOWERVALUE = (ushort) ( 1 ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 518;
                DISPLAYSETPOWERVALUE = (ushort) ( 0 ) ; 
                } 
            
            __context__.SourceCodeLine = 521;
            WRITECMDLEN1 (  __context__ , (ushort)( 17 ), (ushort)( DISPLAYID  .Value ), (ushort)( DISPLAYSETPOWERVALUE )) ; 
            __context__.SourceCodeLine = 522;
            Functions.Delay (  (int) ( 200 ) ) ; 
            __context__.SourceCodeLine = 523;
            WRITECMDLEN1 (  __context__ , (ushort)( 17 ), (ushort)( DISPLAYID  .Value ), (ushort)( DISPLAYSETPOWERVALUE )) ; 
            __context__.SourceCodeLine = 524;
            Functions.Delay (  (int) ( 200 ) ) ; 
            __context__.SourceCodeLine = 525;
            WRITECMDLEN1 (  __context__ , (ushort)( 17 ), (ushort)( DISPLAYID  .Value ), (ushort)( DISPLAYSETPOWERVALUE )) ; 
            
            }
            
        private void DISPLAYSETAUDIOVOLUME (  SplusExecutionContext __context__, ushort VAL ) 
            { 
            
            __context__.SourceCodeLine = 530;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( VAL > 100 ))  ) ) 
                {
                __context__.SourceCodeLine = 531;
                VAL = (ushort) ( 100 ) ; 
                }
            
            __context__.SourceCodeLine = 533;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( VAL < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 534;
                VAL = (ushort) ( 0 ) ; 
                }
            
            __context__.SourceCodeLine = 536;
            WRITECMDLEN1 (  __context__ , (ushort)( 18 ), (ushort)( DISPLAYID  .Value ), (ushort)( VAL )) ; 
            
            }
            
        private void DISPLAYGETINPUTSTATE (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 541;
            WRITECMDLEN0 (  __context__ , (ushort)( 20 ), (ushort)( DISPLAYID  .Value )) ; 
            
            }
            
        private void DISPLAYSETINPUT (  SplusExecutionContext __context__, ushort IN ) 
            { 
            
            __context__.SourceCodeLine = 546;
            WRITECMDLEN1 (  __context__ , (ushort)( 20 ), (ushort)( DISPLAYID  .Value ), (ushort)( (IN & 255) )) ; 
            __context__.SourceCodeLine = 548;
            if ( Functions.TestForTrue  ( ( OPTIMISTICFEEDBACK  .Value)  ) ) 
                { 
                __context__.SourceCodeLine = 568;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (IN == 37))  ) ) 
                    { 
                    __context__.SourceCodeLine = 569;
                    INPUTDP1FBK  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 571;
                    INPUTDP1FBK  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 574;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (IN == 33))  ) ) 
                    { 
                    __context__.SourceCodeLine = 575;
                    INPUTHDMI1FBK  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 577;
                    INPUTHDMI1FBK  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 580;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (IN == 24) ) || Functions.TestForTrue ( Functions.BoolToInt (IN == 31) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 581;
                    INPUTDVIFBK  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 583;
                    INPUTDVIFBK  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 586;
                Functions.ProcessLogic ( ) ; 
                } 
            
            
            }
            
        private void DISPLAYSETAUDIOMUTE (  SplusExecutionContext __context__, ushort STATE ) 
            { 
            
            __context__.SourceCodeLine = 592;
            WRITECMDLEN1 (  __context__ , (ushort)( 19 ), (ushort)( DISPLAYID  .Value ), (ushort)( (STATE & 255) )) ; 
            
            }
            
        private void DISPLAYSETVIDEOMUTE (  SplusExecutionContext __context__, ushort STATE ) 
            { 
            
            __context__.SourceCodeLine = 597;
            WRITECMDLEN1 (  __context__ , (ushort)( 132 ), (ushort)( DISPLAYID  .Value ), (ushort)( (STATE & 255) )) ; 
            
            }
            
        private void DISPLAYGETSERIALNUMBER (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 602;
            WRITECMDLEN0 (  __context__ , (ushort)( 11 ), (ushort)( DISPLAYID  .Value )) ; 
            
            }
            
        private void DISPLAYGETDISPLAYSTATUS (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 607;
            WRITECMDLEN0 (  __context__ , (ushort)( 13 ), (ushort)( DISPLAYID  .Value )) ; 
            
            }
            
        private void DISPLAYGETSOFTWAREVERSION (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 612;
            WRITECMDLEN0 (  __context__ , (ushort)( 14 ), (ushort)( DISPLAYID  .Value )) ; 
            
            }
            
        private void DISPLAYGETSOFTWAREMODEL (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 617;
            WRITECMDLEN0 (  __context__ , (ushort)( 138 ), (ushort)( DISPLAYID  .Value )) ; 
            
            }
            
        private void DISPLAYGETSTATUSTEMPERATURELED (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 623;
            WRITECMDLEN1 (  __context__ , (ushort)( 0 ), (ushort)( DISPLAYID  .Value ), (ushort)( 2 )) ; 
            
            }
            
        private void DISPLAYPARSERESPONSE (  SplusExecutionContext __context__, CrestronString RES ) 
            { 
            ushort POS = 0;
            
            ushort TMP = 0;
            
            CrestronString TMPSTR;
            TMPSTR  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 32, this );
            
            
            __context__.SourceCodeLine = 633;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u00AA" , RES ) < 1 ))  ) ) 
                { 
                __context__.SourceCodeLine = 634;
                Trace( "SAMSUNG: ->displayParseResponse: Wrong response: {0} ({1:d})\r\n", PRINTALLCHARS (  __context__ , RES) , (short)Functions.Length( RES )) ; 
                __context__.SourceCodeLine = 635;
                return ; 
                } 
            
            __context__.SourceCodeLine = 638;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0003\u0041\u0011" , RES ) > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 640;
                if ( Functions.TestForTrue  ( ( Functions.Find( "\u0011\u0000" , RES ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 641;
                    POWERFBK  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 644;
                if ( Functions.TestForTrue  ( ( Functions.Find( "\u0011\u0001" , RES ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 645;
                    POWERFBK  .Value = (ushort) ( 1 ) ; 
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 652;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0003\u0041\u0012" , RES ) > 0 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 653;
                    POS = (ushort) ( (Functions.Find( "\u0041\u0012" , RES ) + 2) ) ; 
                    __context__.SourceCodeLine = 655;
                    AUDIOVOLUMEFBK  .Value = (ushort) ( (Byte( RES , (int)( POS ) ) * 655) ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 657;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0003\u0041\u0014" , RES ) > 0 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 658;
                        POS = (ushort) ( (Functions.Find( "\u0041\u0014" , RES ) + 2) ) ; 
                        __context__.SourceCodeLine = 660;
                        Trace( "SAMSUNG: ->displayParseResponse: Input: {0} ({1:d}) {2:d}\r\n", PRINTALLCHARS (  __context__ , RES) , (short)Functions.Length( RES ), (short)Byte( RES , (int)( POS ) )) ; 
                        __context__.SourceCodeLine = 682;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (Byte( RES , (int)( POS ) ) == 37))  ) ) 
                            { 
                            __context__.SourceCodeLine = 683;
                            INPUTDP1FBK  .Value = (ushort) ( 1 ) ; 
                            } 
                        
                        else 
                            { 
                            __context__.SourceCodeLine = 685;
                            INPUTDP1FBK  .Value = (ushort) ( 0 ) ; 
                            } 
                        
                        __context__.SourceCodeLine = 688;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (Byte( RES , (int)( POS ) ) == 33))  ) ) 
                            { 
                            __context__.SourceCodeLine = 689;
                            INPUTHDMI1FBK  .Value = (ushort) ( 1 ) ; 
                            } 
                        
                        else 
                            { 
                            __context__.SourceCodeLine = 691;
                            INPUTHDMI1FBK  .Value = (ushort) ( 0 ) ; 
                            } 
                        
                        __context__.SourceCodeLine = 694;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (Byte( RES , (int)( POS ) ) == 24) ) || Functions.TestForTrue ( Functions.BoolToInt (Byte( RES , (int)( POS ) ) == 31) )) ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 695;
                            INPUTDVIFBK  .Value = (ushort) ( 1 ) ; 
                            } 
                        
                        else 
                            { 
                            __context__.SourceCodeLine = 697;
                            INPUTDVIFBK  .Value = (ushort) ( 0 ) ; 
                            } 
                        
                        __context__.SourceCodeLine = 700;
                        Functions.ProcessLogic ( ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 702;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0003\u004e\u0014" , RES ) > 0 ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 704;
                            CreateWait ( "__SPLS_TMPVAR__WAITLABEL_1__" , 15 , __SPLS_TMPVAR__WAITLABEL_1___Callback ) ;
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 708;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0003\u0041\u0013" , RES ) > 0 ))  ) ) 
                                { 
                                __context__.SourceCodeLine = 710;
                                if ( Functions.TestForTrue  ( ( Functions.Find( "\u0013\u0000" , RES ))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 711;
                                    AUDIOMUTEFBK  .Value = (ushort) ( 0 ) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 714;
                                if ( Functions.TestForTrue  ( ( Functions.Find( "\u0013\u0001" , RES ))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 715;
                                    AUDIOMUTEFBK  .Value = (ushort) ( 1 ) ; 
                                    } 
                                
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 719;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0004\u0041\u0050\u0020" , RES ) > 0 ))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 721;
                                    POS = (ushort) ( (Functions.Find( "\u0050\u0020" , RES ) + 2) ) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 724;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0008\u0041\u000D" , RES ) > 0 ))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 726;
                                        POS = (ushort) ( (Functions.Find( "\u0041\u000D" , RES ) + 1) ) ; 
                                        __context__.SourceCodeLine = 729;
                                        STATUSLAMPERR  .Value = (ushort) ( Byte( RES , (int)( (POS + 1) ) ) ) ; 
                                        __context__.SourceCodeLine = 732;
                                        STATUSTEMPERR  .Value = (ushort) ( Byte( RES , (int)( (POS + 2) ) ) ) ; 
                                        __context__.SourceCodeLine = 735;
                                        TMP = (ushort) ( Byte( RES , (int)( (POS + 3) ) ) ) ; 
                                        __context__.SourceCodeLine = 736;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( TMP > 1 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 736;
                                            STATUSBRIGHTERR  .Value = (ushort) ( 0 ) ; 
                                            }
                                        
                                        __context__.SourceCodeLine = 737;
                                        STATUSBRIGHTERR  .Value = (ushort) ( TMP ) ; 
                                        __context__.SourceCodeLine = 740;
                                        TMP = (ushort) ( Byte( RES , (int)( (POS + 4) ) ) ) ; 
                                        __context__.SourceCodeLine = 741;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( TMP > 1 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 741;
                                            TMP = (ushort) ( 1 ) ; 
                                            }
                                        
                                        __context__.SourceCodeLine = 742;
                                        STATUSNOSYNCERR  .Value = (ushort) ( TMP ) ; 
                                        __context__.SourceCodeLine = 745;
                                        TEMPERATUREDEVFBK  .Value = (ushort) ( (Byte( RES , (int)( (POS + 5) ) ) * 10) ) ; 
                                        __context__.SourceCodeLine = 748;
                                        TMP = (ushort) ( Byte( RES , (int)( (POS + 6) ) ) ) ; 
                                        __context__.SourceCodeLine = 749;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( TMP > 1 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 749;
                                            TMP = (ushort) ( 0 ) ; 
                                            }
                                        
                                        __context__.SourceCodeLine = 750;
                                        STATUSFANERR  .Value = (ushort) ( TMP ) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 752;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "0x14\u0041\u000B" , RES ) > 0 ))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 754;
                                            POS = (ushort) ( (Functions.Find( "\u0041\u000B" , RES ) + 2) ) ; 
                                            __context__.SourceCodeLine = 755;
                                            SERIALFBK  .UpdateValue ( Functions.Mid ( RES ,  (int) ( POS ) ,  (int) ( ((Functions.Length( RES ) - POS) - 1) ) )  ) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 758;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0041\u000E" , RES ) > 0 ))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 760;
                                                POS = (ushort) ( (Functions.Find( "\u0041\u000E" , RES ) + 2) ) ; 
                                                __context__.SourceCodeLine = 761;
                                                VERSIONFBK  .UpdateValue ( Functions.Mid ( RES ,  (int) ( POS ) ,  (int) ( ((Functions.Length( RES ) - POS) - 1) ) )  ) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 777;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u0041\u008A" , RES ) > 0 ))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 779;
                                                    POS = (ushort) ( (Functions.Find( "\u0041\u008A" , RES ) + 2) ) ; 
                                                    __context__.SourceCodeLine = 780;
                                                    MANUFACTURERFBK  .UpdateValue ( "Samsung"  ) ; 
                                                    __context__.SourceCodeLine = 781;
                                                    TYPEFBK  .UpdateValue ( Functions.Mid ( RES ,  (int) ( POS ) ,  (int) ( ((Functions.Length( RES ) - POS) - 1) ) )  ) ; 
                                                    } 
                                                
                                                else 
                                                    {
                                                    __context__.SourceCodeLine = 783;
                                                    if ( Functions.TestForTrue  ( ( 0)  ) ) 
                                                        { 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        } 
                                                    
                                                    }
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    }
                
                }
            
            __context__.SourceCodeLine = 797;
            Functions.ProcessLogic ( ) ; 
            __context__.SourceCodeLine = 799;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( STATUSLAMPERR  .Value ) || Functions.TestForTrue ( STATUSTEMPERR  .Value )) ) ) || Functions.TestForTrue ( STATUSNOSYNCERR  .Value )) ) ) || Functions.TestForTrue ( STATUSFANERR  .Value )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 801;
                WARNFBK  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 802;
                ERRORFBK  .Value = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 804;
                TMPSTR  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 805;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUSLAMPERR  .Value > 0 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 805;
                    MakeString ( TMPSTR , "{0}{1}", TMPSTR , "Screen Error! " ) ; 
                    }
                
                __context__.SourceCodeLine = 806;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUSTEMPERR  .Value > 0 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 806;
                    MakeString ( TMPSTR , "{0}{1}", TMPSTR , "Temperature Error! " ) ; 
                    }
                
                __context__.SourceCodeLine = 807;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUSNOSYNCERR  .Value > 0 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 807;
                    MakeString ( TMPSTR , "{0}{1}", TMPSTR , "No sync! " ) ; 
                    }
                
                __context__.SourceCodeLine = 808;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUSFANERR  .Value > 0 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 808;
                    MakeString ( TMPSTR , "{0}{1}", TMPSTR , "Fan Error! " ) ; 
                    }
                
                __context__.SourceCodeLine = 810;
                INFOFBK  .UpdateValue ( TMPSTR  ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 812;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( LIFETIMEFBK  .Value > 800 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 814;
                    WARNFBK  .Value = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 815;
                    ERRORFBK  .Value = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 816;
                    INFOFBK  .UpdateValue ( "Lifetime limit > 80%"  ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 818;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( LIFETIMEFBK  .Value > 990 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 820;
                        WARNFBK  .Value = (ushort) ( 1 ) ; 
                        __context__.SourceCodeLine = 821;
                        ERRORFBK  .Value = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 822;
                        INFOFBK  .UpdateValue ( "Lifetime limit > 99%"  ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 824;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUSBRIGHTERR  .Value > 0 ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 826;
                            WARNFBK  .Value = (ushort) ( 1 ) ; 
                            __context__.SourceCodeLine = 827;
                            ERRORFBK  .Value = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 828;
                            INFOFBK  .UpdateValue ( "Brightness Sensor Error!"  ) ; 
                            } 
                        
                        else 
                            { 
                            __context__.SourceCodeLine = 832;
                            WARNFBK  .Value = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 833;
                            ERRORFBK  .Value = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 834;
                            INFOFBK  .UpdateValue ( "Working"  ) ; 
                            } 
                        
                        }
                    
                    }
                
                }
            
            
            }
            
        public void __SPLS_TMPVAR__WAITLABEL_1___CallbackFn( object stateInfo )
        {
        
            try
            {
                Wait __LocalWait__ = (Wait)stateInfo;
                SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
                __LocalWait__.RemoveFromList();
                
            
            __context__.SourceCodeLine = 705;
            DISPLAYGETINPUTSTATE (  __context__  ) ; 
            
        
        
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler(); }
            
        }
        
    object CONNECT_OnPush_0 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 845;
            Trace( "SAMSUNG: tcpConnect: Trying to Connect to {0}...\r\n", TCPIPADDRESS ) ; 
            __context__.SourceCodeLine = 847;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "10.124.237." , TCPIPADDRESS  ) <= 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 848;
                return  this ; 
                }
            
            __context__.SourceCodeLine = 850;
            Functions.SocketConnectClient ( TCPCLIENT , TCPIPADDRESS ,  (ushort) ( TCPPORT ) ,  (ushort) ( 1 ) ) ; 
            __context__.SourceCodeLine = 852;
            TCPCONNECT (  __context__ , TCPIPADDRESS ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object CONNECT_OnRelease_1 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 857;
        TCPDISCONNECT (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GETPOWERSTATE_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 862;
        DISPLAYGETPOWERSTATE (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POWERON_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 867;
        DISPLAYSETPOWER (  __context__ , (ushort)( 1 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POWEROFF_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 872;
        DISPLAYSETPOWER (  __context__ , (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GETINPUTSTATE_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 877;
        DISPLAYGETINPUTSTATE (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTDP1_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 882;
        DISPLAYSETINPUT (  __context__ , (ushort)( 37 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTDP2_OnPush_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 887;
        DISPLAYSETINPUT (  __context__ , (ushort)( 38 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTDP3_OnPush_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 892;
        DISPLAYSETINPUT (  __context__ , (ushort)( 39 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTHDMI1_OnPush_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 897;
        DISPLAYSETINPUT (  __context__ , (ushort)( 33 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTHDMI2_OnPush_10 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 902;
        DISPLAYSETINPUT (  __context__ , (ushort)( 35 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTHDMI3_OnPush_11 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 907;
        DISPLAYSETINPUT (  __context__ , (ushort)( 49 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object INPUTDVI_OnPush_12 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 912;
        DISPLAYSETINPUT (  __context__ , (ushort)( 24 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object AUDIOMUTEONOFF_OnPush_13 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 917;
        DISPLAYSETAUDIOMUTE (  __context__ , (ushort)( 1 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object AUDIOMUTEONOFF_OnRelease_14 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 922;
        DISPLAYSETAUDIOMUTE (  __context__ , (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object AUDIOVOLUME_OnChange_15 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort SCALED = 0;
        
        
        __context__.SourceCodeLine = 938;
        SCALED = (ushort) ( (AUDIOVOLUME  .UshortValue / 655) ) ; 
        __context__.SourceCodeLine = 940;
        DISPLAYSETAUDIOVOLUME (  __context__ , (ushort)( SCALED )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object TCPCLIENT_OnSocketConnect_16 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        short STATUS = 0;
        
        
        __context__.SourceCodeLine = 948;
        STATUS = (short) ( TCPCLIENT.SocketStatus ) ; 
        __context__.SourceCodeLine = 949;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( STATUS < 0 ))  ) ) 
            { 
            __context__.SourceCodeLine = 950;
            Trace( "SAMSUNG: tcpClient error Connecting to {0}. Status: {1:d}\r\n", TCPIPADDRESS , (short)STATUS) ; 
            } 
        
        else 
            { 
            __context__.SourceCodeLine = 952;
            Trace( "SAMSUNG: tcpClient Connected to {0} on port {1:d}\r\n", TCPIPADDRESS , (short)TCPPORT) ; 
            __context__.SourceCodeLine = 954;
            CONNECTFBK  .Value = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 955;
            Functions.ProcessLogic ( ) ; 
            __context__.SourceCodeLine = 963;
            DISPLAYGETSERIALNUMBER (  __context__  ) ; 
            __context__.SourceCodeLine = 964;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 965;
            DISPLAYGETSOFTWAREVERSION (  __context__  ) ; 
            __context__.SourceCodeLine = 966;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 967;
            DISPLAYGETSOFTWAREMODEL (  __context__  ) ; 
            __context__.SourceCodeLine = 968;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 970;
            REFRESHSTATUSCTR = (ushort) ( (300 + 1) ) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object TCPCLIENT_OnSocketDisconnect_17 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 976;
        CONNECTFBK  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 978;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( TCPCLIENT.SocketStatus < 0 ))  ) ) 
            {
            __context__.SourceCodeLine = 979;
            Trace( "SAMSUNG: Socket disConnected remotely\r\n") ; 
            }
        
        else 
            {
            __context__.SourceCodeLine = 981;
            Trace( "SAMSUNG: Local disConnect complete\r\n") ; 
            }
        
        __context__.SourceCodeLine = 983;
        WARNFBK  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 984;
        ERRORFBK  .Value = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 985;
        INFOFBK  .UpdateValue ( "Network Connection lost"  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object TCPCLIENT_OnSocketStatus_18 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        
        __context__.SourceCodeLine = 991;
        Trace( "SAMSUNG: tcpClient.SocketStatus: {0:d}\r\n", (short)TCPCLIENT.SocketStatus) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

object TCPCLIENT_OnSocketReceive_19 ( Object __Info__ )

    { 
    SocketEventInfo __SocketInfo__ = (SocketEventInfo)__Info__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SocketInfo__);
        CrestronString TCPRESPONSE;
        TCPRESPONSE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 128, this );
        
        
        __context__.SourceCodeLine = 999;
        TCPRESPONSE  .UpdateValue ( TCPCLIENT .  SocketRxBuf  ) ; 
        __context__.SourceCodeLine = 1000;
        Functions.ClearBuffer ( TCPCLIENT .  SocketRxBuf ) ; 
        __context__.SourceCodeLine = 1005;
        DISPLAYPARSERESPONSE (  __context__ , Functions.Upper( TCPRESPONSE )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SocketInfo__ ); }
    return this;
    
}

public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 1020;
        TCPPORT = (ushort) ( 1515 ) ; 
        __context__.SourceCodeLine = 1022;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 1029;
        Trace( "=================================================\r\n") ; 
        __context__.SourceCodeLine = 1030;
        Trace( "SAMSUNG Signage Ctrl\r\n") ; 
        __context__.SourceCodeLine = 1031;
        Trace( "V:  2023-06-03 (Rev 4)\r\n") ; 
        __context__.SourceCodeLine = 1032;
        Trace( "(c) 2023 evekto GmbH, Erlangen, Germany\r\n") ; 
        __context__.SourceCodeLine = 1033;
        Trace( "==================================================") ; 
        __context__.SourceCodeLine = 1035;
        MakeString ( PERSISTANTFILE , "\\User\\cfg\\{0:d2}-samsungSig{1:d2}.uuid", (short)GetProgramNumber(), (short)DISPLAYID  .Value) ; 
        __context__.SourceCodeLine = 1036;
        READUPTIME (  __context__  ) ; 
        __context__.SourceCodeLine = 1038;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( UPTIMESECONDS < 360000 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1039;
            UPTIMESECONDS = (uint) ( 360000 ) ; 
            } 
        
        __context__.SourceCodeLine = 1042;
        HOSTFBK  .UpdateValue ( TCPIPADDRESS  ) ; 
        __context__.SourceCodeLine = 1044;
        REFRESHSTATUSCTR = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1046;
        while ( Functions.TestForTrue  ( ( 1)  ) ) 
            { 
            __context__.SourceCodeLine = 1048;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( CONNECT  .Value ) && Functions.TestForTrue ( Functions.BoolToInt (TCPCLIENT.SocketStatus == 2) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1050;
                
                    {
                    int __SPLS_TMPVAR__SWTCH_1__ = ((int)REFRESHSTATUSCTR);
                    
                        { 
                        if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 11) ) ) ) 
                            { 
                            __context__.SourceCodeLine = 1053;
                            DISPLAYGETPOWERSTATE (  __context__  ) ; 
                            } 
                        
                        else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 12) ) ) ) 
                            { 
                            __context__.SourceCodeLine = 1056;
                            DISPLAYGETINPUTSTATE (  __context__  ) ; 
                            } 
                        
                        else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 13) ) ) ) 
                            { 
                            __context__.SourceCodeLine = 1059;
                            DISPLAYGETDISPLAYSTATUS (  __context__  ) ; 
                            } 
                        
                        } 
                        
                    }
                    
                
                __context__.SourceCodeLine = 1063;
                REFRESHSTATUSCTR = (ushort) ( (REFRESHSTATUSCTR + 1) ) ; 
                __context__.SourceCodeLine = 1066;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( REFRESHSTATUSCTR > 300 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1068;
                    if ( Functions.TestForTrue  ( ( POWERFBK  .Value)  ) ) 
                        { 
                        __context__.SourceCodeLine = 1070;
                        UPTIMESECONDS = (uint) ( (UPTIMESECONDS + REFRESHSTATUSCTR) ) ; 
                        __context__.SourceCodeLine = 1071;
                        LIFETIMEINTERNAL = (uint) ( (((UPTIMESECONDS * 1) / 36) / 50000) ) ; 
                        __context__.SourceCodeLine = 1073;
                        UPTIMEFBK  .Value = (ushort) ( Functions.LowWord( (uint)( (UPTIMESECONDS / 3600) ) ) ) ; 
                        __context__.SourceCodeLine = 1074;
                        LIFETIMEFBK  .Value = (ushort) ( Functions.LowWord( (uint)( LIFETIMEINTERNAL ) ) ) ; 
                        __context__.SourceCodeLine = 1076;
                        WRITEUPTIME (  __context__  ) ; 
                        } 
                    
                    __context__.SourceCodeLine = 1080;
                    REFRESHSTATUSCTR = (ushort) ( 0 ) ; 
                    } 
                
                } 
            
            __context__.SourceCodeLine = 1084;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 1046;
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    TCPRESPONSEOVF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 128, this );
    PERSISTANTFILE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 64, this );
    TCPCLIENT  = new SplusTcpClient ( 128, this );
    
    CONNECT = new Crestron.Logos.SplusObjects.DigitalInput( CONNECT__DigitalInput__, this );
    m_DigitalInputList.Add( CONNECT__DigitalInput__, CONNECT );
    
    GETPOWERSTATE = new Crestron.Logos.SplusObjects.DigitalInput( GETPOWERSTATE__DigitalInput__, this );
    m_DigitalInputList.Add( GETPOWERSTATE__DigitalInput__, GETPOWERSTATE );
    
    POWERON = new Crestron.Logos.SplusObjects.DigitalInput( POWERON__DigitalInput__, this );
    m_DigitalInputList.Add( POWERON__DigitalInput__, POWERON );
    
    POWEROFF = new Crestron.Logos.SplusObjects.DigitalInput( POWEROFF__DigitalInput__, this );
    m_DigitalInputList.Add( POWEROFF__DigitalInput__, POWEROFF );
    
    GETINPUTSTATE = new Crestron.Logos.SplusObjects.DigitalInput( GETINPUTSTATE__DigitalInput__, this );
    m_DigitalInputList.Add( GETINPUTSTATE__DigitalInput__, GETINPUTSTATE );
    
    INPUTDP1 = new Crestron.Logos.SplusObjects.DigitalInput( INPUTDP1__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTDP1__DigitalInput__, INPUTDP1 );
    
    INPUTDP2 = new Crestron.Logos.SplusObjects.DigitalInput( INPUTDP2__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTDP2__DigitalInput__, INPUTDP2 );
    
    INPUTDP3 = new Crestron.Logos.SplusObjects.DigitalInput( INPUTDP3__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTDP3__DigitalInput__, INPUTDP3 );
    
    INPUTHDMI1 = new Crestron.Logos.SplusObjects.DigitalInput( INPUTHDMI1__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTHDMI1__DigitalInput__, INPUTHDMI1 );
    
    INPUTHDMI2 = new Crestron.Logos.SplusObjects.DigitalInput( INPUTHDMI2__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTHDMI2__DigitalInput__, INPUTHDMI2 );
    
    INPUTHDMI3 = new Crestron.Logos.SplusObjects.DigitalInput( INPUTHDMI3__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTHDMI3__DigitalInput__, INPUTHDMI3 );
    
    INPUTDVI = new Crestron.Logos.SplusObjects.DigitalInput( INPUTDVI__DigitalInput__, this );
    m_DigitalInputList.Add( INPUTDVI__DigitalInput__, INPUTDVI );
    
    GETAUDIOMUTESTATE = new Crestron.Logos.SplusObjects.DigitalInput( GETAUDIOMUTESTATE__DigitalInput__, this );
    m_DigitalInputList.Add( GETAUDIOMUTESTATE__DigitalInput__, GETAUDIOMUTESTATE );
    
    AUDIOMUTEONOFF = new Crestron.Logos.SplusObjects.DigitalInput( AUDIOMUTEONOFF__DigitalInput__, this );
    m_DigitalInputList.Add( AUDIOMUTEONOFF__DigitalInput__, AUDIOMUTEONOFF );
    
    AUDIOMUTEON = new Crestron.Logos.SplusObjects.DigitalInput( AUDIOMUTEON__DigitalInput__, this );
    m_DigitalInputList.Add( AUDIOMUTEON__DigitalInput__, AUDIOMUTEON );
    
    AUDIOMUTEOFF = new Crestron.Logos.SplusObjects.DigitalInput( AUDIOMUTEOFF__DigitalInput__, this );
    m_DigitalInputList.Add( AUDIOMUTEOFF__DigitalInput__, AUDIOMUTEOFF );
    
    GETVIDEOMUTESTATE = new Crestron.Logos.SplusObjects.DigitalInput( GETVIDEOMUTESTATE__DigitalInput__, this );
    m_DigitalInputList.Add( GETVIDEOMUTESTATE__DigitalInput__, GETVIDEOMUTESTATE );
    
    VIDEOMUTEONOFF = new Crestron.Logos.SplusObjects.DigitalInput( VIDEOMUTEONOFF__DigitalInput__, this );
    m_DigitalInputList.Add( VIDEOMUTEONOFF__DigitalInput__, VIDEOMUTEONOFF );
    
    VIDEOMUTEON = new Crestron.Logos.SplusObjects.DigitalInput( VIDEOMUTEON__DigitalInput__, this );
    m_DigitalInputList.Add( VIDEOMUTEON__DigitalInput__, VIDEOMUTEON );
    
    VIDEOMUTEOFF = new Crestron.Logos.SplusObjects.DigitalInput( VIDEOMUTEOFF__DigitalInput__, this );
    m_DigitalInputList.Add( VIDEOMUTEOFF__DigitalInput__, VIDEOMUTEOFF );
    
    GETSERIALNUMBER = new Crestron.Logos.SplusObjects.DigitalInput( GETSERIALNUMBER__DigitalInput__, this );
    m_DigitalInputList.Add( GETSERIALNUMBER__DigitalInput__, GETSERIALNUMBER );
    
    GETDISPLAYSTATUS = new Crestron.Logos.SplusObjects.DigitalInput( GETDISPLAYSTATUS__DigitalInput__, this );
    m_DigitalInputList.Add( GETDISPLAYSTATUS__DigitalInput__, GETDISPLAYSTATUS );
    
    GETSOFTWAREVERSION = new Crestron.Logos.SplusObjects.DigitalInput( GETSOFTWAREVERSION__DigitalInput__, this );
    m_DigitalInputList.Add( GETSOFTWAREVERSION__DigitalInput__, GETSOFTWAREVERSION );
    
    GETTEMPERATUREDEV = new Crestron.Logos.SplusObjects.DigitalInput( GETTEMPERATUREDEV__DigitalInput__, this );
    m_DigitalInputList.Add( GETTEMPERATUREDEV__DigitalInput__, GETTEMPERATUREDEV );
    
    GETTEMPERATUREEXE = new Crestron.Logos.SplusObjects.DigitalInput( GETTEMPERATUREEXE__DigitalInput__, this );
    m_DigitalInputList.Add( GETTEMPERATUREEXE__DigitalInput__, GETTEMPERATUREEXE );
    
    GETTEMPERATURELED = new Crestron.Logos.SplusObjects.DigitalInput( GETTEMPERATURELED__DigitalInput__, this );
    m_DigitalInputList.Add( GETTEMPERATURELED__DigitalInput__, GETTEMPERATURELED );
    
    CONNECTFBK = new Crestron.Logos.SplusObjects.DigitalOutput( CONNECTFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( CONNECTFBK__DigitalOutput__, CONNECTFBK );
    
    WARNFBK = new Crestron.Logos.SplusObjects.DigitalOutput( WARNFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( WARNFBK__DigitalOutput__, WARNFBK );
    
    ERRORFBK = new Crestron.Logos.SplusObjects.DigitalOutput( ERRORFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( ERRORFBK__DigitalOutput__, ERRORFBK );
    
    POWERFBK = new Crestron.Logos.SplusObjects.DigitalOutput( POWERFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( POWERFBK__DigitalOutput__, POWERFBK );
    
    INPUTDP1FBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTDP1FBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTDP1FBK__DigitalOutput__, INPUTDP1FBK );
    
    INPUTDP2FBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTDP2FBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTDP2FBK__DigitalOutput__, INPUTDP2FBK );
    
    INPUTDP3FBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTDP3FBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTDP3FBK__DigitalOutput__, INPUTDP3FBK );
    
    INPUTHDMI1FBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTHDMI1FBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTHDMI1FBK__DigitalOutput__, INPUTHDMI1FBK );
    
    INPUTHDMI2FBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTHDMI2FBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTHDMI2FBK__DigitalOutput__, INPUTHDMI2FBK );
    
    INPUTHDMI3FBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTHDMI3FBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTHDMI3FBK__DigitalOutput__, INPUTHDMI3FBK );
    
    INPUTDVIFBK = new Crestron.Logos.SplusObjects.DigitalOutput( INPUTDVIFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( INPUTDVIFBK__DigitalOutput__, INPUTDVIFBK );
    
    AUDIOMUTEFBK = new Crestron.Logos.SplusObjects.DigitalOutput( AUDIOMUTEFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( AUDIOMUTEFBK__DigitalOutput__, AUDIOMUTEFBK );
    
    VIDEOMUTEFBK = new Crestron.Logos.SplusObjects.DigitalOutput( VIDEOMUTEFBK__DigitalOutput__, this );
    m_DigitalOutputList.Add( VIDEOMUTEFBK__DigitalOutput__, VIDEOMUTEFBK );
    
    STATUSLAMPERR = new Crestron.Logos.SplusObjects.DigitalOutput( STATUSLAMPERR__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATUSLAMPERR__DigitalOutput__, STATUSLAMPERR );
    
    STATUSTEMPERR = new Crestron.Logos.SplusObjects.DigitalOutput( STATUSTEMPERR__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATUSTEMPERR__DigitalOutput__, STATUSTEMPERR );
    
    STATUSBRIGHTERR = new Crestron.Logos.SplusObjects.DigitalOutput( STATUSBRIGHTERR__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATUSBRIGHTERR__DigitalOutput__, STATUSBRIGHTERR );
    
    STATUSNOSYNCERR = new Crestron.Logos.SplusObjects.DigitalOutput( STATUSNOSYNCERR__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATUSNOSYNCERR__DigitalOutput__, STATUSNOSYNCERR );
    
    STATUSFANERR = new Crestron.Logos.SplusObjects.DigitalOutput( STATUSFANERR__DigitalOutput__, this );
    m_DigitalOutputList.Add( STATUSFANERR__DigitalOutput__, STATUSFANERR );
    
    AUDIOVOLUME = new Crestron.Logos.SplusObjects.AnalogInput( AUDIOVOLUME__AnalogSerialInput__, this );
    m_AnalogInputList.Add( AUDIOVOLUME__AnalogSerialInput__, AUDIOVOLUME );
    
    AUDIOVOLUMEFBK = new Crestron.Logos.SplusObjects.AnalogOutput( AUDIOVOLUMEFBK__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( AUDIOVOLUMEFBK__AnalogSerialOutput__, AUDIOVOLUMEFBK );
    
    TEMPERATUREDEVFBK = new Crestron.Logos.SplusObjects.AnalogOutput( TEMPERATUREDEVFBK__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( TEMPERATUREDEVFBK__AnalogSerialOutput__, TEMPERATUREDEVFBK );
    
    UPTIMEFBK = new Crestron.Logos.SplusObjects.AnalogOutput( UPTIMEFBK__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( UPTIMEFBK__AnalogSerialOutput__, UPTIMEFBK );
    
    LIFETIMEFBK = new Crestron.Logos.SplusObjects.AnalogOutput( LIFETIMEFBK__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIFETIMEFBK__AnalogSerialOutput__, LIFETIMEFBK );
    
    NAMEFBK = new Crestron.Logos.SplusObjects.StringOutput( NAMEFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( NAMEFBK__AnalogSerialOutput__, NAMEFBK );
    
    MANUFACTURERFBK = new Crestron.Logos.SplusObjects.StringOutput( MANUFACTURERFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( MANUFACTURERFBK__AnalogSerialOutput__, MANUFACTURERFBK );
    
    TYPEFBK = new Crestron.Logos.SplusObjects.StringOutput( TYPEFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( TYPEFBK__AnalogSerialOutput__, TYPEFBK );
    
    SERIALFBK = new Crestron.Logos.SplusObjects.StringOutput( SERIALFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( SERIALFBK__AnalogSerialOutput__, SERIALFBK );
    
    HOSTFBK = new Crestron.Logos.SplusObjects.StringOutput( HOSTFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( HOSTFBK__AnalogSerialOutput__, HOSTFBK );
    
    VERSIONFBK = new Crestron.Logos.SplusObjects.StringOutput( VERSIONFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( VERSIONFBK__AnalogSerialOutput__, VERSIONFBK );
    
    INFOFBK = new Crestron.Logos.SplusObjects.StringOutput( INFOFBK__AnalogSerialOutput__, this );
    m_StringOutputList.Add( INFOFBK__AnalogSerialOutput__, INFOFBK );
    
    DISPLAYID = new UShortParameter( DISPLAYID__Parameter__, this );
    m_ParameterList.Add( DISPLAYID__Parameter__, DISPLAYID );
    
    OPTIMISTICFEEDBACK = new UShortParameter( OPTIMISTICFEEDBACK__Parameter__, this );
    m_ParameterList.Add( OPTIMISTICFEEDBACK__Parameter__, OPTIMISTICFEEDBACK );
    
    TCPIPADDRESS = new StringParameter( TCPIPADDRESS__Parameter__, this );
    m_ParameterList.Add( TCPIPADDRESS__Parameter__, TCPIPADDRESS );
    
    __SPLS_TMPVAR__WAITLABEL_1___Callback = new WaitFunction( __SPLS_TMPVAR__WAITLABEL_1___CallbackFn );
    
    CONNECT.OnDigitalPush.Add( new InputChangeHandlerWrapper( CONNECT_OnPush_0, true ) );
    CONNECT.OnDigitalRelease.Add( new InputChangeHandlerWrapper( CONNECT_OnRelease_1, true ) );
    GETPOWERSTATE.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETPOWERSTATE_OnPush_2, true ) );
    POWERON.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWERON_OnPush_3, true ) );
    POWEROFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWEROFF_OnPush_4, true ) );
    GETINPUTSTATE.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETINPUTSTATE_OnPush_5, true ) );
    INPUTDP1.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTDP1_OnPush_6, true ) );
    INPUTDP2.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTDP2_OnPush_7, true ) );
    INPUTDP3.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTDP3_OnPush_8, true ) );
    INPUTHDMI1.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTHDMI1_OnPush_9, true ) );
    INPUTHDMI2.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTHDMI2_OnPush_10, true ) );
    INPUTHDMI3.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTHDMI3_OnPush_11, true ) );
    INPUTDVI.OnDigitalPush.Add( new InputChangeHandlerWrapper( INPUTDVI_OnPush_12, true ) );
    AUDIOMUTEONOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( AUDIOMUTEONOFF_OnPush_13, true ) );
    AUDIOMUTEONOFF.OnDigitalRelease.Add( new InputChangeHandlerWrapper( AUDIOMUTEONOFF_OnRelease_14, true ) );
    AUDIOVOLUME.OnAnalogChange.Add( new InputChangeHandlerWrapper( AUDIOVOLUME_OnChange_15, true ) );
    TCPCLIENT.OnSocketConnect.Add( new SocketHandlerWrapper( TCPCLIENT_OnSocketConnect_16, false ) );
    TCPCLIENT.OnSocketDisconnect.Add( new SocketHandlerWrapper( TCPCLIENT_OnSocketDisconnect_17, false ) );
    TCPCLIENT.OnSocketStatus.Add( new SocketHandlerWrapper( TCPCLIENT_OnSocketStatus_18, false ) );
    TCPCLIENT.OnSocketReceive.Add( new SocketHandlerWrapper( TCPCLIENT_OnSocketReceive_19, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_SAMSUNGSIGNAGE2301 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}


private WaitFunction __SPLS_TMPVAR__WAITLABEL_1___Callback;


const uint CONNECT__DigitalInput__ = 0;
const uint GETPOWERSTATE__DigitalInput__ = 1;
const uint POWERON__DigitalInput__ = 2;
const uint POWEROFF__DigitalInput__ = 3;
const uint GETINPUTSTATE__DigitalInput__ = 4;
const uint INPUTDP1__DigitalInput__ = 5;
const uint INPUTDP2__DigitalInput__ = 6;
const uint INPUTDP3__DigitalInput__ = 7;
const uint INPUTHDMI1__DigitalInput__ = 8;
const uint INPUTHDMI2__DigitalInput__ = 9;
const uint INPUTHDMI3__DigitalInput__ = 10;
const uint INPUTDVI__DigitalInput__ = 11;
const uint GETAUDIOMUTESTATE__DigitalInput__ = 12;
const uint AUDIOMUTEONOFF__DigitalInput__ = 13;
const uint AUDIOMUTEON__DigitalInput__ = 14;
const uint AUDIOMUTEOFF__DigitalInput__ = 15;
const uint GETVIDEOMUTESTATE__DigitalInput__ = 16;
const uint VIDEOMUTEONOFF__DigitalInput__ = 17;
const uint VIDEOMUTEON__DigitalInput__ = 18;
const uint VIDEOMUTEOFF__DigitalInput__ = 19;
const uint GETSERIALNUMBER__DigitalInput__ = 20;
const uint GETDISPLAYSTATUS__DigitalInput__ = 21;
const uint GETSOFTWAREVERSION__DigitalInput__ = 22;
const uint GETTEMPERATUREDEV__DigitalInput__ = 23;
const uint GETTEMPERATUREEXE__DigitalInput__ = 24;
const uint GETTEMPERATURELED__DigitalInput__ = 25;
const uint AUDIOVOLUME__AnalogSerialInput__ = 0;
const uint CONNECTFBK__DigitalOutput__ = 0;
const uint WARNFBK__DigitalOutput__ = 1;
const uint ERRORFBK__DigitalOutput__ = 2;
const uint POWERFBK__DigitalOutput__ = 3;
const uint INPUTDP1FBK__DigitalOutput__ = 4;
const uint INPUTDP2FBK__DigitalOutput__ = 5;
const uint INPUTDP3FBK__DigitalOutput__ = 6;
const uint INPUTHDMI1FBK__DigitalOutput__ = 7;
const uint INPUTHDMI2FBK__DigitalOutput__ = 8;
const uint INPUTHDMI3FBK__DigitalOutput__ = 9;
const uint INPUTDVIFBK__DigitalOutput__ = 10;
const uint AUDIOMUTEFBK__DigitalOutput__ = 11;
const uint VIDEOMUTEFBK__DigitalOutput__ = 12;
const uint STATUSLAMPERR__DigitalOutput__ = 13;
const uint STATUSTEMPERR__DigitalOutput__ = 14;
const uint STATUSBRIGHTERR__DigitalOutput__ = 15;
const uint STATUSNOSYNCERR__DigitalOutput__ = 16;
const uint STATUSFANERR__DigitalOutput__ = 17;
const uint AUDIOVOLUMEFBK__AnalogSerialOutput__ = 0;
const uint TEMPERATUREDEVFBK__AnalogSerialOutput__ = 1;
const uint UPTIMEFBK__AnalogSerialOutput__ = 2;
const uint LIFETIMEFBK__AnalogSerialOutput__ = 3;
const uint NAMEFBK__AnalogSerialOutput__ = 4;
const uint MANUFACTURERFBK__AnalogSerialOutput__ = 5;
const uint TYPEFBK__AnalogSerialOutput__ = 6;
const uint SERIALFBK__AnalogSerialOutput__ = 7;
const uint HOSTFBK__AnalogSerialOutput__ = 8;
const uint VERSIONFBK__AnalogSerialOutput__ = 9;
const uint INFOFBK__AnalogSerialOutput__ = 10;
const uint TCPIPADDRESS__Parameter__ = 10;
const uint DISPLAYID__Parameter__ = 11;
const uint OPTIMISTICFEEDBACK__Parameter__ = 12;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
