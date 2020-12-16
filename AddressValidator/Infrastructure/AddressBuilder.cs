using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressValidator.Infrastructure.Models;

namespace AddressValidator.Infrastructure
{
    public class AddressBuilder: IAddressBuilder
    {
        private string Exception = "Київ";

        public string BuildAddressString(GoogleResponse googleResponse)
        {
            if (googleResponse == null)
                return string.Empty;

            Address address = BuildAddress(googleResponse);

            var sb = new StringBuilder();

            List<string> componentsList = new List<string>();

            componentsList.Add(address.AdministrativeAreaLevel1);
            if (!string.IsNullOrWhiteSpace(address.AdministrativeAreaLevel2) &&
                !address.AdministrativeAreaLevel2.Contains(Exception))
            {
                componentsList.Add(address.AdministrativeAreaLevel2);
            }
            componentsList.Add(address.Locality);
            componentsList.Add(address.Route);
            componentsList.Add(address.StreetNumber);
            componentsList.Add(address.Subremise);

            var componentsArray = componentsList
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (!componentsArray.Any())
                return string.Empty;

            sb.AppendJoin(", ", componentsArray);

            return sb.ToString();
        }

        private Address BuildAddress(GoogleResponse googleResponse)
        {
            var address = new Address();

            AddressComponent[] addressComponents = googleResponse
                .Results?
                .FirstOrDefault()?
                .AddressComponents;

            if (addressComponents == null || !addressComponents.Any())
                return address;

            foreach (AddressComponent component in addressComponents)
            {
                string type = component.Types?.FirstOrDefault();

                switch (type?.ToLowerInvariant())
                {
                    case "street_number":
                        address.StreetNumber = component.LongName;
                        break;
                    case "subpremise":
                        address.Subremise = component.LongName;
                        break;
                    case "route":
                        address.Route = component.LongName;
                        break;
                    case "locality":
                        address.Locality = component.LongName;
                        break;
                    case "administrative_area_level_1":
                        address.AdministrativeAreaLevel1 = component.LongName;
                        break;
                    case "administrative_area_level_2":
                        address.AdministrativeAreaLevel2 = component.LongName;
                        break;
                }
            }

            return address;
        }
    }
}
