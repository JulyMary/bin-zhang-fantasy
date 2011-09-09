using System;
using Fantasy.BusinessEngine;
namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    interface IBusinessEntityGlyph
    {
        bool IsShortCut { get; }

        IBusinessEntity Entity {get;}
    }
}
