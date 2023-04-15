

using Autofac;
using ImageCropResize.WinCore.Core;
using ImageCropResize.WinCore.Handlers.SettingStoreHandler;
using ImageCropResize.WinCore.Models;
using System.Reflection;

namespace ImageCropResize
{
    internal static class Program
    {
        public static IContainer? Container;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Container = ConfigureServices();

            using (var scope = Container.BeginLifetimeScope())
            {
                var handler = scope.Resolve<IStoreSettingHandler>();
                handler.CreateOrReadSettingJson();
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            using var main = Container.Resolve<MainWindow>();
            Application.Run(main);
        }

        private static IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindow>()
                .As<IMainWindow>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<SaveSettings>()
                .As<ISaveSettings>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<StoreSettingHandler>()
                .As<IStoreSettingHandler>();
            builder.RegisterType<SettingDataStore>()
                .As<ISettingDataStore>();
            builder.RegisterType<SettingManager>()
                .As<ISettingManager>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsSelf().AsImplementedInterfaces(); 

            return builder.Build();
        }
    }
}