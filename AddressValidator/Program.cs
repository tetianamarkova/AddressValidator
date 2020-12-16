using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AddressValidator
{
    using Domain;
    using Infrastructure;
    using Infrastructure.Clients;
    using Infrastructure.Models.AppSettings;

    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = null;

            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json", true, true)
                    .Build();

                ServiceCollection services = new ServiceCollection();

                ConfigureService(configuration, services);

                serviceProvider = services.BuildServiceProvider();

                if (serviceProvider == null)
                    throw new ArgumentNullException(nameof(serviceProvider));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            WriteDisclaimer();

            try
            {
                IJob job = serviceProvider.GetService<IJob>();

                if (job == null) 
                    return;

                var jobTask = job.RunTask();
                jobTask.Wait(); 
                    
                if (jobTask.IsFaulted)
                    Console.WriteLine("Job was faulted");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void ConfigureService(IConfiguration configuration, ServiceCollection services)
        {
            services.AddSingleton(configuration);

            IConfigurationSection googleSettingsSection = configuration.GetSection("GoogleSettings");

            services.Configure<GoogleSettings>(googleSettingsSection);
            services.Configure<FileLocations>(configuration.GetSection("FileLocations"));

            services.AddScoped<IAddressBuilder, AddressBuilder>();
            services.AddScoped<IJob, Job>();

            services.AddHttpClient<IGoogleService, GoogleService>()
                .ConfigureHttpClient((provider, client) =>
                {
                    var settings = new GoogleSettings();
                    googleSettingsSection.Bind(settings);
                    client.BaseAddress = new Uri(settings.BaseUrl);
                });
        }

        private static void WriteDisclaimer()
        {
            Console.WriteLine("Вітаю, друже!");
            Console.WriteLine();
            Console.WriteLine("Перед тим, як розпочати роботу, переконайся що вхідний та вихідний файли правильні та шляхи до них правильно прописані в appSettings.config файлі");
            Console.WriteLine("Вхідний файл має містити лише адреси, які треба провалідувати. Кожен адрес має бути на окремому рядку.");
            Console.WriteLine("Також вхідний файл НЕ має містити жодного іншого тексту (наприклад заголовків CSV), окрім безпосередньо адрес");
            Console.WriteLine("Будь ласка, переконайся що вихідний файл пустий, адже програма буде продовжувати дозапис в цей файл");
            Console.WriteLine("Зроби собі чашку чаю або кави та отримай насолоду від валідації адрес. Бажаю приємного проведення часу!");
            Console.WriteLine();
            Console.WriteLine("Нажми будь-яку кнопку аби продовжити...");

            Console.ReadKey();
        }
    }
}
