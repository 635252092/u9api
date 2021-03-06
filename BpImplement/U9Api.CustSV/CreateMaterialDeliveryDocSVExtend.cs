﻿namespace U9Api.CustSV
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using U9Api.CustSV.Utils;
    using U9Api.CustSV.Model.Request;
    using UFIDA.U9.Base;
    using UFIDA.U9.CBO.Pub.Controller;
    using UFIDA.U9.ISV.MO;
    using UFSoft.UBF.AopFrame;
    using UFIDA.U9.MO.MO;
    using UFIDA.U9.IssueNew.MaterialDeliveryBP;
    using UFIDA.U9.IssueNew.IssueApplyBE;
    using UFIDA.U9.IssueNew.MaterialDeliveryDocBE;
    using UFSoft.UBF.Business;
    using System.Linq;
    using UFIDA.U9.CBO.Enums;
    using U9Api.CustSV.Base;

    /// <summary>
    /// CreateMaterialDeliveryDocSV partial 
    /// </summary>	
    public partial class CreateMaterialDeliveryDocSV
    {
        internal BaseStrategy Select()
        {
            return new CreateMaterialDeliveryDocSVImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
    internal partial class CreateMaterialDeliveryDocSVImpementStrategy : BaseStrategy
    {
        public CreateMaterialDeliveryDocSVImpementStrategy() { }

        public override object Do(object obj)
        {
            CreateMaterialDeliveryDocSV bpObj = (CreateMaterialDeliveryDocSV)obj;

            if (string.IsNullOrEmpty(bpObj.JsonRequest))
            {
                return JsonUtil.GetFailResponse("string.IsNullOrEmpty(bpObj.JsonRequest)");
            }
            CreateMaterialDeliveryDocRequest reqHeader = null;
            try
            {
                reqHeader = JsonUtil.GetJsonObject<CreateMaterialDeliveryDocRequest>(bpObj.JsonRequest);
                if (reqHeader == null)
                {
                    throw new Exception("request == null");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.GetFailResponse("JSON格式错误;" + ex.Message);
            }
            string res = "";
            StringBuilder debugInfo = new StringBuilder();
            debugInfo.AppendLine("strat...");
            debugInfo.AppendLine(JsonUtil.GetJsonString(Context.LoginOrg == null ? "Context.LoginOrg==null" : "Context.LoginOrg ok" + Context.LoginOrg.ID));

            using (UFSoft.UBF.Transactions.UBFTransactionScope scope = new UFSoft.UBF.Transactions.UBFTransactionScope(UFSoft.UBF.Transactions.TransactionOption.RequiresNew))
            {
                try
                {
                    DrawMaterialOutAndIn drawMaterialOutAndInProxy = new DrawMaterialOutAndIn();
                    bool isFirst = true;
                    List<MaterialInfo> list = new List<MaterialInfo>();
                    foreach (var reqLine in reqHeader.MOPickLines)
                    {
                        UFIDA.U9.MO.MO.MOPickList _srcPlist = MOPickList.Finder.Find(string.Format("MO.Org={0} and MO.DocNo='{1}' and DocLineNO={2}", Context.LoginOrg.ID, reqLine.SrcDocNo, reqLine.SrcLineNo));

                        if (_srcPlist == null)
                        {
                            return JsonUtil.GetFailResponse("未能获取到订单:" + reqLine.SrcDocNo + " 行号:" + reqLine.SrcDocNo + " 备料信息！", debugInfo);
                        }
                        if (isFirst)
                        {
                            #region MyRegion
                            //UFIDA.U9.Issue.MaterialDeliveryDocTypeBE.MaterialDeliveryDocType docType = UFIDA.U9.Issue.MaterialDeliveryDocTypeBE.MaterialDeliveryDocType.Finder.Find(string.Format("Org={0} and Code='{1}'", Context.LoginOrg.ID, reqHeader.DocTypeCode));
                            //if (docType == null)
                            //{
                            //    return JsonUtil.GetFailResponse("MaterialDeliveryDocType" + U9Contant.NoFindDocType, debugInfo);
                            //}
                            //if (docType.ConfirmType != UFIDA.U9.Base.Doc.ConfirmTypeEnum.ComfirmWork)
                            //{
                            //    return JsonUtil.GetFailResponse("请修改单据类型的确认方式为【确认作业】");
                            //}
                            #endregion

                            isFirst = false;
                            UFIDA.U9.Base.Doc.PushToDocTypeRuleLine.EntityList listPushToDocTypeRuleLine = UFIDA.U9.Base.Doc.PushToDocTypeRuleLine.Finder.FindAll($"PushToDocTypeRule.PushToDocTypeConfig.SourceEntity.Name='MO' and PushToDocTypeRule.PushToDocTypeConfig.TargetEntity.Name='MaterialDeliveryDoc' and SourceOrg={_srcPlist.MO.Org.ID} and EntityFieldValue3='{_srcPlist.MO.MODocType.ID.ToString()}'");
                            if (listPushToDocTypeRuleLine == null || listPushToDocTypeRuleLine.Count == 0)
                            {
                                throw U9Exception.GetException("单据类型为空，请检查【推式生单配置】中【组织】，【来源单据类型】（条件3）等是否填写正确;");
                            }
                            else if (listPushToDocTypeRuleLine.Count > 1)
                            {
                                throw U9Exception.GetException("【推式生单配置】：【来源单据类型】（条件3）同一组织下只能配置一个");
                            }
                            UFIDA.U9.Issue.MaterialDeliveryDocTypeBE.MaterialDeliveryDocType docType = UFIDA.U9.Issue.MaterialDeliveryDocTypeBE.MaterialDeliveryDocType.Finder.FindByID(listPushToDocTypeRuleLine[0].TargetDocType);
                            if (docType == null)
                            {
                                throw U9Exception.GetException("【推式生单配置】没有找到对应的【目标单据类型】");
                            }
                            if (docType.ConfirmType != UFIDA.U9.Base.Doc.ConfirmTypeEnum.ComfirmWork)
                            {
                                throw U9Exception.GetException("请修改材料出入库单据类型的确认方式为【确认作业】");
                            }
                            drawMaterialOutAndInProxy.MaterialDeliveryDocType = docType.Key;
                        }
                        if (_srcPlist.MO.TotalStartQty == 0)
                        {
                            throw U9Exception.GetException("工单:" + reqLine.SrcDocNo + " 未开工,不能创建领料单", debugInfo);
                        }

                        MaterialInfo materialInfoData = new MaterialInfo();
                        materialInfoData.MOPick = _srcPlist.Key;
                        materialInfoData.IssuedQty = reqLine.ItemQty;
                        //materialInfoData.DemandQty = _srcPlist.ActualReqQty;
                        materialInfoData.DemandQty = reqLine.ItemQty;
                        materialInfoData.SupplyWh = UFIDA.U9.CBO.SCM.Warehouse.Warehouse.FindByCode(Context.LoginOrg, reqLine.WHCode).Key;
                        materialInfoData.DemandOrg = Context.LoginOrg.Key;
                        list.Add(materialInfoData);
                    }

                    drawMaterialOutAndInProxy.SplitCondition = new UFIDA.U9.IssueNew.IssueBP.BatchIssueApplySplitCond();


                    drawMaterialOutAndInProxy.BatchMaterialOutDTOList = new List<MaterialInfo>();
                    drawMaterialOutAndInProxy.BatchMaterialOutDTOList = list;
                    drawMaterialOutAndInProxy.IsPush = true;
                    drawMaterialOutAndInProxy.IsIssue = true;
                    drawMaterialOutAndInProxy.DocDate = DateTime.Parse(reqHeader.BusinessDate);
                    List<MaterialDeliveryDoc.EntityKey> listIssueKeyDTO = drawMaterialOutAndInProxy.Do();

                    //sv.Do();
                    if (listIssueKeyDTO == null || listIssueKeyDTO.Count == 0)
                    {
                        throw U9Exception.GetException("材料出库创建失败，请检查生产订单、备料表是否异常", debugInfo);
                    }
                    else if (listIssueKeyDTO.Count > 1)
                    {
                        throw U9Exception.GetException("不支持同时出库多张生产订单！");
                    }
                    MaterialDeliveryDoc doc = listIssueKeyDTO[0].GetEntity();

                    using (ISession session = Session.Open())
                    {
                        doc.DocNo = reqHeader.WmsDocNo;
                        doc.Memo = reqHeader.Remark;
                        doc.BusinessDate = DateTime.Parse(reqHeader.BusinessDate);
                        doc.ConfirmDate = doc.BusinessDate;
                        HashSet<CreateMaterialDeliveryDocRequest.MOPickLine> hs = new HashSet<CreateMaterialDeliveryDocRequest.MOPickLine>();
                        foreach (var docLine in doc.MaterialDeliveryDocLines)
                        {
                            CreateMaterialDeliveryDocRequest.MOPickLine curLine = reqHeader.MOPickLines.Where(a => a.SrcDocNo == docLine.SourceDoc.SrcDocNo
                            && a.SrcLineNo == Convert.ToInt32(docLine.SourceDoc.SrcDocLineNo)
                            && !hs.Contains(a)
                            ).FirstOrDefault();
                            if (curLine == null)
                            {
                                continue;
                            }

                            hs.Add(curLine);
                            docLine.SupplyWh = UFIDA.U9.CBO.SCM.Warehouse.Warehouse.FindByCode(Context.LoginOrg, curLine.WHCode);
                            if (!string.IsNullOrEmpty(curLine.LotCode)
                                && CommonUtil.IsNeedLot(curLine.ItemCode, doc.Org.ID))
                            {
                                docLine.LotMaster = CommonUtil.GetLot(curLine.LotCode, curLine.ItemCode, curLine.WHCode, LotSnStatusEnum.M_ManufactureOut.Value);
                                docLine.LotNo = curLine.LotCode;
                            }
                            docLine.StorageType = StorageTypeEnum.Useable;
                        }
                        session.Commit();
                    }
                    if (reqHeader.IsAutoApprove)
                    {
                        U9Api.CustSV.ApproveMaterialDeliveryDocSV sv3 = new
                          ApproveMaterialDeliveryDocSV();
                        CommonApproveRequest req = new CommonApproveRequest();
                        req.DocNo = doc.DocNo;
                        sv3.JsonRequest = JsonUtil.GetJsonString(req);

                        res = sv3.Do();
                    }
                    else
                    {
                        res = JsonUtil.GetSuccessResponse(doc.DocNo, doc.DocState.Name, debugInfo);
                    }

                }
                catch (Exception ex)
                {
                    LogUtil.WriteDebugInfoLog(debugInfo.ToString());
                    scope.Rollback();
                    //throw Base.U9Exception.GetInnerException(ex);
                    return JsonUtil.GetFailResponse(Base.U9Exception.GetInnerException(ex).Message, debugInfo);
                }
                scope.Commit();
                //if (beginConfirmType != docType.ConfirmType)
                //{
                //    using (ISession session = Session.Open())
                //    {
                //        docType.ConfirmType = beginConfirmType;
                //        session.Commit();
                //    }
                //}

                return res;
            }
        }
    }

    #endregion


}