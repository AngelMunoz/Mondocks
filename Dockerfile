ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:7.0.11-jammy-amd64 AS dotnet-cli

ENV \
    # Unset ASPNETCORE_URLS from base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # SDK version
    DOTNET_SDK_VERSION=7.0.401 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # Opt out of telemetry until after we install jupyter when building the image, this prevents caching of machine id
    DOTNET_CLI_TELEMETRY_OPTOUT=true


RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
	git \
	wget \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='2544f58c7409b1fd8fe2c7f600f6d2b6a1929318071f16789bd6abf6deea00bd496dd6ba7f2573bbf17c891c4f56a372a073e57712acfd3e80ea3eb1b3f9c3d0' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help


FROM jupyter/base-notebook:ubuntu-22.04

ENV \
    DOTNET_RUNNING_IN_DOCKER=true \
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USER_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # `dotnet-interactive` expects some cultural-specific data, which entails installing `libicu`
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true \
    # Opt out of telemetry until after we install jupyter when building the image, this prevents caching of machine id
    DOTNET_CLI_TELEMETRY_OPTOUT=true

# Copy Notebooks
COPY notebooks ${HOME}/notebooks

# Copy package sources
COPY nugetprops.config ${HOME}/nuget.config

USER root
COPY --from=dotnet-cli ["/usr/share/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
RUN chown -R ${NB_UID} ${HOME} 
USER ${NB_USER}

# Install lastest build from master branch of Microsoft.DotNet.Interactive
# RUN dotnet tool install -g Microsoft.dotnet-interactive --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json"

# Latest stable for net7.0 from nuget.org
RUN dotnet tool install -g Microsoft.dotnet-interactive --version 1.0.446104

ENV PATH="${PATH}:${HOME}/.dotnet/tools"

# Install kernel specs
RUN dotnet interactive jupyter install

# Enable telemetry once we install jupyter for the image
ENV DOTNET_CLI_TELEMETRY_OPTOUT=false

# Set root to notebooks
WORKDIR ${HOME}/notebooks/
