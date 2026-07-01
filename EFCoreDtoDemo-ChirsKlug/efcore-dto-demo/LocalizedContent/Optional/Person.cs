namespace LocalizedContent.Optional;

public class Person
{
    public string FirstName { get; }

    public Option<string> LastName { get; }

    private Person(string firstName, Option<string> lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    public static Person Create(string firstName, string lastName)
    {
        return new Person(firstName, Option<string>.Some(lastName));
    }

    public static Person Create(string firstName)
    {
        return new Person(firstName, Option<string>.None());
    }

    public static Person Create(string firstName, Option<string> lastName)
    {
        return new Person(firstName, lastName);
    }

    override public string ToString()
    {
        return LastName.Map(lastName => $"{FirstName} {lastName}")
            .Reduce(FirstName);
    }
}
