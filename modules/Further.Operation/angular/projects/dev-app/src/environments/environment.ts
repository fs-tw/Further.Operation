import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44344/',
  redirectUri: baseUrl,
  clientId: 'Operation_App',
  responseType: 'code',
  scope: 'offline_access Operation',
  requireHttps: true
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Operation',
    logoUrl: '',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44344',
      rootNamespace: 'Further.Operation',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
    Operation: {
      url: 'https://localhost:44360',
      rootNamespace: 'Further.Operation',
    },
  },
} as Environment;
