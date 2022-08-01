﻿using EnterpriseCQRS.Data.Model;
using EnterpriseCQRS.Domain.Responses;
using System.Collections.Generic;

namespace EnterpriseCQRS.Domain.Commands.ProductCommand
{
    public class CalculateTransactionCommand : BaseCommand<GenericResponse<IList<Total>>>
    {
        public string Sku { get; set; }
    }
}
