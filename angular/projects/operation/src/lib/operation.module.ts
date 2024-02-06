import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { OperationComponent } from './components/operation.component';
import { OperationRoutingModule } from './operation-routing.module';

@NgModule({
  declarations: [OperationComponent],
  imports: [CoreModule, ThemeSharedModule, OperationRoutingModule],
  exports: [OperationComponent],
})
export class OperationModule {
  static forChild(): ModuleWithProviders<OperationModule> {
    return {
      ngModule: OperationModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<OperationModule> {
    return new LazyModuleFactory(OperationModule.forChild());
  }
}
