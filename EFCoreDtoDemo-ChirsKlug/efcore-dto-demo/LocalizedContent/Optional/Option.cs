namespace LocalizedContent.Optional;

//public class Option<T> where T : class
//{
//    public Option(T value)
//    {
//        Value = value;
//    }
//    public T Value { get; }
//    public static implicit operator Option<T>(T value) => new(value);
//    public static implicit operator T(Option<T> option) => option.Value;
//    public static Option<T> None => null;
//}

public class Option<T> where T : class
{
    private T? _value;

    private Option() { }

    public static Option<T> Some(T obj) => new() { _value = obj };
    public static Option<T> None() => new();

    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class
    {
        var result = new Option<TResult>() { _value = _value is not null ? map(_value) : null };

        return result;
    }

    public T Reduce(T orElse)
    {
        return _value ?? orElse;
    }
}

