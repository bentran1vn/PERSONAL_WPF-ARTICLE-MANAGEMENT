using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class NewsTag
{
    public string NewsTagId { get; set; } = null!;

    public string? NewsArticleId { get; set; }

    public int? TagId { get; set; }

    public virtual NewsArticle? NewsArticle { get; set; }

    public virtual Tag? Tag { get; set; }
}
