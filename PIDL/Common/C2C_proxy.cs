﻿




// Generated by PIDL compiler.
// Do not modify this file, but modify the source .pidl file.

using System;
using System.Net;

            
using Nettention.Proud; 
namespace Game.Network.C2C
{
	public class Proxy:Nettention.Proud.RmiProxy
	{
public bool Movement(Nettention.Proud.HostID remote,Nettention.Proud.RmiContext rmiContext, UnityEngine.Vector3 position)
{
	Nettention.Proud.Message __msg=new Nettention.Proud.Message();
		__msg.SimplePacketMode = core.IsSimplePacketMode();
		Nettention.Proud.RmiID __msgid= Common.Movement;
		__msg.Write(__msgid);
		MyMarshaler.Write(__msg, position);
		
	Nettention.Proud.HostID[] __list = new Nettention.Proud.HostID[1];
	__list[0] = remote;
		
	return RmiSend(__list,rmiContext,__msg,
		RmiName_Movement, Common.Movement);
}

public bool Movement(Nettention.Proud.HostID[] remotes,Nettention.Proud.RmiContext rmiContext, UnityEngine.Vector3 position)
{
	Nettention.Proud.Message __msg=new Nettention.Proud.Message();
__msg.SimplePacketMode = core.IsSimplePacketMode();
Nettention.Proud.RmiID __msgid= Common.Movement;
__msg.Write(__msgid);
MyMarshaler.Write(__msg, position);
		
	return RmiSend(remotes,rmiContext,__msg,
		RmiName_Movement, Common.Movement);
}
#if USE_RMI_NAME_STRING
// RMI name declaration.
// It is the unique pointer that indicates RMI name such as RMI profiler.
public const string RmiName_Movement="Movement";
       
public const string RmiName_First = RmiName_Movement;
#else
// RMI name declaration.
// It is the unique pointer that indicates RMI name such as RMI profiler.
public const string RmiName_Movement="";
       
public const string RmiName_First = "";
#endif
		public override Nettention.Proud.RmiID[] GetRmiIDList() { return Common.RmiIDList; } 
	}
}

