using ACT.Core.Extensions;
using ACT.Core.Logger.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ACT.Core.Logger.Types
{


#if DEBUG
    internal class PluginHashInstructionsInfo
    {
        public override string ToString()
        {
            return new Y().ToString();
        }
        private class Y
        {
            public override string ToString()
            {
                return "20d24b3e-5e8b-4f46-afe6-77e579083e3e__ACT__205412c1-e046-4b2d-af16-186a73f8cb9a  This is the EXAMPLE Source for ACT Example Plugins HASH (SHA3512)->Base64 The Second Guid Is DB UserID/CompanyID";
            }
        }
    }
#endif
}
