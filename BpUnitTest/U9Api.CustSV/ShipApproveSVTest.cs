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
	public class ShipApproveSVTest
	{
		private Proxy.ShipApproveSVProxy obj = new Proxy.ShipApproveSVProxy();

		public ShipApproveSVTest()
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