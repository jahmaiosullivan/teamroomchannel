using System.Web.Optimization;

namespace Teamroom.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new StyleBundle("~/global/styles").Include(
                "~/scripts/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/scripts/plugins/bootstrap/bootstrap-wysihtml5/bootstrap-wysihtml5.css",
                "~/areas/admin/content/plugins/font-awesome/css/font-awesome.min.css",
                "~/content/layout.css",
                "~/areas/admin/Content/css/components.css",
                "~/content/styles.css"
            ));

            bundles.Add(new ScriptBundle("~/global/scripts").Include(
                //"~/Scripts/jquery-*",
                //"~/Scripts/jquery-ui-1.10.3.custom.min.js",
                //"~/scripts/plugins/jquery-migrate.min.js",
                //"~/Scripts/plugins/jquery-validation/js/jquery.validate.min.js",
                //"~/Scripts/jquery.singleuploadimage.js",
                //"~/scripts/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                //"~/scripts/metronic.js",
                //"~/scripts/plugins/bootstrap/js/bootstrap.min.js",
                //"~/scripts/plugins/bootstrap/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
                //"~/scripts/plugins/bootstrap/bootstrap-wysihtml5/bootstrap-wysihtml5.js",
                "~/scripts/knockout-3.0.0.js",
                "~/scripts/knockout.mapping-latest.js",
                "~/scripts/main.js"
            ));

             bundles.Add(new ScriptBundle("~/scripts/fileupload").Include(
               "~/Scripts/load-image.all.min.js",
               "~/Scripts/plugins/jquery-file-upload/js/jquery.iframe-transport.js",
               "~/Scripts/plugins/jquery-file-upload/js/jquery.fileupload.js",
               "~/Scripts/plugins/jquery-file-upload/js/jquery.fileupload-process.js",
               "~/Scripts/plugins/jquery-file-upload/js/jquery.fileupload-image.js",
               "~/Scripts/plugins/jquery-file-upload/js/jquery.fileupload-validate.js"
            ));

             bundles.Add(new StyleBundle("~/styles/fileupload").Include(
               "~/Scripts/plugins/jquery-file-upload/css/jquery.fileupload.css",
               "~/Scripts/plugins/jquery-file-upload/css/jquery.fileupload-ui.css"
            ));

            #region "Admin Scripts"
            bundles.Add(new ScriptBundle("~/admin/scripts").Include(
                "~/scripts/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js",
                "~/scripts/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                "~/scripts/plugins/jquery.blockui.min.js",
                "~/scripts/plugins/jquery.cokie.min.js",
                "~/scripts/plugins/uniform/jquery.uniform.min.js",
                "~/scripts/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                "~/scripts/plugins/jqvmap/jqvmap/jquery.vmap.js",
                "~/scripts/plugins/jqvmap/jqvmap/maps/jquery.vmap.russia.js",
                "~/scripts/plugins/jqvmap/jqvmap/maps/jquery.vmap.world.js",
                "~/scripts/plugins/jqvmap/jqvmap/maps/jquery.vmap.europe.js",
                "~/scripts/plugins/jqvmap/jqvmap/maps/jquery.vmap.germany.js",
                "~/scripts/plugins/jqvmap/jqvmap/maps/jquery.vmap.usa.js",
                "~/scripts/plugins/jqvmap/jqvmap/data/jquery.vmap.sampledata.js",
                "~/scripts/plugins/flot/jquery.flot.min.js",
                "~/scripts/plugins/flot/jquery.flot.resize.min.js",
                "~/scripts/plugins/flot/jquery.flot.categories.min.js",
                "~/scripts/plugins/jquery.pulsate.min.js",
                "~/scripts/plugins/bootstrap-daterangepicker/moment.min.js",
                "~/scripts/plugins/bootstrap-daterangepicker/daterangepicker.js",
                "~/scripts/plugins/fullcalendar/fullcalendar.min.js",
                "~/scripts/plugins/jquery-easypiechart/jquery.easypiechart.min.js",
                "~/scripts/plugins/jquery.sparkline.min.js",
                "~/Scripts/toggles.min.js",
                "~/areas/admin/content/layout/scripts/layout.js",
                "~/areas/admin/content/layout/scripts/demo.js",
                "~/areas/admin/content/pages/scripts/index.js",
                "~/areas/admin/content/pages/scripts/tasks.js"));

            bundles.Add(new StyleBundle("~/admin/styles").Include(
                "~/scripts/plugins/uniform/css/uniform.default.css",
                "~/scripts/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                "~/scripts/plugins/bootstrap-daterangepicker/daterangepicker-bs3.css",
                "~/scripts/plugins/fullcalendar/fullcalendar.min.css",
                "~/scripts/plugins/jqvmap/jqvmap/jqvmap.css",
                "~/areas/admin/Content/pages/css/tasks.css",
                "~/areas/admin/Content/css/plugins.css",
                "~/areas/admin/Content/layout/css/themes/default.css",
                "~/areas/admin/Content/layout/css/custom.css",
                "~/Content/toggles-full.css",
                "~/Content/toggles-light.css"
            ));
            #endregion

        }
    }
}