namespace ActiveStudy.Domain
{
    public class Country
    {
        public string Name { get; }

        public string Code { get; }

        public Country(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}