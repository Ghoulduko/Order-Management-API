namespace Order.Application.Interfaces.Helper;

public interface IValidator<T>
{
    void Validate(T req);
}