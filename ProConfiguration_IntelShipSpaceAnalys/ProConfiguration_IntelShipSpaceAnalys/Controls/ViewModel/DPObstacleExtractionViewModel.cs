using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    public class DPObstacleExtractionViewModel : DockPane
    {
        #region DockPane属性
        public DPObstacleExtractionViewModel()
        {

        }
        
        protected override void OnShow(bool isShow)
        {
            //初始化
            if (isShow)
            {
                if (GeoDataTool.DefaultProject == null) return;
                CourseOIDList = GetAllCourse();
                CourseListSelectedIndex = 0;

                ObstaclePath = GeoDataTool.DefaultProject.DefaultGeodatabasePath + "\\" + ConstDefintion.ConstFeatureClass_AisPoint;
            }

        }

        public static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(ConstDefintion.ConstID_DPObstacleExtraction);
            bool flag = pane == null;
            if (!flag)
            {
                pane.Activate();
            }
        }
        #endregion

        #region 属性定义
        private ObservableCollection<string> _CourseOIDList;

        public ObservableCollection<string> CourseOIDList
        {
            get
            {
                if (_CourseOIDList == null)
                    _CourseOIDList = new ObservableCollection<string>();
                return _CourseOIDList; 
            }
            set { SetProperty(ref _CourseOIDList, value, () => CourseOIDList); }
        }
        private string _CourseListSelectedItem;

        public string CourseListSelectedItem
        {
            get { return _CourseListSelectedItem; }
            set { SetProperty(ref _CourseListSelectedItem, value, () => CourseListSelectedItem); }
        }

        private int _CourseListSelectedIndex;

        public int CourseListSelectedIndex
        {
            get { return _CourseListSelectedIndex; }
            set { SetProperty(ref _CourseListSelectedIndex, value, () => CourseListSelectedIndex); }
        }


        private string _ObstaclePath;

        public string ObstaclePath
        {
            get { return _ObstaclePath; }
            set { SetProperty(ref _ObstaclePath, value, () => ObstaclePath); }
        }

        private string  _ExtraDistance;

        public string  ExtraDistance
        {
            get { return _ExtraDistance; }
            set { SetProperty(ref _ExtraDistance, value, () => ExtraDistance); }
        }

        #endregion

        #region 命令
        public ICommand SetCourseSelectedItemCommand
        {
            get
            {
                return new RelayCommand((args) => CourseSelectionChanged(args), () => true);
            }
        }
        public ICommand RefleshCourseCommand
        {
            get
            {
                return new RelayCommand((args) => RefleshCourse(args), () => true);
            }
        }
        public ICommand OpenObstacleCommand
        {
            get
            {
                return new RelayCommand((args) => OpenObstacle(args), () => true);
            }
        }


        public ICommand ExcuteCommand
        {
            get
            {
                return new RelayCommand((args) => Excute(args), () => true);
            }
        }




        #endregion

        #region 方法
        private ObservableCollection<string> GetAllCourse()
        {
            var task = QueuedTask.Run(()=> 
            {
                ObservableCollection<string> result = new ObservableCollection<string>();
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(GeoDataTool.DefaultProject.DefaultGeodatabasePath))))
                {
                    using (FeatureClass course = geodatabase.OpenDataset<FeatureClass>(ConstDefintion.ConstFeatureClass_Course))
                    {
                        using (RowCursor rowCursor = course.Search(null, false))
                        {
                            while (rowCursor.MoveNext())
                            {
                                using (Row row = rowCursor.Current)
                                {
                                    result.Add(row.GetObjectID().ToString());
                                }
                            }
                        }
                    }
                        
                }
                return result;
            });
            return task.Result;
        }
        private void RefleshCourse(object args)
        {
            CourseOIDList = GetAllCourse();
            CourseListSelectedIndex = 0;
        }
        private void CourseSelectionChanged(object args)
        {
            SelectCourse();
        }

        private async void SelectCourse()
        {
            string fieldName = string.Empty;
            var oidField = QueuedTask.Run(() =>
            {
                string name;
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(GeoDataTool.DefaultProject.DefaultGeodatabasePath))))
                {
                    FeatureClassDefinition fcd = geodatabase.GetDefinition<FeatureClassDefinition>(ConstDefintion.ConstFeatureClass_Course);
                    name = fcd.GetObjectIDField();
                }
                return name;
            }).Result;
            string path = ConstDefintion.ConstFeatureClass_Course;
            string whereClause = $"{oidField} = {CourseListSelectedItem}";

            ProgressDialog progressDialogDomain = new ProgressDialog($"切换选择中", ConstDefintion.ConstStr_GeoprocessCancling, false);
            var arg = Geoprocessing.MakeValueArray(path, "NEW_SELECTION", whereClause);
            var task = await Geoprocessing.ExecuteToolAsync("SelectLayerByAttribute_management", arg, null, new CancelableProgressorSource(progressDialogDomain).Progressor);
            if (!task.IsFailed)
            {
                if (!(MapView.Active == null))
                {
                    await MapView.Active.ZoomToSelectedAsync();
                }
            }
        }
        private void Excute(object args)
        {
            double distance;
            if (double.TryParse(ExtraDistance, out distance))
            {
                ExcuteThis(distance);

            }
            else
            {
                MessageBox.Show("未能成功解析提取距离");
                return;
            }
        }

        private async void ExcuteThis(double distance)
        {
           var result =  await  ExtraObstacle(distance);
            //清除选项 以及选择该图层
            if(result!=null && !result.IsFailed)
            {
                if (MapView.Active != null)
                {
                    List<Layer> layers = new List<Layer>();
                    Layer obstacle = MapView.Active.Map.GetLayersAsFlattenedList().FirstOrDefault(l => l.Name == ConstDefintion.ConstFeatureClass_CourseObstacle);
                    if (obstacle != null)
                    {
                        layers.Add(obstacle);
                    }
                    MapView.Active.SelectLayers(layers);
                }

            }

        }

        private async Task<IGPResult> ExtraObstacle(double distance)
        {
            string fieldName = string.Empty;
            var oidField = QueuedTask.Run(() =>
            {
                string name;
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(GeoDataTool.DefaultProject.DefaultGeodatabasePath))))
                {
                    FeatureClassDefinition fcd = geodatabase.GetDefinition<FeatureClassDefinition>(ConstDefintion.ConstFeatureClass_Course);
                    name = fcd.GetObjectIDField();
                }
                return name;
            }).Result;
            string path = GeoDataTool.DefaultProject.DefaultGeodatabasePath +"\\" + ConstDefintion.ConstFeatureClass_Course;
            string outpath = GeoDataTool.DefaultProject.DefaultGeodatabasePath + "\\" + ConstDefintion.ConstFeatureClass_SelectedCourse;
            string whereClause = $"{oidField} = {CourseListSelectedItem}";
            ProgressDialog progressDialogDomain = new ProgressDialog($"正在创建被选择航线图层", ConstDefintion.ConstStr_GeoprocessCancling, false);
            var arg = Geoprocessing.MakeValueArray(path, outpath, whereClause);
            var gp = await Geoprocessing.ExecuteToolAsync("Select_analysis", arg, null, new CancelableProgressorSource(progressDialogDomain).Progressor);
            if (!gp.IsFailed)
            {
                ProgressDialog progressDialog1 = new ProgressDialog($"正在构造缓冲区", ConstDefintion.ConstStr_GeoprocessCancling, false);
                string outBufferpath = GeoDataTool.DefaultProject.DefaultGeodatabasePath + "\\" + ConstDefintion.ConstFeatureClass_CourseBuffer;
                string str_distance = $"{distance} NauticalMiles";
                var arg1 = Geoprocessing.MakeValueArray(outpath, outBufferpath, str_distance);
                var gp1 = await Geoprocessing.ExecuteToolAsync("Buffer_analysis", arg1, null, new CancelableProgressorSource(progressDialog1).Progressor);
                if (!gp1.IsFailed)
                {
                    ProgressDialog progressDialog2 = new ProgressDialog($"正在提取碍航物", ConstDefintion.ConstStr_GeoprocessCancling, false);
                    string insArgs = $"{ObstaclePath} 1;{outBufferpath} 2";
                    string outcourseObstacle = GeoDataTool.DefaultProject.DefaultGeodatabasePath + "\\" + ConstDefintion.ConstFeatureClass_CourseObstacle;
                    var arg2 = Geoprocessing.MakeValueArray(insArgs, outcourseObstacle);
                    var gp2 = await Geoprocessing.ExecuteToolAsync("Intersect_analysis", arg2, null, new CancelableProgressorSource(progressDialog2).Progressor);
                    return gp2;
                }
            }
            return null;
        }

        private void OpenObstacle(object args)
        {
            OpenItemDialog oid = new OpenItemDialog()
            {
                MultiSelect = false,
                Title = "选择碍航物要素",
                Filter = ItemFilters.featureClasses_all
            };
            if(oid.ShowDialog() == true)
            {
                var item = oid.Items.FirstOrDefault();
                ObstaclePath = item.Path;
            }
        }
        #endregion
    }
}
