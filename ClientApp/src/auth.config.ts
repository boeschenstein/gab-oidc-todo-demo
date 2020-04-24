import { AuthConfig } from 'angular-oauth2-oidc';

export function createAuthConfig(callbackUrl: string): AuthConfig {
  return {
    // Id of the client, also called application id in Azure AD
    clientId: '3012a28e-e292-4eca-92e6-3b51e276de8e',
    // Url of the Identity Provider. This is login.microsoft.com followed by the tenant (directory) id
    issuer: 'https://login.microsoftonline.com/4925c6ef-421e-417d-a9ce-4b559466d6ff/v2.0',
    redirectUri: callbackUrl,
    postLogoutRedirectUri: callbackUrl,
    skipIssuerCheck: true,
    strictDiscoveryDocumentValidation: false,
    // set the scope for the permissions the client should request
    // The first three are defined by OIDC.
    scope: 'openid profile api://todo/ToDo.ReadWrite'
  };
}
