using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BackupOSToNAS
{
    internal interface ITaskConfig
    {
        Dictionary<string, string> KeyValues { get; }
        bool Write(string fileName);
        bool Read(string fileName);
    }
    //备份参数信息，最终写入为配置文件，交给PE中的程序执行备份任务
    internal class BackupAndRestoreConfig : ITaskConfig
    {
        private static Encoding utf8Encoding;
        public const string TargetProperty = "Target";
        public const string NASNameProperty = "NASName";
        public const string NASPathProperty = "NASPath";
        public const string NASUserProperty = "NASUser";
        public const string NASPasswordProperty = "NASPassword";
        public const string OperationProperty = "Operation";
        public const string DeviceGuidProperty = "DeviceGuid";
        public const string OSLoaderGuidProperty = "OSLoaderGuid";
        public const string LastDefaultGuidProperty = "LastDefaultGuid";
        public const string RestoreOperation = "restore";
        public const string BackupOperation = "backup";
        public const string BackupFileTypeProperty = "BackupFileType";
        public const string IsLocalBackupProperty = "IsLocalBackup";
        public const string ExtraGhoArgumentProperty = "ExtraGhoArgument";
        public const string ExtraWimArgumentProperty = "ExtraWimArgument";
        public const string LocalPathProperty = "LocalPath";
        public const string LocalDeviceProperty = "LocalDevice";
        public const string Yes = "yes";
        public const string No = "no";
        public Dictionary<string, string> KeyValues { get; private set; }
        static BackupAndRestoreConfig()
        {
            utf8Encoding = new UTF8Encoding(false);
        }
        public BackupAndRestoreConfig(
            string target, string nasName,
            string nasPath, string nasUser,
            string nasPassword, string operation,
            Guid deviceGuid, Guid OSLoaderGuid,
            Guid LastDefaultGuid, string BackupFileType,
            string IsLocalBackup, string ExtraGhoArgument,
            string ExtraWimArgument, string LocalPath, 
            string LocalDevice)
        {
            KeyValues = new Dictionary<string, string>
            {
                { TargetProperty, target },
                { NASNameProperty, nasName },
                { NASPathProperty, nasPath },
                { NASUserProperty, nasUser },
                { NASPasswordProperty, nasPassword },
                { OperationProperty, operation },
                { DeviceGuidProperty, deviceGuid.ToString() },
                { OSLoaderGuidProperty, OSLoaderGuid.ToString() },
                { LastDefaultGuidProperty, LastDefaultGuid.ToString() },
                { BackupFileTypeProperty, BackupFileType },
                { IsLocalBackupProperty, IsLocalBackup },
                { ExtraGhoArgumentProperty, ExtraGhoArgument },
                { ExtraWimArgumentProperty, ExtraWimArgument },
                { LocalPathProperty,LocalPath },
                { LocalDeviceProperty,LocalDevice },
            };
        }
        public BackupAndRestoreConfig()
        {
            KeyValues = new Dictionary<string, string>
            {
                { TargetProperty, "" },
                { NASNameProperty, "" },
                { NASPathProperty, "" },
                { NASUserProperty, "" },
                { NASPasswordProperty, "" },
                { OperationProperty, "" },
                { DeviceGuidProperty, "" },
                { OSLoaderGuidProperty, "" },
                { LastDefaultGuidProperty, "" },
                { BackupFileTypeProperty, "" },
                { IsLocalBackupProperty, "" },
                { ExtraGhoArgumentProperty, "" },
                { ExtraWimArgumentProperty, "" },
                { LocalPathProperty,"" },
                { LocalDeviceProperty,"" },
            };
        }
        public bool Read(string fileName)
        {
            string configText;
            try
            {
                configText = File.ReadAllText(fileName);
                object obj = JsonConvert.DeserializeObject(configText);
                if (obj != null && obj is JObject @object)
                {
                    JObject jobj = @object;
                    KeyValues.Clear();
                    foreach (KeyValuePair<string, JToken> keyValue in jobj)
                    {
                        if (keyValue.Value.Value<string>() != null && keyValue.Value.Type == JTokenType.String)
                            KeyValues.Add(keyValue.Key, keyValue.Value.Value<string>());
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Write(string fileName)
        {
            StringBuilder buffer = new StringBuilder();
            JsonTextWriter writer = new JsonTextWriter(new StringWriter(buffer));
            writer.WriteStartObject();
            foreach (KeyValuePair<string, string> keyValue in KeyValues)
            {
                writer.WritePropertyName(keyValue.Key);
                writer.WriteValue(keyValue.Value);
            }
            writer.WriteEndObject();
            try
            {
                File.WriteAllText(fileName, buffer.ToString(), utf8Encoding);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
