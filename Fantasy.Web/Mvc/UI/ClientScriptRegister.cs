using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.UI
{
    public class ClientScriptRegister : IClientSideScriptFactory
    {

        #region IClientSideScript Members

        public void Register(string id, string content)
        {
            if (!this._scripts.Any(s => string.Equals(id, s.Id, StringComparison.OrdinalIgnoreCase)))
            {
                this._scripts.Add(new ClientScript() { Id = id, Content = content }); ;
            }
        }

        private List<ClientScript> _scripts = new List<ClientScript>();

        private class ClientScript
        {
            public string Id { get; set; }

            public string Content { get; set; }
        }

        #endregion
    }
}