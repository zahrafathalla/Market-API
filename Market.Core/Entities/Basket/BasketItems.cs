﻿namespace Market.Core.Entities.Basket
{
    public class BasketItems
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
    }
}