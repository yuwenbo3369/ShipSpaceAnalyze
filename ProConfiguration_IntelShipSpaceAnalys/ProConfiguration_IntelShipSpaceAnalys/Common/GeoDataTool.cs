using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.CIM;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Catalog.Wizards.CreateFeatureClass;
using ArcGIS.Desktop.Mapping;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    public class GeoDataTool
    {
        public static Project DefaultProject;
        public DatabaseConnectionProperties OracleConnectionProperty;
        public Geodatabase OracleDatabase;
        public Geodatabase FileGeoDatabase;
        
        public static MapView GetActiveMap()
        {
            if(MapView.Active == null)
            {
                return null;
            }
            return MapView.Active;
        }

        public static void ActivePane(string panename)
        {
            Pane basicPane;
            List<Pane> panes = FrameworkApplication.Panes.Find(panename);
            if (panes == null || panes.Count == 0)
            {
                basicPane = FrameworkApplication.Panes.Create(panename);
            }
            else
            {
                basicPane = panes[0];
            }
            basicPane.Activate();
        }

        public static Geodatabase GetFileDatabaseSync(string path)
        {
            Geodatabase geodatabase1 = null;
            try
            {
                geodatabase1 = QueuedTask.Run(() =>{
                        return new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(path)));
                }).Result;
            }
            catch (GeodatabaseNotFoundOrOpenedException ex)
            {
                LogAPI.WriteErrorLog(ex);
            }
            return geodatabase1;
        }

        public async Task GeodatabaseOpenAsync(string path)
        {
            FileGeoDatabase = await QueuedTask.Run(() => GetFileDatabase(path));
            
        }

        public static Geodatabase GetSdeDatabaseSync(string path)
        {
            Geodatabase geodatabase1 = null;
            try
            {
                geodatabase1 = QueuedTask.Run(() => {
                    return new Geodatabase(new DatabaseConnectionFile(new Uri(path)));
                }).Result;
            }
            catch (GeodatabaseNotFoundOrOpenedException ex)
            {
                LogAPI.WriteErrorLog(ex);
            }
            return geodatabase1;
        }

        public Geodatabase GetFileDatabase(string path)
        {
            Geodatabase geodatabase1 = null;
            try
            {
                geodatabase1 = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(path)));
            }
            catch (GeodatabaseNotFoundOrOpenedException exception)
            {
                // Handle Exception.
            }
            return geodatabase1;
        }

        public async Task AddGDBLayer2Map(string layerpath, Map Activemap)
        {
             
            Uri uri = new Uri(layerpath);
            await QueuedTask.Run(() => LayerFactory.Instance.CreateLayer(uri, Activemap));
            //IReadOnlyList<Domain> domains = FileGeoDatabase.GetDomains();
        }

        public static Item GetDefaultGDBItem()
        {
            string defaultgdbpath = Project.Current.DefaultGeodatabasePath;
            string defaultgdbname = defaultgdbpath.Substring(defaultgdbpath.LastIndexOf('\\') + 1);
            return Project.Current.GetItems<GDBProjectItem>().FirstOrDefault(r => r.Name == defaultgdbname);
        }
    }
}
