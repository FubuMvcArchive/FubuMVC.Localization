using System.Globalization;
using Bottles;
using FubuCore;
using FubuLocalization;
using FubuLocalization.Basic;
using FubuMVC.Core;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.UI;

namespace FubuMVC.Localization
{
    public class BasicLocalizationSupport : IFubuRegistryExtension
    {
        private readonly FubuPackageRegistry _internalRegistry = new FubuPackageRegistry();

        public BasicLocalizationSupport()
        {
            _internalRegistry.Services(x =>
            {
                x.SetServiceIfNone(new CultureInfo("en-US"));
                x.SetServiceIfNone<ILocalizationCache, LocalizationCache>();
                x.SetServiceIfNone<ILocalizationMissingHandler, LocalizationMissingHandler>();
                x.SetServiceIfNone<ILocalizationProviderFactory, LocalizationProviderFactory>();
            
                x.SetServiceIfNone<ILocalizationStorage, BottleAwareXmlLocalizationStorage>();
            });

            _internalRegistry.Import<HtmlConventionRegistry>(x => x.Labels.Add(new LabelBuilder()));
            
        }

        public CultureInfo DefaultCulture
        {
            set
            {
                _internalRegistry.Services(x => x.ReplaceService(value));
            }
        }

        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            _internalRegistry.As<IFubuRegistryExtension>().Configure(registry);

            registry.Services(x =>
            {
                    x.AddService<IActivator, SpinUpLocalizationCaches>();
            });

            
        }

    }
}