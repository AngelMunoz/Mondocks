{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixpkgs-unstable";
    flake-parts.url = "github:hercules-ci/flake-parts";
  };
  outputs = { self, nixpkgs, flake-parts }@inputs:
    flake-parts.lib.mkFlake { inherit inputs; } {
      perSystem = { system, pkgs, ... }: {
        # Basically "patches" the `pkgs` input to `perSystem`
        _module.args.pkgs = import inputs.nixpkgs {
          inherit system;
          config.allowUnfreePredicate = pkg: builtins.elem (nixpkgs.lib.getName pkg) [ "mongodb" "mongodb-compass" ];
        };
        formatter = pkgs.nixfmt;
        devShells.default = pkgs.mkShell {
          buildInputs = with pkgs;
            [ dotnet-sdk ];
          shellHook = ''
            dotnet tool restore
          '';
        };
      };
      systems = [ "x86_64-linux" "x86_64-darwin" "aarch64-darwin" ];
    };
}
