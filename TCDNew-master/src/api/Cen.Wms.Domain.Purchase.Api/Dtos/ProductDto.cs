﻿namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Abc { get; set; }
        public string[] Barcodes { get; set; }
    }
}
