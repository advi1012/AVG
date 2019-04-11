﻿namespace AvG_Abgabe_1___Webapp.Controllers
{
    /// <summary>
    /// Klasse zur Definition von HATEAOS
    /// </summary>
  public class LinkDto
  {
    public string Href { get; private set; }
    public string Rel { get; private set; }
    public string Method { get; private set; }
    public LinkDto(string href, string rel, string method)
    {
        this.Href = href;
        this.Rel = rel;
        this.Method = method;
    }
  }
}