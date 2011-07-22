﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using NHibernate;

namespace Fantasy.Studio.BusinessEngine
{
    class DocumentPadModel
    {
        private List<IDocumentTreeViewItem> _rootDocuments = null;
        public List<IDocumentTreeViewItem> RootDocuments
        {
            get
            {
                if (this._rootDocuments == null)
                {
                    this._rootDocuments = new List<IDocumentTreeViewItem>();
                    IEntityService es = ServiceManager.Services.GetRequiredService<IEntityService>();
                    ISession session = es.DefaultSession;
                    BusinessPackage root = session.Get<BusinessPackage>(BusinessPackage.RootPackageId);
                    PackageTreeViewItem rootItem = new PackageTreeViewItem(root);
                    this._rootDocuments.Add(rootItem);
                }
                return _rootDocuments;
            }
        }
    }
}