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
    internal class btnEucDistance : Button
    {
        protected override void OnClick()
        {
            #region 
            try
            {
                OpenEucDistance();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        private async void OpenEucDistance()
        {
            await CommonMethod.OpenMap();
            string projectDir = System.IO.Path.GetDirectoryName(Project.Current.Path);
            string path = projectDir + "\\"+ConstDefintion.ConstRaster_RaderRangeDsName;
            Envelope env = CommonMethod.GetRasterExtent(path).Result;
            var environments = Geoprocessing.MakeEnvironmentArray(extent: env, mask: ConstDefintion.ConstLayer_RaderRangeLayerName);;
            Geoprocessing.OpenToolDialog("sa.EucDistance", null, environments, true, null);
        }
    }
}
