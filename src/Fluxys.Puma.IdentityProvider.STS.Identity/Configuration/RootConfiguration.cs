﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Skoruba.Duende.IdentityServer.Shared.Configuration.Configuration.Identity;
using Fluxys.Puma.IdentityProvider.STS.Identity.Configuration.Interfaces;

namespace Fluxys.Puma.IdentityProvider.STS.Identity.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {      
        public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
    }
}







