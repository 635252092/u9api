﻿







namespace U9Api.CustSV
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.Runtime.Serialization;
	using System.IO;
	using UFSoft.UBF.Util.Context;
	using UFSoft.UBF;
	using UFSoft.UBF.Exceptions;
	using UFSoft.UBF.Service.Base ;

    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.UFIDA.org", Name="U9Api.CustSV.IPullShip2SV")]
    public interface IPullShip2SV
    {
		[ServiceKnownType(typeof(ApplicationContext))]
		[ServiceKnownType(typeof(PlatformContext))]
		[ServiceKnownType(typeof(ThreadContext))]
		[ServiceKnownType(typeof( UFSoft.UBF.Business.BusinessException))]
		[ServiceKnownType(typeof( UFSoft.UBF.Business.EntityNotExistException))]
		[ServiceKnownType(typeof( UFSoft.UBF.Business.AttributeInValidException))]
		[ServiceKnownType(typeof(UFSoft.UBF.Business.AttrsContainerException))]
		[ServiceKnownType(typeof(UFSoft.UBF.Exceptions.MessageBase))]
			[FaultContract(typeof(UFSoft.UBF.Service.ServiceLostException))]
		[FaultContract(typeof(UFSoft.UBF.Service.ServiceException))]
		[FaultContract(typeof(UFSoft.UBF.Service.ServiceExceptionDetail))]
		[FaultContract(typeof(ExceptionBase))]
		[FaultContract(typeof(Exception))]
		[OperationContract()]
        System.String Do(IContext context ,out IList<MessageBase> outMessages ,System.String jsonRequest);
    }

    [UFSoft.UBF.Service.ServiceImplement]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class PullShip2SVStub : ServiceStubBase, IPullShip2SV
    {
        #region IPullShip2SV Members

        //[OperationBehavior]
        public System.String Do(IContext context ,out IList<MessageBase> outMessages, System.String jsonRequest)
        {
			
			ICommonDataContract commonData = CommonDataContractFactory.GetCommonData(context, out outMessages);
			return DoEx(commonData, jsonRequest);
        }
        
        //[OperationBehavior]
        public System.String DoEx(ICommonDataContract commonData, System.String jsonRequest)
        {
			this.CommonData = commonData ;
            try
            {
                BeforeInvoke("U9Api.CustSV.PullShip2SV");                
                PullShip2SV objectRef = new PullShip2SV();
	
				objectRef.JsonRequest = jsonRequest;

				//处理返回类型.
				System.String result = objectRef.Do();
				return result ;
						return result;

	        }
			catch (System.Exception e)
            {
				DealException(e);
				throw;
            }
            finally
            {
				FinallyInvoke("U9Api.CustSV.PullShip2SV");
            }
        }
	#endregion
    }
}
