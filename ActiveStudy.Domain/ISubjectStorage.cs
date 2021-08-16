using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveStudy.Domain
{
    public interface ISubjectStorage
    {
        Task<Subject> GetByIdAsync(string id);
        Task<IEnumerable<Subject>> SearchAsync(string countryCode);
        Task<IEnumerable<Subject>> SearchAsync(IEnumerable<string> subjects);
    }

    public class InMemorySubjectStorage : ISubjectStorage
    {
        private static Dictionary<string, IEnumerable<Subject>> CountrySubjects => new Dictionary<string, IEnumerable<Subject>>
        {
            { "EN", EnglishSubjects },
            { "UA", UkraineSubjects }
        };

        private static readonly IEnumerable<Subject> EnglishSubjects = new List<Subject>
        {
            new Subject("en-geography", "Geography"),
            new Subject("en-history", "History"),
            new Subject("en-literacy", "Literacy"),
            new Subject("en-music", "Music"),
            new Subject("en-mathematics", "Mathematics")
        };

        private static readonly IEnumerable<Subject> UkraineSubjects = new List<Subject>
        {
            new Subject("ua-ukrainska-mova", "Українська мова"),
            new Subject("ua-ukrainska-literatyra", "Українська література"),
            new Subject("ua-istoriya", "Історія"),
            new Subject("ua-matematika", "Математика"),
            new Subject("ua-algebra", "Алгебра"),
            new Subject("ua-geometrіya", "Геометрія"),
            new Subject("ua-fizika", "Фізика"),
            new Subject("ua-himiya", "Хімія"),
            new Subject("ua-biologiya", "Біологія"),
            new Subject("ua-informatika", "Інформатика"),
            new Subject("ua-anglіjska-mova", "Іноземна мова (Англійська мова)"),
            new Subject("ua-nіmecka-mova", "Іноземна мова (Німецька мова)"),
            new Subject("ua-francuzka-mova", "Іноземна мова (Французька мова)"),
            new Subject("ua-ispanska-mova", "Іноземна мова (Іспанська мова)"),
            new Subject("ua-geografіya", "Географія"),
            new Subject("ua-zarubіzhna-lіteratura", "Зарубіжна література"),
            new Subject("ua-mustectvo", "Мистецтво"),
            new Subject("ua-trudove-navchannya", "Трудове навчання"),
            new Subject("ua-prurodoznavstvo", "Природознавство"),
            new Subject("ua-osnovu-zdorovya", "Основи здоров'я"),
            new Subject("ua-fіzichna-kultura", "Фізична культура"),
            new Subject("ua-pravoznavstvo", "Правознавство")
        };

        private static readonly IEnumerable<Subject> Subjects = EnglishSubjects.Concat(UkraineSubjects);

        public Task<Subject> GetByIdAsync(string id)
        {
            return Task.FromResult(Subjects.FirstOrDefault(s => s.Id == id));
        }

        public Task<IEnumerable<Subject>> SearchAsync(string countryCode)
        {
            return Task.FromResult(CountrySubjects.ContainsKey(countryCode) ? CountrySubjects[countryCode] : Enumerable.Empty<Subject>());
        }

        public Task<IEnumerable<Subject>> SearchAsync(IEnumerable<string> subjects)
        {
            return Task.FromResult(Subjects.Where(s => subjects.Contains(s.Id)));
        }
    }
}