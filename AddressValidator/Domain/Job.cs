using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AddressValidator.Infrastructure;
using AddressValidator.Infrastructure.Clients;
using AddressValidator.Infrastructure.Models;
using AddressValidator.Infrastructure.Models.AppSettings;
using Microsoft.Extensions.Options;

namespace AddressValidator.Domain
{
    public class Job : IJob
    {
        private readonly IGoogleService _googleService;
        private readonly IAddressBuilder _addressBuilder;
        private readonly FileLocations _fileLocations;

        private readonly string[] _validExtensions = { ".csv", ".txt" };

        public Job(IGoogleService googleService,
            IAddressBuilder addressBuilder,
            IOptions<FileLocations> fileLocationOptions)
        {
            _googleService = googleService;
            _addressBuilder = addressBuilder;
            _fileLocations = fileLocationOptions.Value;

        }

        public async Task RunTask()
        {
            if (string.IsNullOrWhiteSpace(_fileLocations.InputFilePath) || !File.Exists(_fileLocations.InputFilePath))
                throw new ArgumentNullException(
                    nameof(_fileLocations.InputFilePath),
                    "Your InputFile does not exists. Please, specify valid file names in the appSettings.json file");

            if (string.IsNullOrWhiteSpace(_fileLocations.OutputFilePath) || !File.Exists(_fileLocations.OutputFilePath))
                throw new ArgumentNullException(
                    nameof(_fileLocations.OutputFilePath),
                    "Your OutputFile does not exists.  Please, specify valid file names in the appSettings.json file");

            if(!_validExtensions.Contains(Path.GetExtension(_fileLocations.InputFilePath)))
                throw new ArgumentException(
                    "Your InputFile is not correct. Only .txt and .csv files are allowed");

            if (!_validExtensions.Contains(Path.GetExtension(_fileLocations.OutputFilePath)))
                throw new ArgumentException(
                    "Your OutputFile is not correct. Only .txt and .csv files are allowed");

            string[] lines = File.ReadLines(_fileLocations.InputFilePath).ToArray();

            foreach (string inputAddress in lines)
            {
                var inputAddressValid = inputAddress.Trim('"', ',', ';');

                GoogleResponse googleResponse = await _googleService.ValidateAsync(inputAddressValid);
                string validatedAddress = _addressBuilder.BuildAddressString(googleResponse);

                Console.WriteLine($"{inputAddressValid};{validatedAddress}");

                using (StreamWriter streamWriter =
                    new StreamWriter(_fileLocations.OutputFilePath, true))
                {
                    streamWriter.WriteLine($"{inputAddressValid};{validatedAddress}");
                }
            }
        }

        private void ChecksAndDisclaimers() { }
    }
}
