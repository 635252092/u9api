﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U9Api.CustSV.Model.Request
{
    class CreateMaterialInRequest
    {
        public string WmsDocNo { get; set; }
        public string BusinessDate { get; set; }
        public bool IsAutoApprove { get; set; }
        public string Remark { get; set; }
        public List<MaterialDeliveryDocLine> MaterialDeliveryDocLines { get; set; }
        public string DocTypeCode { get; set; }
        public class MaterialDeliveryDocLine
        {
            public string MoDocNo { get; set; }

            public string SrcDocNo { get; set; }
            public string WHCode { get; set; }
            //public int SrcLineNo { get; set; }
            public string LotCode { get; set; }
            public decimal ItemQty { get; set; }
            public string ItemCode { get; set; }
        }
    }
}
