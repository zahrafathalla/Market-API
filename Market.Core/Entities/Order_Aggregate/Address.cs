﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Entities.Order_Aggregate
{
    public class Address
    {
        private Address() { }
        public Address(string firstName, string lastName, string street, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; }= null!;
        public string Street { get; set; } = null!;
        public string City { get; set; }=null!;
        public string Country { get; set; } = null!;


    }
}
