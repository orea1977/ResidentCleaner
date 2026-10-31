// ResidentCleaner - 天際線 2 市民清理模組（選單版）
// 在「選項 → ResidentCleaner」提供兩個按鈕，只在遊戲中按下時才執行清理。

using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;

namespace ResidentCleaner
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger(nameof(ResidentCleaner)).SetShowsErrorsInUI(false);

        private Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info("ResidentCleaner loaded");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleSource(m_Setting, false));
            GameManager.instance.localizationManager.AddSource("zh-HANT", new LocaleSource(m_Setting, true));

            updateSystem.UpdateAt<ResidentCleanerSystem>(SystemUpdatePhase.MainLoop);
        }

        public void OnDispose()
        {
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}
