using System;
using Fantasy.BusinessEngine.Events;
namespace Fantasy.BusinessEngine.Services
{
    public interface INHConfigurationService
    {
        NHibernate.Cfg.Configuration Configuration { get; }

    }
}
