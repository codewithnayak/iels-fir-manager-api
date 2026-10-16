
using System.Text.RegularExpressions;
using IELS.FIR.Domain.Exceptions;

namespace IELS.FIR.Domain.ValueObjects;

/// <summary>
/// 
/// </summary>
public sealed record FirNumber
{
    private static readonly Regex Format = new(
        @"^[A-Z0-9-]{3,30}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public string Value { get; }

    private FirNumber(string value)
    {
        Value = value;
    }

/// <summary>
/// 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
/// <exception cref="DomainException"></exception>
    public static FirNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("FIR number is required.");

        var normalized = value.Trim().ToUpperInvariant();

        return !Format.IsMatch(normalized) ? throw new DomainException("Invalid FIR number format.") : new FirNumber(normalized);
    }

    public override string ToString() => Value;
}
