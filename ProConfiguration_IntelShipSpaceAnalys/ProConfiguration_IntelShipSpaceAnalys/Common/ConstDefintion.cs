using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    public class ConstDefintion
    {
        #region DAML ID
        public const string ConstID_ContentDockPane = "esri_core_contentsDockPane";
        public const string ConstID_SketchLineTool = "esri_editing_SketchLineTool";
        public const string ConstID_EditingTab = "esri_editing_EditingTab";
        public const string ConstID_CreateFeaturesDockPane = "esri_editing_CreateFeaturesDockPane";
        public const string ConstID_DPObstacleExtraction = "ProConfiguration_IntelShipSpaceAnalys_DockPane_DPObstacleExtraction";
        #endregion

        #region 要素类 地图 gdb等Pro工程要素
        public const string ConstMap_SpaceAnalyzeMap = "SpaceAnalys";
        public const string ConstLayer_RaderRangeLayerName = "rader";
        public const string ConstRaster_RaderRangeDsName = "rader.tif";
        public const string ConstFeatureClass_Course = "course";
        public const string ConstFeatureClass_SelectedCourse = "SelectedCourse";
        public const string ConstFeatureClass_CourseBuffer = "CourseBuffer";
        public const string ConstFeatureClass_CourseObstacle = "CourseObstacle";
        public const string ConstFeatureClass_AisPoint = "AisLocation";
        #endregion

        #region 自定义
        public const string ConstPath_DefaultProjectPath = @"F:\360MoveData\Users\adma\Documents\ArcGIS\Projects\IntelShipSpaceAnalys\IntelShipSpaceAnalys.aprx";
        public const string ConstStr_GeoprocessCancling = "取消";
        #endregion
    }
}
