﻿





namespace U9Api.CustSV
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;
	using UFSoft.UBF.AopFrame; 	

	/// <summary>
	/// 销售退回收货 business operation
	/// 
	/// </summary>
	[Serializable]	
	public partial class CreateRcvFromRMRSV
	{
	    #region Fields
		private System.String jsonRequest;
		
	    #endregion
		
	    #region constructor
		public CreateRcvFromRMRSV()
		{}
		
	    #endregion

	    #region member		
		/// <summary>
		/// Json请求	
		/// 销售退回收货.Misc.Json请求
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
		[Transaction(UFSoft.UBF.Transactions.TransactionOption.Required)]
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
