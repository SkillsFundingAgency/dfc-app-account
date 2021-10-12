### Digital First Careers � Accounts app

## Introduction

This project provides a Account details app for use in the Composite UI (Shell application) to allow users to view their account details..

Details of the Composite UI application may be found here https://github.com/SkillsFundingAgency/dfc-composite-shell

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (web application, service and repository layers, with associated unit test and integration test projects).

### Installing

Clone the project and open the solution in Visual Studio 2019.

## List of dependencies

|Item	|Purpose|
|-------|-------|
|Azure Cosmos DB | Document storage |
|DFC.Compui.Telemetry|Telemetry|
|DFC.Compui.Cosmos| Repository|
|DFC.Content.Pkg.Netcore| UI elements|
|DFC.Compui.Subscriptions|Subscriptions API client|
|DFC.Personalisation.Common| Common Library|
|DFC.Personalisation.CommonUI| Shared UI elements|

## Local Config Files

Once you have cloned the public repo you need to remove the -template part from the configuration file names listed below.

| Location | Filename | Rename to |
|-------|-------|-------|
| DFC.App.Account | appsettings-template.json | appsettings.json |

## Configuring to run locally

The project contains an "appsettings-template.json" file which contains account appsettings for the web app. To use this file, copy it to "appsettings.json" and edit and replace the configuration item values with values suitable for your environment.

By default, the appsettings include local Azure Cosmos Emulator configurations using the well known configuration values for the account data and shred content storage (in separate collections). These may be changed to suit your environment if you are not using the Azure Cosmos Emulator.

## Running locally

To run this product locally, you will need to configure the list of dependencies, once configured and the configuration files updated, it should be F5 to run and debug locally. The application can be run using IIS Express or full IIS.

To run the project, start the web application. Once running, browse to the main entry point which is the "https://localhost::44355/health/endpoints". This will show a list of endpoints.

The Accounts app is designed to be run from within the Composite UI, therefore running the app outside of the Composite UI will only allow you to get to routes that are not authenticated.

The accounts app exspects a token to be present on each request to confirm you are othenticated. The values for the token are set in the app settings file listed above.

## Deployments

This accounts app will be deployed as an individual deployment for consumption by the Composite UI.

## Assets

CSS, JS, images and fonts used in this site can found in the following repository https://github.com/SkillsFundingAgency/dfc-digital-assets

## Built With

* Microsoft Visual Studio 2019
* .Net Core 3.1

## References

Please refer to https://github.com/SkillsFundingAgency/dfc-digital for additional instructions on configuring individual components like Cosmos.