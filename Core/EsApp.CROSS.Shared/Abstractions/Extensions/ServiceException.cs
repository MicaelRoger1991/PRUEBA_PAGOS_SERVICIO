using System.Runtime.Serialization;

namespace EsApp.CROSS.Shared.Abstractions.Extensions;

[Serializable]
public sealed class ServiceException : Exception
{
    public Error? Error { get; private set; }

    public ServiceException(Error error)
    {
        Error = error;
    }

    [Obsolete("Implementado por la obs del sonarqube")]
    private ServiceException(SerializationInfo info, StreamingContext context)
       : base(info, context)
    {
    }

    /// <summary>
    /// Implementado por la obs del sonarqube
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    [Obsolete("Implementado por la obs del sonarqube")]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
    }
}