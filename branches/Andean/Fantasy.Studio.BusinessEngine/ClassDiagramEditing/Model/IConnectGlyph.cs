using System;
using System.Collections.ObjectModel;
namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public interface IConnectGlyph
    {
        ObservableCollection<Point> IntermediatePoints { get; }
    }
}
