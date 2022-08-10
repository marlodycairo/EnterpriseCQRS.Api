using EnterpriseCQRS.Data;
using EnterpriseCQRS.Data.Model;
using EnterpriseCQRS.Domain.Commands.ProductCommand;
using EnterpriseCQRS.Domain.Responses;
using EnterpriseCQRS.Services.CommandHandlers.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnterpriseCQRS.Services.CommandHandlers.ProductCommandHandler
{
    public class ProductCommandHandler
    {
        public class GetTransactionCommandHandler : IRequestHandler<GetTransactionCommand, GenericResponse<IList<Transaction>>>
        {
            private readonly CommittedCapacityContext _context;
            private readonly ILogger logger;

            public GetTransactionCommandHandler(CommittedCapacityContext context, ILogger<GetTransactionCommandHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<GenericResponse<IList<Transaction>>> Handle(GetTransactionCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("comienza a ejecutar el handler");
                var url = new Uri("http://quiet-stone-2094.herokuapp.com/transactions.json");
                var response = new GenericResponse<IList<Transaction>>();
                var transactions = new Utilities<Transaction>();

                logger.LogWarning("se realiza proceso de eliminado de info de la tabla");
                _context.Database.ExecuteSqlRaw("DELETE FROM [Transaction]");
                logger.LogWarning("Termino el proceso de eliminado de info de la tabla");

                logger.LogWarning("se realiza proceso de consumir servicio externo");
                var responses = await transactions.ExternalServiceUtility(url);
                logger.LogWarning("Termino el proceso de eliminado de info de la tabla");

                if (responses.Result is null)
                {
                    responses.Message = " el servicio externo no devolvio datos";
                    return responses;
                }

                logger.LogWarning("se realiza guardado de info en la tabla");
                await _context.Transaction.AddRangeAsync(responses.Result, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                logger.LogWarning("termino proceso de  guardado de info en la tabla");
                response.Message = "Guardado exitoso";
                //response.Result = responses.Result;

                return response;
            }
        }

        public class GetRateCommandHandler : IRequestHandler<GetRateCommand, GenericResponse<IList<Rates>>>
        {
            private readonly CommittedCapacityContext _context;
            private readonly ILogger<GetRateCommandHandler> logger;

            public GetRateCommandHandler(CommittedCapacityContext context, ILogger<GetRateCommandHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<GenericResponse<IList<Rates>>> Handle(GetRateCommand request, CancellationToken cancellationToken)
            {
                logger.LogInformation("comienza a ejecutar el handler");
                var url = new Uri("http://quiet-stone-2094.herokuapp.com/rates.json");
                var response = new GenericResponse<IList<Rates>>();
                var rates = new Utilities<Rates>();

                logger.LogInformation("se realiza proceso de eliminado de info de la tabla");
                _context.Database.ExecuteSqlRaw("DELETE FROM [Rates]");
                logger.LogInformation("Termino el proceso de eliminado de info de la tabla");

                logger.LogInformation("se realiza proceso de consumir servicio externo");
                var responses = await rates.ExternalServiceUtility(url);
                logger.LogInformation("Termino el proceso de eliminado de info de la tabla");

                if (responses.Result is null)
                {
                    responses.Message = " el servicio externo no devolvio datos";
                    return responses;
                }

                logger.LogInformation("se realiza guardado de info en la tabla");
                await _context.Rates.AddRangeAsync(responses.Result, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                logger.LogInformation("termino proceso de  guardado de info en la tabla");

                response.Message = "Guardado exitoso";
                //response.Result = responses.Result;
                return response;
            }
        }

        public class CalculateTransactionCommandHandler : IRequestHandler<CalculateTransactionCommand, GenericResponse<IList<Total>>>
        {
            private readonly CommittedCapacityContext _context;

            public CalculateTransactionCommandHandler(CommittedCapacityContext context)
            {
                _context = context;
            }

            public async Task<GenericResponse<IList<Total>>> Handle(CalculateTransactionCommand request, CancellationToken cancellationToken)
            {
                var response = new GenericResponse<IList<Total>>();
                var rates = await _context.Rates.ToListAsync();
                var transactions = await _context.Transaction.Where(p => p.Sku == request.Sku).ToListAsync();
                
                var conversionMoneda = GetConversiones(rates, transactions);
                
                return response;
            }

            private static List<Conversiones> GetConversiones(List<Rates> rates, List<Transaction> transactions)
            {
                var conversiones = new List<Conversiones>();


                return conversiones;
            }
        }
    }
}
