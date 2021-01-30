﻿





namespace U9Api.CustSV
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;
	using UFSoft.UBF.AopFrame; 	

	/// <summary>
	/// 弃审出货 business operation
	/// 
	/// </summary>
	[Serializable]	
	public partial class UnApproveShipSV
	{
	    #region Fields
		private System.String jsonRequest;
		
	    #endregion
		
	    #region constructor
		public UnApproveShipSV()
		{}
		
	    #endregion

	    #region member		
		/// <summary>
		/// Json请求	
		/// 弃审出货.Misc.Json请求
		/// </summary>
		/// <value></value>
		public System.String JsonRequest
		{
			get
			{
				return this.jsonRequest;
			}
			set
			{
				jsonRequest = value;
			}
		}
	    #endregion
		
	    #region do method 
		[Transaction(UFSoft.UBF.Transactions.TransactionOption.Supported)]
		[Logger]
		[Authorize]
		public System.String Do()
		{	
		    BaseStrategy selector = Select();	
				System.String result =  (System.String)selector.Execute(this);	
		    
			return result ; 
		}			
	    #endregion 					
	} 		
}
