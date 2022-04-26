using System.Collections.Generic;

namespace MX7EDPSSAP.Application.Models.Response
{
    public class GetAvailAisleForPrePutResponse
    {
        public int skuId { get; set; }

        public string skuCode { get; set; }

        public string totalRequiredQty { get; set; }

        public string totalAllocQty { get; set; }

        public string totalRemainingQty { get; set; }

        public List<AvailableZonesForPreput> availableZones { get; set; } = new List<AvailableZonesForPreput>();
    }

    public class AvailableZonesForPreput
    {
        public int zoneId { get; set; }

        public string zoneCode { get; set; }

        public List<SkuUomForPreput> skuUom { get; set; } = new List<SkuUomForPreput>();
    }

    public class SkuUomForPreput
    {
        public int skuUomId { get; set; }

        public string skuUomCode { get; set; }

        public string baseUomConversionString { get; set; }

        public int requiredQty { get; set; }
    }
}