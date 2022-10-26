﻿namespace Compass.IdentityService.Infrastructure;

public class SmtpOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}