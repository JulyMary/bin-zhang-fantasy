using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Properties;
using System.Windows.Markup;
using System.IO;
using Fantasy.IO;

namespace Fantasy.AddIns
{
    public class DefaultAddInTree : IAddInTree
    {

        private DefaultAddInTreeNode Root = new DefaultAddInTreeNode() { ID = string.Empty };

        private List<AddIn> _addIns = new List<AddIn>();

        public AddIn[] AddIns
        {
            get { return _addIns.ToArray(); }
        }

        public IAddInTreeNode GetTreeNode(string path)
        {
            DefaultAddInTreeNode rs = this.Root;
            string[] segs = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string id in segs)
            {
                rs = rs.FindChild(id);
                if (rs == null)
                {
                    throw new AddInException(string.Format(Resources.CannotFindPathInAddInTreeText, path)); 
                }
            }
            return rs;
        }

        public void InsertAddIn(AddIn addIn)
        {
            foreach(Extension extension in addIn.Extensions)
            {
                this.AddExtensionToTree(extension, addIn);
            }
        }

        private void AddExtensionToTree(Extension extension, AddIn addIn)
        {
            ConditionCollection condition = new ConditionCollection();
            if(extension.Conditional != null)
            {
                condition.Add(extension.Conditional);
            }
            DefaultAddInTreeNode node = this.CreateTreeNode(extension.Path, this.Root);

            if (extension.Codons != null)
            {
                string previous = null;
                foreach (ICodon codon in extension.Codons)
                {
                    this.AddCodonToTree(node, condition, codon, addIn, previous);
                    previous = codon.ID;
                }
            }
        }

        private void AddCodonToTree(DefaultAddInTreeNode parentNode, ConditionCollection parentCondition, ICodon codon, AddIn addIn, string previous)
        {
            ConditionCollection condition = new ConditionCollection(parentCondition);
            DefaultAddInTreeNode node = this.CreateTreeNode(codon.ID, parentNode);
            if (node.Codon == null)
            {
                node.Codon = codon;
                node.AddIn = addIn;
                List<string> before = new List<string>();
                if (!string.IsNullOrWhiteSpace(codon.InsertBefore))
                {
                    before.AddRange(codon.InsertBefore.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                }
                node.InsertBefore = before.ToArray();

                List<string> after = new List<string>();
                if (!string.IsNullOrWhiteSpace(codon.InsertAfter))
                {
                    after.AddRange(codon.InsertAfter.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                }
                if (previous != null)
                {
                    after.Add(previous);
                }
                node.InsertAfter = after.ToArray(); 
            }
            else
            {
                throw new AddInException(string.Format(node.FullPath, addIn.Name, node.AddIn.Name));
            }

            if (codon.Conditional != null)
            {
                condition.Add(codon.Conditional);
            }

            node.Condition = condition;
            string childPrevious = null;
            foreach (ICodon child in codon.Codons)
            {
                AddCodonToTree(node, condition, child, addIn, childPrevious);
                childPrevious = child.ID;
            }

        }

        private DefaultAddInTreeNode CreateTreeNode(string path, DefaultAddInTreeNode parent)
        {
            string[] segs = path.Split(new char[] { '/' }, 2, StringSplitOptions.RemoveEmptyEntries);
            DefaultAddInTreeNode rs = parent;
            if(segs.Length > 0)
            {
                rs = parent.FindChild(segs[0]);
                if (rs == null)
                {
                    rs = new DefaultAddInTreeNode() { ID = segs[0], Parent = parent };
                    parent.ChildNodes.Add(rs);
                }

                if (segs.Length == 2 && !string.IsNullOrWhiteSpace(segs[1]))
                {
                    rs = CreateTreeNode(segs[1], rs);
                }
            }
            return rs;
        }

        public void RemoveAddIn(AddIn addIn)
        {
            throw new NotImplementedException();
        }

        public static void Initialize(IEnumerable<string> addInFiles)
        {
            AddInTree.Tree = new DefaultAddInTree();
            foreach (string file in addInFiles)
            {
                FileStream fs = LongPathFile.Open(file, FileMode.Open, FileAccess.Read);
                AddIn addIn = null;
                try
                {
                    addIn = (AddIn)XamlReader.Load(fs);
                }
                finally
                {
                    fs.Close();
                }
                AddInTree.Tree.InsertAddIn(addIn);
            }
        }

    }
}
