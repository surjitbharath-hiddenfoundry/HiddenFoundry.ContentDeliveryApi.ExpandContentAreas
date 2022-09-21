# HiddenFoundry.ContentDeliveryApi.ExpandContentAreas

Install-Package HiddenFoundry.ContentDeliveryApi.ExpandContentAreas
Configuration is optional and not specifying this will default the expand maximum level is 2. Which is enough to expand the first nested content area.

To configure the maximum levels deep you wish the recursively load through, add the following in your startup.cs


services.ConfigureRecursiveContentAreaContentApiOptions(o =>
    o.MaxExpandContentAreaLevels = 4;
});

Database has already been setup with an example page of nested blocks with content areas.
Make a request to: https://localhost:5000/en/standard-page-with-blocks/?expand=*

