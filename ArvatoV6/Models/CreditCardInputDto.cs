﻿using System;
namespace ArvatoV6.Models
{
    public class CreditCardInputDto
    {
        public string CardHolder { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
    }
}

