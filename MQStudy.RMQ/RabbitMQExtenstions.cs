/*
* --------------------------------------------------
* Copyright(C)		: 
* Namespace			: MQStudy.RMQ
* FileName			: RabbitMQExtenstions.cs
* CLR Version		: 4.0.30319.42000
* Author			: Theo
* CreateTime		: 2020/12/17 10:54:49
* --------------------------------------------------
*/
using Microsoft.Extensions.Configuration;
using MQStudy.RMQ;

namespace Microsoft.Extensions.DependencyInjection
{
    ///<summary>
    /// RabbitMQExtenstions
    ///</summary>
    public static class RabbitMQExtenstions
    {
        public static IServiceCollection AddRabbitMQClient(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddRabbitMQClient(configuration.GetSection(RabbitMQOptions.Position).Get<RabbitMQOptions>());
        }

        public static IServiceCollection AddRabbitMQClient(this IServiceCollection services, RabbitMQOptions options)
        {
            RabbitMQClient.Init(options);
            services.AddScoped<IRabbitMQClient, RabbitMQClient>();
            return services;
        }
    }
}