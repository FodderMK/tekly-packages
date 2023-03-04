using Tekly.Balance;
using Tekly.Common.TimeProviders;
using Tekly.Common.Utils;
using Tekly.Content;
using Tekly.DataModels.Models;
using Tekly.Glass;
using Tekly.Injectors;
using Tekly.Logging;
using Tekly.TreeState.StandardActivities;
using Tekly.Webster;
using UnityEngine;

namespace TeklySample.App
{
    public class AppCore : MonoBehaviour, IInjectionProvider
    {
        [SerializeField] private Glass m_glass;
        [SerializeField] private TimeProviderRef m_localTimeProviderRef;

        public void Provide(InjectorContainer container)
        {
            ITimeProvider localTimeProvider = new LocalTimeProvider();
            m_localTimeProviderRef.Initialize(localTimeProvider);
            
            var balanceManager = new BalanceManager(ContentProvider.Instance);

            container.Register(this);
            container.Register(new AppData());
            container.Register(ContentProvider.Instance);
            container.Register(balanceManager);
            container.Register(RootModel.Instance);
            container.Register(localTimeProvider);
            
            container.Register(m_glass);
            
            RootModel.Instance.Add("app", new AppModel(balanceManager));

            Debug.Log("Crash Detected: " + CrashCanary.Instance.CrashDetected);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
                return;
            }

            ModelManager.Instance.Tick();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            TkLogger.Initialize();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeDebug()
        {
            WebsterServer.Start(true);
            WebsterServer.AddRouteHandler<SampleWebsterHandler>();
        }
    }
}