using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;

internal partial class SubscriptionPublishSettings
{
    private const string XmlElement_SubscriptionInfo = "SubscriptionInfo";
    private const string SubscriptionSettingsSuffix = WorkbookPublishSettings.WorkbookSettingsSuffix;

    /// <summary>
    /// TRUE if the file is an internal settings file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    internal static bool IsSettingsFile(string filePath)
    {
        return filePath.EndsWith(SubscriptionSettingsSuffix, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Save Subscription metadata in a XML file along-side the workbook file
    /// </summary>
    /// <param name="wb">Information about the workbook we have downloaded</param>
    /// <param name="localSubscriptionPath">Local path to the twb/twbx of the workbook</param>
    /// <param name="userLookups">If non-NULL contains the mapping of users/ids so we can look up the owner</param>
    internal static void CreateSettingsFile(SiteSubscription ds, string localSubscriptionPath, KeyedLookup<SiteUser> userLookups)
    {

        string contentOwnerName = null; //Start off assuming we have no content owner information
        if (userLookups != null)
        {
            contentOwnerName = WorkbookPublishSettings.helper_LookUpOwnerId(ds.OwnerId, userLookups);
        }

        var xml = System.Xml.XmlWriter.Create(PathForSettingsFile(localSubscriptionPath));
        xml.WriteStartDocument();
            xml.WriteStartElement(XmlElement_SubscriptionInfo);

                //If we have an owner name, write it out
                if (!string.IsNullOrWhiteSpace(contentOwnerName))
                {
                  XmlHelper.WriteValueElement(xml,  WorkbookPublishSettings.XmlElement_ContentOwner, contentOwnerName);
                }
            xml.WriteEndElement(); //end: WorkbookInfo
        xml.WriteEndDocument();
        xml.Close();
    }

    /// <summary>
    /// Generates the path/filename of the Settings file that corresponds to the Subscription path
    /// </summary>
    /// <param name="SubscriptionPath"></param>
    /// <returns></returns>
    private static string PathForSettingsFile(string SubscriptionPath)
    {
        return WorkbookPublishSettings.PathForSettingsFile(SubscriptionPath);
    }


    /// <summary>
    /// Look up any saved settings we have associated with a Subscription on our local file systemm
    /// </summary>
    /// <param name="SubscriptionWithPath"></param>
    /// <returns></returns>
    internal static SubscriptionPublishSettings GetSettingsForSavedSubscription(string SubscriptionWithPath)
    {
        //Sanity test: If the Subscription is not there, then we probably have an incorrect path
        AppDiagnostics.Assert(File.Exists(SubscriptionWithPath), "Underlying Subscription does not exist");

        //Find the path to the settings file
        var pathToSettingsFile = PathForSettingsFile(SubscriptionWithPath);
        if (!File.Exists(pathToSettingsFile))
        {
            return new SubscriptionPublishSettings(null);
        }

        //===================================================================
        //We've got a setings file, let's parse it!
        //===================================================================
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(pathToSettingsFile);

        //Show sheets
        string ownerName = WorkbookPublishSettings.ParseXml_GetOwnerName(xmlDoc);

        //Return the Settings data
        return new SubscriptionPublishSettings(ownerName);
    }

}
