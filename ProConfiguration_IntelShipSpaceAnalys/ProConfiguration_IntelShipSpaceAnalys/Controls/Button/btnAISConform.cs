using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Internal.Core;
using ArcGIS.Core.Data.Raster;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    internal class btnAISConform : Button
    {
        protected override void OnClick()
        {
            #region 
            try
            {
                var args = Geoprocessing.MakeValueArray(string.Empty,GeoDataTool.DefaultProject.DefaultGeodatabasePath+"\\"+ConstDefintion.ConstFeatureClass_AisPoint,string.Empty,string.Empty);
                 Geoprocessing.OpenToolDialog("management.XYTableToPoint", args,null,false,null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }
    }
}
