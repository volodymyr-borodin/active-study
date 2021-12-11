using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveStudy.Domain
{
    public interface ICountryStorage
    {
        Task<Country> GetByCodeAsync(string countryCode);
        Task<IEnumerable<Country>> SearchAsync();
    }

    public class InMemoryCountryStorage : ICountryStorage
    {
        private static readonly IEnumerable<Country> Countries = new List<Country>
        {
            new Country("Україна", "UA"),
            new Country("Россия", "RU")
        };

        public Country Default { get; }

        public Task<Country> GetByCodeAsync(string countryCode)
        {
            return Task.FromResult(Countries.FirstOrDefault(c => c.Code == countryCode));
        }

        public Task<IEnumerable<Country>> SearchAsync()
        {
            return Task.FromResult(Countries);
        }
    }
}