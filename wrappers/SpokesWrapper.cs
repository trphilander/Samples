﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Interop.Plantronics;
using System.Collections.ObjectModel;
using System.Threading;

[assembly: CLSCompliant(true)]

/*******
 * 
 * SpokesWrapper.cs
 * 
 * SpokesWrapper.cs is a wrapper around the Plantronics Spokes COM Service API for C# .NET (.NET Framework 4 and higher).
 * 
 * It's purpose is to make it easier and simpler to integrate support for Plantronics devices into any applications.
 * 
 * It achieves this by hiding a lot of the more tricky aspects of integration behind the wrapper and presenting
 * a simple and consistent set of Event Handlers and functions for the core features of the SDK that the user
 * will typically be needing.
 * 
 * *** WARNING !!! This source code is provided *As Is*! It is intented as a sample code to show ways of integrating
 * with the Spokes "COM Service .NET API". However in case of problems please feel free to contact Lewis Collins
 * directly via the PDC site at this address: http://developer.plantronics.com/people/lcollins/ ***
 * 
 * The latest version of this file will also be maintained (and feel free to create your own Fork!) on Github, here:
 * 
 * https://github.com/pltdev/Samples/tree/master/wrappers
 * 
 * Read more about Plantronics Spokes at the Plantronics Developer Connection web site:
 * 
 * http://developer.plantronics.com/community/devzone/
 * 
 * Lewis Collins
 * 
 * http://developer.plantronics.com/people/lcollins
 * 
 * VERSION HISTORY:
 * ********************************************************************************
 * Version 1.0.27:
 * Date: 11th Nov 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.55399.3
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added GetMute function to request the mute state
 *
 * Version 1.0.26:
 * Date: 29th Jan 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.38701.2
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added HoldCall and ResumeCall functions to inform Spokes
 *       of the state of multiple calls in your softphone queue
 *
 * Version 1.0.25:
 * Date: 10th Oct 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added missing callid member from OnCallArgs so we can know id
 *       of calls in other softphones etc
 *
 * Version 1.0.24b:
 * Date: 19th Sept 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added missing AnswerCall function to allow softphone to let Spokes know
 *       when user answered incoming call using softphone GUI.
 *
 * Version 1.0.24:
 * Date: 17th Sept 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added knowledge of the Plantronics device capabilities through
 *       deployment of supplementary file: "DeviceCapabilities.csv"
 *       This file should be placed in the working directory of the calling
 *       application.
 *
 * Version 1.0.23:
 * Date: 28th June 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added new IsSpokesInstalled convenience function. You can optionally call this before
 *       calling the Spokes.Instance.Connect method to check if Spokes is installed.
 *
 * Version 1.0.22:
 * Date: 28th June 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added new IsSpokesInstalled convenience function. You can optionally call this before
 *       calling the Spokes.Instance.Connect method to check if Spokes is installed.
 *       It is called by default anyway when Connect is called so that if Spokes is not
 *       installed, Connect will immediately return false (failed to connect), rather
 *       than cause a timeout waiting for COM Object to instantiate (and fail).
 *
 * Version 1.0.21:
 * Date: 17th June 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Fix to initial undocking detection for Savi 7xx
 *
 * Version 1.0.20:
 * Date: 30th May 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Final fix to docking detection of Legend UC!!!
 *
 * Version 1.0.19:
 * Date: 30th May 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Yet another minor fix to docking detection of Legend UC!
 *
 * Version 1.0.18:
 * Date: 28th May 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Another minor fix to docking detection of Legend UC
 *
 * Version 1.0.17:
 * Date: 24th May 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Adding BatteryLevelChange event notification when the battery charge state
 *       changes.
 *     - Adding GetBatteryLevel methods to be called by app in response
 *       to the above event notification event to obtain the current battery
 *       level.
 *     - Minor fix to docking detection of Legend UC
 *
 * Version 1.0.16:
 * Date: 14th May 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Adding base button press event handler so apps can know when base button
 *       is pressed.
 *     - Changing headset button press event registration to avoid duplicate headset
 *       talk button pressed events
 *
 * Version 1.0.15:
 * Date: 25th Apr 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Add function to send a custom message to headset
 *
 * Version 1.0.14:
 * Date: 12th Apr 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Minor fix to establish when _not_ on softphone call on device attach
 *     - Added initialization of display devices (dialpad devices) in case user
 *       wants to use CallRequested event (call dialled) from those devices
 *
 * Version 1.0.13:
 * Date: 11th Apr 2013
 * Compatible/tested with Spokes SDK version(s): 2.8.24304.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added docking/undocking (charging base) detection for Voyager Legend UC headset
 *
 * Version 1.0.12:
 * Date: 5th Apr 2013
 * Compatible with Spokes SDK version(s): 2.8.24304.0, 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added get ongoing softphone call status when you first start your app!
 *
 * Version 1.0.11:
 * Date: 14th Mar 2013
 * Compatible with Spokes SDK version(s): 2.8.24304.0, 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added try/catch exception handling and debug around registering for extended headsetstatechange
 *       and extended basestatechange events that were used for "asyncronous" method of receiving serial
 *       numbers from device (works for base serial, NOT for headset serial)
 *     - Also added "syncronous" method of obtaining serial numbers following discussion with Ramesh Mar 2013
 *       however this is NOT working for base OR headset serial! (So left in "asyncronous" method for now,
 *       at least we have base!)
 *
 * 
 * Version 1.0.10b:
 * Date: 08th Mar 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - New, get all the device state info on inrange trigger (not just proximity and headset serial number)
 *     
 * Version 1.0.10:
 * Date: 05th Mar 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Updated some internal methods to public (they didn't need to be internal)
 *     
 * Version 1.0.9:
 * Date: 22nd Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Add proximity enabled / proximity disabled event handlers
 *
 * Version 1.0.8:
 * Date: 21st Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Add headset button events via new ButtonPressed event handler in the wrapper.
 *       NOTE: you are advised NOT to use headset buttons e.g. talk button for
 *       call control, but rather use the IncomingCall/OutgoingCall functions
 *       and CallAnswered/CallEnded event handlers. Using talk button will
 *       cause problems with multiline devices as talk button events for the
 *       deskphone (+EHS) will also be received by your app through the SDK!!!!
 *       Also bad interactions can occur with talk button and other softphones
 *       on your system e.g. Lync if you try to use raw button events.
 *       You have been warned.
 *     - Add CallRequested event handler to obtain user call requested events from
 *       dialpad devices (Calisto P240/800 series)
 *
 * Version 1.0.7:
 * Date: 19th Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Change namespace to Plantronics.UC.SpokesWrapper (from Plantronics.UC.Spokes)
 *
 * Version 1.0.6:
 * Date: 14th Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Minor fix to incorrect worn state in TakenOff event handler
 *
 * Version 1.0.5:
 * Date: 8th Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added flag for don/doff/dock/undock event to say if it is the initial status
 *       so an app can ignore the initial status if it wants to (i.e. not lock screen
 *       when it first runs and receives initial status event!)
 *
 * Version 1.0.4:
 * Date: 6th Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added new method to ask if link is active
 *
 * Version 1.0.3:
 * Date: 1st Feb 2013
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Added new "line active changed" event handler so apps can know when 
 *       line to device is active or not
 *
 * Version 1.0.2:
 * Date: 4th December 2012
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Adding multiline device features e.g. for Savi 7xx
 *
 * Version 1.0.1:
 * Date: 30th November 2012
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Fixed order of events for DetachDevice flow
 *     - Fixed need to check for null serial number member
 *
 * Version 1.0:
 * Date: 30th November 2012
 * Compatible with Spokes SDK version(s): 2.7.14092.0
 * Changed by: Lewis Collins
 *   Changes:
 *     - Adds code to extract serial number (thanks Nemanja)
 *     - Adds comments to all publicly expose methods and event handlers  
 * ********************************************************************************
 * 
 **/

namespace Plantronics.UC.SpokesWrapper
{
    /// <summary>
    /// interface to allow your application's class to handle log debug tracing from the SpokesWrapper...
    /// </summary>
    public interface DebugLogger
    {
        void DebugPrint(string methodname, string str);
    }

    /// <summary>
    /// Struct to hold info on Plantronics device capabilities
    /// </summary>
    public struct SpokesDeviceCaps
    {
        public bool HasProximity;
        public bool HasMobCallerId; // means mobile caller id and mobile call state
        public bool HasMobCallState; // a subset of mobile caller id, just the mobile call state
        public bool HasDocking;
        public bool HasWearingSensor;
        public bool HasMultiline;
        public bool IsWireless;
        public string ProductId;

        /// <summary>
        /// Constructor: pass in boolean values for whether it has the given device capabilities or not
        /// </summary>
        public SpokesDeviceCaps(bool HasProximity, bool HasMobCallerId, bool HasMobCallState, bool HasDocking, bool HasWearingSensor, bool HasMultiline, bool IsWireless)
        {
            this.HasProximity = HasProximity;
            this.HasMobCallerId = HasMobCallerId;
            this.HasMobCallState = HasMobCallState;
            this.HasDocking = HasDocking;
            this.HasWearingSensor = HasWearingSensor;
            this.HasMultiline = HasMultiline;
            this.IsWireless = IsWireless;
            this.ProductId = "";
        }

        /// <summary>
        /// Returns a nice string representation of device capabilites, e.g. for use in logs
        /// </summary>
        public override string ToString()
        {
            return "Proximity = " + HasProximity + "\r\n" +
                "Mobile Caller Id = " + HasMobCallerId + "\r\n" +
                "Mobile Caller State = " + HasMobCallState + "\r\n" +
                "Dockable = " + HasDocking + "\r\n" +
                "Wearing Sensor = " + HasWearingSensor + "\r\n" +
                "Multiline = " + HasMultiline + "\r\n" +
                "Is Wireless = " + IsWireless + "\r\n";
        }
    }

    /// <summary>
    /// Event args for Mute Changed event handler
    /// </summary>
    public class MuteChangedArgs : EventArgs
    {
        public bool m_muteon = false;

        public MuteChangedArgs(bool isMuteOn)
        {
            m_muteon = isMuteOn;
        }
    }

        /// <summary>
    /// Event args for Line Active Changed event handler
    /// </summary>
    public class LineActiveChangedArgs : EventArgs
    {
        public bool m_lineactive = false;

        public LineActiveChangedArgs(bool isLineActive)
        {
            m_lineactive = isLineActive;
        }
    }

    /// <summary>
    /// Event args for Attached (device attached) event handler
    /// </summary>
    public class AttachedArgs : EventArgs
    {
        public IDevice m_device = null;

        public AttachedArgs(IDevice aDevice)
        {
            m_device = aDevice;
        }
    }

    /// <summary>
    /// Event args for TakenOff/PutOn events (wearing state) event handlers
    /// </summary>
    public class WearingStateArgs : EventArgs
    {
        public bool m_worn = false;
        public bool m_isInitialStateEvent = false;

        public WearingStateArgs(bool worn, bool isInitialStateEvent)
        {
            m_worn = worn;
            m_isInitialStateEvent = isInitialStateEvent;
        }
    }

    /// <summary>
    /// Event args for Docked/UnDocked events (docking) event handlers
    /// </summary>
    public class DockedStateArgs : EventArgs
    {
        public bool m_docked = false;
        public bool m_isInitialStateEvent = false;

        public DockedStateArgs(bool docked, bool isInitialStateEvent)
        {
            m_docked = docked;
            m_isInitialStateEvent = isInitialStateEvent;
        }
    }

    /// <summary>
    /// Enumeration of call states
    /// </summary>
    public enum OnCallCallState
    {
        Ringing,
        OnCall,
        Idle
    }

    /// <summary>
    /// Event args for OnCall event handler
    /// </summary>
    public class OnCallArgs : EventArgs
    {
        public int CallId { get; set; }
        public string CallSource;
        public bool Incoming;
        public OnCallCallState State;

        public OnCallArgs(int callid, string source, bool isIncoming, OnCallCallState state)
        {
            CallId = callid;
            CallSource = source;
            Incoming = isIncoming;
            State = state;
        }
    }

    /// <summary>
    /// Enumeration of mobile call states
    /// </summary>
    public enum MobileCallState
    {
        Ringing,
        OnCall,
        Idle
    }

    /// <summary>
    /// Event args for OnMobileCall event handler
    /// </summary>
    public class OnMobileCallArgs : EventArgs
    {
        public bool Incoming;
        public MobileCallState State;

        public OnMobileCallArgs(bool isIncoming, MobileCallState state)
        {
            Incoming = isIncoming;
            State = state;
        }
    }

    /// <summary>
    /// Event args for MobileCallerId event handler
    /// </summary>
    public class MobileCallerIdArgs : EventArgs
    {
        public string MobileCallerId { get; set; }

        public MobileCallerIdArgs(string mobilecallerid)
        {
            MobileCallerId = mobilecallerid;
        }
    }

    /// <summary>
    /// Enumeration of serial numbers in a Plantronics device (i.e. Headset and base/usb adaptor)
    /// </summary>
    public enum SerialNumberTypes
    {
        Headset,
        Base
    }

    /// <summary>
    /// Event args for SerialNumber event handler
    /// </summary>
    public class SerialNumberArgs : EventArgs
    {
        public string SerialNumber { get; set; }
        public SerialNumberTypes SerialNumberType { get; set; }

        public SerialNumberArgs(string serialnumber, SerialNumberTypes serialnumtype)
        {
            SerialNumber = serialnumber;
            SerialNumberType = serialnumtype;
        }
    }

    /// <summary>
    /// Event args for CallAnswered event handler
    /// </summary>
    public class CallAnsweredArgs : EventArgs
    {
        public int CallId { get; set; }
        public string CallSource { get; set; }

        public CallAnsweredArgs(int callid, string callsource)
        {
            CallId = callid;
            CallSource = callsource;
        }
    }

    /// <summary>
    /// Event args for CallEnded event handler
    /// </summary>
    public class CallEndedArgs : EventArgs
    {
        public int CallId { get; set; }
        public string CallSource { get; set; }

        public CallEndedArgs(int callid, string callsource)
        {
            CallId = callid;
            CallSource = callsource;
        }
    }

    /// <summary>
    /// Event args for NotOnCall event handler
    /// </summary>
    public class NotOnCallArgs : EventArgs
    {
        public int CallId { get; set; }
        public string CallSource { get; set; }

        public NotOnCallArgs(int callid, string callsource)
        {
            CallId = callid;
            CallSource = callsource;
        }
    }

    /// <summary>
    /// Used with MultiLineStateArgs to hold active/held status of multiple lines (PC, Mobile, Deskphone)
    /// </summary>
    public struct MultiLineStateFlags
    {
        public bool PCActive { get; set; }
        public bool MobileActive { get; set; }
        public bool DeskphoneActive { get; set; }
        public bool PCHeld { get; set; }
        public bool MobileHeld { get; set; }
        public bool DeskphoneHeld { get; set; }
    }

    /// <summary>
    /// EventArgs used with MultiLineStateChanged event handler to receive status of multiple lines (PC, Mobile, Deskphone) 
    /// when the state of any of these lines changes.
    /// </summary>
    public class MultiLineStateArgs : EventArgs
    {
        public MultiLineStateFlags MultiLineState { get; set; }

        public MultiLineStateArgs(MultiLineStateFlags multilinestate)
        {
            MultiLineState = multilinestate;
        }
    }

    /// <summary>
    /// EventArgs used with ButtonPress event handler to receive details of which button
    /// was pressed
    /// </summary>
    public class ButtonPressArgs : EventArgs
    {
        public HeadsetButton headsetButton;
        public AudioType audioType;
        public bool mute;

        public ButtonPressArgs(HeadsetButton headsetButton, AudioType audioType, bool aMute)
        {
            this.headsetButton = headsetButton;
            this.audioType = audioType;
            this.mute = aMute;
        }
    }

    /// <summary>
    /// EventArgs used with BaseButtonPress event handler to receive details of which button
    /// was pressed
    /// </summary>
    public class BaseButtonPressArgs : EventArgs
    {
        public BaseButton baseButton;

        public BaseButtonPressArgs(BaseButton baseButton)
        {
            this.baseButton = baseButton;
        }
    }

    /// <summary>
    /// EventArgs used with CallRequested event handler to receive details of the
    /// number requested to dial from dialpad device (Calisto P240/800 series)
    /// </summary>
    public class CallRequestedArgs : EventArgs
    {
        public Contact m_contact { get; set; }

        public CallRequestedArgs(Contact aContact)
        {
            m_contact = aContact;
        }
    }

    /// <summary>
    /// Enumeration of multiline device line types
    /// </summary>
    public enum Multiline_LineType
    {
        PC,
        Mobile,
        Deskphone
    }

    /// <summary>
    /// Defines a Spokes object which you can use to communicate with Plantronics devices.
    /// Cannot instantiate directly. To obtain singleton call Spokes.Instance.
    /// Note: using singleton model to avoid possibility of multiple instantiation
    /// as specified in: http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </summary>
    public sealed class Spokes
    {
        private static volatile Spokes instance;
        private static object syncRoot = new Object();

        private List<SpokesDeviceCaps> m_AllDeviceCapabilities;

        /// <summary>
        /// Default constructor, cannot be called directly. To obtain singleton call Spokes.Instance.
        /// </summary>
        private Spokes()
        {
            m_debuglog = null;

            PreLoadAllDeviceCapabilities();
        }

        private SpokesDeviceCaps GetMyDeviceCapabilities()
        {
	        SpokesDeviceCaps retval = new SpokesDeviceCaps();
            retval.ProductId="";
            string prodidstr;
            
            if (m_activeDevice!=null && m_AllDeviceCapabilities.Count()>0)
            {
                prodidstr = string.Format("{0:X}", m_activeDevice.ProductID).ToUpper();

                foreach (SpokesDeviceCaps caps in m_AllDeviceCapabilities)
                {
                    if (caps.ProductId.CompareTo(prodidstr)==0)
                    {
                        // we got a match of our product!
                        DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Got a match of our Plantronics device in DeviceCapabilities.csv:");
                        DebugPrint(MethodInfo.GetCurrentMethod().Name, prodidstr);
                        retval = caps;
                        break;
                    }
                }
            }
	        return retval;
        }

        private void PreLoadAllDeviceCapabilities()
        {
	        string line;

	        SpokesDeviceCaps devicecaps;
            m_AllDeviceCapabilities = new List<SpokesDeviceCaps>();

	        try 
	        {
                System.IO.StreamReader in_stream = 
                   new System.IO.StreamReader("DeviceCapabilities.csv");

		        while((line = in_stream.ReadLine()) != null)
                {
				        if (line.Length>0)
				        {
					        if (line.Substring(0,1).CompareTo("#")!=0 && line.Substring(0,1).CompareTo(",")!=0)
					        {
						        // not a comment line or empty line (with only commas)

                                devicecaps = new SpokesDeviceCaps();
                                string[] words = line.Split(',');

						        int i = 0;
                                string token = "";

                                foreach (string word in words)
	                            {
                                    token = word.ToUpper();
                                    switch(i)
							        {
							        case 0:
								        devicecaps.ProductId = word;
								        break;
							        case 1:
								        // no action - this is the device name we don't need
								        break;
							        case 2:
								        devicecaps.HasProximity = token.CompareTo("YES")==0 ? true : false;
								        break;
							        case 3:
                                        devicecaps.HasMobCallerId = token.CompareTo("YES") == 0 ? true : false;
								        break;
							        case 4:
                                        devicecaps.HasMobCallState = token.CompareTo("YES") == 0 ? true : false;
								        break;
							        case 5:
                                        devicecaps.HasDocking = token.CompareTo("YES") == 0 ? true : false;
								        break;
							        case 6:
                                        devicecaps.HasWearingSensor = token.CompareTo("YES") == 0 ? true : false;
								        break;
							        case 7:
                                        devicecaps.HasMultiline = token.CompareTo("YES") == 0 ? true : false;
								        break;
							        case 8:
                                        devicecaps.IsWireless = token.CompareTo("YES") == 0 ? true : false;

								        // now, add the devicecaps to our list:
								        m_AllDeviceCapabilities.Add(devicecaps);

								        break;
							        }

                                    i++;
                                }

						        //DebugPrint(__FUNCTION__, "got some tokens");
					        }
				        }
				        //devicecaps
		        }
		        in_stream.Close();
	        }
	        catch(Exception e) {
		        //std::cerr << "Exception opening/reading/closing file\n";
		        DebugPrint(MethodInfo.GetCurrentMethod().Name, "Exception reading DeviceCapabilities.csv. Does this file exist in current working directory?\r\n"+e.ToString());
	        }
        }

        /// <summary>
        /// Default desctructor, disconnects from Spokes
        /// </summary>
        ~Spokes()
        {
            if (isConnected)
                Disconnect();
        }

        /// <summary>
        /// Returns the single Instance of Spokes which you can use to communicate with Plantronics devices
        /// </summary>
        public static Spokes Instance
        {
            get 
            {
                if (instance == null) 
                {
                    lock (syncRoot) 
                    {
                        // Instantiate a singleton Spokes object
                        if (instance == null) 
                            instance = new Spokes();
                    }
                }

                return instance;
            }
        }

        #region Spokes interfaces definitions
        static ISessionCOMManager m_sessionComManager = null;
        static IComSession m_comSession = null;
        static IDevice m_activeDevice = null;
        static ISessionCOMManagerEvents_Event m_sessionManagerEvents;
        static ICOMCallEvents_Event m_sessionEvents;
        static IDeviceCOMEvents_Event m_deviceComEvents;
        static IDeviceListenerCOMEvents_Event m_deviceListenerEvents;
        static IATDCommand m_atdCommand;
        static IHostCommand m_hostCommand;
        static IHostCommandExt m_hostCommandExt;
        public static string m_devicename = "";
        #endregion

        DebugLogger m_debuglog = null;

        /// <summary>
        /// A property containing flags that indicate the capabilities of the attached Plantronics device (if any).
        /// </summary>
        public SpokesDeviceCaps DeviceCapabilities;

        /// <summary>
        /// Returns boolean to indicate whether there is currently a Plantronics device attached to the PC or not.
        /// </summary>
        public bool HasDevice
        {
            get
            {
                return (m_activeDevice != null);
            }
        }

        /// <summary>
        /// Returns a reference to the currently active Plantronics call control device attached to the PC (if any).
        /// </summary>
        public IDevice GetDevice
        {
            get
            {
                return m_activeDevice;
            }
        }

        bool m_mobIncoming = false; // mobile call direction flag
        bool m_voipIncoming = false; // mobile call direction flag

        #region Base Device State member fields
        MultiLineStateFlags m_activeHeldFlags;
        #endregion

        // C# event handlers that can be used to register for Spokes events...

        // Wearing sensor:
        public delegate void TakenOffEventHandler(object sender, WearingStateArgs e);
        public delegate void PutOnEventHandler(object sender, WearingStateArgs e);

        // Proximity:
        public delegate void NearEventHandler(object sender, EventArgs e);
        public delegate void FarEventHandler(object sender, EventArgs e);
        public delegate void ProximityEnabledEventHandler(object sender, EventArgs e);
        public delegate void ProximityDisabledEventHandler(object sender, EventArgs e);
        public delegate void ProximityUnknownEventHandler(object sender, EventArgs e);
        // In range/out of range:
        public delegate void InRangeEventHandler(object sender, EventArgs e);
        public delegate void OutOfRangeEventHandler(object sender, EventArgs e);
        // Docked/undocked:
        public delegate void DockedEventHandler(object sender, DockedStateArgs e);
        public delegate void UnDockedEventHandler(object sender, DockedStateArgs e);

        // Mobile caller id:
        public delegate void MobileCallerIdEventHandler(object sender, MobileCallerIdArgs e);
        public delegate void OnMobileCallEventHandler(object sender, OnMobileCallArgs e);
        public delegate void NotOnMobileCallEventHandler(object sender, EventArgs e);

        // Serial number (receives as result of earlier request for serial number):
        public delegate void SerialNumberEventHandler(object sender, SerialNumberArgs e);

        // Call control (headset button call control notification from Spokes):
        public delegate void CallAnsweredEventHandler(object sender, CallAnsweredArgs e);
        public delegate void CallEndedEventHandler(object sender, CallEndedArgs e);
        public delegate void CallSwitchedEventHandler(object sender, EventArgs e);
        // Call state notification (is user on a call or not?):
        public delegate void OnCallEventHandler(object sender, OnCallArgs e);
        public delegate void NotOnCallEventHandler(object sender, NotOnCallArgs e);
        // Mute sync:
        public delegate void MuteChangedEventHandler(object sender, MuteChangedArgs e);
        // Line active awareness:
        public delegate void LineActiveChangedEventHandler(object sender, LineActiveChangedArgs e);
        
        // Device attach/detach:
        public delegate void AttachedEventHandler(object sender, AttachedArgs e);
        public delegate void DetachedEventHandler(object sender, EventArgs e);
        // Device capabilities changed (depends on type of device attached):
        public delegate void CapabilitiesChangedEventHandler(object sender, EventArgs e);

        // Multiline device line state changed (for multi-line device, e.g. Savi 7xx):
        public delegate void MultiLineStateChangedEventHandler(object sender, MultiLineStateArgs e);

        // Button press event:
        public delegate void ButtonPressEventHandler(object sender, ButtonPressArgs e);

        // Base button press event:
        public delegate void BaseButtonPressEventHandler(object sender, BaseButtonPressArgs e);

        // Call Requested event:
        public delegate void CallRequestedEventHandler(object sender, CallRequestedArgs e);
                 
        // Battery Level Changed event:
        public delegate void BatteryLevelChangedEventHandler(object sender, EventArgs e);

        // Definition of event handlers that clients can use to be notified whenever the
        // spokes event occurs:

        // Wearing sensor: ************************************************************
        /// <summary>
        /// Triggered when the user takes off the headset (with products that support wearing sensor)
        /// </summary>
        public event TakenOffEventHandler TakenOff;

        /// <summary>
        /// Triggered when the user puts on the headset (with products that support wearing sensor)
        /// </summary>
        public event PutOnEventHandler PutOn;

        // Proximity: ************************************************************
        /// <summary>
        /// Triggered when a Plantronics device comes near to PC dongle
        /// </summary>
        public event NearEventHandler Near;

        /// <summary>
        /// Triggered when a Plantronics device goes far from PC dongle
        /// </summary>
        public event FarEventHandler Far;

        /// <summary>
        /// Triggered when a Plantronics device proximity has been enabled
        /// </summary>
        public event ProximityEnabledEventHandler ProximityEnabled;

        /// <summary>
        /// Triggered when a Plantronics device proximity has been disabled
        /// </summary>
        public event ProximityDisabledEventHandler ProximityDisabled;

        /// <summary>
        /// Triggered when a Plantronics device proximity is unknown
        /// </summary>
        public event ProximityEnabledEventHandler ProximityUnknown;

        /// <summary>
        /// Triggered when a Plantronics device comes into range of PC dongle
        /// </summary>
        public event InRangeEventHandler InRange;

        /// <summary>
        /// Triggered when a Plantronics device goes out of range of PC dongle
        /// </summary>
        public event OutOfRangeEventHandler OutOfRange;

        /// <summary>
        /// Triggered when a Plantronics device is docked in its base or cradle
        /// </summary>
        public event DockedEventHandler Docked;

        /// <summary>
        /// Triggered when a Plantronics device is undocked from its base or cradle
        /// </summary>
        public event DockedEventHandler UnDocked;

        // Mobile caller id: ************************************************************
        /// <summary>
        /// Triggered when a caller id has been received
        /// </summary>
        public event MobileCallerIdEventHandler MobileCallerId;

        // Call state notification (is user on a call or not?): ************************************************************
        /// <summary>
        /// Triggered when some mobile calling activity is detected with the device
        /// </summary>
        public event OnMobileCallEventHandler OnMobileCall;

        /// <summary>
        /// Triggered when mobile calling activity comes to an end with the device
        /// </summary>
        public event NotOnMobileCallEventHandler NotOnMobileCall;

        // Serial number (receives as result of earlier request for serial number): ************************************************************
        /// <summary>
        /// Triggered when a serial number has been received from device
        /// </summary>
        public event SerialNumberEventHandler SerialNumber;

        // Call control (headset button call control notification from Spokes): ************************************************************
        /// <summary>
        /// Triggered when the user answers a call using the headset device
        /// </summary>
        public event CallAnsweredEventHandler CallAnswered;

        /// <summary>
        /// Triggered when the user answers a call using the headset device
        /// </summary>
        public event CallEndedEventHandler CallEnded;

        /// <summary>
        /// Triggered when the user tries to switch call using the headset device by pressing switch (flash) button
        /// </summary>
        public event CallSwitchedEventHandler CallSwitched;

        // Call state notification (is user on a call or not?): ************************************************************
        /// <summary>
        /// Triggered when some calling activity is detected with the device
        /// </summary>
        public event OnCallEventHandler OnCall;

        /// <summary>
        /// Triggered when calling activity comes to an end with the device
        /// </summary>
        public event NotOnCallEventHandler NotOnCall;

        // Mute sync: ************************************************************
        /// <summary>
        /// Triggered when the user mutes or unmutes the headset device
        /// </summary>
        public event MuteChangedEventHandler MuteChanged;

        // Mute sync: ************************************************************
        /// <summary>
        /// Triggered when the spokes activates or deactivates the line to the headset device
        /// </summary>
        public event LineActiveChangedEventHandler LineActiveChanged;

        // Device attach/detach: ************************************************************
        /// <summary>
        /// Triggered when the user attached a Plantronics device
        /// </summary>
        public event AttachedEventHandler Attached;

        /// <summary>
        /// Triggered when the user detaches a Plantronics device
        /// </summary>
        public event DetachedEventHandler Detached;

        // Device capabilities changed (depends on type of device attached): ************************************************************
        /// <summary>
        /// Triggered when the capabilities available on the device changes, e.g. asyncronous proximity registration is completed or mobile caller id registration is completed
        /// </summary>
        public event CapabilitiesChangedEventHandler CapabilitiesChanged;

        // Multiline device line state changed (for multi-line device, e.g. Savi 7xx): ************************************************************
        /// <summary>
        /// Triggered when there is a change of the active or held states of any of the lines of multi-line device (e.g. Savi 7xx)
        /// </summary>
        public event MultiLineStateChangedEventHandler MultiLineStateChanged;

        // Triggered when a button press event is generated by device: ************************************************************
        /// <summary>
        /// Triggered when a button press event is generated by device
        /// NOTE: you are advised NOT to use headset buttons e.g. talk button for
        /// call control, but rather use the IncomingCall/OutgoingCall functions
        /// and CallAnswered/CallEnded event handlers. (more notes in SpokesWrapper.cs)
        /// </summary>
        public event ButtonPressEventHandler ButtonPress;

        // Triggered when a base button press event is generated by device: ************************************************************
        /// <summary>
        /// Triggered when a base button press event is generated by device
        /// </summary>
        public event BaseButtonPressEventHandler BaseButtonPress;

        // Triggered when a user call requested event is received from dialpad device: ************************************************************
        /// <summary>
        /// Triggered when a user call requested event is received from dialpad device
        /// </summary>
        public event CallRequestedEventHandler CallRequested;

        // Triggered when the battery level or battery status changes on the device: ************************************************************
        /// <summary>
        /// Triggered when the battery level or battery status changes on the device.
        /// In response you can call GetBatteryLevel method to
        /// get the battery level.
        /// </summary>
        public event BatteryLevelChangedEventHandler BatteryLevelChanged;

        // Now for the implementation of the event handlers:

        // Wearing sensor: ************************************************************
        // Invoke the Doffed event; called whenever user doffs (takes off) their headset
        private void OnTakenOff(WearingStateArgs e)
        {
            if (TakenOff != null)
                TakenOff(this, e);
        }

        private void OnPutOn(WearingStateArgs e)
        {
            if (PutOn != null)
                PutOn(this, e);
        }

        // Proximity: ************************************************************
        private void OnNear(EventArgs e)
        {
            if (Near != null)
                Near(this, e);
        }

        private void OnFar(EventArgs e)
        {
            if (Far != null)
                Far(this, e);
        }

        private void OnProximityEnabled(EventArgs e)
        {
            if (ProximityEnabled != null)
                ProximityEnabled(this, e);
        }

        private void OnProximityDisabled(EventArgs e)
        {
            if (ProximityDisabled != null)
                ProximityDisabled(this, e);
        }

        private void OnProximityUnknown(EventArgs e)
        {
            if (ProximityUnknown != null)
                ProximityUnknown(this, e);
        }

        private void OnInRange(EventArgs e)
        {
            if (InRange != null)
                InRange(this, e);
        }

        private void OnOutOfRange(EventArgs e)
        {
            if (OutOfRange != null)
                OutOfRange(this, e);
        }

        private void OnDocked(DockedStateArgs e)
        {
            if (Docked != null)
                Docked(this, e);
        }

        private void OnUnDocked(DockedStateArgs e)
        {
            if (UnDocked != null)
                UnDocked(this, e);

            if (e.m_isInitialStateEvent)
            {
                m_battlevEventCount = 0;
            }
        }

        // Mobile caller id: ************************************************************
        private void OnMobileCallerId(MobileCallerIdArgs e)
        {
            if (MobileCallerId != null)
                MobileCallerId(this, e);
        }

        private void OnOnMobileCall(OnMobileCallArgs e)
        {
            if (OnMobileCall != null)
                OnMobileCall(this, e);
        }

        private void OnNotOnMobileCall(EventArgs e)
        {
            if (NotOnMobileCall != null)
                NotOnMobileCall(this, e);
        }

        // Serial number (receives as result of earlier request for serial number): ************************************************************
        private void OnSerialNumber(SerialNumberArgs e)
        {
            if (SerialNumber != null)
                SerialNumber(this, e);
        }

        // Call control (headset button call control notification from Spokes): ************************************************************
        private void OnCallAnswered(CallAnsweredArgs e)
        {
            if (CallAnswered != null)
                CallAnswered(this, e);
        }

        private void OnCallEnded(CallEndedArgs e)
        {
            if (CallEnded != null)
                CallEnded(this, e);
        }

        private void OnCallSwitched(EventArgs e)
        {
            if (CallSwitched != null)
                CallSwitched(this, e);
        }

        // Call state notification (is user on a call or not?): ************************************************************
        private void OnOnCall(OnCallArgs e)
        {
            if (OnCall != null)
                OnCall(this, e);
        }

        private void OnNotOnCall(NotOnCallArgs e)
        {
            if (NotOnCall != null)
                NotOnCall(this, e);
        }

        // Mute sync: ************************************************************
        private void OnMuteChanged(MuteChangedArgs e)
        {
            if (MuteChanged != null)
                MuteChanged(this, e);
        }

        // Line active: ************************************************************
        private void OnLineActiveChanged(LineActiveChangedArgs e)
        {
            if (LineActiveChanged != null)
                LineActiveChanged(this, e);
        }

        // Device attach/detach: ************************************************************
        private void OnAttached(AttachedArgs e)
        {
            if (Attached != null)
                Attached(this, e);
        }

        private void OnDetached(EventArgs e)
        {
            if (Detached != null)
                Detached(this, e);
        }

        // Device capabilities changed (depends on type of device attached): ************************************************************
        private void OnCapabilitiesChanged(EventArgs e)
        {
            if (CapabilitiesChanged != null)
                CapabilitiesChanged(this, e);
        }

        // Multiline device line state changed (for multi-line device, e.g. Savi 7xx): ************************************************************
        private void OnMultiLineStateChanged(MultiLineStateArgs e)
        {
            if (MultiLineStateChanged != null)
                MultiLineStateChanged(this, e);
        }

        // Triggered when a button press event is generated by device: ************************************************************
        private void OnButtonPress(ButtonPressArgs e)
        {
            if (ButtonPress != null)
                ButtonPress(this, e);
        }

        // Triggered when a base button press event is generated by device: ************************************************************
        private void OnBaseButtonPress(BaseButtonPressArgs e)
        {
            if (BaseButtonPress != null)
                BaseButtonPress(this, e);
        }
        
        // Triggered when a user call requested event is received from dialpad device: ************************************************************
        private void OnCallRequested(CallRequestedArgs e)
        {
            if (CallRequested != null)
                CallRequested(this, e);
        }

        // Triggered when battery status changes on attached device: ************************************************************
        private void OnBatteryLevelChanged(EventArgs e)
        {
            if (BatteryLevelChanged != null)
                BatteryLevelChanged(this, e);
        }

        bool isConnected = false;
        private bool m_lastdocked = false;
        private int m_battlevEventCount = 0;
        private bool m_firstlegenddockcheck = true; // will become false after first check of Legend docked state (via charging events)
        private bool m_ignorenextundockedevent = false;
        private bool m_ignorenextbattlevevent = false;

        /// <summary>
        /// If your application class implements the Spokes.DebugLogger interface you can pass a reference to your application class
        /// to the SetLogger method. This allows your class to be responsible for debug logging of Spokes related debug trace information.
        /// </summary>
        /// <param name="aLogger">For this parameter pass the "this" reference of your class that implements Spokes.DebugLogger interface.</param>
        public void SetLogger(DebugLogger aLogger)
        {
            m_debuglog = aLogger;
        }

        /// <summary>
        /// This method returns a boolean to indicate if the Spokes software runtime is currently installed on the system.
        /// If the return value is false then any subsequent attempt to call Spokes.Instance.Connect("My App") will fail
        /// because it means that Spokes is not installed so there is no out of proc COM Service for your app to connect to.
        /// Note: Is also called by default at start of Connect method, so it is not necessary to call this directly from
        /// your app, but you have the option.
        /// Note: current version of this function is designed for Spokes 2.x and 3.x. For future major releases would need updating
        /// in IsSpokesComSessionManagerClassRegistered private function below.
        /// </summary>
        public bool IsSpokesInstalled(int spokesMajorVersion = 2)     // TODO: always insert the CORRECT major version for this Spokes Wrapper version here!
        {
            return IsSpokesComSessionManagerClassRegistered(spokesMajorVersion);
        }

        private bool IsSpokesComSessionManagerClassRegistered(int spokesMajorVersion)
        {
            bool foundCOMSessionManagerKey = false;

            DebugPrint(MethodInfo.GetCurrentMethod().Name, "About to look see if Spokes SessionManager is in registry");
            try
            {
                // reg keys of interest...
                string Spokes2xKeyName = @"Plantronics.UC.Common.SessionComManager\CLSID";
                string Spokes3xKeyName = @"Plantronics.COMSessionManager\CLSID";
                string Spokes2xSubKeyName = @"{F9E7AE8D-31E2-4968-BA53-3CC5E5A3100A}";
                string Spokes3xSubKeyName = @"{750B4A16-1338-4DB0-85BB-C6C89E4CB9AC}";

                // open them all...
                Microsoft.Win32.RegistryKey Spokes2xKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Spokes2xKeyName, false); // non writable
                Microsoft.Win32.RegistryKey Spokes3xKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Spokes3xKeyName, false); // non writable

                DebugPrint(MethodInfo.GetCurrentMethod().Name, "About to check if Spokes is installed, Major Version = " + spokesMajorVersion + ".x");

                switch (spokesMajorVersion)
                {
                    case 2:
                        // is Spokes 2x installed?
                        if (Spokes2xKey != null)
                        {
                            if (Spokes2xKey.GetValue(null) != null)
                            {
                                // did we find Spokes 2x SessionCOMManager key?
                                if (Spokes2xKey.GetValue(null).ToString() == Spokes2xSubKeyName)
                                    foundCOMSessionManagerKey = true;
                            }
                            Spokes2xKey.Close();
                        }
                        break;
                    case 3:
                        // is Spokes 3x installed?
                        if (Spokes3xKey != null)
                        {
                            if (Spokes3xKey.GetValue(null) != null)
                            {
                                // did we find Spokes 3x COMSessionManager key?
                                if (Spokes3xKey.GetValue(null).ToString() == Spokes3xSubKeyName)
                                    foundCOMSessionManagerKey = true;
                            }
                            Spokes3xKey.Close();
                        }
                        break;
                    default:
                        DebugPrint(MethodInfo.GetCurrentMethod().Name, "Attempt to check for unknown Spokes Major Version: " + spokesMajorVersion);
                        break;
                }
            }
            catch (Exception e)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "An exception was caught while looking to see if Spokes SessionManager is in registry.\r\nException = " + e.ToString());
            }

            return foundCOMSessionManagerKey;
        }

        /// <summary>
        /// Instruct Spokes object to connect to Spokes runtime engine and register itself
        /// so that it can begin to communicate with the attached Plantronics device.
        /// </summary>
        /// <param name="SessionName">Optional name of your appplication's session within Spokes runtime engine. If omitted it will default to "COM Session".</param>
        public bool Connect(string SessionName = "COM Session")
        {
            if (!IsSpokesInstalled())
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "FATAL ERROR: cannot connect if Spokes COMSessionManager/SessionCOMManager class is not registered! Spokes not installed (or wrong major version installed for this Spokes Wrapper)!");
                return false; // cannot connect if Spokes COM SessionManager class is not registered! Spokes not installed!
            }
            if (isConnected) return true;
            DeviceCapabilities =
                new SpokesDeviceCaps(false, false, false, false, false, false, false); // we don't yet know what the capabilities are
            OnCapabilitiesChanged(EventArgs.Empty);
            bool success = false;
            try
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // create session manager, and attach to session manager events
                m_sessionComManager = new SessionComManagerClass();
                m_sessionManagerEvents = m_sessionComManager as ISessionCOMManagerEvents_Event;
                if (m_sessionManagerEvents != null)
                {
                    m_sessionManagerEvents.CallStateChanged += m_sessionComManager_CallStateChanged;
                    m_sessionManagerEvents.DeviceStateChanged += m_sessionComManager_DeviceStateChanged;
                }
                else
                    success = false;

                ////////////////////////////////////////////////////////////////////////////////////////
                // register session to spokes
                m_comSession = m_sessionComManager.Register(SessionName);
                if (m_comSession != null)
                {
                    // attach to session call events
                    m_sessionEvents = m_comSession.CallEvents as ICOMCallEvents_Event;
                    if (m_sessionEvents != null)
                    {
                        m_sessionEvents.CallRequested += m_sessionEvents_CallRequested;
                        m_sessionEvents.CallStateChanged += m_sessionEvents_CallStateChanged;

                    }
                    else
                        success = false;

                    ////////////////////////////////////////////////////////////////////////////////////////
                    // Attach to active device and print all device information
                    // and registers for proximity (if supported by device)
                    AttachDevice();
                    success = true;
                }
            }
            catch (System.Exception e)
            {
                success = false;
                throw new Exception("Failed to connect to Spokes", e);
            }
            return success;
        }

        /// <summary>
        /// Instruct Spokes object to disconnect from Spokes runtime engine and unregister its
        /// session in Spokes.
        /// </summary>
        public void Disconnect()
        {
            DetachDevice();
            try
            {

            if (m_comSession != null)
            {
                if (m_sessionEvents != null)
                {
                    // release session events
                    m_sessionEvents.CallRequested -= m_sessionEvents_CallRequested;
                    m_sessionEvents.CallStateChanged -= m_sessionEvents_CallStateChanged;
                    Marshal.ReleaseComObject(m_sessionEvents);
                    m_sessionEvents = null;
                }
                // unregister session
                if (m_sessionEvents != null)
                {
                    m_sessionManagerEvents.DeviceStateChanged -= m_sessionComManager_DeviceStateChanged;
                }
                m_sessionComManager.UnRegister(m_comSession);
                Marshal.ReleaseComObject(m_comSession);
                m_comSession = null;
            }
            if (m_sessionComManager != null)
            {
                Marshal.ReleaseComObject(m_sessionComManager);
                m_sessionComManager = null;
            }
            }
            catch (Exception e)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Exception caught in disconnect: " + e.ToString());
            }
        }

        #region Print Session and Device events to console
        // print session manager events
        void m_sessionComManager_DeviceStateChanged(object sender, _DeviceStateEventArgs e)
        {

            // if our "Active device" was unplugged, detach from it and attach to new one
            if (e.State == DeviceState.DeviceState_Removed && m_activeDevice != null && string.Compare(e.DevicePath, m_activeDevice.DevicePath, true) == 0)
            {
                DetachDevice();
                AttachDevice();

            }
            else if (e.State == DeviceState.DeviceState_Added && m_activeDevice == null)
            {
                // if device is plugged, and we don't have "Active device", just attach to it
                AttachDevice();
            }
        }

        // print session manager events
        void m_sessionComManager_CallStateChanged(object sender, _CallStateEventArgs e)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: call state event = " + e.ToString());

            switch (e.Action)
            {
                case CallState.CallState_CallRinging:
                    m_voipIncoming = true;
                    // Getting here indicates user is ON A CALL!
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Calling activity detected!" + e.ToString());
                    OnOnCall(new OnCallArgs(e.CallId.Id, e.CallSource, m_voipIncoming, OnCallCallState.Ringing));
                    break;
                case CallState.CallState_MobileCallRinging:
                    m_mobIncoming = true;
                    // user incoming mobile call
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Mobile Calling activity detected!" + e.ToString());
                    OnOnMobileCall(new OnMobileCallArgs(m_mobIncoming, MobileCallState.Ringing));
                    break;
                case CallState.CallState_MobileCallInProgress:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Mobile Calling activity detected!" + e.ToString());
                    OnOnMobileCall(new OnMobileCallArgs(m_mobIncoming, MobileCallState.OnCall));
                    break;
                case CallState.CallState_AcceptCall:
                case CallState.CallState_CallInProgress:                    
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Call was ansswered/in progress!" + e.ToString());
                    OnOnCall(new OnCallArgs(e.CallId.Id, e.CallSource, m_voipIncoming, OnCallCallState.OnCall));
                    OnCallAnswered(new CallAnsweredArgs(e.CallId.Id, e.CallSource));
                    break;
                case CallState.CallState_HoldCall:
                case CallState.CallState_Resumecall:
                case CallState.CallState_TransferToHeadSet:
                case CallState.CallState_TransferToSpeaker:
                    // Getting here indicates user is ON A CALL!
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Calling activity detected!" + e.ToString());
                    OnOnCall(new OnCallArgs(e.CallId.Id, e.CallSource, m_voipIncoming, OnCallCallState.OnCall));
                    break;
                case CallState.CallState_MobileCallEnded:
                    m_mobIncoming = false;
                    // Getting here indicates user HAS FINISHED A CALL!
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Mobile Calling activity ended." + e.ToString());
                    OnNotOnMobileCall(EventArgs.Empty);
                    break;
                case CallState.CallState_CallEnded:
                case CallState.CallState_CallIdle:
                case CallState.CallState_RejectCall:
                case CallState.CallState_TerminateCall:
                    m_voipIncoming = false;
                    // Getting here indicates user HAS FINISHED A CALL!
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Calling activity ended." + e.ToString());
                    OnNotOnCall(new NotOnCallArgs(e.CallId.Id, e.CallSource));
                    OnCallEnded(new CallEndedArgs(e.CallId.Id, e.CallSource));
                    break;
                default:
                    // ignore other call state events
                    break;
            }
        }

        // used internally to get mobile caller id when we are notified of mobile caller id
        // event from Spokes
        private string GetMobileCallerID()
        {
            string retval = "";
            if (m_atdCommand != null)
            {
                try
                {
                    retval = m_atdCommand.CallerID;
                }
                catch (System.Exception e)
                {
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: Exception occured getting mobile caller id\r\nException = " + e.ToString());
                }
            }
            return retval;
        }

        // print session events
        void m_sessionEvents_CallStateChanged(object sender, _CallStateEventArgs e)
        {
            string id = e.CallId != null ? e.CallId.Id.ToString() : "none";

        }
        // print session events
        void m_sessionEvents_CallRequested(object sender, _CallRequestEventArgs e)
        {
            string contact = e.Contact != null ? e.Contact.Name : "none";
            DebugPrint(MethodInfo.GetCurrentMethod().Name, string.Format("Session CallRequested event: Contact:({0})", contact));
            OnCallRequested(new CallRequestedArgs(e.Contact));
        }
        // print device listner events
        void m_deviceListenerEvents_Handler(object sender, _DeviceListenerEventArgs e)
        {
            switch (e.DeviceEventType)
            {
                case DeviceEventType.DeviceEventType_ATDButtonPressed:
                    break;
                case DeviceEventType.DeviceEventType_ATDStateChanged:
                    DeviceListener_ATDStateChanged(sender, e);
                    break;
                case DeviceEventType.DeviceEventType_BaseButtonPressed:
                    DeviceListener_BaseButtonPressed(sender, e);
                    break;
                case DeviceEventType.DeviceEventType_BaseStateChanged:
                    DeviceListener_BaseStateChanged(sender, e);
                    break;
                case DeviceEventType.DeviceEventType_HeadsetButtonPressed:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "DeviceEventType_HeadsetButtonPressed "+e.HeadsetButtonPressed.ToString());
                    break;
                case DeviceEventType.DeviceEventType_HeadsetStateChanged:
                default:
                    break;
            }
        }

        // Respond to various base button presses by passing button pressed event to app event handler
        private void DeviceListener_BaseButtonPressed(object sender, _DeviceListenerEventArgs e)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseButtonPressed: {0}", e.BaseButtonPressed.ToString()));
            OnBaseButtonPress(new BaseButtonPressArgs(e.BaseButtonPressed));
        }

        // Respond to various base state changes by updating our knowledge of multiline active/held states...
        void DeviceListener_BaseStateChanged(object sender, _DeviceListenerEventArgs e)
        {
            // write your own code to react to the state change
            switch (e.BaseStateChange)
            {
                case BaseStateChange.BaseStateChange_Unknown:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: Unknown"));
                    break;
                case BaseStateChange.BaseStateChange_PstnLinkEstablished:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: PstnLinkEstablished"));
                    GetActiveStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_PstnLinkDown:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: PstnLinkDown"));
                    GetActiveStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_VoipLinkEstablished:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: VoipLinkEstablished"));
                    GetActiveStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_VoipLinkDown:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: VoipLinkDown"));
                    GetActiveStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_AudioMixer:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: AudioMixer"));
                    break;
                case BaseStateChange.BaseStateChange_RFLinkWideBand:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: RFLinkWideBand"));
                    break;
                case BaseStateChange.BaseStateChange_RFLinkNarrowBand:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: RFLinkNarrowBand"));
                    break;
                case BaseStateChange.BaseStateChange_MobileLinkEstablished:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: MobileLinkEstablished"));
                    GetActiveStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_MobileLinkDown:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: MobileLinkDown"));
                    GetActiveStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_InterfaceStateChanged:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: InterfaceStateChanged"));
                    GetHoldStates();
                    OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
                    break;
                case BaseStateChange.BaseStateChange_AudioLocationChanged:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: AudioLocationChanged"));
                    break;
                // LC add handler for basestate change serial number
                // We should be able to extract serial number at this point
                case BaseStateChange.BaseStateChange_SerialNumber:
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BaseStateChanged: SerialNumber"));
                    ReadSerialNumber(SerialNumberTypes.Base);
                    break;
            }
        }

        void m_deviceListenerEvents_HandlerMethods(object sender, _DeviceListenerEventArgs e)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Received Spokes Event: " + e.ToString());

            switch (e.DeviceEventType)
            {
                case DeviceEventType.DeviceEventType_HeadsetStateChanged:
                    switch (e.HeadsetStateChange)
                    {
                        case HeadsetStateChange.HeadsetStateChange_Don:
                            OnPutOn(new WearingStateArgs(true, false));
                            break;
                        case HeadsetStateChange.HeadsetStateChange_Doff:
                            OnTakenOff(new WearingStateArgs(false, false));
                            break;
                        case HeadsetStateChange.HeadsetStateChange_Near:
                            OnNear(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_Far:
                            OnFar(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_ProximityDisabled:
                            // Note: intepret this event as that the mobile phone has gone out of Bluetooth
                            // range and is no longer paired to the headset.
                            // Lock the PC, but immediately re-enable proximity
                            OnProximityDisabled(EventArgs.Empty);
                            // Immediately re-enable proximity
                            RegisterForProximity(true);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_ProximityEnabled:
                            OnProximityEnabled(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_ProximityUnknown:
                            OnProximityUnknown(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_InRange:
                            OnInRange(EventArgs.Empty);
                            //// Immediately re-enable proximity
                            //RegisterForProximity(true);
                            //// Request headset serial number (maybe user paired with another?)
                            //RequestSingleSerialNumber(SerialNumberTypes.Headset);
                            // New, get all the device state info on inrange trigger:
                            // now poll for current state (proximity, mobile call status, donned status, mute status)
                            GetInitialDeviceState();

                            // tell app to look again at battery level (headset in range)
                            OnBatteryLevelChanged(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_OutofRange:
                            OnOutOfRange(EventArgs.Empty);
                            OnSerialNumber(new SerialNumberArgs("", SerialNumberTypes.Headset));

                            // tell app to look again at battery level (headset out of range / disconnected)
                            OnBatteryLevelChanged(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_BatteryLevel:
                            // note; on legend we always get a battery level when docking...
                            // what is in this event, anything we can use to infer docked?
                            DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("HeadsetStateChanged: BatteryLevel"));
                            if (!m_ignorenextbattlevevent && (!m_lastdocked && m_battlevEventCount < 10)) // TEST, allow 10 of these thru so it gets docking more quickly on legend!
                            {
                                // Only if we were undocked and there were no battery level events
                                // since we last undocked should we detect batt charge status (docking status).
                                // This is so this only happens once, otherwise we get stuck in a loop
                                // Because looking at the battery info triggers another batterylevel changed
                                // event!
                                m_ignorenextundockedevent = true;
                                m_ignorenextbattlevevent = true;
                                m_lastdocked = DetectLegendDockedState(false); // NOTE: will ALWAYS trigger another batterylevel change event
                                // AND another UnDocked event if we are undocked!
                                m_battlevEventCount++;
                            }
                            else
                            {
                                if (m_ignorenextbattlevevent) m_ignorenextbattlevevent = false;
                                if (m_lastdocked) m_ignorenextundockedevent = false;
                            }

                            // tell app to look at battery level (it has changed)
                            OnBatteryLevelChanged(EventArgs.Empty);
                            break;
                        case HeadsetStateChange.HeadsetStateChange_Docked:
                        case HeadsetStateChange.HeadsetStateChange_DockedCharging:  // new, for legend, but is v SLOW, sometimes never comes
                            // only send to app the docked value if it is different to m_lastdocked (i.e. if it has changed)
                            if (!m_lastdocked) OnDocked(new DockedStateArgs(true, false));
                            m_lastdocked = true;
                            m_battlevEventCount = 0;
                            break;
                        case HeadsetStateChange.HeadsetStateChange_UnDocked:
                            if (!m_ignorenextundockedevent)
                            {
                                // only send to app the docked value if it is different to m_lastdocked (i.e. if it has changed)
                                if (m_lastdocked) OnUnDocked(new DockedStateArgs(false, false));
                                if (m_lastdocked) m_battlevEventCount = 0; // if we were docked before, set battlev event count to zero
                                // i.e. we would like to check the next battery level event
                                m_lastdocked = false;
                            }
                            m_ignorenextundockedevent = false;
                            break;
                        case HeadsetStateChange.HeadsetStateChange_MuteON:
                            OnMuteChanged(new MuteChangedArgs(true));
                            break;
                        case HeadsetStateChange.HeadsetStateChange_MuteOFF:
                            OnMuteChanged(new MuteChangedArgs(false));
                            break;
                        case HeadsetStateChange.HeadsetStateChange_MonoON:
                            OnLineActiveChanged(new LineActiveChangedArgs(true));
                            break;
                        case HeadsetStateChange.HeadsetStateChange_MonoOFF:
                            OnLineActiveChanged(new LineActiveChangedArgs(false));
                            break;
                        // LC add handler for headsetstate change serial number
                        // We should be able to extract serial number at this point
                        case HeadsetStateChange.HeadsetStateChange_SerialNumber:
                            DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("HeadsetStateChanged: SerialNumber"));
                            ReadSerialNumber(SerialNumberTypes.Headset);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

        }

        private bool DetectLegendDockedState(bool getinitialstate = false)
        {
            bool isdocked = false;
            if (m_activeDevice != null && m_activeDevice.ProductName.ToUpper().Contains("BT300"))
            {
                if (m_hostCommandExt != null)
                {
                    switch (m_hostCommandExt.BatteryInfo.Charge)
                    {
                        case BTChargingStatus.BTChargingStatus_ConnectedAndChargeError:
                        case BTChargingStatus.BTChargingStatus_ConnectedAndFastCharging:
                        case BTChargingStatus.BTChargingStatus_ConnectedAndTrickleCharging:
                        case BTChargingStatus.BTChargingStatus_ConnectedNotCharging:
                            // charging, so send docked event (for legend)
                            DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("BT300 received charger connected status: assume Legend is docked!"));
                            // only send to app the docked value if it is different to m_lastdocked (i.e. if it has changed)
                            if (IsFirstLegendDockedCheck() || !m_lastdocked) OnDocked(new DockedStateArgs(true, getinitialstate));
                            isdocked = true;
                            break;
                        case BTChargingStatus.BTChargingStatus_NotBatteryPowered:
                        case BTChargingStatus.BTChargingStatus_NotConnected:
                        case BTChargingStatus.BTChargingStatus_Unknown:
                            // only send to app the docked value if it is different to m_lastdocked (i.e. if it has changed)
                            if (IsFirstLegendDockedCheck() || m_lastdocked) OnUnDocked(new DockedStateArgs(false, getinitialstate));
                            isdocked = false;
                            break;
                    }
                }
            }
            return isdocked;
        }

        private bool IsFirstLegendDockedCheck()
        {
            bool retval = m_firstlegenddockcheck;
            m_firstlegenddockcheck = false;
            return retval;
        }

        /// <summary>
        /// Request from Spokes information about the battery level in the attached wireless device.
        /// Typically your app will call this after receiving a BatteryLevel headset event.
        /// </summary>
        /// <returns>An BatteryLevel structure containing information about the battery level.</returns>
        public BatteryLevel GetBatteryLevel()
        {
            BatteryLevel level = BatteryLevel.BatteryLevel_Empty;
            try
            {
                if (m_activeDevice != null)
                {
                    if (m_hostCommandExt != null)
                    {
                        level = m_hostCommandExt.BatteryLevel;
                    }
                }
            }
            catch (Exception) { }
            return level;
        }

        private void ReadSerialNumber(SerialNumberTypes serialNumberType)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: About to read out serial number for: " + serialNumberType);
            try
            {
                byte[] serialbuf = new byte[16]; // temp storage for the guid
                if (m_hostCommandExt != null)
                {
                    switch (serialNumberType)
                    {
                        case SerialNumberTypes.Headset:
                            m_hostCommandExt.GetSerialNumber_2(DeviceType.DeviceType_Headset, serialbuf);
                            if (serialbuf[0] != 0)
                            {
                                string serialStr = byteArrayToString(serialbuf);
                                //Console.WriteLine(string.Format("Headset serial number: {0}", serialStr));
                                OnSerialNumber(new SerialNumberArgs(serialStr, SerialNumberTypes.Headset));
                            }
                            else
                            {
                                DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("FAILED TO READ SERIAL from HeadsetStateChanged: SerialNumber event / m_hostCommandExt.GetSerialNumber_2(DeviceType.DeviceType_Headset, serialbuf);"));
                            }
                            break;
                        case SerialNumberTypes.Base:
                            m_hostCommandExt.GetSerialNumber_2(DeviceType.DeviceType_Base, serialbuf);
                            if (serialbuf[0] != 0)
                            {
                                string serialStr = byteArrayToString(serialbuf);
                                //Console.WriteLine(string.Format("Base serial number: {0}", serialStr));
                                OnSerialNumber(new SerialNumberArgs(serialStr, SerialNumberTypes.Base));
                            }
                            else
                            {
                                DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("FAILED TO READ SERIAL from BaseStateChanged: SerialNumber event / m_hostCommandExt.GetSerialNumber_2(DeviceType.DeviceType_Base, serialbuf);"));
                            }
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: serial number may not be supported on your device.");
            }
        }

        //        #region DeviceListener events
        void DeviceListener_ATDStateChanged(object sender, _DeviceListenerEventArgs e)
        {
            //HANDLE ATD State changes
            switch (e.ATDStateChange)
            {
                case ATDStateChange.ATDStateChange_MobileInComing:
                    m_mobIncoming = true; // set on call flag
                    OnOnMobileCall(new OnMobileCallArgs(m_mobIncoming, MobileCallState.Ringing));
                    break;
                case ATDStateChange.ATDStateChange_MobileOnCall:
                    OnOnMobileCall(new OnMobileCallArgs(m_mobIncoming, MobileCallState.OnCall));
                    break;
                case ATDStateChange.ATDStateChange_MobileCallEnded:
                    m_mobIncoming = false; // clear mobile call direction flag
                    OnNotOnMobileCall(EventArgs.Empty);
                    break;
                case ATDStateChange.ATDStateChange_MobileCallerID:
                    OnMobileCallerId(new MobileCallerIdArgs(GetMobileCallerID()));
                    break;
                case ATDStateChange.ATDStateChange_MobileOutGoing:
                    break;
                case ATDStateChange.ATDStateChange_PstnInComingCallRingOn:
                    break;
                case ATDStateChange.ATDStateChange_PstnInComingCallRingOff:
                    break;
            }
        }

        // print device events
        void m_deviceComEvents_Handler(object sender, _DeviceEventArgs e)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, string.Format("Device Event: Audio:{0} Buton:{1} Mute:{2} Usage:{3}", e.AudioState, e.ButtonPressed, e.Mute, e.Usage.ToString()));

            if (e.ButtonPressed == HeadsetButton.HeadsetButton_Flash)
            {
                OnCallSwitched(EventArgs.Empty);
            }

            OnButtonPress(new ButtonPressArgs(e.ButtonPressed, e.AudioState, e.Mute));
        }
        #endregion

        // attach to device events
        private void AttachDevice()
        {
            try
            {
                m_activeDevice = m_comSession.ActiveDevice;
            }
            catch (Exception e)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Exception caught attaching to device: "+e.ToString());
            }
            if (m_activeDevice != null)
            {
                // LC assume minimum first set of device capabilities...
                DeviceCapabilities =
                    new SpokesDeviceCaps(false, false, false, false, false, false, false);
                OnCapabilitiesChanged(EventArgs.Empty);

                OnSerialNumber(new SerialNumberArgs("", SerialNumberTypes.Base));
                OnSerialNumber(new SerialNumberArgs("", SerialNumberTypes.Headset));

                // LC have seen case where ProductName was empty but InternalName was not...
                if (m_activeDevice.ProductName.Length > 0)
                {
                    m_devicename = m_activeDevice.ProductName;
                }
                else if (m_activeDevice.InternalName.Length > 0)
                {
                    m_devicename = m_activeDevice.InternalName;
                }
                else
                {
                    m_devicename = "Could not determine device name";
                }

                m_lastdocked = false;
                m_battlevEventCount = 0;

                m_deviceComEvents = m_activeDevice.DeviceEvents as IDeviceCOMEvents_Event;
                if (m_deviceComEvents != null)
                {
                    // Attach to device events
                    m_deviceComEvents.ButtonPressed += m_deviceComEvents_Handler;
                    m_deviceComEvents.AudioStateChanged += m_deviceComEvents_Handler;
                    //m_deviceComEvents.FlashPressed += m_deviceComEvents_Handler;
                    m_deviceComEvents.MuteStateChanged += m_deviceComEvents_Handler;
                    //m_deviceComEvents.SmartPressed += m_deviceComEvents_Handler;
                    //m_deviceComEvents.TalkPressed += m_deviceComEvents_Handler;
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: AttachedEventHandler to device events");
                }
                else
                {
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error: unable to attach to device events");
                    return;
                }

                // if the ActiveDevice is a Calisto device, need additional initialization
                if (m_activeDevice.HostCommand.IsSupported(FeatureType.FeatureType_DisplayDevice))
                {
                    InitDisplayDevice();
                }

                m_deviceListenerEvents = m_activeDevice.DeviceListener as IDeviceListenerCOMEvents_Event;
                if (m_deviceListenerEvents != null)
                {
                    // Attach to device listener events
                    m_deviceListenerEvents.ATDStateChanged += m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.BaseButtonPressed += m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.BaseStateChanged += m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.HeadsetButtonPressed += m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.HeadsetStateChanged += m_deviceListenerEvents_HandlerMethods;
                }
                else
                {
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error: unable to attach to device listener events");
                    return;
                }

                // The below call (now with exception catching) tries to register
                // for extended headsetstatechange and base events that were used
                // for reading serial numbers from device using "asyncronous" method.
                // Following discussion with Ramesh Feb 2013 the Spokes Wrapper 
                // primarily tries to read serial numbers using the "syncronous"
                // method based on HeadsetStateChange/BaseStateChange serial number
                // events and GetSerialNumber_2 method. However, this has been left
                // in because GetSerialNumber_2 is not working! (At least this way the
                // base serial number is obtainable! LC 14th Mar 2013)
               RegisterForExtendedEvents();

                m_hostCommand = m_activeDevice.HostCommand;
                if (m_hostCommand == null) DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error: unable to obtain host command interface");
                m_atdCommand = m_activeDevice.HostCommand as IATDCommand;
                if (m_atdCommand == null) DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error: unable to obtain atd command interface");
                m_hostCommandExt = m_activeDevice.HostCommand as IHostCommandExt;
                if (m_hostCommandExt == null) DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error: unable to obtain host command ext interface");

                UpdateOtherDeviceCapabilities();

                // trigger user's event handler
                OnAttached(new AttachedArgs(m_activeDevice));

                // now poll for current state (proximity, mobile call status, donned status, mute status)
                GetInitialDeviceState();

                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: AttachedEventHandler to device");
            }
        }

        // Initialization code for Calisto devices
        private void InitDisplayDevice()
        {
            if(m_activeDevice != null)
            {
                try
                {
                    IDisplayDeviceListener ddl = m_activeDevice.DeviceListener as IDisplayDeviceListener;
                    if(ddl != null)
                    {
                        DDSoftphoneID ddSP = DDSoftphoneID.DDSoftphoneID_Unknown;
                        bool bActive = true;
                        ddl.SetDefaultSoftphone(ddSP);
                        ddl.SetPresence(ddSP, bActive ? DDPresence.DDPresence_Available : DDPresence.DDPresence_Closed);
                        ddl.SetDateTimeFormat(Thread.CurrentThread.CurrentCulture.Name);
                        ddl.SetLocale(Thread.CurrentThread.CurrentCulture.LCID);
                        ddl.SetDateTime(DateTime.Now);

                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_AvayaIPAgent, "AvayaIPAgent");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_AvayaIPSoftphone, "AvayaIPSoftphone");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_AvayaOneXAgent, "AvayaOneXAgent");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_AvayaOneXCommunicator, "AvayaOneXCommunicator");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_CiscoIPCommunicator, "CiscoIPCommunicator");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_CSF, "CiscoUCClientsCSF");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_IBMSameTime, "IBMSameTime");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_MSOfficeCommunicator, "MicrosoftOfficeCommunicator");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_NECSP350, "NECSP350");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_ShoreTel, "ShoreTelCallManager");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_ShoreTelCommunicator, "ShoreTelCommunicator");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_Skype, "Skype");
                        ddl.SetSoftphoneName(DDSoftphoneID.DDSoftphoneID_Unknown, "Unknown");
                    }
                }
                catch(Exception ex)
                {
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Exception in InitDisplayDevice(): " + ex.ToString());
                }
            }
        }

        private void RegisterForExtendedEvents()
        {
            // LC, Nemanja change, wire up serial number friendly events
            try
            {
                IDeviceCOMEventsExt_Event eex = m_deviceComEvents as IDeviceCOMEventsExt_Event;
                eex.HeadsetStateChanged += eex_HeadsetStateChanged;
            }
            catch (Exception)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Failed to register for extended headsetstatechange events.");
            }
            try
            {
                IBaseCOMEvents_Event be = m_deviceComEvents as IBaseCOMEvents_Event;
                be.BaseEventReceived += be_BaseEventReceived;
            }
            catch (Exception)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Failed to register for extended basestatechange events.");
            }
        }

        // NEW, Nemanja's code to get the serial ID's !
        private void be_BaseEventReceived(object sender, _BaseEventArgs e)
        {
            if (e.SerialNumber!=null && e.SerialNumber[0] != 0)
            {
                string serialStr = byteArrayToString(e.SerialNumber);
                //Console.WriteLine(string.Format("Base serial number: {0}", serialStr));
                OnSerialNumber(new SerialNumberArgs(serialStr, SerialNumberTypes.Base));
            }
        }

        // NEW, Nemanja's code to get the serial ID's !
        private void eex_HeadsetStateChanged(object sender, _HeadsetStateEventArgs e)
        {
            if (e.SerialNumber != null && e.SerialNumber[0] != 0)
            {
                string serialStr = byteArrayToString(e.SerialNumber);
                //Console.WriteLine(string.Format("Headset serial number: {0}", serialStr));
                OnSerialNumber(new SerialNumberArgs(serialStr, SerialNumberTypes.Headset));
            }
        }

        // NEW, Nemanja's code to get the serial ID's !
        private static string byteArrayToString(byte[] p)
        {
            StringBuilder b = new StringBuilder();
            foreach (byte x in p)
                b.Append(x.ToString("X2"));
            return b.ToString();
        }

        // now poll for current state (proximity, mobile call status, donned status, mute status)
        private void GetInitialDeviceState()
        {
            if (m_activeDevice != null)
            {
                RegisterForProximity(true);

                GetInitialSoftphoneCallStatus(); // are we on a call?

                GetInitialMobileCallStatus(); // are we on a call?

                GetInitialDonnedStatus(); // are we donned?

                GetInitialMuteStatus();

                RequestAllSerialNumbers();

                GetLastDockedStatus();

                GetActiveAndHeldStates();

                OnLineActiveChanged(new LineActiveChangedArgs(m_hostCommand.AudioState == AudioType.AudioType_MonoOn)); // is the line active?
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: No device is attached, cannot get initial device state.");
            }
        }

        private void GetInitialSoftphoneCallStatus()
        {
            if (m_sessionComManager.CallManagerState.HasActiveCall)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: We have an ACTIVE call!");

                OnOnCall(new OnCallArgs(0, "", false, OnCallCallState.OnCall)); // for now just say we are on a call

                // TODO Raise a TT - the ICallInfo interface is NOT exposed via Spokes SDK .NET API!
                //Collection<ICallInfo> calls = (Collection<ICall>)m_sessionComManager.CallManagerState.GetCalls;
                //DebugPrint(MethodInfo.GetCurrentMethod().Name, "Got Calls");
            }
            else
            {
                OnNotOnCall(new NotOnCallArgs(-1, "")); // we are not on a call
            }
        }

        private void GetActiveAndHeldStates()
        {
            try
            {
                GetHoldStates();
                GetActiveStates();
                OnMultiLineStateChanged(new MultiLineStateArgs(m_activeHeldFlags));
            }
            catch (Exception)
            {
                // probably the attached device doesn't have multiline, lets inform user...
                DeviceCapabilities.HasMultiline = false;
                OnCapabilitiesChanged(EventArgs.Empty);
            }
        }

        private void DebugPrint(string methodname, string message)
        {
            if (m_debuglog!=null)
                m_debuglog.DebugPrint(methodname, message);
        }

        // hard coded other device caps, beside caller id
        private void UpdateOtherDeviceCapabilities()
        {
	        // NEW if DeviceCapabilities.csv file exists in your app's current working directory with a list of device
	        // features in the following format (one device per line):
	        // ProductId,DeviceName,HasProximity,HasMobCallerId,HasMobCallState,HasDocking,HasWearingSensor,HasMultiline,IsWireless
	        // Then use those capabilities for current active device
	        //

	        // Is the m_AllDeviceCapabilities vector populated? And is my device id in there?
	        SpokesDeviceCaps myDeviceCapabilities = GetMyDeviceCapabilities();

            if (myDeviceCapabilities.ProductId.Length > 0)
            {
                // we have found device in the DeviceCapabilities.csv file
                DeviceCapabilities.HasProximity = myDeviceCapabilities.HasProximity;
                DeviceCapabilities.HasMobCallerId = myDeviceCapabilities.HasMobCallerId;
                DeviceCapabilities.HasMobCallState = myDeviceCapabilities.HasMobCallState;
                DeviceCapabilities.HasDocking = myDeviceCapabilities.HasDocking;
                DeviceCapabilities.HasWearingSensor = myDeviceCapabilities.HasWearingSensor;
                DeviceCapabilities.HasMultiline = myDeviceCapabilities.HasMultiline;
                DeviceCapabilities.IsWireless = myDeviceCapabilities.IsWireless;
            }
            else
            {
                // OK, the Spokes Wrapper user maybe doesn't have the DeviceCapabilities.csv file
                // deployed in app's working directory. Falling back to old hard-coded capabilities
                // (which don't cover the whole product range
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Did not find product in DeviceCapabilities.csv or DeviceCapabilities.csv not present for device:");
                DebugPrint(MethodInfo.GetCurrentMethod().Name, m_devicename);
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Will assume minimum capabilities, unless overridden by hard-coded capabilities in UpdateOtherDeviceCapabilities function.");

                // LC temporarily hard-code some device capabilities
                // e.g. fact that Blackwire C710/C720 do not support proximity, docking and is not wireless
                string devname = m_devicename;
                if (devname != null)
                {
                    devname = devname.ToUpper();
                    // TODO: Overtime keep this code up-to-date with ALL our current products!!!
                    // TODO: Future plan automatically work out device features based on HID Usages
                    // (for now I think hard-coded works quite well)
                    if (devname.Contains("BLACKWIRE"))
                    {
                        DeviceCapabilities.IsWireless = false;
                        DeviceCapabilities.HasDocking = false;
                        DeviceCapabilities.HasWearingSensor = false;
                    }
                    if (devname.Contains("C710") || devname.Contains("C720"))
                    {
                        DeviceCapabilities.HasProximity = false;
                        DeviceCapabilities.HasMobCallerId = false;
                        DeviceCapabilities.HasWearingSensor = true;
                        DeviceCapabilities.HasDocking = false;
                        DeviceCapabilities.IsWireless = false;
                    }
                    // LC new - if using vpro or vlegend then disable docking feature...
                    if (devname.Contains("BT300"))
                    {
                        DeviceCapabilities.HasProximity = true;
                        DeviceCapabilities.HasMobCallerId = true;
                        DeviceCapabilities.HasMobCallState = true;
                        DeviceCapabilities.HasWearingSensor = true;
                        DeviceCapabilities.HasDocking = true; // updated, legend does have docking
                        DeviceCapabilities.IsWireless = true;
                    }
                    if (devname.Contains("SAVI 7"))
                    {
                        DeviceCapabilities.HasWearingSensor = false;
                        DeviceCapabilities.HasMultiline = true;
                        DeviceCapabilities.HasMobCallState = true;
                        DeviceCapabilities.HasDocking = true;
                        DeviceCapabilities.IsWireless = true;
                    }
                }
            }

            OnCapabilitiesChanged(EventArgs.Empty);
        }

        // detach from device events
        void DetachDevice()
        {
            if (m_activeDevice != null)
            {
                if (m_deviceComEvents != null)
                {
                    // commented out - not using these any more (see AttachDevice comment)
                    //// LC, new unregister the serial number events
                    //IDeviceCOMEventsExt_Event eex = m_deviceComEvents as IDeviceCOMEventsExt_Event;
                    //eex.HeadsetStateChanged -= eex_HeadsetStateChanged;
                    //IBaseCOMEvents_Event be = m_deviceComEvents as IBaseCOMEvents_Event;
                    //be.BaseEventReceived -= be_BaseEventReceived;

                    // unregister device event handlers
                    m_deviceComEvents.ButtonPressed -= m_deviceComEvents_Handler;
                    m_deviceComEvents.AudioStateChanged -= m_deviceComEvents_Handler;
                    m_deviceComEvents.FlashPressed -= m_deviceComEvents_Handler;
                    m_deviceComEvents.MuteStateChanged -= m_deviceComEvents_Handler;
                    m_deviceComEvents.SmartPressed -= m_deviceComEvents_Handler;
                    m_deviceComEvents.TalkPressed -= m_deviceComEvents_Handler;

                    Marshal.ReleaseComObject(m_deviceComEvents);
                    m_deviceComEvents = null;
                }
                if (m_deviceListenerEvents != null)
                {
                    // unregister device listener event handlers
                    m_deviceListenerEvents.ATDStateChanged -= m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.BaseButtonPressed -= m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.BaseStateChanged -= m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.HeadsetButtonPressed -= m_deviceListenerEvents_Handler;
                    m_deviceListenerEvents.HeadsetStateChanged -= m_deviceListenerEvents_HandlerMethods;

                    RegisterForProximity(false);
                    Marshal.ReleaseComObject(m_deviceListenerEvents);
                    m_deviceListenerEvents = null;
                }

                Marshal.ReleaseComObject(m_activeDevice);
                m_activeDevice = null;

                m_hostCommand = null;
                m_hostCommandExt = null;
                m_atdCommand = null;

                // LC Device was disconnected, clear down the GUI state...
                m_mobIncoming = false; // clear mobile call direction flag
                m_voipIncoming = false; // clear call direction flag
                OnNotOnCall(new NotOnCallArgs(0,""));
                OnNotOnMobileCall(EventArgs.Empty);

                OnSerialNumber(new SerialNumberArgs("", SerialNumberTypes.Base));
                OnSerialNumber(new SerialNumberArgs("", SerialNumberTypes.Headset));

                // LC Device was disconnected, remove capability data
                DeviceCapabilities = new SpokesDeviceCaps(false, false, false, false, false, false, false); // no device = no capabilities!
                m_devicename = "";
                OnCapabilitiesChanged(EventArgs.Empty);

                m_lastdocked = false;
                m_battlevEventCount = 0;

                // trigger user's event handler
                OnDetached(EventArgs.Empty);

                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: DetachedEventHandler from device");
            }
            m_devicename = "";
        }

        private void RegisterForProximity(bool register)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: About to register for proximity.");
            try
            {
                if (m_hostCommandExt != null)
                {
                    m_hostCommandExt.EnableProximity(register); // enable proximity reporting for device
                    if (register) m_hostCommandExt.GetProximity();    // request to receive asyncrounous near/far proximity event to HeadsetStateChanged event handler. (note: will return it once. To get continuous updates of proximity you would need a to call GetProximity() repeatedly, e.g. in a worker thread).
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Completed request to register for proximity.");

                    DeviceCapabilities.HasProximity = true;

                    // Tweak availability of proximity per-device...
                    string devname = m_devicename.ToUpper();
                    if (devname.Contains("C710") || devname.Contains("C720"))
                    {
                        DeviceCapabilities.HasProximity = false;
                    }

                    OnCapabilitiesChanged(EventArgs.Empty);
                }
            }
            catch (System.Exception)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: proximity may not be supported on your device.");
                // uh-oh proximity may not be supported... disable it as option in GUI
                DeviceCapabilities.HasProximity = false;
                OnCapabilitiesChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Instruct Spokes to tell us the serial numbers of attached Plantronics device, i.e. headset and base/usb adaptor.
        /// </summary>
        public void RequestAllSerialNumbers()
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: About to request serial numbers.");
            RequestSingleSerialNumber(SerialNumberTypes.Base);
            RequestSingleSerialNumber(SerialNumberTypes.Headset);
        }

        // Some internal methods to get line active/held states of multi-line devices:
        private void GetHoldStates()
        {
            m_activeHeldFlags.DeskphoneHeld = GetHoldState(LineType.LineType_PSTN);
            m_activeHeldFlags.MobileHeld = GetHoldState(LineType.LineType_Mobile);
            m_activeHeldFlags.PCHeld = GetHoldState(LineType.LineType_VOIP);
            DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("Current Interface Hold States: PSTN: {0} Mobile: {1} VOIP: {2}",
                m_activeHeldFlags.DeskphoneHeld, m_activeHeldFlags.MobileHeld, m_activeHeldFlags.PCHeld));
        }

        private void GetActiveStates()
        {
            m_activeHeldFlags.DeskphoneActive = GetActiveState(LineType.LineType_PSTN);
            m_activeHeldFlags.MobileActive = GetActiveState(LineType.LineType_Mobile);
            m_activeHeldFlags.PCActive = GetActiveState(LineType.LineType_VOIP);
            DebugPrint(MethodInfo.GetCurrentMethod().Name, String.Format("Current Interface Is Line Active States: PSTN: {0} Mobile: {1} VOIP: {2}",
                m_activeHeldFlags.DeskphoneActive, m_activeHeldFlags.MobileActive, m_activeHeldFlags.PCActive));
        }

        private bool GetHoldState(LineType lineType)
        {
            bool state = false; // default - unknown state

            //Get the current hold state
            if (m_hostCommandExt!=null)
            {
                state = m_hostCommandExt.GetHoldState(lineType);
            }

            return state;
        }

        private bool GetActiveState(LineType lineType)
        {
            bool state = false; // default - unknown state

            //Get the current active state
            if (m_hostCommandExt != null)
            {
                state = m_hostCommandExt.IsLineActive(lineType);
            }

            return state;
        }

        // new get last docked status of device when app first runs
        private bool GetLastDockedStatus()
        {
            bool docked = false;
            try
            {
                if (m_hostCommandExt != null)
                {
                    m_lastdocked = m_hostCommandExt.IsHeadsetDocked;
                    docked = m_lastdocked;
                    if (docked) OnDocked(new DockedStateArgs(true, true));
                    else OnUnDocked(new DockedStateArgs(false, true));
                }
            }
            catch (Exception)
            {
                // probably we don't support docking, lets inform user...
                if (m_activeDevice.ProductName.Contains("BT300"))
                {
                    DeviceCapabilities.HasDocking = true; // updated, legend does have docking
                      // TODO Raise TT for IsHeadsetDocked should be implemented for BT300!
                    m_ignorenextundockedevent = true;
                    m_ignorenextbattlevevent = true;
                    m_lastdocked = DetectLegendDockedState(true);
                    docked = m_lastdocked;
                }
                else
                {
                    DeviceCapabilities.HasDocking = false;
                }
                OnCapabilitiesChanged(EventArgs.Empty);
            }
            return docked;
        }

        /// <summary>
        /// Instructs a mobile that is paired with Plantronics device to dial an outbound mobile call.
        /// </summary>
        /// <param name="numbertodial">The phone number you wish the mobile to call.</param>
        public void DialMobileCall(string numbertodial)
        {
            if (m_atdCommand != null)
            {
                m_atdCommand.MakeMobileCall(numbertodial);
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error, unable to dial mobile call. atd command is null.");
            }
        }

        /// <summary>
        /// Instructs a mobile that is paired with Plantronics device to answer an inbound (ringing) mobile call
        /// </summary>
        public void AnswerMobileCall()
        {
            if (m_atdCommand != null)
            {
                m_atdCommand.AnswerMobileCall();
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error, unable to answer mobile call. atd command is null.");
            }
        }

        /// <summary>
        /// Instructs a mobile that is paired with Plantronics device to end on ongoing mobile call
        /// </summary>
        public void EndMobileCall()
        {
            if (m_atdCommand != null)
            {
                m_atdCommand.EndMobileCall();
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error, unable to end mobile call. atd command is null.");
            }
        }

        private void GetInitialMobileCallStatus()
        {
            if (m_atdCommand != null)
            {
                try
                {
                    m_atdCommand.GetMobileCallStatus(); // are we on a call?

                    bool tmpHasCallerId = true; // device does support caller id feature

                    // LC temporarily hard-code some device capabilities
                    // e.g. fact that Blackwire C710/C720 do not support proximity, docking and is not wireless
                    string devname = m_devicename;
                    if (devname != null)
                    {
                        devname = devname.ToUpper();
                        if (devname.Contains("SAVI 7"))
                        {
                            tmpHasCallerId = false; // Savi 7xx does not support caller id feature
                        }
                        if (devname.Contains("BLACKWIRE"))
                        {
                            tmpHasCallerId = false; // Blackwire range does not support caller id feature
                        }
                        if (devname.Contains("C710") || devname.Contains("C720"))
                        {
                            tmpHasCallerId = false; // Blackwire 700 range does not support caller id feature
                        }
                    }

                    DeviceCapabilities.HasMobCallerId = tmpHasCallerId; // set whether device supports caller id feature
                    OnCapabilitiesChanged(EventArgs.Empty);
                }
                catch (System.Exception e)
                {
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: Exception occured getting mobile call status\r\nException = " + e.ToString());
                    DeviceCapabilities.HasMobCallerId = false;
                    OnCapabilitiesChanged(EventArgs.Empty);
                }
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Error, unable to get mobile status. atd command is null.");
                DeviceCapabilities.HasMobCallerId = false; // device does not support caller id feature
                OnCapabilitiesChanged(EventArgs.Empty);
            }
        }

        // new get last donned status of device when app first runs
        private void GetInitialDonnedStatus()
        {
            try
            {
                if (m_hostCommandExt != null)
                {
                    HeadsetState laststate = m_hostCommandExt.HeadsetState;
                    switch (laststate)
                    {
                        case HeadsetState.HeadsetState_Doff:
                            OnTakenOff(new WearingStateArgs(false, true));
                            break;
                        case HeadsetState.HeadsetState_Don:
                            OnPutOn(new WearingStateArgs(true, true));
                            break;
                    }
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Last donned state was: "+laststate);
                }
            }
            catch (Exception e)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Other Exception in GetInitialDonnedStatus(): " + e.ToString());
            }
        }

        private void GetInitialMuteStatus()
        {
            try
            {
                if (m_hostCommand != null)
                {
                    OnMuteChanged(new MuteChangedArgs(m_hostCommand.Mute));
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: Last mute state was: " + m_hostCommand.Mute);
                }
            }
            catch (Exception e)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Other Exception in GetInitialMuteStatus(): " + e.ToString());
            }
        }

        /// <summary>
        /// Allows your softphone application to inform Plantronics device about an incoming call. The Plantronics device will then automatically ring. 
        /// Note: will automatically open audio/rf link to wireless device.
        /// </summary>
        /// <param name="callid">A unique numeric identifier for the call that your application and Spokes will use to identify it as.</param>
        /// <param name="contactname">Optional caller's contact name that will display on Plantronics display devices, e.g. Calisto P800 and P240 devices.</param>
        /// <returns>Boolean indicating if command was issued successfully or not.</returns>
        public bool IncomingCall(int callid, string contactname = "")
        {
            bool success = false;
            try
            {
                if (m_comSession != null)
                {
                    ContactCOM contact = new ContactCOM() { Name = contactname };
                    CallCOM call = new CallCOM() { Id = callid };
                    m_comSession.CallCommand.IncomingCall(call, contact, RingTone.RingTone_Unknown, AudioRoute.AudioRoute_ToHeadset);
                    //ConnectAudioLinkToDevice(true);
                    success = true;
                }
            }
            catch (Exception) { success = false; }
            return success;
        }

        /// <summary>
        /// Allows your softphone application to inform Plantronics device about an outgoing call. Note: will automatically open audio/rf link to wireless device.
        /// </summary>
        /// <param name="callid">A unique numeric identifier for the call that your application and Spokes will use to identify it as.</param>
        /// <param name="contactname">Optional caller's contact name that will display on Plantronics display devices, e.g. Calisto P800 and P240 devices.</param>
        /// <returns>Boolean indicating if command was issued successfully or not.</returns>
        public bool OutgoingCall(int callid, string contactname = "")
        {
            bool success = false;
            try
            {
                if (m_comSession != null)
                {
                    ContactCOM contact = new ContactCOM() { Name = contactname };
                    CallCOM call = new CallCOM() { Id = callid };
                    m_comSession.CallCommand.OutgoingCall(call, contact, AudioRoute.AudioRoute_ToHeadset);
                    ConnectAudioLinkToDevice(true);
                    success = true;
                }
            }
            catch (Exception) { success = false; }
            return success;
        }

        /// <summary>
        /// Informs Spokes that user answered an incoming (ringing) softphone call using your softphone GUI
        /// </summary>
        /// <param name="callid">The unique numeric id that defines which softphone call was answered.</param>
        /// <returns>Boolean indicating if the command was called succesfully or not.</returns>
        public bool AnswerCall(int callid)
        {
            bool success = false;
            try
            {
                if (m_comSession != null)
                {
                    CallCOM call = new CallCOM() { Id = callid };
                    m_comSession.CallCommand.AnsweredCall(call);
                    success = true;
                }
            }
            catch (Exception) { success = false; }
            return success;
        }

        /// <summary>
        /// Informs Spokes that user and/or softphone placed a call on hold
        /// </summary>
        /// <param name="callid">The unique numeric id that defines which softphone call was placed on hold.</param>
        /// <returns>Boolean indicating if the command was called succesfully or not.</returns>
        public bool HoldCall(int callid)
        {
            bool success = false;
            try
            {
                if (m_comSession != null)
                {
                    CallCOM call = new CallCOM() { Id = callid };
                    m_comSession.CallCommand.HoldCall(call);
                    success = true;
                }
            }
            catch (Exception) { success = false; }
            return success;
        }

        /// <summary>
        /// Informs Spokes that user and/or softphone resumed a call from hold
        /// </summary>
        /// <param name="callid">The unique numeric id that defines which softphone call was resumed.</param>
        /// <returns>Boolean indicating if the command was called succesfully or not.</returns>
        public bool ResumeCall(int callid)
        {
            bool success = false;
            try
            {
                if (m_comSession != null)
                {
                    CallCOM call = new CallCOM() { Id = callid };
                    m_comSession.CallCommand.ResumeCall(call);
                    success = true;
                }
            }
            catch (Exception) { success = false; }
            return success;
        }

        /// <summary>
        /// Instructs Spokes to end an ongoing softphone call.
        /// </summary>
        /// <param name="callid">The unique numeric id that defines which softphone call you want to end.</param>
        /// <returns>Boolean indicating if the command was called succesfully or not.</returns>
        public bool EndCall(int callid)
        {
            bool success = false;
            try
            {
                if (m_comSession != null)
                {
                    CallCOM call = new CallCOM() { Id = callid };
                    m_comSession.CallCommand.TerminateCall(call);
                    success = true;
                }
            }
            catch (Exception) { success = false; }
            return success;
        }

        /// <summary>
        /// Instruct Spokes to tell us a serial number of the attached Plantronics device, i.e. headset or base/usb adaptor.
        /// </summary>
        /// <param name="serialNumberType">Allows you to say if you would like the headset or base/usb adaptor serial number.</param>
        public void RequestSingleSerialNumber(SerialNumberTypes serialNumberType)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: About to request serial number for: " + serialNumberType);
            try
            {
                if (m_hostCommandExt != null)
                {
                    switch (serialNumberType)
                    {
                        case SerialNumberTypes.Headset:
                            m_hostCommandExt.GetSerialNumber(DeviceType.DeviceType_Headset);
                            break;
                        case SerialNumberTypes.Base:
                            m_hostCommandExt.GetSerialNumber(DeviceType.DeviceType_Base);
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: serial number may not be supported on your device.");
            }
        }

        #region Call Control Helper Classes
        // Call abstraction
        public class CallCOM : Interop.Plantronics.CallId
        {
            private int id = 0;
            #region ICall Members

            public int ConferenceId
            {
                get { return 0; }
                set { }
            }

            public int Id
            {
                get { return id; }
                set { id = value; }
            }

            public bool InConference
            {
                get { return false; }
                set { }
            }

            #endregion
        }

        // Contact abstraction
        public class ContactCOM : Interop.Plantronics.Contact
        {
            private string email;
            private string friendlyName;
            private string homePhone;
            private int id;
            private string mobPhone;
            private string name;
            private string phone;
            private string sipUri;
            private string workPhone;
            #region IContact Members

            public string Email
            {
                get
                {
                    return email;
                }
                set
                {
                    email = value;
                }
            }

            public string FriendlyName
            {
                get
                {
                    return friendlyName;
                }
                set
                {
                    friendlyName = value;
                }
            }

            public string HomePhone
            {
                get
                {
                    return homePhone;
                }
                set
                {
                    homePhone = value;
                }
            }

            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    id = value;
                }
            }

            public string MobilePhone
            {
                get
                {
                    return mobPhone;
                }
                set
                {
                    mobPhone = value;
                }
            }

            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                }
            }

            public string Phone
            {
                get
                {
                    return phone;
                }
                set
                {
                    phone = value;
                }
            }

            public string SipUri
            {
                get
                {
                    return sipUri;
                }
                set
                {
                    sipUri = value;
                }
            }

            public string WorkPhone
            {
                get
                {
                    return workPhone;
                }
                set
                {
                    workPhone = value;
                }
            }

            #endregion
        }
        #endregion

        /// <summary>
        /// This function will establish or close the audio link between PC and the Plantronics audio device.
        /// It is required to be called where your app needs audio (i.e. when on a call) in order to support Plantronics wireless devices, because
        /// opening the audio link will also bring up the RF link.
        /// </summary>
        /// <param name="connect">Tells Spokes whether to open or close the audio/rf link to device</param>
        public void ConnectAudioLinkToDevice(bool connect)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Setting audio link active = " + connect.ToString());
            if (m_activeDevice != null)
            {
                m_activeDevice.HostCommand.AudioState =
                    connect ? AudioType.AudioType_MonoOn : AudioType.AudioType_MonoOff;
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: cannot set audio link state, no device");
            }
        }

        /// <summary>
        /// This function return true or false to indicate whether there is an active audio/rf
        /// link between PC and the device.
        /// </summary>
        public bool IsAudioLinkToDeviceActive()
        {
            bool isActive = false;
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Querying audio link active.");
            if (m_activeDevice != null)
            {
                isActive = m_activeDevice.HostCommand.AudioState == AudioType.AudioType_MonoOn;
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: cannot get audio link state, no device");
            }
            return isActive;
        }

        /// <summary>
        /// Set the microphone mute state of the attached Plantronics device.
        /// Note: For wireless devices mute only works when the audio/rf link is active (see also ConnectAudioLinkToDevice method).
        /// </summary>
        /// <param name="mute">A boolean indicating if you want mute on or off</param>
        public void SetMute(bool mute)
        {
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Setting mute = " + mute.ToString());
            if (m_activeDevice != null && m_hostCommandExt != null)
            {
                m_hostCommandExt.SetHeadsetMute(mute);
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: cannot set mute, no device");
            }
        }

        /// <summary>
        /// Get the microphone mute state of the attached Plantronics device.
        /// Note: For wireless devices mute only works when the audio/rf link is active (see also ConnectAudioLinkToDevice method).
        /// </summary>
        /// <returns>Boolean indicating if the headset is muted or not.</returns>
        public bool GetMute()
        {
            bool retval = false;
            DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Getting mute");
            if (m_activeDevice != null && m_hostCommandExt != null)
            {
                retval = m_hostCommand.Mute;
            }
            else
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "Spokes: INFO: cannot get mute, no device");
            }
            return retval;
        }

        /// <summary>
        /// Instruct the Plantronics multiline device to activate or deactivate the specified phone line.
        /// </summary>
        /// <param name="multiline_LineType">The line to activate or deactive, PC, Mobile or Desk Phone</param>
        /// <param name="activate">Boolean indicating whether to activate or de-activate the line</param>
        public void SetLineActive(Multiline_LineType multiline_LineType, bool activate)
        {
            if (m_hostCommandExt != null)
            {
                switch (multiline_LineType)
                {
                    case Multiline_LineType.PC:
                        m_hostCommandExt.SetActiveLink(LineType.LineType_VOIP, activate);
                        break;
                    case Multiline_LineType.Mobile:
                        m_hostCommandExt.SetActiveLink(LineType.LineType_Mobile, activate);
                        break;
                    case Multiline_LineType.Deskphone:
                        m_hostCommandExt.SetActiveLink(LineType.LineType_PSTN, activate);
                        break;
                }
            }
        }

        /// <summary>
        /// Instruct the Plantronics multiline device to place on hold or remove from hold the specified phone line.
        /// </summary>
        /// <param name="multiline_LineType">The line to place on hold or remove from hold, PC, Mobile or Desk Phone</param>
        /// <param name="hold">Boolean indicating whether to hold or un-hold the line</param>
        public void SetLineHold(Multiline_LineType multiline_LineType, bool hold)
        {
            if (m_hostCommandExt != null)
            {
                switch (multiline_LineType)
                {
                    case Multiline_LineType.PC:
                        m_hostCommandExt.Hold(LineType.LineType_VOIP, hold);
                        break;
                    case Multiline_LineType.Mobile:
                        m_hostCommandExt.Hold(LineType.LineType_Mobile, hold);
                        break;
                    case Multiline_LineType.Deskphone:
                        m_hostCommandExt.Hold(LineType.LineType_PSTN, hold);
                        break;
                }
            }
        }

        /// <summary>
        /// Used to send a custom message to the attahched Plantronics device
        /// NOTE: currently no way to check the response from headset
        /// as IDevice.DataRecieved is not exposed by SDK. Should be in future.
        /// </summary>
        /// <param name="message">The custom message to send as a string</param>
        public void SendCustomMessageToHeadset(string message)
        {
            try
            {
                if (m_activeDevice != null && m_hostCommand != null)
                {
                    IDeviceSettings devSet = m_hostCommand as IDeviceSettings;

                    byte[] outbuf = System.Text.Encoding.UTF8.GetBytes(message);

                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Sending custom message to headset = " + message);

                    devSet.SetData(CustomData.CustomData_RawHIDPipeData, outbuf);
                }
                else
                {
                    DebugPrint(MethodInfo.GetCurrentMethod().Name, "WARNING: Skipping sending custom message, no device connected!");
                }
            }
            catch (Exception e)
            {
                DebugPrint(MethodInfo.GetCurrentMethod().Name, "INFO: Exception sending custom message to headset:\r\n" + e.ToString());
            }
        }
    }
}
