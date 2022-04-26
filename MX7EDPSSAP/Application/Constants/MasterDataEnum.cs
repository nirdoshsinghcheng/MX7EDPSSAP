using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MX7EDPSSAP.Helpers.EnumHelper;

namespace MX7EDPSSAP.Application.Constants
{
    public class MasterDataEnum
    {
        public enum MasterDataTableName
        {
            [EnumCode("Aisle")]
            Aisle,

            [EnumCode("Container Type")]
            ContainerType,

            [EnumCode("Dolly")]
            Dolly,

            [EnumCode("Location")]
            Location,

            [EnumCode("Product Type")]
            ProductType,

            [EnumCode("Role")]
            Role,

            [EnumCode("Route")]
            Route,

            [EnumCode("SKU")]
            Sku,

            [EnumCode("SKU UOM")]
            SkuUom,

            [EnumCode("Status")]
            Status,

            [EnumCode("Store")]
            Store,
        }
    }
}