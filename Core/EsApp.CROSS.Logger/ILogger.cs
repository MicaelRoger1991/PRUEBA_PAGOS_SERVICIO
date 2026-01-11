using System;

namespace EsApp.CROSS.Logger;

public interface ILogger
{
    void Debug(string format, params object[] objects);
    void Debug(string message);
    void Error(string format, params object[] objects);
    void Error(string message);
    void Fatal(string format, params object[] objects);
    void Fatal(string message);
}
