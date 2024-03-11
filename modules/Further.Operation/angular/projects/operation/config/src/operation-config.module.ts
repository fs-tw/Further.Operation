import { ModuleWithProviders, NgModule } from '@angular/core';
import { OPERATION_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class OperationConfigModule {
  static forRoot(): ModuleWithProviders<OperationConfigModule> {
    return {
      ngModule: OperationConfigModule,
      providers: [OPERATION_ROUTE_PROVIDERS],
    };
  }
}
