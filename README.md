# HiddenFoundry.ContentDeliveryApi.ExpandContentAreas

Install-Package HiddenFoundry.ContentDeliveryApi.ExpandContentAreas
Configuration is optional and not specifying this will default the expand maximum level is 2. Which is enough to expand the first nested content area.

To configure the maximum levels deep you wish the recursively load through, add the following in your startup.cs


services.ConfigureRecursiveContentAreaContentApiOptions(o =>
    o.MaxExpandContentAreaLevels = 4;
});