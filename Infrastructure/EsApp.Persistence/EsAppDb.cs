using System;
using EsApp.CROSS.Encrypt;
using Microsoft.Extensions.Options;

namespace EsApp.Persistence.DataAccess;

public class EsAppDb(ISecurityEncrypt securityEncrypt, IOptions<DataBaseSettings> dataBaseSettings)
    : EsApp.Persistence.DataAccess.DataAccess(securityEncrypt, nameof(EsAppDb), dataBaseSettings)
{

}
