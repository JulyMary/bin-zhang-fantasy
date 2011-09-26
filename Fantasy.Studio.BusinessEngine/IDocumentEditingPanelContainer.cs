using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine
{
    public interface IDocumentEditingPanelContainer
    {
        IDocumentEditingPanel ActivePanel { get; set; }

        ICollection<IDocumentEditingPanel> Panels { get; }
    }
}
