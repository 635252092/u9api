﻿

namespace U9Api.CustSV
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	using NUnit.Framework;
	
	/// <summary>
	/// Business operation test
	/// </summary> 
	[TestFixture]		
	public class RcvRptDocApproveSVTest
	{
		private Proxy.RcvRptDocApproveSVProxy obj = new Proxy.RcvRptDocApproveSVProxy();

		public RcvRptDocApproveSVTest()
		{
		}
		#region AutoTestCode ...
		[Test]
		public void TestDo()
		{
			obj.Do() ;  
		
		}
		#endregion 				
	}
	
}