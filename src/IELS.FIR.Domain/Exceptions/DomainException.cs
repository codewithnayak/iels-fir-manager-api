
namespace IELS.FIR.Domain.Exceptions;

/// <summary>
/// 
/// </summary>
public sealed class DomainException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public DomainException(string message) : base(message)
    {
        
    }
}
