using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Unity.Entities;

namespace ResidentCleaner
{
    [FileLocation(nameof(ResidentCleaner))]
    [SettingsUIGroupOrder(kCleanGroup)]
    [SettingsUIShowGroupName(kCleanGroup)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";
        public const string kCleanGroup = "Clean";

        public Setting(IMod mod) : base(mod) { }

        // 按鈕一：只刪除引用已不存在物件的市民
        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(kSection, kCleanGroup)]
        public bool CleanBrokenResidents { set { Request(false); } }

        // 按鈕二：刪除所有外出中的市民
        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(kSection, kCleanGroup)]
        public bool CleanAllResidents { set { Request(true); } }

        private static void Request(bool all)
        {
            World world = World.DefaultGameObjectInjectionWorld;
            world?.GetOrCreateSystemManaged<ResidentCleanerSystem>().RequestClean(all);
        }

        public override void SetDefaults() { }
    }

    public class LocaleSource : IDictionarySource
    {
        private readonly Setting m_Setting;
        private readonly bool m_Chinese;

        public LocaleSource(Setting setting, bool chinese)
        {
            m_Setting = setting;
            m_Chinese = chinese;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            string broken = nameof(Setting.CleanBrokenResidents);
            string all = nameof(Setting.CleanAllResidents);

            if (m_Chinese)
            {
                return new Dictionary<string, string>
                {
                    { m_Setting.GetSettingsLocaleID(), "ResidentCleaner 市民清理" },
                    { m_Setting.GetOptionTabLocaleID(Setting.kSection), "主要" },
                    { m_Setting.GetOptionGroupLocaleID(Setting.kCleanGroup), "清理外出中的市民（需在遊戲中使用）" },

                    { m_Setting.GetOptionLabelLocaleID(broken), "清除異常市民" },
                    { m_Setting.GetOptionDescLocaleID(broken), "只刪除引用已不存在物件（車輛、車道、目的地）的外出市民。結果寫在 Logs\\ResidentCleaner.log。" },
                    { m_Setting.GetOptionWarningLocaleID(broken), "確定要清除異常市民嗎？建議先存檔。" },

                    { m_Setting.GetOptionLabelLocaleID(all), "清除所有外出市民" },
                    { m_Setting.GetOptionDescLocaleID(all), "刪除所有外出中的市民分身。市民本身和住處會保留，之後會重新出門。" },
                    { m_Setting.GetOptionWarningLocaleID(all), "確定要清除所有外出市民嗎？建議先存檔。" },
                };
            }

            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "ResidentCleaner" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kCleanGroup), "Clean residents (in game only)" },

                { m_Setting.GetOptionLabelLocaleID(broken), "Clean broken residents" },
                { m_Setting.GetOptionDescLocaleID(broken), "Delete residents that reference missing vehicles, lanes or targets. Result is written to Logs\\ResidentCleaner.log." },
                { m_Setting.GetOptionWarningLocaleID(broken), "Clean broken residents? Saving first is recommended." },

                { m_Setting.GetOptionLabelLocaleID(all), "Clean all residents" },
                { m_Setting.GetOptionDescLocaleID(all), "Delete all residents currently outside. Citizens and homes are kept." },
                { m_Setting.GetOptionWarningLocaleID(all), "Clean all residents? Saving first is recommended." },
            };
        }

        public void Unload() { }
    }
}
