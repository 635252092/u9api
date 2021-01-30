﻿





namespace U9Api.CustSV
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;
	using UFSoft.UBF.AopFrame; 	

	/// <summary>
	/// 成品入库审核 business operation
	/// 
	/// </summary>
	[Serializable]	
	public partial class RcvRptDocApproveSV
	{
	    #region Fields
		private System.String jsonRequest;
		
	    #endregion
		
	    #region constructor
		public RcvRptDocApproveSV()
		{}
		
	    #endregion

	    #region member		
		/// <summary>
		/// Json请求	
		/// 成品入库审核.Misc.Json请求
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
