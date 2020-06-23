using ArcGIS.Core.Data;
using ArcGIS.Core.Data.Raster;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    public class CommonMethod
    {
        public static async Task<Map> OpenMap()
        {
            Map map;
            if (FrameworkApplication.Panes.ActivePane != null && FrameworkApplication.Panes.ActivePane.Caption == ConstDefintion.ConstMap_SpaceAnalyzeMap)
            {
                map = MapView.Active.Map;
            }
            else
            {
                map = await FindOpenExistingMapAsync(ConstDefintion.ConstMap_SpaceAnalyzeMap);
            }
            return map;
        }
        public static async Task<Map> FindOpenExistingMapAsync(string mapName)
        {
            return await QueuedTask.Run(async () =>
            {
                Map map = null;
                Project proj = Project.Current;

                //Finding the first project item with name matches with mapName
                MapProjectItem mpi =
                    proj.GetItems<MapProjectItem>()
                        .FirstOrDefault(m => m.Name.Equals(mapName, StringComparison.CurrentCultureIgnoreCase));
                if (mpi != null)
                {
                    map = mpi.GetMap();
                    //Opening the map in a mapview
                    await ProApp.Panes.CreateMapPaneAsync(map);
                }
                return map;
            });

        }
        public static Task<Envelope> GetRasterExtent(string path)
        {
            Task<Envelope> task = QueuedTask.Run(() =>
            {
                string directory = System.IO.Path.GetDirectoryName(path);
                string fileName = System.IO.Path.GetFileName(path);
                FileSystemConnectionPath connectionPath = new FileSystemConnectionPath(new System.Uri(directory), FileSystemDatastoreType.Raster);
                FileSystemDatastore dataStore = new FileSystemDatastore(connectionPath);
                RasterDataset fileRasterDataset = dataStore.OpenDataset<RasterDataset>(fileName);
                Raster raster = fileRasterDataset.CreateFullRaster();
                Envelope env = raster.GetExtent();
                return env;
            });
            return task;
        } 
    }
}
