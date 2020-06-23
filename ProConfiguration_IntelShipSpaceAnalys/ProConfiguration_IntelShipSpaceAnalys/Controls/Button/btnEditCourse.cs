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
    internal class btnEditCourse : Button
    {
        protected override void OnClick()
        {
            #region 
            try
            {
                EditCourse();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        private async void EditCourse()
        {
            await QueuedTask.Run(() =>
            {
                //hide all tools except line tool on layer
                var featLayer = MapView.Active.Map.FindLayers(ConstDefintion.ConstFeatureClass_Course).First();
                var editTemplates = featLayer.GetTemplates();
                var newCIMEditingTemplates = new List<CIMEditingTemplate>();
                foreach (var et in editTemplates)
                {
                    // initialize template by activating default tool
                    et.ActivateDefaultToolAsync();
                    var cimEditTemplate = et.GetDefinition();
                    // get the visible tools on this template
                    var allTools = et.ToolIDs.ToList();
                    // add the hidden tools on this template
                    allTools.AddRange(cimEditTemplate.GetExcludedToolDamlIds().ToList());
                    // hide all the tools then allow the line tool
                    cimEditTemplate.SetExcludedToolDamlIds(allTools.ToArray());
                    cimEditTemplate.AllowToolDamlID(ConstDefintion.ConstID_SketchLineTool);
                    newCIMEditingTemplates.Add(cimEditTemplate);
                }
                // update the layer templates
                var layerDef = featLayer.GetDefinition() as CIMFeatureLayer;
                // Set AutoGenerateFeatureTemplates to false for template changes to stick

                layerDef.AutoGenerateFeatureTemplates = false;
                layerDef.FeatureTemplates = newCIMEditingTemplates.ToArray();
                featLayer.SetDefinition(layerDef);
                MapView.Active.ZoomToAsync(featLayer);

            });
            FrameworkApplication.ActivateTab(ConstDefintion.ConstID_EditingTab);
            DockPane dp = FrameworkApplication.DockPaneManager.Find(ConstDefintion.ConstID_CreateFeaturesDockPane);
            if (dp != null)
                dp.Activate();
        }
    }
}
