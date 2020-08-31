 import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CreateInventoryComponent } from './create-inventory/create-inventory.component';
import { ListInventoryComponent } from './list-inventory/list-inventory.component';
import { ReadInventoryComponent } from './read-inventory/read-inventory.component';
import { InventoryService } from './service/inventory.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { Http, Response, Headers } from '@angular/http';
import { appSettings } from './appSettings';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from "@angular/forms";  

@NgModule({
  declarations: [
    AppComponent,
    CreateInventoryComponent,
    ListInventoryComponent,
    ReadInventoryComponent
  ],
  imports: [
    BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      HttpModule,
      ReactiveFormsModule
  ],
  providers: [InventoryService, appSettings],
  bootstrap: [AppComponent]
})
export class AppModule { }
