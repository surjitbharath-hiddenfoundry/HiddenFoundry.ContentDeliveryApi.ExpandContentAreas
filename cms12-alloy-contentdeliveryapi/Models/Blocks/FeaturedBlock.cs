using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace cms12_alloy.Models.Blocks;

[ContentType(DisplayName = "FeaturedBlock", GUID = "ee98fd79-9d9c-4270-91ca-e22aab0ff3d2", Description = "")]
public class FeaturedBlock : BlockData
{
            [Display(
                Name = "Name",
                Description = "Name field's description",
                GroupName = SystemTabNames.Content,
                Order = 1)]
            public virtual string Name { get; set; }

    [Display(
    Name = "Name",
    Description = "Name field's description",
    GroupName = SystemTabNames.Content,
    Order = 1)]
    public virtual ContentArea MainContentArea { get; set; }

}
