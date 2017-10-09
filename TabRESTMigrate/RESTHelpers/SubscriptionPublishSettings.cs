using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.IO;

internal partial class SubscriptionPublishSettings
{
    /// <summary>
    /// The name of the owner of the content (NULL if unknown)
    /// </summary>
    public readonly string OwnerName;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ownerName"></param>
    public SubscriptionPublishSettings(string ownerName)
    {
        this.OwnerName = ownerName;
    }
}
