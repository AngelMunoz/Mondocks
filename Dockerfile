ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.22-jammy-amd64 AS dotnet-cli

ENV \
    DOTNET_SDK_VERSION=6.0.414 \
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    NUGET_XMLDOC_MODE=skip \
    # Opt out of telemetry until after we install jupyter when building the image, this prevents caching of machine id
    DOTNET_INTERACTIVE_CLI_TELEMETRY_OPTOUT=true

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
	git \
	wget \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='79bb0576df990bb1bdb2008756587fbf6068562887b67787f639fa51cf1a73d06a7272a244ef34de627dee4bb82377f91f49de9994cbaeb849412df4e711db40' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./sdk-manifests ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help


FROM jupyter/base-notebook:ubuntu-22.04


ARG NB_USER=jovyan
ARG NB_UID=1000
ENV USER ${NB_USER}
ENV NV_UID ${NB_UID}
ENV HOME /home/${NB_USER}

# Copy Notebooks
COPY notebooks ${HOME}/notebooks

# Copy package sources
COPY nugetprops.config ${HOME}/nuget.config

USER root
COPY --from=dotnet-cli ["/usr/share/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
RUN chown -R ${NB_UID} ${HOME} 
USER ${NB_USER}

# Install nteract
RUN python3 -m pip install --no-cache-dir nteract-on-jupyter

# Install lastest build from master branch of Microsoft.DotNet.Interactive
# RUN dotnet tool install -g Microsoft.dotnet-interactive --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json"

#latest stable from nuget.org
RUN dotnet tool install -g Microsoft.dotnet-interactive --version 1.0.446104
#RUN dotnet tool install -g Microsoft.dotnet-interactive --add-source "https://api.nuget.org/v3/index.json"

ENV PATH="${PATH}:${HOME}/.dotnet/tools"
RUN echo "$PATH"

# Install kernel specs
RUN dotnet interactive jupyter install

# Enable telemetry once we install jupyter for the image
ENV DOTNET_INTERACTIVE_CLI_TELEMETRY_OPTOUT=false

# Set root to notebooks
WORKDIR ${HOME}/notebooks/
