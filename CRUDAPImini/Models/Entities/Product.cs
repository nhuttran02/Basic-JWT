﻿namespace CRUDAPImini.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public decimal Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
