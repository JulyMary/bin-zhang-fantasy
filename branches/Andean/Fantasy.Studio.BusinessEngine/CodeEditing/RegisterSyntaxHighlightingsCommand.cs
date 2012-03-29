using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Studio.BusinessEngine.Properties;
using ICSharpCode.AvalonEdit.Highlighting;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class RegisterSyntaxHighlightingsCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            foreach(FileSyntaxHighlight h in Settings.Default.SystemFileSyntax.Union(Settings.Default.FileSyntax))
            {
                IHighlightingDefinition def = HighlightingManager.Instance.GetDefinition(h.Language);
                HighlightingManager.Instance.RegisterHighlighting(null, h.Extension.Split(';'), def); 
            }

            return null;
        }

        #endregion
    }
}
