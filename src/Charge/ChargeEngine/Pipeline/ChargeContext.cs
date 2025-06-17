using Modular.Charge.ChargeEngine.Messaging.Models;
using Modular.Charge.Domain.Entities;

namespace Modular.Charge.ChargeEngine.Pipeline;

public class ChargeContext
{
    public ChargeRequest Request { get; }
    public Sale? Sale { get; set; }
    public Transaction? Transaction { get; set; }
    public string? PaymentId { get; set; }
    public Dictionary<string, object> Data { get; } = new();
    public List<string> Errors { get; set; } = new();
    public bool IsValid => !Errors.Any();

    public ChargeContext(ChargeRequest request)
    {
        Request = request;
    }

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public T? GetData<T>(string key)
    {
        if (Data.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return default;
    }

    public void SetData<T>(string key, T value)
    {
        Data[key] = value!;
    }
} 