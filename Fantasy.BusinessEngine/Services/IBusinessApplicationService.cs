﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IBusinessApplicationService
    {

        BusinessApplication Create(Guid id);

        BusinessApplication Create(Type t);

        BusinessApplication CreateByName(string name);

        void ReleaseApplication(BusinessApplication application);

        BusinessApplicationData GetApplicationData(Guid id);

        string GetApplicationFriendlyName(Guid id);


        string EncryptRootId(Guid applicationId, Guid objectId);

        Guid DecryptRootId(Guid applicationId, string cipherText);
    }
}
