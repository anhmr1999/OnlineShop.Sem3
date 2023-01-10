using Shop.EntityFramework.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Web.Common.ObjectRequests
{
    public class BillObject
    {
        public string Code { get; set; }
        public int Quantity { get; set; }     
    }
    public class BillEditObject
    {
        public Guid Id { get; set; }
        public BillStatusEnum Status { get; set; }
    }
}