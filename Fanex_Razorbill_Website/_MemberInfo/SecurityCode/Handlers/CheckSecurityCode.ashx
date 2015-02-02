<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SecurityCode.CheckSecurityCode_Handler" %>
/*
* Created 20130415@kan: Handler for change security code
* Revision ?@? - ...
*
*/
namespace Sunplus.Agent.UI._MemberInfo.SecurityCode
{
	using System;
	using System.Web;
	using Fanex.Age.UI;

	public class CheckSecurityCode_Handler : Fanex.Age.UI.TplHandler
	{
		public CheckSecurityCode_Handler()
		{ }
		
		public override void RenderResult(ref Fanex.Age.UI.JSONData data)
		{
			Result r = View as Result;
			data.AsResult(r);
		}

		public override void ProcessRequest(ref System.Collections.Hashtable p)
		{ }

		public override object ValidateRequest(System.Collections.Hashtable p)
		{
			return null;
		}

		public override object InvokeData(System.Collections.Hashtable p)
		{
			return null;
		}
	}
}