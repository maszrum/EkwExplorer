namespace EkwExplorer.Core
{
    public interface ISqlQueries
    {
        string AddBook { get; }
        string AddProperty { get; }
        string GetRandomNotFilledBook { get; }
        string IsAnyNotFilled { get; }
        string UpdateBook { get; }
    }
}