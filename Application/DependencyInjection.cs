using MediatR;
//using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
//using RegistrationInBot.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            /*string path = typeof(RegistrationController).Assembly.Location;
            Assembly assembly = Assembly.LoadFrom(path);*/
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
