using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway.API.Validation;
using PaymentGateway.Application.Models;

namespace PaymentGateway.API.V1.Payments
{
    public class PostPaymentModel
    {
        [CreditCard, Required]
        public string CardNumber { get; set; }

        [Range(1, 12), Required]
        public int MonthExpired { get; set; }

        [Range(2020, 2030), Required]
        public int YearExpired { get; set; }

        [MinLength(3), MaxLength(3), Required]
        public string Ccv { get; set; }

        [Range(0, 10000), IsNotDefault, Required]
        public decimal Amount { get; set; }

        [IsNotDefault, Required]
        public Currency Currency { get; set; }
    }
}
