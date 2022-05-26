
namespace MX7EDPSSAP.Model
{
    public class RawData
    {
        public string DCCODE { get; set; }
        public string OrderDate { get; set;}
        public string B1 { get; set;}
        public string B2 { get; set; }
        public string B3 { get; set; }
        public string DeliveryDate { get; set; }
        public string StoreCode { get; set;}
        public string Name1 { get; set;}
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string Citytext { get; set; }
        public string StoreAddress { get; set; }
        public string Region { get; set; }
        public string STO { get; set;} 
        public string STOLine { get; set;}
        public string Article { get;set;} 
        public string Name { get; set;}
        public string CLD { get; set; }
        public string CLDEA { get; set; }
        public string Ea { get; set; }
        public string IN { get; set; }
        public string INEA { get; set; }
        public string Each { get; set; }
        public string EAN { get; set; }
        public string EANUOM { get; set; }
        public string EAN1 { get; set; }
        public string EANUOM1 { get; set; }
        public string EAN2 { get; set; }
        public string EANUOM2 { get; set; }
        public string EAN3 { get; set; }
        public string EANUOM3 { get; set; } 
        public string EAN4 { get; set; }
        public string EANUOM4 { get; set; }
        public string EAN5 { get; set; }
        public string EANUOM5 { get; set; }
        public string AUX1 { get; set; }
        public string AUX2 { get; set; }
        public string Route { get; set; }
        public string Stop { get; set; }
        public string Priority { get; set; }
        public string StorageCondition { get; set; }
        public string Wave { get; set; }


    }

    public class SOHRawData
    {
        public string Location { get; set; }
        public string itemCode { get; set; }
        public string quantity { get; set; }
        public string AreaCode { get; set; }
        public string UoM { get; set; }
    }

    public class PutData
    {
        public string order_no { get; set; }
        public string order_date { get; set; }
        public string put_date { get; set; }
        public string store_code { get; set; }
        public string upc { get; set; }
        public string podo_no { get; set; }
        public string qty_ordered { get; set; }
        public string qty_put { get; set; }
        public string sku { get; set; }
    }

    public class PutDetailData
    {
        public string order_no { get; set; }
        public string order_date { get; set; }
        public string put_date { get; set; }
        public string store_code { get; set; }
        public string upc { get; set; }
        public string podo_no { get; set; }
        public string qty_ordered { get; set; }
        public string qty_put { get; set; }
        public string sku { get; set; }
        public string tote_id { get; set; }
        public string seal_id { get; set; }
        public string tote_label { get; set; }
        public string AUX_1 { get; set; }
        public string AUX_2 { get; set; }
        public string AUX_3 { get; set; }
        public string AUX_4 { get; set; }
        public string AUX_5 { get; set; }
    }

}
